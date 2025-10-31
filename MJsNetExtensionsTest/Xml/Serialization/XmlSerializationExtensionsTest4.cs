using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJsNetExtensions;
using MJsNetExtensions.Xml.Serialization;
using MJsNetExtensionsTest.Xml.Serialization.TestClasses3;
using MJsNetExtensionsTest.Xml.Validation;

namespace MJsNetExtensionsTest.Xml.Serialization
{
    [TestClass]
    public class XmlSerializationExtensionsTest4
    {
        [TestMethod]
        public void DummyDataContainerSerializationWithNoXmlNamespaceAndNoXsdLocationTest1_ExpectSuccess()
        {
            // Arrange:
            DummyDataContainer response = GenerateDummyDataContainerInstance();

            // Act:
            string resultString = response.ToXml();

            // Assert:
            Assert.IsNotNull(resultString);
            Assert.AreEqual("<DummyDataContainer>\r\n  <Branch>12</Branch>\r\n  <UserName>samuraj</UserName>\r\n  <RequestStartTime>2021-05-12T19:09:48.5914009+02:00</RequestStartTime>\r\n  <Port>25</Port>\r\n  <Children>\r\n    <DummySubContainer>\r\n      <CustomerNo>1234</CustomerNo>\r\n      <Country>Deutchland</Country>\r\n    </DummySubContainer>\r\n    <DummySubContainer>\r\n      <CustomerNo>5678</CustomerNo>\r\n      <Country>Österreich</Country>\r\n    </DummySubContainer>\r\n  </Children>\r\n</DummyDataContainer>", resultString);

            // Act:
            DummyDataContainer parsedResponse = resultString.ParseXmlTo<DummyDataContainer>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void DummyDataContainerSerializationWithNoXmlNamespaceAndNoXsdLocationTest1A_ExpectSuccess()
        {
            // Arrange:
            DummyDataContainer response = GenerateDummyDataContainerInstance();
            XmlToStringSerializationSettings settings = new XmlToStringSerializationSettings(); //NOTE: empty settings introduce Namespaces == null

            // Act:
            string resultString = response.ToXml(settings);

            // Assert:
            Assert.IsNotNull(resultString);
            Assert.AreEqual("<DummyDataContainer>\r\n  <Branch>12</Branch>\r\n  <UserName>samuraj</UserName>\r\n  <RequestStartTime>2021-05-12T19:09:48.5914009+02:00</RequestStartTime>\r\n  <Port>25</Port>\r\n  <Children>\r\n    <DummySubContainer>\r\n      <CustomerNo>1234</CustomerNo>\r\n      <Country>Deutchland</Country>\r\n    </DummySubContainer>\r\n    <DummySubContainer>\r\n      <CustomerNo>5678</CustomerNo>\r\n      <Country>Österreich</Country>\r\n    </DummySubContainer>\r\n  </Children>\r\n</DummyDataContainer>", resultString);

            // Act:
            DummyDataContainer parsedResponse = resultString.ParseXmlTo<DummyDataContainer>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void DummyDataContainerSerializationWithNoXmlNamespaceAndNoXsdLocationTest2_ExpectSuccess()
        {
            // Arrange:
            DummyDataContainer response = GenerateDummyDataContainerInstance();

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
            Assert.AreEqual("<DummyDataContainer>\r\n  <Branch>12</Branch>\r\n  <UserName>samuraj</UserName>\r\n  <RequestStartTime>2021-05-12T19:09:48.5914009+02:00</RequestStartTime>\r\n  <Port>25</Port>\r\n  <Children>\r\n    <DummySubContainer>\r\n      <CustomerNo>1234</CustomerNo>\r\n      <Country>Deutchland</Country>\r\n    </DummySubContainer>\r\n    <DummySubContainer>\r\n      <CustomerNo>5678</CustomerNo>\r\n      <Country>Österreich</Country>\r\n    </DummySubContainer>\r\n  </Children>\r\n</DummyDataContainer>", resultString);

            // Act:
            DummyDataContainer parsedResponse = resultString.ParseXmlTo<DummyDataContainer>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void DummyDataContainerSerializationWithNoXmlNamespaceAndNoXsdLocationTest3_ExpectSuccess()
        {
            // Arrange:
            DummyDataContainer response = GenerateDummyDataContainerInstance();

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
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DummyDataContainer>\r\n  <Branch>12</Branch>\r\n  <UserName>samuraj</UserName>\r\n  <RequestStartTime>2021-05-12T19:09:48.5914009+02:00</RequestStartTime>\r\n  <Port>25</Port>\r\n  <Children>\r\n    <DummySubContainer>\r\n      <CustomerNo>1234</CustomerNo>\r\n      <Country>Deutchland</Country>\r\n    </DummySubContainer>\r\n    <DummySubContainer>\r\n      <CustomerNo>5678</CustomerNo>\r\n      <Country>Österreich</Country>\r\n    </DummySubContainer>\r\n  </Children>\r\n</DummyDataContainer>", resultString);

            // Act:
            DummyDataContainer parsedResponse = resultString.ParseXmlTo<DummyDataContainer>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void DummyDataContainerSerializationWithNoXmlNamespaceAndNoXsdLocationTest4_ExpectSuccess()
        {
            // Arrange:
            DummyDataContainer response = GenerateDummyDataContainerInstance();

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
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<DummyDataContainer>\r\n  <Branch>12</Branch>\r\n  <UserName>samuraj</UserName>\r\n  <RequestStartTime>2021-05-12T19:09:48.5914009+02:00</RequestStartTime>\r\n  <Port>25</Port>\r\n  <Children>\r\n    <DummySubContainer>\r\n      <CustomerNo>1234</CustomerNo>\r\n      <Country>Deutchland</Country>\r\n    </DummySubContainer>\r\n    <DummySubContainer>\r\n      <CustomerNo>5678</CustomerNo>\r\n      <Country>Österreich</Country>\r\n    </DummySubContainer>\r\n  </Children>\r\n</DummyDataContainer>", resultString);

            // Act:
            DummyDataContainer parsedResponse = resultString.ParseXmlTo<DummyDataContainer>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void DummyDataContainerSerializationWithNoXmlNamespaceAndNoXsdLocationTest5_ExpectSuccess()
        {
            // Arrange:
            DummyDataContainer response = GenerateDummyDataContainerInstance();

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
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<DummyDataContainer>\r\n  <Branch>12</Branch>\r\n  <UserName>samuraj</UserName>\r\n  <RequestStartTime>2021-05-12T19:09:48.5914009+02:00</RequestStartTime>\r\n  <Port>25</Port>\r\n  <Children>\r\n    <DummySubContainer>\r\n      <CustomerNo>1234</CustomerNo>\r\n      <Country>Deutchland</Country>\r\n    </DummySubContainer>\r\n    <DummySubContainer>\r\n      <CustomerNo>5678</CustomerNo>\r\n      <Country>Österreich</Country>\r\n    </DummySubContainer>\r\n  </Children>\r\n</DummyDataContainer>", resultString);

            // Act:
            DummyDataContainer parsedResponse = xmlFilePath.LoadXmlTo<DummyDataContainer>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        private static DummyDataContainer GenerateDummyDataContainerInstance()
        {
            return new DummyDataContainer
            {
                Branch = 12,
                UserName = "samuraj",
                Port = 25,
                RequestStartTime = DateTime.Parse("2021-05-12T19:09:48.5914009+02:00"),
                Children = new List<DummySubContainer>
                {
                    new()
                    {
                        Country = "Deutchland",
                        CustomerNo = 1234,
                    },
                    new()
                    {
                        Country = "Österreich",
                        CustomerNo = 5678,
                    },
                },
            };
        }
    }
}
