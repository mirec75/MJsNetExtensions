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
    /// Summary description for ValidationResultTest2
    /// </summary>
    public partial class ValidationResultTest
    {
        #region Add Error Message
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultAddErrorMessageTest1_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.AddErrorMessage(null, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultAddErrorMessageTest2_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.AddErrorMessage(null, "");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultAddErrorMessageTest3_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.AddErrorMessage(null, " \r\t\n ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        public void ValidationResultAddErrorMessageTest3_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string invalidReason1 = "Test msg1";
            string invalidReason2 = "tesT msg2";

            // Act:
            validationResult.AddErrorMessage(null, invalidReason1);
            validationResult.AddErrorMessage(null, invalidReason2);

            // Assert:
            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {invalidReason1}{{Sep}}{invalidReason2}");
        }

        [TestMethod]
        public void ValidationResultAddErrorMessageTest4_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string invalidReason1 = "Test msg1 ";
            string invalidReason2 = "tesT msg2 ";

            // Act:
            validationResult.AddErrorMessage(null, invalidReason1 + "{0}", null);
            validationResult.AddErrorMessage(null, invalidReason2 + "{0}", null);

            // Assert:
            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {invalidReason1}{{Sep}}{invalidReason2}");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidationResultAddErrorMessageTest42_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string invalidReason1 = "Test msg1 ";

            // Act:
            validationResult.AddErrorMessage(null, invalidReason1 + "{0}");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        public void ValidationResultAddErrorMessageTest5_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string invalidReason1 = "Test msg1 ";
            string invalidReason2 = "tesT msg2 ";

            // Act:
            validationResult.AddErrorMessage(null, invalidReason1 + "{0}", (object) null);
            validationResult.AddErrorMessage(null, invalidReason2 + "{0}", (string) null);

            // Assert:
            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {invalidReason1}{{Sep}}{invalidReason2}");
        }

        [TestMethod]
        public void ValidationResultAddErrorMessageTest6_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string invalidReason1 = "Test msg1 ";
            string invalidReason2 = "tesT msg2 ";
            string param1 = "foo";
            int param2 = 7;

            // Act:
            validationResult.AddErrorMessage(null, invalidReason1 + "{0}: {1}", param1, param2);
            validationResult.AddErrorMessage(null, invalidReason2 + "{0}: {1}", param1, param2);

            // Assert:
            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {invalidReason1}{param1}: {param2}{{Sep}}{invalidReason2}{param1}: {param2}");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidationResultAddErrorMessageTest7_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string invalidReason1 = "foo {12} ha!";

            // Act:
            validationResult.AddErrorMessage(null, invalidReason1);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
            //Assert.IsFalse(validationResult.IsValid);
            //Assert.AreEqual($"Invalid {this.GetType().Name}: {invalidReason1}", validationResult.InvalidReason);
        }

        [TestMethod]
        public void ValidationResultAddErrorMessageTest72_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            string invalidReason1 = "foo {12} ha!";

            // Act:
            validationResult.AddErrorMessage(null, invalidReason1.EscapeForFurtherFormatting());

            // Assert:
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {invalidReason1}", validationResult.InvalidReason);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidationResultAddErrorMessageTest8_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.AddErrorMessage(null, "foo {12} ha!", null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidationResultAddErrorMessageTest9_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.AddErrorMessage(null, "foo {12} ha!", null, 2, 3, 4, "barr");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        #endregion Add Error Message

        #region Invalidate if False
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNotTest1_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNot(false, null, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNotTest2_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNot(false, null, "");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNotTest3_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNot(false, null, " \r\t\n ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNotTest4_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNot(true, null, null);

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNotTest5_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNot(true, null, "");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationResultInvalidateIfNotTest6_ExpectException()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);

            // Act:
            validationResult.InvalidateIfNot(true, null, " \r\t\n ");

            // Assert:
            Assert.Fail("An exception was expected, but did not occur!");
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNotTest7_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            bool goodCondition = true;

            // Act:
            bool checkValue = validationResult.InvalidateIfNot(goodCondition, null, "foo");

            // Assert:
            Assert.AreEqual(goodCondition, checkValue);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(null, validationResult.InvalidReason);
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNotTest8_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            bool goodCondition = true;

            // Act:
            bool checkValue1 = validationResult.InvalidateIfNot(goodCondition, null, "Test msg1");
            bool checkValue2 = validationResult.InvalidateIfNot(goodCondition, null, "tesT msg2");

            // Assert:
            Assert.AreEqual(goodCondition, checkValue1);
            Assert.AreEqual(goodCondition, checkValue2);
            Assert.IsTrue(validationResult.IsValid);
            Assert.AreEqual(null, validationResult.InvalidReason);
        }

        [TestMethod]
        public void ValidationResultInvalidateIfNotTest9_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this);
            bool goodCondition = false;
            string invalidReason1 = "Test msg1";
            string invalidReason2 = "tesT msg2";

            // Act:
            bool checkValue = validationResult.InvalidateIfNot(goodCondition, null, invalidReason1);

            // Assert:
            Assert.AreEqual(goodCondition, checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"Invalid {this.GetType().Name}: {invalidReason1}", validationResult.InvalidReason);

            // Act:
            checkValue = validationResult.InvalidateIfNot(goodCondition, null, invalidReason2);

            // Assert:
            Assert.AreEqual(goodCondition, checkValue);
            ValidationResultTest.AssertValidationResultsInvalidReason(validationResult, $"Invalid {this.GetType().Name}: {invalidReason1}{{Sep}}{invalidReason2}");
        }
        #endregion Invalidate if False
    }
}
