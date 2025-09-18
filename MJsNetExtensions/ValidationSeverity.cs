namespace MJsNetExtensions
{
    /// <summary>
    /// Validation Severity enumeration
    /// </summary>
    public enum ValidationSeverity
    {
        /// <summary>
        /// Warning
        /// </summary>
        Warning = 0,

        /// <summary>
        /// Error
        /// </summary>
        Error = 1,

        /// <summary>
        /// Error Stop the Validation
        /// </summary>
        ErrorStopValidation = 2,

        /// <summary>
        /// Exception
        /// </summary>
        Exception = 3,
    };
}
