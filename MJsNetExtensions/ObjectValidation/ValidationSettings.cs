using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJsNetExtensions.ObjectNavigation;

namespace MJsNetExtensions.ObjectValidation
{
    /// <summary>
    /// Summary description for ValidationSettings
    /// </summary>
    public class ValidationSettings : TypeHierarchyIteratorSettings
    {
        #region Properties

        /// <summary>
        /// If set to true by caller (validation requestor), then the validation will stop on first error.
        /// </summary>
        public bool StopOnFirstError { get; set; }

        #endregion Properties

        #region API - Public Methods

        /// <summary>
        /// Clone mehtod
        /// </summary>
        /// <returns></returns>
        public override TypeHierarchyIteratorSettings Clone()
        {
            return base.Clone();
            //return new ValidationSettings
            //{
            //    //ListPublicProperties = this.ListPublicProperties,
            //    ListNonpublicProperties = this.ListNonpublicProperties,
            //    ListPublicFields = this.ListPublicFields,
            //    ListNonpublicFields = this.ListNonpublicFields,
            //    StopOnFirstError = this.StopOnFirstError,
            //};
        }

        #endregion API - Public Methods
    }
}
