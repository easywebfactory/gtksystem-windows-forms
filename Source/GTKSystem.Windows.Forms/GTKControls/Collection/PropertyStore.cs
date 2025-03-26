﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Forms;

/// <summary>
///  This is a small class that can efficiently store property values.
///  It tries to optimize for size first, "get" access second, and
///  "set" access third.
/// </summary>
internal partial class PropertyStore
{
    private static int _currentKey;

    private IntegerEntry[]? _intEntries;
    private ObjectEntry[]? _objEntries;

    /// <summary>
    ///  Retrieves an integer value from our property list.
    ///  This will set value to zero and return false if the
    ///  list does not contain the given key.
    /// </summary>
    public bool ContainsInteger(int key)
    {
        GetInteger(key, out var found);
        return found;
    }

    /// <summary>
    ///  Retrieves an integer value from our property list.
    ///  This will set value to zero and return false if the
    ///  list does not contain the given key.
    /// </summary>
    public bool ContainsObject(int key)
    {
        GetObject(key, out var found);
        return found;
    }

    /// <summary>
    ///  Creates a new key for this property store. This is NOT
    ///  guarded by any thread safety so if you are calling it on
    ///  multiple threads you should guard. For our purposes,
    ///  we're fine because this is designed to be called in a class
    ///  initializer, and we never have the same class hierarchy
    ///  initializing on multiple threads at once.
    /// </summary>
    public static int CreateKey() => _currentKey++;

    public Color GetColor(int key) => GetColor(key, out _);

    /// <summary>
    ///  A wrapper around GetObject designed to reduce the boxing hit
    /// </summary>
    public Color GetColor(int key, out bool found)
    {
        var storedObject = GetObject(key, out found);
        if (found)
        {
            if (storedObject is ColorWrapper wrapper)
            {
                return wrapper.Color;
            }

            Debug.Assert(storedObject is null,
                $"Have non-null object that isn't a color wrapper stored in a color entry!{Environment.NewLine}Did someone SetObject instead of SetColor?");
        }

        found = false;
        return Color.Empty;
    }

    /// <summary>
    ///  A wrapper around GetObject designed to reduce the boxing hit.
    /// </summary>
    public Padding GetPadding(int key, out bool found)
    {
        var storedObject = GetObject(key, out found);
        if (found)
        {
            if (storedObject is PaddingWrapper wrapper)
            {
                return wrapper.Padding;
            }

            Debug.Assert(storedObject is null,
                $"Have non-null object that isn't a padding wrapper stored in a padding entry!{Environment.NewLine}Did someone SetObject instead of SetPadding?");
        }

        found = false;
        return Padding.Empty;
    }

    /// <summary>
    ///  A wrapper around GetObject designed to reduce the boxing hit.
    /// </summary>
    public Size GetSize(int key, out bool found)
    {
        var storedObject = GetObject(key, out found);
        if (found)
        {
            if (storedObject is SizeWrapper wrapper)
            {
                return wrapper.Size;
            }

            Debug.Assert(storedObject is null,
                $"Have non-null object that isn't a padding wrapper stored in a padding entry!{Environment.NewLine}Did someone SetObject instead of SetPadding?");
        }

        found = false;
        return Size.Empty;
    }

    /// <summary>
    ///  A wrapper around GetObject designed to reduce the boxing hit.
    /// </summary>
    public Rectangle GetRectangle(int key, out bool found)
    {
        var storedObject = GetObject(key, out found);
        if (found)
        {
            if (storedObject is RectangleWrapper wrapper)
            {
                return wrapper.Rectangle;
            }

            Debug.Assert(storedObject is null,
                $"Have non-null object that isn't a Rectangle wrapper stored in a Rectangle entry!{Environment.NewLine}Did someone SetObject instead of SetRectangle?");
        }

        found = false;
        return Rectangle.Empty;
    }

    /// <summary>
    ///  Retrieves an integer value from our property list.
    ///  This will set value to zero and return false if the
    ///  list does not contain the given key.
    /// </summary>
    public int GetInteger(int key) => GetInteger(key, out _);

