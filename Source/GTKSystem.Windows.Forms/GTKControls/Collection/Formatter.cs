using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms;

internal class Formatter
{
    private static readonly Type? stringType;

    private static readonly Type booleanType;

    private static readonly Type checkStateType;

    private static readonly object? parseMethodNotFound;

    private static readonly object? defaultDataSourceNullValue;

    static Formatter()
    {
        stringType = typeof(string);
        booleanType = typeof(bool);
        checkStateType = typeof(CheckState);
        parseMethodNotFound = new object();
        defaultDataSourceNullValue = DBNull.Value;
    }

    private static object? ChangeType(object? value, Type? type, IFormatProvider? formatInfo)
    {
        object? obj;
        try
        {
            if (formatInfo == null)
            {
                formatInfo = CultureInfo.CurrentCulture;
            }
            obj = Convert.ChangeType(value, type??typeof(object), formatInfo);
        }
        catch (InvalidCastException invalidCastException1)
        {
            var invalidCastException = invalidCastException1;
            throw new FormatException(invalidCastException.Message, invalidCastException);
        }
        return obj;
    }

    private static bool EqualsFormattedNullValue(object? value, object? formattedNullValue, IFormatProvider? formatInfo)
    {
        var str = formattedNullValue as string;
        var str1 = value as string;
        if (str == null || str1 == null)
        {
            return Equals(value, formattedNullValue);
        }
        if (str.Length != str1.Length)
        {
            return false;
        }
        return string.Compare(str1, str, true, GetFormatterCulture(formatInfo)) == 0;
    }

    public static object? FormatObject(object? value, Type? targetType, TypeConverter? sourceConverter, TypeConverter? targetConverter, string formatString, IFormatProvider? formatInfo, object? formattedNullValue, object? dataSourceNullValue)
    {
        if (IsNullData(value, dataSourceNullValue))
        {
            value = DBNull.Value;
        }
        var type = targetType;
        targetType = NullableUnwrap(targetType);
        sourceConverter = NullableUnwrap(sourceConverter);
        targetConverter = NullableUnwrap(targetConverter);
        var flag = targetType != type;
        var obj = FormatObjectInternal(value, targetType, sourceConverter, targetConverter, formatString, formatInfo, formattedNullValue);
        if ((type?.IsValueType??false) && obj == null && !flag)
        {
            throw new FormatException(GetCantConvertMessage(value, targetType));
        }
        return obj;
    }

    private static object? FormatObjectInternal(object? value, Type? targetType, TypeConverter? sourceConverter, TypeConverter? targetConverter, string formatString, IFormatProvider? formatInfo, object? formattedNullValue)
    {
        if (value == DBNull.Value || value == null)
        {
            if (formattedNullValue != null)
            {
                return formattedNullValue;
            }
            if (targetType == stringType)
            {
                return string.Empty;
            }
            if (targetType != checkStateType)
            {
                return null;
            }
            return CheckState.Indeterminate;
        }
        if (targetType == stringType && value is IFormattable && !string.IsNullOrEmpty(formatString))
        {
            return (value as IFormattable)?.ToString(formatString, formatInfo);
        }
        var type = value.GetType();
        var converter = TypeDescriptor.GetConverter(type);
        targetType ??= typeof(object);
        if (sourceConverter != null && sourceConverter != converter && sourceConverter.CanConvertTo(targetType))
        {
            return sourceConverter.ConvertTo(null, GetFormatterCulture(formatInfo), value, targetType);
        }
        var typeConverter = TypeDescriptor.GetConverter(targetType);
        if (targetConverter != null && targetConverter != typeConverter && targetConverter.CanConvertFrom(type))
        {
            return targetConverter.ConvertFrom(null!, GetFormatterCulture(formatInfo), value);
        }
        if (targetType == checkStateType)
        {
            if (type == booleanType)
            {
                return (bool)value ? CheckState.Checked : CheckState.Unchecked;
            }
            if (sourceConverter == null)
            {
                sourceConverter = converter;
            }
            if (sourceConverter != null && sourceConverter.CanConvertTo(booleanType))
            {
                return (bool)sourceConverter.ConvertTo(null, GetFormatterCulture(formatInfo), value, booleanType) ? CheckState.Checked : CheckState.Unchecked;
            }
        }
        if (targetType.IsAssignableFrom(type))
        {
            return value;
        }
        if (sourceConverter == null)
        {
            sourceConverter = converter;
        }
        if (targetConverter == null)
        {
            targetConverter = typeConverter;
        }
        if (sourceConverter != null && sourceConverter.CanConvertTo(targetType))
        {
            return sourceConverter.ConvertTo(null, GetFormatterCulture(formatInfo), value, targetType);
        }
        if (targetConverter != null && targetConverter.CanConvertFrom(type))
        {
            return targetConverter.ConvertFrom(null!, GetFormatterCulture(formatInfo), value);
        }
        if (!(value is IConvertible))
        {
            throw new FormatException(GetCantConvertMessage(value, targetType));
        }
        return ChangeType(value, targetType, formatInfo);
    }

    private static string GetCantConvertMessage(object? value, Type? targetType)
    {
        var str = value == null ? "Formatter_CantConvertNull" : "Formatter_CantConvert";
        return string.Format(CultureInfo.CurrentCulture, str, [value, targetType?.Name]);
    }

    public static object? GetDefaultDataSourceNullValue(Type? type)
    {
        if (type is { IsValueType: false })
        {
            return null;
        }
        return defaultDataSourceNullValue;
    }

    private static CultureInfo GetFormatterCulture(IFormatProvider? formatInfo)
    {
        if (!(formatInfo is CultureInfo))
        {
            return CultureInfo.CurrentCulture;
        }
        return formatInfo as CultureInfo??CultureInfo.InvariantCulture;
    }

