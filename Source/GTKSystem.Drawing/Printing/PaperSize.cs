// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;

namespace System.Drawing.Printing;

public class PaperSize
{
    private PaperKind _kind;
    private string _name;

    // Standard hundredths of an inch units
    private int _width;
    private int _height;
    private readonly bool _createdByDefaultConstructor;

    public PaperSize()
    {
        _kind = PaperKind.Custom;
        _name = string.Empty;
        _createdByDefaultConstructor = true;
    }

    public PaperSize(PaperKind kind, string name, int width, int height)
    {
        _kind = kind;
        _name = name;
        _width = width;
        _height = height;
    }

    public PaperSize(string name, int width, int height)
    {
        _kind = PaperKind.Custom;
        _name = name;
        _width = width;
        _height = height;
    }

    public int Height
    {
        get => _height;
        set
        {
            if (_kind != PaperKind.Custom && !_createdByDefaultConstructor)
            {
                throw new ArgumentException(@"PSizeNotCustom", nameof(value));
            }

            _height = value;
        }
    }

    public PaperKind Kind
        => PaperKind.Custom;

    public string PaperName
    {
        get => _name;
        set
        {
            if (_kind != PaperKind.Custom && !_createdByDefaultConstructor)
            {
                throw new ArgumentException(@"PSizeNotCustom", nameof(value));
            }

            _name = value;
        }
    }

    public int RawKind
    {
        get => (int)_kind;
        set => _kind = (PaperKind)value;
    }

    public int Width
    {
        get => _width;
        set
        {
            if (_kind != PaperKind.Custom && !_createdByDefaultConstructor)
            {
                throw new ArgumentException(@"PSizeNotCustom", nameof(value));
            }

            _width = value;
        }
    }

    public override string ToString() => $"[PaperSize {PaperName} Kind={Kind} Height={Height.ToString(CultureInfo.InvariantCulture)} Width={Width.ToString(CultureInfo.InvariantCulture)}]";
}