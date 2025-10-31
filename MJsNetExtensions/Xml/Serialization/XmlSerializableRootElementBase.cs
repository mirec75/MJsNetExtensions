namespace MJsNetExtensions.Xml.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Schema;
    using System.Xml.Serialization;


    /// <summary>
    /// Summary description for XmlSerializableRootElementBase
    /// </summary>
    public abstract class XmlSerializableRootElementBase : IXsiSchemaLocationInformation
    {
        /// <summary>
        /// Gets the data object containig the <see cref="XsiSchemaLocationInformation"/>.
        /// </summary>
        [XmlIgnore]
        public abstract IXsiSchemaLocationInformation XsiSchemaLocationInformation { get; }

        /// <summary>
        /// Get the default XML namespace of the XSD
        /// </summary>
        [XmlIgnore]
        public virtual string DefaultNamespace => this.XsiSchemaLocationInformation?.DefaultNamespace;

        /// <summary>
        /// Get the XSD location <see cref="Uri"/>.
        /// </summary>
        [XmlIgnore]
        public virtual string XsdLocationUrl => this.XsiSchemaLocationInformation?.XsdLocationUrl;

        /// <summary>
        /// If true, then the serialized XML contains an XSD reference in the form of xsi:schemaLocation or xsi:noNamespaceSchemaLocation
        /// </summary>
        [XmlIgnore]
        public virtual bool AddSchemaLocationToResultXml => this.XsiSchemaLocationInformation?.AddSchemaLocationToResultXml ?? false;

        /// <summary>
        /// The value of this attribute corresponds to the xsi:schemaLocation. It is not null only if <see cref="AddSchemaLocationToResultXml"/> is true AND the <see cref="DefaultNamespace"/> and <see cref="XsdLocationUrl"/> ARE NOT empty.
        /// NOTE for implementors: use [XmlAttribute(WellKnownXmlConsts.XmlSchemaLocationAttributeName, Namespace = XmlSchema.InstanceNamespace)] for this property!
        /// </summary>
        [XmlAttribute(WellKnownXmlConstants.XmlSchemaLocationAttributeName, Namespace = XmlSchema.InstanceNamespace)]
        public virtual string XsiSchemaLocationAttributeValue
        {
            //NOTE: see the discussion here to know why this property is implemented this way:
            // Force XML serialization to serialize readonly property
            // https://stackoverflow.com/questions/5585364/force-xml-serialization-to-serialize-readonly-property
            get => !this.AddSchemaLocationToResultXml || string.IsNullOrWhiteSpace(this.DefaultNamespace) ? null : $"{this.DefaultNamespace} {this.XsdLocationUrl}";
            set
            {
                //NOTE: following exception caused XmlSerializer.Deserialize() to crash. So I commented it just out:
                //throw new NotSupportedException($"Setting the {nameof(XsiSchemaLocationAttributeValue)} property is not supported!");
            }
        }

        /// <summary>
        /// The value of this attribute corresponds to the xsi:noNamespaceSchemaLocation. It is not null only if <see cref="AddSchemaLocationToResultXml"/> is true AND <see cref="XsdLocationUrl"/> is NOT empty AND the <see cref="DefaultNamespace"/> IS empty.
        /// NOTE for implementors: use [XmlAttribute(WellKnownXmlConsts.XmlNoNamespaceSchemaLocationAttributeName, Namespace = XmlSchema.InstanceNamespace)] for this property!
        /// </summary>
        [XmlAttribute(WellKnownXmlConstants.XmlNoNamespaceSchemaLocationAttributeName, Namespace = XmlSchema.InstanceNamespace)]
        public virtual string XsiNoNamespaceSchemaLocationAttributeValue
        {
            get => this.AddSchemaLocationToResultXml && string.IsNullOrWhiteSpace(this.DefaultNamespace) ? this.XsdLocationUrl : null;
            set
            {
                //NOTE: following exception caused XmlSerializer.Deserialize() to crash. So I commented it just out:
                //throw new NotSupportedException($"Setting the {nameof(XsiNoNamespaceSchemaLocationAttributeValue)} property is not supported!");
            }
        }
    }
}
