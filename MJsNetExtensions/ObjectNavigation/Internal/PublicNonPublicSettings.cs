using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJsNetExtensions.ObjectNavigation.Internal
{
    /// <summary>
    /// Summary description for PublicNonPublicSettings
    /// </summary>
    internal sealed class PublicNonPublicSettings
    {
        #region Properties

        /// <summary>
        /// The Container Type, Properties or Fields of which have to be enumerated.
        /// </summary>
        public Type ContainerType { get; set; }

        /// <summary>
        /// Flag indicating if the public instance have to be listed. Default is: false.
        /// </summary>
        public bool ListPublic { get; set; }

        /// <summary>
        /// Flag indicating if the NON public instance have to be listed. Default is: false.
        /// </summary>
        public bool ListNonpublic { get; set; }

        #endregion Properties

        #region API - Public Methods - Object Equality Comparison

        /// <summary>
        /// Clone method
        /// </summary>
        /// <returns></returns>
        public PublicNonPublicSettings Clone()
        {
            // make ShallowCopy first:
            return (PublicNonPublicSettings)this.MemberwiseClone();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as PublicNonPublicSettings);
        }

        /// <summary>
        /// Determines whether the specified <see cref="PublicNonPublicSettings"/> is equal to this instance.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <returns>True if the given <see cref="PublicNonPublicSettings"/> equals this instance</returns>
        public bool Equals(PublicNonPublicSettings that)
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

            if (this.ContainerType != that.ContainerType)
            {
                return false;
            }

            if (this.ListPublic != that.ListPublic)
            {
                return false;
            }

            if (this.ListNonpublic != that.ListNonpublic)
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
            int hash = this.ContainerType?.GetHashCode() ?? 0;
            hash ^= this.ListPublic.GetHashCode();
            hash ^= this.ListNonpublic.GetHashCode();

            return hash;
        }
        #endregion API - Public Methods - Object Equality Comparison
    }
}
