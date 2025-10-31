#pragma warning disable S3011
namespace MJsNetExtensions.Xml.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;


    /// <summary>
    /// XML Validation Issue class.
    /// </summary>
    public class XmlValidationIssue
    {
        #region Construction is internal!
        // prohibit user default construction
        private XmlValidationIssue() { }

        /// <summary>
        /// The usual validation handler message drive creation of XmlValidationIssue
        /// </summary>
        /// <param name="reader">Param from validation handler</param>
        /// <param name="args">Param from validation handler</param>
        /// <param name="xmlFile">The XML file being validated</param>
        /// <param name="hasDefinitionFile">HasAdditionalDefinitionFile || HasDefinitionFilesGivenInConstruction</param>
        internal static XmlValidationIssue Create(XmlReader reader, ValidationEventArgs args, string xmlFile, bool hasDefinitionFile)
        {
            Throw.IfNull(reader, nameof(reader));
            Throw.IfNull(args, nameof(args));
            Throw.IfNullOrWhiteSpace(xmlFile, nameof(xmlFile));

            XmlValidationIssue validationIssue = new XmlValidationIssue
            {
                XmlFile = xmlFile,
                Severity = (args.Severity == XmlSeverityType.Error) ? ValidationSeverity.Error : ValidationSeverity.Warning,
            };

            XmlSchemaValidationException validationException = args.Exception as XmlSchemaValidationException;

            if (args.Severity == XmlSeverityType.Warning                        // if Validation Engine clasified this as a warning
              && validationException != null                                                // and we got a Validation Exception
              && reader != null && reader.NodeType == XmlNodeType.Element && reader.Depth == 0  // and we are just now on the root element
              )
            {
                if (args.Message.Contains(DoNotGoToWebXmlResolver.CannotResolveUri, StringComparison.CurrentCultureIgnoreCase) ||
                    args.Message.Contains(DoNotGoToWebXmlResolver.ResolvedUriAlreadyLoaded, StringComparison.CurrentCultureIgnoreCase)
                    )
                {
                    // this can be safely ignored
                    return null;
                }
                else if (hasDefinitionFile)
                {
                    // investigate if this is a root element not covered by any XSD

                    PropertyInfo piGetRes = validationException.GetType().GetProperty("GetRes", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
                    string exceptionKind = (string)piGetRes.GetValue(validationException, null);

                    if (exceptionKind == "Sch_NoElementSchemaFound")
                    {
                        validationIssue.Severity = ValidationSeverity.ErrorStopValidation;
                    }
                }
            }

            // store the important issue data
            validationIssue.LineNumber = args.Exception.LineNumber;
            validationIssue.LinePosition = args.Exception.LinePosition;
            validationIssue.Message = args.Message;

            if (validationException != null)
            {
                if (!string.IsNullOrWhiteSpace(validationException.SourceUri))
                {
                    Uri sourceUri = new Uri(validationException.SourceUri);
                    if (!string.Equals(xmlFile, sourceUri.LocalPath, StringComparison.OrdinalIgnoreCase))
                    {
                        validationIssue.Message += " Source of error: " + validationException.SourceUri;
                    }
                }

                // Following line is necessary, because the resulting ValidationEventArgs.Message just contains this message, given 
                if (validationException.InnerException != null)
                {
                    if (validationIssue.Message.Contains(validationException.InnerException.Message, StringComparison.Ordinal))
                    {
                        validationIssue.Message += GetInnerExceptionMessages(validationException.InnerException.InnerException);
                    }
                    else
                    {
                        validationIssue.Message += GetInnerExceptionMessages(validationException.InnerException);
                    }
                }
            }

            // compute the last part of XPath, in a case we do not get the chance to get it set from the reader
            validationIssue.XPath = string.Empty;
            if (reader.NodeType == XmlNodeType.Element || reader.NodeType == XmlNodeType.EndElement || reader.NodeType == XmlNodeType.Attribute)
            {
                if (reader.NodeType == XmlNodeType.Attribute)
                {
                    validationIssue.AttributeLocalName = reader.LocalName;
                    validationIssue.XPath += "@";
                }

                if (!string.IsNullOrWhiteSpace(reader.Prefix))
                {
                    validationIssue.XPath += reader.Prefix + ":";
                }

                validationIssue.XPath += reader.LocalName;
            }

            return validationIssue;
        }

        internal XmlValidationIssue(Exception ex, string xmlFile)
          : this()
        {
            Throw.IfNull(ex, nameof(ex));
            Throw.IfNullOrWhiteSpace(xmlFile, nameof(xmlFile));

            this.XmlFile = xmlFile;
            this.Severity = ValidationSeverity.Exception;
            this.Message = ex.ToString();

            if (ex is XmlException xex)
            {
                this.LineNumber = xex.LineNumber;
                this.LinePosition = xex.LinePosition;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Property for internal purposes only - for XPath computation in XmlValidatorBase
        /// </summary>
        internal string AttributeLocalName { get; private set; }

        // public properties:

        /// <summary>
        /// Gets the <see cref="ValidationSeverity"/> of this issue.
        /// </summary>
        public ValidationSeverity Severity { get; private set; }

        /// <summary>
        /// Gets the XmlFile.
        /// </summary>
        public string XmlFile { get; private set; }

        /// <summary>
        /// Gets the LineNumber of this issue.
        /// </summary>
        public int LineNumber { get; private set; } = -1;

        /// <summary>
        /// Gets the LinePosition of this issue.
        /// </summary>
        public int LinePosition { get; private set; } = -1;

        /// <summary>
        /// Gets the XPath related to this issue.
        /// </summary>
        public string XPath { get; internal set; }

        /// <summary>
        /// Gets the Message of this issue.
        /// </summary>
        public string Message { get; private set; }

        #endregion Properties

        #region API - Public Methods
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.XPath))
                return string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}: {3}: {4}",
                  this.XmlFile, this.LineNumber, this.LinePosition,
                  this.Severity, this.Message);
            else
                return string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}: {3}: xPath: {4} Message: {5}",
                  this.XmlFile, this.LineNumber, this.LinePosition,
                  this.Severity, this.XPath, this.Message);
        }
        #endregion API - Public Methods

        #region Private Methods
        private static string GetInnerExceptionMessages(Exception ex, string indent = "")
        {
            if (ex == null)
            {
                return null;
            }

            string result = $"\n{indent}--> {ex.Message}{GetInnerExceptionMessages(ex.InnerException, indent + "  ")}";

            return result;
        }
        #endregion Private Methods

    }
}
