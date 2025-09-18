namespace MJsNetExtensions.ObjectNavigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for pre and post object structure (subtree) processing item handler.
    /// The pre-process item handler defines the acction to be executed on each hierarchy structure item by <see cref="PreAndPostVisitingTypeHierarchyIterator{T}"/>.
    /// </summary>
    public interface IPreAndPostProcessItemHandler<T>
    {
        /// <summary>
        /// Item pre-structure-process handling method.
        /// </summary>
        /// <param name="hierarchyPathItem">The hierarchy path item to call pre-process on.</param>
        void ItemPreProcess(HierarchyPathItem<T> hierarchyPathItem);

        /// <summary>
        /// Item post-structure-process handling method.
        /// </summary>
        /// <param name="hierarchyPathItem">The hierarchy path item to call post-process on.</param>
        void ItemPostProcess(HierarchyPathItem<T> hierarchyPathItem);
    }
}
