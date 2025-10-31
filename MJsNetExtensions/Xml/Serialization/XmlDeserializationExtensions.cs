#pragma warning disable S125

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MJsNetExtensions.ObjectValidation;
using MJsNetExtensions.Xml.Validation;

namespace MJsNetExtensions.Xml.Serialization
{
    /// <summary>
    /// Summary description for XmlDeserializationExtensions
    /// </summary>
    public static class XmlDeserializationExtensions
    {
        #region Plain deserialization without Validation
        /// <summary>
        /// Generic method to deserialize an XML string <paramref name="serialized"/> to an object of the desired generic type: <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="serialized">XML string containing the serialized object.</param>
        /// <returns>The deserialized object of type: <typeparamref name="T"/>, or throws.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="serialized"/> is null or white space</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T ParseXmlTo<T>(this string serialized)
        {
            Throw.IfNullOrWhiteSpace(serialized, nameof(serialized));

            try
            {
                using StringReader stringReader = new StringReader(serialized);
                return stringReader.XmlTo<T>();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"serialized: {serialized.Substring(0, Math.Min(1000, serialized.Length))}", ex);
            }
        }

        /// <summary>
        /// Generic method to deserialize an XML stored in <paramref name="xmlFilePath"/> to an object of the desired generic type: <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="xmlFilePath">File path to the file containing the XML of the serialized object.</param>
        /// <returns>The deserialized object of type: <typeparamref name="T"/>, or throws.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="xmlFilePath"/> is null or white space</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T LoadXmlTo<T>(this string xmlFilePath)
        {
            Throw.IfNullOrWhiteSpace(xmlFilePath, nameof(xmlFilePath));

            try
            {
                using StreamReader streamReader = new StreamReader(xmlFilePath);
                return streamReader.XmlTo<T>();
            }
            //catch (IOException ex) { }  <--> has already the necessary info inside
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"XML File Path: {xmlFilePath}", ex);
            }
        }

        /// <summary>
        /// Generic method to deserialize an XML to an object of the desired generic type: <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the XML of the serialized object.</param>
        /// <returns>The deserialized object or default value of type: <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="xmlReader"/> is null or white space</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T XmlTo<T>(this XmlReader xmlReader)
        {
            Throw.IfNull(xmlReader, nameof(xmlReader));

            T deserialized = default(T);

            XmlSerializer serializer = typeof(T).CreateCorrectXmlSerializer();
            deserialized = (T)serializer.Deserialize(xmlReader);

            return deserialized;
        }

        /// <summary>
        /// Generic method to deserialize an XML to an object of the desired generic type: <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="textReader">The <see cref="TextReader"/> containing the XML of the serialized object.</param>
        /// <returns>The deserialized object or default value of type: <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="textReader"/> is null or white space</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T XmlTo<T>(this TextReader textReader)
        {
            Throw.IfNull(textReader, nameof(textReader));

            T deserialized = default(T);

            XmlSerializer serializer = typeof(T).CreateCorrectXmlSerializer();

            // Secure XmlReader settings
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                // set XmlResolver to null, mitigating the security risks flagged by CA5369. This ensures deserialization is performed safely.
                XmlResolver = null
            };

            //NOTE: Do not call .Deserialize directly on a Stream or TextReader, which can allow unsafe XML features like DTDs and external entity resolution
            using XmlReader xmlReader = XmlReader.Create(textReader, settings);
            deserialized = (T)serializer.Deserialize(xmlReader);

            return deserialized;
        }

        /// <summary>
        /// Generic method to deserialize an XML to an object of the desired generic type: <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="stream">The <see cref="Stream"/> containing the XML of the serialized object.</param>
        /// <returns>The deserialized object or default value of type: <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="stream"/> is null</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T XmlTo<T>(this Stream stream)
        {
            Throw.IfNull(stream, nameof(stream));

            T deserialized = default(T);

            XmlSerializer serializer = typeof(T).CreateCorrectXmlSerializer();

            // Secure XmlReader settings
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                // set XmlResolver to null, mitigating the security risks flagged by CA5369. This ensures deserialization is performed safely.
                XmlResolver = null
            };

            //NOTE: Do not call .Deserialize directly on a Stream or TextReader, which can allow unsafe XML features like DTDs and external entity resolution
            using XmlReader xmlReader = XmlReader.Create(stream, settings);
            deserialized = (T)serializer.Deserialize(xmlReader);

            return deserialized;
        }
        #endregion Plain deserialization without Validation

        #region Forwarders for ReadXmlToAndValidate Deserialization with Validation

        /// <summary>
        /// Generic method to deserialize an XML string <paramref name="serialized"/> to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="serialized">XML string containing the serialized object.</param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object 
        /// of type: <typeparamref name="T"/>, or default value, if <paramref name="serialized"/> was null or empty.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="serialized"/> is null or white space</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T ParseXmlToAndValidate<T>(this string serialized)
        {
            return TryParseXmlToAndValidate<T>(serialized)
                .OperationResultToResultOrException<T>();
        }

        /// <summary>
        /// Generic method to deserialize an XML stored in <paramref name="xmlFilePath"/> to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="xmlFilePath">File path to the file containing the XML of the serialized object.</param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object 
        /// of type: <typeparamref name="T"/>, or default value, if <paramref name="xmlFilePath"/> was null or empty.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="xmlFilePath"/> is null or white space</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T LoadXmlToAndValidate<T>(this string xmlFilePath)
        {
            return TryLoadXmlToAndValidate<T>(xmlFilePath)
                .OperationResultToResultOrException<T>();
        }

        /// <summary>
        /// Generic method deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the XML of the serialized object.</param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object of type: <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="xmlReader"/> is null</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T ReadXmlToAndValidate<T>(this XmlReader xmlReader)
        {
            return TryReadXmlToAndValidate<T>(xmlReader)
                .OperationResultToResultOrException<T>();
        }

        /// <summary>
        /// Generic method deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="textReader">The <see cref="TextReader"/> containing the XML of the serialized object.</param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object of type: <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="textReader"/> is null</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T ReadXmlToAndValidate<T>(this TextReader textReader)
        {
            return TryReadXmlToAndValidate<T>(textReader)
                .OperationResultToResultOrException<T>();
        }

        /// <summary>
        /// Generic method to deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="stream">The <see cref="Stream"/> containing the XML of the serialized object.</param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object of type: <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="stream"/> is null</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T ReadXmlToAndValidate<T>(this Stream stream)
        {
            return TryReadXmlToAndValidate<T>(stream)
                .OperationResultToResultOrException<T>();
        }
        #endregion Forwarders for ReadXmlToAndValidate Deserialization with Validation

        #region Forwarders for ReadXmlToAndValidate Deserialization with Validation With Settings

        /// <summary>
        /// Generic method to deserialize an XML string <paramref name="serialized"/> to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="serialized">XML string containing the serialized object.</param>
        /// <param name="settings">Optional. Can be null. The XML and object validation settings: <see cref="XmlDeserializationSettings{t}"/></param>
        /// <returns>If successfull, it returns the result of XML and Strongly-Typed Object Validations: <typeparamref name="T"/>, else it throws an <see cref="InvalidOperationException"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="serialized"/> is null or white space</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T ParseXmlToAndValidate<T>(this string serialized, XmlDeserializationSettings<T> settings)
        {
            return TryParseXmlToAndValidate<T>(serialized, settings)
                .OperationResultToResultOrException<T>();
        }

        /// <summary>
        /// Generic method to deserialize an XML stored in <paramref name="xmlFilePath"/> to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="xmlFilePath">File path to the file containing the XML of the serialized object.</param>
        /// <param name="settings">Optional. Can be null. The XML and object validation settings: <see cref="XmlDeserializationSettings{t}"/></param>
        /// <returns>If successfull, it returns the result of XML and Strongly-Typed Object Validations: <typeparamref name="T"/>, else it throws an <see cref="InvalidOperationException"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="xmlFilePath"/> is null or white space</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T LoadXmlToAndValidate<T>(this string xmlFilePath, XmlDeserializationSettings<T> settings)
        {
            return TryLoadXmlToAndValidate<T>(xmlFilePath, settings)
                .OperationResultToResultOrException<T>();
        }

        /// <summary>
        /// Generic method deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the XML of the serialized object.</param>
        /// <param name="settings">Optional. Can be null. The XML and object validation settings: <see cref="XmlDeserializationSettings{t}"/></param>
        /// <returns>If successfull, it returns the result of XML and Strongly-Typed Object Validations: <typeparamref name="T"/>, else it throws an <see cref="InvalidOperationException"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="xmlReader"/> is null</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T ReadXmlToAndValidate<T>(this XmlReader xmlReader, XmlDeserializationSettings<T> settings)
        {
            return TryReadXmlToAndValidate<T>(xmlReader, settings)
                .OperationResultToResultOrException<T>();
        }

        /// <summary>
        /// Generic method deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="textReader">The <see cref="TextReader"/> containing the XML of the serialized object.</param>
        /// <param name="settings">Optional. Can be null. The XML and object validation settings: <see cref="XmlDeserializationSettings{t}"/></param>
        /// <returns>If successfull, it returns the result of XML and Strongly-Typed Object Validations: <typeparamref name="T"/>, else it throws an <see cref="InvalidOperationException"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="textReader"/> is null</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T ReadXmlToAndValidate<T>(this TextReader textReader, XmlDeserializationSettings<T> settings)
        {
            return TryReadXmlToAndValidate<T>(textReader, settings)
                .OperationResultToResultOrException<T>();
        }

        /// <summary>
        /// Generic method to deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="stream">The <see cref="Stream"/> containing the XML of the serialized object.</param>
        /// <param name="settings">Optional. Can be null. The XML and object validation settings: <see cref="XmlDeserializationSettings{t}"/></param>
        /// <returns>If successfull, it returns the result of XML and Strongly-Typed Object Validations: <typeparamref name="T"/>, else it throws an <see cref="InvalidOperationException"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="stream"/> is null</exception>
        /// <exception cref="InvalidOperationException">if there is a problem deserializing</exception>
        public static T ReadXmlToAndValidate<T>(this Stream stream, XmlDeserializationSettings<T> settings)
        {
            return TryReadXmlToAndValidate<T>(stream, settings)
                .OperationResultToResultOrException<T>();
        }
        #endregion Forwarders for ReadXmlToAndValidate Deserialization with Validation With Settings

        #region Forwarders for TryReadXmlToAndValidate Deserialization with Validation

        /// <summary>
        /// Generic method to deserialize an XML string <paramref name="serialized"/> to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="serialized">XML string containing the serialized object.</param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object 
        /// of type: <typeparamref name="T"/>, or default value, if <paramref name="serialized"/> was null or empty.</returns>
        public static OperationResult<T> TryParseXmlToAndValidate<T>(this string serialized)
        {
            return TryParseXmlToAndValidate<T>(serialized, null);
        }

        /// <summary>
        /// Generic method to deserialize an XML stored in <paramref name="xmlFilePath"/> to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="xmlFilePath">File path to the file containing the XML of the serialized object.</param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object 
        /// of type: <typeparamref name="T"/>, or default value, if <paramref name="xmlFilePath"/> was null or empty.</returns>
        public static OperationResult<T> TryLoadXmlToAndValidate<T>(this string xmlFilePath)
        {
            return TryLoadXmlToAndValidate<T>(xmlFilePath, null);
        }

        /// <summary>
        /// Generic method deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the XML of the serialized object.</param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object of type: <typeparamref name="T"/>.</returns>
        public static OperationResult<T> TryReadXmlToAndValidate<T>(this XmlReader xmlReader)
        {
            return TryReadXmlToAndValidate<T>(xmlReader, null);
        }

        /// <summary>
        /// Generic method deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="textReader">The <see cref="TextReader"/> containing the XML of the serialized object.</param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object of type: <typeparamref name="T"/>.</returns>
        public static OperationResult<T> TryReadXmlToAndValidate<T>(this TextReader textReader)
        {
            return TryReadXmlToAndValidate<T>(textReader, null);
        }

        /// <summary>
        /// Generic method to deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="stream">The <see cref="Stream"/> containing the XML of the serialized object.</param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object of type: <typeparamref name="T"/>.</returns>
        public static OperationResult<T> TryReadXmlToAndValidate<T>(this Stream stream)
        {
            return TryReadXmlToAndValidate<T>(stream, null);
        }
        #endregion Forwarders for TryReadXmlToAndValidate Deserialization with Validation

        #region TryReadXmlToAndValidate Deserialization with Validation
        /// <summary>
        /// Generic method to optionally validate the XML string <paramref name="serialized"/> (if chosen so via <paramref name="settings"/>), then deserialize the XML string <paramref name="serialized"/>
        /// to an object of the desired generic type: <typeparamref name="T"/> and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="serialized">XML string containing the serialized object.</param>
        /// <param name="settings">Optional. Can be null. The XML and object validation settings: <see cref="XmlDeserializationSettings{t}"/></param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object 
        /// of type: <typeparamref name="T"/>, or default value, if <paramref name="serialized"/> was null or empty.</returns>
        public static OperationResult<T> TryParseXmlToAndValidate<T>(this string serialized, XmlDeserializationSettings<T> settings)
        {
            if (string.IsNullOrWhiteSpace(serialized))
            {
                return OperationResult<T>.CreateArgumentNullFailure(nameof(serialized));
            }

            return TryReadXmlToAndValidateInner(
                xmlValidator => xmlValidator.CreateValidatingXmlReader(serialized, settings?.VirtualXmlFileName),
                settings
                );
        }

        /// <summary>
        /// Generic method to optionally validate the XML stored in <paramref name="xmlFilePath"/> (if chosen so via <paramref name="settings"/>), then deserialize the XML stored in <paramref name="xmlFilePath"/>
        /// to an object of the desired generic type: <typeparamref name="T"/> and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="xmlFilePath">File path to the file containing the XML of the serialized object.</param>
        /// <param name="settings">Optional. Can be null. The XML and object validation settings: <see cref="XmlDeserializationSettings{t}"/></param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object 
        /// of type: <typeparamref name="T"/>.</returns>
        public static OperationResult<T> TryLoadXmlToAndValidate<T>(this string xmlFilePath, XmlDeserializationSettings<T> settings)
        {
            if (string.IsNullOrWhiteSpace(xmlFilePath))
            {
                return OperationResult<T>.CreateArgumentNullFailure(nameof(xmlFilePath));
            }

            return TryReadXmlToAndValidateInner(
                xmlValidator => xmlValidator.CreateValidatingXmlReader(xmlFilePath),
                settings
                );
        }

        /// <summary>
        /// Generic method to optionally validate the XML (if chosen so via <paramref name="settings"/>), then deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the XML of the serialized object.</param>
        /// <param name="settings">Optional. Can be null. The XML and object validation settings: <see cref="XmlDeserializationSettings{t}"/></param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object of type: <typeparamref name="T"/>.</returns>
        public static OperationResult<T> TryReadXmlToAndValidate<T>(this XmlReader xmlReader, XmlDeserializationSettings<T> settings)
        {
            if (xmlReader == null)
            {
                return OperationResult<T>.CreateArgumentNullFailure(nameof(xmlReader));
            }

            return TryReadXmlToAndValidateInner(
                xmlValidator => xmlValidator.CreateValidatingXmlReader(xmlReader, settings?.VirtualXmlFileName),
                settings
                );
        }

        /// <summary>
        /// Generic method to optionally validate the XML (if chosen so via <paramref name="settings"/>), then deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="textReader">The <see cref="TextReader"/> containing the XML of the serialized object.</param>
        /// <param name="settings">Optional. Can be null. The XML and object validation settings: <see cref="XmlDeserializationSettings{t}"/></param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object of type: <typeparamref name="T"/>.</returns>
        public static OperationResult<T> TryReadXmlToAndValidate<T>(this TextReader textReader, XmlDeserializationSettings<T> settings)
        {
            if (textReader == null)
            {
                return OperationResult<T>.CreateArgumentNullFailure(nameof(textReader));
            }

            return TryReadXmlToAndValidateInner(
                xmlValidator => xmlValidator.CreateValidatingXmlReader(textReader, settings?.VirtualXmlFileName),
                settings
                );
        }

        /// <summary>
        /// Generic method to optionally validate the XML (if chosen so via <paramref name="settings"/>), then deserialize the XML to an object of the desired generic type: <typeparamref name="T"/> 
        /// and validate it using the family of <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> if possible,
        /// i.e. if <typeparamref name="T"/> implements one of the interfaces of the <see cref="ISimpleValidatable"/>, or <see cref="ISimpleValidatableAndUpdatable"/> interface family.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <param name="stream">The <see cref="Stream"/> containing the XML of the serialized object.</param>
        /// <param name="settings">Optional. Can be null. The XML and object validation settings: <see cref="XmlDeserializationSettings{t}"/></param>
        /// <returns>The result of XML and Strongly-Typed Object Validations: <see cref="OperationResult{T}"/> containing an error or if successfull - the deserialized and validated object of type: <typeparamref name="T"/>.</returns>
        public static OperationResult<T> TryReadXmlToAndValidate<T>(this Stream stream, XmlDeserializationSettings<T> settings)
        {
            if (stream == null)
            {
                return OperationResult<T>.CreateArgumentNullFailure(nameof(stream));
            }

            return TryReadXmlToAndValidateInner(
                xmlValidator => xmlValidator.CreateValidatingXmlReader(stream, settings?.VirtualXmlFileName),
                settings
                );
        }
        #endregion Deserialization with Validation

        #region Private Methods

        private static OperationResult<T> TryReadXmlToAndValidateInner<T>(Func<XmlValidator, XmlReader> getXmlReader, XmlDeserializationSettings<T> settings)
        {
            T deserialized = default(T);

            XmlValidator xmlValidator = null;

            // Try to call the optional Custom Get or Create XmlValidator function:
            try
            {
                xmlValidator = settings?.GetXmlValidator?.Invoke(settings.XmlValidatorSettings);    // 1st prio XmlValidator creation
            }
            catch (Exception ex)
            {
                return OperationResult<T>.CreateInvalidOperationFailure(
                    $"Custom Get or Create XmlValidator function {nameof(settings.GetXmlValidator)} provided in settings failed.",
                    ex
                    );
            }

            // Validate XML and Desetialize it to object simultaneously:
            Exception catchedEx = null;
            try
            {
                if (xmlValidator == null)
                {
                    xmlValidator = XmlValidator.Create(settings?.XmlValidatorSettings);             // 2nd prio XmlValidator creation
                }

                using XmlReader xmlReader = getXmlReader(xmlValidator);
                XmlSerializer serializer = typeof(T).CreateCorrectXmlSerializer();
                deserialized = (T)serializer.Deserialize(xmlReader);
            }
            catch (IOException ex)
            {
                catchedEx = ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                catchedEx = ex;
            }
            catch (XmlException)
            {
                //NOTE: this is already handled in the xmlValidator.LastValidationResult!
            }
            //catch (WebException ex)  <--> NOTE WebException derives from InvalidOperationException
            catch (InvalidOperationException ex)
            {
                catchedEx = ex;
            }

            // Handle XML Validation and Deserialization outcome:
            if (xmlValidator?.LastValidationResult != null &&
                !xmlValidator.LastValidationResult.IsValid
                )
            {
                return OperationResult<T>.CreateFailure(xmlValidator.LastValidationResult.InvalidReason);
            }
            if (catchedEx != null)
            {
                return OperationResult<T>.CreateFailure(catchedEx);
            }
            if (deserialized == null)
            {
                return OperationResult<T>.CreateInvalidOperationFailure("The deserialized object is null!");
            }

            // -----------------------
            // Valiate created object:
            return InitializeAndValidateDeserialized(settings, deserialized);
        }

        //NOTE: this compiles, but how to call it?!
        //private static OperationResult<T> ValidateDeserialized<T, TV, TVU>(XmlDeserializationSettings<T> settings, T deserialized)
        //    where TV : T, ISimpleValidatable
        //    where TVU : T, ISimpleValidatableAndUpdatable
        //{
        //    // -----------------------------------
        //    // Try to do Strongly-Typed Validation of the created object
        //    switch (deserialized)
        //    {
        //        case TV vali: return vali.TryValidate<TV>(settings?.ObjectValidationSettings) as OperationResult<T>;
        //        case TVU valiAndUpd: return valiAndUpd.TryValidateAndUpdate<TVU>(settings?.ObjectValidationSettings) as OperationResult<T>;
        //        default: return OperationResult<T>.CreateSuccess(deserialized);
        //    }
        //}

        private static OperationResult<T> InitializeAndValidateDeserialized<T>(XmlDeserializationSettings<T> settings, T deserialized)
        {
            // --------------------------
            // Initialize created object:
            try
            {
                // Call the optional custom post-creation pre-validation action:
                settings?.PostCreationPreValidationAction?.Invoke(deserialized);
            }
            catch (Exception ex)
            {
                return OperationResult<T>.CreateInvalidOperationFailure("Custom Post-Creation-Pre-Validation-Action provided in settings failed.", ex);
            }

            // -----------------------------------
            // Try to do Strongly-Typed Validation of the created object

            if (deserialized is ISimpleValidatableAndUpdatable validatableAndUpdatable)
            {
                return validatableAndUpdatable.TryValidateAndUpdate(settings?.ObjectValidationSettings) //NOTE: simple cast is not possible: "as OperationResult<T>;"
                    .CreateGenericResult<T>(deserialized);
            }

            if (deserialized is ISimpleValidatable validatable)
            {
                return validatable.TryValidate(settings?.ObjectValidationSettings) //NOTE: simple cast is not possible: "as OperationResult<T>;"
                    .CreateGenericResult<T>(deserialized);
            }

            //else if (validationOperationResult.Result.IsValid)
            return OperationResult<T>.CreateSuccess(deserialized);
        }

        #endregion TryReadXmlToAndValidate Deserialization with Validation
    }
}
