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
    /// Summary description for ValidationResultTest6
    /// </summary>
    public partial class ValidationResultTest
    {
        [TestMethod]
        public void ValidationResultIntegrateOptionalSubResult1_ExpectSuccess()
        {
            // Arrange:
            ValidationResult validationResult = new ValidationResult(this) { };
            string propName = "foo";
            string invalidReason2 = "tesT msg2 ";
            string prop2Name = "bar";

            ValidationResult childInvalidResult = new ValidationResult(this) { };
            childInvalidResult.AddErrorMessage(null, invalidReason2 + "{0}", prop2Name);

            string expectedInvalidReason1 = $"{invalidReason2}{prop2Name}";
            string expectedInvalidSubCompPart = $"Invalid {this.GetType().Name}: {this.GetType().Name}: {expectedInvalidReason1}";

            string dfltSeparator = ValidationResultTest.__defaultSeparator;

            // Act & Assert:
            bool checkValue = validationResult.IntegrateSubResult(null);
            Assert.IsTrue(checkValue);
            Assert.IsTrue(validationResult.IsValid);

            checkValue = validationResult.IntegrateSubResult(new ValidationResult(this));
            Assert.IsTrue(checkValue);
            Assert.IsTrue(validationResult.IsValid);

            checkValue = validationResult.IntegrateSubResult(null);
            Assert.IsTrue(checkValue);
            Assert.IsTrue(validationResult.IsValid);

            checkValue = validationResult.IntegrateSubResult(new ValidationResult(this));
            Assert.IsTrue(checkValue);
            Assert.IsTrue(validationResult.IsValid);


            checkValue = validationResult.IntegrateSubResult(childInvalidResult);
            Assert.IsFalse(checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(expectedInvalidSubCompPart, validationResult.InvalidReason);

            validationResult.CurrentObjectPath = $"/{propName}";
            checkValue = validationResult.IntegrateSubResult(childInvalidResult);
            Assert.IsFalse(checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual($"{expectedInvalidSubCompPart}{dfltSeparator}/{propName}.{this.GetType().Name}: {expectedInvalidReason1}", validationResult.InvalidReason);

            validationResult.CurrentObjectPath = $"/{propName}/haha Hihi";
            checkValue = validationResult.IntegrateSubResult(childInvalidResult);
            Assert.IsFalse(checkValue);
            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(
                $"{expectedInvalidSubCompPart}{dfltSeparator}/{propName}.{this.GetType().Name}: {expectedInvalidReason1}{dfltSeparator}/{propName}/haha Hihi.{this.GetType().Name}: {expectedInvalidReason1}", 
                validationResult.InvalidReason
                );
        }

        //private class ValidValidatable : ISimpleValidatable
        //{
        //    public void PreStructureValidation(ValidationResult validationResult)
        //    {
        //    }
        //}

        //private class InvalidValidatable : ISimpleValidatable
        //{
        //    private const string ErrMsg = "Dummy invalid reason.";
        //    private const string ExpectedPrefix = $"Invalid {nameof(InvalidValidatable)}: ";
        //    public static readonly string ExpectedErrMsg = $"{ExpectedPrefix}{ErrMsg}";

        //    public void PreStructureValidation(ValidationResult validationResult)
        //    {
        //        validationResult?.AddErrorMessage(null, ErrMsg);
        //    }
        //}

    }
}
