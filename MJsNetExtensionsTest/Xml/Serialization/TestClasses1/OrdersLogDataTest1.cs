namespace MJsNetExtensionsTest.Xml.Serialization.TestClasses1
{
    using MJsNetExtensions;
    using MJsNetExtensions.ObjectValidation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;


    /// <summary>
    /// Summary description for OrdersLogDataTest1
    /// </summary>
    public class OrdersLogDataTest1 : ISimpleValidatable
    {
        #region Properties

        /// <summary>
        /// E.g. "ChPaDetailsLog" for Product Avaliability. This is used to address the SQL DB Table.
        /// </summary>
        [XmlAttribute]
        public string DbTableName { get; set; }

        public long Id { get; set; }

        public long LogId { get; set; }

        public int OrderNo { get; set; }

        public int PosCountRequest { get; set; }

        public int PosCountResponse { get; set; }


        public DateTime CreationDateTime { get; set; }

        public int CreationDate { get; set; }

        #endregion Properties

        #region API - Public Methods

        /// <summary>
        /// The Pre Structure validation method, which is called from inside of <see cref="ValidationExtensions.Validate(IValidatable)"/> before all subcomponents of the validatable object gets validated.
        /// In this phase all the simple properties have to be validated, which are not closely related to complex object subtree of the validatable object.
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public void PreStructureValidation(ValidationResult validationResult)
        {
            validationResult
                .ThrowIfNull(nameof(validationResult))
                .InvalidateIfNullOrWhiteSpace(this.DbTableName, nameof(this.DbTableName));
            validationResult.InvalidateIf(this.OrderNo == 0, "{0} not provided", nameof(this.OrderNo));
            validationResult.InvalidateIf(this.PosCountRequest == 0, "{0} not provided", nameof(this.PosCountRequest));
            validationResult.InvalidateIf(this.PosCountResponse == 0, "{0} not provided", nameof(this.PosCountResponse));
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
            return this.Equals(obj as OrdersLogDataTest1);
        }

        /// <summary>
        /// Determines whether the specified <see cref="OrdersLogDataTest1"/> is equal to this instance.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <returns>True if the given <see cref="OrdersLogDataTest1"/> equals this instance</returns>
        public bool Equals(OrdersLogDataTest1 that)
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

            if (this.OrderNo != that.OrderNo)
            {
                return false;
            }

            if (this.PosCountRequest != that.PosCountRequest)
            {
                return false;
            }

            if (this.PosCountResponse != that.PosCountResponse)
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
            hash ^= this.OrderNo.GetHashCode();
            hash ^= this.PosCountRequest.GetHashCode();
            hash ^= this.PosCountResponse.GetHashCode();
            hash ^= this.CreationDateTime.GetHashCode();
            hash ^= this.CreationDate.GetHashCode();

            return hash;
        }
        #endregion Object Equality Comparison

        #endregion API - Public Methods
    }
}
