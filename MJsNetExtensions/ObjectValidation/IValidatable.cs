namespace MJsNetExtensions.ObjectValidation
{
    using System;
    using MJsNetExtensions.ObjectNavigation;

    /// <summary>
    /// Interface for Strongly-Typed Object Validation of validatable classes (or struct) which can be validated using <see cref="ValidationExtensions.Validate(ISimpleValidatable)"/> returning <see cref="ValidationResult"/>.
    /// NOTE: This interface is intended for pure validation of objects, i.e. there sould be NO side effects during the validation of object implementing this interface.
    /// This interface is already in use in <see cref="Throw.IfNullOrInvalid(ISimpleValidatable, string)"/>.
    /// </summary>
    public interface IValidatable : ISimpleValidatable
    {
        /// <summary>
        /// The Post Structure validation method, which is called from inside of <see cref="PreAndPostVisitingTypeHierarchyIterator{T}"/> 
        /// through <see cref="ValidationPreAndPostProcessItemHandler{T}"/> after all subcomponents of the validatable object gets validated.
        /// In this phase the complex object subtree is already validated and one can react selectively, based on the intermediate state of Validation <seealso cref="ValidationResult.IsValid"/>
        /// and validate all the simple properties, which are closely related to complex object subtree of this validatable object.
        /// NOTE: This method is intended for pure validation of objects, i.e. there sould be NO side effects during the validation of object implementing this interface.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        void PostStructureValidation(ValidationResult validationResult);
    }
}
