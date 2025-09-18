namespace MJsNetExtensions.Xml.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Mode of URI location translation for the <see cref="DoNotGoToWebXmlResolver"/>
    /// </summary>
    public enum UriTranslationMode
    {
        /// <summary>
        /// No custom translation. Every URI resolves as defined in the default resolver <see cref="System.Xml.XmlUrlResolver"/>.
        /// </summary>
        TranslateNoneButRelayOnXmlUrlResolver = 0,

        /// <summary>
        /// This option means: take all existing Files and UNCs being resolved, else procede with the logic of <see cref="ForcedTranslateAllAddresses"/> for all other cases, e.g.: Web URIs, or the non existing Files, or UNC paths.
        /// </summary>
        TranslateWebAddressesOnly = 1,

        /// <summary>
        /// This is the DEFAULT resolving mode.
        /// Forced translation of all URIs in the XML file being validated (Files, UNCs, Web Addresses, ... stored in the XML attributes: xsi:schemaLocation or xsi:noNamespaceSchemaLocation) 
        /// to the locations defined in the provided <see cref="DoNotGoToWebXmlResolver.KnownDefinitionFiles"/>, or using the <see cref="DoNotGoToWebXmlResolver.CurrentBaseUri"/>, 
        /// which is automaticaly set to the directory path of the currently validated XML file.
        /// If the URI is resolved relative to the <see cref="DoNotGoToWebXmlResolver.KnownDefinitionFiles"/>, or the <see cref="DoNotGoToWebXmlResolver.CurrentBaseUri"/>, then it is automatically added to the known file list.
        /// This enables loading of the XSDs / DTDs included and imported in the <see cref="DoNotGoToWebXmlResolver.KnownDefinitionFiles"/>.
        /// </summary>
        ForcedTranslateAllAddresses = 2
    };

}
