// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using GLib;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;


namespace System.Windows.Forms
{

    public sealed class Cursor : IDisposable, ISerializable
    {
        private static Size s_cursorSize = Size.Empty;
        private readonly byte[]? _cursorData;
        private readonly bool _freeHandle;
        private IntPtr _handle;
        internal string? CursorsProperty { get; }
        internal Gdk.CursorType CursorType { get; set; }
        internal Gdk.Pixbuf CursorPixbuf{ get; set; }
        internal Point CursorsXY { get; }
        internal Cursor(string resource, string cursorsProperty)
            : this(typeof(Cursors).Assembly.GetManifestResourceStream(resource))
        {
            CursorsProperty = cursorsProperty;
            _freeHandle = false;
        }

        public Cursor(IntPtr handle)
        {
            _freeHandle = false;
            _handle= handle;
            var cur =new Gdk.Cursor(handle);
            _cursorData = cur.Image.ReadPixelBytes().Data;
        }
        public Cursor(Gdk.CursorType cursorType)
        {
            _freeHandle = false;
            CursorType = cursorType;
        }
        public Cursor(string fileName)
        {
            _cursorData = File.ReadAllBytes(fileName);
            _freeHandle = true;
        }

        public Cursor(Type type, string resource): this(type.Module.Assembly.GetManifestResourceStream(resource)!)
        {
        }
        internal Cursor(string resource, int x, int y)
            : this(typeof(Cursors).Assembly.GetManifestResourceStream(resource))
        {
            CursorsXY = new Point(x, y);
            _freeHandle = false;
        }
        public Cursor(Stream stream)
        {
            _freeHandle = true;
            CursorType = Gdk.CursorType.CursorIsPixmap;
            CursorPixbuf = new Gdk.Pixbuf(stream).ScaleSimple(48, 48, Gdk.InterpType.Tiles);
        }

        public static unsafe Rectangle Clip
        {
            get
            {
                return new Rectangle(0, 0, 16, 16);
            }
            set
            {

            }
        }

        public static Cursor? Current
        {
            get;
            set;
        }

        public IntPtr Handle
        {
            get
            {
                return IntPtr.Zero;
            }
        }

        public Point HotSpot
        {
            get;
        }

        public static Point Position
        {
            get;
            set;
        }

        public Size Size
        {
            get;
        }
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

        }

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

        internal unsafe byte[] GetData()
        {
            return (byte[])_cursorData.Clone();
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
            else if (_handle != IntPtr.Zero)
            {
                return _handle.GetHashCode();
            }
            else
                return 0;

        }

        public override bool Equals(object? obj) => obj is Cursor cursor && this == cursor;
    }
}