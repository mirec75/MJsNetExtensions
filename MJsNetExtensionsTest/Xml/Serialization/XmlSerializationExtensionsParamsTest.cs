using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJsNetExtensions;
using MJsNetExtensions.Xml.Serialization;
using MJsNetExtensionsTest.Xml.Serialization.TestClasses1;
using MJsNetExtensionsTest.Xml.Serialization.TestClasses3;

namespace MJsNetExtensionsTest.Xml.Serialization
{
    [TestClass]
    public class XmlSerializationExtensionsParamsTest
    {
        #region XmlTo Parameter Tests
        [TestMethod]
        public void XmlToNullParam_ExpectException()
        {
            // Arrange:
            string serializedObjXml = null;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => serializedObjXml.ParseXmlTo<DummyNotes>());
        }

        [TestMethod]
        public void XmlToEmptyParam_ExpectException()
        {
            // Arrange:
            string serializedObjXml = "";

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => serializedObjXml.ParseXmlTo<DummyNotes>());
        }

        [TestMethod]
        public void XmlToWhiteSpaceParam_ExpectException()
        {
            // Arrange:
            string serializedObjXml = " \t \r \n ";

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => serializedObjXml.ParseXmlTo<DummyNotes>());
        }

        [TestMethod]
        public void XmlToJustTextParam_ExpectSuccess()
        {
            // Arrange:
            string serializedObjXml = " hahah ";

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => serializedObjXml.ParseXmlTo<DummyNotes>());
        }


        [TestMethod]
        public void XmlToWrongXml1Param_ExpectSuccess()
        {
            // Arrange:
            string serializedObjXml = " <hahah ";

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => serializedObjXml.ParseXmlTo<DummyNotes>());
        }

        [TestMethod]
        public void XmlToWrongXml2Param_ExpectException()
        {
            // Arrange:
            string serializedObjXml = " <hahah> ";

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => serializedObjXml.ParseXmlTo<DummyNotes>());
        }

        [TestMethod]
        public void XmlToWrongXml3Param_ExpectException()
        {
            // Arrange:
            string serializedObjXml = " <hahah> uff! </hahax>";

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => serializedObjXml.ParseXmlTo<DummyNotes>());
        }

        [TestMethod]
        public void XmlToValdidWrongXml4Param_ExpectException()
        {
            // Arrange:
            string serializedObjXml = " <hahah> uff! </hahah>";

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => serializedObjXml.ParseXmlTo<DummyNotes>());
        }

        [TestMethod]
        public void XmlToValdidWrongXml5Param_ExpectSuccess()
        {
            // Arrange:
            string serializedObjXml = "<DummyNotes Version=\"1.1\" InterfaceVersion=\"7\">\r\n  <hahah> uff! </hahah> \r\n</DummyNotes>";

            // Act:
            DummyNotes parsedResponse = serializedObjXml.ParseXmlTo<DummyNotes>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
        }

        [TestMethod]
        public void XmlToInvaldidWrongXml6Param_ExpectException()
        {
            // Arrange:
            string serializedObjXml = "<DummyNotes Version=\"1.1\" InterfaceVersion=\"7\">\r\n  <hahah> uff! </hahax> \r\n</DummyNotes>";

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => serializedObjXml.ParseXmlTo<DummyNotes>());
        }
        #endregion XmlTo Parameter Tests

        #region ToXml Parameter Tests
        [TestMethod]
        public void ToXmlXSREBNullParam1_ExpectSuccess()
        {
            // Arrange:
            DummyNotes response = null;

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => response.ToXml());
        }

        [TestMethod]
        public void ToXmlXSREBNullParam2_ExpectSuccess()
        {
            // Arrange:
            DummyNotes response = null;

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => response.ToXml((XmlToStringSerializationSettings)null));
        }

        [TestMethod]
        public void ToXmlXSREBNullParam3_ExpectSuccess()
        {
            // Arrange:
            DummyNotes response = null;

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => response.ToXml((XmlToStringSerializationSettings)null));
        }

        [TestMethod]
        public void ToXmlObjNullParam1_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest1 response = null;

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => response.ToXml());
        }

        [TestMethod]
        public void ToXmlObjNullParam3_ExpectException()
        {
            // Arrange:
            CommonLogDataTest1 response = null;

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => response.ToXml((XmlToStringSerializationSettings)null));
        }

        [TestMethod]
        public void ToXmlObjNullParam4_ExpectException()
        {
            // Arrange:
            CommonLogDataTest1 response = null;

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => response.ToXml((XmlWriter)null));
        }

        [TestMethod]
        public void ToXmlObjNullParam5_ExpectException()
        {
            // Arrange:
            CommonLogDataTest1 response = null;

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => response.ToXml((TextWriter)null));
        }

        [TestMethod]
        public void ToXmlObjNullParam6_ExpectException()
        {
            // Arrange:
            CommonLogDataTest1 response = null;

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => response.ToXml((Stream)null));
        }
        #endregion ToXml Parameter Tests

        #region TryToXml Parameter Tests
        [TestMethod]
        public void TryToXmlXSREBNullParam1_ExpectSuccess()
        {
            // Arrange:
            DummyNotes response = null;

            var curCul = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            // Act:
            OperationResult<string> result = XmlSerializationExtensions.TryToXml(response);

            Thread.CurrentThread.CurrentCulture = curCul;
            Thread.CurrentThread.CurrentUICulture = curCul;

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.FailureMessage));
            Assert.IsNotNull(result.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), result.Exception.GetType());

            // Comment out on non-DE systems:
            bool equals =
                string.Equals("ArgumentNullException: Der Wert darf nicht NULL sein. (Parameter 'toSerialize')", result.FailureMessage, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("ArgumentNullException: Value cannot be null. (Parameter 'toSerialize')", result.FailureMessage, StringComparison.OrdinalIgnoreCase)
                ;
            Assert.IsTrue(equals);
        }

        [TestMethod]
        public void TryToXmlXSREBNullParam2_ExpectSuccess()
        {
            // Arrange:
            DummyNotes response = null;

            // Act:
            OperationResult<string> result = XmlSerializationExtensions.TryToXml(response, (XmlToStringSerializationSettings)null);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.FailureMessage));
            Assert.IsNotNull(result.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), result.Exception.GetType());
        }


        [TestMethod]
        public void TryToXmlXSREBNullParam3_ExpectSuccess()
        {
            // Arrange:
            DummyNotes response = null;

            // Act:
            OperationResult<string> result = XmlSerializationExtensions.TryToXml(response, (XmlToStringSerializationSettings)null);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.FailureMessage));
            Assert.IsNotNull(result.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), result.Exception.GetType());
        }

        [TestMethod]
        public void TryToXmlObjNullParam1_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest1 response = null;

            // Act:
            OperationResult<string> result = XmlSerializationExtensions.TryToXml(response);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.FailureMessage));
            Assert.IsNotNull(result.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), result.Exception.GetType());
        }

        [TestMethod]
        public void TryToXmlObjNullParam3_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest1 response = null;

            // Act:
            OperationResult<string> result = XmlSerializationExtensions.TryToXml(response, (XmlToStringSerializationSettings)null);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.FailureMessage));
            Assert.IsNotNull(result.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), result.Exception.GetType());
        }
        #endregion TryToXml Parameter Tests
    }
}