    /// <summary>
    ///  Retrieves an integer value from our property list.
    ///  This will set value to zero and return false if the
    ///  list does not contain the given key.
    /// </summary>
    public int GetInteger(int key, out bool found)
    {
        var keyIndex = SplitKey(key, out var element);
        if (!LocateIntegerEntry(keyIndex, out var index))
        {
            found = false;
            return default;
        }

        // We have found the relevant entry. See if
        // the bitmask indicates the value is used.
        if (((1 << element) & _intEntries![index].Mask) == 0)
        {
            found = false;
            return default;
        }

        found = true;
        switch (element)
        {
            case 0:
                return _intEntries[index].Value1;
            case 1:
                return _intEntries[index].Value2;
            case 2:
                return _intEntries[index].Value3;
            case 3:
                return _intEntries[index].Value4;
            default:
                Debug.Fail("Invalid element obtained from LocateIntegerEntry");
                return default;
        }
    }

    /// <summary>
    ///  Retrieves an object value from our property list.
    ///  This will set value to null and return false if the
    ///  list does not contain the given key.
    /// </summary>
    public object? GetObject(int key) => GetObject(key, out _);

    /// <summary>
    ///  Retrieves an object value from our property list.
    ///  This will set value to null and return false if the
    ///  list does not contain the given key.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve.</typeparam>
    /// <param name="key">The key corresponding to the object in the property list.</param>
    /// <param name="value">Output parameter where the object will be set if found.
    ///  Will be set to null if the key is not present.</param>
    /// <remarks><para>If a null value is set for a given key
    ///  it will return true and a null value.</para></remarks>
    /// <returns>True if an object (including null) is found for the given key; otherwise, false.</returns>
    public bool TryGetObject<T>(int key, out T? value)
    {
        var entry = GetObject(key, out var found);
        Debug.Assert(!found || entry is null || entry is T, $"Entry is not of type {typeof(T)}, but of type {entry?.GetType()}");
        if (typeof(T).IsValueType || typeof(T).IsEnum || typeof(T).IsPrimitive)
        {
            value = found && entry is not null ? (T?)entry : default;
            return found;
        }

        value = found ? (T?)entry : default;
        return found;
    }

    public bool ContainsObjectThatIsNotNull(int key)
    {
        var entry = GetObject(key, out var found);
        return found && entry is not null;
    }

    /// <summary>
    ///  Retrieves an object value from our property list.
    ///  This will set value to null and return false if the
    ///  list does not contain the given key.
    /// </summary>
    public object? GetObject(int key, out bool found)
    {
        var keyIndex = SplitKey(key, out var element);
        if (!LocateObjectEntry(keyIndex, out var index))
        {
            found = false;
            return null;
        }

        // We have found the relevant entry. See if
        // the bitmask indicates the value is used.
        if (((1 << element) & _objEntries![index].Mask) == 0)
        {
            found = false;
            return null;
        }

        found = true;
        switch (element)
        {
            case 0:
                return _objEntries[index].Value1;
            case 1:
                return _objEntries[index].Value2;
            case 2:
                return _objEntries[index].Value3;
            case 3:
                return _objEntries[index].Value4;
            default:
                Debug.Fail("Invalid element obtained from LocateObjectEntry");
                return null;
        }
    }

    /// <summary>
    ///  Locates the requested entry in our array if entries. This does
    ///  not do the mask check to see if the entry is currently being used,
    ///  but it does locate the entry. If the entry is found, this returns
    ///  true and fills in index and element. If the entry is not found,
    ///  this returns false. If the entry is not found, index will contain
    ///  the insert point at which one would add a new element.
    /// </summary>
    private bool LocateIntegerEntry(short _, out int index)
    {
        index = 0;
        return false;
    }

    /// <summary>
    ///  Locates the requested entry in our array if entries. This does
    ///  not do the mask check to see if the entry is currently being used,
    ///  but it does locate the entry. If the entry is found, this returns
    ///  true and fills in index and element. If the entry is not found,
    ///  this returns false. If the entry is not found, index will contain
    ///  the insert point at which one would add a new element.
    /// </summary>
    private bool LocateObjectEntry(short _, out int index)
    {
        index = 0;
        return false;
    }

    /// <summary>
    ///  Removes the given key from the array
    /// </summary>
    public void RemoveInteger(int key)
    {
    }

