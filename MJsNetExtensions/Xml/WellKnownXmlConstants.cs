namespace MJsNetExtensions.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    /// <summary>
    /// Summary description for WellKnownXmlConstants
    /// </summary>
    public static class WellKnownXmlConstants
    {
        /// <summary>
        /// The well known xsi:schemaLocation XML attribute local name. See explanation on: https://www.w3.org/TR/xmlschema11-1/#xsi_schemaLocation
        /// </summary>
        public const string XmlSchemaLocationAttributeName = "schemaLocation";

        /// <summary>
        /// The well known xsi:noNamespaceSchemaLocation XML attribute local name. See explanation on: https://www.w3.org/TR/xmlschema11-1/#xsi_schemaLocation
        /// </summary>
        public const string XmlNoNamespaceSchemaLocationAttributeName = "noNamespaceSchemaLocation";
    }
}
