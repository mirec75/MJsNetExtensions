namespace MJsNetExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using MJsNetExtensions.ObjectNavigation;
    using MJsNetExtensions.ObjectValidation;

    /// <summary>
    /// Helper class to simplify and shorten the code necessary for testing paramters.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Throw")]
    public static class Throw
    {
        #region Argument Checking
        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="parameter">the parameter to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static T IfNull<T>([ValidatedNotNull] T parameter, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (ReferenceEquals(parameter, null))
            {
                throw new ArgumentNullException(name);
            }

            return parameter;
        }

        /// <summary>
        /// Helper parameter checker. A convenience parameter test, unifying test if null and if parameter is valid.
        /// </summary>
        /// <param name="parameter">the parameter of validatable type <see cref="ISimpleValidatable"/> which is to check.</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null or invalid.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static ISimpleValidatable IfNullOrInvalid([ValidatedNotNull] ISimpleValidatable parameter, string name)
        {
            return IfNullOrInvalid(parameter, name, null);
        }

        /// <summary>
        /// Helper parameter checker. A convenience parameter test, unifying test if null and if parameter is valid.
        /// </summary>
        /// <param name="parameter">the parameter of validatable type <see cref="ISimpleValidatable"/> which is to check.</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <param name="settings">Optional. Can be null. The object validation settings: <see cref="ValidationSettings"/></param>
        /// <returns>The <paramref name="parameter"/> if not null or invalid.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static ISimpleValidatable IfNullOrInvalid([ValidatedNotNull] ISimpleValidatable parameter, string name, ValidationSettings settings)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (ReferenceEquals(parameter, null))
            {
                throw new ArgumentNullException(name);
            }

            Throw.IfInvalid(parameter.Validate(settings));

            return parameter;
        }

        /// <summary>
        /// Helper parameter checker. A convenience parameter test, unifying test if null and if parameter is valid.
        /// </summary>
        /// <param name="parameter">the parameter of validatable type <see cref="ISimpleValidatableAndUpdatable"/> which is to check.</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null or invalid.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static ISimpleValidatableAndUpdatable IfNullOrInvalid([ValidatedNotNull] ISimpleValidatableAndUpdatable parameter, string name)
        {
            return IfNullOrInvalid(parameter, name, null);
        }

        /// <summary>
        /// Helper parameter checker. A convenience parameter test, unifying test if null and if parameter is valid.
        /// </summary>
        /// <param name="parameter">the parameter of validatable type <see cref="ISimpleValidatableAndUpdatable"/> which is to check.</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <param name="settings">Optional. Can be null. The object validation settings: <see cref="ValidationSettings"/></param>
        /// <returns>The <paramref name="parameter"/> if not null or invalid.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static ISimpleValidatableAndUpdatable IfNullOrInvalid([ValidatedNotNull] ISimpleValidatableAndUpdatable parameter, string name, ValidationSettings settings)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (ReferenceEquals(parameter, null))
            {
                throw new ArgumentNullException(name);
            }

            Throw.IfInvalid(parameter.ValidateAndUpdate(settings));

            return parameter;
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="conditionResult">the parameter to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="conditionResult"/> if its value is false.</returns>
        /// <exception cref="ArgumentNullException">If the parameter name is NullOrEmpty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the conditionResult is true.</exception>
        public static bool If(bool conditionResult, string name)
        {
            return If(conditionResult, name, null);
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
        public static bool If(bool conditionResult, string name, string messageFormat, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (conditionResult)
            {
                if (string.IsNullOrWhiteSpace(messageFormat))
                {
                    throw new ArgumentOutOfRangeException(name);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(name, messageFormat.TryFormatInvariant(args));
                }
            }

            return conditionResult;
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="conditionResult">the parameter to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="conditionResult"/> if its value is true.</returns>
        /// <exception cref="ArgumentNullException">If the parameter name is NullOrEmpty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the conditionResult is false.</exception>
        public static bool IfNot(bool conditionResult, string name)
        {
            return IfNot(conditionResult, name, null);
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
        public static bool IfNot(bool conditionResult, string name, string messageFormat, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!conditionResult)
            {
                if (string.IsNullOrWhiteSpace(messageFormat))
                {
                    throw new ArgumentOutOfRangeException(name);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(name, messageFormat.TryFormatInvariant(args));
                }
            }

            return conditionResult;
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null or empty.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static IEnumerable IfNullOrEmpty([ValidatedNotNull] IEnumerable parameter, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!(parameter?.Cast<object>()?.Any() ?? false))
            {
                throw new ArgumentNullException(name);
            }

            return parameter;
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null or empty.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static IEnumerable<T> IfNullOrEmpty<T>([ValidatedNotNull] IEnumerable<T> parameter, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!(parameter?.Any() ?? false))
            {
                throw new ArgumentNullException(name);
            }

            return parameter;
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null or empty.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrEmpty.</exception>
        public static string IfNullOrEmpty([ValidatedNotNull] string parameter, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentNullException(name);
            }

            return parameter;
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="name">The parameter name to be reported to caller in an exception.</param>
        /// <returns>The <paramref name="parameter"/> if not null, empty or whithe space.</returns>
        /// <exception cref="ArgumentNullException">If one of the parameters is NullOrWhiteSpace.</exception>
        public static string IfNullOrWhiteSpace([ValidatedNotNull] string parameter, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentNullException(name);
            }

            return parameter;
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
        public static bool InvalidOperationIf(bool conditionResult, string formatString, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(formatString))
            {
                throw new ArgumentNullException(nameof(formatString));
            }

            if (conditionResult)
            {
                var message = string.Format(CultureInfo.InvariantCulture, formatString, args);
                throw new InvalidOperationException(message);
            }

            return conditionResult;
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
        public static bool InvalidOperationIfNot(bool conditionResult, string formatString, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(formatString))
            {
                throw new ArgumentNullException(nameof(formatString));
            }

            if (!conditionResult)
            {
                var message = string.Format(CultureInfo.InvariantCulture, formatString, args);
                throw new InvalidOperationException(message);
            }

            return conditionResult;
        }

        /// <summary>
        /// Helper parameter checker
        /// </summary>
        /// <param name="validationResult">The <see cref="IValidationResult"/> to evaluate</param>
        /// <returns>The <paramref name="validationResult"/> if not null or invalid.</returns>
        /// <exception cref="ArgumentNullException">If the parameter is null.</exception>
        /// <exception cref="InvalidOperationException">If the validation result is invalid.</exception>
        public static IValidationResult IfInvalid([ValidatedNotNull] IValidationResult validationResult)
        {
            ArgumentNullException.ThrowIfNull(validationResult);

            if (!validationResult.IsValid)
            {
                throw new InvalidOperationException(validationResult.InvalidReason);
            }

            return validationResult;
        }
        #endregion
    }
}
