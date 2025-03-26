// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows.Forms;

/// <summary>
///  SelectionRangeConverter is a class that can be used to convert
///  SelectionRange objects from one data type to another.  Access this
///  class through the TypeDescriptor.
/// </summary>
public class SelectionRangeConverter : TypeConverter
{
    /// <summary>
    ///  Determines if this converter can convert an object in the given source
    ///  type to the native type of the converter.
    /// </summary>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        if (sourceType == typeof(string) || sourceType == typeof(DateTime))
        {
            return true;
        }
        return base.CanConvertFrom(context, sourceType);
    }

    /// <summary>
    ///  Gets a value indicating whether this converter can
    ///  convert an object to the given destination type using the context.
    /// </summary>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type destinationType)
    {
        if (destinationType == typeof(InstanceDescriptor) || destinationType == typeof(DateTime))
        {
            return true;
        }
        return base.CanConvertTo(context, destinationType);
    }

    /// <summary>
    ///  Converts the given object to the converter's native type.
    /// </summary>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (value is string)
        {
            var text = ((string)value).Trim();
            if (text.Length == 0)
            {
                return new SelectionRange(DateTime.Now.Date, DateTime.Now.Date);
            }

            // Separate the string into the two dates, and parse each one
            //
            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }
            var separator = culture.TextInfo.ListSeparator[0];
            var tokens = text.Split(separator);

            if (tokens.Length == 2)
            {
                var dateTimeConverter = TypeDescriptor.GetConverter(typeof(DateTime));
                var start = (DateTime)dateTimeConverter.ConvertFromString(context!, culture, tokens[0]);
                var end = (DateTime)dateTimeConverter.ConvertFromString(context!, culture, tokens[1]);
                return new SelectionRange(start, end);
            }

            throw new ArgumentException(string.Format("Text Parse Failed Format:{0},{1}",
                text,
                "Start" + separator + " End"));
        }

        if (value is DateTime dt)
        {
            return new SelectionRange(dt, dt);
        }

        return base.ConvertFrom(context!, culture!, value);
    }

    /// <summary>
    ///  Converts the given object to another type.  The most common types to convert
    ///  are to and from a string object.  The default implementation will make a call
    ///  to ToString on the object if the object is valid and if the destination
    ///  type is string.  If this cannot convert to the desitnation type, this will
    ///  throw a NotSupportedException.
    /// </summary>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == null)
        {
            throw new ArgumentNullException(nameof(destinationType));
        }

        if (value is SelectionRange range)
        {
            if (destinationType == typeof(string))
            {
                if (culture == null)
                {
                    culture = CultureInfo.CurrentCulture;
                }
                var sep = culture.TextInfo.ListSeparator + " ";
                var props = GetProperties(value);
                var args = new string[props?.Count??0];

                for (var i = 0; i < (props?.Count??0); i++)
                {
                    var propValue = props?[i].GetValue(value);
                    if (propValue != null)
                    {
                        args[i] = TypeDescriptor.GetConverter(propValue).ConvertToString(context!, culture, propValue)??string.Empty;
                    }
                }

                return string.Join(sep, args);
            }
            if (destinationType == typeof(DateTime))
            {
                return range.Start;
            }
            if (destinationType == typeof(InstanceDescriptor))
            {
                var ctor = typeof(SelectionRange).GetConstructor([
                    typeof(DateTime), typeof(DateTime)
                ]);
                if (ctor != null)
                {
                    return new InstanceDescriptor(ctor, new object[] { range.Start, range.End });
                }
            }
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }

    /// <summary>
    ///  Creates an instance of this type given a set of property values
    ///  for the object.  This is useful for objects that are immutable, but still
    ///  want to provide changable properties.
    /// </summary>
    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    {
        try
        {
            return new SelectionRange((DateTime)propertyValues["Start"],
                (DateTime)propertyValues["End"]);
        }
        catch (InvalidCastException invalidCast)
        {
            throw new ArgumentException("Property Value Invalid Entry", invalidCast);
        }
        catch (NullReferenceException nullRef)
        {
            throw new ArgumentException("Property Value Invalid Entry", nullRef);
        }
    }

    /// <summary>
    ///  Determines if changing a value on this object should require a call to
    ///  CreateInstance to create a new value.
    /// </summary>
    public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
    {
        return true;
    }

    /// <summary>
    ///  Retrieves the set of properties for this type.  By default, a type has
    ///  does not return any properties.  An easy implementation of this method
    ///  can just call TypeDescriptor.GetProperties for the correct data type.
    /// </summary>
    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
    {
        var props = TypeDescriptor.GetProperties(typeof(SelectionRange), attributes);
        return props.Sort(["Start", "End"]);
    }

    /// <summary>
    ///  Determines if this object supports properties.  By default, this
    ///  is false.
    /// </summary>
    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    {
        return true;
    }
}