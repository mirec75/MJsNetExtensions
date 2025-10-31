namespace MJsNetExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;


    /// <summary>
    /// Embedded Resource Helper - inspired by: 
    /// https://jonlabelle.com/snippets/view/csharp/embedded-resource-helper
    /// </summary>
    public static class EmbeddedResourceHelper
    {
        #region Statics and Consts

        private static readonly Regex InvalidResourceFilePathCharsReplacer =
            new(
                string.Format(CultureInfo.InvariantCulture, @"[{0} =$\[\](){{}}`°~!@^\-+;,']", Regex.Escape(StringExtensions.InvalidFileNameAndPathChars)),
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled
                );

        #endregion Statics and Consts

        #region API - Public Methods

        /// <summary>
        /// Formats the <paramref name="resourceName"/>, which is a resource "relavive" path, relative to the <paramref name="assembly"/>'s namespace.
        /// It can contain backslashes '\' and slashes '/' which will be automatically replaced by a dot '.' each, or blanks and hyphens '-', which will be replaced by underscores "_".
        /// </summary>
        /// <param name="assembly">the assebmly, from which the embedded resource is to be read.</param>
        /// <param name="resourceName">The resource "relavive" path, relative to the <paramref name="assembly"/>'s namespace.
        /// It can contain backslashes '\' and slashes '/' which will be automatically replaced by a dot '.' each, or blanks and hyphens '-', which will be replaced by underscores "_".</param>
        /// <returns>The formatted <paramref name="resourceName"/> aaa.</returns>
        public static string FormatResourceName(Assembly assembly, string resourceName)
        {
            Throw.IfNull(assembly, nameof(assembly));
            Throw.IfNullOrWhiteSpace(resourceName, nameof(resourceName));

            resourceName = resourceName.Replace("/", "\\", StringComparison.InvariantCulture);

            string path = Path.GetDirectoryName(resourceName);
            string fileName = Path.GetFileName(resourceName);

            Throw.InvalidOperationIf(string.IsNullOrWhiteSpace(fileName), "The parameter {0} value: '{1}' results in empty file name.", nameof(resourceName), resourceName);

            string result = assembly.GetName().Name + ".";
            if (!string.IsNullOrWhiteSpace(path))
            {
                path = path
                    .Replace("\\", ".", StringComparison.InvariantCulture)
                    .Replace("/", ".", StringComparison.InvariantCulture)
                    ;

                result +=
                    InvalidResourceFilePathCharsReplacer.Replace(path, "_")
                    + "."
                    ;
            }

            result += fileName;

            return result;
        }

        /// <summary>
        /// Get embedded resource <paramref name="resourceName"/> content as a <see cref="string"/> from the <see cref="Assembly.GetCallingAssembly()"/>.
        /// </summary>
        /// <param name="resourceName">The resource "relavive" path, relative to the <see cref="Assembly.GetCallingAssembly()"/>'s namespace.
        /// It can contain backslashes '\' and slashes '/' which will be automatically replaced by a dot '.' each, or blanks and hyphens '-', which will be replaced by underscores "_".</param>
        /// <returns><see cref="string"/> content of the <see cref="Assembly.GetCallingAssembly()"/>'s embedded resource named by <paramref name="resourceName"/>.</returns>
        public static string GetEmbeddedResource(string resourceName)
        {
            return GetEmbeddedResource(resourceName, Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Get embedded resource <paramref name="resourceName"/> content as a <see cref="string"/> from the <paramref name="assembly"/>.
        /// </summary>
        /// <param name="resourceName">The resource "relavive" path, relative to the <paramref name="assembly"/>'s namespace.
        /// It can contain backslashes '\' and slashes '/' which will be automatically replaced by a dot '.' each, or blanks and hyphens '-', which will be replaced by underscores "_".</param>
        /// <param name="assembly">the assebmly, from which the embedded resource is to be read.</param>
        /// <returns><see cref="string"/> content of the <paramref name="assembly"/>'s embedded resource named by <paramref name="resourceName"/>.</returns>
        public static string GetEmbeddedResource(string resourceName, Assembly assembly)
        {
            return GetEmbeddedResource(resourceName, assembly, GetStringFromStream);
        }


        /// <summary>
        /// Get embedded resource <paramref name="resourceName"/> content as a <see cref="byte[]"/> from the <see cref="Assembly.GetCallingAssembly()"/>.
        /// </summary>
        /// <param name="resourceName">The resource "relavive" path, relative to the <see cref="Assembly.GetCallingAssembly()"/>'s namespace.
        /// It can contain backslashes '\' and slashes '/' which will be automatically replaced by a dot '.' each, or blanks and hyphens '-', which will be replaced by underscores "_".</param>
        /// <returns><see cref="byte[]"/> content of the <see cref="Assembly.GetCallingAssembly()"/>'s embedded resource named by <paramref name="resourceName"/>.</returns>
        public static byte[] GetEmbeddedResourceAsBytes(string resourceName)
        {
            return GetEmbeddedResourceAsBytes(resourceName, Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Get embedded resource <paramref name="resourceName"/> content as a <see cref="string"/> from the <paramref name="assembly"/>.
        /// </summary>
        /// <param name="resourceName">The resource "relavive" path, relative to the <paramref name="assembly"/>'s namespace.
        /// It can contain backslashes '\' and slashes '/' which will be automatically replaced by a dot '.' each, or blanks and hyphens '-', which will be replaced by underscores "_".</param>
        /// <param name="assembly">the assebmly, from which the embedded resource is to be read.</param>
        /// <returns><see cref="string"/> content of the <paramref name="assembly"/>'s embedded resource named by <paramref name="resourceName"/>.</returns>
        public static byte[] GetEmbeddedResourceAsBytes(string resourceName, Assembly assembly)
        {
            return GetEmbeddedResource(resourceName, assembly, GetBytesFromStream);
        }

        #endregion API - Public Methods

        #region Private Methods

        private static T GetEmbeddedResource<T>(string resourceName, Assembly assembly, Func<Stream, T> getResut)
        {
            Throw.IfNull(assembly, nameof(assembly));

            resourceName = FormatResourceName(assembly, resourceName);
            using Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                return default(T);
            }

            return getResut(resourceStream);
        }

        private static string GetStringFromStream(Stream stream)
        {
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private static byte[] GetBytesFromStream(Stream stream)
        {
            using MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }
        #endregion Private Methods
    }
}
