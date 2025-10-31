namespace MJsNetExtensionsTest.Xml.Serialization.TestClasses2
{
    using MJsNetExtensions;
    using TestClasses1;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using MJsNetExtensions.ObjectValidation;


    /// <summary>
    /// Summary description for DetailsLogDataTest2
    /// </summary>
    public class DetailsLogDataTest2 : ISimpleValidatableAndUpdatable
    {
        #region Properties

        public long Id { get; set; }

        public long LogId { get; set; }

        public DateTime DetailDateTime { get; set; } = DateTime.MinValue;

        public DummyLevel Level { get; set; } = DummyLevel.None;

        public string Component { get; set; }

        public string Message { get; set; }

        #endregion Properties

        #region API - Public Methods

        /// <summary>
        /// The Pre Structure validation method, which is called from inside of <see cref="ValidationExtensions.Validate(IValidatable)"/> before all subcomponents of the validatable object gets validated.
        /// In this phase all the simple properties have to be validated, which are not closely related to complex object subtree of the validatable object.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public void PreStructureValidationAndUpdate(ValidationResult validationResult)
        {
            validationResult
                .ThrowIfNull(nameof(validationResult))
                .InvalidateIf(this.DetailDateTime == DateTime.MinValue, "{0} not provided", nameof(this.DetailDateTime));
            validationResult.InvalidateIf(this.Level == DummyLevel.None, "{0} not provided", nameof(this.Level));
            validationResult.InvalidateIfNullOrWhiteSpace(this.Component, nameof(this.Component));
            validationResult.InvalidateIfNullOrWhiteSpace(this.Message, nameof(this.Message));
        }

        #region Object Equality Comparison
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as DetailsLogDataTest2);
        }

        /// <summary>
        /// Determines whether the specified <see cref="DetailsLogDataTest2"/> is equal to this instance.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <returns>True if the given <see cref="DetailsLogDataTest2"/> equals this instance</returns>
        public bool Equals(DetailsLogDataTest2 that)
        {
            // If parameter is null, return false. 
            if (ReferenceEquals(that, null))
            {
                return false;
            }

            // Optimization for a common success case. 
            if (ReferenceEquals(this, that))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false. 
            if (this.GetType() != that.GetType())
            {
                return false;
            }

            // Return true if the fields match. 
            // Note that the base class is not invoked because it is 
            // System.Object, which defines Equals as reference equality. 

            if (this.Id != that.Id)
            {
                return false;
            }

            if (this.LogId != that.LogId)
            {
                return false;
            }

            if (this.DetailDateTime != that.DetailDateTime)
            {
                return false;
            }

            if (this.Level != that.Level)
            {
                return false;
            }

            if (!string.Equals(this.Component, that.Component, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(this.Message, that.Message, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = this.Id.GetHashCode();
            hash ^= this.LogId.GetHashCode();
            hash ^= this.DetailDateTime.GetHashCode();
            hash ^= this.Level.GetHashCode();
            hash ^= (this.Component ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= (this.Message ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);

            return hash;
        }
        #endregion Object Equality Comparison

        #endregion API - Public Methods
    }
}
