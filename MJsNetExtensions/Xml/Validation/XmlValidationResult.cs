#pragma warning disable S125
namespace MJsNetExtensions.Xml.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MJsNetExtensions.ObjectValidation;


    /// <summary>
    /// Summary description for XmlValidationResult
    /// </summary>
    public sealed class XmlValidationResult : IValidationResult
    {
        #region Fields

        private List<XmlValidationIssue> xmlValidationIssues = new();

        //private string validationResultMessage = null;

        #endregion Fields

        #region Construction / Destruction
        // prohibit user default creation
        internal XmlValidationResult() { }

        #endregion Construction / Destruction

        #region Properties

        /// <summary>
        /// Gets a flag ingdicating the summarized overall validation state - if the validation was successfull, or not.
        /// </summary>
        public bool IsValid => this.ErrorsCount == 0 && !this.WasException;

        /// <summary>
        /// Gets the final validation human readable summary message, saying if the validation was successful or not.
        /// </summary>
        public string ValidationResultMessage => GetValidationResultMessage();

        /// <summary>
        /// Collection of <see cref="XmlValidationIssue"/>-s.
        /// </summary>
        public IEnumerable<XmlValidationIssue> XmlValidationIssues => xmlValidationIssues;

        /// <summary>
        /// The kind of XML we are validating, e.g. "Config", or just some "XML", etc.
        /// It will be used as an additional description in result message, like: $"The {XmlKind} file {xmlFilePath} is valid ...".
        /// If the XmlKind is e.g. "Config", then the message will be "The Config file abc.config is valid ..."
        /// Default is "XML".
        /// </summary>
        public string XmlKind { get; private set; } = "XML";

        /// <summary>
        /// The type of validation, e.g. XSD, DTD, etc. <see cref="XmlValidationType"/>.
        /// </summary>
        public XmlValidationType XmlValidationType { get; private set; } = XmlValidationType.XSD;

        /// <summary>
        /// The Validated XML File
        /// </summary>
        public string ValidatedXmlFile { get; private set; } = XmlValidator.DefaultVirtualFileName;

        /// <summary>
        /// Gets the stored Known Definition file paths. E.g. XSDs or DTDs.
        /// </summary>
        public IEnumerable<string> KnownDefinitionFiles { get; private set; } = [];

        /// <summary>
        /// Gets the count of errors.
        /// </summary>
        public int ErrorsCount { get; internal set; }

        /// <summary>
        /// Gets the count of warnings.
        /// </summary>
        public int WarningsCount { get; internal set; }

        /// <summary>
        /// Flag indicating if an exception occurred during validation.
        /// </summary>
        public bool WasException { get; internal set; }


        #region Remaining IValidationResult Properties

        /// <summary>
        /// If the <see cref="IsValid"/> is true, this property returns null.
        /// Othewise it returns the final aggregated invalid reason string, consisting of the optional <see cref="InvalidReasonPrefix"/>
        /// and the collected partial <see cref="InvalidReasons"/> for all error conditions which were true. All particles are separated by <see cref="Environment.NewLine"/>.
        /// </summary>
        public string InvalidReason => this.IsValid ? null : ToString(ValidationSeverity.Error);

        /// <summary>
        /// The optional invalid reason prefix. If provided and the <see cref="IsValid"/> is false, then it is uesed as a prefix of the final <see cref="InvalidReason"/> message.
        /// </summary>
        public string InvalidReasonPrefix { get => this.ValidationResultMessage;
            set { /* ignore */ } }

        /// <summary>
        /// The sorted collection of the single invalid reason particles, which form finally the aggregated <see cref="InvalidReason"/>.
        /// </summary>
        public IEnumerable<string> InvalidReasons => GetValidationMessages(ValidationSeverity.Error);

        #endregion Remaining IValidationResult Properties

        #endregion Properties

        #region API - Public Methods
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return ToString(default(ValidationSeverity)); // <-- means: Take All!
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="separator">A separator string, to separate each individual invalid reason message.
        /// If the parameterless <seealso cref="ToString()"/> is called, then the default separator: <see cref="Environment.NewLine"/> is used.</param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(string separator)
        {
            return ToString(default(ValidationSeverity), separator);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="severityAndAbove">creates a result string only with severity and above messages. <see cref="ValidationSeverity.Warning"/> means: take all.</param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(ValidationSeverity severityAndAbove)
        {
            return ToString(severityAndAbove, Environment.NewLine);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="severityAndAbove">creates a result string only with severity and above messages. <see cref="ValidationSeverity.Warning"/> means: take all.</param>
        /// <param name="separator">A separator string, to separate each individual invalid reason message.
        /// If the parameterless <seealso cref="ToString()"/> is called, then the default separator: <see cref="Environment.NewLine"/> is used.</param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(ValidationSeverity severityAndAbove, string separator)
        {
            return string.Join(separator, GetValidationMessages(severityAndAbove));
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="severityAndAbove">creates a result string only with severity and above messages. <see cref="ValidationSeverity.Warning"/> means: take all.</param>
        /// <returns>A string that represents the current object.</returns>
        public IEnumerable<string> GetValidationMessages(ValidationSeverity severityAndAbove)
        {
            List<string> messages = new List<string>();

            messages.AddRange(
                this.XmlValidationIssues
                .Where(it => it.Severity >= severityAndAbove)
                .Select(it => it.ToString())
                );

            if (severityAndAbove < ValidationSeverity.Error ||
                messages.Count == 0
                )
            {
                messages.Insert(0, this.ValidationResultMessage);
            }

            return messages;
        }
        #endregion API - Public Methods

        #region Internal Methods

        internal void HandleXmlValidationIssue(object sender, XmlValidationIssueEventArgs e)
        {
            if (e?.Issue != null)
            {
                this.xmlValidationIssues.Add(e.Issue);
            }
        }

        internal void UpdateInfo(XmlValidator xmlValidator, string xmlFilePath)
        {
            this.XmlKind = xmlValidator.Settings.XmlKind;
            this.XmlValidationType = xmlValidator.XmlValidationType;
            this.KnownDefinitionFiles = xmlValidator.KnownDefinitionFiles;

            this.ValidatedXmlFile = xmlFilePath;
        }

        private string GetValidationResultMessage()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(CultureInfo.InvariantCulture, "The {0} file \"{1}\" is ", this.XmlKind, this.ValidatedXmlFile);

            if (!this.IsValid)
            {
                sb.Append("in");
            }

            sb.Append("valid");

            if (this.KnownDefinitionFiles.Any())
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, " according to {0}s: \"{1}\"", this.XmlValidationType, string.Join("; ", this.KnownDefinitionFiles));
            }

            if (this.WasException)
            {
                sb.Append(", Exception catched.");
            }

            if (this.ErrorsCount != 0)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, ", Errors: {0}", this.ErrorsCount);
            }

            if (this.WarningsCount != 0)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, ", Warnings: {0}", this.WarningsCount);
            }

            sb.Append('.');

            if (!this.IsValid || this.WasException || this.ErrorsCount != 0 || this.WarningsCount != 0)
            {
                sb.Append(" See the validation detail messages.");
            }

            //this.validationResultMessage = sb.ToString();
            return sb.ToString();
        }
        #endregion Internal Methods
    }
}
