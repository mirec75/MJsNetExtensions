namespace MJsNetExtensionsTest.Xml.Serialization.TestClasses3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;


    /// <summary>
    /// Summary description for DummyError
    /// </summary>
    public class DummyError
    {
        [XmlAttribute]
        public string Number { get; set; }

        [XmlAttribute]
        public string Message { get; set; }

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
            return this.Equals(obj as DummyError);
        }

        /// <summary>
        /// Determines whether the specified <see cref="DummyError"/> is equal to this instance.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <returns>True if the given <see cref="DummyError"/> equals this instance</returns>
        public bool Equals(DummyError that)
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

            if (!string.Equals(this.Number, that.Number, StringComparison.OrdinalIgnoreCase))
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
            int hash = (this.Number ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= (this.Message ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);

            return hash;
        }
        #endregion Object Equality Comparison
    }
}
