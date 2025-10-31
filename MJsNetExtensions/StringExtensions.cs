#pragma warning disable S125
namespace MJsNetExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Buffers; // Add this for SearchValues
    using Xml;


    /// <summary>
    /// String and StringBuilder Extensions
    /// </summary>
    public static class StringExtensions
    {
        #region String and StringBuilder Extensions

        private static readonly char[] ZeroCharArray = "0".ToCharArray(); // or: new[] { '0' }

        private static Dictionary<char, char> SelectedDiacriticsToPlainAsciiLetters = new()
        {
            ['Æ'] = 'A',
            ['æ'] = 'a',
            ['Ø'] = 'O',
            ['ø'] = 'o',
            ['Þ'] = 'B',
            ['þ'] = 'b',
            ['Ð'] = 'D',
            ['ð'] = 'd',
        };

        /// <summary>
        /// String containing all invalid chars as defined by: <see cref="Path.GetInvalidFileNameChars()"/> 
        /// and file system path invalid chars as defined by: <see cref="Path.GetInvalidPathChars()"/>
        /// </summary>
        public static readonly string InvalidFileNameAndPathChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

        /// <summary>
        /// <see cref="Regex"/> for replacing all <see cref="InvalidFileNameAndPathChars"/>.
        /// </summary>
        public static readonly Regex InvalidFileNameAndPathCharsReplacer =
            new(
                string.Format(CultureInfo.InvariantCulture, "[{0}]", Regex.Escape(InvalidFileNameAndPathChars)),
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled
                );

        private static readonly SearchValues<char> CsvEscapeCharsSearchValues = SearchValues.Create(";\"\r\n");

        #endregion Statics and Constants

        #region String Extensions


        /// <summary>
        /// Trim Leading Zeros, e.g. make from "0012345" a result "12345"
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string TrimLeadingZeros(this string text)
        {
            string trimmedText = null;

            if (text != null)
            {
                trimmedText = text.TrimStart(ZeroCharArray);
            }

            return trimmedText;
        }

        /// <summary>
        /// Convenience method wrapping the call "container.IndexOf(contentParticle, comparisonType) >= 0" and check of <paramref name="container"/>.
        /// </summary>
        /// <param name="container">The "container" string, which shall be checked, if it contains <paramref name="contentParticle"/>.</param>
        /// <param name="contentParticle">The string particle which shall be seeked in <paramref name="container"/>.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>True if <paramref name="container"/> contains <paramref name="contentParticle"/>, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="contentParticle"/> is null</exception>
        /// <exception cref="ArgumentException">if comparisonType is not a valid <seealso cref="System.StringComparison"/> value.</exception>
        public static bool Contains(this string container, string contentParticle, StringComparison comparisonType)
        {
            if (container == null)
            {
                if (contentParticle == null)
                {
                    return true;
                }

                return false;
            }

            if (contentParticle == null)
            {
                return false;
            }

            return container.Contains(contentParticle, comparisonType);
        }

        /// <summary>
        /// Turn a string into an escaped CSV cell output if necessary
        /// </summary>
        /// <param name="text">Text to convert to correct escaped CSV format</param>
        /// <returns>The escaped CSV cell text if the input text contains one of the chars ";\"\r\n", else the input text is returned.</returns>
        public static string EscapeToCsvCell(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            int positionOfCharToEscape = text.AsSpan().IndexOfAny(CsvEscapeCharsSearchValues);
            bool mustQuote = positionOfCharToEscape > -1;
            //mustQuote |= string.IsNullOrWhiteSpace(text); // quoting blanks, tabs etc...

            if (!mustQuote)
            {
                return text;
            }
            //else == mustQuote

            StringBuilder sb = new StringBuilder();

            sb.Append('\"');
            sb.Append(text.Replace("\"", "\"\"", StringComparison.InvariantCulture));
            sb.Append('\"');

            return sb.ToString();
        }

        /// <summary>
        /// Converts a list of objects to CSV line with a default separator ";"
        /// </summary>
        /// <param name="particles">The list of objects to serialize</param>
        /// <returns>The CSV line</returns>
        public static string ToCsvLine(this IEnumerable<object> particles)
        {
            return ToCsvLine(particles, ";");
        }

        /// <summary>
        /// Converts a list of objects to CSV line with a default separator ";"
        /// </summary>
        /// <param name="particles">The list of objects to serialize</param>
        /// <param name="separator">Optional separator. If null empty or whitespace, then the default separator ";" is used.</param>
        /// <returns>The CSV line</returns>
        public static string ToCsvLine(this IEnumerable<object> particles, string separator)
        {
            // handling of optional param:
            if (particles == null)
            {
                return string.Empty;
            }

            // handling of optional param:
            if (string.IsNullOrWhiteSpace(separator))
            {
                separator = ";";
            }

            // DO serialize:
            var normalizedCsvStrings = particles
                .Select(it => it?.ToString().EscapeToCsvCell())
                .ToList()
                ;

            string ret = string.Join(separator, normalizedCsvStrings);
            return ret;
        }


        /// <summary>
        /// Escape string for further formatting, i.e. doubles each '{' and '}'.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string EscapeForFurtherFormatting(this string text)
        {
            if (text == null)
            {
                return text;
            }

            string result = text.Replace("{", "{{", StringComparison.InvariantCulture);
            result = result.Replace("}", "}}", StringComparison.InvariantCulture);

            return result;
        }

        /// <summary>
        /// Replace Strings is convenience method for a case sensitive or insensitive string replacements, wrapping <seealso cref="Regex.Replace(string, string, string, RegexOptions)"/>.
        /// </summary>
        /// <param name="content">The "container" string, where the replacements have to be done.</param>
        /// <param name="contentReplacementsDictionary">the key / value dictionary for the replacements</param>
        /// <param name="caseSensitive">Flag indicating if the string replacements shall search case sensitive (if true) or insensitive (if fasle) for the keys of the <paramref name="contentReplacementsDictionary"/>.</param>
        /// <returns></returns>
        public static string ReplaceStrings(this string content, IDictionary<string, string> contentReplacementsDictionary, bool caseSensitive)
        {
            if (string.IsNullOrWhiteSpace(content) ||
                contentReplacementsDictionary == null ||
                !contentReplacementsDictionary.Any()
                )
            {
                return content;
            }

            string result = content;

            foreach (var kvp in contentReplacementsDictionary)
            {
                //result = result.Replace(kvp.Key, kvp.Value); //NOTE: this is case sensitive

                result = Regex.Replace(result, kvp.Key, kvp.Value, (caseSensitive) ? RegexOptions.None : RegexOptions.IgnoreCase);
            }

            return result;
        }

        /// <summary>
        /// Exception free <seealso cref="string.Format(string, object[])"/> alternative.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns>The formatted string or string describing the error.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object[])", Justification = "MJ: explicitely supporting the default string.Format() method, which is based implicitley on current user's locale settings.")]
        public static string TryFormat(this string format, params object[] args)
        {
            object[] args2 = args ?? [null,]; // correcting .Net error if called from Throw.If(true, "haha", "msg1 {0}", null);

            return TryFormatInner(
                () => string.Format(format, args2),
                format,
                args2
                );
        }

        /// <summary>
        /// Exception free <seealso cref="string.Format(IFormatProvider, string, object[])"/> alternative.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="cultureInfo"></param>
        /// <param name="args"></param>
        /// <returns>The formatted string or string describing the error.</returns>
        public static string TryFormat(this string format, CultureInfo cultureInfo, params object[] args)
        {
            object[] args2 = args ?? [null,]; // correcting .Net error if called from Throw.If(true, "haha", "msg1 {0}", null);

            return TryFormatInner(
                () => string.Format(cultureInfo ?? CultureInfo.CurrentCulture, format, args2),
                format,
                args2
                );
        }

        /// <summary>
        /// Exception free <seealso cref="string.Format(IFormatProvider, string, object[])"/> alternative.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns>The formated string or string describing the error.</returns>
        public static string TryFormatInvariant(this string format, params object[] args)
        {
            object[] args2 = args ?? [null,]; // correcting .Net error if called from Throw.If(true, "haha", "msg1 {0}", null);

            return TryFormatInner(
                () => string.Format(CultureInfo.InvariantCulture, format, args2),
                format,
                args2
                );
        }

        private static string TryFormatInner(Func<string> doFormat, string format, params object[] args)
        {
            string resultString;

            if (format == null)
            {
                resultString = $"Error: Format is null! Args: [{string.Join(", ", args ?? [])}]";
                return resultString;
            }

            try
            {
                resultString = doFormat();
            }
            catch (FormatException ex)
            {
                resultString = $"Error: String Format Exception: {ex.Message} Format: {format}, Args: [{string.Join(", ", args ?? [])}]";
            }

            return resultString;
        }

        /// <summary>
        /// A try of implementing a helper which optionally 1st normalizes space in text 
        /// and then tries to split one the one line text to several lines according <paramref name="maxLineLength"/>.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLineLength"></param>
        /// <param name="normalizeSpaces"></param>
        /// <returns></returns>
        public static List<string> SplitHumanReadableTextToLines(this string text, int maxLineLength, bool normalizeSpaces)
        {
            List<string> lines = new List<string>();

            if (string.IsNullOrWhiteSpace(text))
            {
                return lines;
            }

            if (normalizeSpaces)
            {
                text = text.NormalizeSpace();
            }
            else
            {
                throw new NotImplementedException($"{nameof(normalizeSpaces)} == false is not implemented yet!");
            }

            if (maxLineLength < 1)
            {
                lines.Add(text);
            }
            else
            {
                // do splitting into lines:
                string textRest = text;

                while (textRest.Length > maxLineLength)
                {
                    int positionOfLastSeparatorInLine = textRest.LastIndexOf(' ', maxLineLength - 1);
                    string line;
                    if (positionOfLastSeparatorInLine < 0)
                    {
                        // just cut the line without a separating space at its maxLineLength
                        line = textRest.Substring(0, maxLineLength);

                        // just continue to take the rest starting at the next char. There was no separating space:
                        positionOfLastSeparatorInLine = maxLineLength;
                    }
                    else
                    {
                        line = textRest.Substring(0, positionOfLastSeparatorInLine);

                        // procede past separator (space)
                        positionOfLastSeparatorInLine++;
                    }

                    lines.Add(line);
                    textRest = textRest.Substring(positionOfLastSeparatorInLine);
                }

                lines.Add(textRest);
            }

            return lines;
        }

        /// <summary>
        /// Helper method. See:
        /// How do I remove diacritics (accents) from a string in .NET?
        /// https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
        /// and:
        /// Convert accented characters to their plain ascii equivalents
        /// https://stackoverflow.com/questions/10054818/convert-accented-characters-to-their-plain-ascii-equivalents
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            // double letter replacements:
            string outputText = text.Replace("Ä", "Ae", StringComparison.InvariantCulture);
            outputText = outputText.Replace("Ö", "Oe", StringComparison.InvariantCulture);
            outputText = outputText.Replace("Ü", "Ue", StringComparison.InvariantCulture);
            outputText = outputText.Replace("ä", "ae", StringComparison.InvariantCulture);
            outputText = outputText.Replace("ö", "oe", StringComparison.InvariantCulture);
            outputText = outputText.Replace("ü", "ue", StringComparison.InvariantCulture);
            outputText = outputText.Replace("ß", "ss", StringComparison.InvariantCulture);


            // How do I remove diacritics (accents) from a string in .NET?
            // https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
            // and:
            // Convert accented characters to their plain ascii equivalents
            // https://stackoverflow.com/questions/10054818/convert-accented-characters-to-their-plain-ascii-equivalents

            var normalizedString = outputText.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                if (SelectedDiacriticsToPlainAsciiLetters.TryGetValue(c, out var replacement))
                {
                    stringBuilder.Append(replacement);
                    continue;
                }

                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            outputText = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            return outputText;
        }

        #endregion String Extensions

        #region StringBuilder Extensions

        /// <summary>
        /// Smart extension concantenating> <seealso cref="StringBuilder.AppendFormat(IFormatProvider, string, object[])"/> and <seealso cref="StringBuilder.Append(char)"/> == '\n'.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void AppendFormatLine(this StringBuilder stringBuilder, string format, params object[] args)
        {
            if (stringBuilder != null && format != null)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, format, args);
                stringBuilder.Append('\n');
            }
        }

        #endregion StringBuilder Extensions

        #region File Name and Path Invalid Chars Replacing

        /// <summary>
        /// Replace file name invalid chars as defined by: <see cref="Path.GetInvalidFileNameChars()"/> 
        /// and file system path invalid chars as defined by: <see cref="Path.GetInvalidPathChars()"/>
        /// in the <paramref name="input"/> with a desired replacement string: <paramref name="replaceEachCharWith"/>. 
        /// NOTE: the <paramref name="replaceEachCharWith"/> is neither checked, nor corrected, if it contains any invalid chars. So the result may contain file name or path invalid chars introduced by <paramref name="replaceEachCharWith"/>,
        /// or may even colapse to an empty oj just whitespace string if replacing the invalid chars with empty string in <paramref name="replaceEachCharWith"/>.
        /// NOTE: if <paramref name="input"/> is null or white space, no exception will be thrown and the <paramref name="input"/> is simply returned.
        /// </summary>
        /// <param name="input">The string to replace all file name or path invalid chars in.</param>
        /// <param name="replaceEachCharWith">The replacement string for each single invalid char occurrence. Can be empty. If null, it will internally be replaced by <see cref="string.Empty"/> to remove all invalid chars from <paramref name="input"/>.</param>
        /// <returns>The result of replacement of all file name and path invalid chars in <paramref name="input"/> with <paramref name="replaceEachCharWith"/>.</returns>
        public static string ReplaceFileNameAndPathInvalidChars(this string input, string replaceEachCharWith)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            // inspired by: https://stackoverflow.com/questions/146134/how-to-remove-illegal-characters-from-path-and-filenames
            string output = InvalidFileNameAndPathCharsReplacer.Replace(input, replaceEachCharWith ?? string.Empty);

            return output;
        }

        #endregion File Name and Path Invalid Chars Replacing
    }
}