    public static object? InvokeStringParseMethod(object? value, Type? targetType, IFormatProvider? formatInfo)
    {
        object? obj;
        try
        {
            var method = targetType?.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, [stringType, typeof(NumberStyles), typeof(IFormatProvider)
            ], null);
            if (method == null)
            {
                method = targetType?.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, [stringType, typeof(IFormatProvider)
                ], null);
                if (method == null)
                {
                    method = targetType?.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, [stringType
                    ], null);
                    obj = method == null ? parseMethodNotFound : method.Invoke(null, [(string?)value]);
                }
                else
                {
                    obj = method.Invoke(null, [(string?)value, formatInfo]);
                }
            }
            else
            {
                obj = method.Invoke(null, [(string?)value, NumberStyles.Any, formatInfo]);
            }
        }
        catch (TargetInvocationException targetInvocationException1)
        {
            var targetInvocationException = targetInvocationException1;
            throw new FormatException(targetInvocationException.InnerException?.Message, targetInvocationException.InnerException);
        }
        return obj;
    }

    public static bool IsNullData(object? value, object? dataSourceNullValue)
    {
        if (value == null || value == DBNull.Value)
        {
            return true;
        }
        return Equals(value, NullData(value.GetType(), dataSourceNullValue));
    }

    private static Type? NullableUnwrap(Type? type)
    {
        if (type == stringType)
        {
            return stringType;
        }
        return Nullable.GetUnderlyingType(type??typeof(object)) ?? type;
    }

    private static TypeConverter? NullableUnwrap(TypeConverter? typeConverter)
    {
        var nullableConverter = typeConverter as NullableConverter;
        if (nullableConverter == null)
        {
            return typeConverter;
        }
        return nullableConverter.UnderlyingTypeConverter;
    }

    public static object? NullData(Type? type, object? dataSourceNullValue)
    {
        if (!(type?.IsGenericType??false) || !(type.GetGenericTypeDefinition() == typeof(Nullable<>)))
        {
            return dataSourceNullValue;
        }
        if (dataSourceNullValue != null && dataSourceNullValue != DBNull.Value)
        {
            return dataSourceNullValue;
        }
        return null;
    }

    public static object? ParseObject(object? value, Type? targetType, Type? sourceType, TypeConverter? targetConverter, TypeConverter? sourceConverter, IFormatProvider? formatInfo, object? formattedNullValue, object? dataSourceNullValue)
    {
        var type = targetType;
        sourceType = NullableUnwrap(sourceType);
        targetType = NullableUnwrap(targetType);
        sourceConverter = NullableUnwrap(sourceConverter);
        targetConverter = NullableUnwrap(targetConverter);
        var obj = ParseObjectInternal(value, targetType, sourceType, targetConverter, sourceConverter, formatInfo, formattedNullValue);
        if (obj != DBNull.Value)
        {
            return obj;
        }
        return NullData(type, dataSourceNullValue);
    }

    private static object? ParseObjectInternal(object? value, Type? targetType, Type? sourceType, TypeConverter? targetConverter, TypeConverter? sourceConverter, IFormatProvider? formatInfo, object? formattedNullValue)
    {
        if (EqualsFormattedNullValue(value, formattedNullValue, formatInfo) || value == DBNull.Value)
        {
            return DBNull.Value;
        }

        targetType ??= typeof(object);
        sourceType ??= typeof(object);
        var converter = TypeDescriptor.GetConverter(targetType);
        if (targetConverter != null && converter != targetConverter && targetConverter.CanConvertFrom(sourceType))
        {
            return targetConverter.ConvertFrom(null!, GetFormatterCulture(formatInfo), value);
        }

        var typeConverter = TypeDescriptor.GetConverter(sourceType);
        if (sourceConverter != null && typeConverter != sourceConverter && sourceConverter.CanConvertTo(targetType))
        {
            return sourceConverter.ConvertTo(null, GetFormatterCulture(formatInfo), value, targetType);
        }
        if (value is string)
        {
            var obj = InvokeStringParseMethod(value, targetType, formatInfo);
            if (obj != parseMethodNotFound)
            {
                return obj;
            }
        }
        else if (value is CheckState state && state != CheckState.Unchecked)
        {
            var checkState = (CheckState)value;
            if (checkState == CheckState.Indeterminate)
            {
                return DBNull.Value;
            }
            if (targetType == booleanType)
            {
                return checkState == CheckState.Checked;
            }
            if (targetConverter == null)
            {
                targetConverter = converter;
            }
            if (targetConverter != null && targetConverter.CanConvertFrom(booleanType))
            {
                return targetConverter.ConvertFrom(null!, GetFormatterCulture(formatInfo), checkState == CheckState.Checked);
            }
        }
        else if (value != null && (targetType.IsInstanceOfType(value)))
        {
            return value;
        }
        if (targetConverter == null)
        {
            targetConverter = converter;
        }
        if (sourceConverter == null)
        {
            sourceConverter = typeConverter;
        }
        if (targetConverter != null && targetConverter.CanConvertFrom(sourceType))
        {
            return targetConverter.ConvertFrom(null!, GetFormatterCulture(formatInfo), value);
        }
        if (sourceConverter != null && sourceConverter.CanConvertTo(targetType))
        {
            return sourceConverter.ConvertTo(null, GetFormatterCulture(formatInfo), value, targetType);
        }
        if (!(value is IConvertible))
        {
            throw new FormatException(GetCantConvertMessage(value, targetType));
        }
        return ChangeType(value, targetType, formatInfo);
    }
}