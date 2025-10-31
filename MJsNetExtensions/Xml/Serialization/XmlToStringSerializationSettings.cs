using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MJsNetExtensions.Xml.Serialization
{
    /// <summary>
    /// Summary description for XmlToStringSerializationSettings
    /// </summary>
    public class XmlToStringSerializationSettings : XmlSerializationSettings
    {
        #region Construction / Destruction
        /// <summary>
        /// Default constructor creating an empty instance of <see cref="XmlToStringSerializationSettings"/>.
        /// </summary>
        public XmlToStringSerializationSettings()
        {
        }

        /// <summary>
        /// Create an instance of <see cref="XmlToStringSerializationSettings"/> with the property: <see cref="XmlSerializationSettings.Namespaces"/> filled with <see cref="XmlSerializerNamespaces"/> without XSD namespace, 
        /// with an optional <paramref name="defaultNamespace"/> and with optional XSI namespace <see cref="XmlSchema.InstanceNamespace"/>, based on <paramref name="addSchemaLocationToResultXml"/>,
        /// whether the result XML shall contain "xsi:schemaLocation", or "xsi:noNamespaceSchemaLocation" attribute, or not.
        /// </summary>
        /// <param name="defaultNamespace">Optional. Can be null or empty. Default XML namespace.</param>
        /// <param name="addSchemaLocationToResultXml">if true, the "xsi" namespace <see cref="XmlSchema.InstanceNamespace"/> will be added to the result XML.</param>
        public XmlToStringSerializationSettings(string defaultNamespace, bool addSchemaLocationToResultXml)
            : base(defaultNamespace, addSchemaLocationToResultXml)
        {
        }

        /// <summary>
        /// Create an instance of <see cref="XmlSerializationSettings"/> with the property: <see cref="XmlSerializationSettings.Namespaces"/> filled with <see cref="XmlSerializerNamespaces"/> without XSD namespace, 
        /// with an optional <paramref name="toSerialize.DefaultNamespace"/> and with optional XSI namespace <see cref="XmlSchema.InstanceNamespace"/>, based on <paramref name="toSerialize.DefaultNamespace"/>,
        /// whether the result XML shall contain "xsi:schemaLocation", or "xsi:noNamespaceSchemaLocation" attribute, or not.
        /// </summary>
        /// <param name="toSerialize">The <see cref="XmlSerializableRootElementBase"/> to serialize.</param>
        public XmlToStringSerializationSettings(XmlSerializableRootElementBase toSerialize)
            : base(toSerialize)
        {
        }
        #endregion Construction / Destruction

        #region Properties

        /// <summary>
        /// Optional, can be null. <see cref="System.Xml.XmlWriterSettings"/> used to serialize an object to XML string. 
        /// If null, the the default is created using: <see cref="CreateOwnSerializerDefaultXmlWriterSettings()"/>.
        /// </summary>
        public XmlWriterSettings XmlWriterSettings { get; set; }

        #endregion Properties

        #region API - Public Methods
        /// <summary>
        /// Create default <see cref="System.Xml.XmlWriterSettings"/> for the <see cref="XmlSerializer"/> with <see cref="XmlWriterSettings.Indent"/> = true 
        /// and <see cref="XmlWriterSettings.OmitXmlDeclaration"/> = true to get rid of the xml declaration &lt;?xml version=\&quot;1.0\&quot; encoding=\&quot;utf-8\&quot;?&gt;.
        /// </summary>
        /// <returns>The newly created and replaced value of property: <see cref="XmlToStringSerializationSettings.XmlWriterSettings"/>.</returns>
        public XmlWriterSettings CreateOwnSerializerDefaultXmlWriterSettings()
        {
            //To get rid of the xml declaration <?xml version=\"1.0\" encoding=\"utf-8\"?> we do following:
            this.XmlWriterSettings = CreateSerializerDefaultXmlWriterSettings();

            return this.XmlWriterSettings;
        }

        /// <summary>
        /// Create default <see cref="System.Xml.XmlWriterSettings"/> for the <see cref="XmlSerializer"/> with <see cref="XmlWriterSettings.Indent"/> = true 
        /// and <see cref="XmlWriterSettings.OmitXmlDeclaration"/> = true to get rid of the xml declaration &lt;?xml version=\&quot;1.0\&quot; encoding=\&quot;utf-8\&quot;?&gt;.
        /// </summary>
        /// <returns>The newly created and replaced value of property: <see cref="XmlToStringSerializationSettings.XmlWriterSettings"/>.</returns>
        public static XmlWriterSettings CreateSerializerDefaultXmlWriterSettings()
        {
            //To get rid of the xml declaration <?xml version=\"1.0\" encoding=\"utf-8\"?> we do following:
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                // overriding defaults:
                OmitXmlDeclaration = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                Indent = true,

                //NOTE: the Encoding = UTF8 is the default! But it is ignored if writing the XML declaration, due to either taking StringBuilder's UTF-16, or the StringWriter's "nothing", or the StringWriterWithEncoding's UTF-8 !
                //Encoding = Encoding.UTF8,
            };

            return xmlWriterSettings;
        }
        #endregion API - Public Methods
    }
}
