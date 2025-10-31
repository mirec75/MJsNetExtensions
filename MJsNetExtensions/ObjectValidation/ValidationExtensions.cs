namespace MJsNetExtensions.ObjectValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MJsNetExtensions.ObjectNavigation;
    using MJsNetExtensions.ObjectNavigation.Internal;

    /// <summary>
    /// Summary description for ValidationExtensions
    /// </summary>
    public static class ValidationExtensions
    {
        #region Throwable Forwarders: Validate and ValidateAndUpdate
        /// <summary>
        /// Validates object subtree (public and private Properties and Fields) of <paramref name="validatable"/> recursively and returns the <see cref="ValidationResult"/>.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatable"/> public and private Properties and Fields of the <paramref name="validatable"/>.
        /// NOTE: This mathod is intended for pure validation of objects, i.e. there are expected NO side effects during the validation of object implementing the <see cref="ISimpleValidatable"/> or derived.
        /// </summary>
        /// <param name="validatable">The validatable object which is about to be validated</param>
        /// <returns>The result of Strongly-Typed Object Validation: <see cref="ValidationResult"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="validatable"/> is null.</exception>
        /// <exception cref="InvalidOperationException">if there was an <see cref="Exception"/> in one of the custom Pre or Post Structure Validation methods of some valiadation particle in the hierarchy.</exception>
        public static ValidationResult Validate([ValidatedNotNull] this ISimpleValidatable validatable)
        {
            return TryValidateAndGetDetails(validatable)
                .OperationResultToResultOrException<ValidationResult>();
        }

        /// <summary>
        /// Validates object subtree (public and private Properties and Fields) of <paramref name="validatable"/> recursively and returns the <see cref="ValidationResult"/>.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatable"/> public and private Properties and Fields of the <paramref name="validatable"/>.
        /// NOTE: This mathod is intended for pure validation of objects, i.e. there are expected NO side effects during the validation of object implementing the <see cref="ISimpleValidatable"/> or derived.
        /// </summary>
        /// <param name="validatable">The validatable object which is about to be validated.</param>
        /// <param name="settings">Optional. Can be null. The object validation settings: <see cref="ValidationSettings"/></param>
        /// <returns>The result of Strongly-Typed Object Validation: <see cref="ValidationResult"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="validatable"/> is null.</exception>
        /// <exception cref="InvalidOperationException">if there was an <see cref="Exception"/> in one of the custom Pre or Post Structure Validation methods of some valiadation particle in the hierarchy.</exception>
        public static ValidationResult Validate([ValidatedNotNull] this ISimpleValidatable validatable, ValidationSettings settings)
        {
            return TryValidateAndGetDetails(validatable, settings)
                .OperationResultToResultOrException<ValidationResult>();
        }

        /// <summary>
        /// Validates and Updates object subtree of <paramref name="validatableAndUpdatable"/> recursively and returns the <see cref="ValidationResult"/>.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatableAndUpdatable"/> and <see cref="ISimpleValidatable"/> public and private Properties and Fields of the <paramref name="validatableAndUpdatable"/>.
        /// NOTE: This method is intended for validation of objects WITH side effects, i.e. intentional internal updates during the validation of object implementing the <see cref="ISimpleValidatableAndUpdatable"/> or derived.
        /// </summary>
        /// <param name="validatableAndUpdatable">The validatable object which is about to be validated and updated</param>
        /// <returns>The result of Strongly-Typed Object Validation: <see cref="ValidationResult"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="validatableAndUpdatable"/> is null.</exception>
        /// <exception cref="InvalidOperationException">if there was an <see cref="Exception"/> in one of the custom Pre or Post Structure Validation methods of some valiadation particle in the hierarchy.</exception>
        public static ValidationResult ValidateAndUpdate([ValidatedNotNull] this ISimpleValidatableAndUpdatable validatableAndUpdatable)
        {
            return TryValidateAndUpdateAndGetDetails(validatableAndUpdatable)
                .OperationResultToResultOrException<ValidationResult>();
        }

        /// <summary>
        /// Validates and Updates object subtree of <paramref name="validatableAndUpdatable"/> recursively and returns the <see cref="ValidationResult"/>.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatableAndUpdatable"/> and <see cref="ISimpleValidatable"/> public and private Properties and Fields of the <paramref name="validatableAndUpdatable"/>.
        /// NOTE: This method is intended for validation of objects WITH side effects, i.e. intentional internal updates during the validation of object implementing the <see cref="ISimpleValidatableAndUpdatable"/> or derived.
        /// </summary>
        /// <param name="validatableAndUpdatable">The validatable object which is about to be validated and updated</param>
        /// <param name="settings">Optional. Can be null. The object validation settings: <see cref="ValidationSettings"/></param>
        /// <returns>The result of Strongly-Typed Object Validation: <see cref="ValidationResult"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="validatableAndUpdatable"/> is null.</exception>
        /// <exception cref="InvalidOperationException">if there was an <see cref="Exception"/> in one of the custom Pre or Post Structure Validation methods of some valiadation particle in the hierarchy.</exception>
        public static ValidationResult ValidateAndUpdate([ValidatedNotNull] this ISimpleValidatableAndUpdatable validatableAndUpdatable, ValidationSettings settings)
        {
            return TryValidateAndUpdateAndGetDetails(validatableAndUpdatable, settings)
                .OperationResultToResultOrException<ValidationResult>();
        }
        #endregion Throwable Forwarders: Validate and ValidateAndUpdate

        #region Forwarders: TryValidate and TryValidateAndUpdate returning: OperationResult<T>
        /// <summary>
        /// Validates object subtree (public and private Properties and Fields) of <paramref name="validatable"/> recursively and returns the <see cref="OperationResult{T}"/> of Strongly-Typed Object Validation.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatable"/> public and private Properties and Fields of the <paramref name="validatable"/>.
        /// NOTE: This mathod is intended for pure validation of objects, i.e. there are expected NO side effects during the validation of object implementing the <see cref="ISimpleValidatable"/> or derived.
        /// </summary>
        /// <param name="validatable">The validatable object which is about to be validated.</param>
        /// <returns>The <see cref="OperationResult{T}"/> of Strongly-Typed Object Validation. If the <paramref name="validatable"/> is not null and validates successfully, then the <see cref="OperationResult.Success"/> is true. 
        /// In all other cases it is false.</returns>
        public static OperationResult<T> TryValidate<T>(this T validatable)
            where T : ISimpleValidatable
        {
            OperationResult<ValidationResult> validationOperationResult = TryValidateAndGetDetails(validatable);

            return validationOperationResult.ToOperationResult<T>(validatable);
        }

        /// <summary>
        /// Validates object subtree (public and private Properties and Fields) of <paramref name="validatable"/> recursively and returns the <see cref="OperationResult{T}"/> of Strongly-Typed Object Validation.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatable"/> public and private Properties and Fields of the <paramref name="validatable"/>.
        /// NOTE: This mathod is intended for pure validation of objects, i.e. there are expected NO side effects during the validation of object implementing the <see cref="ISimpleValidatable"/> or derived.
        /// </summary>
        /// <param name="validatable">The validatable object which is about to be validated.</param>
        /// <param name="settings">Optional. Can be null. The object validation settings: <see cref="ValidationSettings"/></param>
        /// <returns>The <see cref="OperationResult{T}"/> of Strongly-Typed Object Validation. If the <paramref name="validatable"/> is not null and validates successfully, then the <see cref="OperationResult.Success"/> is true. 
        /// In all other cases it is false.</returns>
        public static OperationResult<T> TryValidate<T>(this T validatable, ValidationSettings settings)
            where T : ISimpleValidatable
        {
            OperationResult<ValidationResult> validationOperationResult = TryValidateAndGetDetails(validatable, settings);

            return validationOperationResult.ToOperationResult<T>(validatable);
        }

        /// <summary>
        /// Validates object subtree (public and private Properties and Fields) of <paramref name="validatableAndUpdatable"/> recursively and returns the <see cref="OperationResult{T}"/> of Strongly-Typed Object Validation.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatableAndUpdatable"/> public and private Properties and Fields of the <paramref name="validatableAndUpdatable"/>.
        /// NOTE: This method is intended for validation of objects WITH side effects, i.e. intentional internal updates during the validation of object implementing the <see cref="ISimpleValidatableAndUpdatable"/> or derived.
        /// </summary>
        /// <param name="validatableAndUpdatable">The validatable object which is about to be validated and updated</param>
        /// <returns>The <see cref="OperationResult{T}"/> of Strongly-Typed Object Validation. If the <paramref name="validatableAndUpdatable"/> is not null and validates successfully, then the <see cref="OperationResult.Success"/> is true. 
        /// In all other cases it is false.</returns>
        public static OperationResult<T> TryValidateAndUpdate<T>(this T validatableAndUpdatable)
            where T : ISimpleValidatableAndUpdatable
        {
            OperationResult<ValidationResult> validationOperationResult = TryValidateAndUpdateAndGetDetails(validatableAndUpdatable);

            return validationOperationResult.ToOperationResult<T>(validatableAndUpdatable);
        }

        /// <summary>
        /// Validates object subtree (public and private Properties and Fields) of <paramref name="validatableAndUpdatable"/> recursively and returns the <see cref="OperationResult{T}"/> of Strongly-Typed Object Validation.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatableAndUpdatable"/> public and private Properties and Fields of the <paramref name="validatableAndUpdatable"/>.
        /// NOTE: This method is intended for validation of objects WITH side effects, i.e. intentional internal updates during the validation of object implementing the <see cref="ISimpleValidatableAndUpdatable"/> or derived.
        /// </summary>
        /// <param name="validatableAndUpdatable">The validatable object which is about to be validated and updated</param>
        /// <param name="settings">Optional. Can be null. The object validation settings: <see cref="ValidationSettings"/></param>
        /// <returns>The <see cref="OperationResult{T}"/> of Strongly-Typed Object Validation. If the <paramref name="validatableAndUpdatable"/> is not null and validates successfully, then the <see cref="OperationResult.Success"/> is true. 
        /// In all other cases it is false.</returns>
        public static OperationResult<T> TryValidateAndUpdate<T>(this T validatableAndUpdatable, ValidationSettings settings)
            where T : ISimpleValidatableAndUpdatable
        {
            OperationResult<ValidationResult> validationOperationResult = TryValidateAndUpdateAndGetDetails(validatableAndUpdatable, settings);

            return validationOperationResult.ToOperationResult<T>(validatableAndUpdatable);
        }
        #endregion Forwarders: TryValidate and TryValidateAndUpdate returning: OperationResult<T>

        #region TryValidate and TryValidateAndUpdate returning: OperationResult<ValidationResult>
        /// <summary>
        /// Validates object subtree (public and private Properties and Fields) of <paramref name="validatable"/> recursively and returns the <see cref="ValidationResult"/>.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatable"/> public and private Properties and Fields of the <paramref name="validatable"/>.
        /// NOTE: This mathod is intended for pure validation of objects, i.e. there are expected NO side effects during the validation of object implementing the <see cref="ISimpleValidatable"/> or derived.
        /// </summary>
        /// <param name="validatable">The validatable object which is about to be validated</param>
        /// <returns>The result of Strongly-Typed Object Validation: <see cref="ValidationResult"/> wrapped in <see cref="OperationResult{T}"/>. NOTE that the <see cref="OperationResult.Success"/> is only false, 
        /// if the <paramref name="validatable"/> was null, or there was an <see cref="Exception"/> in one of the custom Pre or Post Structure Validation methods of some valiadation particle in the hierarchy.</returns>
        public static OperationResult<ValidationResult> TryValidateAndGetDetails(this ISimpleValidatable validatable)
        {
            return TryValidateAndGetDetails(validatable, null);
        }

        /// <summary>
        /// Validates object subtree (public and private Properties and Fields) of <paramref name="validatable"/> recursively and returns the <see cref="ValidationResult"/>.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatable"/> public and private Properties and Fields of the <paramref name="validatable"/>.
        /// NOTE: This mathod is intended for pure validation of objects, i.e. there are expected NO side effects during the validation of object implementing the <see cref="ISimpleValidatable"/> or derived.
        /// </summary>
        /// <param name="validatable">The validatable object which is about to be validated.</param>
        /// <param name="settings">Optional. Can be null. The object validation settings: <see cref="ValidationSettings"/></param>
        /// <returns>The result of Strongly-Typed Object Validation: <see cref="ValidationResult"/> wrapped in <see cref="OperationResult{T}"/>. NOTE that the <see cref="OperationResult.Success"/> is only false, 
        /// if the <paramref name="validatable"/> was null, or there was an <see cref="Exception"/> in one of the custom Pre or Post Structure Validation methods of some valiadation particle in the hierarchy.</returns>
        public static OperationResult<ValidationResult> TryValidateAndGetDetails(this ISimpleValidatable validatable, ValidationSettings settings)
        {
            if (validatable == null)
            {
                return OperationResult<ValidationResult>.CreateArgumentNullFailure(nameof(validatable));
            }

            //Throw.IfNull(validatable, nameof(validatable)); --> validated if not null in constructor of: ValidationResult called by constructor of ValidationPreAndPostProcessItemHandler
            ValidationPreAndPostProcessItemHandler<ISimpleValidatable> itemHandler =
                new ValidationPreAndPostProcessItemHandler<ISimpleValidatable>(
                    validatable,
                    (validatableItem, validationResult) => validatableItem.PreStructureValidation(validationResult),
                    (validatableItem, validationResult) =>
                    {
                        IValidatable fullValidatableItem = validatableItem as IValidatable;
                        fullValidatableItem?.PostStructureValidation(validationResult);
                    },
                    settings
                    );

            PreAndPostVisitingTypeHierarchyIterator<ISimpleValidatable>
                .PreAndPostProcessStructure(
                    validatable,
                    settings,
                    itemHandler
                    );

            itemHandler.ValidationResult.CurrentObjectPath = null;

            if (itemHandler.CustomException != null)
            {
                return OperationResult<ValidationResult>.CreateFailure(itemHandler.CustomException);
            }

            return OperationResult<ValidationResult>.CreateSuccess(itemHandler.ValidationResult);
        }

        /// <summary>
        /// Validates and Updates object subtree of <paramref name="validatableAndUpdatable"/> recursively and returns the <see cref="ValidationResult"/>.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatableAndUpdatable"/> public and private Properties and Fields of the <paramref name="validatableAndUpdatable"/>.
        /// NOTE: This method is intended for validation of objects WITH side effects, i.e. intentional internal updates during the validation of object implementing the <see cref="ISimpleValidatableAndUpdatable"/> or derived.
        /// </summary>
        /// <param name="validatableAndUpdatable">The validatable object which is about to be validated and updated</param>
        /// <returns>The result of Strongly-Typed Object Validation: <see cref="ValidationResult"/> wrapped in <see cref="OperationResult{T}"/>. NOTE that the <see cref="OperationResult.Success"/> is only false, 
        /// if the <paramref name="validatableAndUpdatable"/> was null, or there was an <see cref="Exception"/> in one of the custom Pre or Post Structure Validation methods of some valiadation particle in the hierarchy.</returns>
        public static OperationResult<ValidationResult> TryValidateAndUpdateAndGetDetails(this ISimpleValidatableAndUpdatable validatableAndUpdatable)
        {
            return TryValidateAndUpdateAndGetDetails(validatableAndUpdatable, null);
        }

        /// <summary>
        /// Validates and Updates object subtree of <paramref name="validatableAndUpdatable"/> recursively and returns the <see cref="ValidationResult"/>.
        /// The validation automatically recursively iterates all <see cref="ISimpleValidatableAndUpdatable"/> public and private Properties and Fields of the <paramref name="validatableAndUpdatable"/>.
        /// NOTE: This method is intended for validation of objects WITH side effects, i.e. intentional internal updates during the validation of object implementing the <see cref="ISimpleValidatableAndUpdatable"/> or derived.
        /// </summary>
        /// <param name="validatableAndUpdatable">The validatable object which is about to be validated and updated</param>
        /// <param name="settings">Optional. Can be null. The object validation settings: <see cref="ValidationSettings"/></param>
        /// <returns>The result of Strongly-Typed Object Validation: <see cref="ValidationResult"/> wrapped in <see cref="OperationResult{T}"/>. NOTE that the <see cref="OperationResult.Success"/> is only false, 
        /// if the <paramref name="validatableAndUpdatable"/> was null, or there was an <see cref="Exception"/> in one of the custom Pre or Post Structure Validation methods of some valiadation particle in the hierarchy.</returns>
        public static OperationResult<ValidationResult> TryValidateAndUpdateAndGetDetails(this ISimpleValidatableAndUpdatable validatableAndUpdatable, ValidationSettings settings)
        {
            if (validatableAndUpdatable == null)
            {
                return OperationResult<ValidationResult>.CreateArgumentNullFailure(nameof(validatableAndUpdatable));
            }

            //Throw.IfNull(validatable, nameof(validatable)); --> validated if not null in constructor of: ValidationResult called by constructor of ValidationPreAndPostProcessItemHandler
            ValidationPreAndPostProcessItemHandler<ISimpleValidatableAndUpdatable> itemHandler =
                new ValidationPreAndPostProcessItemHandler<ISimpleValidatableAndUpdatable>(
                    validatableAndUpdatable,
                    (validatableItem, validationResult) => validatableItem.PreStructureValidationAndUpdate(validationResult),
                    (validatableAndUpdatableItem, validationResult) =>
                    {
                        IValidatableAndUpdatable fullValidatableAndUpdatableItem = validatableAndUpdatableItem as IValidatableAndUpdatable;
                        fullValidatableAndUpdatableItem?.PostStructureValidationAndUpdate(validationResult);
                    },
                    settings
                    );


            PreAndPostVisitingTypeHierarchyIterator<ISimpleValidatableAndUpdatable>
                .PreAndPostProcessStructure(
                    validatableAndUpdatable,
                    settings,
                    itemHandler
                    );

            itemHandler.ValidationResult.CurrentObjectPath = null;

            if (itemHandler.CustomException != null)
            {
                return OperationResult<ValidationResult>.CreateFailure(itemHandler.CustomException);
            }

            return OperationResult<ValidationResult>.CreateSuccess(itemHandler.ValidationResult);
        }

        #endregion TryValidate and TryValidateAndUpdate returning: OperationResult<ValidationResult>
    }
}
