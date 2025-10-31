namespace MJsNetExtensionsTest
{
    using MJsNetExtensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;



    /// <summary>
    /// Summary description for GeneralExtensionsTest
    /// </summary>
    [TestClass]
    public class EnumerableExtensionsInterleaveTest
    {
        [TestMethod]
        public void InterleaveEnumerationsOfEqualLengthTest1_ExpectSuccess()
        {
            // Arrange:
            var first = new[] { "1", "2", "3", };
            var second = new[] { "a", "b", "c", };

            // Act:
            var result = first.InterleaveEnumerationsOfEqualLength(second).ToArray();

            string resultText = result.Aggregate((acc, it) => acc += it);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultText);
            Assert.AreEqual("1a2b3c", resultText);
        }

        [TestMethod]
        public void InterleaveEnumerationsOfEqualLengthTest2_ExpectSuccess()
        {
            // Arrange:
            var first = new[] { "1", "2", "3", "4", "5", };
            var second = new[] { "a", "b", "c", };

            // Act:
            var result = first.InterleaveEnumerationsOfEqualLength(second).ToArray();

            string resultText = result.Aggregate((acc, it) => acc += it);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultText);
            Assert.AreEqual("1a2b3c", resultText);
        }

        [TestMethod]
        public void InterleaveEnumerationsOfEqualLengthTest3_ExpectSuccess()
        {
            // Arrange:
            var first = new[] { "1", "2", "3", };
            var second = new[] { "a", "b", "c", "d", "e", };

            // Act:
            var result = first.InterleaveEnumerationsOfEqualLength(second).ToArray();

            string resultText = result.Aggregate((acc, it) => acc += it);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultText);
            Assert.AreEqual("1a2b3c", resultText);
        }

        [TestMethod]
        public void InterleaveTest1_ExpectSuccess()
        {
            // Arrange:
            var first = new[] { "1", "2", "3", };
            var second = new[] { "a", "b", "c", };

            // Act:
            var result = first.Interleave(second).ToArray();

            string resultText = result.Aggregate((acc, it) => acc += it);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultText);
            Assert.AreEqual("1a2b3c", resultText);
        }

        [TestMethod]
        public void InterleaveTest2_ExpectSuccess()
        {
            // Arrange:
            var first = new[] { "1", "2", "3", "4", "5", };
            var second = new[] { "a", "b", "c", };

            // Act:
            var result = first.Interleave(second).ToArray();

            string resultText = result.Aggregate((acc, it) => acc += it);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultText);
            Assert.AreEqual("1a2b3c45", resultText);
        }

        [TestMethod]
        public void InterleaveTest3_ExpectSuccess()
        {
            // Arrange:
            var first = new[] { "1", "2", "3", };
            var second = new[] { "a", "b", "c", "d", "e", };

            // Act:
            var result = first.Interleave(second).ToArray();

            string resultText = result.Aggregate((acc, it) => acc += it);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultText);
            Assert.AreEqual("1a2b3cde", resultText);
        }

    }
}
