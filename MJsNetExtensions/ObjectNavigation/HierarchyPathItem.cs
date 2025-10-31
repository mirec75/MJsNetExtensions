using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJsNetExtensions.ObjectNavigation
{
    /// <summary>
    /// Summary description for HierarchyPathItem
    /// </summary>
    public class HierarchyPathItem<T>
    {
        #region Fields
        #endregion Fields

        #region Construction / Destruction

        /// <summary>
        /// Constructing new <see cref="HierarchyPathItem{T}"/>.
        /// </summary>
        /// <param name="parent">Optional parent. If null, then this is the path root.</param>
        /// <param name="item">The item related by this <see cref="HierarchyPathItem{T}"/>.</param>
        /// <param name="itemName">The name of the related <seealso cref="HierarchyPathItem{T}.Item"/></param>
        public HierarchyPathItem(HierarchyPathItem<T> parent, T item, string itemName)
        {
            this.Parent = parent;

            this.Item = item.ThrowIfNull(nameof(item));
            this.ItemName = itemName.ThrowIfNullOrWhiteSpace(nameof(itemName));

            //NOTE: intentionally let the rooth's path be empty!
            //      This is because of IValidationResult.InvalidReasonPrefix, where the root's object type is already noted.
            //      So I don't want to have it doubled.
            if (this.Parent != null)
            {
                // only prepend the own ItemPath with parent's one, if it is not empty, i.e. if parent is not the root.
                if (this.Parent.ItemPath != null)
                {
                    this.ItemPath = this.Parent.ItemPath + "/";
                }

                this.ItemPath += this.ItemName;
            }
        }
        #endregion Construction / Destruction

        #region Properties

        /// <summary>
        /// An optional parent <see cref="HierarchyPathItem{T}"/>. If null, then this is the path root.
        /// </summary>
        public HierarchyPathItem<T> Parent { get; private set; }

        /// <summary>
        /// The <seealso cref="HierarchyPathItem{T}.Item"/> related by this <see cref="HierarchyPathItem{T}"/>.
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        /// The name of the related <seealso cref="HierarchyPathItem{T}.Item"/>.
        /// </summary>
        public string ItemName { get; private set; }

        /// <summary>
        /// The full path of the related <seealso cref="HierarchyPathItem{T}.Item"/> from the root.
        /// </summary>
        public string ItemPath { get; private set; }

        /// <summary>
        /// If this property is set to true during <see cref="IPreAndPostProcessItemHandler{T}.ItemPreProcess(HierarchyPathItem{T})"/> or <see cref="IPreAndPostProcessItemHandler{T}.ItemPostProcess(HierarchyPathItem{T})"/>,
        /// then the <see cref="PreAndPostVisitingTypeHierarchyIterator{T}"/> stops any further processing immediately.
        /// </summary>
        public bool StopProcessing { get; set; }

        #endregion Properties
    }
}
