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
    using System.Threading;
    using System.Globalization;

    [TestClass]
    public class XmlDeserializationExtensionsXmlStringTest
    {
        #region Properties

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion Properties

        #region Global initialization and cleanup
        // You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}

        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup()
        // {
        // }
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize()
        // {
        // }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #endregion Global initialization and cleanup


        #region ParseXmlTo Tests

        [TestMethod]
        public void XmlDeserializationParseXmlToTest1_ExpectSuccess()
        {
            // Arrange:
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

            // Act:
            customerOrder deserialized = requestXmlStr.ParseXmlTo<customerOrder>();

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
        public void XmlDeserializationParseXmlToTest2_ExpectSuccess()
        {
            // Arrange:
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

            // Act:
            customerOrder deserialized = requestXmlStr.ParseXmlTo<customerOrder>();

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
        public void XmlDeserializationParseXmlToParamTest1_ExpectException()
        {
            // Arrange:
            string requestXmlStr =
                "<productAvailabilityRequest version=\"1.1\" interfaceVersion=\"2\">" +
                //string requestXmlStr = "<deliveryNoteRequest version=\"1.1\" sendExpiryDates=\"true\" interfaceVersion=\"2\" xmlns=\"\">" +
                "<client number=\"5920018\" password=\"na\" />" +
                "<products>" +
                "<product numberA=\"01\" quantity=\"20\"/>" +
                "<productX number=\"361815\" quantity=\"20\"/>" +
                "<product number=\"98683\" quantityX=\"20\"/>" +
                "</products>" +
                "</productAvailabilityRequest>"
                ;

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => requestXmlStr.ParseXmlTo<customerOrder>());
        }

        [TestMethod]
        public void XmlDeserializationParseXmlToParamTest2_ExpectException()
        {
            // Arrange:
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

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => requestXmlStr.ParseXmlTo<customerOrder>());
        }

        [TestMethod]
        public void XmlDeserializationParseXmlToParamTest3_ExpectException()
        {
            // Arrange:
            string requestXmlStr = null;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => requestXmlStr.ParseXmlTo<customerOrder>());
        }

        [TestMethod]
        public void XmlDeserializationParseXmlToParamTest4_ExpectException()
        {
            // Arrange:
            string requestXmlStr = string.Empty;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => requestXmlStr.ParseXmlTo<customerOrder>());
        }

        [TestMethod]
        public void XmlDeserializationParseXmlToParamTest5_ExpectException()
        {
            // Arrange:
            string requestXmlStr = " \r  \t  \n ";

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => requestXmlStr.ParseXmlTo<customerOrder>());
        }
        #endregion ParseXmlTo Tests

        #region TryParseXmlToAndValidate XML string NO Settings
        [TestMethod]
        public void XmlDeserializationTryParseXmlToAndValidateNOSettingsTest1_ExpectOpFailure()
        {
            // Arrange:
            string requestXmlStr = null;

            // Act:
            OperationResult<customerOrder> operationResult = requestXmlStr.TryParseXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNotNull(operationResult.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), operationResult.Exception.GetType());

            // Act:
            //var curCul = Thread.CurrentThread.CurrentCulture;
            //Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            operationResult = requestXmlStr.TryParseXmlToAndValidate<customerOrder>(new XmlDeserializationSettings<customerOrder>());

            //Thread.CurrentThread.CurrentCulture = curCul;
            //Thread.CurrentThread.CurrentUICulture = curCul;

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNotNull(operationResult.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), operationResult.Exception.GetType());

            // Comment out on non-DE systems:
            bool equals =
                string.Equals("ArgumentNullException: Der Wert darf nicht NULL sein. (Parametername 'serialized')", operationResult.FailureMessage, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("ArgumentNullException: Value cannot be null. (Parameter 'serialized')", operationResult.FailureMessage, StringComparison.OrdinalIgnoreCase)
                ;
            Assert.IsTrue(equals);
        }

        [TestMethod]
        public void XmlDeserializationTryParseXmlToAndValidateNOSettingsTest2_ExpectOpFailure()
        {
            // Arrange:
            string requestXmlStr = "";

            // Act:
            OperationResult<customerOrder> operationResult = requestXmlStr.TryParseXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNotNull(operationResult.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), operationResult.Exception.GetType());

            // Act:
            operationResult = requestXmlStr.TryParseXmlToAndValidate<customerOrder>(new XmlDeserializationSettings<customerOrder>());

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNotNull(operationResult.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), operationResult.Exception.GetType());
        }

        [TestMethod]
        public void XmlDeserializationTryParseXmlToAndValidateNOSettingsTest3_ExpectOpFailure()
        {
            // Arrange:
            string requestXmlStr = "  \r\n\t ";

            // Act:
            OperationResult<customerOrder> operationResult = requestXmlStr.TryParseXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNotNull(operationResult.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), operationResult.Exception.GetType());

            // Act:
            operationResult = requestXmlStr.TryParseXmlToAndValidate<customerOrder>(new XmlDeserializationSettings<customerOrder>());

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNotNull(operationResult.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), operationResult.Exception.GetType());
        }

        [TestMethod]
        public void XmlDeserializationTryParseXmlToAndValidateNOSettingsTest4_ExpectOpFailure()
        {
            // Arrange:
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

            // Act:
            OperationResult<customerOrder> operationResult = requestXmlStr.TryParseXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNull(operationResult.Exception);
        }

        [TestMethod]
        public void XmlDeserializationTryParseXmlToAndValidateNOSettingsTest42_ExpectSuccess()
        {
            // Arrange:
            string requestXmlStr =
                "<productAvailabilityRequest version=\"1.1\" interfaceVersion=\"2\">" +
                //string requestXmlStr = "<deliveryNoteRequest version=\"1.1\" sendExpiryDates=\"true\" interfaceVersion=\"2\" xmlns=\"\">" +
                "<client number=\"5920018\" password=\"na\" />" +
                "<products>" +
                "<product numberA=\"01\" quantity=\"20\"/>" +
                "<productX number=\"361815\" quantity=\"20\"/>" +
                "<product number=\"98683\" quantityX=\"20\"/>" +
                "</products>" +
                "</productAvailabilityRequest>"
                ;

            // Act:
            OperationResult<customerOrder> operationResult = requestXmlStr.TryParseXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNotNull(operationResult.Exception);
            Assert.AreEqual(typeof(InvalidOperationException), operationResult.Exception.GetType());
        }

        [TestMethod]
        public void XmlDeserializationTryParseXmlToAndValidateNOSettingsTest5_ExpectSuccess()
        {
            // Arrange:
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

            // Act:
            OperationResult<customerOrder> operationResult = requestXmlStr.TryParseXmlToAndValidate<customerOrder>();

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsTrue(operationResult.Success);
            Assert.IsNull(operationResult.FailureMessage);
            Assert.IsNull(operationResult.Exception);

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
        public void XmlDeserializationTryParseXmlToAndValidateNOSettingsTest6_ExpectSuccess()
        {
            // Arrange:
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

            // Act:
            OperationResult<customerOrder> operationResult = requestXmlStr.TryParseXmlToAndValidate<customerOrder>();

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
        #endregion TryParseXmlToAndValidate XML string NO Settings

        #region TryParseXmlToAndValidate XML string WITH Settings

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlDeserializationTryParseXmlToAndValidateWITHSettingsTest1_ExpectSuccess()
        {
            // Arrange:
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

            string xsdFileName = @"Test\XSD\customerOrder.xsd";

            // Act:
            OperationResult<customerOrder> operationResult =
                requestXmlStr
                .TryParseXmlToAndValidate<customerOrder>(
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
        public void XmlDeserializationTryParseXmlToAndValidateWITHSettingsTest2_ExpectOpFailure()
        {
            // Arrange:
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

            string xsdFileName = @"this-file-Does-Not-Exist.xsd"; 

            // Act:
            OperationResult<customerOrder> operationResult =
                requestXmlStr
                .TryParseXmlToAndValidate<customerOrder>(
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
        public void XmlDeserializationTryParseXmlToAndValidateWITHSettings4_ExpectXmlValidationErrors()
        {
            // Arrange:
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

            string xsdFileName = @"Test\XSD\customerOrder.xsd";

            // Act:
            OperationResult<customerOrder> operationResult =
                requestXmlStr
                .TryParseXmlToAndValidate<customerOrder>(
                    new XmlDeserializationSettings<customerOrder>
                    {
                        XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = xsdFileName, },
                    });

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNull(operationResult.Exception);

            customerOrder deserialized = operationResult.Result;
            Assert.IsNull(deserialized);
            //Expected FailureMessage: "The XML file \"foo.xml\" is invalid according to XSDs: \"C:\\TFS\\Prod\\MJsNetExtensions\\dev\\src\\TestResults\\Deploy_Miroslav.JABLONSKY 2021-04-29 15_51_36\\Out\\Test\\XSD\\customerOrder.xsd\", Errors: 3. See the validation detail messages.\r\nfoo.xml:1:395: Error: xPath: customerOrder/orderLines/productOrderLine/pharmaCode/@haha Message: Das haha-Attribut wurde nicht deklariert.\r\nfoo.xml:1:408: Error: xPath: customerOrder/orderLines/productOrderLine/pharmaCode Message: Das Element darf keinen Text enthalten. Das Inhaltsmodell ist leer.\r\nfoo.xml:1:437: Error: xPath: customerOrder/orderLines/productOrderLine/phantasyElement Message: Das Element 'productOrderLine' in Namespace 'https://salesweb.customer.com/schemas/' hat ein ungültiges untergeordnetes Element 'phantasyElement' in Namespace 'https://salesweb.customer.com/schemas/'."
        }

        [TestMethod]
        public void XmlDeserializationTryParseXmlToAndValidateWITHSettings42_ExpectXmlValidationError()
        {
            // Arrange:
            string requestXmlStr =
                "<productAvailabilityRequest version=\"1.1\" interfaceVersion=\"2\">" +
                //string requestXmlStr = "<deliveryNoteRequest version=\"1.1\" sendExpiryDates=\"true\" interfaceVersion=\"2\" xmlns=\"\">" +
                "<client number=\"5920018\" password=\"na\" />" +
                "<products>" +
                "<product numberA=\"01\" quantity=\"20\"/>" +
                "<productX number=\"361815\" quantity=\"20\"/>" +
                "<product number=\"98683\" quantityX=\"20\"/>" +
                "</products>" +
                "</productAvailabilityRequest>"
                ;

            string xsdFileName = @"Test\XSD\customerOrder.xsd";

            // Act:
            OperationResult<customerOrder> operationResult =
                requestXmlStr
                .TryParseXmlToAndValidate<customerOrder>(
                    new XmlDeserializationSettings<customerOrder>
                    {
                        XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = xsdFileName, },
                    });

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.AreEqual(
                "InvalidOperationException: There is an error in the XML document. --> InvalidOperationException: <productAvailabilityRequest xmlns=''> was not expected.",
                operationResult.FailureMessage);
            Assert.IsNotNull(operationResult.Exception);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlDeserializationTryParseXmlToAndValidateWITHSettings5_ExpectXmlValidationError()
        {
            // Arrange:
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

            string xsdFileName = @"Test\XSD\customerOrder.xsd";

            // Act:
            OperationResult<customerOrder> operationResult =
                requestXmlStr
                .TryParseXmlToAndValidate<customerOrder>(
                    new XmlDeserializationSettings<customerOrder>
                    {
                        XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = xsdFileName, },
                    });

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNull(operationResult.Exception);

            customerOrder deserialized = operationResult.Result;
            Assert.IsNull(deserialized);
            //Expected FailureMessage: "The XML file \"foo.xml\" is invalid according to XSDs: \"C:\\TFS\\Prod\\MJsNetExtensions\\dev\\src\\TestResults\\Deploy_Miroslav.JABLONSKY 2021-04-29 15_46_32\\Out\\Test\\XSD\\customerOrder.xsd\", Exception catched., Errors: 1. See the validation detail messages.\r\nfoo.xml:1:407: Exception: xPath: customerOrder/orderLines/productOrderLine Message: System.Xml.XmlException: 'id' ist ein doppelter Attributname. Zeile 1, Position 407.\r\n   bei System.Xml.XmlTextReaderImpl.Throw(Exception e)\r\n   bei System.Xml.XmlTextReaderImpl.AttributeDuplCheck()\r\n   bei System.Xml.XmlTextReaderImpl.ParseAttributes()\r\n   bei System.Xml.XmlTextReaderImpl.ParseElement()\r\n   bei System.Xml.XmlTextReaderImpl.ParseElementContent()\r\n   bei System.Xml.XmlTextReaderImpl.Read()\r\n   bei System.Xml.XsdValidatingReader.Read()\r\n   bei MJsNetExtensions.Xml.Validation.ValidatingXmlReader.<Read>b__44_0() in C:\\TFS\\Prod\\MJsNetExtensions\\dev\\src\\MJsNetExtensions\\Xml\\Validation\\ValidatingXmlReader.cs:Zeile 129.\r\n   bei MJsNetExtensions.Xml.Validation.ValidatingXmlReader.ProtectingFunc[T](Func`1 functionToProtect) in C:\\TFS\\Prod\\MJsNetExtensions\\dev\\src\\MJsNetExtensions\\Xml\\Validation\\ValidatingXmlReader.cs:Zeile 285."
        }
        #endregion Tests

        #region Thread Local Validator Facade with Deserialization and Validation
        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void ThreadLocalXmlDeserializationTryParseXmlToAndValidateWITHSettingsTest1_ExpectSuccess()
        {
            // Arrange:
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

            string xsdFileName = @"Test\XSD\customerOrder.xsd";

            // Act:
            OperationResult<customerOrder> operationResult = null;

            using (ThreadLocalXmlValidatorFacade tlValidationFacade = ThreadLocalXmlValidatorFacade.Create(new XmlValidatorSettings { XmlDefinitionFilePath = xsdFileName, }))
            {
                operationResult =
                    requestXmlStr
                    .TryParseXmlToAndValidate<customerOrder>(
                        new XmlDeserializationSettings<customerOrder>
                        {
                            GetXmlValidator = xmlValidatorSettings => tlValidationFacade.GetXmlValidator(),
                            XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = "fooBar-NonExisting.XSD", },
                        });
            }

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
        [DeploymentItem(@"Test", @"Test")]
        public void ThreadLocalXmlDeserializationTryParseXmlToAndValidateWITHSettingsTest2_ExpectErrors()
        {
            // Arrange:
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

            // Act:
            OperationResult<customerOrder> operationResult = null;

            using (ThreadLocalXmlValidatorFacade tlValidationFacade = ThreadLocalXmlValidatorFacade.Create(new XmlValidatorSettings { XmlDefinitionFilePath = "fooBar-NonExisting.XSD", }))
            {
                operationResult =
                    requestXmlStr
                    .TryParseXmlToAndValidate<customerOrder>(
                        new XmlDeserializationSettings<customerOrder>
                        {
                            GetXmlValidator = xmlValidatorSettings => tlValidationFacade.GetXmlValidator(),
                            XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = "fooBar-NonExisting.XSD", },
                        });
            }

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNotNull(operationResult.Exception);
            Assert.AreEqual(typeof(InvalidOperationException), operationResult.Exception.GetType());
            Assert.IsNotNull(operationResult.Exception.InnerException);
            Assert.AreEqual(typeof(FileNotFoundException), operationResult.Exception.InnerException.GetType());
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void ThreadLocalXmlDeserializationTryParseXmlToAndValidateWITHSettingsTest3_ExpectErrors()
        {
            // Arrange:
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

            // Act:
            OperationResult<customerOrder> operationResult = null;

            using (ThreadLocalXmlValidatorFacade tlValidationFacade = ThreadLocalXmlValidatorFacade.Create(new XmlValidatorSettings { XmlDefinitionFilePath = "fooBar-NonExisting.XSD", }))
            {
                operationResult =
                    requestXmlStr
                    .TryParseXmlToAndValidate<customerOrder>(
                        new XmlDeserializationSettings<customerOrder>
                        {
                            GetXmlValidator = xmlValidatorSettings => null,
                            XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = "fooBar-NonExisting.XSD", },
                        });
            }

            // Assert:
            Assert.IsNotNull(operationResult);
            Assert.IsFalse(operationResult.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResult.FailureMessage));
            Assert.IsNotNull(operationResult.Exception);
            Assert.AreEqual(typeof(FileNotFoundException), operationResult.Exception.GetType());
            Assert.IsNull(operationResult.Exception.InnerException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void ThreadLocalXmlDeserializationTryParseXmlToAndValidateWITHSettingsTest4_ExpectSuccess()
        {
            // Arrange:
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

            string xsdFileName = @"Test\XSD\customerOrder.xsd";

            // Act:
            OperationResult<customerOrder> operationResult = null;

            using (ThreadLocalXmlValidatorFacade tlValidationFacade = ThreadLocalXmlValidatorFacade.Create(new XmlValidatorSettings { XmlDefinitionFilePath = "fooBar-NonExisting.XSD", }))
            {
                operationResult =
                    requestXmlStr
                    .TryParseXmlToAndValidate<customerOrder>(
                        new XmlDeserializationSettings<customerOrder>
                        {
                            GetXmlValidator = xmlValidatorSettings => null, //NOTE: because of returning NULL here, the FileNotFoundException is never thrown in ThreadLocalXmlValidatorFacade
                            XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = xsdFileName, },  // The XmlValidator will be created based on this settings
                        });
            }

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
        [DeploymentItem(@"Test", @"Test")]
        public void ThreadLocalXmlDeserializationTryParseXmlToAndValidateWITHSettingsTest5_ExpectSuccess()
        {
            // Arrange:
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

            string xsdFileName = @"Test\XSD\customerOrder.xsd";

            // Act:
            OperationResult<customerOrder> operationResult = 
                requestXmlStr
                .TryParseXmlToAndValidate<customerOrder>(
                    new XmlDeserializationSettings<customerOrder>
                    {
                        GetXmlValidator = xmlValidatorSettings => XmlValidator.Create(xmlValidatorSettings), //NOTE: because of returning NULL here, the FileNotFoundException is never thrown in ThreadLocalXmlValidatorFacade
                        XmlValidatorSettings = new XmlValidatorSettings { XmlDefinitionFilePath = xsdFileName, },  // The XmlValidator will be created based on this settings
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
        #endregion Thread Local Validator Facade with Deserialization and Validation
    }
}
