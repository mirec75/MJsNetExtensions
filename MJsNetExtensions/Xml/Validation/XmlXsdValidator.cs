#pragma warning disable S125
namespace MJsNetExtensions.Xml.Validation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;

    /// <summary>
    /// General XML Validation functionality against a given Xml Schema set.
    /// </summary>
    public class XmlXsdValidator : XmlValidator
    {
        #region Construction / Destruction
        /// <summary>
        /// Construct a XSD XML Validator
        /// </summary>
        /// <param name="settings">The <see cref="XmlValidatorSettings"/> object used to configure the new <see cref="XmlValidator"/>.</param>
        protected internal XmlXsdValidator(XmlValidatorSettings settings)
          : base(settings)
        {
            Throw.IfNot(settings?.XmlValidationType == XmlValidationType.XSD, nameof(settings), "Wrong {0}: {1}. It must be: {2}", nameof(XmlValidatorSettings.XmlValidationType), settings?.XmlValidationType, XmlValidationType.XSD);
        }
        #endregion Construction / Destruction

        #region Properties and Fields

        /// <summary>
        /// The type of validation is XSD <see cref="XmlValidationType.XSD"/>.
        /// </summary>
        public override XmlValidationType XmlValidationType => XmlValidationType.XSD;

        /// <summary>
        /// Gets the <see cref="XmlSchemaSet"/> to use when performing schema validation.
        /// </summary>
        public XmlSchemaSet Schemas => this.OwnValidatingReaderSettings.Schemas;

        #endregion Properties and Fields

        #region API - Public Methods
        /// <summary>
        /// Internal pure Step specific construction time Initialize
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // set XSD vaidating
            this.OwnValidatingReaderSettings.ValidationType = ValidationType.Schema;

            // special settings
            this.OwnValidatingReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            this.OwnValidatingReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            this.OwnValidatingReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            // NOTE: XmlSchemaValidationFlags.ProcessIdentityConstraints 
            //   and XmlSchemaValidationFlags.AllowXmlAttributes are set per default

            //NOTE: explicitely not giving the "DoNotGoToWebXmlResolver" to OwnValidatingReaderSettings.Schemas.XmlResolver! So following line stays commented, until a debug session for better understanding is necessary:
            //this.OwnValidatingReaderSettings.Schemas.XmlResolver = this.DoNotGoToWebXmlResolver;
            //
            // Reason: the setting is not necessary! We work now in mode: DoNotGoToWebXmlResolver.DisableSpecificResolvingAndRelayJustOnStandardXmlUrlResolverResolving() anyway -> that's why I use actually the default XmlUrlResolver functionality!
            // -> using the line above you just would end up with the "DoNotGoToWebXmlResolver" acting like the default XmlUrlResolver... so there is no need. 
            //    Interesting is, in debugger we would see the XSD resolving and loading of all included and imported XSDs as each known definition file is resolved during the following call in the foreach() below:
            //    this.OwnValidatingReaderSettings.Schemas.Add(null, xsdFile.ToString());
            // -> AND it does NOT matter, if the user provided "this.KnownDefinitionFiles" are Web URIs or local files, I use them as is, as given by the caller. The caller / user is responsible for the XSDs
            //    and the proper location they are loaded from.
            // CONCLUSION: the DoNotGoToWebXmlResolver is actually used only for resolving the actual XML file being validated AND for any ADDITIONAL XSDs provided in the XML attributes: xsi:schemaLocation or xsi:noNamespaceSchemaLocation of the validated XML...

            this.OwnValidatingReaderSettings.Schemas.XmlResolver = this.DoNotGoToWebXmlResolver;

            // read and compile all XSDs
            foreach (Uri xsdFile in this.DoNotGoToWebXmlResolver.KnownDefinitionFiles.Values)
            {
                try
                {
                    this.OwnValidatingReaderSettings.Schemas.Add(null, xsdFile.ToString());
                }
                catch (IOException)
                {
                    throw; // just rethrow. The right info is already inside it.
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw new UnauthorizedAccessException($"Can not access provided XSD file: {xsdFile}", ex);
                }
                catch (WebException ex)
                {
                    throw new WebException($"Can not access provided XSD file: {xsdFile}", ex);
                }
                catch (XmlException ex)
                {
                    throw new XmlException($"Can not parse provided XSD file: {xsdFile}", ex);
                }
                catch (Exception ex)
                {
#pragma warning disable CA2201,S112
                    throw new Exception($"General error while accessing provided XSD file: {xsdFile}", ex);
#pragma warning restore CA2201,S112
                }
            }

            //NOTE: all XSDs are already resolved and loaded, now do just compile them:
            this.OwnValidatingReaderSettings.Schemas.Compile();

            // Signal now to the resolver, the known definition files are loaded and understood without problems:
            this.DoNotGoToWebXmlResolver.KnownDefinitionFilesAreLoaded();
        }
        #endregion API - Public Methods
    }
}
