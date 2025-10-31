using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJsNetExtensions.ObjectNavigation;

namespace MJsNetExtensions.ObjectValidation
{
    /// <summary>
    /// The pre- and post-process item handler for Strongly-Typed Object Validation.
    /// It defines the validation acction to be executed on each hierarchy structure item by <see cref="PreAndPostVisitingTypeHierarchyIterator{T}"/> in.
    /// </summary>
    public class ValidationPreAndPostProcessItemHandler<T> : IPreAndPostProcessItemHandler<T>
    {
        #region Fields

        private Action<T, ValidationResult> preProcessItem;
        private Action<T, ValidationResult> postProcessItem;

        #endregion Fields

        #region Construction / Destruction
        /// <summary>
        /// The object to validate, i.e. root of validation.
        /// </summary>
        /// <param name="validatableRoot"></param>
        /// <param name="preProcessItem">The pre-process acction to be executed on each hierarchy structure item.</param>
        /// <param name="postProcessItem">The post-process acction to be executed on each hierarchy structure item.</param>
        /// <param name="settings">The <see cref="ValidationSettings"/>.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="validatableRoot"/> is null.</exception>
        public ValidationPreAndPostProcessItemHandler([ValidatedNotNull] T validatableRoot, Action<T, ValidationResult> preProcessItem, Action<T, ValidationResult> postProcessItem, ValidationSettings settings)
        {
            //Throw.IfNull(validatable, nameof(validatable)); --> validated if not null in constructor of: ValidationResult
            this.ValidationResult = new ValidationResult(validatableRoot);
            this.ValidatableRoot = validatableRoot;

            Throw.If(preProcessItem == null && postProcessItem == null, nameof(preProcessItem), "at least one of: {0} or {1} have to be provided.", nameof(preProcessItem), nameof(postProcessItem));
            this.preProcessItem = preProcessItem;
            this.postProcessItem = postProcessItem;

            if (settings != null)
            {
                this.Settings = settings;
            }
        }
        #endregion Construction / Destruction

        #region Properties

        /// <summary>
        /// The root object of validation
        /// </summary>
        public T ValidatableRoot { get; private set; }

        /// <summary>
        /// The result of validation.
        /// </summary>
        public ValidationResult ValidationResult { get; private set; }

        /// <summary>
        /// The Strongly-Typed Object Validation settings - <see cref="ValidationSettings"/>.
        /// </summary>
        public ValidationSettings Settings { get; private set; } = new();

        /// <summary>
        /// If not null, then one of the custom Pre or Post Structure Validation methods of some valiadation particle in the hierarchy threw an Exception.
        /// </summary>
        public Exception CustomException { get; private set; }

        #endregion Properties

        #region API - Public Methods

        /// <summary>
        /// Item pre-structure-process handling method.
        /// </summary>
        /// <param name="hierarchyPathItem">The hierarchy path item to call pre-process on.</param>
        public void ItemPreProcess(HierarchyPathItem<T> hierarchyPathItem)
        {
            // Pre-custom validation processing
            Throw.IfNull(hierarchyPathItem, nameof(hierarchyPathItem));

            this.ValidationResult.CurrentObjectPath = hierarchyPathItem.ItemPath;

            // The custom validation call:
            try
            {
                preProcessItem?.Invoke(hierarchyPathItem.Item, this.ValidationResult);
            }
            catch (Exception ex)
            {
                this.CustomException = ex;
                hierarchyPathItem.StopProcessing = true;
                this.ValidationResult.AddErrorMessage(null, $"Custom Pre Structure Validation method threw an exception: {ex}");
            }

            // Post-custom validation processing
            if (this.Settings.StopOnFirstError && !this.ValidationResult.IsValid)
            {
                hierarchyPathItem.StopProcessing = true;
            }
        }

        /// <summary>
        /// Item post-structure-process handling method.
        /// </summary>
        /// <param name="hierarchyPathItem">The hierarchy path item to call post-process on.</param>
        public void ItemPostProcess(HierarchyPathItem<T> hierarchyPathItem)
        {
            // Pre-custom validation processing:
            Throw.IfNull(hierarchyPathItem, nameof(hierarchyPathItem));

            this.ValidationResult.CurrentObjectPath = hierarchyPathItem.ItemPath;

            // The custom validation call:
            try
            {
                this.postProcessItem?.Invoke(hierarchyPathItem.Item, this.ValidationResult);
            }
            catch (Exception ex)
            {
                this.CustomException = ex;
                hierarchyPathItem.StopProcessing = true;
                this.ValidationResult.AddErrorMessage(null, $"Custom Post Structure Validation method threw an exception: {ex}");
            }

            // Post-custom validation processing
            if (this.Settings.StopOnFirstError && !this.ValidationResult.IsValid)
            {
                hierarchyPathItem.StopProcessing = true;
            }
        }

        #endregion API - Public Methods
    }
}
