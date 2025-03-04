// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms;

/// <summary>
///  Provides a type converter to convert <see cref='Keys'/> objects to and from various
///  other representations.
/// </summary>
public class KeysConverter : TypeConverter, IComparer
{
    private IDictionary? keyNames;
    private List<string>? displayOrder;
    private StandardValuesCollection? values;

    private const Keys firstDigit = Keys.D0;
    private const Keys lastDigit = Keys.D9;
    private const Keys firstAscii = Keys.A;
    private const Keys lastAscii = Keys.Z;
    private const Keys firstNumpadDigit = Keys.NumPad0;
    private const Keys lastNumpadDigit = Keys.NumPad9;

    private void AddKey(string key, Keys value)
    {
        if (keyNames != null)
        {
            keyNames[key] = value;
        }

        displayOrder?.Add(key);
    }

    private void Initialize()
    {
        keyNames = new Hashtable(34);
        displayOrder = new List<string>(34);

        //AddKey(SR.toStringEnter, Keys.Return);
        //AddKey("F12", Keys.F12);
        //AddKey("F11", Keys.F11);
        //AddKey("F10", Keys.F10);
        //AddKey(SR.toStringEnd, Keys.End);
        //AddKey(SR.toStringControl, Keys.Control);
        //AddKey("F8", Keys.F8);
        //AddKey("F9", Keys.F9);
        //AddKey(SR.toStringAlt, Keys.Alt);
        //AddKey("F4", Keys.F4);
        //AddKey("F5", Keys.F5);
        //AddKey("F6", Keys.F6);
        //AddKey("F7", Keys.F7);
        //AddKey("F1", Keys.F1);
        //AddKey("F2", Keys.F2);
        //AddKey("F3", Keys.F3);
        //AddKey(SR.toStringPageDown, Keys.Next);
        //AddKey(SR.toStringInsert, Keys.Insert);
        //AddKey(SR.toStringHome, Keys.Home);
        //AddKey(SR.toStringDelete, Keys.Delete);
        //AddKey(SR.toStringShift, Keys.Shift);
        //AddKey(SR.toStringPageUp, Keys.Prior);
        //AddKey(SR.toStringBack, Keys.Back);

        //new whidbey keys follow here...
        // Add string mappings for these values (numbers 0-9) so that the keyboard shortcuts
        // will be displayed properly in menus.
        AddKey("0", Keys.D0);
        AddKey("1", Keys.D1);
        AddKey("2", Keys.D2);
        AddKey("3", Keys.D3);
        AddKey("4", Keys.D4);
        AddKey("5", Keys.D5);
        AddKey("6", Keys.D6);
        AddKey("7", Keys.D7);
        AddKey("8", Keys.D8);
        AddKey("9", Keys.D9);
    }

    /// <summary>
    ///  Access to a lookup table of name/value pairs for keys.  These are localized
    ///  names.
    /// </summary>
    private IDictionary KeyNames
    {
        get
        {
            if (keyNames == null)
            {
                Debug.Assert(displayOrder == null);
                Initialize();
            }
            return keyNames!;
        }
    }

    private List<string> DisplayOrder
    {
        get
        {
            if (displayOrder == null)
            {
                Debug.Assert(keyNames == null);
                Initialize();
            }
            return displayOrder!;
        }
    }

