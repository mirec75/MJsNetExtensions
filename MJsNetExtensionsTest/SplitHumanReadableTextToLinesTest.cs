namespace MJsNetExtensionsTest
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using MJsNetExtensions;
    using MJsNetExtensions.Xml;

    [TestClass]
    public class SplitHumanReadableTextToLinesTest
    {
        #region Parameter Null, Zero and simmilar Tests
        [TestMethod]
        public void SplitHumanReadableTextToLines_TestNullText_ExpectSuccess()
        {
            // Arrange:
            string text = null;

            // Act:
            List<string> lines = text.SplitHumanReadableTextToLines(10, true);

            // Assert:
            Assert.IsNotNull(lines);
            Assert.AreEqual(0, lines.Count);
        }

        [TestMethod]
        public void SplitHumanReadableTextToLines_TestEmptyText_ExpectSuccess()
        {
            // Arrange:
            string text = string.Empty;

            // Act:
            List<string> lines = text.SplitHumanReadableTextToLines(10, true);

            // Assert:
            Assert.IsNotNull(lines);
            Assert.AreEqual(0, lines.Count);
        }

        [TestMethod]
        public void SplitHumanReadableTextToLines_TestZeroMaxLength_ExpectSuccess()
        {
            // Arrange:
            string text = "bla bla bla bla bla bla bla bla";

            // Act:
            List<string> lines = text.SplitHumanReadableTextToLines(0, true);

            // Assert:
            Assert.IsNotNull(lines);
            Assert.AreEqual(1, lines.Count);
            Assert.AreEqual(text.NormalizeSpace(), lines[0]);
        }

        [TestMethod]
        public void SplitHumanReadableTextToLines_TestMinusMaxLength_ExpectSuccess()
        {
            // Arrange:
            string text = "bla bla bla bla bla bla bla bla";

            // Act:
            List<string> lines = text.SplitHumanReadableTextToLines(-7, true);

            // Assert:
            Assert.IsNotNull(lines);
            Assert.AreEqual(1, lines.Count);
            Assert.AreEqual(text.NormalizeSpace(), lines[0]);
        }

        [TestMethod]
        public void SplitHumanReadableTextToLines_TestNormalizeSpacesFalse_ExpectException()
        {
            // Arrange:
            string text = "\r bla \r\nbla \t";

            // Act:
            Assert.ThrowsExactly<NotImplementedException>(() => text.SplitHumanReadableTextToLines(-7, false));
        }
        #endregion Parameter Null, Zero and simmilar Tests

        [TestMethod]
        public void SplitHumanReadableTextToLines_Test01_ExpectSuccess()
        {
            // Arrange:
            string text = "bla bla bla bla bla bla bla bla";

            // Act:
            List<string> lines = text.SplitHumanReadableTextToLines(10, true);

            // Assert:
            Assert.IsNotNull(lines);
            Assert.AreEqual(4, lines.Count);
            Assert.AreEqual("bla bla", lines[0]);
            Assert.AreEqual("bla bla", lines[1]);
            Assert.AreEqual("bla bla", lines[2]);
            Assert.AreEqual("bla bla", lines[3]);
        }

        [TestMethod]
        public void SplitHumanReadableTextToLines_Test02_ExpectSuccess()
        {
            // Arrange:
            string text = "\r bla \r\nbla \t";

            // Act:
            List<string> lines = text.SplitHumanReadableTextToLines(10, true);

            // Assert:
            Assert.IsNotNull(lines);
            Assert.AreEqual(1, lines.Count);
            Assert.AreEqual("bla bla", lines[0]);
        }

        [TestMethod]
        public void SplitHumanReadableTextToLines_Test03_ExpectSuccess()
        {
            // Arrange:
            string text = "bla blub bl bli blaa hacha mamamamamiamaaaauuuuuuaa yiiiha!";

            // Act:
            List<string> lines = text.SplitHumanReadableTextToLines(10, true);

            // Assert:
            Assert.IsNotNull(lines);
            Assert.AreEqual(8, lines.Count);
            Assert.AreEqual("bla blub", lines[0]);
            Assert.AreEqual("bl bli", lines[1]);
            Assert.AreEqual("blaa", lines[2]);
            Assert.AreEqual("hacha", lines[3]);
            Assert.AreEqual("mamamamami", lines[4]);
            Assert.AreEqual("amaaaauuuu", lines[5]);
            Assert.AreEqual("uuaa", lines[6]);
            Assert.AreEqual("yiiiha!", lines[7]);
        }

        [TestMethod]
        public void SplitHumanReadableTextToLines_Test04_ExpectSuccess()
        {
            // Arrange:
            string text = "   \tbla\r \nblub\r\n bl\tbli\tblaa\t \t hacha mamamamamiamaaaauuuuuuaa yiiiha!\t\t\t\r\n";

            // Act:
            List<string> lines = text.SplitHumanReadableTextToLines(10, true);

            // Assert:
            Assert.IsNotNull(lines);
            Assert.AreEqual(8, lines.Count);
            Assert.AreEqual("bla blub", lines[0]);
            Assert.AreEqual("bl bli", lines[1]);
            Assert.AreEqual("blaa", lines[2]);
            Assert.AreEqual("hacha", lines[3]);
            Assert.AreEqual("mamamamami", lines[4]);
            Assert.AreEqual("amaaaauuuu", lines[5]);
            Assert.AreEqual("uuaa", lines[6]);
            Assert.AreEqual("yiiiha!", lines[7]);
        }

        [TestMethod]
        public void SplitHumanReadableTextToLines_Test05_ExpectSuccess()
        {
            // Arrange:
            string text = "Dieser Beleg dient der Chargendokumentation. Bitte bewahren Sie dieses Dokument auf, da Sie es im Falle eines Chargenrückrufes benötigen.";

            // Act:
            List<string> lines = text.SplitHumanReadableTextToLines(114, true);

            // Assert:
            Assert.IsNotNull(lines);
            Assert.AreEqual(2, lines.Count);
            Assert.AreEqual("Dieser Beleg dient der Chargendokumentation. Bitte bewahren Sie dieses Dokument auf, da Sie es im Falle eines", lines[0]);
            Assert.AreEqual("Chargenrückrufes benötigen.", lines[1]);
        }

        [TestMethod]
        public void SplitHumanReadableTextToLines_Test06_ExpectSuccess()
        {
            // Arrange:
            string text = "Ce document est utilisé pour la documentation des lots. Veuillez conserver ce document car vous en aurez besoin en cas de rappel de lots.";

            // Act:
            List<string> lines = text.SplitHumanReadableTextToLines(114, true);

            // Assert:
            Assert.IsNotNull(lines);
            Assert.AreEqual(2, lines.Count);
            Assert.AreEqual("Ce document est utilisé pour la documentation des lots. Veuillez conserver ce document car vous en aurez besoin", lines[0]);
            Assert.AreEqual("en cas de rappel de lots.", lines[1]);
        }
    }
}
