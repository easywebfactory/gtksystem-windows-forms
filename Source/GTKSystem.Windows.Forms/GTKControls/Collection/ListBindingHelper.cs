using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms;

internal class ListBindingHelper
{
    /// <summary>Returns the name of an underlying list, given a data source and optional <see cref="T:System.ComponentModel.PropertyDescriptor" /> array.</summary>
    /// <returns>The name of the list in the data source, as described by <paramref name="listAccessors" />, orthe name of the data source type.</returns>
    /// <param name="list">The data source to examine for the list name.</param>
    /// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects to find in the data source. This can be null.</param>
    public static string? GetListName(object? list, PropertyDescriptor[]? listAccessors)
    {
        string? listNameFromType;
        if (list == null)
        {
            return string.Empty;
        }

        if (list is not ITypedList typedList)
        {
            Type propertyType;
            if (listAccessors == null || listAccessors.Length == 0)
            {
                propertyType = list is not Type type ? list.GetType() : type;
            }
            else
            {
                propertyType = listAccessors[0].PropertyType;
            }
            listNameFromType = GetListNameFromType(propertyType);
        }
        else
        {
            listNameFromType = typedList.GetListName(listAccessors);
        }
        return listNameFromType;
    }

    private static string? GetListNameFromType(Type type)
    {
        string? name;
        if (typeof(Array).IsAssignableFrom(type))
        {
            name = type.GetElementType()?.Name;
        }
        else if (!typeof(IList).IsAssignableFrom(type))
        {
            name = type.Name;
        }
        else
        {
            var typedIndexer = GetTypedIndexer(type);
            name = typedIndexer == null ? type.Name : typedIndexer.PropertyType.Name;
        }
        return name;
    }

    /// <summary>Returns the data type of the items in the specified data source.</summary>
    /// <returns>For complex data binding, the <see cref="T:System.Type" /> of the items represented by the <paramref name="dataMember" /> in the data source; otherwise, the <see cref="T:System.Type" /> of the item in the list itself.</returns>
    /// <param name="dataSource">The data source to examine for items. </param>
    /// <param name="dataMember">The optional name of the property on the data source that is to be used as the data member. This can be null.</param>
    public static Type? GetListItemType(object? dataSource, string? dataMember)
    {
        if (dataSource == null)
        {
            return typeof(object);
        }
        if (string.IsNullOrEmpty(dataMember))
        {
            return GetListItemType(dataSource);
        }
        var listItemProperties = GetListItemProperties(dataSource);
        if (listItemProperties == null)
        {
            return typeof(object);
        }
        var propertyDescriptor = listItemProperties.Find(dataMember, true);
        if (propertyDescriptor == null || propertyDescriptor.PropertyType.FullName == typeof(ICustomTypeDescriptor).FullName)
        {
            return typeof(object);
        }
        return GetListItemType(propertyDescriptor.PropertyType);
    }

    /// <summary>Returns an object, typically a list, from the evaluation of a specified data source and optional data member.</summary>
    /// <returns>An <see cref="T:System.Object" /> representing the underlying list if it was found; otherwise, <paramref name="dataSource" />.</returns>
    /// <param name="dataSource">The data source from which to find the list.</param>
    /// <param name="dataMember">The name of the data source property that contains the list. This can be null.</param>
    /// <exception cref="T:System.ArgumentException">The specified data member name did not match any of the properties found for the data source.</exception>
    public static object? GetList(object? dataSource, string? dataMember)
    {
        object? obj;
        dataSource = GetList(dataSource);
        if (dataSource == null || dataSource is Type || string.IsNullOrEmpty(dataMember))
        {
            return dataSource;
        }
        var propertyDescriptor = GetListItemProperties(dataSource)?.Find(dataMember, true);
        if (propertyDescriptor == null)
        {
            throw new ArgumentException(string.Format("DataSourceDataMemberPropNotFound {0}", dataMember ));
        }
        if (!(dataSource is ICurrencyManagerProvider))
        {
            obj = !(dataSource is IEnumerable) ? dataSource : GetFirstItemByEnumerable(dataSource as IEnumerable);
        }
        else
        {
            var currencyManager = (dataSource as ICurrencyManagerProvider)?.CurrencyManager;
            obj = currencyManager is { Position: >= 0 } && currencyManager.Position <= currencyManager.Count - 1 ? currencyManager.Current : null;
        }
        if (obj == null)
        {
            return null;
        }
        return propertyDescriptor.GetValue(obj);
    }

