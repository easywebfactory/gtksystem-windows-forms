using System.Runtime.InteropServices;

namespace System.Drawing.Imaging;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
internal struct WmfMetaHeader
{
    internal short _type;

    internal short _headerSize;

    internal short _version;

    internal int _size;

    internal short _noObjects;

    internal int _maxRecord;

    internal short _noParameters;
}