    /// <summary>
    ///  Determines if this converter can convert an object in the given source
    ///  type to the native type of the converter.
    /// </summary>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        if (sourceType == typeof(string) || sourceType == typeof(Enum[]))
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
        if (destinationType == typeof(Enum[]))
        {
            return true;
        }
        return base.CanConvertTo(context, destinationType);
    }

    /// <summary>
    ///  Compares two key values for equivalence.
    /// </summary>
    public int Compare(object? a, object? b)
    {
        return string.Compare(ConvertToString(a ?? string.Empty), ConvertToString(b ?? string.Empty), false, CultureInfo.InvariantCulture);
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
                return null;
            }

            // Parse an array of key tokens.
            //
            var tokens = text.Split('+');
            for (var i = 0; i < tokens.Length; i++)
            {
                tokens[i] = tokens[i].Trim();
            }

            // Now lookup each key token in our key hashtable.
            //
            Keys key = 0;
            var foundKeyCode = false;

            for (var i = 0; i < tokens.Length; i++)
            {
                var obj = KeyNames[tokens[i]];

                if (obj == null)
                {

                    // Key was not found in our table.  See if it is a valid value in
                    // the Keys enum.
                    //
                    obj = Enum.Parse(typeof(Keys), tokens[i]);
                }

                if (obj != null)
                {
                    var currentKey = (Keys)obj;

                    if ((currentKey & Keys.KeyCode) != 0)
                    {

                        // We found a match.  If we have previously found a
                        // key code, then check to see that this guy
                        // isn't a key code (it is illegal to have, say,
                        // "A + B"
                        //
                        if (foundKeyCode)
                        {
                            throw new FormatException("SR.KeysConverterInvalidKeyCombination");
                        }
                        foundKeyCode = true;
                    }

                    // Now OR the key into our current key
                    //
                    key |= currentKey;
                }
                else
                {

                    // We did not match this key.  Report this as an error too.
                    //
                    throw new FormatException(string.Format("SR.KeysConverterInvalidKeyName: {0}", tokens[i]));

                }
            }

            return key;
        }

        if (value is Enum[])
        {
            long finalValue = 0;
            foreach (var e in (Enum[])value)
            {
                finalValue |= Convert.ToInt64(e, CultureInfo.InvariantCulture);
            }
            return Enum.ToObject(typeof(Keys), finalValue);
        }

        if (context != null && culture != null)
        {
            return base.ConvertFrom(context, culture, value);
        }

        return null;
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

        if (value is Keys || value is int)
        {
            var asString = destinationType == typeof(string);
            var asEnum = false;
            if (!asString)
            {
                asEnum = destinationType == typeof(Enum[]);
            }

            if (asString || asEnum)
            {
                var key = (Keys)value;
                var added = false;
                var terms = new ArrayList();
                var modifiers = key & Keys.Modifiers;

                // First, iterate through and do the modifiers. These are
                // additive, so we support things like Ctrl + Alt
                //
                for (var i = 0; i < DisplayOrder.Count; i++)
                {
                    var keyString = DisplayOrder[i];
                    var keyValue = (Keys)keyNames![keyString];
                    if (((int)keyValue & (int)modifiers) != 0)
                    {

                        if (asString)
                        {
                            if (added)
                            {
                                terms.Add("+");
                            }

                            terms.Add(keyString);
                        }
                        else
                        {
                            terms.Add(keyValue);
                        }

                        added = true;
                    }
                }

                // Now reset and do the key values.  Here, we quit if
                // we find a match.
                //
                var keyOnly = key & Keys.KeyCode;
                var foundKey = false;

                if (added && asString)
                {
                    terms.Add("+");
                }

                for (var i = 0; i < DisplayOrder.Count; i++)
                {
                    var keyString = DisplayOrder[i];
                    var keyValue = (Keys)keyNames![keyString];
                    if (keyValue.Equals(keyOnly))
                    {

                        if (asString)
                        {
                            terms.Add(keyString);
                        }
                        else
                        {
                            terms.Add(keyValue);
                        }

                        foundKey = true;
                        break;
                    }
                }

                // Finally, if the key wasn't in our list, add it to
                // the end anyway.  Here we just pull the key value out
                // of the enum.
                //
                if (!foundKey && Enum.IsDefined(typeof(Keys), (int)keyOnly))
                {
                    if (asString)
                    {
                        terms.Add(keyOnly.ToString());
                    }
                    else
                    {
                        terms.Add(keyOnly);
                    }
                }

                if (asString)
                {
                    var b = new StringBuilder(32);
                    foreach (string t in terms)
                    {
                        b.Append(t);
                    }
                    return b.ToString();
                }

                return (Enum[])terms.ToArray(typeof(Enum));
            }
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }

    /// <summary>
    ///  Retrieves a collection containing a set of standard values
    ///  for the data type this validator is designed for.  This
    ///  will return null if the data type does not support a
    ///  standard set of values.
    /// </summary>
    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        if (values == null)
        {
            var list = new ArrayList();

            var keys = KeyNames.Values;

            foreach (var o in keys)
            {
                list.Add(o);
            }

            list.Sort(this);

            values = new StandardValuesCollection(list.ToArray());
        }
        return values;
    }

    /// <summary>
    ///  Determines if the list of standard values returned from
    ///  GetStandardValues is an exclusive list.  If the list
    ///  is exclusive, then no other values are valid, such as
    ///  in an enum data type.  If the list is not exclusive,
    ///  then there are other valid values besides the list of
    ///  standard values GetStandardValues provides.
    /// </summary>
    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
        return false;
    }

    /// <summary>
    ///  Determines if this object supports a standard set of values
    ///  that can be picked from a list.
    /// </summary>
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
        return true;
    }
}