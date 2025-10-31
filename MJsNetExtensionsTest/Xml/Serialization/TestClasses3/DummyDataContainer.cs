using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJsNetExtensionsTest.Xml.Serialization.TestClasses3
{
    /// <summary>
    /// Summary description for DummyDataContainer
    /// </summary>
    public class DummyDataContainer
    {
        public short Branch { get; set; }

        public string UserName { get; set; }

        public DateTime RequestStartTime { get; set; } = DateTime.MinValue;

        public int Port { get; set; }

        public List<DummySubContainer> Children { get; set; }

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
            return this.Equals(obj as DummyDataContainer);
        }

        /// <summary>
        /// Determines whether the specified <see cref="DummyDataContainer"/> is equal to this instance.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <returns>True if the given <see cref="DummyDataContainer"/> equals this instance</returns>
        public bool Equals(DummyDataContainer that)
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

            if (!string.Equals(this.UserName, that.UserName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (this.Branch != that.Branch)
            {
                return false;
            }

            if (this.Port != that.Port)
            {
                return false;
            }

            if (this.RequestStartTime != that.RequestStartTime)
            {
                return false;
            }

            if (this.Children != that.Children)
            {
                if ((this.Children?.Count ?? 0) != (that.Children?.Count ?? 0))
                {
                    return false;
                }

                if (this.Children != null && that.Children != null)
                {
                    for (int ii = 0; ii < this.Children.Count; ii++)
                    {
                        if (this.Children[ii] == that.Children[ii])
                        { }
                        else if (this.Children[ii] == null)
                        {
                            return false;
                        }
                        else if (!this.Children[ii].Equals(that.Children[ii]))
                        {
                            return false;
                        }
                    }
                }
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
            int hash = this.Branch.GetHashCode();
            hash ^= (this.UserName ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= this.Port.GetHashCode();
            hash ^= this.RequestStartTime.GetHashCode();

            if ((this.Children?.Count > 0))
            {
                foreach (var detail in this.Children.Where(it => it != null))
                {
                    hash ^= detail.GetHashCode();
                }
            }
            return hash;
        }
        #endregion Object Equality Comparison
    }
}
