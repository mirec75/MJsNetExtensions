namespace MJsNetExtensions.WindowsImpersonation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;


    /// <summary>
    /// Summary description for NativeMethods
    /// </summary>
    internal static class NativeMethods
    {
        #region Statics and Constst

        /// <summary>
        /// C++ extern Windows method used for Windows Impersonation (LodOn) inside a .Net programm.
        /// </summary>
        /// <param name="lpszUserName"></param>
        /// <param name="lpszDomain"></param>
        /// <param name="lpszPassword"></param>
        /// <param name="dwLogonType"></param>
        /// <param name="dwLogonProvider"></param>
        /// <param name="phToken"></param>
        /// <returns>True if successfull, false otherwise.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1414:MarkBooleanPInvokeArgumentsWithMarshalAs")]
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(
            String lpszUserName, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider,
            out SafeTokenHandle phToken
            );

        internal const int LOGON32_PROVIDER_DEFAULT = 0;
        internal const int LOGON32_LOGON_INTERACTIVE = 2;

        /// <summary>
        /// Method for closing a Win32 System handle.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

        #endregion Statics and Constst

    }
}
