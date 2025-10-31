namespace MJsNetExtensionsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MJsNetExtensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    /// <summary>
    /// Summary description for EmbeddedResourceHelperAsStringTest
    /// </summary>
    [TestClass]
    public class EmbeddedResourceHelperAsStringTest
    {
        [TestMethod]
        public void Test1ExistingRelativePath_ExpectSuccess()
        {
            // Arrange:
            string resourceName = @"Test\Emb-edded Res_sourc-e  = `$[1()]{}°~!@^_+;,.'\Dummy Embedded-Resource = `$[2()]{}°~!@^_+;,.'.txt";

            // Act:
            string result = EmbeddedResourceHelper.GetEmbeddedResource(resourceName);

            // Assert:
            Assert.IsNotNull(result);
            Assert.AreEqual("Hi I'm a dummy! :)", result);
        }

        [TestMethod]
        public void Test2ExistingRelativePath_ExpectSuccess()
        {
            // Arrange:
            string resourceName = @"Dummy Embedded-Resource = 1.txt";

            // Act:
            string result = EmbeddedResourceHelper.GetEmbeddedResource(resourceName);

            // Assert:
            Assert.IsNotNull(result);
            Assert.AreEqual("Hi I'm a dummy! :)", result);
        }

        [TestMethod]
        public void Test3NonExistingRelativePath_ExpectSuccess()
        {
            // Arrange:
            string resourceName = @"xTestZ\Emb-edded Res_sourc-e  = `$[1()]{}°~!@^_+;,.'\Dummy Embedded-Resource = `$[2()]{}°~!@^_+;,.'.txt";

            // Act:
            string result = EmbeddedResourceHelper.GetEmbeddedResource(resourceName);

            // Assert:
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Test4NonExistingFieName_ExpectSuccess()
        {
            // Arrange:
            string resourceName = @"zzZ Dummy Embedded-Resource = `$[2()]{}°~!@^_+;,.'.txt zz";

            // Act:
            string result = EmbeddedResourceHelper.GetEmbeddedResource(resourceName);

            // Assert:
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Test5NonExistingRelativePath_ExpectException()
        {
            // Arrange:
            string resourceName = @"\\\\\";

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => EmbeddedResourceHelper.GetEmbeddedResource(resourceName));
        }

        [TestMethod]
        public void Test6NonExistingRelativePath_ExpectException()
        {
            // Arrange:
            string resourceName = @"/////";

            // Act:
            Assert.ThrowsExactly<InvalidOperationException>(() => EmbeddedResourceHelper.GetEmbeddedResource(resourceName));
        }

        [TestMethod]
        public void Test7NullResourceName_ExpectException()
        {
            // Arrange:
            string resourceName = null;

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => EmbeddedResourceHelper.GetEmbeddedResource(resourceName));
        }

        [TestMethod]
        public void Test8EmptyResourceName_ExpectException()
        {
            // Arrange:
            string resourceName = @"";

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => EmbeddedResourceHelper.GetEmbeddedResource(resourceName));
        }

        [TestMethod]
        public void Test9WhitespaceResourceName_ExpectException()
        {
            // Arrange:
            string resourceName = " \t\r\n ";

            // Act:
            Assert.ThrowsExactly<ArgumentNullException>(() => EmbeddedResourceHelper.GetEmbeddedResource(resourceName));
        }

    }
}
