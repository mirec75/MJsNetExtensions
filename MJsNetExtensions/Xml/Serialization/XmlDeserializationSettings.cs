using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MJsNetExtensions.ObjectValidation;
using MJsNetExtensions.Xml.Validation;

namespace MJsNetExtensions.Xml.Serialization
{
    /// <summary>
    /// The XML Deserialization Settings
    /// </summary>
    public class XmlDeserializationSettings<T>
    {
        #region Properties

        /// <summary>
        /// Optional. Can be null. 1st priority Get or Create "Factory" like method to get / create <see cref="XmlValidator"/> for the XML Validation. 
        /// If provided, the optional <see cref="XmlValidatorSettings"/> is just used as a parameter for this Get or Create method.
        /// NOTE: this is a convenience method, to be able to utilize e.g. <see cref="ThreadLocalXmlValidatorFacade"/>.
        /// </summary>
        public Func<XmlValidatorSettings, XmlValidator> GetXmlValidator { get; set; }

        /// <summary>
        /// Optional. Can be null. The <see cref="Validation.XmlValidatorSettings"/> object used to create and configure the new XSD or DTD <see cref="XmlValidator"/>.
        /// If the <see cref="GetXmlValidator"/> is null, then this property is simply used in a call: <see cref="XmlValidator.Create(XmlValidatorSettings)"/>.
        /// NOTE: in a case this settings is null or has empty settings, and there is no <see cref="GetXmlValidator"/> or is based on this property, 
        /// then no XSD or DTD validation will be done. Just XML well-formedness will be checked.
        /// </summary>
        public XmlValidatorSettings XmlValidatorSettings { get; set; }

        /// <summary>
        /// Optional. Can be null. A "virtual" file name or a full path to the XML file which represents the actual XML being read via <see cref="XmlReader"/>, or <see cref="TextReader"/> or <see cref="Stream"/> using the methods:
        /// <see cref="XmlValidator.CreateValidatingXmlReader(XmlReader, string)"/>, or <see cref="XmlValidator.CreateValidatingXmlReader(Stream, string)"/>, or <see cref="XmlValidator.CreateValidatingXmlReader(string, string)"/>.
        /// THE FILE WILL NOT BE ACCESSED! It servees merely for validation messages.
        /// If null or empty, it will be automatically internally replaced by file name: "foo.xml" <see cref="XmlValidator.DefaultVirtualFileName"/>.
        /// </summary>
        public string VirtualXmlFileName { get; set; }

        /// <summary>
        /// Optional. Can be null. A post creation pre validation action, which will be called right after the XML deserialization 
        /// and before the configSection.ValidateAndInitialize() is called.
        /// </summary>
        public Action<T> PostCreationPreValidationAction { get; set; }

        /// <summary>
        /// Optional. Can be null. The Strongly-Typed Object Validation settings - <see cref="ValidationSettings"/>.
        /// </summary>
        public ValidationSettings ObjectValidationSettings { get; set; } = new();

        #endregion Properties

        #region API - Public Methods - Object Equality Comparison

        /// <summary>
        /// Clone method
        /// </summary>
        /// <returns></returns>
        public XmlDeserializationSettings<T> Clone()
        {
            // make ShallowCopy first:
            XmlDeserializationSettings<T> clone = (XmlDeserializationSettings<T>)this.MemberwiseClone();

            // deep copy:
            clone.XmlValidatorSettings = this.XmlValidatorSettings.Clone();
            clone.ObjectValidationSettings = (ValidationSettings) this.ObjectValidationSettings.Clone();

            return clone;
        }

        #endregion API - Public Methods - Object Equality Comparison
    }
}
