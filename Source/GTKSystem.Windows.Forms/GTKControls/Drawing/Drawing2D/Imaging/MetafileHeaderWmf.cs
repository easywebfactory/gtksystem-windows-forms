using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal sealed class MetafileHeaderWmf
	{
		public MetafileType type;

		public int size = Marshal.SizeOf<MetafileHeaderWmf>();

		public int version;

		public EmfPlusFlags emfPlusFlags;

		public float dpiX;

		public float dpiY;

		public int X;

		public int Y;

		public int Width;

		public int Height;

		[MarshalAs(UnmanagedType.Struct)]
		public MetaHeader WmfHeader = new MetaHeader();

		public int dummy1;

		public int dummy2;

		public int dummy3;

		public int dummy4;

		public int dummy5;

		public int dummy6;

		public int dummy7;

		public int dummy8;

		public int dummy9;

		public int dummy10;

		public int dummy11;

		public int dummy12;

		public int dummy13;

		public int dummy14;

		public int dummy15;

		public int dummy16;

		public int EmfPlusHeaderSize;

		public int LogicalDpiX;

		public int LogicalDpiY;
	}
}
