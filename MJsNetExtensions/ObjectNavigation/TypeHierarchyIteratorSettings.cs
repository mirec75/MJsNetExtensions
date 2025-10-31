namespace MJsNetExtensions.ObjectNavigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Summary description for TypeHierarchyIteratorSettings
    /// </summary>
    public class TypeHierarchyIteratorSettings
    {
        #region Properties

        /// <summary>
        /// Flag indicating if the public instance properties have to be listed. In current implementation it is always true and can not be modified.
        /// </summary>
        public bool ListPublicProperties => true;

        /// <summary>
        /// Flag indicating if the NON public instance properties have to be listed. Default is: false.
        /// </summary>
        public bool ListNonpublicProperties { get; set; }

        /// <summary>
        /// Flag indicating if the public instance Fields have to be listed. Default is: false.
        /// </summary>
        public bool ListPublicFields { get; set; }

        /// <summary>
        /// Flag indicating if the NON public instance Fields have to be listed. Default is: false.
        /// </summary>
        public bool ListNonpublicFields { get; set; }

        #endregion Properties

        #region API - Public Methods

        /// <summary>
        /// Clone mehtod
        /// </summary>
        /// <returns></returns>
        public virtual TypeHierarchyIteratorSettings Clone()
        {
            // make ShallowCopy first:
            return (TypeHierarchyIteratorSettings)this.MemberwiseClone();

            //return new TypeHierarchyIteratorSettings
            //{
            //    //ListPublicProperties = this.ListPublicProperties,
            //    ListNonpublicProperties = this.ListNonpublicProperties,
            //    ListPublicFields = this.ListPublicFields,
            //    ListNonpublicFields = this.ListNonpublicFields,
            //};
        }

        #endregion API - Public Methods
    }
}
