namespace MJsNetExtensions.WindowsImpersonation
{
    using Microsoft.Win32.SafeHandles;
    using System;


    /// <summary>
    /// Summary description for SafeTokenHandle
    /// </summary>
    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        #region Construction / Destruction

        // prohibit user default construction
        private SafeTokenHandle()
            : base(true)
        {
        }

        #endregion Construction / Destruction

        #region API - Public Methods
        #endregion API - Public Methods

        #region Private Methods

        /// <summary>
        /// Release the Win32 System Handle stored and represented by the object of this class.
        /// </summary>
        /// <returns></returns>
        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(handle);
        }
        #endregion Private Methods
    }
}