    /// <summary>
    ///  Removes the given key from the array
    /// </summary>
    public void RemoveObject(int key)
    {
        var entryKey = SplitKey(key, out var element);
        if (!LocateObjectEntry(entryKey, out var index))
        {
            return;
        }

        if (((1 << element) & _objEntries![index].Mask) == 0)
        {
            // This element is not being used - return right away
            return;
        }

        // Declare that the element is no longer used
        _objEntries[index].Mask &= (short)~(short)(1 << element);

        if (_objEntries[index].Mask == 0)
        {
            // This object entry is no longer in use - let's remove it all together
            // not great for perf but very simple and we don't expect to remove much
            if (_objEntries.Length == 1)
            {
                // Instead of allocating an array of length 0, we simply reset the array to null.
                _objEntries = null;
            }
            else
            {
                var newEntries = new ObjectEntry[_objEntries.Length - 1];
                if (index > 0)
                {
                    Array.Copy(_objEntries, 0, newEntries, 0, index);
                }

                if (index < newEntries.Length)
                {
                    Debug.Assert(_objEntries.Length - index - 1 > 0);
                    Array.Copy(_objEntries, index + 1, newEntries, index, _objEntries.Length - index - 1);
                }

                _objEntries = newEntries;
            }
        }
        else
        {
            // This object entry is still in use - let's just clean up the deleted element
            switch (element)
            {
                case 0:
                    _objEntries[index].Value1 = null;
                    break;

                case 1:
                    _objEntries[index].Value2 = null;
                    break;

                case 2:
                    _objEntries[index].Value3 = null;
                    break;

                case 3:
                    _objEntries[index].Value4 = null;
                    break;

                default:
                    Debug.Fail("Invalid element obtained from LocateObjectEntry");
                    break;
            }
        }
    }

    public void SetColor(int key, Color value)
    {
        var storedObject = GetObject(key, out var found);
        if (!found)
        {
            SetObject(key, new ColorWrapper(value));
        }
        else
        {
            if (storedObject is ColorWrapper wrapper)
            {
                // re-using the wrapper reduces the boxing hit.
                wrapper.Color = value;
            }
            else
            {
                Debug.Assert(storedObject is null,
                    "object should either be null or ColorWrapper"); // could someone have SetObject to this key behind our backs?
                SetObject(key, new ColorWrapper(value));
            }
        }
    }

    public void SetPadding(int key, Padding value)
    {
        var storedObject = GetObject(key, out var found);
        if (!found)
        {
            SetObject(key, new PaddingWrapper(value));
        }
        else
        {
            if (storedObject is PaddingWrapper wrapper)
            {
                // re-using the wrapper reduces the boxing hit.
                wrapper.Padding = value;
            }
            else
            {
                Debug.Assert(storedObject is null,
                    "object should either be null or PaddingWrapper"); // could someone have SetObject to this key behind our backs?
                SetObject(key, new PaddingWrapper(value));
            }
        }
    }

    public void SetRectangle(int key, Rectangle value)
    {
        var storedObject = GetObject(key, out var found);
        if (!found)
        {
            SetObject(key, new RectangleWrapper(value));
        }
        else
        {
            if (storedObject is RectangleWrapper wrapper)
            {
                // re-using the wrapper reduces the boxing hit.
                wrapper.Rectangle = value;
            }
            else
            {
                Debug.Assert(storedObject is null,
                    "object should either be null or RectangleWrapper"); // could someone have SetObject to this key behind our backs?
                SetObject(key, new RectangleWrapper(value));
            }
        }
    }

    public void SetSize(int key, Size value)
    {
        var storedObject = GetObject(key, out var found);
        if (!found)
        {
            SetObject(key, new SizeWrapper(value));
        }
        else
        {
            if (storedObject is SizeWrapper wrapper)
            {
                // re-using the wrapper reduces the boxing hit.
                wrapper.Size = value;
            }
            else
            {
                Debug.Assert(storedObject is null,
                    "object should either be null or SizeWrapper"); // could someone have SetObject to this key behind our backs?
                SetObject(key, new SizeWrapper(value));
            }
        }
    }

