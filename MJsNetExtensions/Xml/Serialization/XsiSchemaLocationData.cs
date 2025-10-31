namespace MJsNetExtensions.Xml.Serialization
{
    using MJsNetExtensions.Xml;
    using MJsNetExtensions.Xml.Validation;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;


    /// <summary>
    /// XSD information helper data class to simplify handling with serializable classes.
    /// </summary>
    public class XsiSchemaLocationData : IXsiSchemaLocationInformation
    {
        #region Properties

        /// <summary>
        /// Gets or sets the XML namespace of the XSD. Default is null.
        /// </summary>
        public virtual string DefaultNamespace { get; set; }

        /// <summary>
        /// Gets or sets the location <see cref="Uri"/> of the XSD. Default is null.
        /// </summary>
        public virtual string XsdLocationUrl { get; set; }

        /// <summary>
        /// If true, then the serialized XML contains an XSD reference in the form of xsi:schemaLocation or xsi:noNamespaceSchemaLocation. Default is false.
        /// </summary>
        public bool AddSchemaLocationToResultXml { get; set; }

        #endregion Properties
    }
}
