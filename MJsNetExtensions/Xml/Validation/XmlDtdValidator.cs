namespace MJsNetExtensions.Xml.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// General XML Validation functionality against a given DTD set.
    /// </summary>
    public class XmlDtdValidator : XmlValidator
    {
        #region Construction / Destruction
        /// <summary>
        /// Construct a DTD XML Validator
        /// </summary>
        /// <param name="settings">The <see cref="XmlValidatorSettings"/> object used to configure the new <see cref="XmlValidator"/>.</param>
        protected internal XmlDtdValidator(XmlValidatorSettings settings)
          : base(settings)
        {
            Throw.IfNot(settings?.XmlValidationType == XmlValidationType.DTD, nameof(settings), "Wrong {0}: {1}. It must be: {2}", nameof(XmlValidatorSettings.XmlValidationType), settings?.XmlValidationType, XmlValidationType.DTD);
        }
        #endregion Construction / Destruction

        #region Properties

        /// <summary>
        /// The type of validation is DTD <see cref="XmlValidationType.DTD"/>.
        /// </summary>
        public override XmlValidationType XmlValidationType => XmlValidationType.DTD;

        #endregion Properties

        #region API - Public Methods
        /// <summary>
        /// Internal pure Step specific construction time Initialize
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.OwnValidatingReaderSettings.ValidationType = ValidationType.DTD;
        }
        #endregion API - Public Methods
    }
}
