using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class MetafileHeaderEmf
	{
		public MetafileType type;

		public int size;

		public int version;

		public EmfPlusFlags emfPlusFlags;

		public float dpiX;

		public float dpiY;

		public int X;

		public int Y;

		public int Width;

		public int Height;

		public SafeNativeMethods.ENHMETAHEADER EmfHeader;

		public int EmfPlusHeaderSize;

		public int LogicalDpiX;

		public int LogicalDpiY;

		internal ref byte GetPinnableReference()
		{
			return ref Unsafe.As<MetafileType, byte>(ref type);
		}
	}
    internal static class SafeNativeMethods
    {
        public struct ENHMETAHEADER
        {
            public int iType;

            public int nSize;

            public int rclBounds_left;

            public int rclBounds_top;

            public int rclBounds_right;

            public int rclBounds_bottom;

            public int rclFrame_left;

            public int rclFrame_top;

            public int rclFrame_right;

            public int rclFrame_bottom;

            public int dSignature;

            public int nVersion;

            public int nBytes;

            public int nRecords;

            public short nHandles;

            public short sReserved;

            public int nDescription;

            public int offDescription;

            public int nPalEntries;

            public int szlDevice_cx;

            public int szlDevice_cy;

            public int szlMillimeters_cx;

            public int szlMillimeters_cy;

            public int cbPixelFormat;

            public int offPixelFormat;

            public int bOpenGL;
        }
    }
}
