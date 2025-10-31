#pragma warning disable S125
namespace MJsNetExtensions.Xml.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    /// <summary>
    /// The common base for XML Validation functionality with a given Xml Schema set or DTDs, etc.
    /// </summary>
    public abstract class XmlValidator
    {
        #region Const and Strings

        /// <summary>
        /// A fallback file name used as a default for xmlFilePath parameter in <see cref="CreateValidatingXmlReader(XmlReader, string)"/>, 
        /// or <see cref="CreateValidatingXmlReader(Stream, string)"/>, or <see cref="CreateValidatingXmlReader(string, string)"/> methods.
        /// It represents the <see cref="XmlReader"/>, or <see cref="TextReader"/> or <see cref="Stream"/> used to read the input XML. 
        /// THE FILE WILL NOT BE ACCESSED! It servees merely for validation messages.
        /// If null or empty, it will be automatically internally replaced by file name: "foo.xml"
        /// </summary>
        public const string DefaultVirtualFileName = "foo.xml";

        #endregion Const and Strings


        #region Fields

        private int reentrancyProtectionCount;

        private string currentValidatedXmlFile = DefaultVirtualFileName;

        private ValidatingXmlReader currentValidatingXmlReader;

        #endregion Fields

        #region Construction / Destruction
        // prohibit user default construction
        private XmlValidator() { }

        /// <summary>
        /// Constructs an Xml Validator Base
        /// </summary>
        /// <param name="settings">The <see cref="XmlValidatorSettings"/> object used to configure the new <see cref="XmlValidator"/>.</param>
        protected XmlValidator(XmlValidatorSettings settings)
        {
            this.Settings = settings;

            this.DoNotGoToWebXmlResolver = new DoNotGoToWebXmlResolver(settings, this.ResolverFoundAdditionalDefinitionFileCallback);
            this.DoNotGoToWebXmlResolver.Credentials = CredentialCache.DefaultCredentials;
            this.DoNotGoToWebXmlResolver.DisableSpecificResolvingAndRelayJustOnStandardXmlUrlResolverResolving(); // to prohibit resolving of the XML file path beeing validated
        }

        /// <summary>
        /// Generic Factory method for creating and initializing of an XML Validator.
        /// NOTE: creating without settings. In this case no XSD or DTD validation will be done. Just XML well-formedness will be checked.
        /// </summary>
        /// <returns>An initialized <see cref="XmlValidator"/> instance according to <see cref="XmlValidatorSettings.XmlValidationType"/>.</returns>
        public static XmlValidator Create()
        {
            return Create(null);
        }

        /// <summary>
        /// Generic Factory method for creating and initializing of an XSD or DTD XML Validator.
        /// </summary>
        /// <param name="settings">Optional. Can be null. The <see cref="XmlValidatorSettings"/> object used to configure the new <see cref="XmlValidator"/>.
        /// NOTE: in a case of creating without settings or with empty settings, no XSD or DTD validation will be done. Just XML well-formedness will be checked.</param>
        /// <returns>An initialized <see cref="XmlValidator"/> instance according to <see cref="XmlValidatorSettings.XmlValidationType"/>.</returns>
        public static XmlValidator Create(XmlValidatorSettings settings)
        {
            if (settings == null)
            {
                // create default settings -> in this case no XSD or DTD validation will be done. Just XML well-formedness will be checked.
                settings = new XmlValidatorSettings();
            }

            // Create and Initialize XML Validator:
            XmlValidator xmlValidator = null;

            switch (settings.XmlValidationType)
            {
                case XmlValidationType.XSD: xmlValidator = new XmlXsdValidator(settings); break;
                case XmlValidationType.DTD: xmlValidator = new XmlDtdValidator(settings); break;

                default:
                    throw new NotSupportedException($"XmlValidationType is not supported yet: {settings.XmlValidationType}");
            }

            xmlValidator.Initialize();

            return xmlValidator;
        }
        #endregion Construction / Destruction

        #region Properties and Fields

        /// <summary>
        /// The type of validation, e.g. XSD, DTD, etc. <see cref="XmlValidationType"/>.
        /// </summary>
        public abstract XmlValidationType XmlValidationType { get; }

        internal XmlValidatorSettings Settings { get; private set; }

        /// <summary>
        /// Mode of URI location translation for the <see cref="DoNotGoToWebXmlResolver"/>
        /// </summary>
        public UriTranslationMode UriTranslationMode => this.DoNotGoToWebXmlResolver?.Mode ?? UriTranslationMode.TranslateNoneButRelayOnXmlUrlResolver;

        internal DoNotGoToWebXmlResolver DoNotGoToWebXmlResolver { get; private set; }

        /// <summary>
        /// This is hidden on purpose. The settings must not be changed by any user.
        /// </summary>
        protected XmlReaderSettings OwnValidatingReaderSettings { get; } = new();

        /// <summary>
        /// This is just a copy of the protected "own" XML reader settings for user purposes. The copy is created using XmlReaderSettings.Clone() Method.
        /// </summary>
        public XmlReaderSettings ValidatingReaderSettings => this.OwnValidatingReaderSettings.Clone();

        /// <summary>
        /// Gets the stored Known Definition file paths. E.g. XSDs or DTDs.
        /// </summary>
        public IEnumerable<string> KnownDefinitionFiles => this.DoNotGoToWebXmlResolver.KnownDefinitionFiles.Values.Select(it => it.IsUnc || it.IsFile ? it.LocalPath : it.OriginalString) ?? [];

        /// <summary>
        /// Indicator if there was found an additional XSD or DTD definition file in the XML beeing validated
        /// </summary>
        protected internal bool HasAdditionalDefinitionFile { get; set; }

        /// <summary>
        /// Root Element Event Handler
        /// </summary>
        public event EventHandler<RootElementEventArgs> RootElementHandler;

        /// <summary>
        /// Indicator if there is any handler registerend on the <see cref="RootElementHandler"/>. 
        /// This has to be implemented like this because of comile error: 
        /// CS0070: The event 'XmlValidator.RootElementHandler' can only appear on the left hand side of += or -= (except when used from within the type 'XmlValidator')
        /// </summary>
        protected internal bool HasRootElementHandlerRegistered => this.RootElementHandler != null;

        /// <summary>
        /// <see cref="XmlValidationIssue"/> Event Handler.
        /// </summary>
        public event EventHandler<XmlValidationIssueEventArgs> XmlValidationIssueHandler;


        /// <summary>
        /// The result of the last XML validation. It is null, if the validation has not been executen until now.
        /// </summary>
        public XmlValidationResult LastValidationResult { get; private set; }

        /// <summary>
        /// Flag indicating, if Definition Files were provided during construction.
        /// </summary>
        protected internal bool HasDefinitionFiles => this.DoNotGoToWebXmlResolver.KnownDefinitionFiles.Count != 0;

        #endregion Properties

        #region API - Public Methods
        /// <summary>
        /// Resolver Found Additional Definition File Callback
        /// </summary>
        /// <param name="foundDefinitionUri">found definition Uri</param>
        protected void ResolverFoundAdditionalDefinitionFileCallback(Uri foundDefinitionUri)
        {
            this.HasAdditionalDefinitionFile = true;
        }

        #region Create Validating XmlReader

        /// <summary>
        /// Creates the validating XmlReader for a file.
        /// Caller is responsible for disposing the XmlReader returned!
        /// </summary>
        /// <param name="xmlFilePath">Full path to the XML file for which a validating XmlReader will be returned</param>
        /// <returns>Caller is responsible for disposing the XmlReader returned!</returns>
        /// <exception cref="ArgumentNullException">if the <paramref name="xmlFilePath"/> is null, epty or just white space.</exception>
        public XmlReader CreateValidatingXmlReader(string xmlFilePath)
        {
            Throw.IfNullOrWhiteSpace(xmlFilePath, nameof(xmlFilePath));

            return ProtectedCreateValidatingXmlReaderInner(
                () => XmlReader.Create(xmlFilePath, this.OwnValidatingReaderSettings),
                xmlFilePath
                );
        }

        /// <summary>
        /// Creates the validating XmlReader for a Stream provided.
        /// Caller is responsible for disposing the XmlReader returned!
        /// </summary>
        /// <param name="xml">A string containing the XML content which will be validated</param>
        /// <param name="xmlFilePath">Optional file name or a full path to the XML file which represents the <paramref name="xml"/>. 
        /// THE FILE WILL NOT BE ACCESSED! It servees merely for validation messages.
        /// If null or empty, it will be automatically internally replaced by file name: "foo.xml" <see cref="DefaultVirtualFileName"/>.</param>
        /// <returns>Caller is responsible for disposing the XmlReader returned!</returns>
        /// <exception cref="ArgumentNullException">if the <paramref name="xml"/> is null, epty or just white space.</exception>
        public XmlReader CreateValidatingXmlReader(string xml, string xmlFilePath)
        {
            Throw.IfNullOrWhiteSpace(xml, nameof(xml));

            //NOTE: after doing iterative test with an XML for 10,000,000 times with a construct like this and a XML string having a bit above 2,000 chars
            //      there was no observable memory leak in a call like:
            //      return XmlReader.Create(new StringReader(xml), this.OwnValidatingReaderSettings);

            return ProtectedCreateValidatingXmlReaderInner(
                () => new StringReader(xml),
                stringReader => XmlReader.Create(stringReader, this.OwnValidatingReaderSettings),
                xmlFilePath
                );
        }

        /// <summary>
        /// Creates the validating XmlReader for a Stream provided.
        /// Caller is responsible for disposing the XmlReader returned!
        /// </summary>
        /// <param name="xmlReader">An <see cref="XmlReader"/> containing XML content for which a validating XmlReader will be returned</param>
        /// <param name="xmlFilePath">Optional file name or a full path to the XML file which represents the <paramref name="xmlReader"/>. 
        /// THE FILE WILL NOT BE ACCESSED! It servees merely for validation messages.
        /// If null or empty, it will be automatically internally replaced by file name: "foo.xml" <see cref="DefaultVirtualFileName"/>.</param>
        /// <returns>Caller is responsible for disposing the XmlReader returned!</returns>
        public XmlReader CreateValidatingXmlReader(XmlReader xmlReader, string xmlFilePath)
        {
            return ProtectedCreateValidatingXmlReaderInner(
                () => XmlReader.Create(xmlReader, this.OwnValidatingReaderSettings),
                xmlFilePath
                );
        }

        /// <summary>
        /// Creates the validating XmlReader for a Stream provided.
        /// Caller is responsible for disposing the XmlReader returned!
        /// </summary>
        /// <param name="textReader">An <see cref="TextReader"/> containing XML content for which a validating XmlReader will be returned</param>
        /// <param name="xmlFilePath">Optional file name or a full path to the XML file which represents the <paramref name="textReader"/>. 
        /// THE FILE WILL NOT BE ACCESSED! It servees merely for validation messages.
        /// If null or empty, it will be automatically internally replaced by file name: "foo.xml" <see cref="DefaultVirtualFileName"/>.</param>
        /// <returns>Caller is responsible for disposing the XmlReader returned!</returns>
        public XmlReader CreateValidatingXmlReader(TextReader textReader, string xmlFilePath)
        {
            return ProtectedCreateValidatingXmlReaderInner(
                () => XmlReader.Create(textReader, this.OwnValidatingReaderSettings),
                xmlFilePath
                );
        }

        /// <summary>
        /// Creates the validating XmlReader for a Stream provided.
        /// Caller is responsible for disposing the XmlReader returned!
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> containing XML content for which a validating XmlReader will be returned</param>
        /// <param name="xmlFilePath">Optional file name or a full path to the XML file which represents the <paramref name="stream"/>. 
        /// THE FILE WILL NOT BE ACCESSED! It servees merely for validation messages.
        /// If null or empty, it will be automatically internally replaced by file name: "foo.xml" <see cref="DefaultVirtualFileName"/>.</param>
        /// <returns>Caller is responsible for disposing the XmlReader returned!</returns>
        public XmlReader CreateValidatingXmlReader(Stream stream, string xmlFilePath)
        {
            return ProtectedCreateValidatingXmlReaderInner(
                () => XmlReader.Create(stream, this.OwnValidatingReaderSettings),
                xmlFilePath
                );
        }
        #endregion Create Validating XmlReader

        #region Validate One Xml

        /// <summary>
        /// Do a complete validation of a file.
        /// </summary>
        /// <param name="xmlFilePath">Full path to the XML file which will be validated.</param>
        /// <returns>After a completed validation returns <see cref="XmlValidationResult"/> with the details of the validation.</returns>
        /// <exception cref="ArgumentNullException">if the <paramref name="xmlFilePath"/> is null, epty or just white space.</exception>
        public XmlValidationResult ValidateOneXml(string xmlFilePath)
        {
            return ValidateOneXmlWrapper(() => CreateValidatingXmlReader(xmlFilePath));
        }

        /// <summary>
        /// Do a complete validation of a file.
        /// </summary>
        /// <param name="xml">A string containing the XML content which will be validated</param>
        /// <param name="xmlFilePath">Optional file name or a full path to the XML file which represents the <paramref name="xml"/>. 
        /// THE FILE WILL NOT BE ACCESSED! It servees merely for validation messages.
        /// If null or empty, it will be automatically internally replaced by file name: "foo.xml" <see cref="DefaultVirtualFileName"/>.</param>
        /// <returns>After a completed validation returns <see cref="XmlValidationResult"/> with the details of the validation.</returns>
        /// <exception cref="ArgumentNullException">if the <paramref name="xml"/> is null, epty or just white space.</exception>
        public XmlValidationResult ValidateOneXml(string xml, string xmlFilePath)
        {
            return ValidateOneXmlWrapper(() => CreateValidatingXmlReader(xml, xmlFilePath));
        }

        /// <summary>
        /// Do a complete validation of a file.
        /// </summary>
        /// <param name="xmlReader">An <see cref="XmlReader"/> containing XML content which will be validated</param>
        /// <param name="xmlFilePath">Optional file name or a full path to the XML file which represents the <paramref name="xmlReader"/>. 
        /// THE FILE WILL NOT BE ACCESSED! It servees merely for validation messages.
        /// If null or empty, it will be automatically internally replaced by file name: "foo.xml" <see cref="DefaultVirtualFileName"/>.</param>
        /// <returns>After a completed validation returns <see cref="XmlValidationResult"/> with the details of the validation.</returns>
        public XmlValidationResult ValidateOneXml(XmlReader xmlReader, string xmlFilePath)
        {
            return ValidateOneXmlWrapper(() => CreateValidatingXmlReader(xmlReader, xmlFilePath));
        }

        /// <summary>
        /// Do a complete validation of a file.
        /// </summary>
        /// <param name="textReader">An <see cref="TextReader"/> containing XML content which will be validated</param>
        /// <param name="xmlFilePath">Optional file name or a full path to the XML file which represents the <paramref name="textReader"/>. 
        /// THE FILE WILL NOT BE ACCESSED! It servees merely for validation messages.
        /// If null or empty, it will be automatically internally replaced by file name: "foo.xml" <see cref="DefaultVirtualFileName"/>.</param>
        /// <returns>After a completed validation returns <see cref="XmlValidationResult"/> with the details of the validation.</returns>
        public XmlValidationResult ValidateOneXml(TextReader textReader, string xmlFilePath)
        {
            return ValidateOneXmlWrapper(() => CreateValidatingXmlReader(textReader, xmlFilePath));
        }

        /// <summary>
        /// Do a complete validation of a file.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> containing XML content which will be validated</param>
        /// <param name="xmlFilePath">Optional file name or a full path to the XML file which represents the <paramref name="stream"/>. 
        /// THE FILE WILL NOT BE ACCESSED! It servees merely for validation messages.
        /// If null or empty, it will be automatically internally replaced by file name: "foo.xml" <see cref="DefaultVirtualFileName"/>.</param>
        /// <returns>After a completed validation returns <see cref="XmlValidationResult"/> with the details of the validation.</returns>
        public XmlValidationResult ValidateOneXml(Stream stream, string xmlFilePath)
        {
            Throw.IfNull(stream, nameof(stream));

            //// reset the stream position to the beginning before validation
            //stream.Position = 0;

            XmlValidationResult result = ValidateOneXmlWrapper(() => CreateValidatingXmlReader(stream, xmlFilePath));

            //// reset the stream position back to the beginning after validation
            //stream.Position = 0;

            return result;
        }
        #endregion Validate One Xml
        #endregion API - Public Methods

        #region Private Methods - Inner implementation detail
        /// <summary>
        /// Mandatory XML Validator initialization, before it can be used for validation.
        /// </summary>
        protected virtual void Initialize()
        {
            // following line is necessary if using ENTITIES in XSD and / or the XML file
            OwnValidatingReaderSettings.DtdProcessing = DtdProcessing.Parse;

            // catch all validation messages here
            OwnValidatingReaderSettings.ValidationEventHandler += this.ValidationHandler;

            // avoid resolwing web uris:
            OwnValidatingReaderSettings.XmlResolver = DoNotGoToWebXmlResolver;

            //NOTE: following setting would cause resolving to apply also to the "KnownDefinitionFiles", which makes no sense in this implementation.
            // I assume that the provided KnownDefinitionFiles are ok to resolve by the default XmlUrlResolver
            //ValidatingReaderSettings.Schemas.XmlResolver = DontGo2WebXmlResolver;

            // to prohibit resolving of the XML file path beeing validated and optionaly also the XSD files
            this.DoNotGoToWebXmlResolver.DisableSpecificResolvingAndRelayJustOnStandardXmlUrlResolverResolving();
        }

        private ValidatingXmlReader ProtectedCreateValidatingXmlReaderInner(Func<XmlReader> getXmlReader, string xmlFilePath)
        {
            //NOTE: not creating the string reader and thus ignoring the "stringReader" param in subsequent call:
            return ProtectedCreateValidatingXmlReaderInner(null, stringReader => getXmlReader(), xmlFilePath);
        }

        private ValidatingXmlReader ProtectedCreateValidatingXmlReaderInner(Func<StringReader> getStringReader, Func<StringReader, XmlReader> getXmlReader, string xmlFilePath)
        {
            // Reentrancy protection:
            int reentrancyCnt = Interlocked.Increment(ref this.reentrancyProtectionCount);
            if (reentrancyCnt > 1)
            {
                Interlocked.Decrement(ref this.reentrancyProtectionCount);
                throw new InvalidOperationException($"The XML Validation was started for the {reentrancyCnt}. time, while the previous XML Validation is still in progress!");
            }

            // Implementing protected creation of IDisposable in a factory method:
            XmlValidator haveReentrancyDecrementResponsibility = this;
            StringReader optionalStringReaderTmp = null;
            XmlReader xmlReaderTmp = null;
            ValidatingXmlReader validatingXmlReaderTmp = null;

            try
            {
                // xmlFilePath correction:
                this.currentValidatedXmlFile = string.IsNullOrWhiteSpace(xmlFilePath) ? DefaultVirtualFileName : xmlFilePath;

                // Temporarily switch of specific resolving:

                // prohibit resolving of the XML file path beeing validated
                this.DoNotGoToWebXmlResolver.DisableSpecificResolvingAndRelayJustOnStandardXmlUrlResolverResolving();

                // follow the currently validated XML and allow resolving of the subsidiary files of the XML file beeing validated
                this.DoNotGoToWebXmlResolver.SetCurrentBaseUri(Path.GetDirectoryName(this.currentValidatedXmlFile));

                // ---------------------------------------------
                // create the validatingXmlReaderTmp in 3 steps:
                optionalStringReaderTmp = getStringReader?.Invoke() ?? null;
                xmlReaderTmp = getXmlReader(optionalStringReaderTmp);

                validatingXmlReaderTmp = ValidatingXmlReader.Create(this, xmlReaderTmp, optionalStringReaderTmp, this.currentValidatedXmlFile);

                // Decouple first part of IDisposable to return it and return it immediatelly now without the protecting dispose in finally:
                xmlReaderTmp = null;
                optionalStringReaderTmp = null;
                haveReentrancyDecrementResponsibility = null;

                // ----------------------------------------
                // Post ValidatingXmlReader creation stuff:
                this.LastValidationResult = validatingXmlReaderTmp.ValidationResult;
                this.currentValidatingXmlReader = validatingXmlReaderTmp;

                // after creating the XmlReader (which used "default XmlUrlResolver" resolving), enable the actual DoNotGoToWebXmlResolver functionality:
                this.DoNotGoToWebXmlResolver.EnableSpecificResolving();

                // Decouple last IDisposable to return it and return it immediatelly now without the protecting dispose in finally:
                ValidatingXmlReader validatingXmlReader = validatingXmlReaderTmp;
                validatingXmlReaderTmp = null;

                return validatingXmlReader;
            }
            finally
            {
                haveReentrancyDecrementResponsibility?.DecrementReentrancyProtectionAndDeregisterXmlReader();
                validatingXmlReaderTmp?.Dispose();
                xmlReaderTmp?.Dispose();
                optionalStringReaderTmp?.Dispose();
            }
        }

        private static XmlValidationResult ValidateOneXmlWrapper(Func<XmlReader> getValidatingXmlReader)
        {
            XmlValidationResult result = null;

            try
            {
                using ValidatingXmlReader reader = (ValidatingXmlReader)getValidatingXmlReader();
                result = reader.ValidationResult;

                while (!reader.StopReadingInvalidXml &&
                       reader.Read()
                      )
                {
                    // just read through the XML
                }
            }
            catch (XmlException)
            {
                // the XmlException is already handled by ValidatingXmlReader and stored in the ValidationResult
                // to here it is just rethrown to indicate the failure of validation
            }

            return result;
        }

        /// <summary>
        /// Decrement the internal Reentrancy Protection counter
        /// </summary>
        protected internal void DecrementReentrancyProtectionAndDeregisterXmlReader()
        {
            Interlocked.Decrement(ref this.reentrancyProtectionCount);
            this.currentValidatingXmlReader = null;
        }

        #region Unused Validation Handler Adding and Clearing
        /// <summary>
        /// Prohibited previous API method
        /// </summary>
        /// <param name="handler"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void SetExclusiveCustomValidationHandler(ValidationEventHandler handler)
        {
            RemoveAllValidationHandlers();
            this.OwnValidatingReaderSettings.ValidationEventHandler += handler;
        }

        /// <summary>
        /// Prohibited previous API method
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void ResetValidationHandlerToDefault()
        {
            RemoveAllValidationHandlers();
            this.OwnValidatingReaderSettings.ValidationEventHandler += this.ValidationHandler;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "S3011:Make sure that this accessibility bypass is safe here.", Justification = "Legacy code not in use - kept for historical reference only.")]
        private void RemoveAllValidationHandlers()
        {
            FieldInfo fiVEH = typeof(XmlReaderSettings).GetField("valEventHandler", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField);
            object obj = fiVEH.GetValue(this.OwnValidatingReaderSettings);
            foreach (Delegate dlgt in ((Delegate)obj).GetInvocationList())
            {
                this.OwnValidatingReaderSettings.ValidationEventHandler -= (ValidationEventHandler)dlgt;
            }
        }
        #endregion Unused Validation Handler Adding and Clearing

        /// <summary>
        /// OnRaiseRootElementEvent wrapper method
        /// </summary>
        /// <param name="root"></param>
        protected internal void OnRaiseRootElementEvent(XElement root)
        {
            this.RootElementHandler?.Invoke(this, new RootElementEventArgs(root));
        }

        /// <summary>
        /// OnRaiseXmlValidationIssueEvent wrapper method
        /// </summary>
        /// <param name="issue"></param>
        protected internal void OnRaiseXmlValidationIssueEvent(XmlValidationIssue issue)
        {
            this.XmlValidationIssueHandler?.Invoke(this, new XmlValidationIssueEventArgs(issue));
        }

        /// <summary>
        /// Catch and process every validation message here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ValidationHandler(object sender, ValidationEventArgs args)
        {
            XmlValidationIssue issue = XmlValidationIssue.Create(
              (XmlReader)sender,
              args,
              this.currentValidatedXmlFile,
              this.HasAdditionalDefinitionFile || this.HasDefinitionFiles
              );

#pragma warning disable CA1508
            if (issue == null)
#pragma warning restore CA1508
            {
                // this means DO IGNORE --> so this can be safely ignored
                return;
            }

            // Handle validation issue:

            if (issue.Severity >= ValidationSeverity.ErrorStopValidation &&
                this.currentValidatingXmlReader != null
                )
            {
                this.currentValidatingXmlReader.StopReadingInvalidXml = true;
            }

            //NOTE: store it for XPath computation during own validation and DO NOT fire it immediately: OnRaiseXmlValidationIssueEvent(issue)
            //      so, that we can add the XPath into the a
            this.currentValidatingXmlReader?.PendingXmlValidationErrors.Add(issue);
        }
        #endregion Private Methods - Inner implementation detail
    }
}
