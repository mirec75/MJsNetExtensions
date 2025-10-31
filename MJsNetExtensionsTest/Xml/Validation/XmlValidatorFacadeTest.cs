namespace MJsNetExtensionsTest.Xml.Validation
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MJsNetExtensions.Xml.Validation;
    using System.Linq;
    using System.IO;
    using System.Xml;
    using System.Net;


    [TestClass]
    public class XmlValidatorFacadeTest
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
        //[ClassInitialize()]
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


        #region Param Tests
        [TestMethod]
        public void XmlValidatorFacadeParamTest1XsdFileMissing_ExpectError()
        {
            // Arrange:
            string xsdFilePath = @"C:\Test\MyDummy.Nonexisting.xsd";            // <-- invalid XSD file path

            string requestXmlStr = "<root />";

            // Act:
            Assert.ThrowsExactly<FileNotFoundException>(() =>
            {
                XmlValidationResult validationResult;
                using (StringReader sr = new StringReader(requestXmlStr))
                    using (XmlReader xmlReader = XmlReader.Create(sr))
                    {
                        validationResult =
                            XmlValidator.Create(new XmlValidatorSettings { XmlDefinitionFilePath = xsdFilePath, })
                                .ValidateOneXml(xmlReader, "blabla.xml");
                    }

                // Log:
                TestContext.WriteLine(validationResult?.ToString());
            });
        }

        [TestMethod]
        public void XmlValidatorFacadeParamTest2XsdUriMissing_ExpectError()
        {
            // Arrange:
            string xsdFilePath = @"https://localhost/MyDummy.Nonexisting.xsd";  // <-- invalid XSD Uri

            string requestXmlStr = "<root />";

            // Act:
            Assert.ThrowsExactly<Exception>(() =>
            {
                XmlValidationResult validationResult;
                using (StringReader sr = new StringReader(requestXmlStr))
                    using (XmlReader xmlReader = XmlReader.Create(sr))
                    {
                        validationResult =
                            XmlValidator.Create(new XmlValidatorSettings { XmlDefinitionFilePath = xsdFilePath, })
                                .ValidateOneXml(xmlReader, "blabla.xml");
                    }

                // Log:
                TestContext.WriteLine(validationResult?.ToString());
            });
        }

        [TestMethod]
        public void XmlValidatorFacadeParamTest3XsdUriNull_ExpectSuccess()
        {
            // Arrange:
            string xsdFilePath = null;  // <-- XSD Uri == null

            string requestXmlStr = "<root />";

            // Act:
            XmlValidationResult validationResult;
            using (StringReader sr = new StringReader(requestXmlStr))
            using (XmlReader xmlReader = XmlReader.Create(sr))
            {
                validationResult =
                    XmlValidator.Create(new XmlValidatorSettings { XmlDefinitionFilePath = xsdFilePath, })
                    .ValidateOneXml(xmlReader, "blabla.xml");
            }

            // Log:
            TestContext.WriteLine(validationResult?.ToString());

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(1, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        public void XmlValidatorFacadeParamTest4XsdUriNull_ExpectSuccess()
        {
            // Arrange:
            string requestXmlStr = "<root />";

            // Act:
            XmlValidationResult validationResult;
            using (StringReader sr = new StringReader(requestXmlStr))
            using (XmlReader xmlReader = XmlReader.Create(sr))
            {
                validationResult =
                    XmlValidator.Create(new XmlValidatorSettings())   // <-- XSD Uri == null
                    .ValidateOneXml(xmlReader, "blabla.xml");
            }

            // Log:
            TestContext.WriteLine(validationResult?.ToString());

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(1, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        public void XmlValidatorFacadeParamTest5XsdUriNull_ExpectSuccess()
        {
            // Arrange:
            string requestXmlStr = "<root />";

            // Act:
            XmlValidationResult validationResult;
            using (StringReader sr = new StringReader(requestXmlStr))
            using (XmlReader xmlReader = XmlReader.Create(sr))
            {
                validationResult =
                    XmlValidator.Create(null)   // <-- XSD Uri == null
                    .ValidateOneXml(xmlReader, "blabla.xml");
            }

            // Log:
            TestContext.WriteLine(validationResult?.ToString());

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(1, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        public void XmlValidatorFacadeParamTest6XsdUriNull_ExpectSuccess()
        {
            // Arrange:
            string requestXmlStr = "<root />";

            // Act:
            XmlValidationResult validationResult;
            using (StringReader sr = new StringReader(requestXmlStr))
            using (XmlReader xmlReader = XmlReader.Create(sr))
            {
                validationResult =
                    XmlValidator.Create()   // <-- XSD Uri == null
                    .ValidateOneXml(xmlReader, "blabla.xml");
            }

            // Log:
            TestContext.WriteLine(validationResult?.ToString());

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(1, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }
        #endregion Param Tests

        #region Tests

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest1XmlFileWithXsdFiles_ExpectSuccess()
        {
            // Arrange:
            string xsdFilePath = @"Test\XSD\Main\DummyMain.xsd";
            string xmlFilePath = @"Test\MyDummy.Valid.xml";

            // Act:
            XmlValidationResult validationResult =
                XmlValidator.Create(new XmlValidatorSettings { XmlDefinitionFilePath = xsdFilePath, })
                .ValidateOneXml(xmlFilePath);

            // Log:
            TestContext.WriteLine(validationResult?.ToString());

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(0, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest2XmlFileWithXsdFiles_ExpectErrors()
        {
            // Arrange:
            string xsdFilePath = @"Test\XSD\Main\DummyMain.xsd";
            string xmlFilePath = @"Test\MyDummy.Invalid.xml";

            // Act:
            XmlValidationResult validationResult =
                XmlValidator.Create(new XmlValidatorSettings { XmlDefinitionFilePath = xsdFilePath, })
                .ValidateOneXml(xmlFilePath);

            // Log:
            TestContext.WriteLine(validationResult?.ToString());

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(8, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(8, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest3XmlReaderWithXsdFiles_ExpectExceptionNotWellFormed()
        {
            // Arrange:
            string requestXmlStr = "<deliveryNoteRequest version=\"1.1\" sendExpiryDates=\"true\" interfaceVersion=\"2\" xmlns=\"\">" +
                "<client number=\"5920018\" password=\"na\" />" +
                "<products>" +
                "<product numberA=\"01\" quantity=\"20\"/>" +
                "<productX number=\"361815\" quantity=\"20\"/>" +
                "<product number=\"98683\" quantityX=\"20\"/>" +
                "</products>" +
                "</productAvailabilityRequest>"
                ;

            string xsdFileName = "productAvailabilityRequest.xsd";

            // Act:
            XmlValidationResult validationResult = ValidateRequestXmlReader(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(2, validationResult.ErrorsCount);
            Assert.AreEqual(validationResult.ErrorsCount, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsTrue(validationResult.WasException);

            // Act:
            validationResult = ValidateRequestXmlString(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(2, validationResult.ErrorsCount);
            Assert.AreEqual(validationResult.ErrorsCount, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsTrue(validationResult.WasException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest4XmlReaderWithXsdFiles_ExpectErrors()
        {
            // Arrange:
            string requestXmlStr = "<deliveryNoteRequest version=\"1.1\" sendExpiryDates=\"true\" interfaceVersion=\"2\" xmlns=\"\">" +
                "<client number=\"5920018\" password=\"na\" />" +
                "<products>" +
                "<product numberA=\"01\" quantity=\"20\"/>" +
                "<productX number=\"361815\" quantity=\"20\"/>" +
                "<product number=\"98683\" quantityX=\"20\"/>" +
                "</products>" +
                "</deliveryNoteRequest>"
                ;

            string xsdFileName = "productAvailabilityRequest.xsd";

            // Act:
            XmlValidationResult validationResult = ValidateRequestXmlReader(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(1, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);

            // Act:
            validationResult = ValidateRequestXmlString(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(1, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest5XmlReaderWithXsdFiles_ExpectErrors()
        {
            // Arrange:
            string requestXmlStr = "<productAvailabilityRequest version=\"1.1\" interfaceVersion=\"2\">" +
            //string requestXmlStr = "<deliveryNoteRequest version=\"1.1\" sendExpiryDates=\"true\" interfaceVersion=\"2\" xmlns=\"\">" +
                "<client number=\"5920018\" password=\"na\" />" +
                "<products>" +
                "<product numberA=\"01\" quantity=\"20\"/>" +
                "<productX number=\"361815\" quantity=\"20\"/>" +
                "<product number=\"98683\" quantityX=\"20\"/>" +
                "</products>" +
                "</productAvailabilityRequest>"
                ;

            string xsdFileName = "productAvailabilityRequest.xsd";

            // Act:
            XmlValidationResult validationResult = ValidateRequestXmlReader(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(2, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(2, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);

            // Act:
            validationResult = ValidateRequestXmlString(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(2, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(2, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest6XmlReaderWithXsdFiles_ExpectSuccess()
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

            string xsdFileName = "customerOrder.xsd";

            // Act:
            XmlValidationResult validationResult = ValidateRequestXmlReader(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(0, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);

            // Act:
            validationResult = ValidateRequestXmlString(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(0, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest7XmlReaderWithXsdFiles_ExpectSuccess()
        {
            // Arrange:
            string requestXmlStr =
                "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "  xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ https://salesweb.customer.com/schemas/POS/customerOrder/customerOrder.xsd\" " +
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

            string xsdFileName = "customerOrder.xsd";

            // Act:
            XmlValidationResult validationResult = ValidateRequestXmlReader(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(0, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);

            // Act:
            validationResult = ValidateRequestXmlString(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(0, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest8XmlReaderWithXsdFiles_ExpectSuccessWithWarning()
        {
            // Arrange:
            string requestXmlStr =
                "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "  xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ https://salesweb.customer.com/schemas/POS/customerOrder/productAvailabilityRequest.xsd\" " +
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

            string xsdFileName = "customerOrder.xsd";

            // Act:
            XmlValidationResult validationResult = ValidateRequestXmlReader(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(1, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);

            // Act:
            validationResult = ValidateRequestXmlString(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(1, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest9XmlReaderWithXsdFiles_ExpectSuccessWithWarning()
        {
            // Arrange:
            string requestXmlStr =
                "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "  xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ http://localhost/schema/Switzerland/productAvailabilityRequest.xsd\" " +
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

            string xsdFileName = "customerOrder.xsd";

            // Act:
            XmlValidationResult validationResult = ValidateRequestXmlReader(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(1, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);

            // Act:
            validationResult = ValidateRequestXmlString(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(1, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest10XmlReaderWithXsdFiles_ExpectSuccessWithWarning()
        {
            // Arrange:
            string requestXmlStr =
                "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "  xsi:schemaLocation=\"http://www.customer.com/schemas/ http://www.customer.com/schemas/customerOrder.xsd\" " +
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

            string xsdFileName = "customerOrder.xsd";

            // Act:
            XmlValidationResult validationResult = ValidateRequestXmlReader(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(0, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);

            // Act:
            validationResult = ValidateRequestXmlString(requestXmlStr, xsdFileName);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(0, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest11XmlReaderWithRuntimeResolvedXsdFiles_ExpectSuccess()
        {
            // Arrange:
            string requestXmlStr =
                "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "  xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ ..\\Out\\Test\\XSD\\customerOrder.xsd\" " +
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

            string xsdFileName = null;

            // Act:
            XmlValidationResult validationResult = ValidateRequestXmlReader(requestXmlStr, xsdFileName, UriTranslationMode.TranslateWebAddressesOnly);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(0, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);

            // Act:
            validationResult = ValidateRequestXmlString(requestXmlStr, xsdFileName, UriTranslationMode.TranslateWebAddressesOnly);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(0, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(0, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        [TestMethod]
        [DeploymentItem(@"Test", @"Test")]
        public void XmlValidatorFacadeTest12XmlReaderWithRuntimeResolvedXsdFiles_ExpectSuccessNoXsdValidationWithWarnings()
        {
            // Arrange:
            string requestXmlStr =
                "<customerOrder xmlns=\"https://salesweb.customer.com/schemas/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "  xsi:schemaLocation=\"https://salesweb.customer.com/schemas/ https://salesweb.customer.com/schemas/POS/customerOrder/customerOrder.xsd\" " +
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

            string xsdFileName = null;

            // Act:
            XmlValidationResult validationResult = ValidateRequestXmlReader(requestXmlStr, xsdFileName, UriTranslationMode.TranslateWebAddressesOnly);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(16, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(16, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);

            // Act:
            validationResult = ValidateRequestXmlString(requestXmlStr, xsdFileName, UriTranslationMode.TranslateWebAddressesOnly);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(16, validationResult.XmlValidationIssues.Count());
            Assert.AreEqual(0, validationResult.ErrorsCount);
            Assert.AreEqual(16, validationResult.WarningsCount);
            Assert.IsFalse(validationResult.WasException);
        }

        #endregion Tests

        #region Helpers

        public XmlValidationResult ValidateRequestXmlReader(string requestXml, string xsdFileName)
        {
            return ValidateRequestXmlReader(requestXml, xsdFileName, UriTranslationMode.ForcedTranslateAllAddresses);
        }
        public XmlValidationResult ValidateRequestXmlReader(string requestXml, string xsdFileName, UriTranslationMode uriTranslationMode)
        {
            string xsdBaseFilePath = @"Test\XSD";
            string xsdFilePath = xsdFileName == null ?
                null :
                Path.Combine(xsdBaseFilePath, xsdFileName);

            string[] xsdFilePaths = xsdFileName == null ? [] : [xsdFilePath];

            // Validate:
            using StringReader sr = new StringReader(requestXml);
            using XmlReader xmlReader = XmlReader.Create(sr);
            XmlValidationResult validationResult =
                XmlValidator.Create(
                        new XmlValidatorSettings
                        {
                            AdditionalXmlDefinitionFilePaths = xsdFilePaths,
                            UriTranslationMode = uriTranslationMode,
                        })
                    .ValidateOneXml(xmlReader, "blabla.xml");

            // Log:
            TestContext.WriteLine(validationResult?.ToString());

            return validationResult;
        }

        public XmlValidationResult ValidateRequestXmlString(string requestXml, string xsdFileName)
        {
            return ValidateRequestXmlString(requestXml, xsdFileName, UriTranslationMode.ForcedTranslateAllAddresses);
        }
        public XmlValidationResult ValidateRequestXmlString(string requestXml, string xsdFileName, UriTranslationMode uriTranslationMode)
        {
            string xsdBaseFilePath = @"Test\XSD";
            string xsdFilePath = xsdFileName == null ?
                null :
                Path.Combine(xsdBaseFilePath, xsdFileName);

            // Validate:
            XmlValidationResult validationResult = 
                XmlValidator.Create(
                    new XmlValidatorSettings
                    {
                        XmlValidationType = XmlValidationType.XSD,
                        XmlDefinitionFilePath = xsdFilePath,
                        UriTranslationMode = uriTranslationMode,
                    })
                .ValidateOneXml(requestXml, null);

            // Log:
            TestContext.WriteLine(validationResult?.ToString());

            return validationResult;
        }

        #endregion Helpers
    }
}
