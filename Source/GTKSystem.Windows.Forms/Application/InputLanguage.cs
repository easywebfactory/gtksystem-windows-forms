﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Gdk;

namespace System.Windows.Forms;

/// <summary>
///  Provides methods and fields to manage the input language.
/// </summary>
public sealed class InputLanguage
{
    /// <summary>
    ///  The HKL handle.
    /// </summary>
    private readonly IntPtr _handle;

    internal InputLanguage(IntPtr handle)
    {
        _handle = handle;
    }

    /// <summary>
    ///  Returns the culture of the current input language.
    /// </summary>
    public CultureInfo Culture => new((int)_handle & 0xFFFF);

    /// <summary>
    ///  Gets or sets the input language for the current thread.
    /// </summary>
    public static InputLanguage? CurrentInputLanguage
    {
        get => new(PangoHelper.ContextGet().Language.Handle);
        set
        {
            if (value == null)
            {
                value = DefaultInputLanguage;
            }

            throw new NotImplementedException(value.ToString());
        }
    }

    /// <summary>
    ///  Returns the default input language for the system.
    /// </summary>
    public static InputLanguage DefaultInputLanguage => new(PangoHelper.ContextGet().Language.Handle);

    /// <summary>
    ///  Returns the handle for the input language.
    /// </summary>
    public IntPtr Handle => _handle;

    /// <summary>
    ///  Returns a list of all installed input languages.
    /// </summary>
    public static InputLanguageCollection InstalledInputLanguages
    {
        get
        {
            var size = 1;
            var ils = new InputLanguage[size];

            for (var i = 0; i < size; i++)
            {
                ils[i] = new InputLanguage(PangoHelper.ContextGet().Language.Handle);
            }

            return new InputLanguageCollection(ils);
        }
    }

    /// <summary>
    ///  Returns the name of the current keyboard layout as it appears in the Windows
    ///  Regional Settings on the computer.
    /// </summary>
    public string LayoutName => PangoHelper.ContextGet().Language.ToString();

    /// <summary>
    ///  Specifies whether two input languages are equal.
    /// </summary>
    public override bool Equals(object? value)
    {
        return value is InputLanguage other && _handle == other._handle;
    }

    /// <summary>
    ///  Returns the input language associated with the specified culture.
    /// </summary>
    public static InputLanguage FromCulture(CultureInfo culture)
    {
        if (culture == null)
        {
            throw new ArgumentNullException(nameof(culture));
        }
        // KeyboardLayoutId is the LCID for built-in cultures, but it
        // is the CU-preferred keyboard language for custom cultures.
        var lcid = culture.KeyboardLayoutId;

        foreach (InputLanguage lang in InstalledInputLanguages)
        {
            if ((unchecked((int)(long)lang._handle) & 0xFFFF) == lcid)
            {
                return lang;
            }
        }
        return DefaultInputLanguage;
    }

    /// <summary>
    ///  Hash code for this input language.
    /// </summary>
    public override int GetHashCode() => unchecked((int)(long)_handle);

    private static string PadWithZeroes(string input, int length)
    {
        return "0000000000000000".Substring(0, length - input.Length) + input;
    }
}