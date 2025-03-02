using System.Runtime.InteropServices;

namespace System.Drawing;

internal static class NativeMethods
{
    internal struct Bitmapinfoheader
    {
        public int biSize;

        public int biWidth;

        public int biHeight;

        public short biPlanes;

        public short biBitCount;

        public int biCompression;

        public int biSizeImage;

        public int biXPelsPerMeter;

        public int biYPelsPerMeter;

        public int biClrUsed;

        public int biClrImportant;
    }

    internal struct Paletteentry
    {
        public byte peRed;

        public byte peGreen;

        public byte peBlue;

        public byte peFlags;
    }

    internal struct Rgbquad
    {
        public byte rgbBlue;

        public byte rgbGreen;

        public byte rgbRed;

        public byte rgbReserved;
    }

    public const int maxPath = 260;

    internal const int smRemotesession = 4096;

    internal const int dibRgbColors = 0;

    internal const int biBitfields = 3;

    internal const int biRgb = 0;

    internal static HandleRef NullHandleRef => new(null, IntPtr.Zero);
}