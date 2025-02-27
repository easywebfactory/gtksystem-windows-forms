// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;
using System.Runtime.Serialization;


namespace System.Windows.Forms;

public sealed class Cursor : IDisposable, ISerializable
{
    private static Size cursorSize = Size.Empty;
    private readonly byte[]? _cursorData;
    public bool FreeHandle { get; }
    private readonly IntPtr _handle;
    private readonly Point hotSpot = default;
    private readonly Size size = default;
    internal string? CursorsProperty { get; }
    internal Gdk.CursorType CursorType { get; set; }
    internal Gdk.Pixbuf? CursorPixbuf { get; set; }
    internal Point CursorsXy { get; }
    internal Cursor(string resource, string cursorsProperty)
        : this(typeof(Cursors).Assembly.GetManifestResourceStream(resource)!)
    {
        CursorsProperty = cursorsProperty;
        FreeHandle = false;
    }

    public Cursor(IntPtr handle)
    {
        FreeHandle = false;
        _handle = handle;
        var cur = new Gdk.Cursor(handle);
        _cursorData = cur.Image.ReadPixelBytes().Data;
    }
    public Cursor(Gdk.CursorType cursorType)
    {
        FreeHandle = false;
        CursorType = cursorType;
    }
    public Cursor(string fileName)
    {
        _cursorData = File.ReadAllBytes(fileName);
        FreeHandle = true;
    }

    public Cursor(Type type, string resource) : this(type.Module.Assembly.GetManifestResourceStream(resource)!)
    {
    }
    internal Cursor(string resource, int x, int y)
        : this(typeof(Cursors).Assembly.GetManifestResourceStream(resource)!)
    {
        CursorsXy = new Point(x, y);
        FreeHandle = false;
    }
    public Cursor(Stream stream)
    {
        FreeHandle = true;
        CursorType = Gdk.CursorType.CursorIsPixmap;
        CursorPixbuf = new Gdk.Pixbuf(stream).ScaleSimple(48, 48, Gdk.InterpType.Tiles);
    }

    public static Rectangle Clip => new(0, 0, 16, 16);

    public static Cursor? Current
    {
        get;
        set;
    }

    public IntPtr Handle => IntPtr.Zero;

    public Point HotSpot => hotSpot;

    public static Point Position
    {
        get;
        set;
    }

    public Size Size => size;

    public object? Tag { get; set; }

    public IntPtr CopyHandle()
    {
        return IntPtr.Zero;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }
        IsDisposed = disposing;
    }

    public bool IsDisposed { get; set; }

    public void Draw(Graphics g, Rectangle targetRect)
    {

    }

    public void DrawStretched(Graphics g, Rectangle targetRect)
    {

    }

    ~Cursor()
    {
        Dispose(disposing: false);
    }

    void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
    {
        throw new PlatformNotSupportedException();
    }

    public static void Hide() { }

    internal byte[]? GetData()
    {
        return (byte[]?)_cursorData?.Clone();
    }

    public static void Show() { }

    public override string ToString() => $"[Cursor: {CursorsProperty ?? base.ToString()}]";

    public static bool operator ==(Cursor? left, Cursor? right)
    {
        return right is null || left is null ? left is null && right is null : left.GetHashCode() == right.GetHashCode();
    }

    public static bool operator !=(Cursor? left, Cursor? right) => !(left == right);

    public override int GetHashCode()
    {
        if (_cursorData != null)
        {
            return _cursorData.GetHashCode();
        }

        var intPtr = GetHandle();
        if (intPtr != default)
        {
            return intPtr.GetHashCode();
        }

        return 0;

    }

    private IntPtr GetHandle()
    {
        return _handle;
    }

    public override bool Equals(object? obj) => obj is Cursor cursor && this == cursor;
}