    public static object? GetList(object? list)
    {
        if (!(list is IListSource))
        {
            return list;
        }
        return (list as IListSource)?.GetList();
    }

    private static PropertyDescriptorCollection? GetListItemPropertiesByInstance(object? target, PropertyDescriptor?[]? listAccessors, int startIndex)
    {
        PropertyDescriptorCollection? properties=null;
        if (listAccessors == null || listAccessors.Length <= startIndex)
        {
            properties = TypeDescriptor.GetProperties(target, BrowsableAttributeList);
        }
        else
        {
            if (target != null)
            {
                var value = listAccessors[startIndex]?.GetValue(target);
                if (value != null)
                {
                    PropertyDescriptor?[]? propertyDescriptorArray = null;
                    if (listAccessors.Length > startIndex + 1)
                    {
                        var length = listAccessors.Length - (startIndex + 1);
                        propertyDescriptorArray = new PropertyDescriptor[length];
                        for (var i = 0; i < length; i++)
                        {
                            propertyDescriptorArray[i] = listAccessors[startIndex + 1 + i];
                        }
                    }
                    properties = GetListItemProperties(value, propertyDescriptorArray);
                }
                else
                {
                    properties = GetListItemPropertiesByType(listAccessors[startIndex]?.PropertyType, listAccessors, startIndex);
                }
            }
        }
        return properties;
    }

    private static PropertyDescriptorCollection? GetListItemPropertiesByType(Type? _, PropertyDescriptor?[]? listAccessors, int startIndex)
    {
        var propertyType = listAccessors?[startIndex]?.PropertyType;
        startIndex++;
        var propertyDescriptorCollections = startIndex < listAccessors?.Length ? GetListItemPropertiesByType(propertyType, listAccessors, startIndex) : GetListItemProperties(propertyType);
        return propertyDescriptorCollections;
    }

    public static PropertyDescriptorCollection? GetListItemProperties(object? list)
    {
        PropertyDescriptorCollection? itemProperties;
        if (list == null)
        {
            return new PropertyDescriptorCollection(null);
        }
        if (!(list is Type))
        {
            var obj = GetList(list);
            if (!(obj is ITypedList))
            {
                itemProperties = !(obj is IEnumerable) ? TypeDescriptor.GetProperties(obj) : GetListItemPropertiesByEnumerable(obj as IEnumerable);
            }
            else
            {
                itemProperties = (obj as ITypedList)?.GetItemProperties(null);
            }
        }
        else
        {
            itemProperties = GetListItemPropertiesByType(list as Type);
        }
        return itemProperties;
    }

    private static PropertyDescriptorCollection GetListItemPropertiesByEnumerable(IEnumerable? enumerable)
    {
        PropertyDescriptorCollection? properties = null;
        var type = enumerable?.GetType();
        if (!typeof(Array).IsAssignableFrom(type))
        {
            if (enumerable is not ITypedList typedList)
            {
                var typedIndexer = GetTypedIndexer(type);
                if (typedIndexer != null && !typeof(ICustomTypeDescriptor).IsAssignableFrom(typedIndexer.PropertyType))
                {
                    properties = TypeDescriptor.GetProperties(typedIndexer.PropertyType, BrowsableAttributeList);
                }
            }
            else
            {
                properties = typedList.GetItemProperties(null);
            }
        }
        else
        {
            properties = TypeDescriptor.GetProperties(type?.GetElementType(), BrowsableAttributeList);
        }
        if (properties == null)
        {
            var firstItemByEnumerable = GetFirstItemByEnumerable(enumerable);
            if (enumerable is string)
            {
                properties = TypeDescriptor.GetProperties(enumerable, BrowsableAttributeList);
            }
            else if (firstItemByEnumerable != null)
            {
                properties = TypeDescriptor.GetProperties(firstItemByEnumerable, BrowsableAttributeList);
                if (!(enumerable is IList) && (properties.Count == 0))
                {
                    properties = TypeDescriptor.GetProperties(enumerable, BrowsableAttributeList);
                }
            }
            else
            {
                properties = new PropertyDescriptorCollection(null);
            }
        }
        return properties;
    }

