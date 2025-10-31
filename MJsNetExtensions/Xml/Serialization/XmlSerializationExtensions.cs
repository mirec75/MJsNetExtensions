#pragma warning disable S125
namespace MJsNetExtensions.Xml.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.Serialization;


    /// <summary>
    /// Summary description for XmlSerializationExtensions
    /// </summary>
    public static class XmlSerializationExtensions
    {
        #region Forwarders for ToXml Serialization for Object
        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML string. NOTE: Throws exceptions!
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then internally the <see cref="XmlSerializationSettings"/> will be created based on it and used for serialization.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <returns>The XML string representing the serialized object</returns>
        /// <exception cref="InvalidOperationException">if <paramref name="toSerialize"/> is null, or if the serialization was not successfull.</exception>
        public static string ToXml(this object toSerialize)
        {
            return TryToXml(toSerialize)
                .OperationResultToResultOrException<string>();
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML string. NOTE: Throws exceptions!
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then the <paramref name="settings"/> will be either created based on it if null,
        /// or updated according to <see cref="XmlSerializationSettings.UpdateSettingsFrom(IXsiSchemaLocationInformation)"/> and must not contradict the data found in <see cref="IXsiSchemaLocationInformation"/>.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="settings">Optional. Can be null. The XML serialization settings: <see cref="XmlToStringSerializationSettings"/></param>
        /// <returns>The result of XML serialization in: <see cref="OperationResult{T}"/> containing an error or if successfull - the XML string representing the serialized object.</returns>
        /// <exception cref="InvalidOperationException">if <paramref name="toSerialize"/> is null, or if the serialization was not successfull.</exception>
        public static string ToXml(this object toSerialize, XmlToStringSerializationSettings settings)
        {
            return TryToXml(toSerialize, settings)
                .OperationResultToResultOrException<string>();
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML file <paramref name="filePath"/>. NOTE: Throws exceptions!
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then internally the <see cref="XmlSerializationSettings"/> will be created based on it and used for serialization.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="filePath">The file path to write to</param>
        /// <returns>The XML string of the serialized object</returns>
        /// <exception cref="InvalidOperationException">if <paramref name="toSerialize"/> is null, or if the serialization was not successfull.</exception>
        public static void ToXmlFile(this object toSerialize, string filePath)
        {
            TryToXmlFile(toSerialize, filePath)
                .ThrowInvalidIfOperationFailed();
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML file <paramref name="filePath"/>. NOTE: Throws exceptions!
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then the <paramref name="settings"/> will be either created based on it if null,
        /// or updated according to <see cref="XmlSerializationSettings.UpdateSettingsFrom(IXsiSchemaLocationInformation)"/> and must not contradict the data found in <see cref="IXsiSchemaLocationInformation"/>.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="filePath">The file path to write to</param>
        /// <param name="settings">Optional. Can be null. The XML serialization settings: <see cref="XmlSerializationSettings"/></param>
        /// <exception cref="InvalidOperationException">if <paramref name="toSerialize"/> is null, or if the serialization was not successfull.</exception>
        public static void ToXmlFile(this object toSerialize, string filePath, XmlSerializationSettings settings)
        {
            TryToXmlFile(toSerialize, filePath, settings)
                .ThrowInvalidIfOperationFailed();
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. NOTE: Throws exceptions!
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then internally the <see cref="XmlSerializationSettings"/> will be created based on it and used for serialization.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="xmlWriter">the <see cref="XmlWriter"/> used to write the XML document.</param>
        /// <exception cref="InvalidOperationException">if <paramref name="toSerialize"/> is null, or if the serialization was not successfull.</exception>
        public static void ToXml(this object toSerialize, XmlWriter xmlWriter)
        {
            TryToXml(toSerialize, xmlWriter)
                .ThrowInvalidIfOperationFailed();
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. NOTE: Throws exceptions!
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then the <paramref name="settings"/> will be either created based on it if null,
        /// or updated according to <see cref="XmlSerializationSettings.UpdateSettingsFrom(IXsiSchemaLocationInformation)"/> and must not contradict the data found in <see cref="IXsiSchemaLocationInformation"/>.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="xmlWriter">the <see cref="XmlWriter"/> used to write the XML document.</param>
        /// <param name="settings">Optional. Can be null. The XML serialization settings: <see cref="XmlSerializationSettings"/></param>
        /// <exception cref="InvalidOperationException">if <paramref name="toSerialize"/> is null, or if the serialization was not successfull.</exception>
        public static void ToXml(this object toSerialize, XmlWriter xmlWriter, XmlSerializationSettings settings)
        {
            TryToXml(toSerialize, xmlWriter, settings)
                .ThrowInvalidIfOperationFailed();
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. NOTE: Throws exceptions!
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then internally the <see cref="XmlSerializationSettings"/> will be created based on it and used for serialization.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="textWriter">the <see cref="TextWriter"/> used to write the XML document.</param>
        /// <exception cref="InvalidOperationException">if <paramref name="toSerialize"/> is null, or if the serialization was not successfull.</exception>
        public static void ToXml(this object toSerialize, TextWriter textWriter)
        {
            TryToXml(toSerialize, textWriter)
                .ThrowInvalidIfOperationFailed();
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. NOTE: Throws exceptions!
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then the <paramref name="settings"/> will be either created based on it if null,
        /// or updated according to <see cref="XmlSerializationSettings.UpdateSettingsFrom(IXsiSchemaLocationInformation)"/> and must not contradict the data found in <see cref="IXsiSchemaLocationInformation"/>.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="textWriter">the <see cref="TextWriter"/> used to write the XML document.</param>
        /// <param name="settings">Optional. Can be null. The XML serialization settings: <see cref="XmlSerializationSettings"/></param>
        /// <exception cref="InvalidOperationException">if <paramref name="toSerialize"/> is null, or if the serialization was not successfull.</exception>
        public static void ToXml(this object toSerialize, TextWriter textWriter, XmlSerializationSettings settings)
        {
            TryToXml(toSerialize, textWriter, settings)
                .ThrowInvalidIfOperationFailed();
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. NOTE: Throws exceptions!
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then internally the <see cref="XmlSerializationSettings"/> will be created based on it and used for serialization.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="stream">the <see cref="Stream"/> used to write the XML document.</param>
        /// <exception cref="InvalidOperationException">if <paramref name="toSerialize"/> is null, or if the serialization was not successfull.</exception>
        public static void ToXml(this object toSerialize, Stream stream)
        {
            TryToXml(toSerialize, stream)
                .ThrowInvalidIfOperationFailed();
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. NOTE: Throws exceptions!
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then the <paramref name="settings"/> will be either created based on it if null,
        /// or updated according to <see cref="XmlSerializationSettings.UpdateSettingsFrom(IXsiSchemaLocationInformation)"/> and must not contradict the data found in <see cref="IXsiSchemaLocationInformation"/>.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="stream">the <see cref="Stream"/> used to write the XML document.</param>
        /// <param name="settings">Optional. Can be null. The XML serialization settings: <see cref="XmlSerializationSettings"/></param>
        /// <exception cref="InvalidOperationException">if <paramref name="toSerialize"/> is null, or if the serialization was not successfull.</exception>
        public static void ToXml(this object toSerialize, Stream stream, XmlSerializationSettings settings)
        {
            TryToXml(toSerialize, stream, settings)
                .ThrowInvalidIfOperationFailed();
        }
        #endregion Forwarders for ToXml Serialization for Object


        #region Forwarders for TryToXml Serialization for Object
        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML string. 
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then internally the <see cref="XmlSerializationSettings"/> will be created based on it and used for serialization.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <returns>The result of XML serialization in: <see cref="OperationResult{T}"/> containing an error or if successfull - the XML string representing the serialized object.</returns>
        public static OperationResult<string> TryToXml(this object toSerialize)
        {
            return TryToXml(toSerialize, (XmlToStringSerializationSettings)null);
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to L file <paramref name="filePath"/>.
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then internally the <see cref="XmlSerializationSettings"/> will be created based on it and used for serialization.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="filePath">The file path to write to</param>
        /// <returns>The result of XML serialization in: <see cref="OperationResult"/>.</returns>
        public static OperationResult TryToXmlFile(this object toSerialize, string filePath)
        {
            return TryToXmlFile(toSerialize, filePath, null);
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. 
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then internally the <see cref="XmlSerializationSettings"/> will be created based on it and used for serialization.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="xmlWriter">the <see cref="XmlWriter"/> used to write the XML document.</param>
        /// <returns>The result of XML serialization in: <see cref="OperationResult"/>.</returns>
        public static OperationResult TryToXml(this object toSerialize, XmlWriter xmlWriter)
        {
            return TryToXml(toSerialize, xmlWriter, null);
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. 
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then internally the <see cref="XmlSerializationSettings"/> will be created based on it and used for serialization.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="textWriter">the <see cref="TextWriter"/> used to write the XML document.</param>
        /// <returns>The result of XML serialization in: <see cref="OperationResult"/>.</returns>
        public static OperationResult TryToXml(this object toSerialize, TextWriter textWriter)
        {
            return TryToXml(toSerialize, textWriter, null);
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. 
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then internally the <see cref="XmlSerializationSettings"/> will be created based on it and used for serialization.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="stream">the <see cref="Stream"/> used to write the XML document.</param>
        /// <returns>The result of XML serialization in: <see cref="OperationResult"/>.</returns>
        public static OperationResult TryToXml(this object toSerialize, Stream stream)
        {
            return TryToXml(toSerialize, stream, null);
        }
        #endregion Forwarders for TryToXml Serialization for Object

        #region THE SERIALIZING IMPLEMENTATION: Serialize an object to XML string

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML string. 
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then the <paramref name="settings"/> will be either created based on it if null,
        /// or updated according to <see cref="XmlSerializationSettings.UpdateSettingsFrom(IXsiSchemaLocationInformation)"/> and must not contradict the data found in <see cref="IXsiSchemaLocationInformation"/>.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="settings">Optional. Can be null. The XML serialization settings: <see cref="XmlToStringSerializationSettings"/></param>
        /// <returns>The result of XML serialization in: <see cref="OperationResult{T}"/> containing an error or if successfull - the XML string representing the serialized object.</returns>
        public static OperationResult<string> TryToXml(this object toSerialize, XmlToStringSerializationSettings settings)
        {
            if (toSerialize == null) { return OperationResult<string>.CreateArgumentNullFailure(nameof(toSerialize)); }

            try
            {
                //To get rid of the xml declaration <?xml version=\"1.0\" encoding=\"utf-8\"?> we do following:
                XmlWriterSettings xmlWriterSettings = settings?.XmlWriterSettings ?? XmlToStringSerializationSettings.CreateSerializerDefaultXmlWriterSettings();

                // Ensure we don't get the xml declaration <?xml version=\"1.0\" encoding=\"utf-8\"?>:
                xmlWriterSettings.OmitXmlDeclaration = true;

                var builder = new StringBuilder();
                OperationResult operationResult;
                //using (var stringWriter = new StringWriter(builder))
                //using (var writer = XmlWriter.Create(stringWriter, writerSettings))
                using (XmlWriter writer = XmlWriter.Create(builder, xmlWriterSettings)) // without writerSettings.OmitXmlDeclaration = true; this results in: <?xml version=\"1.0\" encoding=\"utf-16\"?>
                {
                    //NOTE: How to serialize an object to XML string without getting xmlns=“…”?
                    //      https://stackoverflow.com/questions/258960/how-to-serialize-an-object-to-xml-without-getting-xmlns
                    //      How can I make the xmlserializer only serialize plain xml?
                    //      https://stackoverflow.com/questions/1772004/how-can-i-make-the-xmlserializer-only-serialize-plain-xml

                    operationResult = TryToXml(toSerialize, writer, settings);
                }

                return operationResult.CreateGenericResult<string>(builder.ToString());
            }
            catch (ArgumentException ex)
            {
                return OperationResult<string>.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            catch (IOException ex)
            {
                return OperationResult<string>.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            catch (XmlException ex)
            {
                return OperationResult<string>.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            //catch (WebException ex)  <--> NOTE WebException derives from InvalidOperationException
            catch (InvalidOperationException ex)
            {
                return OperationResult<string>.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML file <paramref name="filePath"/>. 
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then the <paramref name="settings"/> will be either created based on it if null,
        /// or updated according to <see cref="XmlSerializationSettings.UpdateSettingsFrom(IXsiSchemaLocationInformation)"/> and must not contradict the data found in <see cref="IXsiSchemaLocationInformation"/>.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="filePath">The file path to write to</param>
        /// <param name="settings">Optional. Can be null. The XML serialization settings: <see cref="XmlSerializationSettings"/></param>
        /// <returns>The result of XML serialization in: <see cref="OperationResult"/>.</returns>
        public static OperationResult TryToXmlFile(this object toSerialize, string filePath, XmlSerializationSettings settings)
        {
            if (toSerialize == null) { return OperationResult.CreateArgumentNullFailure(nameof(toSerialize)); }
            if (string.IsNullOrWhiteSpace(filePath)) { return OperationResult.CreateArgumentNullFailure(nameof(filePath)); }

            try
            {
                using StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8);
                return TryToXml(toSerialize, writer, settings);
            }
            catch (ArgumentException ex)
            {
                return OperationResult.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            catch (IOException ex)
            {
                return OperationResult.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return OperationResult.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            catch (XmlException ex)
            {
                return OperationResult.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            //catch (WebException ex)  <--> NOTE WebException derives from InvalidOperationException
            catch (InvalidOperationException ex)
            {
                return OperationResult.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. 
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then the <paramref name="settings"/> will be either created based on it if null,
        /// or updated according to <see cref="XmlSerializationSettings.UpdateSettingsFrom(IXsiSchemaLocationInformation)"/> and must not contradict the data found in <see cref="IXsiSchemaLocationInformation"/>.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="xmlWriter">the <see cref="XmlWriter"/> used to write the XML document.</param>
        /// <param name="settings">Optional. Can be null. The XML serialization settings: <see cref="XmlSerializationSettings"/></param>
        /// <returns>The result of XML serialization in: <see cref="OperationResult"/>.</returns>
        public static OperationResult TryToXml(this object toSerialize, XmlWriter xmlWriter, XmlSerializationSettings settings)
        {
            if (xmlWriter == null) { return OperationResult.CreateArgumentNullFailure(nameof(xmlWriter)); }

            return TryToXmlInner(toSerialize,
                (serializer, settingsCorrected) => serializer.Serialize(xmlWriter, toSerialize, settingsCorrected.Namespaces),
                settings
                );
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. 
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then the <paramref name="settings"/> will be either created based on it if null,
        /// or updated according to <see cref="XmlSerializationSettings.UpdateSettingsFrom(IXsiSchemaLocationInformation)"/> and must not contradict the data found in <see cref="IXsiSchemaLocationInformation"/>.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="textWriter">the <see cref="TextWriter"/> used to write the XML document.</param>
        /// <param name="settings">Optional. Can be null. The XML serialization settings: <see cref="XmlSerializationSettings"/></param>
        /// <returns>The result of XML serialization in: <see cref="OperationResult"/>.</returns>
        public static OperationResult TryToXml(this object toSerialize, TextWriter textWriter, XmlSerializationSettings settings)
        {
            if (textWriter == null) { return OperationResult.CreateArgumentNullFailure(nameof(textWriter)); }

            return TryToXmlInner(toSerialize,
                (serializer, settingsCorrected) => serializer.Serialize(textWriter, toSerialize, settingsCorrected.Namespaces),
                settings
                );
        }

        /// <summary>
        /// Serialize <paramref name="toSerialize"/> object to XML. 
        /// If the objects implements <see cref="IXsiSchemaLocationInformation"/>, then the <paramref name="settings"/> will be either created based on it if null,
        /// or updated according to <see cref="XmlSerializationSettings.UpdateSettingsFrom(IXsiSchemaLocationInformation)"/> and must not contradict the data found in <see cref="IXsiSchemaLocationInformation"/>.
        /// </summary>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="stream">the <see cref="Stream"/> used to write the XML document.</param>
        /// <param name="settings">Optional. Can be null. The XML serialization settings: <see cref="XmlSerializationSettings"/></param>
        /// <returns>The result of XML serialization in: <see cref="OperationResult"/>.</returns>
        public static OperationResult TryToXml(this object toSerialize, Stream stream, XmlSerializationSettings settings)
        {
            if (stream == null) { return OperationResult.CreateArgumentNullFailure(nameof(stream)); }

            return TryToXmlInner(toSerialize,
                (serializer, settingsCorrected) => serializer.Serialize(stream, toSerialize, settingsCorrected.Namespaces),
                settings
                );
        }

        /// <summary>
        /// Factory method which creates a correct <see cref="XmlSerializer"/> and provides it an <see cref="XmlRootAttribute"/> in its constructor in a case, there is just <see cref="XmlTypeAttribute"/> defined on the <see cref="Type"/> of <paramref name="toSerialize"/>.
        /// NOTE: this method was necessary to avoid (workaround) nasty serializarion / deserialization exception for types with <see cref="XmlTypeAttribute"/>, but without <see cref="XmlRootAttribute"/>.
        /// </summary>
        /// <param name="toSerialize">An <see cref="object"/> to serialize or deserialize using <see cref="XmlSerializer"/>.</param>
        /// <returns>Correct <see cref="XmlSerializer"/> and provides it an <see cref="XmlRootAttribute"/> in its constructor in a case, there is just <see cref="XmlTypeAttribute"/> defined on the <see cref="Type"/> of <paramref name="toSerialize"/>.</returns>
        public static XmlSerializer CreateCorrectXmlSerializer(this object toSerialize)
        {
            string defaultNamespace;
            return toSerialize.CreateCorrectXmlSerializer(out defaultNamespace);
        }

        /// <summary>
        /// Factory method which creates a correct <see cref="XmlSerializer"/> and provides it an <see cref="XmlRootAttribute"/> in its constructor in a case, there is just <see cref="XmlTypeAttribute"/> defined on the <see cref="Type"/> of <paramref name="toSerialize"/>.
        /// NOTE: this method was necessary to avoid (workaround) nasty serializarion / deserialization exception for types with <see cref="XmlTypeAttribute"/>, but without <see cref="XmlRootAttribute"/>.
        /// </summary>
        /// <param name="toSerialize">An <see cref="object"/> to serialize or deserialize using <see cref="XmlSerializer"/>.</param>
        /// <param name="defaultNamespace">A possibility to get the extracted default (root) namespace of the <paramref name="toSerialize"/>.</param>
        /// <returns>Correct <see cref="XmlSerializer"/> and provides it an <see cref="XmlRootAttribute"/> in its constructor in a case, there is just <see cref="XmlTypeAttribute"/> defined on the <see cref="Type"/> of <paramref name="toSerialize"/>.</returns>
        public static XmlSerializer CreateCorrectXmlSerializer(this object toSerialize, out string defaultNamespace)
        {
            Throw.IfNull(toSerialize, nameof(toSerialize));

            Type typeToSerialize = toSerialize.GetType();
            return typeToSerialize.CreateCorrectXmlSerializer(out defaultNamespace);
        }

        /// <summary>
        /// Factory method which creates a correct <see cref="XmlSerializer"/> and provides it an <see cref="XmlRootAttribute"/> in its constructor in a case, there is just <see cref="XmlTypeAttribute"/> defined on the <paramref name="typeToSerialize"/>.
        /// NOTE: this method was necessary to avoid (workaround) nasty serializarion / deserialization exception for types with <see cref="XmlTypeAttribute"/>, but without <see cref="XmlRootAttribute"/>.
        /// </summary>
        /// <param name="typeToSerialize">A <see cref="Type"/> to serialize or deserialize using <see cref="XmlSerializer"/>.</param>
        /// <returns>Correct <see cref="XmlSerializer"/> and provides it an <see cref="XmlRootAttribute"/> in its constructor in a case, there is just <see cref="XmlTypeAttribute"/> defined on the <paramref name="typeToSerialize"/>.</returns>
        public static XmlSerializer CreateCorrectXmlSerializer(this Type typeToSerialize)
        {
            string defaultNamespace;
            return typeToSerialize.CreateCorrectXmlSerializer(out defaultNamespace);
        }

        /// <summary>
        /// Factory method which creates a correct <see cref="XmlSerializer"/> and provides it an <see cref="XmlRootAttribute"/> in its constructor in a case, there is just <see cref="XmlTypeAttribute"/> defined on the <paramref name="typeToSerialize"/>.
        /// NOTE: this method was necessary to avoid (workaround) nasty serializarion / deserialization exception for types with <see cref="XmlTypeAttribute"/>, but without <see cref="XmlRootAttribute"/>.
        /// </summary>
        /// <param name="typeToSerialize">A <see cref="Type"/> to serialize or deserialize using <see cref="XmlSerializer"/>.</param>
        /// <param name="defaultNamespace">A possibility to get the extracted default (root) namespace of the <paramref name="typeToSerialize"/>.</param>
        /// <returns>Correct <see cref="XmlSerializer"/> and provides it an <see cref="XmlRootAttribute"/> in its constructor in a case, there is just <see cref="XmlTypeAttribute"/> defined on the <paramref name="typeToSerialize"/>.</returns>
        public static XmlSerializer CreateCorrectXmlSerializer(this Type typeToSerialize, out string defaultNamespace)
        {
            Throw.IfNull(typeToSerialize, nameof(typeToSerialize));

            XmlTypeAttribute xmlTypeAttribute = typeToSerialize.GetCustomAttributes(typeof(XmlTypeAttribute), true)?.Cast<XmlTypeAttribute>().FirstOrDefault();
            XmlRootAttribute xmlRootAttribute = typeToSerialize.GetCustomAttributes(typeof(XmlRootAttribute), true)?.Cast<XmlRootAttribute>().FirstOrDefault();
            
            if (xmlTypeAttribute != null && xmlRootAttribute == null)
            {
                // we have to create a XmlRootAttribute, otherwise we are about to loase the root element namespace without this correction!
                xmlRootAttribute = new XmlRootAttribute { Namespace = xmlTypeAttribute.Namespace, DataType = xmlTypeAttribute.TypeName, };
            }

            defaultNamespace = xmlRootAttribute?.Namespace ?? null;

            XmlSerializer serializer = new XmlSerializer(typeToSerialize, xmlRootAttribute); // it's ok, if xmlRootAttribute is null
            return serializer;
        }
        #endregion THE SERIALIZING IMPLEMENTATION: Serialize an object to XML string

        #region Private Methods
        private static OperationResult TryToXmlInner(object toSerialize, Action<XmlSerializer, XmlSerializationSettings> callSerialize, XmlSerializationSettings settings)
        {
            if (toSerialize == null) { return OperationResult.CreateArgumentNullFailure(nameof(toSerialize)); }

            try
            {
                IXsiSchemaLocationInformation decoratedToSerialize = toSerialize as IXsiSchemaLocationInformation;
                if (settings == null)
                {
                    if (decoratedToSerialize == null)
                    {
                        settings = new XmlSerializationSettings(null, false);
                    }
                    else
                    {
                        settings = new XmlSerializationSettings(decoratedToSerialize.DefaultNamespace, decoratedToSerialize.AddSchemaLocationToResultXml);
                    }
                }
                else if (decoratedToSerialize != null)
                {
                    settings.UpdateSettingsFrom(decoratedToSerialize);
                }

                string defaultNamespace;
                XmlSerializer serializer = toSerialize.CreateCorrectXmlSerializer(out defaultNamespace);

                settings.UpdateSettingsFrom(defaultNamespace, false);

                callSerialize(serializer, settings);

                return OperationResult.CreateSuccess();
            }
            catch (ArgumentException ex)
            {
                return OperationResult.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            catch (TypeLoadException ex)
            {
                return OperationResult.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            catch (IOException ex)
            {
                return OperationResult.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return OperationResult.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            catch (XmlException ex)
            {
                return OperationResult.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
            //catch (WebException ex)  <--> NOTE WebException derives from InvalidOperationException
            catch (InvalidOperationException ex)
            {
                return OperationResult.CreateInvalidOperationFailure($"Error serializing: {toSerialize.GetType().FullName}", ex);
            }
        }
        #endregion Private Methods

    }
}
