using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJsNetExtensions.ObjectValidation;

namespace MJsNetExtensions
{
    /// <summary>
    /// Summary description for OperationResultExtensions
    /// </summary>
    public static class OperationResultExtensions
    {
        #region API - Public Methods

        /// <summary>
        /// Converts the <paramref name="validationResult"/> to a <see cref="OperationResult"/>.
        /// </summary>
        /// <param name="validationResult">The <see cref="IValidationResult"/> to convert.</param>
        /// <returns>The newly created <see cref="OperationResult"/> with the result of validation.</returns>
        public static OperationResult ToOperationResult(this IValidationResult validationResult)
        {
            if (validationResult == null)
            {
                return OperationResult.CreateArgumentNullFailure(nameof(validationResult));
            }

            if (!validationResult.IsValid)
            {
                return OperationResult.CreateFailure(validationResult.InvalidReason);
            }

            return OperationResult.CreateSuccess();
        }

        /// <summary>
        /// Converts the <paramref name="validationResult"/> to a <see cref="OperationResult{T}"/> with result type: <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Note, that the result (type) must not necessary be the validated object (type).</typeparam>
        /// <param name="validationResult">The <see cref="IValidationResult"/> to convert. If null, the returned <see cref="OperationResult{T}"/> contains failure.</param>
        /// <param name="result">The result object must not be null. It is mostly object intended to be validated, validation reasult of which is stored in <paramref name="validationResult"/>.</param>
        /// <returns>The newly created <see cref="OperationResult{T}"/> with the <paramref name="result"/> is not null and if <paramref name="validationResult"/> was valid (and not null). 
        /// Else, the returned <see cref="OperationResult{T}"/> contains the failure information.</returns>
        public static OperationResult<T> ToOperationResult<T>(this IValidationResult validationResult, T result)
        {
            if (validationResult == null) { return OperationResult<T>.CreateArgumentNullFailure(nameof(validationResult)); }
            if (result == null) { return OperationResult<T>.CreateArgumentNullFailure(nameof(result)); }

            if (!validationResult.IsValid)
            {
                return OperationResult<T>.CreateFailure(validationResult.InvalidReason);
            }

            return OperationResult<T>.CreateSuccess(result);
        }

        /// <summary>
        /// Converts the <paramref name="validationOperationResult"/> to a <see cref="OperationResult{T}"/> with result type: <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Note, that the result (type) must not necessary be the validated object (type).</typeparam>
        /// <param name="validationOperationResult">The <see cref="OperationResult{ValidationResult}"/> to convert. If null, the returned <see cref="OperationResult{T}"/> contains failure.</param>
        /// <param name="result">The result object must not be null. It is mostly object intended to be validated, validation reasult of which is stored in <paramref name="validationOperationResult"/>.</param>
        /// <returns>The newly created <see cref="OperationResult{T}"/> with the <paramref name="result"/> if not null and <paramref name="validationOperationResult"/> was successfull and valid. 
        /// Else, the returned <see cref="OperationResult{T}"/> contains the failure information.</returns>
        public static OperationResult<T> ToOperationResult<T>(this OperationResult<ValidationResult> validationOperationResult, T result)
        {
            if (validationOperationResult == null) { return OperationResult<T>.CreateArgumentNullFailure(nameof(validationOperationResult)); }
            if (result == null) { return OperationResult<T>.CreateArgumentNullFailure(nameof(result)); }

            // Handle the Result of Strongly-Typed Validation Operation:
            if (!validationOperationResult.Success)
            {
                return validationOperationResult.CreateFailure<T>();
            }

            if (!validationOperationResult.Result.IsValid)
            {
                return OperationResult<T>.CreateFailure(validationOperationResult.Result.InvalidReason);
            }

            //else:
            return OperationResult<T>.CreateSuccess(result);
        }

        #endregion API - Public Methods
    }
}
