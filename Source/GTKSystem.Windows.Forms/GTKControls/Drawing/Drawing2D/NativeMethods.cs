using System.Runtime.InteropServices;

namespace System.Drawing
{
	internal static class NativeMethods
	{
		internal struct BITMAPINFOHEADER
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

		internal struct PALETTEENTRY
		{
			public byte peRed;

			public byte peGreen;

			public byte peBlue;

			public byte peFlags;
		}

		internal struct RGBQUAD
		{
			public byte rgbBlue;

			public byte rgbGreen;

			public byte rgbRed;

			public byte rgbReserved;
		}

		public const int MAX_PATH = 260;

		internal const int SM_REMOTESESSION = 4096;

		internal const int DIB_RGB_COLORS = 0;

		internal const int BI_BITFIELDS = 3;

		internal const int BI_RGB = 0;

		internal static HandleRef NullHandleRef => new HandleRef(null, IntPtr.Zero);
	}
}
