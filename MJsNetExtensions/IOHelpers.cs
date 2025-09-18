namespace MJsNetExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Public Static Helpers
    /// </summary>
    public static class IOHelpers
    {
        #region Directory thread local cache
        /// <summary>
        /// This Directory hash is destroyed first upon process termination. It coult be just filled up.
        /// </summary>
        private static ThreadLocal<HashSet<string>> threadLocalCreatedDirectoryHash = new ThreadLocal<HashSet<string>>(() => new HashSet<string>());

        /// <summary>
        /// Optimized creating of directories, using ThreadLocal storage with a HashSet of upper case stored directory paths
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void EnsureDirectoryExists(this string directoryPath)
        {
            Throw.IfNullOrWhiteSpace(directoryPath, nameof(directoryPath));

            string dirPathUpper = directoryPath.ToUpperInvariant();

            if (!threadLocalCreatedDirectoryHash.Value.Contains(dirPathUpper))
            {
                // do just one try to create it, if it does not exist
                threadLocalCreatedDirectoryHash.Value.Add(dirPathUpper);

                Directory.CreateDirectory(directoryPath);
            }
        }
        #endregion Directory thread local cache

        #region MemoryStream to and from file Helpers
        /// <summary>
        /// Get the MemoryStream from file and reset the memory stream position to its begining
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static MemoryStream GetMemoryStreamFromFile(this string filePath)
        {
            Throw.IfNullOrWhiteSpace(filePath, nameof(filePath));

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Can not find the file: " + filePath, filePath);
            }

            MemoryStream memoryStream = null;
            try
            {
                FileInfo fi = new FileInfo(filePath);

                memoryStream = new MemoryStream((int)fi.Length);

                // open and read in the input data
                using (FileStream fileStream = fi.OpenRead())
                {
                    // this SetLength() is a must to get let memoryStream allocate the buffer, which we use in the next line!
                    memoryStream.SetLength(fi.Length);  // this is a must!
                    fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);
                }

                // reset position to start
                memoryStream.Position = 0;
            }
            catch (Exception ex)
            {
                if (memoryStream != null)
                    memoryStream.Dispose();
                memoryStream = null;

                string message = string.Format(CultureInfo.InvariantCulture, "Can not create a MemoryStream for the file {0}", filePath);
                throw new IOException(message, ex);
            }

            return memoryStream;
        }

        /// <summary>
        /// Dump One Stream To File
        /// </summary>
        /// <param name="memoryStream">The memory stream to dump to file</param>
        /// <param name="filePath">File path, where to dump the memory stream</param>
        public static void DumpOneStreamToFile(this MemoryStream memoryStream, string filePath)
        {
            Throw.IfNull(memoryStream, nameof(memoryStream));
            Throw.IfNullOrWhiteSpace(filePath, nameof(filePath));

            // reset memory stream position
            memoryStream.Position = 0;

            // ensure dir exists
            string dirPath = Path.GetDirectoryName(filePath);
            EnsureDirectoryExists(dirPath);

            // overwrite it if file exists
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                memoryStream.WriteTo(fileStream);
            }

            // reset memory stream position
            memoryStream.Position = 0;
        }
        #endregion MemoryStream to and from file Helpers

        #region MD5 Checksum
        /// <summary>
        /// Get MD5 Checksum of the file's content. Used to compare "file" contents.
        /// </summary>
        /// <param name="filePath">The filePath, for which content the MD5 checksum is to be computed.</param>
        /// <returns>The MD5 checksum of the file's content.</returns>
        public static string GetChecksum(this string filePath)
        {
            Stream stream = null;

            try
            {
                stream = File.OpenRead(filePath);

                using (var bufStream = new BufferedStream(stream, 1000000))
                {
                    stream = null;

                    return GetChecksum(bufStream);
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
                stream = null;
            }
        }

        /// <summary>
        /// Get MD5 Checksum of the stream content. Used to compare "file" contents.
        /// </summary>
        /// <param name="stream">The stream, for which content the MD5 checksum is to be computed.</param>
        /// <returns>The MD5 checksum of the stream's content.</returns>
        public static string GetChecksum(this Stream stream)
        {
            Throw.IfNull(stream, nameof(stream));

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            string md5CheckSum = null;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] checksum = md5Hash.ComputeHash(stream);
                md5CheckSum = BitConverter.ToString(checksum).Replace("-", String.Empty);
            }

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            return md5CheckSum;
        }
        #endregion MD5 Checksum
    }
}
