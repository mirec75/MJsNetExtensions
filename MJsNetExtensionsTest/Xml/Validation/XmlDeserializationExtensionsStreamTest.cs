#pragma warning disable S125
namespace MJsNetExtensionsTest.Xml.Validation
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MJsNetExtensions.Xml.Validation;
    using System.Linq;
    using System.IO;
    using System.Xml;
    using System.Net;
    using Customer.Types.Schema.Samples.customerOrder;
    using MJsNetExtensions.Xml.Serialization;
    using MJsNetExtensions;
    using System.Reflection;

    [TestClass]
    public class XmlDeserializationExtensionsStreamTest
    {
        #region Properties

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion Properties

        #region LoadXmlTo Tests

        [TestMethod]
        public void XmlDeserializationLoadXmlToTest1_ExpectSuccess()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr =
                  "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                  "  version=\"1.0\" productDescriptionDesired=\"true\" compressionDesired=\"true\" language=\"de\">" +
                  "  <client number=\"9999011\" password=\"Yehewa23\"/>  " +
                  "  <orderHeader referenceNumber=\"SWCl\"/>  " +
                  "  <orderLines>" +
                  "    <productOrderLine orderQuantity=\"1\">" +
                  "      <pharmaCode id=\"0000655\"/>" +
                  "    </productOrderLine>" +
                  "  </orderLines>" +
                  "</customerOrder>"
                  ;
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            // Act:
            customerOrder deserialized = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                deserialized = xmlReader.XmlTo<customerOrder>();

            // Assert:
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(1, deserialized.version);
            Assert.AreEqual(1, deserialized.interfaceVersion);
            Assert.IsTrue(deserialized.productDescriptionDesired);
            Assert.IsTrue(deserialized.compressionDesired);
            Assert.IsTrue(deserialized.languageSpecified);
            Assert.AreEqual(languageType.de, deserialized.language);

            Assert.IsNotNull(deserialized.client);
            Assert.AreEqual("9999011", deserialized.client.number);
            Assert.AreEqual("Yehewa23", deserialized.client.password);

            Assert.IsNotNull(deserialized.orderHeader);
            Assert.AreEqual("SWCl", deserialized.orderHeader.referenceNumber);
            Assert.IsNull(deserialized.orderHeader.orderDetails);
            Assert.IsFalse(deserialized.orderHeader.phoneCallDesiredSpecified);

            Assert.IsNotNull(deserialized.orderLines);
            Assert.AreEqual(1, deserialized.orderLines.Length);
            Assert.AreEqual(1, deserialized.orderLines.OfType<productOrderLine>().Count());

            var orderLine1 = (productOrderLine)deserialized.orderLines[0];
            Assert.IsNotNull(orderLine1);
            Assert.AreEqual("1", orderLine1.orderQuantity);
            Assert.IsNotNull(orderLine1.Item as pharmaCode);
            Assert.AreEqual("0000655", (orderLine1.Item as pharmaCode).id);
        }

        [TestMethod]
        public void XmlDeserializationLoadXmlToTest2_ExpectSuccess()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr =
                    "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "  version=\"1.0\" productDescriptionDesired=\"true\" compressionDesired=\"true\" language=\"de\">" +
                    "  <client number=\"9999011\" password=\"Yehewa23\"/>  " +
                    "  <orderHeader referenceNumber=\"SWCl\"/>  " +
                    "  <orderLines>" +
                    "    <XXproductOrderLine orderQuantity=\"1\">" +
                    "      <pharmaCode id=\"0000655\"/>" +
                    "    </XXproductOrderLine>" +
                    "  </orderLines>" +
                    "</customerOrder>"
                    ;
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            // Act:
            customerOrder deserialized = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                deserialized = xmlReader.XmlTo<customerOrder>();

            // Assert:
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(1, deserialized.version);
            Assert.AreEqual(1, deserialized.interfaceVersion);
            Assert.IsTrue(deserialized.productDescriptionDesired);
            Assert.IsTrue(deserialized.compressionDesired);
            Assert.IsTrue(deserialized.languageSpecified);
            Assert.AreEqual(languageType.de, deserialized.language);

            Assert.IsNotNull(deserialized.client);
            Assert.AreEqual("9999011", deserialized.client.number);
            Assert.AreEqual("Yehewa23", deserialized.client.password);

            Assert.IsNotNull(deserialized.orderHeader);
            Assert.AreEqual("SWCl", deserialized.orderHeader.referenceNumber);
            Assert.IsNull(deserialized.orderHeader.orderDetails);
            Assert.IsFalse(deserialized.orderHeader.phoneCallDesiredSpecified);

            Assert.IsNotNull(deserialized.orderLines);
            Assert.AreEqual(0, deserialized.orderLines.Length);
        }

        [TestMethod]
        public void XmlDeserializationLoadXmlToParamTest1_ExpectException()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr =
                    "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "  version=\"1.0\" productDescriptionDesired=\"true\" compressionDesired=\"true\" language=\"de\">" +
                    "  <client number=\"9999011\" password=\"Yehewa23\"/>  " +
                    "  <orderHeader referenceNumber=\"SWCl\"/>  " +
                    "  <orderLines>" +
                    "    <productOrderLine orderQuantity=\"1\">" +
                    " <<<   <pharmaCode id=\"0000655\"/>" +
                    "    </productOrderLine>" +
                    "  </orderLines>" +
                    "</customerOrder>"
                    ;
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            // Act:
            customerOrder deserialized = null;
            using var xmlReader = XmlReader.Create(xmlFilePath);
            Assert.ThrowsExactly<InvalidOperationException>(() => deserialized = xmlReader.XmlTo<customerOrder>());
        }

        [TestMethod]
        public void XmlDeserializationLoadXmlToParamTest3_ExpectException()
        {
            // Arrange:

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => ((Stream)null).XmlTo<customerOrder>());
        }

        #endregion LoadXmlTo Tests

        #region TryLoadXmlToAndValidate XML string NO Settings
        [TestMethod]
        public void XmlDeserializationTryLoadXmlToAndValidateNOSettingsTest1_ExpectOpFailure()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr = null;
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            // Act:
            OperationResult<customerOrder> operationResult = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                operationResult = xmlReader.TryReadXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));

            // Act:
            operationResult = xmlFilePath.TryLoadXmlToAndValidate<customerOrder>(new XmlDeserializationSettings<customerOrder>());

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
        }

        [TestMethod]
        public void XmlDeserializationTryLoadXmlToAndValidateNOSettingsTest2_ExpectOpFailure()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr = "";
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            // Act:
            OperationResult<customerOrder> operationResult = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                operationResult = xmlReader.TryReadXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));

            // Act:
            operationResult = xmlFilePath.TryLoadXmlToAndValidate<customerOrder>(new XmlDeserializationSettings<customerOrder>());

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
        }

        [TestMethod]
        public void XmlDeserializationTryLoadXmlToAndValidateNOSettingsTest3_ExpectOpFailure()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr = "  \r\n\t ";
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            // Act:
            OperationResult<customerOrder> operationResult = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                operationResult = xmlReader.TryReadXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));

            // Act:
            operationResult = xmlFilePath.TryLoadXmlToAndValidate<customerOrder>(new XmlDeserializationSettings<customerOrder>());

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
        }

        [TestMethod]
        public void XmlDeserializationTryLoadXmlToAndValidateNOSettingsTest4_ExpectOpFailure()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr =
                    "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "  version=\"1.0\" productDescriptionDesired=\"true\" compressionDesired=\"true\" language=\"de\">" +
                    "  <client number=\"9999011\" password=\"Yehewa23\"/>  " +
                    "  <orderHeader referenceNumber=\"SWCl\"/>  " +
                    "  <orderLines>" +
                    "    <productOrderLine orderQuantity=\"1\">" +
                    " <<<   <pharmaCode id=\"0000655\"/>" +
                    "    </productOrderLine>" +
                    "  </orderLines>" +
                    "</customerOrder>"
                    ;
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            // Act:
            OperationResult<customerOrder> operationResult = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                operationResult = xmlReader.TryReadXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNull(operationResult.Exception);
        }

        [TestMethod]
        public void XmlDeserializationTryLoadXmlToAndValidateNOSettingsTest5_ExpectSuccess()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr =
                    "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "  version=\"1.0\" productDescriptionDesired=\"true\" compressionDesired=\"true\" language=\"de\">" +
                    "  <client number=\"9999011\" password=\"Yehewa23\"/>  " +
                    "  <orderHeader referenceNumber=\"SWCl\"/>  " +
                    "  <orderLines>" +
                    "    <XXproductOrderLine orderQuantity=\"1\">" +
                    "      <pharmaCode id=\"0000655\"/>" +
                    "    </XXproductOrderLine>" +
                    "  </orderLines>" +
                    "</customerOrder>"
                    ;
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            // Act:
            OperationResult<customerOrder> operationResult = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                operationResult = xmlReader.TryReadXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsTrue(operationResult.Success);

            customerOrder deserialized = operationResult.Result;
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(1, deserialized.version);
            Assert.AreEqual(1, deserialized.interfaceVersion);
            Assert.IsTrue(deserialized.productDescriptionDesired);
            Assert.IsTrue(deserialized.compressionDesired);
            Assert.IsTrue(deserialized.languageSpecified);
            Assert.AreEqual(languageType.de, deserialized.language);

            Assert.IsNotNull(deserialized.client);
            Assert.AreEqual("9999011", deserialized.client.number);
            Assert.AreEqual("Yehewa23", deserialized.client.password);

            Assert.IsNotNull(deserialized.orderHeader);
            Assert.AreEqual("SWCl", deserialized.orderHeader.referenceNumber);
            Assert.IsNull(deserialized.orderHeader.orderDetails);
            Assert.IsFalse(deserialized.orderHeader.phoneCallDesiredSpecified);

            Assert.IsNotNull(deserialized.orderLines);
            Assert.AreEqual(0, deserialized.orderLines.Length);
        }

        [TestMethod]
        public void XmlDeserializationTryLoadXmlToAndValidateNOSettingsTest6_ExpectSuccess()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr =
                    "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "  version=\"1.0\" productDescriptionDesired=\"true\" compressionDesired=\"true\" language=\"de\">" +
                    "  <client number=\"9999011\" password=\"Yehewa23\"/>  " +
                    "  <orderHeader referenceNumber=\"SWCl\"/>  " +
                    "  <orderLines>" +
                    "    <productOrderLine orderQuantity=\"1\">" +
                    "      <pharmaCode id=\"0000655\"/>" +
                    "    </productOrderLine>" +
                    "  </orderLines>" +
                    "</customerOrder>"
                    ;
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            // Act:
            OperationResult<customerOrder> operationResult = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                operationResult = xmlReader.TryReadXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsTrue(operationResult.Success);

            customerOrder deserialized = operationResult.Result;
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(1, deserialized.version);
            Assert.AreEqual(1, deserialized.interfaceVersion);
            Assert.IsTrue(deserialized.productDescriptionDesired);
            Assert.IsTrue(deserialized.compressionDesired);
            Assert.IsTrue(deserialized.languageSpecified);
            Assert.AreEqual(languageType.de, deserialized.language);

            Assert.IsNotNull(deserialized.client);
            Assert.AreEqual("9999011", deserialized.client.number);
            Assert.AreEqual("Yehewa23", deserialized.client.password);

            Assert.IsNotNull(deserialized.orderHeader);
            Assert.AreEqual("SWCl", deserialized.orderHeader.referenceNumber);
            Assert.IsNull(deserialized.orderHeader.orderDetails);
            Assert.IsFalse(deserialized.orderHeader.phoneCallDesiredSpecified);

            Assert.IsNotNull(deserialized.orderLines);
            Assert.AreEqual(1, deserialized.orderLines.Length);
            Assert.AreEqual(1, deserialized.orderLines.OfType<productOrderLine>().Count());

            var orderLine1 = (productOrderLine)deserialized.orderLines[0];
            Assert.IsNotNull(orderLine1);
            Assert.AreEqual("1", orderLine1.orderQuantity);
            Assert.IsNotNull(orderLine1.Item as pharmaCode);
            Assert.AreEqual("0000655", (orderLine1.Item as pharmaCode).id);
        }
        #endregion TryLoadXmlToAndValidate XML string NO Settings

        #region TryLoadXmlToAndValidate XML string WITH Settings

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlDeserializationTryLoadXmlToAndValidateWITHSettingsTest1_ExpectSuccess()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr =
                    "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "  version=\"1.0\" productDescriptionDesired=\"true\" compressionDesired=\"true\" language=\"de\">" +
                    "  <client number=\"9999011\" password=\"Yehewa23\"/>  " +
                    "  <orderHeader referenceNumber=\"SWCl\"/>  " +
                    "  <orderLines>" +
                    "    <productOrderLine orderQuantity=\"1\">" +
                    "      <pharmaCode id=\"0000655\"/>" +
                    "    </productOrderLine>" +
                    "  </orderLines>" +
                    "</customerOrder>"
                    ;
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            string xsdFileName = @"Test\XSD\customerOrder.xsd";

            // Act:
            OperationResult<customerOrder> operationResult = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                operationResult = xmlReader.TryReadXmlToAndValidate<customerOrder>(
                    new XmlDeserializationSettings<customerOrder>
                    {
                        XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = xsdFileName, },
                    });

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsTrue(operationResult.Success);

            customerOrder deserialized = operationResult.Result;
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(1, deserialized.version);
            Assert.AreEqual(1, deserialized.interfaceVersion);
            Assert.IsTrue(deserialized.productDescriptionDesired);
            Assert.IsTrue(deserialized.compressionDesired);
            Assert.IsTrue(deserialized.languageSpecified);
            Assert.AreEqual(languageType.de, deserialized.language);

            Assert.IsNotNull(deserialized.client);
            Assert.AreEqual("9999011", deserialized.client.number);
            Assert.AreEqual("Yehewa23", deserialized.client.password);

            Assert.IsNotNull(deserialized.orderHeader);
            Assert.AreEqual("SWCl", deserialized.orderHeader.referenceNumber);
            Assert.IsNull(deserialized.orderHeader.orderDetails);
            Assert.IsFalse(deserialized.orderHeader.phoneCallDesiredSpecified);

            Assert.IsNotNull(deserialized.orderLines);
            Assert.AreEqual(1, deserialized.orderLines.Length);
            Assert.AreEqual(1, deserialized.orderLines.OfType<productOrderLine>().Count());

            var orderLine1 = (productOrderLine)deserialized.orderLines[0];
            Assert.IsNotNull(orderLine1);
            Assert.AreEqual("1", orderLine1.orderQuantity);
            Assert.IsNotNull(orderLine1.Item as pharmaCode);
            Assert.AreEqual("0000655", (orderLine1.Item as pharmaCode).id);
        }


        [TestMethod]
        public void XmlDeserializationTryLoadXmlToAndValidateWITHSettingsTest2_ExpectOpFailure()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr =
                    "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "  version=\"1.0\" productDescriptionDesired=\"true\" compressionDesired=\"true\" language=\"de\">" +
                    "  <client number=\"9999011\" password=\"Yehewa23\"/>  " +
                    "  <orderHeader referenceNumber=\"SWCl\"/>  " +
                    "  <orderLines>" +
                    "    <productOrderLine orderQuantity=\"1\">" +
                    "      <pharmaCode id=\"0000655\"/>" +
                    "    </productOrderLine>" +
                    "  </orderLines>" +
                    "</customerOrder>"
                    ;
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            string xsdFileName = @"this-file-Does-Not-Exist.xsd";

            // Act:
            OperationResult<customerOrder> operationResult = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                operationResult = xmlReader.TryReadXmlToAndValidate<customerOrder>(
                    new XmlDeserializationSettings<customerOrder>
                    {
                        XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = xsdFileName, },
                    });

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNotNull(operationResult.Exception);
            Assert.AreEqual(typeof(FileNotFoundException), operationResult.Exception.GetType());
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlDeserializationTryLoadXmlToAndValidateWITHSettings4_ExpectXmlValidationErrors()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr =
                    "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "  version=\"1.0\" productDescriptionDesired=\"true\" compressionDesired=\"true\" language=\"de\">" +
                    "  <client number=\"9999011\" password=\"Yehewa23\"/>  " +
                    "  <orderHeader referenceNumber=\"SWCl\"/>  " +
                    "  <orderLines>" +
                    "    <productOrderLine orderQuantity=\"1\">" +
                    "      <pharmaCode id=\"0000655\" haha=\"Uff!\" > tralala </pharmaCode>" +
                    "      <phantasyElement haha=\"Uff!\" />" +
                    "    </productOrderLine>" +
                    "  </orderLines>" +
                    "</customerOrder>"
                    ;
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            string xsdFileName = @"Test\XSD\customerOrder.xsd";

            // Act:
            OperationResult<customerOrder> operationResult = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                operationResult = xmlReader.TryReadXmlToAndValidate<customerOrder>(
                    new XmlDeserializationSettings<customerOrder>
                    {
                        XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = xsdFileName, },
                    });

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);

            customerOrder deserialized = operationResult.Result;
            Assert.IsNull(deserialized);
            //Expected FailureMessage: "The XML file \"foo.xml\" is invalid according to XSDs: \"C:\\TFS\\Prod\\MJsNetExtensions\\dev\\src\\TestResults\\Deploy_Miroslav.JABLONSKY 2021-04-29 15_51_36\\Out\\Test\\XSD\\customerOrder.xsd\", Errors: 3. See the validation detail messages.\r\nfoo.xml:1:395: Error: xPath: customerOrder/orderLines/productOrderLine/pharmaCode/@haha Message: Das haha-Attribut wurde nicht deklariert.\r\nfoo.xml:1:408: Error: xPath: customerOrder/orderLines/productOrderLine/pharmaCode Message: Das Element darf keinen Text enthalten. Das Inhaltsmodell ist leer.\r\nfoo.xml:1:437: Error: xPath: customerOrder/orderLines/productOrderLine/phantasyElement Message: Das Element 'productOrderLine' in Namespace 'https://salesweb.customer.com/schemas/' hat ein ungültiges untergeordnetes Element 'phantasyElement' in Namespace 'https://salesweb.customer.com/schemas/'."
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlDeserializationTryLoadXmlToAndValidateWITHSettings5_ExpectXmlValidationException()
        {
            // Arrange:
            string xmlFilePath = Path.Combine(CommonStatics.__programDirectory, "foobar.xml");

            if (true)
            {
                string requestXmlStr =
                    "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "  version=\"1.0\" productDescriptionDesired=\"true\" compressionDesired=\"true\" language=\"de\">" +
                    "  <client number=\"9999011\" password=\"Yehewa23\"/>  " +
                    "  <orderHeader referenceNumber=\"SWCl\"/>  " +
                    "  <orderLines>" +
                    "    <productOrderLine orderQuantity=\"1\">" +
                    "      <pharmaCode id=\"0000655\" haha=\"Uff!\" id=\"0000655\"> tralala </pharmaCode>" +
                    "      <phantasyElement haha=\"Uff!\" />" +
                    "    </productOrderLine>" +
                    "  </orderLines>" +
                    "</customerOrder>"
                    ;
                File.WriteAllText(xmlFilePath, requestXmlStr);
            }

            string xsdFileName = @"Test\XSD\customerOrder.xsd";

            // Act:
            OperationResult<customerOrder> operationResult = null;
            using (var xmlReader = XmlReader.Create(xmlFilePath))
                operationResult = xmlReader.TryReadXmlToAndValidate<customerOrder>(
                    new XmlDeserializationSettings<customerOrder>
                    {
                        XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = xsdFileName, },
                    });

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);

            customerOrder deserialized = operationResult.Result;
            Assert.IsNull(deserialized);
            //Expected FailureMessage: "The XML file \"foo.xml\" is invalid according to XSDs: \"C:\\TFS\\Prod\\MJsNetExtensions\\dev\\src\\TestResults\\Deploy_Miroslav.JABLONSKY 2021-04-29 15_46_32\\Out\\Test\\XSD\\customerOrder.xsd\", Exception catched., Errors: 1. See the validation detail messages.\r\nfoo.xml:1:407: Exception: xPath: customerOrder/orderLines/productOrderLine Message: System.Xml.XmlException: 'id' ist ein doppelter Attributname. Zeile 1, Position 407.\r\n   bei System.Xml.XmlTextReaderImpl.Throw(Exception e)\r\n   bei System.Xml.XmlTextReaderImpl.AttributeDuplCheck()\r\n   bei System.Xml.XmlTextReaderImpl.ParseAttributes()\r\n   bei System.Xml.XmlTextReaderImpl.ParseElement()\r\n   bei System.Xml.XmlTextReaderImpl.ParseElementContent()\r\n   bei System.Xml.XmlTextReaderImpl.Read()\r\n   bei System.Xml.XsdValidatingReader.Read()\r\n   bei MJsNetExtensions.Xml.Validation.ValidatingXmlReader.<Read>b__44_0() in C:\\TFS\\Prod\\MJsNetExtensions\\dev\\src\\MJsNetExtensions\\Xml\\Validation\\ValidatingXmlReader.cs:Zeile 129.\r\n   bei MJsNetExtensions.Xml.Validation.ValidatingXmlReader.ProtectingFunc[T](Func`1 functionToProtect) in C:\\TFS\\Prod\\MJsNetExtensions\\dev\\src\\MJsNetExtensions\\Xml\\Validation\\ValidatingXmlReader.cs:Zeile 285."
        }
        #endregion Tests
    }
}
