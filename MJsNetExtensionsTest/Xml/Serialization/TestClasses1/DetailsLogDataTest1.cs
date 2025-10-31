namespace MJsNetExtensionsTest.Xml.Serialization.TestClasses1
{
    using MJsNetExtensions;
    using MJsNetExtensions.ObjectValidation;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;


    /// <summary>
    /// Summary description for DetailsLogDataTest1
    /// </summary>
    public class DetailsLogDataTest1 : IValidatable
    {
        #region Fields
        public CommonLogDataTest1 ownerFieldPublic;
        protected CommonLogDataTest1 ownerFieldProtected;
        private CommonLogDataTest1 ownerFieldPrivate;
        internal CommonLogDataTest1 ownerFieldInternal;


        #endregion Fields

        #region Properties

        /// <summary>
        /// E.g. "ChPaDetailsLog" for Product Avaliability. This is used to address the SQL DB Table.
        /// </summary>
        [XmlAttribute]
        public string DbTableName { get; set; }

        public long Id { get; set; }

        public long LogId { get; set; }

        public DateTime DetailDateTime { get; set; } = DateTime.MinValue;

        public DummyLevel Level { get; set; } = DummyLevel.None;

        public string Component { get; set; }

        public string Message { get; set; }

        public DateTime CreationDateTime { get; set; } = DateTime.MinValue;

        public int CreationDate { get; set; }

        [XmlIgnore]
        public CommonLogDataTest1 Owner { get; set; }

        [XmlIgnore]
        public DetailsLogDataTest1[] Siblings { get; set; }

        [XmlIgnore]
        public Dictionary<string, DetailsLogDataTest1> SiblingsDict { get; set; }

        [XmlIgnore]
        public Dictionary<DetailsLogDataTest1, string> ReverseSiblingsDict { get; set; }

        [XmlIgnore]
        private IEnumerable SiblingsEnumerable => this.Siblings;

        [XmlIgnore]
        protected IEnumerator SiblingsEnumerator => this.Siblings?.GetEnumerator();

        #endregion Properties

        #region API - Public Methods

        /// <summary>
        /// The Pre Structure validation method, which is called from inside of <see cref="ValidationFactory.Validate(ISimpleValidatable)"/> before all subcomponents of the validatable object gets validated.
        /// In this phase all the simple properties have to be validated, which are not closely related to complex object subtree of the validatable object.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public void PreStructureValidation(ValidationResult validationResult)
        {
            validationResult
                .ThrowIfNull(nameof(validationResult))
                .InvalidateIfNullOrWhiteSpace(this.DbTableName, nameof(this.DbTableName));
            validationResult.InvalidateIf(this.DetailDateTime == DateTime.MinValue, "{0} not provided", nameof(this.DetailDateTime));
            validationResult.InvalidateIf(this.Level == DummyLevel.None, "{0} not provided", nameof(this.Level));
            validationResult.InvalidateIfNullOrWhiteSpace(this.Component, nameof(this.Component));
            validationResult.InvalidateIfNullOrWhiteSpace(this.Message, nameof(this.Message));

            //NOTE: this is a HACK according to IValidatable philosophy: "not modifying", but this is just for UnitTest purposes, so its OK!
            this.ownerFieldPublic = this.Owner;
            this.ownerFieldProtected = this.Owner;
            this.ownerFieldPrivate = this.Owner;
            this.ownerFieldInternal = this.Owner;
        }

        /// <summary>
        /// The Post Structure validation method, which is called from inside of <see cref="ValidationFactory.Validate(ISimpleValidatable)"/> after all subcomponents of the validatable object gets validated.
        /// In this phase the complex object subtree is already validated and one can react selectively, based on the intermediate state of Validation <seealso cref="ValidationResult.IsValid"/>
        /// and validate all the simple properties, which are closely related to complex object subtree of this validatable object.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public void PostStructureValidation(ValidationResult validationResult)
        {
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
            return this.Equals(obj as DetailsLogDataTest1);
        }

        /// <summary>
        /// Determines whether the specified <see cref="DetailsLogDataTest1"/> is equal to this instance.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <returns>True if the given <see cref="DetailsLogDataTest1"/> equals this instance</returns>
        public bool Equals(DetailsLogDataTest1 that)
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

            if (!string.Equals(this.DbTableName, that.DbTableName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

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

            if (this.CreationDateTime != that.CreationDateTime)
            {
                return false;
            }

            if (this.CreationDate != that.CreationDate)
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
            int hash = (this.DbTableName ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= this.Id.GetHashCode();
            hash ^= this.LogId.GetHashCode();
            hash ^= this.DetailDateTime.GetHashCode();
            hash ^= this.Level.GetHashCode();
            hash ^= (this.Component ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= (this.Message ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= this.CreationDateTime.GetHashCode();
            hash ^= this.CreationDate.GetHashCode();

            return hash;
        }
        #endregion Object Equality Comparison

        #endregion API - Public Methods
    }
}
