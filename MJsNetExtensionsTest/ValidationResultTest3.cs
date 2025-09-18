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
        #region Invalidate if Null
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullTest1_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNull(null, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullTest2_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNull(null, "");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullTest3_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNull(null, " \r\t\n ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullTest4_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNull(this, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullTest5_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNull(this, "");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNullTest6_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNull(this, " \r\t\n ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullTest7_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            bool checkValue = validationResult.InvalidateIfNull(this, "foo");

            // Assert:
            Assert.AreEqual(true, checkValue);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(null, validationResult.InvalidReason);
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullTest8_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            bool checkValue1 = validationResult.InvalidateIfNull(this, "Test msg1");
            bool checkValue2 = validationResult.InvalidateIfNull(this, "tesT msg2");

            // Assert:
            Assert.AreEqual(true, checkValue1);
            Assert.AreEqual(true, checkValue2);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(null, validationResult.InvalidReason);
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullTest9_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string propName = "foo";

            // Act:
            bool checkValue = validationResult.InvalidateIfNull(null, propName);

            // Assert:
            Assert.AreEqual(false, checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {propName} == null", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIfNull(null, propName);

            // Assert:
            Assert.AreEqual(false, checkValue);

            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {propName} == null{{sep}}{propName} == null");
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNullTest10_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string propName = "foo";

            // Act:
            bool checkValue = validationResult.InvalidateIfNull(null, propName);

            // Assert:
            Assert.AreEqual(false, checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {propName} == null", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIfNull(null, propName);

            // Assert:
            Assert.AreEqual(false, checkValue);

            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {propName} == null{{sep}}{propName} == null");
        }
        #endregion Invalidate if Null
    }
}
