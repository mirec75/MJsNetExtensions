namespace MJsNetExtensions.ObjectValidation
{
    using System;
    using MJsNetExtensions.ObjectNavigation;

    /// <summary>
    /// Interface for Strongly-Typed Object Validation and Update of validatable classes (or struct) which can be validated using <see cref="ValidationExtensions.ValidateAndUpdate(ISimpleValidatableAndUpdatable)"/> returning <see cref="ValidationResult"/>.
    /// NOTE: This interface is intended for validation of objects WITH side effects, i.e. intentional internal updates during the validation of object implementing this interface.
    /// This interface is already in use in <see cref="Throw.IfNullOrInvalid(ISimpleValidatableAndUpdatable, string)"/>.
    /// </summary>
    public interface ISimpleValidatableAndUpdatable
    {
        /// <summary>
        /// The Pre Structure Validation and Update method, which is called from inside of <see cref="PreAndPostVisitingTypeHierarchyIterator{T}"/> 
        /// through <see cref="ValidationPreAndPostProcessItemHandler{T}"/> before all subcomponents of the validatable object gets validated and updated.
        /// In this phase all the simple properties have to be validated and updated, which are not closely related to complex object subtree of the validatable object.
        /// NOTE: This method is intended for validation of objects WITH optional side effects, i.e. intentional internal updates during the validation of object implementing this interface.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        void PreStructureValidationAndUpdate(ValidationResult validationResult);
    }
}
