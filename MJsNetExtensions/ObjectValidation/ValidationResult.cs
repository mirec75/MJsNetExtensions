#pragma warning disable S125
namespace MJsNetExtensions.ObjectValidation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    /// <summary>
    /// Validation Result is a class fixing the the old pattern: "bool IsValid(out string invalidReason) {...}" and levearaging the validation to use validation result object instead.
    /// An object of this class undergoes 2 phases: a build phase, where the valiation results are collected 
    /// and a reporting phase after validation finished. For reporting the <see cref="IsValid"/> and <see cref="InvalidReason"/> are the main properties.
    /// Other methods and properties are servig for collecting validation results and formatting the optional <see cref="InvalidReason"/>, which in not null only in a case <see cref="IsValid"/> is false.
    /// Initially an instance of this class has <see cref="IsValid"/> set to true and <see cref="InvalidReason"/> set to null.
    /// </summary>
    public class ValidationResult : IValidationResult
    {
        #region Fields

        private readonly List<string> invalidReasons = new();

        private string nameOfRoot;
        private string invalidReasonPrefix;

        #endregion Fields

        #region Construction / Destruction
        /// <summary>
        /// Convenience constructor, to instantiate a <see cref="ValidationResult"/> with <see cref="InvalidReasonPrefix"/> reporting the caller type.
        /// </summary>
        /// <param name="callerType">The <see cref="Type"/> of the caller object used to build the <see cref="InvalidReasonPrefix"/> used in the final <see cref="InvalidReason"/>.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="callerType"/> is null.</exception>
        public ValidationResult([ValidatedNotNull] Type callerType)
        {
            Throw.IfNull(callerType, nameof(callerType));

            this.CallerTypeName = callerType.Name;
            this.nameOfRoot = this.CallerTypeName;
            this.invalidReasonPrefix = $"Invalid {this.nameOfRoot}: ";
        }

        /// <summary>
        /// Convenience constructor, to instantiate a <see cref="ValidationResult"/> with <see cref="InvalidReasonPrefix"/> reporting the caller type.
        /// </summary>
        /// <param name="caller">The caller object which is used to get its type to report the type name in the final <see cref="InvalidReason"/>.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="caller"/> is null.</exception>
        public ValidationResult([ValidatedNotNull] object caller)
          : this(caller is Type ? caller as Type : caller?.GetType())
        {
        }
        #endregion Construction / Destruction

        #region Properties

        /// <summary>
        /// The type name of the caller, who was a construction parameter to this class.
        /// </summary>
        public string CallerTypeName { get; private set; }

        /// <summary>
        /// The optional name of root. It is used as the descriptor (or better a name) in the <see cref="InvalidReason"/>, to state what is invalid.
        /// If not provided explicitely by the caller, it is set per default to the caller's type name.
        /// NOTE: setting value of this property explicitely recomputes the <see cref="InvalidReason"/>.
        /// If provided value is null, empty or whitespace, then the <see cref="CallerTypeName"/> is used as a default for this property.
        /// </summary>
        public string NameOfRoot
        {
            get => this.nameOfRoot;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.nameOfRoot = this.CallerTypeName;
                }
                else
                {
                    this.nameOfRoot = value;
                }
                this.invalidReasonPrefix = $"Invalid {this.nameOfRoot}: ";
            }
        }

        /// <summary>
        /// The optional invalid reason prefix. If provided and the <see cref="IsValid"/> is false, then it is uesed as a prefix of the final <see cref="InvalidReason"/> message.
        /// </summary>
        public string InvalidReasonPrefix
        {
            get => this.invalidReasonPrefix;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.invalidReasonPrefix = $"Invalid {this.NameOfRoot}: ";
                }
                else
                {
                    this.invalidReasonPrefix = value;
                }
            }
        }

        /// <summary>
        /// A flag (boolean) indicating the actual validation result. Initially it is always true (default value). 
        /// After the validation it is true, if all the checked error (invalidation) conditions were false. 
        /// Otherwise the <see cref="IsValid"/> is false, i.e. if any of the checked error (invalidation) conditions was true.
        /// </summary>
        public bool IsValid { get; private set; } = true;

        /// <summary>
        /// If the <see cref="IsValid"/> is true, this property returns null.
        /// Othewise it returns the final aggregated invalid reason string, consisting of the optional <see cref="InvalidReasonPrefix"/>
        /// and the collected partial <see cref="InvalidReasons"/> for all error conditions which were true. All particles are separated by <see cref="Environment.NewLine"/>.
        /// </summary>
        public string InvalidReason => this.ToString();

        /// <summary>
        /// The sorted collection of the single invalid reason particles, which form finally the aggregated <see cref="InvalidReason"/>.
        /// </summary>
        public IEnumerable<string> InvalidReasons => this.invalidReasons;

        /// <summary>
        /// Optional object path in an object tree, used als prefix for each subsuequent <see cref="ValidationResult.AddErrorMessage(string, string, object[])"/> call or any method derived from it.
        /// </summary>
        public string CurrentObjectPath { get; set; }

        #endregion Properties

        #region API - Public Methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return ToString(Environment.NewLine); // <-- the default separator
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="separator">A separator string, to separate each indiviaual invalid reason message.
        /// If the parameterles <seealso cref="ToString()"/> is called, then the default separator: <see cref="Environment.NewLine"/> is used.</param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(string separator)
        {
#pragma warning disable S2225
            return this.IsValid ?
                null :
                this.InvalidReasonPrefix + string.Join(separator, this.invalidReasons)
                ;
#pragma warning restore S2225
        }

        #endregion API - Public Methods


        #region Simple "Particle" Validation Methods

        /// <summary>
        /// Directly adds the <paramref name="errorFormat"/> in the invalid reasons, and invalidates this <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="propertyName">The name of the related property (optional, may be null or empty).</param>
        /// <param name="errorFormat">Error message composite format string. It shall be only the specific message which becomes part one of the final <see cref="InvalidReason"/> string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public void AddErrorMessage(string propertyName, string errorFormat, params object[] args)
        {
            Throw.IfNullOrWhiteSpace(errorFormat, nameof(errorFormat));

            // add optional prefix:
            string errorMessage = null;

            bool hasPath = !string.IsNullOrWhiteSpace(this.CurrentObjectPath);
            bool hasPropName = !string.IsNullOrWhiteSpace(propertyName);
            if (hasPath || hasPropName)
            {
                if (hasPath) { errorMessage += this.CurrentObjectPath; }
                if (hasPath && hasPropName) { errorMessage += "."; }
                if (hasPropName) { errorMessage += propertyName; }

                if (errorFormat[0] != '=' &&
                    errorFormat[0] != '>' &&
                    errorFormat[0] != '<'
                    )
                {
                    errorMessage += ":";
                }

                errorMessage += " ";
            }

            // -----------
            // DO NOT "Try" format: 
            //NOTE: this is a little bit different than calling: errorFormat.TryFormatInvariant(args). This call throws FormatException, if the caller provided bad implementation
            //      of a call to this method, i.e. if the errorFormat and the args does not match or fit together.

            object[] args2 = args ?? [null,]; // correcting .Net error if called from Throw.If(true, "haha", "msg1 {0}", null);
            errorMessage += string.Format(CultureInfo.InvariantCulture, errorFormat, args2);

            // ------------------------
            // Store the Invalid Reason
            invalidReasons.Add(errorMessage);
            this.IsValid = false;
        }

        /// <summary>
        /// Checks the <paramref name="errorConditionResult"/> and only if it is true, this <see cref="ValidationResult"/> will be invalidated
        /// and the <paramref name="errorFormat"/> will be added in the invalid reasons.
        /// </summary>
        /// <param name="errorConditionResult">The error condition result</param>
        /// <param name="propertyName">The name of the related property (optional, may be null or empty).</param>
        /// <param name="errorFormat">Error message composite format string. It shall be only the specific message which becomes part one of the final <see cref="InvalidReason"/> string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>the negation of provided <paramref name="errorConditionResult"/>.</returns>
        public bool InvalidateIf(bool errorConditionResult, string propertyName, string errorFormat, params object[] args)
        {
            Throw.IfNullOrWhiteSpace(errorFormat, nameof(errorFormat));

            if (errorConditionResult)
            {
                AddErrorMessage(propertyName, errorFormat, args);
            }

            return !errorConditionResult;
        }

        /// <summary>
        /// Checks the <paramref name="goodConditionResult"/> and only if it is false, this <see cref="ValidationResult"/> will be invalidated
        /// and the <paramref name="errorFormat"/> will be added in the invalid reasons.
        /// </summary>
        /// <param name="goodConditionResult">The good (i.e. expected positive) condition result</param>
        /// <param name="propertyName">The name of the related property (optional, may be null or empty).</param>
        /// <param name="errorFormat">Error message composite format string. It shall be only the specific message which becomes part one of the final <see cref="InvalidReason"/> string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>the provided <paramref name="goodConditionResult"/>.</returns>
        public bool InvalidateIfNot(bool goodConditionResult, string propertyName, string errorFormat, params object[] args)
        {
            Throw.IfNullOrWhiteSpace(errorFormat, nameof(errorFormat));

            if (!goodConditionResult)
            {
                AddErrorMessage(propertyName, errorFormat, args);
            }

            return goodConditionResult;
        }

        /// <summary>
        /// Checks if the <paramref name="parameter"/> is null and only if it is null, this <see cref="ValidationResult"/> will be invalidated
        /// and the <paramref name="propertyName"/> will be added as a message particle in the invalid reasons.
        /// </summary>
        /// <param name="parameter">the parameter to check</param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <returns>the true if the <paramref name="parameter"/> is not null, false otherwise.</returns>
        public bool InvalidateIfNull(object parameter, string propertyName)
        {
            Throw.IfNullOrWhiteSpace(propertyName, nameof(propertyName));

            if (ReferenceEquals(parameter, null))
            {
                AddErrorMessage(propertyName, "== null");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the <paramref name="parameter"/> is Null or Empty and only if it is Null or Empty, this <see cref="ValidationResult"/> will be invalidated
        /// and the <paramref name="propertyName"/> will be added as a message particle in the invalid reasons.
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <returns>the true if the <paramref name="parameter"/> is not null or empty, false otherwise.</returns>
        public bool InvalidateIfNullOrEmpty(IEnumerable parameter, string propertyName)
        {
            Throw.IfNullOrWhiteSpace(propertyName, nameof(propertyName));

            if (!(parameter?.Cast<object>()?.Any() ?? false))
            {
                AddErrorMessage(propertyName, "== null or empty");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the <paramref name="parameter"/> is Null or Empty and only if it is Null or Empty, this <see cref="ValidationResult"/> will be invalidated
        /// and the <paramref name="propertyName"/> will be added as a message particle in the invalid reasons.
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <returns>the true if the <paramref name="parameter"/> is not null or empty, false otherwise.</returns>
        public bool InvalidateIfNullOrEmpty<T>(IEnumerable<T> parameter, string propertyName)
        {
            Throw.IfNullOrWhiteSpace(propertyName, nameof(propertyName));

            if (!(parameter?.Any() ?? false))
            {
                AddErrorMessage(propertyName, "== null or empty");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the <paramref name="parameter"/> is NullOrEmpty and only if it is NullOrEmpty, this <see cref="ValidationResult"/> will be invalidated
        /// and the <paramref name="propertyName"/> will be added as a message particle in the invalid reasons.
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <returns>the true if the <paramref name="parameter"/> is not null or empty, false otherwise.</returns>
        public bool InvalidateIfNullOrEmpty(string parameter, string propertyName)
        {
            Throw.IfNullOrWhiteSpace(propertyName, nameof(propertyName));

            if (string.IsNullOrEmpty(parameter))
            {
                AddErrorMessage(propertyName, "== null or empty");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the <paramref name="parameter"/> is NullOrWhiteSpace and only if it is NullOrWhiteSpace, this <see cref="ValidationResult"/> will be invalidated
        /// and the <paramref name="propertyName"/> will be added as a message particle in the invalid reasons.
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <returns>the true if the <paramref name="parameter"/> is not null, empty or white space, false otherwise.</returns>
        public bool InvalidateIfNullOrWhiteSpace(string parameter, string propertyName)
        {
            Throw.IfNullOrWhiteSpace(propertyName, nameof(propertyName));

            if (string.IsNullOrWhiteSpace(parameter))
            {
                AddErrorMessage(propertyName, "== null or white space");
                return false;
            }

            return true;
        }
        #endregion Simple "Particle" Validation Methods

        #region Derived "Particle" Validation Methods

        /// <summary>
        /// Checks if the <paramref name="parameter"/> is Null or Empty or contains Duplicates and only if it is Null or Empty or contains Duplicates, this <see cref="ValidationResult"/> will be invalidated
        /// and the <paramref name="propertyName"/> will be added as a message particle in the invalid reasons.
        /// </summary>
        /// <param name="parameter">the parameter value to check</param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <returns>the true if the <paramref name="parameter"/> is not null or empty, false otherwise.</returns>
#pragma warning disable CA1851
        public bool InvalidateIfNullEmptyOrDuplicates<T>(IEnumerable<T> parameter, string propertyName)
        {
            if (!InvalidateIfNullOrEmpty<T>(parameter, propertyName))
            {
                return false;
            }
            else if (parameter.Count() != parameter.Distinct().Count())
            {
                AddErrorMessage(propertyName, "contains duplicates");
                return false;
            }

            return true;
        }
#pragma warning restore CA1851

        /// <summary>
        /// Tries to parse a <see cref="TimeSpan"/> from a string.
        /// </summary>
        /// <param name="timeSpanText">The text to parse as <see cref="TimeSpan"/></param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <param name="timeSpan">Resulting <see cref="TimeSpan"/></param>
        /// <returns>True if a <see cref="TimeSpan"/> could be parsed, false otherwise.</returns>
        public bool TryParseTimeSpanOrStoreMessage(string timeSpanText, string propertyName, out TimeSpan timeSpan)
        {
            if (TimeSpan.TryParse(timeSpanText, out timeSpan))
            {
                return true;
            }
            else
            {
                AddErrorMessage(propertyName, "TimeSpan string \"{0}\" could not be parsed. Provide it in a TimeSpan.TryParse() format, e.g.: HH:mm:ss", timeSpanText);
                timeSpan = TimeSpan.Zero;
                return false;
            }
        }

        /// <summary>
        /// Tries to parse a <see cref="TimeSpan"/> from a string.
        /// </summary>
        /// <param name="timeSpanText">The text to parse as <see cref="TimeSpan"/></param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <returns>Resulting <see cref="TimeSpan"/> if successfull, or <see cref="TimeSpan.Zero"/> otherwise.</returns>
        public TimeSpan ParseTimeSpanOrStoreMessage(string timeSpanText, string propertyName)
        {
            TimeSpan tempTimeSpan;
            if (!TryParseTimeSpanOrStoreMessage(timeSpanText, propertyName, out tempTimeSpan))
            {
                tempTimeSpan = TimeSpan.Zero; // all reported already!
            }

            return tempTimeSpan;
        }

        /// <summary>
        /// Tries to parse a <see cref="TimeSpan"/> from a string and validates successfully if it is >= <paramref name="target"/>.
        /// </summary>
        /// <param name="timeSpanText">The text to parse as <see cref="TimeSpan"/></param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <param name="target">The target value to compare the parsed <see cref="TimeSpan"/> to.</param>
        /// <returns>Resulting <see cref="TimeSpan"/> if successfull, or <see cref="TimeSpan.Zero"/> otherwise.</returns>
        public TimeSpan ParseTimeSpanGreaterThanOrEqualElseStoreMessage(string timeSpanText, string propertyName, TimeSpan target)
        {
            TimeSpan tempTimeSpan;
            if (!TryParseTimeSpanOrStoreMessage(timeSpanText, propertyName, out tempTimeSpan))
            {
                return TimeSpan.Zero; // all reported already!
            }

            if (tempTimeSpan < target)
            {
                AddErrorMessage(propertyName, "TimeSpan shall be >= {0:g}, but the \"{1}\" evaluated to {2:g}", target, timeSpanText, tempTimeSpan);
                return TimeSpan.Zero;
            }

            return tempTimeSpan;
        }

        /// <summary>
        /// Tries to parse a <see cref="TimeSpan"/> from a string and validates successfully if it is > <paramref name="target"/>.
        /// </summary>
        /// <param name="timeSpanText">The text to parse as <see cref="TimeSpan"/></param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <param name="target">The target value to compare the parsed <see cref="TimeSpan"/> to.</param>
        /// <returns>Resulting <see cref="TimeSpan"/> if successfull, or <see cref="TimeSpan.Zero"/> otherwise.</returns>
        public TimeSpan ParseTimeSpanGreaterThanElseStoreMessage(string timeSpanText, string propertyName, TimeSpan target)
        {
            TimeSpan tempTimeSpan;
            if (!TryParseTimeSpanOrStoreMessage(timeSpanText, propertyName, out tempTimeSpan))
            {
                return TimeSpan.Zero; // all reported already!
            }

            if (tempTimeSpan <= target)
            {
                AddErrorMessage(propertyName, "TimeSpan shall be > {0:g}, but the \"{1}\" evaluated to {2:g}", target, timeSpanText, tempTimeSpan);
                return TimeSpan.Zero;
            }

            return tempTimeSpan;
        }

        /// <summary>
        /// Tries to parse a <see cref="TimeSpan"/> from a string and validates successfully if it is &lt;= <paramref name="target"/>.
        /// </summary>
        /// <param name="timeSpanText">The text to parse as <see cref="TimeSpan"/></param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <param name="target">The target value to compare the parsed <see cref="TimeSpan"/> to.</param>
        /// <returns>Resulting <see cref="TimeSpan"/> if successfull, or <see cref="TimeSpan.Zero"/> otherwise.</returns>
        public TimeSpan ParseTimeSpanLessThanOrEqualElseStoreMessage(string timeSpanText, string propertyName, TimeSpan target)
        {
            TimeSpan tempTimeSpan;
            if (!TryParseTimeSpanOrStoreMessage(timeSpanText, propertyName, out tempTimeSpan))
            {
                return TimeSpan.Zero; // all reported already!
            }

            if (tempTimeSpan > target)
            {
                AddErrorMessage(propertyName, "TimeSpan shall be <= {0:g}, but the \"{1}\" evaluated to {2:g}", target, timeSpanText, tempTimeSpan);
                return TimeSpan.Zero;
            }

            return tempTimeSpan;
        }

        /// <summary>
        /// Tries to parse a <see cref="TimeSpan"/> from a string and validates successfully if it is &lt; <paramref name="target"/>.
        /// </summary>
        /// <param name="timeSpanText">The text to parse as <see cref="TimeSpan"/></param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <param name="target">The target value to compare the parsed <see cref="TimeSpan"/> to.</param>
        /// <returns>Resulting <see cref="TimeSpan"/> if successfull, or <see cref="TimeSpan.Zero"/> otherwise.</returns>
        public TimeSpan ParseTimeSpanLessThanElseStoreMessage(string timeSpanText, string propertyName, TimeSpan target)
        {
            TimeSpan tempTimeSpan;
            if (!TryParseTimeSpanOrStoreMessage(timeSpanText, propertyName, out tempTimeSpan))
            {
                return TimeSpan.Zero; // all reported already!
            }

            if (tempTimeSpan >= target)
            {
                AddErrorMessage(propertyName, "TimeSpan shall be < {0:g}, but the \"{1}\" evaluated to {2:g}", target, timeSpanText, tempTimeSpan);
                return TimeSpan.Zero;
            }

            return tempTimeSpan;
        }

        /// <summary>
        /// Tries to parse a <see cref="TimeSpan"/> from a string and validates successfully if it is:
        /// if <paramref name="inclusive"/> == true and >= <paramref name="min"/> and &lt;= <paramref name="max"/>, or:
        /// if <paramref name="inclusive"/> == false and > <paramref name="min"/> and &lt; <paramref name="max"/>.
        /// </summary>
        /// <param name="timeSpanText">The text to parse as <see cref="TimeSpan"/></param>
        /// <param name="propertyName">The name of the related property.</param>
        /// <param name="min">The minimal timespan accepted</param>
        /// <param name="max">The maximal timespan accepted</param>
        /// <param name="inclusive">If true, then <paramref name="min"/> and <paramref name="max"/> are alloved values, else not.</param>
        /// <returns>Resulting <see cref="TimeSpan"/> if successfull, or <see cref="TimeSpan.Zero"/> otherwise.</returns>
        public TimeSpan ParseTimeSpanBetweenOrStoreMessage(string timeSpanText, string propertyName, TimeSpan min, TimeSpan max, bool inclusive)
        {
            Throw.If(min > max, nameof(min), "{0}: {1:g} is not <= {2}: {3:g}", nameof(min), min, nameof(max), max);

            TimeSpan tempTimeSpan;
            if (!TryParseTimeSpanOrStoreMessage(timeSpanText, propertyName, out tempTimeSpan))
            {
                return TimeSpan.Zero; // all reported already!
            }

            if (inclusive && (tempTimeSpan < min || tempTimeSpan > max))
            {
                AddErrorMessage(propertyName, "TimeSpan shall be in range: {0:g} <= X <= {1:g}, but is: {2:g}", min, max, tempTimeSpan);
                return TimeSpan.Zero;
            }
            else if (!inclusive && (tempTimeSpan <= min || tempTimeSpan >= max))
            {
                AddErrorMessage(propertyName, "TimeSpan shall be in range: {0:g} < X < {1:g}, but is: {2:g}", min, max, tempTimeSpan);
                return TimeSpan.Zero;
            }

            return tempTimeSpan;
        }

        #endregion Derived "Particle" Validation Methods

        #region Single SubResult or SubComponent Validations
        /// <summary>
        /// Checks if the <paramref name="subResult"/> is null or valid. Only if it is not null and invalid, this <see cref="ValidationResult"/> will be invalidated
        /// and the <paramref name="subResult"/>'s <see cref="ValidationResult.InvalidReason"/>
        /// will be added as a message particle in own invalid reasons.
        /// </summary>
        /// <param name="subResult">the parameter to check</param>
        /// <returns>the true if the <paramref name="subResult"/> is null or valid, false otherwise (i.e. not null and invalid).</returns>
        public bool IntegrateSubResult(ValidationResult subResult)
        {
            if (subResult == null || subResult.IsValid)
            {
                return true;
            }

            // Integrate each single message;
            foreach (var oneInvalidReason in subResult.InvalidReasons)
            {
                AddErrorMessage(subResult.NameOfRoot, oneInvalidReason.EscapeForFurtherFormatting());
            }

            return false;
        }

        /// <summary>
        /// Checks if the <paramref name="subResult"/> is null or valid. Only if it is not null and invalid, this <see cref="ValidationResult"/> will be invalidated
        /// and the <paramref name="subResult"/>'s <see cref="IValidationResult.InvalidReason"/>
        /// will be added as a message particle in own invalid reasons.
        /// </summary>
        /// <param name="subResult">the parameter to check</param>
        /// <returns>the true if the <paramref name="subResult"/> is null or valid, false otherwise (i.e. not null and invalid).</returns>
        public bool IntegrateSubResult(IValidationResult subResult)
        {
            return IntegrateSubResult(subResult, null);
        }

        /// <summary>
        /// Checks if the <paramref name="subResult"/> is null or valid. Only if it is not null and invalid, this <see cref="ValidationResult"/> will be invalidated
        /// and the <paramref name="subResult"/>'s <see cref="IValidationResult.InvalidReason"/>
        /// will be added as a message particle in own invalid reasons.
        /// </summary>
        /// <param name="subResult">the parameter to check</param>
        /// <param name="propertyName">Optional. Can be nul. If provided, it will be added as a prefix for each of <paramref name="subResult"/>'s <see cref="IValidationResult.InvalidReasons"/>.</param>
        /// <returns>the true if the <paramref name="subResult"/> is null or valid, false otherwise (i.e. not null and invalid).</returns>
        public bool IntegrateSubResult(IValidationResult subResult, string propertyName)
        {
            if (subResult == null || subResult.IsValid)
            {
                return true;
            }

            // Integrate each single message;
            foreach (var oneInvalidReason in subResult.InvalidReasons)
            {
                AddErrorMessage(propertyName, oneInvalidReason.EscapeForFurtherFormatting());
            }

            return false;
        }
        #endregion Single SubResult or SubComponent Validations
    }
}
