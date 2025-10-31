namespace MJsNetExtensions.ObjectNavigation
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using MJsNetExtensions.ObjectNavigation.Internal;

    /// <summary>
    /// Depth first search Pre and Post Visiting Strongly-Typed Object Hierarchy Iterator helper class used e.g. for Strongly-Typed Object Validation. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PreAndPostVisitingTypeHierarchyIterator<T>
    {
        #region Statics and Consts

        private static ConcurrentDictionary<PublicNonPublicSettings, List<PropertyInfo>> settingsWithTypeToPropertyInfoDict = new();
        private static ConcurrentDictionary<PublicNonPublicSettings, List<FieldInfo>> settingsWithTypeToFieldInfoDict = new();

        #endregion Statics and Consts


        #region Fields

        private PublicNonPublicSettings propertiesSettings;
        private PublicNonPublicSettings fieldsSettings;

        #endregion Fields

        #region Construction / Destruction
        /// <summary>
        /// User default construction is intentionally prohibited
        /// </summary>
        private PreAndPostVisitingTypeHierarchyIterator() { }


        /// <summary>
        /// Pre- and Post-Process Strongly-Typed Object Structure
        /// </summary>
        /// <param name="structureRoot">Its ok, if structureRoot is null.</param>
        /// <param name="settings">Optional settings. May be null.</param>
        /// <param name="preAndPostProcessItemHandler">The pre-process item handler defining the acction to be executed on each hierarchy structure item.</param>
        public static void PreAndPostProcessStructure(
            T structureRoot,
            TypeHierarchyIteratorSettings settings,
            IPreAndPostProcessItemHandler<T> preAndPostProcessItemHandler
            )
        {
            Type rootType = typeof(T);
            Throw.InvalidOperationIf(IsSimple(rootType), "The type of {0}: is {1} which is a simple type, which is not allowed.", nameof(structureRoot), rootType);
            Throw.IfNull(preAndPostProcessItemHandler, nameof(preAndPostProcessItemHandler));

            if (structureRoot == null)
            {
                return;
            }

            PreAndPostVisitingTypeHierarchyIterator<T> context = new PreAndPostVisitingTypeHierarchyIterator<T>
            {
                PreAndPostProcessItemHandler = preAndPostProcessItemHandler,
            };

            context.Initialize(settings);

            HierarchyPathItem<T> pathItem = new HierarchyPathItem<T>(null, structureRoot, structureRoot.GetType().Name);

            context.PreAndPostProcessRecursively(pathItem);
        }

        #endregion Construction / Destruction

        #region Properties

        private TypeHierarchyIteratorSettings Settings { get; set; } = new();

        private IPreAndPostProcessItemHandler<T> PreAndPostProcessItemHandler { get; set; }

        /// <summary>
        /// A <see cref="HashSet{T}"/> for keeping track of objects marked as already processed to avoid reprocessing and endless recursion on cyclic references or several references to the same object.
        /// </summary>
        private HashSet<T> ProcessedItems { get; } = new();

        /// <summary>
        /// A helper to ansure hierarchy first processing of the structure. I.e.: Owner -> Child relations have to be served in the first place before sibling relations.
        /// </summary>
        private HashSet<T> ItemsReservedForProcessing { get; } = new();

        /// <summary>
        /// If this property is set to true as a result of <see cref="IPreAndPostProcessItemHandler{T}.ItemPreProcess(HierarchyPathItem{T})"/> or <see cref="IPreAndPostProcessItemHandler{T}.ItemPostProcess(HierarchyPathItem{T})"/>,
        /// then the <see cref="PreAndPostVisitingTypeHierarchyIterator{T}"/> stops any further processing immediately.
        /// </summary>
        private bool StopProcessing { get; set; }

        #endregion Properties

        #region Private Methods
        /// <summary>
        /// Mandatory internal initialzatin to be called from a factory method only
        /// </summary>
        /// <param name="settings"></param>
        private void Initialize(TypeHierarchyIteratorSettings settings)
        {
            if (settings != null)
            {
                this.Settings = settings.Clone();
            }

            this.propertiesSettings = new PublicNonPublicSettings
            {
                ListPublic = this.Settings.ListPublicProperties,
                ListNonpublic = this.Settings.ListNonpublicProperties,
            };

            this.fieldsSettings = new PublicNonPublicSettings
            {
                ListPublic = this.Settings.ListPublicFields,
                ListNonpublic = this.Settings.ListNonpublicFields,
            };
        }

        /// <summary>
        /// Internal recursive structure iteration method.
        /// </summary>
        /// <param name="pathItem"></param>
        private void PreAndPostProcessRecursively(HierarchyPathItem<T> pathItem)
        {
            // ------------------
            // Protection checks:
            if (pathItem == null ||         //NOTE: this and next line can not be simplified to: (pathItem?.Item == null) because of "T"
                pathItem.Item == null ||
                !this.ProcessedItems.Add(pathItem.Item)   // == endless recursion protection
                )
            {
                return;
            }

            // protection of non structural (cross structural) processing. Consider: Owner -> Child relations have to be served in the first place before sibling relations! See: DetailsLogDataTest1 in UnitTests
            if (this.ItemsReservedForProcessing.Contains(pathItem.Item))
            {
                return;
            }

            // ------------------------
            // Pre-Structure-Processing

            //NOTE: if the user implementation is broken and throws, then DON'T CATCH and let the exception fly through to the caller of the PreAndPostVisitingTypeHierarchyIterator.
            try
            {
                this.PreAndPostProcessItemHandler.ItemPreProcess(pathItem);
            }
            catch (Exception ex)
            {
                this.StopProcessing = true;
                throw new InvalidOperationException($"{pathItem.ItemPath}: Custom Pre Process method threw an exception", ex);
            }

            if (this.StopProcessing || pathItem.StopProcessing)
            {
                return;
            }

            // -----------
            // Do recurse:

            // 1st Collect all children, to be protected for being processes by subcomponents of this object.
            // The combination of childrenToProcess and this.itemsReservedForProcessing helps to prioritize the Owner -> Child relations higher, before sibling relations! See: DetailsLogDataTest1 in UnitTests
            List<HierarchyPathItem<T>> childrenToProcess = new List<HierarchyPathItem<T>>();

            // First all Properties:
            foreach (PropertyInfo property in GetNonSimpleProperties(pathItem.Item))
            {
                CollectChildrenToProcess(pathItem, property.PropertyType, () => property.GetValue(pathItem.Item), property.Name, childrenToProcess);
            }

            // Then all Fields:
            foreach (FieldInfo field in GetNonSimpleFields(pathItem.Item))
            {
                CollectChildrenToProcess(pathItem, field.FieldType, () => field.GetValue(pathItem.Item), field.Name, childrenToProcess);
            }

            // Do recurse own children:
            foreach (HierarchyPathItem<T> childPathItem in childrenToProcess)
            {
                // only my children (from: childrenToProcess) may be removed from: itemsReservedForProcessing and processed.
                Throw.InvalidOperationIfNot(this.ItemsReservedForProcessing.Remove(childPathItem.Item), "{0} not found in ItemsReservedForProcessing!", childPathItem.ItemPath ?? childPathItem.ItemName);
                PreAndPostProcessRecursively(childPathItem);

                if (this.StopProcessing || childPathItem.StopProcessing)
                {
                    return;
                }
            }

            // -------------------------
            // Post-Structure-Processing

            //NOTE: if the user implementation is broken and throws, then DON'T CATCH and let the exception fly through to the caller of the PreAndPostVisitingTypeHierarchyIterator.
            try
            {
                this.PreAndPostProcessItemHandler.ItemPostProcess(pathItem);
            }
            catch (Exception ex)
            {
                this.StopProcessing = true;
                throw new InvalidOperationException($"{pathItem.ItemPath}: Custom Post Process method threw an exception", ex);
            }
        }

        /// <summary>
        /// Internal collect of direct children or another related objects of type T (owner, siblings, etc.)
        /// </summary>
        /// <param name="parentPathItem"></param>
        /// <param name="valueType"></param>
        /// <param name="valueGetter"></param>
        /// <param name="childName">is the property or field name.</param>
        /// <param name="childrenToProcess"></param>
        /// <returns></returns>
        private bool CollectChildrenToProcess(HierarchyPathItem<T> parentPathItem, Type valueType, Func<object> valueGetter, string childName, List<HierarchyPathItem<T>> childrenToProcess)
        {
            //Throw.IfNull(valueType, nameof(valueType));
            //Throw.IfNull(valueGetter, nameof(valueGetter));
            //Throw.IfNull(childrenToProcess, nameof(childrenToProcess));

            bool collected4processing = false;

            if (typeof(T).IsAssignableFrom(valueType))
            {
                TryRegisterAsChildForProcessing(parentPathItem, (T)valueGetter(), childName, childrenToProcess);
                collected4processing = true;
            }
            else if (typeof(IEnumerable<T>).IsAssignableFrom(valueType))
            {
                int ii = 0;
                foreach (T subComponent in valueGetter() as IEnumerable<T> ?? [])
                {
                    TryRegisterAsChildForProcessing(parentPathItem, subComponent, $"{childName}[{ii++}]", childrenToProcess);
                }
                collected4processing = true;
            }
            else if (typeof(IEnumerable<T>).IsAssignableFrom(valueType))
            {
                int ii = 0;
                foreach (T subComponent in valueGetter() as IEnumerable<T> ?? [])
                {
                    TryRegisterAsChildForProcessing(parentPathItem, subComponent, $"{childName}[{ii++}]", childrenToProcess);
                }
                collected4processing = true;
            }
            //NOTE: this 2 if-s did not work!
            //else if (typeof(IDictionary<object, T>).IsAssignableFrom(valueType)) { ... }
            //else if (typeof(IDictionary<T, object>).IsAssignableFrom(valueType)) { ... }
            else if (typeof(IDictionary).IsAssignableFrom(valueType))
            {
                collected4processing = HandleIDictionary(parentPathItem, valueType, valueGetter, childName, childrenToProcess);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(valueType) &&
                     valueGetter() is IEnumerable enumerable)
            {
                int ii = 0;
                foreach (object subComponent in enumerable)
                {
                    if (subComponent is T)
                    {
                        TryRegisterAsChildForProcessing(parentPathItem, (T)subComponent, $"{childName}[{ii}]", childrenToProcess);
                    }
                    ii++;
                }
                //NOTE: can not set: handled = true;    --> what if some objects of the IEnumerable implement TMain and some just TAlternative ?!
                //      That's why I'm not marking this as handled. On the other hand if an object would get processed twice, the recursion protection "processedItems" would prohibit this.
            }

            return collected4processing;
        }

        private void TryRegisterAsChildForProcessing(HierarchyPathItem<T> parentPathItem, T childItem, string itemName, List<HierarchyPathItem<T>> childrenToProcess)
        {
            if (childItem != null &&
                !this.ProcessedItems.Contains(childItem) &&   // == endless recursion protection
                this.ItemsReservedForProcessing.Add(childItem)
                )
            {

                HierarchyPathItem<T> childPathItem = new HierarchyPathItem<T>(parentPathItem, childItem, itemName);
                childrenToProcess.Add(childPathItem);
            }
        }

        private bool HandleIDictionary(HierarchyPathItem<T> parentPathItem, Type valueType, Func<object> valueGetter, string childName, List<HierarchyPathItem<T>> childrenToProcess)
        {
            bool collected4processing;
            bool isDictGeneric = false;
            bool isKeySimpleType = false;
            bool handleKeys = false;
            bool handleDictValues = false;
            Type[] arguments = valueType.GetGenericArguments();

            if (valueType.IsGenericType && arguments?.Length >= 2)
            {
                Type keyType = arguments[0];
                Type dictValueType = arguments[1];

                isDictGeneric = true;
                isKeySimpleType = IsSimple(keyType);
                handleKeys = typeof(T).IsAssignableFrom(keyType);
                handleDictValues = typeof(T).IsAssignableFrom(dictValueType);

                //NOTE: I let here the TMain --> TAlternative falback a chance, for a case, if values are e.g. TAlternative and dict values are TMain
                collected4processing = handleKeys && handleDictValues;
            }
            else
            {
                handleKeys = true;
                handleDictValues = true;

                //NOTE: I let here the TMain --> TAlternative falback a chance, for this non-generic dict, where TAlternative and TMain can be present in both keys and values in a mix.
                collected4processing = false;
            }

            if (handleKeys || handleDictValues)
            {
                IDictionary dict = (valueGetter() as IDictionary);

                if (dict == null)
                {
                    collected4processing = true; // stop any fallback
                }
                else
                {
                    // Try hanle both if possible: the Keys and the Dict-Values
                    int ii = 0;
                    foreach (DictionaryEntry kvp in dict)
                    {
                        if (handleKeys && kvp.Key is T)
                        {
                            TryRegisterAsChildForProcessing(parentPathItem, (T)kvp.Key, $"{childName}.Keys[{ii}]", childrenToProcess);
                        }

                        if (handleDictValues && kvp.Value is T)
                        {
                            bool keyIsSimple = false;
                            if (isDictGeneric)
                            {
                                if (isKeySimpleType)
                                {
                                    keyIsSimple = true;
                                }
                            }
                            else
                            {
                                if (kvp.Key == null || IsSimple(kvp.Key.GetType()))
                                {
                                    keyIsSimple = true;
                                }
                            }

                            string nameAddition;
                            if (keyIsSimple)
                            {
                                string keyStr = kvp.Key?.ToString() ?? "<null>";
                                if (keyStr.Length > 100)
                                {
                                    keyStr = keyStr.Substring(0, 100);
                                }
                                nameAddition = $"[{keyStr}]";
                            }
                            else
                            {
                                nameAddition = $".Values[{ii}]";
                            }

                            TryRegisterAsChildForProcessing(parentPathItem, (T)kvp.Value, $"{childName}{nameAddition}", childrenToProcess);
                        }

                        ii++;
                    }
                }
            }

            return collected4processing;
        }

        /// <summary>
        /// Get (cached) Non simple properties according to <see cref="PreAndPostVisitingTypeHierarchyIterator{T}.Settings"/> of the <paramref name="propertiesContainer"/>.
        /// </summary>
        /// <param name="propertiesContainer">The container <see cref="Type"/> which properties have to be returned.</param>
        /// <returns>A collection of <see cref="PropertyInfo"/>-s of the properties</returns>
        private List<PropertyInfo> GetNonSimpleProperties(object propertiesContainer)
        {
            //Throw.IfNull(propertiesContainer, nameof(propertiesContainer));

            if (!this.propertiesSettings.ListPublic &&
                !this.propertiesSettings.ListNonpublic
                )
            {
                return new List<PropertyInfo>();
            }

            PublicNonPublicSettings currentPropSettings = this.propertiesSettings.Clone();
            currentPropSettings.ContainerType = propertiesContainer.GetType();

            List<PropertyInfo> propList;
            if (!settingsWithTypeToPropertyInfoDict.TryGetValue(currentPropSettings, out propList))
            {
                BindingFlags flags = BindingFlags.Instance;

                if (this.Settings.ListPublicProperties)
                {
                    flags |= BindingFlags.Public;
                }

                if (this.Settings.ListNonpublicProperties)
                {
                    flags |= BindingFlags.NonPublic;
                }

                propList = currentPropSettings.ContainerType
                    .GetProperties(flags)
                    .Where(it =>
                        !IsSimple(it.PropertyType)
                        )
                    .ToList()
                    ;

                settingsWithTypeToPropertyInfoDict.TryAdd(currentPropSettings, propList);
            }

            return propList;
        }

        /// <summary>
        /// Get (cached) Non simple fields according to <see cref="PreAndPostVisitingTypeHierarchyIterator{T}.Settings"/> of the <paramref name="fieldsContainer"/>.
        /// </summary>
        /// <param name="fieldsContainer">The container <see cref="Type"/> which fields have to be returned.</param>
        /// <returns>A collection of <see cref="FieldInfo"/>-s of the fields</returns>
        private List<FieldInfo> GetNonSimpleFields(object fieldsContainer)
        {
            //Throw.IfNull(fieldsContainer, nameof(fieldsContainer));

            if (!this.fieldsSettings.ListPublic &&
                !this.fieldsSettings.ListNonpublic
                )
            {
                return new List<FieldInfo>();
            }

            PublicNonPublicSettings currentFieldsSettings = this.fieldsSettings.Clone();
            currentFieldsSettings.ContainerType = fieldsContainer.GetType();

            List<FieldInfo> fieldsList;
            if (!settingsWithTypeToFieldInfoDict.TryGetValue(currentFieldsSettings, out fieldsList))
            {
                BindingFlags flags = BindingFlags.Instance;

                if (this.Settings.ListPublicFields)
                {
                    flags |= BindingFlags.Public;
                }

                if (this.Settings.ListNonpublicFields)
                {
                    flags |= BindingFlags.NonPublic;
                }

                fieldsList = currentFieldsSettings.ContainerType
                    .GetFields(flags)
                    .Where(it =>
                        it?.Name?.Length > 0 &&     //NOTE: this shall never happen. But -> skip if the prop has no name.
                        it.Name[0] != '<' &&        // skip backing fields, like: "<Owner>k__BackingField"
                        !IsSimple(it.FieldType)     // skip all simple types
                        )
                    .ToList()
                    ;

                settingsWithTypeToFieldInfoDict.TryAdd(currentFieldsSettings, fieldsList);
            }

            return fieldsList;
        }

        /// <summary>
        /// Helper for checking a Type if it belongs to simple types, like int, char, string, DateTime, etc.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsSimple(Type type)
        {
            //Throw.IfNull(type, nameof(type));

            // copied from:
            // How do I tell if a type is a “simple” type? i.e. holds a single value
            // https://stackoverflow.com/questions/863881/how-do-i-tell-if-a-type-is-a-simple-type-i-e-holds-a-single-value

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(typeInfo.GetGenericArguments()[0]);
            }

            return typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(DateTime))
              || type.Equals(typeof(TimeSpan))
              ;
        }
        #endregion Private Methods
    }
}