    private static bool IsListBasedType(Type? type)
    {
        if (typeof(IList).IsAssignableFrom(type) || typeof(ITypedList).IsAssignableFrom(type) || typeof(IListSource).IsAssignableFrom(type))
        {
            return true;
        }
        if (type is { IsGenericType: true, IsGenericTypeDefinition: false } && typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
        {
            return true;
        }
        var interfaces = type?.GetInterfaces()?? [];
        for (var i = 0; i < interfaces.Length; i++)
        {
            var type1 = interfaces[i];
            if (type1.IsGenericType && typeof(IList<>).IsAssignableFrom(type1.GetGenericTypeDefinition()))
            {
                return true;
            }
        }
        return false;
    }
    private static PropertyInfo? GetTypedIndexer(Type? type)
    {
        PropertyInfo? propertyInfo = null;
        if (!IsListBasedType(type))
        {
            return null;
        }
        var properties = type?.GetProperties(BindingFlags.Instance | BindingFlags.Public)??[];
        for (var i = 0; i < properties.Length; i++)
        {
            if (properties[i].GetIndexParameters().Length != 0 && properties[i].PropertyType != typeof(object))
            {
                propertyInfo = properties[i];
                if (propertyInfo.Name == "Item")
                {
                    break;
                }
            }
        }
        return propertyInfo;
    }
    private static object? GetFirstItemByEnumerable(IEnumerable? enumerable)
    {
        object? current = null;
        if (!(enumerable is IList))
        {
            try
            {
                var enumerator = enumerable?.GetEnumerator();
                using var enumerator1 = enumerator as IDisposable;
                if (enumerator != null)
                {
                    enumerator.Reset();
                    if (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                    }

                    enumerator.Reset();
                }
            }
            catch (NotSupportedException)
            {
                current = null;
            }
        }
        else
        {
            current = enumerable is IList { Count: > 0 } lists ? lists[0] : null;
        }
        return current;
    }

    private static PropertyDescriptorCollection? GetListItemPropertiesByEnumerable(IEnumerable? enumerable, PropertyDescriptor?[]? listAccessors)
    {
        PropertyDescriptorCollection? listItemPropertiesByEnumerable;
        if (listAccessors == null || listAccessors.Length == 0)
        {
            listItemPropertiesByEnumerable = GetListItemPropertiesByEnumerable(enumerable);
        }
        else
        {
            listItemPropertiesByEnumerable = enumerable is not ITypedList typedList ? GetListItemPropertiesByEnumerable(enumerable, listAccessors, 0) : typedList.GetItemProperties(listAccessors);
        }
        return listItemPropertiesByEnumerable;
    }

    private static PropertyDescriptorCollection? GetListItemPropertiesByEnumerable(IEnumerable? iEnumerable, PropertyDescriptor?[]? listAccessors, int startIndex)
    {
        PropertyDescriptorCollection? listItemPropertiesByInstance;
        object? list = null;
        var firstItemByEnumerable = GetFirstItemByEnumerable(iEnumerable);
        if (firstItemByEnumerable != null)
        {
            list = GetList(listAccessors?[startIndex]?.GetValue(firstItemByEnumerable));
        }
        if (list != null)
        {
            startIndex++;
            if (list is not IEnumerable enumerable)
            {
                listItemPropertiesByInstance = GetListItemPropertiesByInstance(list, listAccessors, startIndex);
            }
            else
            {
                listItemPropertiesByInstance = startIndex != listAccessors?.Length ? GetListItemPropertiesByEnumerable(enumerable, listAccessors, startIndex) : GetListItemPropertiesByEnumerable(enumerable);
            }
        }
        else
        {
            listItemPropertiesByInstance = GetListItemPropertiesByType(listAccessors?[startIndex]?.PropertyType, listAccessors, startIndex);
        }
        return listItemPropertiesByInstance;
    }

    /// <summary>Returns the <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that describes the properties of an item type contained in the specified data member of a data source. Uses the specified <see cref="T:System.ComponentModel.PropertyDescriptor" /> array to indicate which properties to examine.</summary>
    /// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> describing the properties of an item type contained in a collection property of the specified data source.</returns>
    /// <param name="dataSource">The data source to be examined for property information.</param>
    /// <param name="dataMember">The optional data member to be examined for property information. This can be null.</param>
    /// <param name="listAccessors">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> array describing which properties of the data member to examine. This can be null.</param>
    /// <exception cref="T:System.ArgumentException">The specified data member could not be found in the specified data source.</exception>
    public static PropertyDescriptorCollection? GetListItemProperties(object? dataSource, string? dataMember, PropertyDescriptor[]? listAccessors)
    {
        dataSource = GetList(dataSource);
        if (!string.IsNullOrEmpty(dataMember))
        {
            var propertyDescriptor = GetListItemProperties(dataSource)?.Find(dataMember, true);
            if (propertyDescriptor == null)
            {
                throw new ArgumentException(string.Format("DataSourceDataMemberPropNotFound {0}", dataMember));
            }
            var num = listAccessors == null ? 1 : listAccessors.Length + 1;
            var propertyDescriptorArray = new PropertyDescriptor[num];
            propertyDescriptorArray[0] = propertyDescriptor;
            for (var i = 1; i < num; i++)
            {
                if (listAccessors != null)
                {
                    propertyDescriptorArray[i] = listAccessors[i - 1];
                }
            }
            listAccessors = propertyDescriptorArray;
        }
        return GetListItemProperties(dataSource, listAccessors);
    }

    public static PropertyDescriptorCollection? GetListItemProperties(object? list, PropertyDescriptor?[]? listAccessors)
    {
        PropertyDescriptorCollection? listItemProperties;
        if (listAccessors == null || listAccessors.Length == 0)
        {
            listItemProperties = GetListItemProperties(list);
        }
        else if (!(list is Type))
        {
            var obj = GetList(list);
            if (!(obj is ITypedList))
            {
                listItemProperties = !(obj is IEnumerable) ? GetListItemPropertiesByInstance(obj, listAccessors, 0) : GetListItemPropertiesByEnumerable(obj as IEnumerable, listAccessors);
            }
            else
            {
                listItemProperties = (obj as ITypedList)?.GetItemProperties(listAccessors);
            }
        }
        else
        {
            listItemProperties = GetListItemPropertiesByType(list as Type, listAccessors);
        }
        return listItemProperties;
    }

    private static PropertyDescriptorCollection? GetListItemPropertiesByType(Type? type, PropertyDescriptor?[]? listAccessors)
    {
        var propertyDescriptorCollections = listAccessors == null || listAccessors.Length == 0 ? GetListItemPropertiesByType(type) : GetListItemPropertiesByType(type, listAccessors, 0);
        return propertyDescriptorCollections;
    }

    private static PropertyDescriptorCollection GetListItemPropertiesByType(Type? type)
    {
        return TypeDescriptor.GetProperties(GetListItemType(type), BrowsableAttributeList);
    }

    public static Type? GetListItemType(object? list)
    {
        if (list == null)
        {
            return null;
        }
        Type? propertyType;
        if (list is Type && typeof(IListSource).IsAssignableFrom(list as Type))
        {
            list = CreateInstanceOfType(list as Type);
        }
        list = GetList(list);
        var type = list is Type ? list as Type : list?.GetType();
        var obj = list is Type ? null : list;
        if (!typeof(Array).IsAssignableFrom(type))
        {
            var typedIndexer = GetTypedIndexer(type);
            if (typedIndexer == null)
            {
                propertyType = !(obj is IEnumerable) ? type : GetListItemTypeByEnumerable(obj as IEnumerable);
            }
            else
            {
                propertyType = typedIndexer.PropertyType;
            }
        }
        else
        {
            propertyType = type?.GetElementType();
        }
        return propertyType;
    }

    private static Type GetListItemTypeByEnumerable(IEnumerable? iEnumerable)
    {
        var firstItemByEnumerable = GetFirstItemByEnumerable(iEnumerable);
        if (firstItemByEnumerable == null)
        {
            return typeof(object);
        }
        return firstItemByEnumerable.GetType();
    }
    private static object? CreateInstanceOfType(Type? type)
    {
        object? obj = null;
        Exception? exception = null;
        try
        {
            obj = SecurityUtils.SecureCreateInstance(type);
        }
        catch (TargetInvocationException targetInvocationException)
        {
            exception = targetInvocationException;
        }
        catch (MethodAccessException methodAccessException)
        {
            exception = methodAccessException;
        }
        catch (MissingMethodException missingMethodException)
        {
            exception = missingMethodException;
        }
        if (exception != null)
        {
            throw new NotSupportedException("BindingSourceInstanceError", exception);
        }
        return obj;
    }
    private static Attribute[]? _browsableAttribute;

    private static Attribute[] BrowsableAttributeList
    {
        get
        {
            if (_browsableAttribute == null)
            {
                _browsableAttribute = [new BrowsableAttribute(true)];
            }
            return _browsableAttribute;
        }
    }
}