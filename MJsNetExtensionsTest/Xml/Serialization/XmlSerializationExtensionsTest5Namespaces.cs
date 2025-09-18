namespace MJsNetExtensionsTest.Xml.Serialization
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MJsNetExtensions.Xml.Serialization;
    using MJsNetExtensionsTest.Xml.Serialization.TestClasses3;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    [TestClass]
    public class XmlSerializationExtensionsTest5Namespaces
    {
        /// <summary>
        /// The 1968 book "The Last Unicorn"
        /// </summary>
        public const string TheLastUnicornIsbnNamespace = "urn:isbn:0451450523"; 

        [TestMethod]
        public void Test1WithNoSettings_ExpectSuccess()
        {
            // Arrange:
            ArticleWholesaler response = GenerateDummyArticleWholesaler();

            // Act:
            string resultString = response.ToXml();

            // Assert:
            Assert.IsNotNull(resultString);
            Assert.AreEqual("<ArticleWholesaler xmlns=\"http://www.abcd.com/index\">\r\n  <INDEX>hehe</INDEX>\r\n  <FROMDATE>2022-02-10</FROMDATE>\r\n  <FILTER>A</FILTER>\r\n</ArticleWholesaler>", resultString);

            // Act:
            ArticleWholesaler parsedResponse = resultString.ParseXmlTo<ArticleWholesaler>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void Test2WithDifferentDefaultXmlNamespaceSettings_ExpectSuccess()
        {
            // Arrange:
            ArticleWholesaler response = GenerateDummyArticleWholesaler();
            XmlToStringSerializationSettings settings = new XmlToStringSerializationSettings(TheLastUnicornIsbnNamespace, false);

            // Act:
            string resultString = response.ToXml(settings);

            // Assert:
            Assert.IsNotNull(resultString);
            Assert.AreEqual("<q1:ArticleWholesaler xmlns=\"urn:isbn:0451450523\" xmlns:q1=\"http://www.abcd.com/index\">\r\n  <q1:INDEX>hehe</q1:INDEX>\r\n  <q1:FROMDATE>2022-02-10</q1:FROMDATE>\r\n  <q1:FILTER>A</q1:FILTER>\r\n</q1:ArticleWholesaler>", resultString);

            // Act:
            ArticleWholesaler parsedResponse = resultString.ParseXmlTo<ArticleWholesaler>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        [TestMethod]
        public void Test3WithEmptySettings_ExpectSuccess()
        {
            // Arrange:
            ArticleWholesaler response = GenerateDummyArticleWholesaler();
            XmlToStringSerializationSettings settings = new XmlToStringSerializationSettings(); //NOTE: empty settings introduce Namespaces == null

            // Act:
            string resultString = response.ToXml(settings);

            // Assert:
            Assert.IsNotNull(resultString);
            Assert.AreEqual("<ArticleWholesaler xmlns=\"http://www.abcd.com/index\">\r\n  <INDEX>hehe</INDEX>\r\n  <FROMDATE>2022-02-10</FROMDATE>\r\n  <FILTER>A</FILTER>\r\n</ArticleWholesaler>", resultString);

            // Act:
            ArticleWholesaler parsedResponse = resultString.ParseXmlTo<ArticleWholesaler>();

            // Assert:
            Assert.IsNotNull(parsedResponse);
            Assert.AreEqual(response, parsedResponse); //NOTE: the "public override bool Equals(object obj)" is called
        }

        private static ArticleWholesaler GenerateDummyArticleWholesaler()
        {
            return new ArticleWholesaler
            {
                FILTER = ArticleWholesalerFILTER.A,
                FROMDATE = new DateTime(2022, 2, 10, 0, 0, 0, DateTimeKind.Local),
                INDEX = "hehe",
            };
        }
    }
}
