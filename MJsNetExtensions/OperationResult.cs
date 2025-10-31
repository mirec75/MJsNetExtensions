namespace MJsNetExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MJsNetExtensions.ObjectValidation;

    /// <summary>
    /// Used for the Operation Result Pattern - avoiding Code Analysis hinted design problem in: CA1021 AvoidOutParameters: https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1021
    /// See the pattern explanation in the 2 following links: Error Handling in SOLID C# .NET – The Operation Result Approach: https://www.codeproject.com/Articles/1022462/Error-Handling-in-SOLID-Csharp-NET-The-Operation-R
    /// The Operation Result Pattern — A Simple Guide: https://medium.com/@cummingsi1993/the-operation-result-pattern-a-simple-guide-fe10ff959080#
    /// </summary>
    public class OperationResult
    {
        #region Construction / Destruction

        /// <summary>
        /// Prohibit user default construction, but allow subclassing
        /// </summary>
        protected OperationResult() { }

        #endregion Construction / Destruction

        #region Properties

        /// <summary>
        /// Flag indicatiing tf the operation was successfull or not.
        /// </summary>
        public bool Success { get; protected set; }

        /// <summary>
        /// The error message describing the problem along the lines of a business validation.
        /// </summary>
        public string FailureMessage { get; protected set; }

        /// <summary>
        /// The exception referring to a technical problem.
        /// </summary>
        public Exception Exception { get; protected set; }

        #endregion Properties

        #region API - Public Methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            if (this.Success)
            {
                return $"Operation was successfull!";
            }
            else if (this.Exception != null)
            {
                return $"Operation failed: {this.Exception}";
            }
            else
            {
                return $"Operation failed: {this.FailureMessage}";
            }
        }

        /// <summary>
        /// Create generic <see cref="OperationResult{T}"/> with <paramref name="result"/> (used only if "this" is successfull) from this object. <see cref="OperationResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The generic resut type.</typeparam>
        /// <param name="result">The result to deliver upon success.</param>
        /// <returns>The newly created <see cref="OperationResult{T}"/>.</returns>
        public virtual OperationResult<TResult> CreateGenericResult<TResult>(TResult result)
        {
            if (this.Success)
            {
                return OperationResult<TResult>.CreateSuccess(result);
            }
            else if (this.Exception != null)
            {
                return OperationResult<TResult>.CreateFailure(this.Exception);
            }
            return OperationResult<TResult>.CreateFailure(this.FailureMessage);
        }

        /// <summary>
        /// Create Failure Result with <see cref="OperationResult{T}"/> from this object. <see cref="OperationResult"/>.
        /// </summary>
        /// <returns>The newly created <see cref="OperationResult{T}"/> indicating a failed operation.</returns>
        public virtual OperationResult<TResult> CreateFailure<TResult>()
        {
            Throw.If(this.Success, nameof(this.Success), "only failed operation result is allowed");

            if (this.Exception != null)
            {
                return OperationResult<TResult>.CreateFailure(this.Exception);
            }
            return OperationResult<TResult>.CreateFailure(this.FailureMessage);
        }
        #endregion API - Public Methods

        #region API - 3 most general Public Static Methods to create a new instance of OperationResult
        /// <summary>
        /// Create Success Result
        /// </summary>
        /// <returns>The newly created <see cref="OperationResult"/> indicating a successfull operation</returns>
        public static OperationResult CreateSuccess()
        {
            return new OperationResult { Success = true, };
        }

        /// <summary>
        /// Create Failure Result.
        /// The difference between failure and exception being that a failure is more along the lines of a business validation, and an exception more of a technical problem.
        /// </summary>
        /// <param name="failureMessage">The error message describing the problem along the lines of a business validation</param>
        /// <returns>The newly created <see cref="OperationResult"/> indicating a failed operation.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="failureMessage"/> is null, empty or whitespace.</exception>
        public static OperationResult CreateFailure([ValidatedNotNull] string failureMessage)
        {
            Throw.IfNullOrWhiteSpace(failureMessage, nameof(failureMessage));

            return new OperationResult { Success = false, FailureMessage = failureMessage, };
        }

        /// <summary>
        /// Create Failure Result.
        /// The difference between failure and exception being that a failure is more along the lines of a business validation, and an exception more of a technical problem.
        /// </summary>
        /// <param name="exception">The exception referring to a technical problem.</param>
        /// <returns>The newly created <see cref="OperationResult{TResult}"/> indicating a failed operation.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="exception"/> is null, empty or whitespace.</exception>
        public static OperationResult CreateFailure([ValidatedNotNull] Exception exception)
        {
            Throw.IfNull(exception, nameof(exception));

            return new OperationResult
            {
                Success = false,
                FailureMessage = exception.JoinMessagesWithTypes(),
                Exception = exception,
            };
        }
        #endregion API - 3 most general Public Static Methods to create a new instance of OperationResult

        #region API - Derived Public Static Methods to create a new Failure instance of OperationResult
        /// <summary>
        /// Create Failure Result with <see cref="ArgumentNullException"/>
        /// </summary>
        /// <param name="parameterName">The parameter name to be reported to caller.</param>
        /// <returns>The newly created <see cref="OperationResult"/> indicating a failed operation.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="parameterName"/> is null, empty or whitespace.</exception>
        public static OperationResult CreateArgumentNullFailure(string parameterName)
        {
            Throw.IfNullOrWhiteSpace(parameterName, nameof(parameterName));

            return CreateFailure(new ArgumentNullException(parameterName));
        }

        /// <summary>
        /// Create Failure Result with <see cref="ArgumentOutOfRangeException"/>
        /// </summary>
        /// <param name="parameterName">The parameter name to be reported to caller.</param>
        /// <returns>The newly created <see cref="OperationResult"/> indicating a failed operation.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="parameterName"/> is null, empty or whitespace.</exception>
        public static OperationResult CreateArgumentOutOfRangeFailure(string parameterName)
        {
            Throw.IfNullOrWhiteSpace(parameterName, nameof(parameterName));

            return CreateFailure(new ArgumentOutOfRangeException(parameterName));
        }

        /// <summary>
        /// Create Failure Result with <see cref="InvalidOperationException"/>
        /// </summary>
        /// <param name="parameterName">The parameter name to be reported to caller.</param>
        /// <returns>The newly created <see cref="OperationResult"/> indicating a failed operation.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="parameterName"/> is null, empty or whitespace.</exception>
        public static OperationResult CreateInvalidOperationFailure(string parameterName)
        {
            Throw.IfNullOrWhiteSpace(parameterName, nameof(parameterName));

            return CreateFailure(new InvalidOperationException(parameterName));
        }

        /// <summary>
        /// Create Failure Result with <see cref="InvalidOperationException"/>
        /// </summary>
        /// <param name="parameterName">The parameter name to be reported to caller.</param>
        /// <param name="ex">Optional. Can be null. Nested <see cref="Exception"/>.</param>
        /// <returns>The newly created <see cref="OperationResult"/> indicating a failed operation.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="parameterName"/> is null, empty or whitespace.</exception>
        public static OperationResult CreateInvalidOperationFailure(string parameterName, Exception ex)
        {
            Throw.IfNullOrWhiteSpace(parameterName, nameof(parameterName));

            return CreateFailure(new InvalidOperationException(parameterName, ex));
        }

        /// <summary>
        /// Create Failure Result with <see cref="InvalidOperationException"/> if the <paramref name="validatable"/> is null, or a Falilure if invalid after the Strongly-Typed Object Validation.
        /// </summary>
        /// <param name="validatable">The validatable object which is about to be validated.</param>
        /// <param name="parameterName">The parameter name to be reported to caller.</param>
        /// <returns>The newly created <see cref="OperationResult"/> indicating a failed operation.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="parameterName"/> is null, empty or whitespace.</exception>
        public static OperationResult GetFailureIfNullOrInvalid(ISimpleValidatable validatable, string parameterName)
        {
            Throw.IfNullOrWhiteSpace(parameterName, nameof(parameterName));

            if (validatable == null)
            {
                return OperationResult.CreateArgumentNullFailure(parameterName);
            }

            OperationResult<ISimpleValidatable> validationOperationResult = validatable.TryValidate();
            if (!validationOperationResult.Success)
            {
                return validationOperationResult;
            }

            return null;
        }

        /// <summary>
        /// Create Failure Result with <see cref="InvalidOperationException"/> if the <paramref name="validatableAndUpdatable"/> is null, or a Falilure if invalid after the Strongly-Typed Object Validation.
        /// </summary>
        /// <param name="validatableAndUpdatable">The validatable object which is about to be validated.</param>
        /// <param name="parameterName">The parameter name to be reported to caller.</param>
        /// <returns>The newly created <see cref="OperationResult"/> indicating a failed operation.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="parameterName"/> is null, empty or whitespace.</exception>
        public static OperationResult GetFailureIfNullOrInvalid(ISimpleValidatableAndUpdatable validatableAndUpdatable, string parameterName)
        {
            Throw.IfNullOrWhiteSpace(parameterName, nameof(parameterName));

            if (validatableAndUpdatable == null)
            {
                return OperationResult.CreateArgumentNullFailure(parameterName);
            }

            OperationResult<ISimpleValidatableAndUpdatable> validationOperationResult = validatableAndUpdatable.TryValidateAndUpdate();
            if (!validationOperationResult.Success)
            {
                return validationOperationResult;
            }

            return null;
        }
        #endregion API - Derived Public Static Methods to create a new Failure instance of OperationResult
    }

}
