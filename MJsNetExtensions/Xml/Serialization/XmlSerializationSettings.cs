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
    /// The XML Serialization Settings
    /// </summary>
    public class XmlSerializationSettings
    {
        #region Construction
        /// <summary>
        /// Default constructor creating an empty instance of <see cref="XmlSerializationSettings"/>.
        /// </summary>
        public XmlSerializationSettings() { }

        /// <summary>
        /// Create an instance of <see cref="XmlSerializationSettings"/> with the property: <see cref="Namespaces"/> filled with <see cref="XmlSerializerNamespaces"/> without XSD namespace, 
        /// with an optional <paramref name="defaultNamespace"/> and with optional XSI namespace <see cref="XmlSchema.InstanceNamespace"/>, based on <paramref name="addSchemaLocationToResultXml"/>,
        /// whether the result XML shall contain "xsi:schemaLocation", or "xsi:noNamespaceSchemaLocation" attribute, or not.
        /// </summary>
        /// <param name="defaultNamespace">Optional. Can be null or empty. Default XML namespace.</param>
        /// <param name="addSchemaLocationToResultXml">if true, the "xsi" namespace <see cref="XmlSchema.InstanceNamespace"/> will be added to the result XML.</param>
        public XmlSerializationSettings(string defaultNamespace, bool addSchemaLocationToResultXml)
        {
            CreateOwnXmlSerializerNamespaces(defaultNamespace, addSchemaLocationToResultXml);
        }

        /// <summary>
        /// Create an instance of <see cref="XmlSerializationSettings"/> with the property: <see cref="Namespaces"/> filled with <see cref="XmlSerializerNamespaces"/> without XSD namespace, 
        /// with an optional <paramref name="toSerialize.DefaultNamespace"/> and with optional XSI namespace <see cref="XmlSchema.InstanceNamespace"/>, based on <paramref name="toSerialize.DefaultNamespace"/>,
        /// whether the result XML shall contain "xsi:schemaLocation", or "xsi:noNamespaceSchemaLocation" attribute, or not.
        /// </summary>
        /// <param name="toSerialize">The <see cref="IXsiSchemaLocationInformation"/> to serialize.</param>
        public XmlSerializationSettings(IXsiSchemaLocationInformation toSerialize)
            : this(toSerialize?.DefaultNamespace, toSerialize?.AddSchemaLocationToResultXml ?? false)
        {
            Throw.IfNull(toSerialize, nameof(toSerialize));
        }
        #endregion Construction

        #region Properties

        /// <summary>
        /// Optional. Can be null. The <see cref="System.Xml.Serialization.XmlSerializerNamespaces"/> for the <see cref="XmlSerializer"/>. 
        /// Is null, then the resulting XML contains xmlns:xsi and xmlns:xsd namespaces...
        /// To omit this, create a neu empty instance of <see cref="System.Xml.Serialization.XmlSerializerNamespaces"/>. 
        /// See: How to serialize an object to XML string without getting xmlns=“…”?
        /// https://stackoverflow.com/questions/258960/how-to-serialize-an-object-to-xml-without-getting-xmlns
        /// </summary>
        public XmlSerializerNamespaces Namespaces { get; set; }

        #endregion Properties

        #region API - Public Methods
        /// <summary>
        /// Check if own settings match the settings of <see cref="IXsiSchemaLocationInformation"/> and update own settings if not configured yet, e.g. add the default namespace, or XSI namespace,
        /// if <see cref="IXsiSchemaLocationInformation.AddSchemaLocationToResultXml"/> is true and the XSI namespace does not exist yet.
        /// </summary>
        /// <param name="toSerialize">The <see cref="IXsiSchemaLocationInformation"/> to serialize.</param>
        public void UpdateSettingsFrom(IXsiSchemaLocationInformation toSerialize)
        {
            UpdateSettingsFrom(toSerialize?.DefaultNamespace, toSerialize?.AddSchemaLocationToResultXml ?? false);
        }

        /// <summary>
        /// Check if own settings match the settings of <see cref="IXsiSchemaLocationInformation"/> and update own settings if not configured yet, e.g. add the default namespace, or XSI namespace,
        /// if <see cref="IXsiSchemaLocationInformation.AddSchemaLocationToResultXml"/> is true and the XSI namespace does not exist yet.
        /// </summary>
        /// <param name="defaultNamespace">Optional. Can be null or empty. Default XML namespace.</param>
        /// <param name="addSchemaLocationToResultXml">if true, the "xsi" namespace <see cref="XmlSchema.InstanceNamespace"/> will be added to the result XML.</param>
        public void UpdateSettingsFrom(string defaultNamespace, bool addSchemaLocationToResultXml)
        {
            if (this.Namespaces == null)
            {
                CreateOwnXmlSerializerNamespaces(defaultNamespace, addSchemaLocationToResultXml);
            }
            else
            {
                var currentNamespaces = this.Namespaces.ToArray();

                // add defaultNamespace if not already existing:
                if (currentNamespaces.All(it => string.CompareOrdinal(it.Namespace, defaultNamespace) != 0))
                {
                    XmlQualifiedName defaultNamespaceQn =
                        currentNamespaces.FirstOrDefault(it => string.IsNullOrWhiteSpace(it.Name)) ?? null;

                    if (string.IsNullOrWhiteSpace(defaultNamespaceQn
                            ?.Namespace)) //NOTE: !! the "this.Namespaces.Add(...)" also overwrites the setting without an error! Nice!! :)
                    {
                        this.Namespaces.Add("", defaultNamespace);
                    }
                    else
                    {
                        // Need to add a new not used prefix for the default namespace, as there is already another default namespace defined:
                        HashSet<string> prefixes = currentNamespaces.Select(it => it.Name).ToHashSet();
                        string newPrefix;
                        int counter = 0;
                        {
                            newPrefix = $"q{++counter}";
                        }
                        while (prefixes.Contains(newPrefix)) ;

                        // store the object's serialization default namespace with the new non-conflicting prefix:
                        this.Namespaces.Add(newPrefix, defaultNamespace);
                    }
                }

                // add XSI namespace if requested and not already existing:
                if (addSchemaLocationToResultXml)
                {
                    XmlQualifiedName xsiNamespace = currentNamespaces.FirstOrDefault(it => string.CompareOrdinal(it.Namespace, XmlSchema.InstanceNamespace) == 0) ?? null;
                    if (xsiNamespace == null)
                    {
                        // If not adding this namespace attribute with an explicite prefix of "xsi", then a cryptical namespace prefix is used for it automatically.
                        this.Namespaces.Add("xsi", XmlSchema.InstanceNamespace);
                    }
                }
                //else --> nevermind. It's the responsibility of the XmlToStringSerializationSettings creator...
            }
        }

        /// <summary>
        /// Create and replace the value of own property: <see cref="Namespaces"/> with <see cref="XmlSerializerNamespaces"/> without XSD namespace, 
        /// with an optional <paramref name="defaultNamespace"/> and with optional XSI namespace <see cref="XmlSchema.InstanceNamespace"/>, based on <paramref name="addSchemaLocationToResultXml"/>,
        /// whether the result XML shall contain "xsi:schemaLocation", or "xsi:noNamespaceSchemaLocation" attribute, or not.
        /// </summary>
        /// <param name="defaultNamespace">Optional. Can be null or empty. Default XML namespace.</param>
        /// <param name="addSchemaLocationToResultXml">if true, the "xsi" namespace <see cref="XmlSchema.InstanceNamespace"/> will be added to the result XML.</param>
        /// <returns>The newly created and replaced value of property: <see cref="Namespaces"/>.</returns>
        public XmlSerializerNamespaces CreateOwnXmlSerializerNamespaces(string defaultNamespace, bool addSchemaLocationToResultXml)
        {
            //Create our own namespaces for the output
            this.Namespaces = CreateXmlSerializerNamespaces(defaultNamespace, addSchemaLocationToResultXml);
            return this.Namespaces;
        }

        /// <summary>
        /// Create and replace the value of own property: <see cref="Namespaces"/> with <see cref="XmlSerializerNamespaces"/> without XSD namespace, 
        /// with an optional <paramref name="defaultNamespace"/> and with optional XSI namespace <see cref="XmlSchema.InstanceNamespace"/>, based on <paramref name="addSchemaLocationToResultXml"/>,
        /// whether the result XML shall contain "xsi:schemaLocation", or "xsi:noNamespaceSchemaLocation" attribute, or not.
        /// </summary>
        /// <param name="defaultNamespace">Optional. Can be null or empty. Default XML namespace.</param>
        /// <param name="addSchemaLocationToResultXml">if true, the "xsi" namespace <see cref="XmlSchema.InstanceNamespace"/> will be added to the result XML.</param>
        /// <returns>The newly created and replaced value of property: <see cref="Namespaces"/>.</returns>
        public static XmlSerializerNamespaces CreateXmlSerializerNamespaces(string defaultNamespace, bool addSchemaLocationToResultXml)
        {
            //NOTE: How to serialize an object to XML string without getting xmlns=“…”?
            //      https://stackoverflow.com/questions/258960/how-to-serialize-an-object-to-xml-without-getting-xmlns
            //      How can I make the xmlserializer only serialize plain xml?
            //      https://stackoverflow.com/questions/1772004/how-can-i-make-the-xmlserializer-only-serialize-plain-xml

            //Create our own namespaces for the output
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", defaultNamespace); //NOTE: you can add an empty namespace here. For 'defaultNamespace' it's ok to be "" or null.

            // Classes and structs implementing IXsiSchemaLocationInformation writes the xsi:schemaLocation or xsi:noNamespaceSchemaLocation attributes forced by this interface. 
            // Classes and structs NOT implementing IXsiSchemaLocationInformation, but requesting to 'addSchemaLocationToResultXml' get the xsi:*schemaLocation attribute by the Specific postprocessing below.
            if (addSchemaLocationToResultXml)
            {
                // If not adding this namespace attribute with an explicite prefix of "xsi", then a cryptical namespace prefix is used for it automatically.
                namespaces.Add("xsi", XmlSchema.InstanceNamespace);
            }

            return namespaces;
        }
        #endregion API - Public Methods
    }
}
