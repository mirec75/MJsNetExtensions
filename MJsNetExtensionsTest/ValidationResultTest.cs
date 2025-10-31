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


    /// <summary>
    /// Summary description for ValidationResultTest
    /// </summary>
    [TestClass]
    public partial class ValidationResultTest
    {
        #region Dummy Test Properties

        public static readonly string __defaultSeparator = Environment.NewLine;

        public string CountryCode { get; set; }
        public string ServiceAbbreviation { get; set; } = "Svc";
        public string UserName { get; set; }

        public DateTime StartProcessing { get; set; } = DateTime.MinValue;

        public int CustomerNo { get; set; } // = 0;


        #endregion Dummy Test Properties


        #region Single Param Constructor Tests
        [TestMethod]
        public void ValidationResultConstructorTest1_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => new ValidationResult(null));
        }

        [TestMethod]
        public void ValidationResultConstructorTest2_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => new ValidationResult((Type)null));
        }

        [TestMethod]
        public void ValidationResultConstructorTest3_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => new ValidationResult((object)null));
        }

        [TestMethod]
        public void ValidationResultConstructorTest4_ExpectSuccess()
        {
            // Arrange:
            // Act:
            ValidationResult validationResult = new ValidationResult(this);

            // Assert:
            Assert.AreEqual($"Invalid {this.GetType().Name}: ", validationResult.InvalidReasonPrefix);
            Assert.IsTrue(validationResult.IsValid);
            Assert.IsNull(validationResult.InvalidReason);
            Assert.IsNotNull(validationResult.InvalidReasons);
            Assert.IsFalse(validationResult.InvalidReasons.Any());
        }

        [TestMethod]
        public void ValidationResultConstructorTest5_ExpectSuccess()
        {
            // Arrange:
            // Act:
            ValidationResult validationResult = new ValidationResult(this.GetType());

            // Assert:
            Assert.AreEqual($"Invalid {this.GetType().Name}: ", validationResult.InvalidReasonPrefix);
            Assert.IsTrue(validationResult.IsValid);
            Assert.IsNull(validationResult.InvalidReason);
            Assert.IsNotNull(validationResult.InvalidReasons);
            Assert.IsFalse(validationResult.InvalidReasons.Any());
        }

        [TestMethod]
        public void ValidationResultConstructorTest6Redirection_ExpectSuccess()
        {
            // Arrange:
            // Act:
            ValidationResult validationResult = new ValidationResult((object)this.GetType());

            // Assert:
            Assert.AreEqual($"Invalid {this.GetType().Name}: ", validationResult.InvalidReasonPrefix);
            Assert.IsTrue(validationResult.IsValid);
            Assert.IsNull(validationResult.InvalidReason);
            Assert.IsNotNull(validationResult.InvalidReasons);
            Assert.IsFalse(validationResult.InvalidReasons.Any());
        }
        #endregion Single Param Constructor Tests

        #region Two Param Constructor Tests
        [TestMethod]
        public void ValidationResultConstructorTest21_ExpectException()
        {
            // Arrange:
            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => new ValidationResult(null));
        }
        #endregion Two Param Constructor Tests

        #region Mixed Test All Good Case

        [TestMethod]
        public void ValidationResultMixedAllTest1_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            bool checkValue1 = validationResult.InvalidateIfNullOrWhiteSpace(this.CountryCode, nameof(this.CountryCode));
            bool checkValue2 = validationResult.InvalidateIfNullOrWhiteSpace(this.ServiceAbbreviation, nameof(this.ServiceAbbreviation));
            bool checkValue3 = validationResult.InvalidateIfNullOrWhiteSpace(this.UserName, nameof(this.UserName));

            bool checkValue4 = validationResult.InvalidateIf(this.StartProcessing == DateTime.MinValue, null, "{0} not provided", nameof(this.StartProcessing));
            bool checkValue5 = validationResult.InvalidateIf(this.CustomerNo < 1, null, "Invalid {0}: {1}", nameof(this.CustomerNo), this.CustomerNo);

            // Assert:
            Assert.IsFalse(checkValue1);
            Assert.IsTrue( checkValue2);
            Assert.IsFalse(checkValue3);
            Assert.IsFalse(checkValue4);
            Assert.IsFalse(checkValue5);

            ValidationResultTest.AssertValidationResultsInvalidReason(
                validationResult, 
                $"Invalid {this.GetType().Name}: {nameof(this.CountryCode)} == null or white space{{Sep}}{nameof(this.UserName)} == null or white space{{Sep}}{nameof(this.StartProcessing)} not provided{{Sep}}Invalid {nameof(this.CustomerNo)}: {this.CustomerNo}" 
                );
        }
        #endregion Mixed Test All Good Case

        #region Invalidate if True
        [TestMethod]
        public void ValidationResultInvalidateIfTest1_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIf(true, null, null));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfTest2_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIf(true, null, ""));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfTest3_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIf(true, null, " \r\t\n "));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfTest4_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIf(false, null, null));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfTest5_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIf(false, null, ""));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfTest6_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => validationResult.InvalidateIf(false, null, " \r\t\n "));
        }

        [TestMethod]
        public void ValidationResultInvalidateIfTest7_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            bool errorCondition = false;

            // Act:
            bool checkValue = validationResult.InvalidateIf(errorCondition, null, "foo");

            // Assert:
            Assert.AreEqual(!errorCondition, checkValue);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(null, validationResult.InvalidReason);
        }

        [TestMethod]
        public void ValidationResultInvalidateIfTest8_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            bool errorCondition = false;

            // Act:
            bool checkValue1 = validationResult.InvalidateIf(errorCondition, null, "Test msg1");
            bool checkValue2 = validationResult.InvalidateIf(errorCondition, null, "tesT msg2");

            // Assert:
            Assert.AreEqual(!errorCondition, checkValue1);
            Assert.AreEqual(!errorCondition, checkValue2);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(null, validationResult.InvalidReason);
        }

        [TestMethod]
        public void ValidationResultInvalidateIfTest9_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            bool errorCondition = true;
            string invalidReason1 = "Test msg1";
            string invalidReason2 = "tesT msg2";

            // Act:
            bool checkValue = validationResult.InvalidateIf(errorCondition, null, invalidReason1);

            // Assert:
            Assert.AreEqual(!errorCondition, checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {invalidReason1}", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIf(errorCondition, null, invalidReason2);

            // Assert:
            Assert.AreEqual(!errorCondition, checkValue);

            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {invalidReason1}{{Sep}}{invalidReason2}");
        }
        #endregion Invalidate if True

        #region Helpers

        public static void AssertValidationResultsInvalidReason(ValidationResult validationResult, string expectedWithSep)
        {
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.IsValid);

            string defaultExpected = expectedWithSep.ReplaceStrings(new Dictionary<string, string> { ["{sep}"] = ValidationResultTest.__defaultSeparator, }, false);

            Assert.AreEqual(defaultExpected, validationResult.InvalidReason);
            Assert.AreEqual(defaultExpected, validationResult.ToString());

            string custSeparator = ", ";
            string custExpected = expectedWithSep.ReplaceStrings(new Dictionary<string, string> { ["{sep}"] = custSeparator, }, false);
            Assert.AreEqual(custExpected, validationResult.ToString(custSeparator));
        }

        #endregion Helpers
    }
}
