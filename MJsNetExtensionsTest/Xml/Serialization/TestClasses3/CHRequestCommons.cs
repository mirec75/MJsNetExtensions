using SomeCustomer.Types.Schema;

namespace Customer.Types.Schema.Samples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using MJsNetExtensions.Xml.Serialization;


    /// <summary>
    /// Summary description for CHRequestCommons
    /// </summary>
    public abstract class CHRequestCommons : XmlSerializableRootElementBase, IRequestCommons
    {
        #region Properties

        [XmlIgnore]
        public abstract int MaxSupportedInterfaceVersion { get; protected set; }

        [XmlIgnore]
        public abstract int InterfaceVersion { get; protected set; }

        [XmlIgnore]
        public abstract bool InterfaceVersionSpecified { get; }

        [XmlIgnore]
        public int BranchNo { get; private set; }

        [XmlIgnore]
        public abstract string ClientNumber { get; protected set; }

        /// <summary>
        /// This is the numerical equivalent to <see cref="ClientNumber"/>.
        /// It is &gt; 0 only if the <see cref="ClientNumber"/> could be parsed as an integer number.
        /// </summary>
        [XmlIgnore]
        public int CustomerNo { get; private set; }

        /// <summary>
        /// Is language attribute provided in the request
        /// </summary>
        [XmlIgnore]
        public abstract bool LanguageSpecified { get; }

        [XmlIgnore]
        public abstract string ReferenceNumber { get; }

        [XmlIgnore]
        public abstract int PositionsCount { get; }

        #endregion Properties

        #region API - Public Methods
        public virtual void PostXmlDeserializeUpdate(string requestString, int branchNo)
        {
            if (!this.InterfaceVersionSpecified || this.InterfaceVersion < 1)
            {
                // Set the XSD default:
                this.InterfaceVersion = 1;
            }
            else if (this.InterfaceVersion > this.MaxSupportedInterfaceVersion)
            {
                //log?.AddDetail($"Wong request interface version {this.InterfaceVersion}. Maximal supported interface version is: {this.MaxSupportedInterfaceVersion}, so reducig version to allowed maximum.", SalesWebConfigBackendLogSeverity.Warn);
                this.InterfaceVersion = this.MaxSupportedInterfaceVersion;
            }

            this.BranchNo = branchNo;

            if (!string.IsNullOrWhiteSpace(this.ClientNumber))
            {
                this.ClientNumber = PurifyClientNumberString(this.ClientNumber);

                int customerNo;
                if (int.TryParse(this.ClientNumber, out customerNo))
                {
                    this.CustomerNo = customerNo;
                }
            }
        }
        #endregion API - Public Methods

        #region Private Methods

        /// <summary>
        /// Purify Client Number string optionally prefixed with 'CH' prefix so, that after prufy it contains only digits.
        /// </summary>
        /// <param name="clientNumber">Client number string optionally prefixed with 'CH' prefix.</param>
        /// <returns>Purified client number string</returns>
        private static string PurifyClientNumberString(string clientNumber)
        {
            string purifiedClientNumber = clientNumber?.Trim().ToUpperInvariant().Replace("CH", "", StringComparison.InvariantCulture);
            return purifiedClientNumber;
        }

        #endregion Private Methods
    }
}
