using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJsNetExtensions.ObjectNavigation;
using MJsNetExtensions.ObjectValidation;

namespace MJsNetExtensions
{
    /// <summary>
    /// Summary description for ThrowExtensions
    /// </summary>
    public static class ThrowExtensions
    {
        #region Argument Checking
        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="parameter">the parameter to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static T ThrowIfNull<T>([ValidatedNotNull] this T parameter, string name)
        {
            return Throw.IfNull(parameter, name);
        }

        /// <summary>
        /// Helper parameter checker. A convenience parameter test, unifying test if null and if parameter is valid.
        /// </summary>
        /// <param name="parameter">the parameter of validatable type <see cref="ISimpleValidatable"/> which is to check.</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null or invalid.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static ISimpleValidatable ThrowIfNullOrInvalid([ValidatedNotNull] this ISimpleValidatable parameter, string name)
        {
            return Throw.IfNullOrInvalid(parameter, name);
        }

        /// <summary>
        /// Helper parameter checker. A convenience parameter test, unifying test if null and if parameter is valid.
        /// </summary>
        /// <param name="parameter">the parameter of validatable type <see cref="ISimpleValidatable"/> which is to check.</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <param name="settings">Optional. Can be null. The object validation settings: <see cref="ValidationSettings"/></param>
        /// <returns>The <paramref name="parameter"/> if not null or invalid.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static ISimpleValidatable ThrowIfNullOrInvalid([ValidatedNotNull] this ISimpleValidatable parameter, string name, ValidationSettings settings)
        {
            return Throw.IfNullOrInvalid(parameter, name, settings);
        }

        /// <summary>
        /// Helper parameter checker. A convenience parameter test, unifying test if null and if parameter is valid.
        /// </summary>
        /// <param name="parameter">the parameter of validatable type <see cref="ISimpleValidatableAndUpdatable"/> which is to check.</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null or invalid.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static ISimpleValidatableAndUpdatable ThrowIfNullOrInvalid([ValidatedNotNull] this ISimpleValidatableAndUpdatable parameter, string name)
        {
            return Throw.IfNullOrInvalid(parameter, name);
        }

