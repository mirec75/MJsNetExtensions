using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJsNetExtensions;
using MJsNetExtensions.ObjectValidation;
using MJsNetExtensions.Xml.Serialization;
using MJsNetExtensionsTest.Xml.Serialization.TestClasses1;
using MJsNetExtensionsTest.Xml.Serialization.TestClasses2;

namespace MJsNetExtensionsTest.Xml.Validation
{
    [TestClass]
    public class XmlDeserializationExtensionsTest1And2
    {
        #region Positive Cpmplex Serialization & Deserialization Tests -> BUT No Namespaces, Just XML out & XML in
        [TestMethod]
        public void XmlSerializationExtensionsToXmlAndXmlToAndValidateTest1_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest1 cld1 = CommonLogDataTest1.GetNewCommonLogData();

            // Act:
            cld1.ThrowIfNullOrInvalid(nameof(cld1));
            string xmlText = XmlSerializationExtensions.ToXml(cld1);

            CommonLogDataTest1 cld2 = XmlDeserializationExtensions.ParseXmlToAndValidate<CommonLogDataTest1>(xmlText);
            ValidationResult validationResult = cld2.Validate();

            // Assert:
            Assert.IsNotNull(cld2);
            Assert.AreEqual(cld1, cld2); //NOTE: the "public override bool Equals(object obj)" is called

            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void XmlSerializationExtensionsToXmlAndXmlToAndValidateTest2_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest1 cld1 = CommonLogDataTest1.GetNewCommonLogData();

            // Act:
            cld1.ThrowIfNullOrInvalid(nameof(cld1));
            string xmlText = XmlSerializationExtensions.ToXml(cld1);

            CommonLogDataTest1 cld2 = xmlText.ParseXmlToAndValidate<CommonLogDataTest1>();
            cld2.ThrowIfNullOrInvalid(nameof(cld1));

            // Assert:
            Assert.IsNotNull(cld2);
            Assert.AreEqual(cld1, cld2); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void XmlSerializationExtensionsToXmlAndXmlToAndValidateTest3_ExpectSuccess()
        {
            // Arrange:
            CommonExLogDataTest1 cld1 = CommonExLogDataTest1.GetNewCommonExLogData();

            // Act:
            cld1.ThrowIfNullOrInvalid(nameof(cld1));
            string xmlText = XmlSerializationExtensions.ToXml(cld1);

            CommonExLogDataTest1 cld2 = xmlText.ParseXmlToAndValidate<CommonExLogDataTest1>();
            cld2.ThrowIfNullOrInvalid(nameof(cld1));

            // Assert:
            Assert.IsNotNull(cld2);
            Assert.AreEqual(cld1, cld2); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void XmlSerializationExtensionsToXmlAndXmlToAndValidateTest4_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest2 cld1 = CommonLogDataTest2.GetNewCommonLogData();

            // Act:
            cld1.ThrowIfNullOrInvalid(nameof(cld1));
            string xmlText = XmlSerializationExtensions.ToXml(cld1);

            CommonLogDataTest2 cld2 = xmlText.ParseXmlToAndValidate<CommonLogDataTest2>();
            cld2.ThrowIfNullOrInvalid(nameof(cld1));

            // Assert:
            Assert.IsNotNull(cld2);
            Assert.AreEqual(cld1, cld2); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void XmlSerializationExtensionsToXmlAndXmlToAndValidateTest5_ExpectSuccess()
        {
            // Arrange:
            CommonExLogDataTest2 cld1 = CommonExLogDataTest2.GetNewCommonExLogData();

            // Act:
            cld1.ThrowIfNullOrInvalid(nameof(cld1));
            string xmlText = XmlSerializationExtensions.ToXml(cld1);

            CommonExLogDataTest2 cld2 = xmlText.ParseXmlToAndValidate<CommonExLogDataTest2>();
            cld2.ThrowIfNullOrInvalid(nameof(cld1));

            // Assert:
            Assert.IsNotNull(cld2);
            Assert.AreEqual(cld1, cld2); //NOTE: the "public override bool Equals(object obj)" is called
        }
        #endregion Positive Cpmplex Serialization & Deserialization Tests -> BUT No Namespaces, Just XML out & XML in
    }
}
