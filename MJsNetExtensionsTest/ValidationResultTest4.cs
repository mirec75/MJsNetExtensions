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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrEmptyTest1_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrEmpty(null, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrEmptyTest2_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrEmpty("", "");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrEmptyTest3_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrEmpty("", " \r\t\n ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrEmptyTest4_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrEmpty("zzz", null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrEmptyTest5_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrEmpty(null, "");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrEmptyTest6_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrEmpty("zzz", " \r\t\n ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrEmptyTest7_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            bool checkValue = validationResult.InvalidateIfNullOrEmpty("zzz", "foo");

            // Assert:
            Assert.AreEqual(true, checkValue);
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
            Assert.AreEqual(true, checkValue1);
            Assert.AreEqual(true, checkValue2);
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
            Assert.AreEqual(false, checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {propName} == null or empty", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIfNullOrEmpty(null, propName);

            // Assert:
            Assert.AreEqual(false, checkValue);

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
            Assert.AreEqual(false, checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {propName} == null or empty", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIfNullOrEmpty(null, propName);

            // Assert:
            Assert.AreEqual(false, checkValue);

            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {propName} == null or empty{{sep}}{propName} == null or empty");
        }
        #endregion Invalidate if NullOrEmpty
    }
}
