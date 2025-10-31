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
    public class OperationResult<TResult> : OperationResult
    {
        #region Construction / Destruction

        /// <summary>
        /// Prohibit user default construction, but allow subclassing
        /// </summary>
        protected OperationResult() { }

        #endregion Construction / Destruction

        #region Properties

        /// <summary>
        /// If <see cref="OperationResult.Success"/> is true, then this property contains the successfully created or provided operation result of the type: <typeparamref name="TResult"/>.
        /// Note, that the operation can also return null as a success result. If the operation fails, then the result is the default for <typeparamref name="TResult"/> (e.g. null).
        /// </summary>
        public TResult Result { get; private set; }

        #endregion Properties

        #region API - Public Methods
        /// <summary>
        /// Create generic <see cref="OperationResult{T}"/> of another generic resut type <typeparamref name="TOtherResult"/> as own generic type <typeparamref name="TResult"/>
        /// with <paramref name="result"/> (used only if "this" is successfull).
        /// </summary>
        /// <typeparam name="TOtherResult">The other generic resut type as own generic type <typeparamref name="TResult"/>.</typeparam>
        /// <param name="result">The result to deliver upon success.</param>
        /// <returns>The newly created <see cref="OperationResult{T}"/>.</returns>
        public override OperationResult<TOtherResult> CreateGenericResult<TOtherResult>(TOtherResult result)
        {
            if (this.Success)
            {
                return OperationResult<TOtherResult>.CreateSuccess(result);
            }
            else if (this.Exception != null)
            {
                return OperationResult<TOtherResult>.CreateFailure(this.Exception);
            }
            return OperationResult<TOtherResult>.CreateFailure(this.FailureMessage);
        }

        /// <summary>
        /// Create Failure Result with <see cref="OperationResult{T}"/> of another result type as own from this object. <see cref="OperationResult{T}"/>.
        /// </summary>
        /// <returns>The newly created <see cref="OperationResult{T}"/> indicating a failed operation.</returns>
        public override OperationResult<TOtherResult> CreateFailure<TOtherResult>()
        {
            Throw.If(this.Success, nameof(this.Success), "only failed operation result is allowed");

            if (this.Exception != null)
            {
                return OperationResult<TOtherResult>.CreateFailure(this.Exception);
            }
            return OperationResult<TOtherResult>.CreateFailure(this.FailureMessage);
        }
        #endregion API - Public Methods

        #region API - 3 most general Public Static Methods to create a new instance of OperationResult
        /// <summary>
        /// Create Success Result
        /// </summary>
        /// <param name="result">The successfully created or provided operation result of the type: <typeparamref name="TResult"/></param>
        /// <returns>The newly created <see cref="OperationResult{TResult}"/> indicating a successfull operation.</returns>
        public static OperationResult<TResult> CreateSuccess(TResult result)
        {
            return new OperationResult<TResult> { Success = true, Result = result, };
        }

        /// <summary>
        /// Create Failure Result.
        /// The difference between failure and exception being that a failure is more along the lines of a business validation, and an exception more of a technical problem.
        /// </summary>
        /// <param name="failureMessage">The error message describing the problem along the lines of a business validation</param>
        /// <returns>The newly created <see cref="OperationResult{TResult}"/> indicating a failed operation.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="failureMessage"/> is null, empty or whitespace.</exception>
        public static new OperationResult<TResult> CreateFailure([ValidatedNotNull] string failureMessage)
        {
            Throw.IfNullOrWhiteSpace(failureMessage, nameof(failureMessage));

            return new OperationResult<TResult> { Success = false, FailureMessage = failureMessage, };
        }

        /// <summary>
        /// Create Failure Result.
        /// The difference between failure and exception being that a failure is more along the lines of a business validation, and an exception more of a technical problem.
        /// </summary>
        /// <param name="exception">The exception referring to a technical problem.</param>
        /// <returns>The newly created <see cref="OperationResult{TResult}"/> indicating a failed operation.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="exception"/> is null, empty or whitespace.</exception>
        public static new OperationResult<TResult> CreateFailure([ValidatedNotNull] Exception exception)
        {
            Throw.IfNull(exception, nameof(exception));

            return new OperationResult<TResult>
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
        public static new OperationResult<TResult> CreateArgumentNullFailure(string parameterName)
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
        public static new OperationResult<TResult> CreateArgumentOutOfRangeFailure(string parameterName)
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
        public static new OperationResult<TResult> CreateInvalidOperationFailure(string parameterName)
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
        public static new OperationResult<TResult> CreateInvalidOperationFailure(string parameterName, Exception ex)
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
        public static new OperationResult<TResult> GetFailureIfNullOrInvalid(ISimpleValidatable validatable, string parameterName)
        {
            var result = OperationResult.GetFailureIfNullOrInvalid(validatable, parameterName);

            return result?.CreateFailure<TResult>();
        }

        /// <summary>
        /// Create Failure Result with <see cref="InvalidOperationException"/> if the <paramref name="validatableAndUpdatable"/> is null, or a Falilure if invalid after the Strongly-Typed Object Validation.
        /// </summary>
        /// <param name="validatableAndUpdatable">The validatable object which is about to be validated.</param>
        /// <param name="parameterName">The parameter name to be reported to caller.</param>
        /// <returns>The newly created <see cref="OperationResult"/> indicating a failed operation.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="parameterName"/> is null, empty or whitespace.</exception>
        public static new OperationResult<TResult> GetFailureIfNullOrInvalid(ISimpleValidatableAndUpdatable validatableAndUpdatable, string parameterName)
        {
            var result = OperationResult.GetFailureIfNullOrInvalid(validatableAndUpdatable, parameterName);

            return result?.CreateFailure<TResult>();
        }
        #endregion API - Derived Public Static Methods to create a new Failure instance of OperationResult
    }

}
