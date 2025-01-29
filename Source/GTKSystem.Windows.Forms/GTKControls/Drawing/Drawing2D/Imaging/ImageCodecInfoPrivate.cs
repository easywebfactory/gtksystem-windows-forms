using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct ImageCodecInfoPrivate
    {
        public Guid Clsid;

        public Guid FormatID;

        public IntPtr CodecName;

        public IntPtr DllName;

        public IntPtr FormatDescription;

        public IntPtr FilenameExtension;

        public IntPtr MimeType;

        public int Flags;

        public int Version;

        public int SigCount;

        public int SigSize;

        public IntPtr SigPattern;

        public IntPtr SigMask;
    }
}