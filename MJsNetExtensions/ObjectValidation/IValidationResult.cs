namespace MJsNetExtensions.ObjectValidation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Boolean Validation Result is a class fixing the the old pattern: "bool IsValid(out string invalidReason) {...}" and levearaging the validation to use validation result object instead.
    /// An object of this class undergoes 2 phases: a build phase, where the valiation results are collected 
    /// and a reporting phase after validation finished. For reporting the <see cref="IsValid"/> and <see cref="InvalidReason"/> are the main properties.
    /// Other methods and properties are servig for collecting validation results and formatting the optional <see cref="InvalidReason"/>, which in not null only in a case <see cref="IsValid"/> is false.
    /// Initially an instance of this class has <see cref="IsValid"/> set to true and <see cref="InvalidReason"/> set to null.
    /// </summary>
    public interface IValidationResult
    {
        /// <summary>
        /// A flag (boolean) indicating the actual validation result. Initially it is always true (default value). 
        /// After the validation it is true, if all the checked error (invalidation) conditions were false. 
        /// Otherwise the <see cref="IsValid"/> is false, i.e. if any of the checked error (invalidation) conditions was true.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// If the <see cref="IsValid"/> is true, this property returns null.
        /// Othewise it returns the final aggregated invalid reason string, consisting of the optional <see cref="InvalidReasonPrefix"/>
        /// and the collected partial <see cref="InvalidReasons"/> for all error (invalidation) conditions which were true. All particles are separated by <see cref="Environment.NewLine"/>.
        /// </summary>
        string InvalidReason { get; }

        /// <summary>
        /// The optional invalid reason prefix. If provided and the <see cref="IsValid"/> is false, then it is uesed as a prefix of the final <see cref="InvalidReason"/> message.
        /// </summary>
        string InvalidReasonPrefix { get; set; }

        /// <summary>
        /// The sorted collection of the single invalid reason particles, which form finally the aggregated <see cref="InvalidReason"/>.
        /// </summary>
        IEnumerable<string> InvalidReasons { get; }
    }
}