namespace MJsNetExtensions.Xml.Serialization
{
    using System;

    /// <summary>
    /// IXsiSchemaLocationInformation interface
    /// </summary>
    public interface IXsiSchemaLocationInformation
    {
        /// <summary>
        /// Get the default XML namespace of the XSD
        /// </summary>
        string DefaultNamespace { get; }

        /// <summary>
        /// Get the XSD location <see cref="Uri"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        string XsdLocationUrl { get; }

        /// <summary>
        /// If true, then the serialized XML contains an XSD reference in the form of xsi:schemaLocation or xsi:noNamespaceSchemaLocation
        /// </summary>
        bool AddSchemaLocationToResultXml { get; }

    }
}
