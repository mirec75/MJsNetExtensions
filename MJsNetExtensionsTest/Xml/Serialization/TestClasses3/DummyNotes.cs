namespace MJsNetExtensionsTest.Xml.Serialization.TestClasses3
{
    using MJsNetExtensions.Xml.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;


    /// <summary>
    /// Summary description for DummyNotes
    /// </summary>
    public class DummyNotes : XmlSerializableRootElementBase
    {
        public const string ChDefaultNamespace = "https://salesweb.customer.com/schemas/";
        public const string DefaultXsdsRootUrl = ChDefaultNamespace;

        public static readonly XsiSchemaLocationData XsdInfoConst = new()
        {
            DefaultNamespace = null,
            XsdLocationUrl = $"{DummyNotes.DefaultXsdsRootUrl}dummyNotes.xsd",
            //AddSchemaLocationToResultXml = false,
        };

        [XmlIgnore]
        public override IXsiSchemaLocationInformation XsiSchemaLocationInformation => XsdInfoConst;


        [XmlAttribute]
        public string Version { get; set; }

        [XmlAttribute]
        public int InterfaceVersion { get; set; } = 1;

        [XmlIgnore]
        public bool InterfaceVersionSpecified { get; set; }

        public DummyError ClientError { get; set; }


        public static DummyNotes CreateErrorContent(string errorCode, string message)
        {
            DummyNotes ret = new DummyNotes
            {
                Version = "1.1",
                InterfaceVersion = 7,
                InterfaceVersionSpecified = false,
                ClientError = new DummyError { Number = errorCode, Message = message },
            };

            return ret;
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
            return this.Equals(obj as DummyNotes);
        }

        /// <summary>
        /// Determines whether the specified <see cref="DummyNotes"/> is equal to this instance.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <returns>True if the given <see cref="DummyNotes"/> equals this instance</returns>
        public bool Equals(DummyNotes that)
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

            if (!string.Equals(this.XsiSchemaLocationAttributeValue, that.XsiSchemaLocationAttributeValue, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(this.XsiNoNamespaceSchemaLocationAttributeValue, that.XsiNoNamespaceSchemaLocationAttributeValue, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(this.Version, that.Version, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (this.InterfaceVersionSpecified != that.InterfaceVersionSpecified)
            {
                return false;
            }

            if (this.InterfaceVersionSpecified && this.InterfaceVersion != that.InterfaceVersion)
            {
                return false;
            }

            if (this.ClientError != null)
            {
                if (!this.ClientError.Equals(that.ClientError))
                {
                    return false;
                }
            }
            else if (this.ClientError != that.ClientError)
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
            int hash = (this.XsiSchemaLocationAttributeValue ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= (this.XsiNoNamespaceSchemaLocationAttributeValue ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= (this.Version ?? "").GetHashCode(StringComparison.OrdinalIgnoreCase);
            hash ^= this.InterfaceVersionSpecified.GetHashCode();

            if (this.InterfaceVersionSpecified)
            {
                hash ^= this.InterfaceVersion.GetHashCode();
            }

            if (this.ClientError != null)
            {
                hash ^= this.ClientError.GetHashCode();
            }

            return hash;
        }
        #endregion Object Equality Comparison
    }
}
