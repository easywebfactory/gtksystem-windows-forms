// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Drawing.Printing;

/// <summary>
///  Provides a type converter to convert <see cref='Margins'/> to and from various other representations, such as a string.
/// </summary>
public class MarginsConverter : ExpandableObjectConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type destinationType)
        => destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (value is string strValue)
        {
        }
        else
        {
            return base.ConvertFrom(context, culture, value);
        }

        var text = strValue.Trim();

        if (text.Length == 0)
        {
            return null;
        }

        // Parse 4 integer values.
        culture ??= CultureInfo.CurrentCulture;
        var sep = culture.TextInfo.ListSeparator[0];
        var tokens = text.Split(sep);
        var values = new int[tokens.Length];
        var intConverter = GetIntConverter();

        for (var i = 0; i < values.Length; i++)
        {
            // Note: ConvertFromString will raise exception if value cannot be converted.
            values[i] = (int)intConverter.ConvertFromString(context!, culture, tokens[i])!;
        }

        if (values.Length != 4)
        {
            throw new ArgumentException("TextParseFailedFormat");
        }

        return new Margins(values[0], values[1], values[2], values[3]);
    }

    private static TypeConverter GetIntConverter() => TypeDescriptor.GetConverter(typeof(int));

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is Margins margins)
        {
            if (destinationType == typeof(string))
            {
                culture ??= CultureInfo.CurrentCulture;
                var sep = culture.TextInfo.ListSeparator + " ";
                var intConverter = GetIntConverter();
                var args = new string[4];
                var nArg = 0;

                // Note: ConvertToString will raise exception if value cannot be converted.
                args[nArg++] = intConverter.ConvertToString(context, culture, margins.Left);
                args[nArg++] = intConverter.ConvertToString(context, culture, margins.Right);
                args[nArg++] = intConverter.ConvertToString(context, culture, margins.Top);
                args[nArg] = intConverter.ConvertToString(context, culture, margins.Bottom);

                return string.Join(sep, args);
            }

            if (destinationType == typeof(InstanceDescriptor))
            {
                if (typeof(Margins).GetConstructor([typeof(int), typeof(int), typeof(int), typeof(int)]) is { } constructor)
                {
                    return new InstanceDescriptor(
                        constructor,
                        new object[] { margins.Left, margins.Right, margins.Top, margins.Bottom });
                }
            }
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }

    public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) => true;

    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    {
        var left = propertyValues["Left"];
        var right = propertyValues["Right"];
        var top = propertyValues["Top"];
        var bottom = propertyValues["Bottom"];

        return left is int && right is int && bottom is int && top is int ? (object)new Margins((int)left, (int)right, (int)top, (int)bottom) : throw new ArgumentException("PropertyValueInvalidEntry");
    }
}