namespace MJsNetExtensions.WindowsImpersonation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;


    /// <summary>
    /// Contains Windows-specific P/Invoke declarations for user impersonation.
    /// This class is only supported on Windows platforms and requires advapi32.dll and kernel32.dll.
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal static class NativeMethods
    {
        #region Statics and Constst

        /// <summary>
        /// Windows API method used for user impersonation (LogOn) from .NET programs.
        /// Requires advapi32.dll which is only available on Windows platforms.
        /// </summary>
        /// <param name="lpszUserName">The user name to log on.</param>
        /// <param name="lpszDomain">The domain or server name (can be null for local accounts).</param>
        /// <param name="lpszPassword">The password for the user account.</param>
        /// <param name="dwLogonType">The type of logon operation to perform.</param>
        /// <param name="dwLogonProvider">The logon provider.</param>
        /// <param name="phToken">A handle to a token that represents the specified user.</param>
        /// <returns>True if successful, false otherwise. Call Marshal.GetLastWin32Error() for error details.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1414:MarkBooleanPInvokeArgumentsWithMarshalAs")]
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern bool LogonUser(
            String lpszUserName, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider,
            out SafeTokenHandle phToken
            );

        internal const int LOGON32_PROVIDER_DEFAULT = 0;
        internal const int LOGON32_LOGON_INTERACTIVE = 2;

        /// <summary>
        /// Closes an open object handle.
        /// Requires kernel32.dll which is only available on Windows platforms.
        /// </summary>
        /// <param name="handle">A valid handle to an open object.</param>
        /// <returns>True if successful, false otherwise.</returns>
        [DllImport("kernel32.dll")]
        [SuppressUnmanagedCodeSecurity]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

        #endregion Statics and Constst

    }
}
