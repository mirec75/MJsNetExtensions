#pragma warning disable CA1419
namespace MJsNetExtensions.WindowsImpersonation
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Runtime.Versioning;


    /// <summary>
    /// Represents a wrapper class for a Windows token handle. This class is only supported on Windows platforms.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        #region Construction / Destruction

        // prohibit user default construction
        private SafeTokenHandle()
            : base(true)
        {
        }

        #endregion Construction / Destruction

        #region Private Methods

        /// <summary>
        /// Release the Win32 System Handle stored and represented by the object of this class.
        /// </summary>
        /// <returns>true if the handle was released successfully; otherwise, false.</returns>
        /// <exception cref="PlatformNotSupportedException">if called on a non-Windows platform.</exception>
        protected override bool ReleaseHandle()
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException(
                    "SafeTokenHandle is only supported on Windows platforms.");
            }

            return NativeMethods.CloseHandle(handle);
        }
        #endregion Private Methods
    }
}
