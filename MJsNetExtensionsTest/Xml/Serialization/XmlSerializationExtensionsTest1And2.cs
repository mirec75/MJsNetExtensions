using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJsNetExtensionsTest.Xml.Serialization.TestClasses1;
using MJsNetExtensions.Xml.Serialization;
using MJsNetExtensionsTest.Xml.Serialization.TestClasses2;
using MJsNetExtensions.ObjectValidation;
using MJsNetExtensions;

namespace MJsNetExtensionsTest.Xml.Serialization
{
    [TestClass]
    public class XmlSerializationExtensionsTest1And2
    {
        #region Positive Cpmplex Serialization & Deserialization Tests -> BUT No Namespaces, Just XML out & XML in
        [TestMethod]
        public void XmlSerializationExtensionsToXmlAndXmlToTest1_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest1 cld1 = CommonLogDataTest1.GetNewCommonLogData();

            // Act:
            cld1.ThrowIfNullOrInvalid(nameof(cld1));
            string xmlText = XmlSerializationExtensions.ToXml(cld1);

            CommonLogDataTest1 cld2 = XmlDeserializationExtensions.ParseXmlTo<CommonLogDataTest1>(xmlText);
            ValidationResult validationResult = cld2.Validate();

            // Assert:
            Assert.IsNotNull(cld2);
            Assert.AreEqual(cld1, cld2); //NOTE: the "public override bool Equals(object obj)" is called

            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void XmlSerializationExtensionsToXmlAndXmlToTest2_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest1 cld1 = CommonLogDataTest1.GetNewCommonLogData();

            // Act:
            cld1.ThrowIfNullOrInvalid(nameof(cld1));
            string xmlText = XmlSerializationExtensions.ToXml(cld1);

            CommonLogDataTest1 cld2 = xmlText.ParseXmlTo<CommonLogDataTest1>();
            cld2.ThrowIfNullOrInvalid(nameof(cld1));

            // Assert:
            Assert.IsNotNull(cld2);
            Assert.AreEqual(cld1, cld2); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void XmlSerializationExtensionsToXmlAndXmlToTest3_ExpectSuccess()
        {
            // Arrange:
            CommonExLogDataTest1 cld1 = CommonExLogDataTest1.GetNewCommonExLogData();

            // Act:
            cld1.ThrowIfNullOrInvalid(nameof(cld1));
            string xmlText = XmlSerializationExtensions.ToXml(cld1);

            CommonExLogDataTest1 cld2 = xmlText.ParseXmlTo<CommonExLogDataTest1>();
            cld2.ThrowIfNullOrInvalid(nameof(cld1));

            // Assert:
            Assert.IsNotNull(cld2);
            Assert.AreEqual(cld1, cld2); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void XmlSerializationExtensionsToXmlAndXmlToTest4_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest2 cld1 = CommonLogDataTest2.GetNewCommonLogData();

            // Act:
            cld1.ThrowIfNullOrInvalid(nameof(cld1));
            string xmlText = XmlSerializationExtensions.ToXml(cld1);

            CommonLogDataTest2 cld2 = xmlText.ParseXmlTo<CommonLogDataTest2>();
            cld2.ThrowIfNullOrInvalid(nameof(cld1));

            // Assert:
            Assert.IsNotNull(cld2);
            Assert.AreEqual(cld1, cld2); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void XmlSerializationExtensionsToXmlAndXmlToTest5_ExpectSuccess()
        {
            // Arrange:
            CommonExLogDataTest2 cld1 = CommonExLogDataTest2.GetNewCommonExLogData();

            // Act:
            cld1.ThrowIfNullOrInvalid(nameof(cld1));
            string xmlText = XmlSerializationExtensions.ToXml(cld1);

            CommonExLogDataTest2 cld2 = xmlText.ParseXmlTo<CommonExLogDataTest2>();
            cld2.ThrowIfNullOrInvalid(nameof(cld1));

            // Assert:
            Assert.IsNotNull(cld2);
            Assert.AreEqual(cld1, cld2); //NOTE: the "public override bool Equals(object obj)" is called
        }
        #endregion Positive Cpmplex Serialization & Deserialization Tests -> BUT No Namespaces, Just XML out & XML in
    }
}
