namespace MJsNetExtensionsTest.Xml.Serialization.TestClasses3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.abcd.com/index")]
    public partial class ArticleWholesaler
    {

        private string iNDEXField;

        private System.DateTime fROMDATEField;

        private ArticleWholesalerFILTER fILTERField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public string INDEX
        {
            get => this.iNDEXField;
            set => this.iNDEXField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime FROMDATE
        {
            get => this.fROMDATEField;
            set => this.fROMDATEField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute()]
        public ArticleWholesalerFILTER FILTER
        {
            get => this.fILTERField;
            set => this.fILTERField = value;
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
            return this.Equals(obj as ArticleWholesaler);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ArticleWholesaler"/> is equal to this instance.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <returns>True if the given <see cref="ArticleWholesaler"/> equals this instance</returns>
        public bool Equals(ArticleWholesaler that)
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

            if (!string.Equals(this.INDEX, that.INDEX, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (this.FILTER != that.FILTER)
            {
                return false;
            }

            if (this.FROMDATE != that.FROMDATE)
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
            int hash = this.FILTER.GetHashCode();
            hash ^= (this.INDEX ?? "").GetHashCode();
            hash ^= this.FROMDATE.GetHashCode();

            return hash;
        }
        #endregion Object Equality Comparison
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.abcd.com/index")]
    public enum ArticleWholesalerFILTER
    {

        /// <remarks/>
        A,

        /// <remarks/>
        D,

        /// <remarks/>
        ALL,
    }

}
