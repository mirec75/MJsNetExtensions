#pragma warning disable S1854
#pragma warning disable IDE0003
#pragma warning disable S2325
#pragma warning disable CA1862
namespace XmlValidatorExe
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    using System.IO;
    using System.Reflection;
    using System.Globalization;
    using MJsNetExtensions.Xml.Validation;

    /// <summary>
    /// A simple XML validator console tool and also as an API for general XML validation usage
    /// See docu 4 more: http://support.microsoft.com/kb/307379
    /// </summary>
    internal sealed class SimpleXmlValidator
    {
        private string progname = "";

        public XmlValidator Validator { get; private set; }


        private static int Main(string[] args)
        {
            SimpleXmlValidator sxv = new SimpleXmlValidator();
            int ret = sxv.ValidationTest(args);
            return ret;
        }

        /// <summary>
        /// Prints heading information for this Program to the Logging Framework.
        /// </summary>
        private static void PrintProgramHeader()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var attributes = assembly.GetCustomAttributes(false);
            string title = attributes.OfType<AssemblyTitleAttribute>().SingleOrDefault()?.Title 
                ?? assembly.GetName().Name 
                ?? "XMLValidator";
            string description = attributes.OfType<AssemblyDescriptionAttribute>().SingleOrDefault()?.Description 
                ?? "This program is intended for XML file validation against XSD or DTD definition.";
            string copyright = attributes.OfType<AssemblyCopyrightAttribute>().SingleOrDefault()?.Copyright;
            Version assemblyVersion = assembly.GetName().Version;

            Assembly assemblyXmlValidator = typeof(XmlValidator).Assembly;
            Version assemblyVersionXmlValidator = assemblyXmlValidator.GetName().Version;

            SimpleTracer.Prnln("-----------------------------------------------------");
            SimpleTracer.Prnln(" {0} - Version: {1}, XmlValidator: {2}", title, assemblyVersion, assemblyVersionXmlValidator);
            SimpleTracer.Prnln(" {0}", description);
            if (!string.IsNullOrWhiteSpace(copyright))
            {
                SimpleTracer.Prnln(" {0}", copyright);
            }
            SimpleTracer.Prnln("-----------------------------------------------------");
        }

        private void PrnUsage()
        {
            SimpleTracer.Prnln();

            PrintProgramHeader();

            SimpleTracer.Prnln();
            SimpleTracer.Prnln("Usage:");
            SimpleTracer.Prnln("{0} (-xsd | -dtd) xmlFile [xsdOrDtdFile(s)] [-doNotResolveWebUris] \n", progname);
            SimpleTracer.Prnln("  (-xsd | -dtd) - only one of this 2 validating options can be used.");
            SimpleTracer.Prnln("  -xsd or -x    - validating using XSD, i.e. XML Schema Definitions.");
            SimpleTracer.Prnln("  -dtd or -d    - validating using DTD. i.e. old times Document Type Definitions\n");
            SimpleTracer.Prnln("  xmlFile       - xml file to be validated.");
            SimpleTracer.Prnln("  xsdOrDtdFile(s)  - (optional) One or more XSD or DTD file(s) to be used for validation.");
            SimpleTracer.Prnln("                  For XSD this parameter overrides / superimposes the setting stored");
            SimpleTracer.Prnln("                  in attribute xsi:schemaLocation. Separate multiple files with , or ;");
            SimpleTracer.Prnln("                  NOTE: for DTD the DTD file name must be referenced in the DOCTYPE definition");
            SimpleTracer.Prnln("                  inside the XML file. This parameter specifies the exact location for resolving");
            SimpleTracer.Prnln("                  of the DTD file, if it cannot be found in the place specified in the DOCTYPE definition.");
            SimpleTracer.Prnln();
            SimpleTracer.Prnln("  -doNotResolveWebUris or -ignoreWeb  - (optional but recommended!) prohibit to resolve");
            SimpleTracer.Prnln("                  Web (e.g. HTTP addresses) and Filesystem URIs which are not relative");
            SimpleTracer.Prnln("                  to the validated XML. The explicit providing of 'xsdOrDtdFile' parameter");
            SimpleTracer.Prnln("                  is mandatory for such cases.");
            SimpleTracer.Prnln("                  If the definition file name in 'xsdOrDtdFile' is the same (case insensitive)");
            SimpleTracer.Prnln("                  as the file name in the relative path inside the definition reference");
            SimpleTracer.Prnln("                  in the XML (i.e. in xsi:schemaLocation or DOCTYPE), it overrides the relative");
            SimpleTracer.Prnln("                  file lookup and the file from the 'xsdOrDtdFile' parameter will be taken instead!\n");
            SimpleTracer.Prnln();
        }

        private SimpleXmlValidator()
        {
        }

        public SimpleXmlValidator(XmlValidatorMode validatorMode, IEnumerable<string> knownDefinitionFiles, bool doNotResolveWebAddresses)
        {
            InternalInitFromProgArgs(validatorMode, knownDefinitionFiles, doNotResolveWebAddresses);
        }

        private void InternalInitFromProgArgs(XmlValidatorMode validatorMode, IEnumerable<string> knownDefinitionFiles, bool doNotResolveWebAddresses)
        {
            UriTranslationMode uriTranslationMode = (doNotResolveWebAddresses) ? UriTranslationMode.ForcedTranslateAllAddresses : UriTranslationMode.TranslateNoneButRelayOnXmlUrlResolver;

            XmlValidatorSettings settings = new XmlValidatorSettings
            {
                XmlValidationType = validatorMode == XmlValidatorMode.Dtd ? XmlValidationType.DTD : XmlValidationType.XSD,
                AdditionalXmlDefinitionFilePaths = knownDefinitionFiles,
                UriTranslationMode = uriTranslationMode,
            };

            this.Validator = XmlValidator.Create(settings);

            // initialize
            this.Validator.RootElementHandler += this.HandleRootElement;
            this.Validator.XmlValidationIssueHandler += this.HandleXmlValidationIssue;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private int ValidationTest(string[] args)
        {
            // set globals

            // set locals
            DateTime startTime = DateTime.Now;
            string xmlFile = string.Empty;
            bool doNotResolveWebAddresses = false;
            List<string> knownDefinitionFiles = new List<string>();
            bool paramsOk = false;
            bool wasException = false;
            XmlValidationResult result = null;

            try
            {
                // assembly attributes...
                Assembly asm = Assembly.GetExecutingAssembly();
                progname = asm.GetName().Name;

                //SimpleTracer.Prnln("Program: {0}  Full Path: {1}", progname, new asm.Location);
                //SimpleTracer.Prnln("Started: {0:R}", startTime);

                if (args == null || args.Length < 2
                    || string.IsNullOrWhiteSpace(args[0])
                    || string.IsNullOrWhiteSpace(args[1])
                  )
                {
                    SimpleTracer.Prnln("Arguments not valid");
                    PrnUsage();
                    return (int)ReturnCode.ErrorReadingParameters;
                }

                xmlFile = Path.GetFullPath(args[1]);
                if (!File.Exists(xmlFile))
                {
                    SimpleTracer.Prnln("Input XML file does not exist: {0}", xmlFile);
                    PrnUsage();
                    return (int)ReturnCode.ErrorReadingParameters;
                }

                if (args.Length > 2)
                {
                    if (string.Equals(args[2], "-doNotResolveWebUris", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(args[2], "-ignoreWeb", StringComparison.OrdinalIgnoreCase))
                    {
                        doNotResolveWebAddresses = true;
                    }
                    else
                    {
                        string xsdFilesParameter = args[2];
                        if (string.IsNullOrEmpty(xsdFilesParameter))
                        {
                            SimpleTracer.Prnln("ERROR: No xsdFileSpecified - XSD / DTD validation not possible.", xsdFilesParameter);
                            PrnUsage();
                            return (int)ReturnCode.ErrorReadingXsdFile;
                        }

                        string[] xsdFiles = xsdFilesParameter.Split([',', ';'], StringSplitOptions.RemoveEmptyEntries);
                        knownDefinitionFiles.AddRange(xsdFiles); //OLD bad: xsdFiles.Where(File.Exists)
                        if (knownDefinitionFiles.Count == 0)
                        {
                            SimpleTracer.Prnln("ERROR: No XSD file(s) could be loaded, which were provided in the program parameters.\nThe provided program XSD file parameter: {0}", xsdFilesParameter);
                            PrnUsage();
                            return (int)ReturnCode.ErrorReadingXsdFile;
                        }
                    }
                }

                if (args.Length > 3)
                {
                    if (string.IsNullOrEmpty(args[3])
                      || (!string.Equals(args[3], "-doNotResolveWebUris", StringComparison.OrdinalIgnoreCase) &&
                          !string.Equals(args[3], "-ignoreWeb", StringComparison.OrdinalIgnoreCase)))
                    {
                        SimpleTracer.Prnln("Last Argument not understood: {0}", args[3]);
                        PrnUsage();
                        return (int)ReturnCode.ErrorReadingXsdFile;
                    }
                    doNotResolveWebAddresses = true;
                }

                var ic = CultureInfo.InvariantCulture;
                Assembly assembly = Assembly.GetExecutingAssembly();
                Version assemblyVersion = assembly.GetName().Version;

                Assembly assemblyXmlValidator = typeof(XmlValidator).Assembly;
                Version assemblyVersionXmlValidator = assemblyXmlValidator.GetName().Version;
                if (args[0].Trim().ToLower(ic) == "-xsd" || args[0].Trim().ToLower(ic) == "-x")
                {
                    SimpleTracer.Prnln("{0:R}: Program: {1} Version: {2}, XmlValidator: {3}  XSD validation Full Path: {4}", startTime, progname, assemblyVersion, assemblyVersionXmlValidator, asm.Location);
                    this.InternalInitFromProgArgs(XmlValidatorMode.Xsd, knownDefinitionFiles, doNotResolveWebAddresses);
                }
                else if (args[0].Trim().ToLower(ic) == "-dtd" || args[0].Trim().ToLower(ic) == "-d")
                {
                    SimpleTracer.Prnln("{0:R}: Program: {1} Version: {2}, XmlValidator: {3}  DTD validation Full Path: {4}", startTime, progname, assemblyVersion, assemblyVersionXmlValidator, asm.Location);
                    this.InternalInitFromProgArgs(XmlValidatorMode.Dtd, knownDefinitionFiles, doNotResolveWebAddresses);
                }
                else
                {
                    SimpleTracer.Prnln("Unknown Option: {0}", args[0]);
                    PrnUsage();
                    return (int)ReturnCode.InvalidParameters;
                }

                paramsOk = true;

                // do validation:
                result = this.Validator.ValidateOneXml(xmlFile);
                //SimpleTracer.Prnln();
                //SimpleTracer.Prnln("=====================");
                //SimpleTracer.Prnln(result.ToString());
                //SimpleTracer.Prnln("=====================");
                //SimpleTracer.Prnln();
            }
            catch (Exception ex)
            {
                wasException = true;
                SimpleTracer.TraceException("An exception occurred during XML validation:", ex);
            }
            finally
            {
                result = this.Validator?.LastValidationResult;

                if (!paramsOk || 
                    result == null // exception was already logged.
                    )
                {
                    // wrong parameters provided -> everything has been already output to the console so -> just exit
                }
                else
                {
                    SimpleTracer.Prn("Validation finished. ");
                    if (wasException)
                        SimpleTracer.Prnln("There was an exception !");
                    else if (result.ErrorsCount == 0)
                    {
                        string warnMsg = (result.WarningsCount == 0) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " (warnings {0})", result.WarningsCount);
                        SimpleTracer.Prnln("The XML file \"{0}\" is a VALID XML file !{1}", xmlFile, warnMsg);
                    }
                    else if (result.ErrorsCount == 1 && result.WasException)
                        SimpleTracer.Prnln("There was an Exception{0}!", (result.WarningsCount > 0) ? " and " + result.WarningsCount + " warnings" : "");
                    else if (result.ErrorsCount == 1)
                        SimpleTracer.Prnln("There is 1 error and {0} warnings!", result.WarningsCount);
                    else if (result.WasException)
                        SimpleTracer.Prnln("There was an Exception, {0} errors and {1} warnings!", result.ErrorsCount - 1, result.WarningsCount);
                    else
                        SimpleTracer.Prnln("There are {0} errors and {1} warnings!", result.ErrorsCount, result.WarningsCount);
                }
            }

            //DateTime endTime = DateTime.Now;
            //TimeSpan duration = endTime.Subtract(startTime);
            //SimpleTracer.Prnln("Finished: {0:R}, took: {1}", endTime, duration.ToString());

            //Console.ReadLine();
            return (!paramsOk || wasException || result?.ErrorsCount != 0) ? (int)ReturnCode.ValidationErrors : (int)ReturnCode.Ok;
        }

        /// <summary>
        /// Handle Root Element context information
        /// </summary>
        /// <param name="reader"></param>
        void HandleRootElement(object sender, RootElementEventArgs e)
        {
            // ignore for now
        }

        /// <summary>
        /// Handle one <see cref="XmlValidationIssue"/>
        /// </summary>
        /// <param name="reader"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        void HandleXmlValidationIssue(object sender, XmlValidationIssueEventArgs e)
        {
            SimpleTracer.PrnString(e?.Issue?.ToString() + "\n");
        }

    }
}

