#pragma warning disable S125,S1854,S1481,S1135
namespace MJsNetExtensions.Xml.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml;

    /// <summary>
    /// XSD Or DTD Found Callback type
    /// </summary>
    /// <param name="foundDefinitionUri"></param>
    internal delegate void XSDOrDTDFoundCallback(Uri foundDefinitionUri);

    /// <summary>
    /// Uri address translation resolver to a list of known (provided) files.
    /// If translation is active and no translation can be found for the given address to resolve, then an UriFormatException is thrown.
    /// </summary>
    internal sealed class DoNotGoToWebXmlResolver : System.Xml.XmlUrlResolver
    {
        #region Statics and Constants
        public const string CannotResolveUri = "As requested, web addresses are NOT being resolved! "; //"Webadressen werden wie gew�nscht NICHT aufgel�st! ";
        public const string ResolvedUriAlreadyLoaded = "Resolved URI was already loaded! "; //a helper to signal resolving this URI was already done, so ignore it as a duplicate.

        /// <summary>
        /// Program full Directory path, e.g.: "C:\...bin\Debug\" (WITH the trailing slash!)
        /// </summary>
        public static readonly string ProgramDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly Uri ProgramDirectoryUri = new(ProgramDirectory);
        #endregion Statics and Constants

        #region Construction / Destruction

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public DoNotGoToWebXmlResolver(XmlValidatorSettings settings, XSDOrDTDFoundCallback xsdOrDTDFoundCallback)
        {
            Throw.IfNullOrInvalid(settings, nameof(settings));

            List<string> knownDefinitionFiles = new List<string>();
            knownDefinitionFiles.Add(settings.XmlDefinitionFilePath);
            if (settings.AdditionalXmlDefinitionFilePaths != null)
            {
                knownDefinitionFiles.AddRange(settings.AdditionalXmlDefinitionFilePaths);
            }

            this.Mode = settings.UriTranslationMode;
            this.specificResolvingDisabledAndRelayingJustOnStandardXmlUrlResolverResolving = false;
            this.CurrentBaseUri = null;

            this.XsdOrDTDFoundCallback = xsdOrDTDFoundCallback;

            foreach (string knownFilePath in knownDefinitionFiles)
            {
                if (string.IsNullOrWhiteSpace(knownFilePath))
                {
                    continue; // --> ignore
                }

                string knownFileFullPath = knownFilePath;
                //TODO: partial FIX: Uri is bad approach for program palth operations. Use Assembly.Location instead of Assembly.CodeBase. Consider paths like: D:\BatchProcessing\#temporary\MyApp -- TODO: check all URIs in Resolver etc for this problem!
                Uri resolvedUri = new Uri(ProgramDirectoryUri, knownFileFullPath);

                if (resolvedUri.IsUnc || resolvedUri.IsFile)
                {
                    knownFileFullPath = Path.GetFullPath(knownFilePath);  // NOTE: that unlike most members of the Path class, this method accesses the file system.

                    if (!DoesTheFileExistAndCacheItIfYes(knownFileFullPath))
                    {
                        string msg = string.Format(CultureInfo.InvariantCulture, "The provided (XSD) file \"{0}\" to resolve to does not exist! Full path: \"{1}\"", knownFilePath, knownFileFullPath);
                        throw new FileNotFoundException(msg, knownFilePath);
                    }

                    //NOTE: knownFileFullPath can be updated:
                    resolvedUri = new Uri(ProgramDirectoryUri, knownFileFullPath);
                }
                else // --> check it as a Web HTTP(S) Uri:
                {
                    CheckIfUriIsValidAndAccessibleAndCacheItIfYesOrThrow(knownFilePath, resolvedUri);
                }

                string fileNameKey = Path.GetFileName(resolvedUri.LocalPath).ToUpperInvariant();
                this.KnownDefinitionFiles[fileNameKey] = resolvedUri;
            }
        }
        #endregion Construction / Destruction

        #region Properties and Fields
        public UriTranslationMode Mode { get; private set; } // = UriTranslationMode.ForcedTranslateAllAddresses; <==> this is set in constructor

        public XSDOrDTDFoundCallback XsdOrDTDFoundCallback { get; private set; }

        public Uri CurrentBaseUri { get; private set; }

        public Dictionary<string, Uri> KnownDefinitionFiles { get; } = new();

        private HashSet<string> AlreadyLoadedKnownDefinitionFileNames { get; } = new();


        private readonly HashSet<string> checkedPathsCache = new();

        /// <summary>
        /// Helper to switch off the custom resolving and relying just on the bare default <see cref="XmlUrlResolver"/> functionality.
        /// </summary>
        private bool specificResolvingDisabledAndRelayingJustOnStandardXmlUrlResolverResolving;
        #endregion Properties and Fields

        #region API

        #region Specific API Methods
        public void DisableSpecificResolvingAndRelayJustOnStandardXmlUrlResolverResolving()
        {
            this.specificResolvingDisabledAndRelayingJustOnStandardXmlUrlResolverResolving = true;
        }

        public void EnableSpecificResolving()
        {
            this.specificResolvingDisabledAndRelayingJustOnStandardXmlUrlResolverResolving = false;
        }

        /// <summary>
        /// Method for signalling, that all the XSDs in KnownDefinitionFiles were loaded and compiled correctly without using this resolver. Thus this resolver does not have to learn them again.
        /// </summary>
        public void KnownDefinitionFilesAreLoaded()
        {
            foreach (string fileNameKey in this.KnownDefinitionFiles.Keys)
            {
                this.AlreadyLoadedKnownDefinitionFileNames.Add(fileNameKey);
            }
        }

        /// <summary>
        /// This is useful if the cached checked files should or must be rechecked upon next hit.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void ResetFileCheckingCache()
        {
            this.checkedPathsCache.Clear();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public void SetCurrentBaseUri(string baseFilePath)
        {
            if (string.IsNullOrWhiteSpace(baseFilePath))
            {
                this.CurrentBaseUri = null;
                return;
            }

            string knownFileFullPath = baseFilePath;
            Uri resolvedUri = new Uri(ProgramDirectoryUri, knownFileFullPath);

            if (resolvedUri.IsUnc || resolvedUri.IsFile)
            {
                knownFileFullPath = Path.GetFullPath(baseFilePath);  // NOTE: that unlike most members of the Path class, this method accesses the file system.

                if (!DoesTheFileExistAndCacheItIfYes(knownFileFullPath) && !Directory.Exists(knownFileFullPath))
                {
                    string msg = string.Format(CultureInfo.InvariantCulture, "The base path \"{0}\" to resolve to does not exist! Full path : \"{1}\"", baseFilePath, knownFileFullPath);
                    throw new FileNotFoundException(msg, baseFilePath);
                }

                //NOTE: knownFileFullPath can be updated:
                resolvedUri = new Uri(ProgramDirectoryUri, knownFileFullPath);
            }

            this.CurrentBaseUri = resolvedUri;
        }
        #endregion

        #region Overriden Stuff
        /// <summary>
        /// Resolves the absolute URI from the base and relative URIs. (This comments are copied from MSDN!)
        /// </summary>
        /// <param name="baseUri">The base URI used to resolve the relative URI.</param>
        /// <param name="relativeUri">The URI to resolve. The URI can be absolute or relative. If absolute, this value effectively replaces the baseUri value. If relative, it combines with the baseUri to make an absolute URI.</param>
        /// <returns>A Uri representing the absolute URI, or null if the relative URI cannot be resolved.</returns>
        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            //NOTE: 
            // 1) in a case of loading an XSD - one of the "known definition files" provided by the user, or in the XML file being validated in the xsi:schemaLocation or xsi:noNamespaceSchemaLocation attributes, 
            //    the baseUri is null or empty (and we usually do not have CurrentBaseUri in this case, because it is not needed),
            //    but for all included and imported XSDs in the "known file", the baseUri is automatically (by .Net) set to the baseUri == the "known file" importing or including the further XSD. 
            //    This approach is recursive for any XSD importing or including another XSD(s)...
            // 2) if loading the XML file to validate, then the baseUri is null or empty. XmlValidatorBase tries to automatically provide the CurrentBaseUri, so the resolving results in a valid URI or local path. 
            //    NOTE, that this could be further refined (for XML file relative paths), by trying following sequence, if the relativeUri does not resolve to an existing path using CurrentBaseUri:
            //      2.1 try to resolve using current directory
            //      2.2 try to resolve using ProgramDirectory
            //      2.3 try to resolve using AppDomain.CurrentDomain.BaseDirectory 
            if (string.IsNullOrWhiteSpace(baseUri?.OriginalString) && this.CurrentBaseUri != null)
            {
                baseUri = this.CurrentBaseUri;
            }

            Uri defaultResolvedUri = base.ResolveUri(baseUri, relativeUri);
            if (specificResolvingDisabledAndRelayingJustOnStandardXmlUrlResolverResolving)
            {
                return defaultResolvedUri;
            }

            Uri resolvedUri = null;
            try
            {
                // the logic
                switch (this.Mode)
                {
                    case UriTranslationMode.TranslateNoneButRelayOnXmlUrlResolver:
                        {
                            return defaultResolvedUri;
                        }

                    case UriTranslationMode.TranslateWebAddressesOnly:
                        {
                            if (defaultResolvedUri.IsUnc || defaultResolvedUri.IsFile)
                            {
                                // TranslateWebAddressesOnly means: take all Files and UNCs!
                                if (DoesTheFileExistAndCacheItIfYes(defaultResolvedUri.LocalPath))
                                {
                                    // remember new accepted known definition file:
                                    string fileNameKey = Path.GetFileName(defaultResolvedUri.LocalPath).ToUpperInvariant();
                                    this.KnownDefinitionFiles[fileNameKey] = defaultResolvedUri;

                                    return defaultResolvedUri;
                                }
                                else
                                {
                                    // or try to resolve the NOT Existing File path (defaultResolvedUri.LocalPath) using the list of known definition files:
                                    resolvedUri = LearningForcedTranslateUri(baseUri, defaultResolvedUri);
                                    return resolvedUri;
                                }
                            }
                            else
                            {
                                // try to resolve the Web Address to a file path using the list of known definition files
                                resolvedUri = LearningForcedTranslateUri(baseUri, defaultResolvedUri);
                                return resolvedUri;
                            }
                        }

                    case UriTranslationMode.ForcedTranslateAllAddresses:
                        {
                            // just resolve the URI with the list of known definition files, no matter what kind of address it is
                            // AND ignore everythig else not specified in the known definition files!
                            resolvedUri = LearningForcedTranslateUri(baseUri, defaultResolvedUri);
                            return resolvedUri;
                        }

                    default:
                        throw new NotSupportedException("Mode is not supported yet: " + this.Mode.ToString());
                }
            }
            catch (UriFormatException ex)
            {
                if (ex.Message.Contains(CannotResolveUri, StringComparison.InvariantCultureIgnoreCase) ||
                    ex.Message.Contains(ResolvedUriAlreadyLoaded, StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    throw;
                }

                string msg = string.Format(CultureInfo.InvariantCulture,
                  "Context: baseUri: '{0}', relativeUri: '{1}', defaultResolvedUri: '{2}', known resolved uri: '{3}'",
                  baseUri,
                  relativeUri,
                  defaultResolvedUri,
                  resolvedUri
                  );
                throw new UriFormatException(msg, ex);
            }

            // this line should never be reached
            throw new UriFormatException(CannotResolveUri + resolvedUri?.AbsoluteUri);
        }

        /// <summary>
        /// Maps a URI resolved in ResolveUri() above to an object containing the actual resource.
        /// </summary>
        /// <param name="absoluteUri">The URI returned from ResolveUri(System.Uri,System.String)</param>
        /// <param name="role">See the documentation of base <see cref="XmlUrlResolver"/>.</param>
        /// <param name="ofObjectToReturn">See the documentation of base <see cref="XmlUrlResolver"/>.</param>
        /// <returns>See the documentation of base <see cref="XmlUrlResolver"/>.</returns>
        public override Object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            Object resolvedEntity = base.GetEntity(absoluteUri, role, ofObjectToReturn);

            return GetEntityInner(absoluteUri, resolvedEntity);
        }

        public override async Task<object> GetEntityAsync(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            Object resolvedEntity = await base.GetEntityAsync(absoluteUri, role, ofObjectToReturn).ConfigureAwait(false);

            return GetEntityInner(absoluteUri, resolvedEntity);
        }
        #endregion Overriden Stuff

        #endregion API

        #region Inner Implementation Detail
        private object GetEntityInner(Uri absoluteUri, object resolvedEntity)
        {
            if (absoluteUri != null)
            {
                // repeatable setting of the same value does not matter (will be ignored silently bu the HashSet...
                string fileNameKey = Path.GetFileName(absoluteUri.LocalPath).ToUpperInvariant();
                this.AlreadyLoadedKnownDefinitionFileNames.Add(fileNameKey);
            }

            if (specificResolvingDisabledAndRelayingJustOnStandardXmlUrlResolverResolving)
            {
                return resolvedEntity;
            }

            if (resolvedEntity != null
                && this.XsdOrDTDFoundCallback != null
                && absoluteUri != null
                && (absoluteUri.LocalPath.EndsWith(".xsd", StringComparison.OrdinalIgnoreCase)
                    || absoluteUri.LocalPath.EndsWith(".dtd", StringComparison.OrdinalIgnoreCase))
               )
            {
                this.XsdOrDTDFoundCallback(absoluteUri);
            }

            return resolvedEntity;
        }

        /// <summary>
        /// Helper for forced resolving
        /// </summary>
        /// <param name="baseUri">An optional base <see cref="Uri"/></param>
        /// <param name="defaultResolvedUriToForceTranslate">The file name of the uriToResolve.LocalPath is taken to resolve the result file path</param>
        /// <returns>The resolved uri based on the known definition files list</returns>
        /// <exception cref="UriFormatException">If the path can not be resolved, or the resolved path can not be found with File.Exists()</exception>
        private Uri LearningForcedTranslateUri(Uri baseUri, Uri defaultResolvedUriToForceTranslate)
        {
            Uri resolvedUri = null;

            string fileNameKey = Path.GetFileName(defaultResolvedUriToForceTranslate.LocalPath).ToUpperInvariant();
            if (this.KnownDefinitionFiles.TryGetValue(fileNameKey, out resolvedUri))
            {
                if (this.AlreadyLoadedKnownDefinitionFileNames.Contains(fileNameKey))
                {
                    //NOTE: this exception is a safe manner to ignore the 2nd loading of the same XSD!
                    //      Learning is used only during validating a concrete XML in XmlValidatorBase only for resolving the XML attributes: xsi:schemaLocation or xsi:noNamespaceSchemaLocation =>
                    //      => the already resolved and loaded URIs does NOT need to be read and parsed again! So throw instead of returning cached resolvedUri. This results in .Net not calling the subsequent GetEntity()
                    //         and results in a XmlValidationIssue, with Severity set to ValidationSeverity.Ignore, which is safely ignored, due to the "ResolvedUriAlreadyLoaded" content!
                    throw new UriFormatException($"{ResolvedUriAlreadyLoaded} Ori resolved Uri: {defaultResolvedUriToForceTranslate} -> translated to known loaded Uri: {resolvedUri}");
                }
                else
                {
                    // allow to read it - until it is marked as loaded in GetEntity():
                    return resolvedUri;
                }
            }

            // Learning to include the subsidiary XSD or DTD files, if in the known definition files was originally included only the root XSD or DTD file(s),
            // but the definition consists of (includes or imports) several separate definition files positioned relatively to each other (and the subsidiary files
            // were not provided in the known definition files in constructor).
            bool doTakeAndLearnIt = false;

            // NOTE: this first "if" is necessary for the this.Mode == UriTranslationMode.ForcedTranslateAllAddresses
            if (!string.IsNullOrWhiteSpace(baseUri?.OriginalString) &&
                (baseUri == this.CurrentBaseUri || this.KnownDefinitionFiles.ContainsValue(baseUri))
                )
            {
                if (defaultResolvedUriToForceTranslate.IsUnc || defaultResolvedUriToForceTranslate.IsFile)
                {
                    if (DoesTheFileExistAndCacheItIfYes(defaultResolvedUriToForceTranslate.LocalPath))
                    {
                        // if it is a resolved existing file, then take it
                        doTakeAndLearnIt = true;
                    }
                    //else -> file does not exist -> throw... which results in ValidationSeverity.Warning
                }
                else if (baseUri.IsAbsoluteUri &&  //NOTE: supporting only XSDs in the same domain as the base!
                        string.Equals(defaultResolvedUriToForceTranslate.Scheme, baseUri.Scheme, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(defaultResolvedUriToForceTranslate.Authority, baseUri.Authority, StringComparison.OrdinalIgnoreCase)
                        )
                {
                    //NOTE: this "else if" branch could theoretically only be executed for a brand new XSD provided in xsi:schemaLocation or xsi:noNamespaceSchemaLocation
                    //      if and only if the caller (the XmlValidatorbase) would provide an appropriate "CurrentBaseUri" which is NEITHER a file NOR an UNC. This is in moment not foreseen!

                    // The defaultResolvedUriToForceTranslate is an absolute web address
                    // AND the defaultResolvedUriToForceTranslate starts with common part of baseUri
                    // and differs only in relative path, then learn it:
                    doTakeAndLearnIt = true;
                }
            }
            //else -> throw exception... which results in ValidationSeverity.Warning

            if (doTakeAndLearnIt)
            {
                this.KnownDefinitionFiles.Add(fileNameKey, defaultResolvedUriToForceTranslate);

                // allow to read it - until it is marked as loaded in GetEntity():
                return defaultResolvedUriToForceTranslate;
            }

            //NOTE: switched off sending the message "CannotResolveUri"
            //throw new UriFormatException(CannotResolveUri + uriToResolve.ToString());
            throw new UriFormatException($"Following Uri could not be translated using the known definition files list: '{defaultResolvedUriToForceTranslate}'");
        }

        private bool DoesTheFileExistAndCacheItIfYes(string filePath)
        {
            string filePathUpper = filePath.ToUpperInvariant();

            if (this.checkedPathsCache.Contains(filePathUpper))
            {
                return true;
            }
            else if (File.Exists(filePath))
            {
                this.checkedPathsCache.Add(filePathUpper);
                return true;
            }

            return false;
        }

        private void CheckIfUriIsValidAndAccessibleAndCacheItIfYesOrThrow(string knownFilePath, Uri resolvedUri)
        {
            string filePathUpper = knownFilePath.ToUpperInvariant();
            string resolvedUriString = resolvedUri.ToString().ToUpperInvariant();

            if (this.checkedPathsCache.Contains(filePathUpper) ||
                this.checkedPathsCache.Contains(resolvedUriString)
                )
            {
                return;
            }
            // else -->

            // Inspired by: C# How can I check if a URL exists/is valid?
            // https://stackoverflow.com/questions/924679/c-sharp-how-can-i-check-if-a-url-exists-is-valid
            try
            {
                using (var client = new WebUriCheckClient())
                {
                    // if URL is fine => no content is downloaded, 
                    // else if non Existing => throws 404 WebException
                    string s2 = client.DownloadStringAsync(resolvedUri).Result;
                }
                    
                this.checkedPathsCache.Add(filePathUpper);
                this.checkedPathsCache.Add(resolvedUriString);
            }
            catch (IOException ex)
            {
                string msgPart = string.Equals(filePathUpper, resolvedUriString, StringComparison.OrdinalIgnoreCase) ? "" : $"--> resolved Uri: {resolvedUri}";
                throw new IOException($"Can not access provided (XSD) file: {knownFilePath} {msgPart}", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                string msgPart = string.Equals(filePathUpper, resolvedUriString, StringComparison.OrdinalIgnoreCase) ? "" : $"--> resolved Uri: {resolvedUri}";
                throw new UnauthorizedAccessException($"Can not access provided (XSD) file: {knownFilePath} {msgPart}", ex);
            }
            catch (WebException ex)
            {
                string msgPart = string.Equals(filePathUpper, resolvedUriString, StringComparison.OrdinalIgnoreCase) ? "" : $"--> resolved Uri: {resolvedUri}";
                throw new WebException($"Can not access provided (XSD) file: {knownFilePath} {msgPart}", ex);
            }
            catch (XmlException ex)
            {
                string msgPart = string.Equals(filePathUpper, resolvedUriString, StringComparison.OrdinalIgnoreCase) ? "" : $"--> resolved Uri: {resolvedUri}";
                throw new WebException($"Can not parse provided (XSD) file: {knownFilePath} {msgPart}", ex);
            }
            catch (Exception ex)
            {
                string msgPart = string.Equals(filePathUpper, resolvedUriString, StringComparison.OrdinalIgnoreCase) ? "" : $"--> resolved Uri: {resolvedUri}";
#pragma warning disable CA2201,S112
                throw new Exception($"General error while accessing provided (XSD) file: {knownFilePath} {msgPart}", ex);
#pragma warning restore CA2201,S112
            }
        }

        /// <summary>
        /// Helper Uri checker class -> not really downloading the content, just trying to access it.
        /// Inspired by: C# How can I check if a URL exists/is valid?
        /// https://stackoverflow.com/questions/924679/c-sharp-how-can-i-check-if-a-url-exists-is-valid
        /// </summary>
        private sealed class WebUriCheckClient : IDisposable
        {
            public bool HeadOnly { get; set; } = true;
            private static readonly HttpClient httpClient = new();

            public void Dispose()
            {
                // No resources to dispose for static HttpClient
            }

            public async Task<string> DownloadStringAsync(Uri address)
            {
                using var request = new HttpRequestMessage(HeadOnly ? HttpMethod.Head : HttpMethod.Get, address);
                using var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return HeadOnly ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
        #endregion Inner Implementation Detail

    }
}