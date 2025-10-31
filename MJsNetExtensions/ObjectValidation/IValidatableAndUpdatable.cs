namespace MJsNetExtensions.ObjectValidation
{
    using System;
    using MJsNetExtensions.ObjectNavigation;

    /// <summary>
    /// Interface for Strongly-Typed Object Validation and Update of validatable classes (or struct) which can be validated using <see cref="ValidationExtensions.ValidateAndUpdate(ISimpleValidatableAndUpdatable)"/> returning <see cref="ValidationResult"/>.
    /// NOTE: This interface is intended for validation of objects WITH side effects, i.e. intentional internal updates during the validation of object implementing this interface.
    /// This interface is already in use in <see cref="Throw.IfNullOrInvalid(ISimpleValidatableAndUpdatable, string)"/>.
    /// </summary>
    public interface IValidatableAndUpdatable : ISimpleValidatableAndUpdatable
    {
        /// <summary>
        /// The Post Structure Validation and Update method, which is called from inside of <see cref="PreAndPostVisitingTypeHierarchyIterator{T}"/> 
        /// through <see cref="ValidationPreAndPostProcessItemHandler{T}"/> after all subcomponents of the validatable object gets validated and updated.
        /// In this phase the complex object subtree is already validated and updated and one can react selectively, based on the intermediate state of Validation <seealso cref="ValidationResult.IsValid"/>
        /// and validate and / or update all the simple properties, which are closely related to complex object subtree of this validatable object.
        /// NOTE: This method is intended for validation of objects WITH optional side effects, i.e. intentional internal updates during the validation of object implementing this interface.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        void PostStructureValidationAndUpdate(ValidationResult validationResult);
    }
}
