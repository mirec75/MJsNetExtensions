namespace MJsNetExtensions.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// String Extensions for use in relation with XML handling
    /// </summary>
    public static class XmlStringExtensions
    {
        #region Statics and Constants
        /// <summary>
        /// Regex to consolidate spaces. Find any single whitespace or a whitespace sequence.
        /// It matches also the non-breaking-space \u00a0 like a white space.
        /// </summary>
        public static readonly Regex ConsolidateSpaces = new(@"\s+", RegexOptions.Multiline | RegexOptions.Compiled);

        #endregion Statics and Constants

        #region API - Public Methods
        /// <summary>
        /// This method is the analogy to the XSL function "normalize-space".
        /// All whitespaces are replaced with space (blank). Then the leading, trailing, and repeating white spaces stripped.
        /// It handles also the non-breaking-space \u00a0 like a white space.
        /// The current string object itself is not modified.
        /// See e.g.: https://msdn.microsoft.com/en-us/library/ms256063(v=vs.120).aspx
        /// </summary>
        /// <param name="value">The (XML) string to normalize.</param>
        /// <returns>Returns the argument string with all white spaces replaced by blanks and then the leading, trailing, and repeating white spaces (blanks) stripped.</returns>
        public static string NormalizeSpace(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            //NOTE: any single whitespace or a whitespace sequence will be replaced by just 1 single blank
            string returnValue = ConsolidateSpaces.Replace(value, " ").Trim();
            return returnValue;
        }
        #endregion API - Public Methods
    }
}
