using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJsNetExtensions.ObjectNavigation;
using MJsNetExtensions.ObjectValidation;
using MJsNetExtensionsTest.Xml.Serialization.TestClasses1;
using MJsNetExtensionsTest.Xml.Serialization.TestClasses2;

namespace MJsNetExtensionsTest
{
    [TestClass]
    public class ValidationResultTest7
    {
        #region Statics and Consts

        private static readonly ValidationSettings FullSettings = new()
        {
            ListNonpublicProperties = true,
            ListPublicFields = true,
            ListNonpublicFields = true,
            StopOnFirstError = false,
        };

        #endregion Statics and Consts

        #region Positive Cpmplex Validation Tests -> With Recursion Protection, etc.
        [TestMethod]
        public void ValidatableValidationTest1_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest1 cld1 = CommonLogDataTest1.GetNewCommonLogData();
            UpdateCLD1Data4Test(cld1);

            // Act:
            ValidationResult validationResult = cld1.Validate();

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void ValidatableValidationTest2_ExpectSuccess()
        {
            // Arrange:
            CommonExLogDataTest1 cld1 = CommonExLogDataTest1.GetNewCommonExLogData();
            UpdateCLD1Data4Test(cld1);

            // Act:
            ValidationResult validationResult = cld1.Validate();

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void ValidatableValidationTest3_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest2 cld1 = CommonLogDataTest2.GetNewCommonLogData();

            // Act:
            //NOTE: OrdersLogDataTest2 will not get validated, because it implements ISimpleValidatable and NOT: ISimpleValidatableAndUpdatable
            //      UNTIL the PreAndPostVisitingTypeHierarchyIteratorWithAlternative is mada available once again!
            ValidationResult validationResult = cld1.ValidateAndUpdate();

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void ValidatableValidationTest4_ExpectSuccess()
        {
            // Arrange:
            CommonExLogDataTest2 cld1 = CommonExLogDataTest2.GetNewCommonExLogData();

            // Act:
            ValidationResult validationResult = cld1.ValidateAndUpdate();

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void ValidatableValidationTest5FullSettings_ExpectSuccess()
        {
            // Arrange:
            CommonLogDataTest1 cld1 = CommonLogDataTest1.GetNewCommonLogData();
            UpdateCLD1Data4Test(cld1);

            // Act:
            ValidationResult validationResult = cld1.Validate(ValidationResultTest7.FullSettings);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void ValidatableValidationTest6FullSettings_ExpectSuccess()
        {
            // Arrange:
            CommonExLogDataTest1 cld1 = CommonExLogDataTest1.GetNewCommonExLogData();
            UpdateCLD1Data4Test(cld1);

            // Act:
            ValidationResult validationResult = cld1.Validate(ValidationResultTest7.FullSettings);

            // Assert:
            Assert.IsNotNull(validationResult);
            Assert.IsTrue(validationResult.IsValid);
        }

        #endregion Positive Cpmplex Validation Tests -> With Recursion Protection, etc.

        #region Helpers

        private static void UpdateCLD1Data4Test(CommonLogDataTest1 cld1)
        {
            cld1.Details[1].Owner = cld1;
            cld1.Details[1].Siblings = cld1.Details.ToArray();
            cld1.Details[2].Siblings = [];

            cld1.Details[1].SiblingsDict = cld1.Details[1].Siblings?.ToDictionary(it => it.Component);
            cld1.Details[1].ReverseSiblingsDict = cld1.Details[1].Siblings?.ToDictionary(it => it, it => it.Component);

            cld1.Details[2].SiblingsDict = new Dictionary<string, DetailsLogDataTest1>();
            cld1.Details[2].ReverseSiblingsDict = new Dictionary<DetailsLogDataTest1, string>();
        }

        #endregion Helpers

    }
}