        /// <summary>
        /// Helper parameter checker. A convenience parameter test, unifying test if null and if parameter is valid.
        /// </summary>
        /// <param name="parameter">the parameter of validatable type <see cref="ISimpleValidatableAndUpdatable"/> which is to check.</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <param name="settings">Optional. Can be null. The object validation settings: <see cref="ValidationSettings"/></param>
        /// <returns>The <paramref name="parameter"/> if not null or invalid.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static ISimpleValidatableAndUpdatable ThrowIfNullOrInvalid([ValidatedNotNull] this ISimpleValidatableAndUpdatable parameter, string name, ValidationSettings settings)
        {
            return Throw.IfNullOrInvalid(parameter, name, settings);
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="conditionResult">the parameter to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="conditionResult"/> if its value is false.</returns>
        /// <exception cref="ArgumentNullException">If the parameter name is NullOrEmpty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the conditionResult is true.</exception>
        public static bool ThrowIf(this bool conditionResult, string name)
        {
            return Throw.If(conditionResult, name);
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="conditionResult">the parameter to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <param name="messageFormat">Error message composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The <paramref name="conditionResult"/> if its value is false.</returns>
        /// <exception cref="ArgumentNullException">If the parameter name is NullOrEmpty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the conditionResult is true.</exception>
        public static bool ThrowIf(this bool conditionResult, string name, string messageFormat, params object[] args)
        {
            return Throw.If(conditionResult, name, messageFormat, args);
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="conditionResult">the parameter to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="conditionResult"/> if its value is true.</returns>
        /// <exception cref="ArgumentNullException">If the parameter name is NullOrEmpty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the conditionResult is false.</exception>
        public static bool ThrowIfNot(this bool conditionResult, string name)
        {
            return Throw.IfNot(conditionResult, name);
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="conditionResult">the parameter to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <param name="messageFormat">Error message composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The <paramref name="conditionResult"/> if its value is true.</returns>
        /// <exception cref="ArgumentNullException">If the parameter name is NullOrEmpty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the conditionResult is false.</exception>
        public static bool ThrowIfNot(this bool conditionResult, string name, string messageFormat, params object[] args)
        {
            return Throw.IfNot(conditionResult, name, messageFormat, args);
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null or empty.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static IEnumerable ThrowIfNullOrEmpty([ValidatedNotNull] this IEnumerable parameter, string name)
        {
            return Throw.IfNullOrEmpty(parameter, name);
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null or empty.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static IEnumerable<T> ThrowIfNullOrEmpty<T>([ValidatedNotNull] this IEnumerable<T> parameter, string name)
        {
            return Throw.IfNullOrEmpty<T>(parameter, name);
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null or empty.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static string ThrowIfNullOrEmpty([ValidatedNotNull] this string parameter, string name)
        {
            return Throw.IfNullOrEmpty(parameter, name);
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null, empty or whithe space.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrWhiteSpace.</exception>
        public static string ThrowIfNullOrWhiteSpace([ValidatedNotNull] this string parameter, string name)
        {
            return Throw.IfNullOrWhiteSpace(parameter, name);
        }
        #endregion Argument Checking

        #region Other Exceptions
        /// <summary>
        /// Helper condition checker
        /// </summary>
        /// <param name="conditionResult">the parameter to check</param>
        /// <param name="formatString">Error message composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The <paramref name="conditionResult"/> if its value is false.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="formatString"/> is NullOrEmpty.</exception>
        /// <exception cref="InvalidOperationException">If the conditionResult is true.</exception>
        public static bool ThrowInvalidOperationIf(this bool conditionResult, string formatString, params object[] args)
        {
            return Throw.InvalidOperationIf(conditionResult, formatString, args);
        }

        /// <summary>
        /// Helper condition checker
        /// </summary>
        /// <param name="conditionResult">the parameter to check</param>
        /// <param name="formatString">Error message composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The <paramref name="conditionResult"/> if its value is true.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="formatString"/> is NullOrEmpty.</exception>
        /// <exception cref="InvalidOperationException">If the conditionResult is false.</exception>
        public static bool ThrowInvalidOperationIfNot(this bool conditionResult, string formatString, params object[] args)
        {
            return Throw.InvalidOperationIfNot(conditionResult, formatString, args);
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="validationResult">The <see cref="IValidationResult"/> to evaluate</param>
        /// <returns>The <paramref name="validationResult"/> if not null or invalid.</returns>
        /// <exception cref="ArgumentNullException">If the parameter is null.</exception>
        /// <exception cref="InvalidOperationException">If the validation result is invalid.</exception>
        public static IValidationResult ThrowIfInvalid([ValidatedNotNull] this IValidationResult validationResult)
        {
            return Throw.IfInvalid(validationResult);
        }

        /// <summary>
        /// Helper extension method to simply handle and <see cref="OperationResult"/>.
        /// </summary>
        /// <param name="operationResult"></param>
        /// <exception cref="ArgumentNullException">If the <paramref name="operationResult"/> is null.</exception>
        /// <exception cref="InvalidOperationException">if <see cref="OperationResult.Success"/> is false</exception>
        public static void ThrowInvalidIfOperationFailed([ValidatedNotNull] this OperationResult operationResult)
        {
            if (operationResult.ThrowIfNull(nameof(operationResult)).Success)
            {
                return;
            }
            else if (operationResult.Exception != null)
            {
                throw new InvalidOperationException("The operation failed:", operationResult.Exception);
            }
            else
            {
                throw new InvalidOperationException(operationResult.FailureMessage);
            }
        }
        /// <summary>
        /// Helper extension method to simply handle and <see cref="OperationResult{TResult}"/>.
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="operationResult"></param>
        /// <returns>The <see cref="OperationResult{TResult}.Result"/> is <paramref name="operationResult"/> if <see cref="OperationResult.Success"/> is true</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="operationResult"/> is null.</exception>
        /// <exception cref="InvalidOperationException">if <see cref="OperationResult.Success"/> is false</exception>
        public static T OperationResultToResultOrException<T>([ValidatedNotNull] this OperationResult<T> operationResult)
        {
            if (operationResult.ThrowIfNull(nameof(operationResult)).Success)
            {
                return operationResult.Result;
            }
            else if (operationResult.Exception != null)
            {
                throw new InvalidOperationException("The operation failed:", operationResult.Exception);
            }
            else
            {
                throw new InvalidOperationException(operationResult.FailureMessage);
            }
        }
        #endregion
    }
}
