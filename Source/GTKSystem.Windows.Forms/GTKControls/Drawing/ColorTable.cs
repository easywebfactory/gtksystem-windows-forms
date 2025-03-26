﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;

namespace System.Drawing;

#if NETSTANDARD
using System.Drawing.Gtk;
#endif

internal static class ColorTable
{
    private static Dictionary<string, Color>? s_colorConstants;

    private static Dictionary<string, Color> GetColors()
    {
        var colors = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
        FillWithProperties(colors, typeof(Color));
        FillWithProperties(colors, typeof(SystemColors));
        return colors;
    }

    private static void FillWithProperties(
        Dictionary<string, Color> dictionary,
        Type typeWithColors)
    {
        foreach (var prop in typeWithColors.GetProperties(BindingFlags.Public | BindingFlags.Static))
        {
            if (prop.PropertyType == typeof(Color))
                dictionary[prop.Name] = (Color)prop.GetValue(null, null)!;
        }
    }

    internal static Dictionary<string, Color> Colors
    {
        get
        {
            return s_colorConstants ??= GetColors();
        }
    }

    internal static bool TryGetNamedColor(string name, out Color result) => Colors.TryGetValue(name, out result);

    internal static bool IsKnownNamedColor(string name) => Colors.TryGetValue(name, out _);
}