using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJsNetExtensions.ObjectValidation;

namespace MJsNetExtensions.Xml.Validation
{
    /// <summary>
    /// Summary description for XmlValidatorSettings
    /// </summary>
    public class XmlValidatorSettings : ISimpleValidatable
    {
        #region Properties

        /// <summary>
        /// The type of validation requested: e.g. XSD or DTD. <see cref="XmlValidationType"/>.
        /// </summary>
        public XmlValidationType XmlValidationType { get; set; } = XmlValidationType.XSD;

        /// <summary>
        /// Optional. The file path or URI of the main XSD or DTD file to be used for validation. It can be null or empty regardless if the <see cref="AdditionalXmlDefinitionFilePaths"/> is null or empty or not empty.
        /// If available, XSD references in XML content (i.e. xsi:schemaLocation or xsi:noNamespaceSchemaLocation attributes) will try to resolve to this file based on file name comparision!
        /// NOTE: it is also OK to have NO XSD or DTD Definition file! In such case we just check the XML Well-Formedness!
        /// </summary>
        public string XmlDefinitionFilePath { get; set; }

        /// <summary>
        /// Optional additional XSD or DTD file paths or URIs to be used for validation.
        /// If available, XSD references in XML content (i.e. xsi:schemaLocation or xsi:noNamespaceSchemaLocation attributes) will try to resolve to this file based on file name comparision!
        /// NOTE: it is also OK to have NO XSD or DTD Definition file! In such case we just check the XML Well-Formedness!
        /// </summary>
        public IEnumerable<string> AdditionalXmlDefinitionFilePaths { get; set; }

        /// <summary>
        /// If you have no idea don't change the property default: <see cref="UriTranslationMode.ForcedTranslateAllAddresses"/>. For more see the <see cref="UriTranslationMode"/>.
        /// </summary>
        public UriTranslationMode UriTranslationMode { get; set; } = UriTranslationMode.ForcedTranslateAllAddresses;

        /// <summary>
        /// The kind of XML we are validating, e.g. "Config", or just some "XML", etc.
        /// It will be used as an additional description in result message, like: $"The {XmlKind} file {xmlFilePath} is valid ...".
        /// If the XmlKind is e.g. "Config", then the message will be "The Config file abc.config is valid ..."
        /// Default is "XML".
        /// </summary>
        public string XmlKind { get; set; } = "XML";

        #endregion Properties

        #region API - Public Methods

        /// <summary>
        /// Creates a copy of the <see cref="XmlValidatorSettings"/> instance.
        /// </summary>
        /// <returns></returns>
        public XmlValidatorSettings Clone() 
        {
            // make ShallowCopy first:
            XmlValidatorSettings clone = (XmlValidatorSettings)this.MemberwiseClone();

            // deep copy:
            //NOTE: MAKE OWN COPY! IEnumerable can be volatile!
            clone.AdditionalXmlDefinitionFilePaths = this.AdditionalXmlDefinitionFilePaths?.ToArray();

            return clone;
        }

        /// <summary>
        /// The settings validation method.
        /// The Pre Structure validation method, which is called from inside of <see cref="ValidationExtensions.Validate(ISimpleValidatable)"/> before all subcomponents of the validatable object gets validated.
        /// In this phase all the simple properties have to be validated, which are not closely related to complex object subtree of the validatable object.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public void PreStructureValidation(ValidationResult validationResult)
        {
            if (!string.IsNullOrWhiteSpace(this.XmlDefinitionFilePath) ||
                (this.AdditionalXmlDefinitionFilePaths?.Any(it => !string.IsNullOrWhiteSpace(it)) ?? false)
                )
            {
                // if there is at least 1 valid XSD or DTD definition file => all ok!
            }
            else
            {
                //NOTE: it is also OK to have NO XSD or DTD Definition file! In such case we just check the XML Well-Formedness!
                // so DO NOT: validationResult.AddErrorMessage($"{nameof(this.XmlDefinitionFilePath)} not provided");
            }

            // make corrections:
            if (string.IsNullOrWhiteSpace(this.XmlKind))
            {
                this.XmlKind = "XML";
            }
        }

        #endregion API - Public Methods
    }
}
