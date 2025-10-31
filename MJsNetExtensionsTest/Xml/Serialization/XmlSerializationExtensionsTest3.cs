using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJsNetExtensions;
using MJsNetExtensions.Xml.Serialization;
using MJsNetExtensionsTest.Xml.Serialization.TestClasses3;
using MJsNetExtensionsTest.Xml.Validation;

namespace MJsNetExtensionsTest.Xml.Serialization
{
    [TestClass]
    public class XmlSerializationExtensionsTest3
    {
        #region Serialization Direct Tests
        [TestMethod]
        public void DeliveryNotesSerializationWithNoXmlNamespaceAndNoXsdLocationTest_ExpectSuccess()
        {
            // Arrange:
            DummyNotes response = DummyNotes.CreateErrorContent("123", "haha");

            // Act:
            string outputString = response.ToXml();

            // Assert:
            Assert.IsNotNull(outputString);
            //Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<deliveryNotes version=\"1.1\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</deliveryNotes>", outputString);
            Assert.AreEqual("<DummyNotes Version=\"1.1\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyNotes>", outputString);

            // Act:
            DummyNotes parsedResponse = outputString.ParseXmlTo<DummyNotes>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void DeliveryNotesSerializationWithNoXmlNamespaceAndNoXsdLocationTest2_ExpectSuccess()
        {
            // arrange:
            DummyNotes response = DummyNotes.CreateErrorContent("123", "haha");
            response.InterfaceVersionSpecified = true;

            // act:
            string outputString = response.ToXml();

            // assert:
            Assert.IsNotNull(outputString);
            Assert.AreEqual("<DummyNotes Version=\"1.1\" InterfaceVersion=\"7\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyNotes>", outputString);

            // Act:
            DummyNotes parsedResponse = outputString.ParseXmlTo<DummyNotes>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void ProductAvailabilitySerializationWithXmlNamespaceTest_ExpectSuccess()
        {
            // arrange:
            DummyAvailability response = DummyAvailability.CreateErrorContent("123", "haha");

            // Act:
            string outputString = response.ToXml();

            // Assert:
            Assert.IsNotNull(outputString);
            Assert.AreEqual("<DummyAvailability xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ https://salesweb.customer.com/schemas/dummyAvailability.xsd\" Version=\"1.1\" xmlns=\"https://salesweb.customer.com/schemas/\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyAvailability>", outputString);

            // Act:
            DummyAvailability parsedResponse = outputString.ParseXmlTo<DummyAvailability>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void ProductAvailabilitySerializationWithXmlNamespaceTest2_ExpectSuccess()
        {
            // arrange:
            DummyAvailability response = DummyAvailability.CreateErrorContent("123", "haha");
            response.InterfaceVersionSpecified = true;

            // act:
            string outputString = response.ToXml();

            // assert:
            Assert.IsNotNull(outputString);
            Assert.AreEqual("<DummyAvailability xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ https://salesweb.customer.com/schemas/dummyAvailability.xsd\" Version=\"1.1\" InterfaceVersion=\"7\" xmlns=\"https://salesweb.customer.com/schemas/\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyAvailability>", outputString);

            // Act:
            DummyAvailability parsedResponse = outputString.ParseXmlTo<DummyAvailability>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }
        #endregion Serialization Direct Tests

        #region Serialization Try Tests
        [TestMethod]
        public void DeliveryNotesSerializationWithNoXmlNamespaceAndNoXsdLocationTryTest_ExpectSuccess()
        {
            // Arrange:
            DummyNotes response = DummyNotes.CreateErrorContent("123", "haha");

            // Act:
            OperationResult<string> result = response.TryToXml();

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.FailureMessage);
            Assert.IsNull(result.Exception);

            Assert.IsNotNull(result.Result);
            //Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<deliveryNotes version=\"1.1\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</deliveryNotes>", result.Result);
            Assert.AreEqual("<DummyNotes Version=\"1.1\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyNotes>", result.Result);

            // Act:
            DummyNotes parsedResponse = result.Result.ParseXmlTo<DummyNotes>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void DeliveryNotesSerializationWithNoXmlNamespaceAndNoXsdLocationTryTest2_ExpectSuccess()
        {
            // arrange:
            DummyNotes response = DummyNotes.CreateErrorContent("123", "haha");
            response.InterfaceVersionSpecified = true;

            // act:
            OperationResult<string> result = response.TryToXml();

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.FailureMessage);
            Assert.IsNull(result.Exception);

            Assert.IsNotNull(result.Result);
            Assert.AreEqual("<DummyNotes Version=\"1.1\" InterfaceVersion=\"7\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyNotes>", result.Result);

            // Act:
            DummyNotes parsedResponse = result.Result.ParseXmlTo<DummyNotes>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void ProductAvailabilitySerializationWithXmlNamespaceTryTest_ExpectSuccess()
        {
            // arrange:
            DummyAvailability response = DummyAvailability.CreateErrorContent("123", "haha");

            // act:
            OperationResult<string> result = response.TryToXml();

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.FailureMessage);
            Assert.IsNull(result.Exception);

            Assert.IsNotNull(result.Result);
            Assert.AreEqual("<DummyAvailability xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ https://salesweb.customer.com/schemas/dummyAvailability.xsd\" Version=\"1.1\" xmlns=\"https://salesweb.customer.com/schemas/\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyAvailability>", result.Result);

            // Act:
            DummyAvailability parsedResponse = result.Result.ParseXmlTo<DummyAvailability>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void ProductAvailabilitySerializationWithXmlNamespaceTryTest2_ExpectSuccess()
        {
            // arrange:
            DummyAvailability response = DummyAvailability.CreateErrorContent("123", "haha");
            response.InterfaceVersionSpecified = true;

            // act:
            OperationResult<string> result = response.TryToXml();

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.FailureMessage);
            Assert.IsNull(result.Exception);

            Assert.IsNotNull(result.Result);
            Assert.AreEqual("<DummyAvailability xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ https://salesweb.customer.com/schemas/dummyAvailability.xsd\" Version=\"1.1\" InterfaceVersion=\"7\" xmlns=\"https://salesweb.customer.com/schemas/\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyAvailability>", result.Result);

            // Act:
            DummyAvailability parsedResponse = result.Result.ParseXmlTo<DummyAvailability>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void ProductAvailabilitySerializationWithXmlNamespaceTryTest3_ExpectSuccess()
        {
            // arrange:
            DummyAvailability response = DummyAvailability.CreateErrorContent("123", "haha");
            response.InterfaceVersionSpecified = true;

            OperationResult result = null;
            string resultString = null;

            // Act:
            using (var sw = new StringWriter(CultureInfo.InvariantCulture))
            {
                using (var xw = XmlWriter.Create(sw, XmlToStringSerializationSettings.CreateSerializerDefaultXmlWriterSettings()))
                {
                    // Build Xml with xw.
                    result = response.TryToXml(xw);
                }
                resultString = sw.ToString();
            }

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.FailureMessage);
            Assert.IsNull(result.Exception);

            Assert.IsNotNull(resultString);
            Assert.AreEqual("<DummyAvailability xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ https://salesweb.customer.com/schemas/dummyAvailability.xsd\" Version=\"1.1\" InterfaceVersion=\"7\" xmlns=\"https://salesweb.customer.com/schemas/\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyAvailability>", resultString);

            // Act:
            DummyAvailability parsedResponse = resultString.ParseXmlTo<DummyAvailability>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void ProductAvailabilitySerializationWithXmlNamespaceTryTest4_ExpectSuccess()
        {
            // arrange:
            DummyAvailability response = DummyAvailability.CreateErrorContent("123", "haha");
            response.InterfaceVersionSpecified = true;

            OperationResult result = null;
            string resultString = null;

            // Act:
            using (var sw = new StringWriter(CultureInfo.InvariantCulture))
            {
                // Build Xml with xw.
                result = response.TryToXml(sw);

                resultString = sw.ToString();
            }

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.FailureMessage);
            Assert.IsNull(result.Exception);

            Assert.IsNotNull(resultString);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DummyAvailability xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ https://salesweb.customer.com/schemas/dummyAvailability.xsd\" Version=\"1.1\" InterfaceVersion=\"7\" xmlns=\"https://salesweb.customer.com/schemas/\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyAvailability>", resultString);

            // Act:
            DummyAvailability parsedResponse = resultString.ParseXmlTo<DummyAvailability>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void ProductAvailabilitySerializationWithXmlNamespaceTryTest5_ExpectSuccess()
        {
            // arrange:
            DummyAvailability response = DummyAvailability.CreateErrorContent("123", "haha");
            response.InterfaceVersionSpecified = true;

            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            // Act:
            OperationResult result = response.TryToXmlFile(xmlFilePath);
            string resultString = File.ReadAllText(xmlFilePath);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.FailureMessage);
            Assert.IsNull(result.Exception);

            Assert.IsNotNull(resultString);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<DummyAvailability xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ https://salesweb.customer.com/schemas/dummyAvailability.xsd\" Version=\"1.1\" InterfaceVersion=\"7\" xmlns=\"https://salesweb.customer.com/schemas/\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyAvailability>", resultString);

            // Act:
            DummyAvailability parsedResponse = xmlFilePath.LoadXmlTo<DummyAvailability>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void ProductAvailabilitySerializationWithXmlNamespaceTryTest6_ExpectSuccess()
        {
            // arrange:
            DummyAvailability response = DummyAvailability.CreateErrorContent("123", "haha");
            response.InterfaceVersionSpecified = true;

            OperationResult result = null;
            string resultString = null;

            // Act:
            using (var stream = new MemoryStream())
            {
                // Build Xml with xw.
                result = response.TryToXml(stream);

                // Rewind:
                stream.Position = 0;

                //resultString = new StreamReader(stream, Encoding.Unicode).ReadToEnd();  <---> DID NOT WORK!
                using var streamReader = new StreamReader(stream);
                resultString = streamReader.ReadToEnd();
            }


            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.FailureMessage);
            Assert.IsNull(result.Exception);

            Assert.IsNotNull(resultString);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<DummyAvailability xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ https://salesweb.customer.com/schemas/dummyAvailability.xsd\" Version=\"1.1\" InterfaceVersion=\"7\" xmlns=\"https://salesweb.customer.com/schemas/\">\r\n  <ClientError Number=\"123\" Message=\"haha\" />\r\n</DummyAvailability>", resultString);

            // Act:
            DummyAvailability parsedResponse = resultString.ParseXmlTo<DummyAvailability>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }
        #endregion Serialization Try Tests

    }
}