    /// <summary>
    ///  Stores the given value in the key.
    /// </summary>
    public void SetInteger(int key, int value)
    {
        var entryKey = SplitKey(key, out var element);
        if (!LocateIntegerEntry(entryKey, out var index))
        {
            // We must allocate a new entry.
            if (_intEntries is not null)
            {
                var newEntries = new IntegerEntry[_intEntries.Length + 1];

                if (index > 0)
                {
                    Array.Copy(_intEntries, 0, newEntries, 0, index);
                }

                if (_intEntries.Length - index > 0)
                {
                    Array.Copy(_intEntries, index, newEntries, index + 1, _intEntries.Length - index);
                }

                _intEntries = newEntries;
            }
            else
            {
                _intEntries = new IntegerEntry[1];
                Debug.Assert(index == 0, "LocateIntegerEntry should have given us a zero index.");
            }

            _intEntries[index].Key = entryKey;
        }

        // Now determine which value to set.
        switch (element)
        {
            case 0:
                _intEntries![index].Value1 = value;
                break;

            case 1:
                _intEntries![index].Value2 = value;
                break;

            case 2:
                _intEntries![index].Value3 = value;
                break;

            case 3:
                _intEntries![index].Value4 = value;
                break;

            default:
                Debug.Fail("Invalid element obtained from LocateIntegerEntry");
                break;
        }

        if (_intEntries != null)
        {
            _intEntries[index].Mask = (short)((1 << element) | (ushort)_intEntries[index].Mask);
        }
    }

    /// <summary>
    ///  Stores the given value in the key.
    /// </summary>
    public void SetObject(int key, object? value)
    {
        var entryKey = SplitKey(key, out var element);
        if (!LocateObjectEntry(entryKey, out var index))
        {
            // We must allocate a new entry.
            if (_objEntries is not null)
            {
                var newEntries = new ObjectEntry[_objEntries.Length + 1];

                if (index > 0)
                {
                    Array.Copy(_objEntries, 0, newEntries, 0, index);
                }

                if (_objEntries.Length - index > 0)
                {
                    Array.Copy(_objEntries, index, newEntries, index + 1, _objEntries.Length - index);
                }

                _objEntries = newEntries;
            }
            else
            {
                _objEntries = new ObjectEntry[1];
                Debug.Assert(index == 0, "LocateObjectEntry should have given us a zero index.");
            }

            _objEntries[index].Key = entryKey;
        }

        // Now determine which value to set.
        switch (element)
        {
            case 0:
                _objEntries![index].Value1 = value;
                break;

            case 1:
                _objEntries![index].Value2 = value;
                break;

            case 2:
                _objEntries![index].Value3 = value;
                break;

            case 3:
                _objEntries![index].Value4 = value;
                break;

            default:
                Debug.Fail("Invalid element obtained from LocateObjectEntry");
                break;
        }

        if (_objEntries != null)
        {
            _objEntries[index].Mask = (short)((ushort)_objEntries[index].Mask | (1 << element));
        }
    }

    /// <summary>
    ///  Takes the given key and splits it into an index and an element.
    /// </summary>
    private static short SplitKey(int key, out short element)
    {
        element = (short)(key & 0x00000003);
        return (short)(key & 0xFFFFFFFC);
    }

    /// <summary>
    ///  Stores the relationship between a key and a value.
    ///  We do not want to be so inefficient that we require
    ///  four bytes for each four byte property, so use an algorithm
    ///  that uses the bottom two bits of the key to identify
    ///  one of four elements in an entry.
    /// </summary>
    private struct IntegerEntry
    {
        public short Key;
        public short Mask;  // only lower four bits are used; mask of used values.
        public int Value1;
        public int Value2;
        public int Value3;
        public int Value4;
    }

    /// <summary>
    ///  Stores the relationship between a key and a value.
    ///  We do not want to be so inefficient that we require
    ///  four bytes for each four byte property, so use an algorithm
    ///  that uses the bottom two bits of the key to identify
    ///  one of four elements in an entry.
    /// </summary>
    private struct ObjectEntry
    {
        public short Key;
        public short Mask;  // only lower four bits are used; mask of used values.
        public object? Value1;
        public object? Value2;
        public object? Value3;
        public object? Value4;
    }
}