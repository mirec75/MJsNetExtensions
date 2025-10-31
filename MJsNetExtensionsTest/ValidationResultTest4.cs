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
        #region Invalidate if NullOrEmpty
        [TestMethod]
        public void ValidationResultInvalidateIfNullOrEmptyTest1_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrEmpty(null, null));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrEmptyTest2_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrEmpty("", ""));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrEmptyTest3_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrEmpty("", " \r\t\n "));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrEmptyTest4_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrEmpty("zzz", null));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrEmptyTest5_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrEmpty(null, ""));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrEmptyTest6_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIfNullOrEmpty("zzz", " \r\t\n "));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrEmptyTest7_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            bool checkValue = validationResult.InvalidateIfNullOrEmpty("zzz", "foo");

            // Assert:
            Assert.IsTrue(checkValue);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(null, validationResult.InvalidReason);
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrEmptyTest8_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            bool checkValue1 = validationResult.InvalidateIfNullOrEmpty("zzz", "Test msg1");
            bool checkValue2 = validationResult.InvalidateIfNullOrEmpty("zzz", "tesT msg2");

            // Assert:
            Assert.IsTrue(checkValue1);
            Assert.IsTrue(checkValue2);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(null, validationResult.InvalidReason);
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrEmptyTest9_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string propName = "foo";

            // Act:
            bool checkValue = validationResult.InvalidateIfNullOrEmpty(null, propName);

            // Assert:
            Assert.IsFalse(checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {propName} == null or empty", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIfNullOrEmpty(null, propName);

            // Assert:
            Assert.IsFalse(checkValue);

            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {propName} == null or empty{{sep}}{propName} == null or empty");
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrEmptyTest10_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string propName = "foo";

            // Act:
            bool checkValue = validationResult.InvalidateIfNullOrEmpty(null, propName);

            // Assert:
            Assert.IsFalse(checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {propName} == null or empty", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIfNullOrEmpty(null, propName);

            // Assert:
            Assert.IsFalse(checkValue);

            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {propName} == null or empty{{sep}}{propName} == null or empty");
        }
        #endregion Invalidate if NullOrEmpty
    }
}
