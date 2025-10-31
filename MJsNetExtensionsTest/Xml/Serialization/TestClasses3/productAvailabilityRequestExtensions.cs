#pragma warning disable CA1708
namespace Customer.Types.Schema.Samples.productAvailabilityRequest
{
    using MJsNetExtensions.Xml.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using System.Xml.XPath;



    /// <summary>
    /// Summary description for productAvailabilityRequestConstructors
    /// </summary>
    public partial class productAvailabilityRequest : CHRequestCommons
    {
        #region Properties
        public static readonly XsiSchemaLocationData XsdInfoConst = new()
        {
            DefaultNamespace = null,
            XsdLocationUrl = "https://salesweb.customer.com/schemas/productAvailabilityRequest.xsd",
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
        public override bool InterfaceVersionSpecified => this.interfaceVersionSpecified;

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
        public override string ReferenceNumber => string.Empty;

        [XmlIgnore]
        public override int PositionsCount => this.products?.product?.Length ?? 0;

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

            if (!string.IsNullOrWhiteSpace(requestString) &&
                this.products?.product != null
                )
            {
                XDocument xdoc = XDocument.Parse(requestString);

                // See also: Using Xpath With Default Namespace in C#
                // https://stackoverflow.com/questions/585812/using-xpath-with-default-namespace-in-c-sharp
                var namespaceManager = new XmlNamespaceManager(new NameTable());
                namespaceManager.AddNamespace("amedis", "https://salesweb.customer.com/schemas/");

                //NOTE: PA request has no namespace!
                var productElements = xdoc.Root.XPathSelectElements("products/product", namespaceManager);
                if (productElements?.Count() == this.products.product.Length)
                {
                    int index = 0;
                    foreach (XElement productElement in productElements)
                    {
                        string numberText = productElement.Attribute("number")?.Value?.Trim();

                        product prod = this.products.product[index];
                        prod.ArticleWithPadding = numberText != null && numberText[0] == '0';

                        index++;
                    }
                }
            }
        }
        #endregion API - Public Methods
    }


    public partial class product
    {
        [XmlIgnore]
        public bool ArticleWithPadding { get; protected internal set; }

    }
}
