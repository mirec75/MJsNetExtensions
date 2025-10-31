namespace MJsNetExtensions.WindowsImpersonation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;


    /// <summary>
    /// Summary description for WinRunAsImpersonation
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class WinRunAsImpersonation
    {
        #region API - Public Methods
        /// <summary>
        /// Method for running code <paramref name="action"/> on different user logged on context, using Win32 impersonation.
        /// This functionality is only available on Windows platforms.
        /// </summary>
        /// <param name="domain">optional domain name of the <paramref name="userName"/>.</param>
        /// <param name="userName">The user name to log on.</param>
        /// <param name="password">Password of the user to log on.</param>
        /// <param name="action">The <seealso cref="Action"/> to execute, if the LogOn to the <paramref name="userName"/> was successfull.</param>
        /// <exception cref="ArgumentException">if <paramref name="userName"/> or <paramref name="action"/> is null or empty.</exception>
        /// <exception cref="Win32Exception">if the log on for the <paramref name="userName"/> failed.</exception>
        /// <exception cref="PlatformNotSupportedException">if called on a non-Windows platform.</exception>
        public static void InteractiveLogOnRunAs(
            string domain,
            string userName,
            string password,
            Action action
            )
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException(
                    "Windows impersonation is only supported on Windows platforms. " +
                    "This functionality requires Windows-specific APIs (advapi32.dll) that are not available on other operating systems.");
            }

            InteractiveLogOnRunAs(domain, userName, password, action, null, Console.Error.WriteLine);
            //InteractiveLogOnRunAs(domain, username, password, action, Console.WriteLine, Console.Error.WriteLine);
        }

        /// <summary>
        /// Method for running code <paramref name="action"/> on different user logged on context, using Win32 impersonation.
        /// This functionality is only available on Windows platforms.
        /// </summary>
        /// <param name="domain">optional domain name of the <paramref name="userName"/>.</param>
        /// <param name="userName">The user name to log on.</param>
        /// <param name="password">Password of the user to log on.</param>
        /// <param name="action">The <seealso cref="Action"/> to execute, if the LogOn to the <paramref name="userName"/> was successfull.</param>
        /// <param name="traceLogOnProgress">Optional tracing function for the LogOn proceeding. Can be null.</param>
        /// <param name="logError">Optional error logging function for the LogOn proceeding. Can be null.</param>
        /// <exception cref="ArgumentException">if <paramref name="userName"/> or <paramref name="action"/> is null or empty.</exception>
        /// <exception cref="Win32Exception">if the log on for the <paramref name="userName"/> failed.</exception>
        /// <exception cref="PlatformNotSupportedException">if called on a non-Windows platform.</exception>
        public static void InteractiveLogOnRunAs(
            string domain, 
            string userName, 
            string password, 
            Action action,
            Action<string> traceLogOnProgress,
            Action<string> logError
            )
        {
            // Runtime platform check for defensive coding
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException(
                    "Windows impersonation is only supported on Windows platforms. " +
                    "This functionality requires Windows-specific APIs (advapi32.dll) that are not available on other operating systems.");
            }

            // https://docs.microsoft.com/en-us/dotnet/api/system.security.principal.windowsidentity.impersonate?redirectedfrom=MSDN&view=netframework-4.8#System_Security_Principal_WindowsIdentity_Impersonate
            // https://forums.asp.net/t/2126438.aspx?Windows+Authentication+to+make+SQL+Server+Connection
            // https://stackoverflow.com/questions/1501704/logonuser-logon32-logon-interactive-and-logon32-logon-network

            Throw.IfNullOrWhiteSpace(userName, nameof(userName));
            Throw.IfNull(action, nameof(action));


            SafeTokenHandle safeTokenHandle;

            // get primary login token
            bool returnValue = NativeMethods.LogonUser(
                   userName,
                   domain,
                   password,
                   NativeMethods.LOGON32_LOGON_INTERACTIVE,
                   NativeMethods.LOGON32_PROVIDER_DEFAULT,
                   out safeTokenHandle);

            if (!returnValue)
            {
                int ret = Marshal.GetLastWin32Error();

                logError?.Invoke($@"LogonUser failed for: {domain}\{userName} with Win32 error code: {ret}");
                throw new Win32Exception(ret);
            }
            //else:...

            using (safeTokenHandle)
            {
                IntPtr logOnHandle = safeTokenHandle.DangerousGetHandle();
                traceLogOnProgress?.Invoke($@"LogonUser successfull for: {domain}\{userName}. Value of Windows NT token: {logOnHandle}");

                // Check the identity:
                traceLogOnProgress?.Invoke($"User Identity Before impersonation: {WindowsIdentity.GetCurrent().Name}");

                // Use the token handle returned by LogonUser.
                using (WindowsIdentity newId = new WindowsIdentity(logOnHandle))
                {
                    System.Security.Principal.WindowsIdentity.RunImpersonated(
                        newId.AccessToken,
                        () => {
                            // Check the identity:
                            traceLogOnProgress?.Invoke($"User Identity After impersonation: {WindowsIdentity.GetCurrent().Name}");

                            // Do stuff for the other user now:
                            action();
                        });                    
                } // <-- Releasing the context object stops the impersonation

                // Check the identity:
                traceLogOnProgress?.Invoke($"User Identity After closing the context: {WindowsIdentity.GetCurrent().Name}");
            }
        }

        #endregion API - Public Methods
    }
}
