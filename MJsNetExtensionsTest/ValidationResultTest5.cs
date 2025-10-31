namespace MJsNetExtensionsTest
{
    using MJsNetExtensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MJsNetExtensions.ObjectValidation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public partial class ValidationResultTest
    {
        #region Invalidate if NullOrWhitespace
        [TestMethod]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest1_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrWhiteSpace(null, null));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest2_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrWhiteSpace("", ""));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest3_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrWhiteSpace("", " \r\t\n "));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest4_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrWhiteSpace("zzz", null));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest5_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrWhiteSpace(null, ""));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest6_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrWhiteSpace("zzz", " \r\t\n "));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest7_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            bool checkValue = validationResult.InvalidateIfNullOrWhiteSpace("zzz", "foo");

            // Assert:
            Assert.IsTrue(checkValue);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(null, validationResult.InvalidReason);
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest8_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            bool checkValue1 = validationResult.InvalidateIfNullOrWhiteSpace("zzz", "Test msg1");
            bool checkValue2 = validationResult.InvalidateIfNullOrWhiteSpace("zzz", "tesT msg2");

            // Assert:
            Assert.IsTrue(checkValue1);
            Assert.IsTrue(checkValue2);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(null, validationResult.InvalidReason);
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest9_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string propName = "foo";

            // Act:
            bool checkValue = validationResult.InvalidateIfNullOrWhiteSpace(null, propName);

            // Assert:
            Assert.IsFalse(checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {propName} == null or white space", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIfNullOrWhiteSpace(null, propName);

            // Assert:
            Assert.IsFalse(checkValue);
            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {propName} == null or white space{{Sep}}{propName} == null or white space");
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest10_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string propName = "foo";

            // Act:
            bool checkValue = validationResult.InvalidateIfNullOrWhiteSpace(null, propName);

            // Assert:
            Assert.IsFalse(checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {propName} == null or white space", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIfNullOrWhiteSpace(null, propName);

            // Assert:
            Assert.IsFalse(checkValue);
            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {propName} == null or white space{{Sep}}{propName} == null or white space");
        }
        #endregion Invalidate if NullOrWhitespace
    }
}
