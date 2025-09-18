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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest1_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrWhiteSpace(null, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest2_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrWhiteSpace("", "");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest3_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrWhiteSpace("", " \r\t\n ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest4_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrWhiteSpace("zzz", null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest5_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrWhiteSpace(null, "");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest6_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNullOrWhiteSpace("zzz", " \r\t\n ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullOrWhiteSpaceTest7_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            bool checkValue = validationResult.InvalidateIfNullOrWhiteSpace("zzz", "foo");

            // Assert:
            Assert.AreEqual(true, checkValue);
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
            Assert.AreEqual(true, checkValue1);
            Assert.AreEqual(true, checkValue2);
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
            Assert.AreEqual(false, checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {propName} == null or white space", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIfNullOrWhiteSpace(null, propName);

            // Assert:
            Assert.AreEqual(false, checkValue);
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
            Assert.AreEqual(false, checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {propName} == null or white space", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIfNullOrWhiteSpace(null, propName);

            // Assert:
            Assert.AreEqual(false, checkValue);
            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {propName} == null or white space{{Sep}}{propName} == null or white space");
        }
        #endregion Invalidate if NullOrWhitespace
    }
}
