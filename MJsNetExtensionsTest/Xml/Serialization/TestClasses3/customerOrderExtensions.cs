namespace Customer.Types.Schema.Samples.customerOrder
{
    using MJsNetExtensions.Xml.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public partial class customerOrder : CHRequestCommons
    {
        #region Properties
        public static readonly XsiSchemaLocationData XsdInfoConst = new()
        {
            DefaultNamespace = "https://salesweb.customer.com/schemas/",
            XsdLocationUrl = "https://salesweb.customer.com/schemas/customerOrder.xsd",
            AddSchemaLocationToResultXml = true,
        };

        [XmlIgnore]
        public override IXsiSchemaLocationInformation XsiSchemaLocationInformation => XsdInfoConst;


        [XmlIgnore]
        public override int MaxSupportedInterfaceVersion { get; protected set; } = 2;

        [XmlIgnore]
        public override int InterfaceVersion
        {
            get => this.interfaceVersion;
            protected set => this.interfaceVersion = value;
        }

        [XmlIgnore]
        public override bool InterfaceVersionSpecified => true;

        [XmlIgnore]
        public override string ClientNumber
        {
            get => this.client?.number;
            protected set
            {
                if (this.client != null)
                {
                    this.client.number = value;
                }
            }
        }

        [XmlIgnore]
        public override bool LanguageSpecified => this.languageSpecified;

        [XmlIgnore]
        public override string ReferenceNumber => this.orderHeader?.referenceNumber;

        [XmlIgnore]
        public override int PositionsCount => this.orderLines?.Length ?? 0;

        #endregion Properties

        #region API - Public Methods

        public override void PostXmlDeserializeUpdate(string requestString, int branchNo)
        {
            base.PostXmlDeserializeUpdate(requestString, branchNo);

            // it is not possible to generalize following if statement, because it the language is generated from XSD several times.... (is there an XSD generating optimization possible?)
            if (this.languageSpecified)
            {
                // Setting language with highest priority (Prio 1):
            }
        }
        #endregion API - Public Methods
    }
}
