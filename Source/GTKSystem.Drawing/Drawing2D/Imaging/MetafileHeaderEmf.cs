using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging;

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

    public SafeNativeMethods.Enhmetaheader EmfHeader;

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
    public struct Enhmetaheader
    {
        public int iType;

        public int nSize;

        public int rclBoundsLeft;

        public int rclBoundsTop;

        public int rclBoundsRight;

        public int rclBoundsBottom;

        public int rclFrameLeft;

        public int rclFrameTop;

        public int rclFrameRight;

        public int rclFrameBottom;

        public int dSignature;

        public int nVersion;

        public int nBytes;

        public int nRecords;

        public short nHandles;

        public short sReserved;

        public int nDescription;

        public int offDescription;

        public int nPalEntries;

        public int szlDeviceCx;

        public int szlDeviceCy;

        public int szlMillimetersCx;

        public int szlMillimetersCy;

        public int cbPixelFormat;

        public int offPixelFormat;

        public int bOpenGl;
    }
}