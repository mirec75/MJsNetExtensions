using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJsNetExtensions;
using System.Globalization;
#pragma warning disable CA1861

namespace MJsNetExtensionsTest
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void RemoveDiacriticsTest_ExpectSuccess()
        {
            // Arrange:
            string text = "Zzz § hehe äöüßÄÖÜ jahaa! Hihi ŠšŽžÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÑŃÒÓÔÕÖØÙÚÛÜÝÞàáâãäåæçèéêëìíîïðñńòóôõöøùúûüýýþÿƒăîâșțĂÎÂȘȚÐß Juhuu";

            // Act:
            string textWithNoDiacritics = text.RemoveDiacritics();

            // Assert:
            Assert.IsNotNull(textWithNoDiacritics);
            Assert.AreEqual("Zzz § hehe aeoeuessAeOeUe jahaa! Hihi SsZzAAAAAeAACEEEEIIIINNOOOOOeOUUUUeYBaaaaaeaaceeeeiiiidnnoooooeouuuueyybyƒaiastAIASTDss Juhuu", textWithNoDiacritics);
        }

        #region EscapeToCsvCell() Tests
        [TestMethod]
        public void EscapeToCsvCellTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string result = "".EscapeToCsvCell();

            // Assert:
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void EscapeToCsvCellTest2_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string result = " ".EscapeToCsvCell();

            // Assert:
            Assert.AreEqual(" ", result);
        }

        [TestMethod]
        public void EscapeToCsvCellTest3_ExpectSuccess()
        {
            // Arrange:
            string input = "\r\t";

            // Act:
            string result = input.EscapeToCsvCell();

            // Assert:
            Assert.AreEqual($"\"{input}\"", result);
        }

        [TestMethod]
        public void EscapeToCsvCellTest4_ExpectSuccess()
        {
            // Arrange:
            string input = "haha\r\nhihi";

            // Act:
            string result = input.EscapeToCsvCell();

            // Assert:
            Assert.AreEqual($"\"{input}\"", result);
        }

        [TestMethod]
        public void EscapeToCsvCellTest5_ExpectSuccess()
        {
            // Arrange:
            string input = "\t";

            // Act:
            string result = input.EscapeToCsvCell();

            // Assert:
            Assert.AreEqual($"{input}", result);
        }

        [TestMethod]
        public void EscapeToCsvCellTest6_ExpectSuccess()
        {
            // Arrange:
            string input = " haha \t hihi ";

            // Act:
            string result = input.EscapeToCsvCell();

            // Assert:
            Assert.AreEqual($"{input}", result);
        }
        #endregion EscapeToCsvCell() Tests

        #region ToCsvLine() Tests
        [TestMethod]
        public void ToCsvLineTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string result = new[] { "" }.ToCsvLine();

            // Assert:
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void ToCsvLineTest2_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string result = new[] { "", "" }.ToCsvLine(null);

            // Assert:
            Assert.AreEqual(";", result);
        }

        [TestMethod]
        public void ToCsvLineTest3_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string result = new[] { " ", "\t" }.ToCsvLine(" \t\r\n");

            // Assert:
            Assert.AreEqual(" ;\t", result);
        }

        [TestMethod]
        public void ToCsvLineTest4_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string result = new[] { "haha", "hihi" }.ToCsvLine(",");

            // Assert:
            Assert.AreEqual("haha,hihi", result);
        }

        [TestMethod]
        public void ToCsvLineTest5_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string result = new[] { "haha", "hihi" }.ToCsvLine("BRUM");

            // Assert:
            Assert.AreEqual("hahaBRUMhihi", result);
        }

        [TestMethod]
        public void ToCsvLineTest6_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string result = Array.Empty<string>().ToCsvLine();

            // Assert:
            Assert.AreEqual("", result);
        }


        [TestMethod]
        public void ToCsvLineTest7_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string[] input = null;
            string result = input.ToCsvLine();

            // Assert:
            Assert.AreEqual("", result);
        }
        #endregion ToCsvLine() Tests



        #region TryFormat Tests - using the Default string.Format variant (without CultureInfo) 
        [TestMethod]
        public void TryFormatDefaultTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string result = "{0}: {1} hehe {2}".TryFormat(7, "foo", 13);

            // Assert:
            Assert.AreEqual("7: foo hehe 13", result);
        }

        [TestMethod]
        public void TryFormatDefaultTest2_ExpectFallback()
        {
            // Arrange:
            string formatString = null;

            // Act:
            string result = formatString.TryFormat(7, "foo", 13);

            // Assert:
            Assert.AreEqual("Error: Format is null! Args: [7, foo, 13]", result);
        }

        [TestMethod]
        public void TryFormatDefaultTest3_ExpectSuccess()
        {
            // Arrange:
            string formatString = "";

            // Act:
            string result = formatString.TryFormat(7, "foo", 13);

            // Assert:
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void TryFormatDefaultTest4_ExpectFallback()
        {
            // Arrange:
            string formatString = "{";

            // Act:
            string result = formatString.TryFormat(7, "foo", 13);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("Error: String Format Exception: ", StringComparison.Ordinal));
            Assert.IsTrue(result.EndsWith(" Format: {, Args: [7, foo, 13]", StringComparison.Ordinal));
        }

        [TestMethod]
        public void TryFormatDefaultTest5_ExpectFallback()
        {
            // Arrange:
            string formatString = "hi {88} lo!";

            // Act:
            string result = formatString.TryFormat(7, "foo", 13);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("Error: String Format Exception: ", StringComparison.Ordinal));
            Assert.IsTrue(result.EndsWith(" Format: hi {88} lo!, Args: [7, foo, 13]", StringComparison.Ordinal));
        }

        [TestMethod]
        public void TryFormatDefaultTest6_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi {1}: {0} * {2} = whatever...";

            // Act:
            string result = formatString.TryFormat(7, "foo", 13);

            // Assert:
            Assert.AreEqual("Hi foo: 7 * 13 = whatever...", result);
        }

        [TestMethod]
        public void TryFormatDefaultTest7_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi {1}: {0} * {2} = whatever...";
            string param1 = null;

            // Act:
            string result = formatString.TryFormat(param1, null, 13);

            // Assert:
            Assert.AreEqual("Hi :  * 13 = whatever...", result);
        }

        [TestMethod]
        public void TryFormatDefaultTest8_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi {1}: {0} * {2} = whatever...";
            object param1 = null;

            // Act:
            string result = formatString.TryFormat(param1, null, 13);

            // Assert:
            Assert.AreEqual("Hi :  * 13 = whatever...", result);
        }

        [TestMethod]
        public void TryFormatDefaultTest9_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi!";

            // Act:
            string result = formatString.TryFormat();

            // Assert:
            Assert.AreEqual("Hi!", result);
        }

        [TestMethod]
        public void TryFormatDefaultTest10_ExpectFallback()
        {
            // Arrange:
            string formatString = "hi {88} lo!";

            // Act:
            string result = formatString.TryFormat();

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("Error: String Format Exception: ", StringComparison.Ordinal));
            Assert.IsTrue(result.EndsWith(" Format: hi {88} lo!, Args: []", StringComparison.Ordinal));
        }
        #endregion TryFormat Tests - using the Default string.Format variant (without CultureInfo) 

        #region TryFormat Tests - using the CurrentCulture string.Format variant (with CurrentCulture parameter)
        [TestMethod]
        public void TryFormatCurrentCultureTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string result = "{0}: {1} hehe {2}".TryFormat(CultureInfo.CurrentCulture, 7, "foo", 13);

            // Assert:
            Assert.AreEqual("7: foo hehe 13", result);
        }

        [TestMethod]
        public void TryFormatCurrentCultureTest2_ExpectFallback()
        {
            // Arrange:
            string formatString = null;

            // Act:
            string result = formatString.TryFormat(CultureInfo.CurrentCulture, 7, "foo", 13);

            // Assert:
            Assert.AreEqual("Error: Format is null! Args: [7, foo, 13]", result);
        }

        [TestMethod]
        public void TryFormatCurrentCultureTest3_ExpectSuccess()
        {
            // Arrange:
            string formatString = "";

            // Act:
            string result = formatString.TryFormat(CultureInfo.CurrentCulture, 7, "foo", 13);

            // Assert:
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void TryFormatCurrentCultureTest4_ExpectFallback()
        {
            // Arrange:
            string formatString = "{";

            // Act:
            string result = formatString.TryFormat(CultureInfo.CurrentCulture, 7, "foo", 13);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("Error: String Format Exception: ", StringComparison.Ordinal));
            Assert.IsTrue(result.EndsWith(" Format: {, Args: [7, foo, 13]", StringComparison.Ordinal));
        }

        [TestMethod]
        public void TryFormatCurrentCultureTest5_ExpectFallback()
        {
            // Arrange:
            string formatString = "hi {88} lo!";

            // Act:
            string result = formatString.TryFormat(CultureInfo.CurrentCulture, 7, "foo", 13);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("Error: String Format Exception: ", StringComparison.Ordinal));
            Assert.IsTrue(result.EndsWith(" Format: hi {88} lo!, Args: [7, foo, 13]", StringComparison.Ordinal));
        }

        [TestMethod]
        public void TryFormatCurrentCultureTest6_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi {1}: {0} * {2} = whatever...";

            // Act:
            string result = formatString.TryFormat(CultureInfo.CurrentCulture, 7, "foo", 13);

            // Assert:
            Assert.AreEqual("Hi foo: 7 * 13 = whatever...", result);
        }

        [TestMethod]
        public void TryFormatCurrentCultureTest7_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi {1}: {0} * {2} = whatever...";
            string param1 = null;

            // Act:
            string result = formatString.TryFormat(CultureInfo.CurrentCulture, param1, null, 13);

            // Assert:
            Assert.AreEqual("Hi :  * 13 = whatever...", result);
        }

        [TestMethod]
        public void TryFormatCurrentCultureTest8_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi {1}: {0} * {2} = whatever...";
            object param1 = null;

            // Act:
            string result = formatString.TryFormat(CultureInfo.CurrentCulture, param1, null, 13);

            // Assert:
            Assert.AreEqual("Hi :  * 13 = whatever...", result);
        }

        [TestMethod]
        public void TryFormatCurrentCultureTest9_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi!";

            // Act:
            string result = formatString.TryFormat(CultureInfo.CurrentCulture);

            // Assert:
            Assert.AreEqual("Hi!", result);
        }

        [TestMethod]
        public void TryFormatCurrentCultureTest10_ExpectFallback()
        {
            // Arrange:
            string formatString = "hi {88} lo!";

            // Act:
            string result = formatString.TryFormat(CultureInfo.CurrentCulture);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("Error: String Format Exception: ", StringComparison.Ordinal));
            Assert.IsTrue(result.EndsWith(" Format: hi {88} lo!, Args: []", StringComparison.Ordinal));
        }
        #endregion TryFormat Tests - using the CurrentCulture string.Format variant (with CurrentCulture parameter)

        #region TryFormatInvariant Tests - using the Invariant string.Format variant (with CultureInfo.InvariantCulture
        [TestMethod]
        public void TryFormatInvariantTest1_ExpectSuccess()
        {
            // Arrange:
            // Act:
            string result = "{0}: {1} hehe {2}".TryFormatInvariant(7, "foo", 13);

            // Assert:
            Assert.AreEqual("7: foo hehe 13", result);
        }

        [TestMethod]
        public void TryFormatInvariantTest2_ExpectFallback()
        {
            // Arrange:
            string formatString = null;

            // Act:
            string result = formatString.TryFormatInvariant(7, "foo", 13);

            // Assert:
            Assert.AreEqual("Error: Format is null! Args: [7, foo, 13]", result);
        }

        [TestMethod]
        public void TryFormatInvariantTest3_ExpectSuccess()
        {
            // Arrange:
            string formatString = "";

            // Act:
            string result = formatString.TryFormatInvariant(7, "foo", 13);

            // Assert:
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void TryFormatInvariantTest4_ExpectFallback()
        {
            // Arrange:
            string formatString = "{";

            // Act:
            string result = formatString.TryFormatInvariant(7, "foo", 13);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("Error: String Format Exception: ", StringComparison.Ordinal));
            Assert.IsTrue(result.EndsWith(" Format: {, Args: [7, foo, 13]", StringComparison.Ordinal));
        }

        [TestMethod]
        public void TryFormatInvariantTest5_ExpectFallback()
        {
            // Arrange:
            string formatString = "hi {88} lo!";

            // Act:
            string result = formatString.TryFormatInvariant(7, "foo", 13);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("Error: String Format Exception: ", StringComparison.Ordinal));
            Assert.IsTrue(result.EndsWith(" Format: hi {88} lo!, Args: [7, foo, 13]", StringComparison.Ordinal));
        }

        [TestMethod]
        public void TryFormatInvariantTest6_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi {1}: {0} * {2} = whatever...";

            // Act:
            string result = formatString.TryFormatInvariant(7, "foo", 13);

            // Assert:
            Assert.AreEqual("Hi foo: 7 * 13 = whatever...", result);
        }

        [TestMethod]
        public void TryFormatInvariantTest7_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi {1}: {0} * {2} = whatever...";
            string param1 = null;

            // Act:
            string result = formatString.TryFormatInvariant(param1, null, 13);

            // Assert:
            Assert.AreEqual("Hi :  * 13 = whatever...", result);
        }

        [TestMethod]
        public void TryFormatInvariantTest8_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi {1}: {0} * {2} = whatever...";
            object param1 = null;

            // Act:
            string result = formatString.TryFormatInvariant(param1, null, 13);

            // Assert:
            Assert.AreEqual("Hi :  * 13 = whatever...", result);
        }

        [TestMethod]
        public void TryFormatInvariantTest9_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi!";

            // Act:
            string result = formatString.TryFormatInvariant();

            // Assert:
            Assert.AreEqual("Hi!", result);
        }

        [TestMethod]
        public void TryFormatInvariantTest10_ExpectFallback()
        {
            // Arrange:
            string formatString = "hi {88} lo!";

            // Act:
            string result = formatString.TryFormatInvariant();

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("Error: String Format Exception: ", StringComparison.Ordinal));
            Assert.IsTrue(result.EndsWith(" Format: hi {88} lo!, Args: []", StringComparison.Ordinal));
        }
        #endregion TryFormatInvariant Tests - using the Invariant string.Format variant (with CultureInfo.InvariantCulture) 

        #region TryFormat Tests - unusual cases
        [TestMethod]
        public void TryFormatNull1stParamTest1_ExpectFallback()
        {
            // Arrange:
            string formatString = "Hi {1}: {0} * {2} = whatever...";

            // Act:

            //NOTE: !! the "null" is misuderstood as a CultureInfo, thus the call invokes: !!
            //      public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
            string result = formatString.TryFormat(null, "foo", 13);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("Error: String Format Exception: ", StringComparison.Ordinal));
            Assert.IsTrue(result.EndsWith(" Format: Hi {1}: {0} * {2} = whatever..., Args: [foo, 13]", StringComparison.Ordinal));
        }

        [TestMethod]
        public void TryFormatNull1stParamTest2_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi!";

            // Act:

            //NOTE: !! the "null" is misuderstood as a CultureInfo, thus the call invokes: !!
            //      public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
            string result = formatString.TryFormat((object)null);

            // Assert:
            Assert.AreEqual("Hi!", result);
        }

        [TestMethod]
        public void TryFormatNull1stParamTest3_ExpectSuccess()
        {
            // Arrange:
            string formatString = "Hi!";

            // Act:

            //NOTE: !! the "null" is misuderstood as a CultureInfo, thus the call invokes: !!
            //      public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
            string result = formatString.TryFormat((CultureInfo)null);

            // Assert:
            Assert.AreEqual("Hi!", result);
        }
        #endregion TryFormat Tests - unusual cases

        #region File Name and Path Invalid Chars Replacing Tests

        [TestMethod]
        public void ReplaceFileNameAndPathInvalidCharsParamsTest1_ExpectSuccess()
        {
            // Arrange:
            string input = null;
            string replaceEachCharWith = null;

            // Act:

            //NOTE: !! the "null" is misuderstood as a CultureInfo, thus the call invokes: !!
            //      public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
            string result = input.ReplaceFileNameAndPathInvalidChars(replaceEachCharWith);

            // Assert:
            Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void ReplaceFileNameAndPathInvalidCharsParamsTest2_ExpectSuccess()
        {
            // Arrange:
            string input = "";
            string replaceEachCharWith = null;

            // Act:

            //NOTE: !! the "null" is misuderstood as a CultureInfo, thus the call invokes: !!
            //      public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
            string result = input.ReplaceFileNameAndPathInvalidChars(replaceEachCharWith);

            // Assert:
            Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void ReplaceFileNameAndPathInvalidCharsParamsTest3_ExpectSuccess()
        {
            // Arrange:
            string input = " \t\r\n ";
            string replaceEachCharWith = null;

            // Act:

            //NOTE: !! the "null" is misuderstood as a CultureInfo, thus the call invokes: !!
            //      public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
            string result = input.ReplaceFileNameAndPathInvalidChars(replaceEachCharWith);

            // Assert:
            Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void ReplaceFileNameAndPathInvalidCharsParamsTest4_ExpectSuccess()
        {
            // Arrange:
            string input = " \t\r\n hehe<>";
            string replaceEachCharWith = null;

            // Act:

            //NOTE: !! the "null" is misuderstood as a CultureInfo, thus the call invokes: !!
            //      public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
            string result = input.ReplaceFileNameAndPathInvalidChars(replaceEachCharWith);

            // Assert:
            Assert.AreEqual("  hehe", result);
        }

        [TestMethod]
        public void ReplaceFileNameAndPathInvalidCharsParamsTest5_ExpectSuccess()
        {
            // Arrange:
            string input = " \t\r\n hehe<>";
            string replaceEachCharWith = string.Empty;

            // Act:

            //NOTE: !! the "null" is misuderstood as a CultureInfo, thus the call invokes: !!
            //      public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
            string result = input.ReplaceFileNameAndPathInvalidChars(replaceEachCharWith);

            // Assert:
            Assert.AreEqual("  hehe", result);
        }

        [TestMethod]
        public void ReplaceFileNameAndPathInvalidCharsTest1_ExpectSuccess()
        {
            // Arrange:
            string input = "Hi!";
            string replaceEachCharWith = "_";

            // Act:

            //NOTE: !! the "null" is misuderstood as a CultureInfo, thus the call invokes: !!
            //      public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
            string result = input.ReplaceFileNameAndPathInvalidChars(replaceEachCharWith);

            // Assert:
            Assert.AreEqual("Hi!", result);
        }

        [TestMethod]
        public void ReplaceFileNameAndPathInvalidCharsTest2_ExpectSuccess()
        {
            // Arrange:
            string replaceEachCharWith = "_";
            string input    = "Hi°!\"§$%&/()=?!`ü+[]{}/ hehe";
            string expected = "Hi°!_§$%&_()=_!`ü+[]{}_ hehe";

            // Act:

            //NOTE: !! the "null" is misuderstood as a CultureInfo, thus the call invokes: !!
            //      public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
            string result = input.ReplaceFileNameAndPathInvalidChars(replaceEachCharWith);

            // Assert:
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ReplaceFileNameAndPathInvalidCharsTest3_ExpectSuccess()
        {
            // Arrange:
            string replaceEachCharWith = "_";
            string input = StringExtensions.InvalidFileNameAndPathChars;
            string expected = "__________________________________________________________________________";

            // Act:

            //NOTE: !! the "null" is misuderstood as a CultureInfo, thus the call invokes: !!
            //      public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
            string result = input.ReplaceFileNameAndPathInvalidChars(replaceEachCharWith);

            // Assert:
            Assert.AreEqual(expected, result);
        }
        #endregion File Name and Path Invalid Chars Replacing Tests
    }
}
