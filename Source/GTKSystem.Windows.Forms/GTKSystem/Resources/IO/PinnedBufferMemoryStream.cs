using System.Runtime.InteropServices;

namespace System.Windows.Forms.Resources.IO;

internal sealed class PinnedBufferMemoryStream : UnmanagedMemoryStream
{
    private byte[] _array;

    private GCHandle _pinningHandle;

    internal unsafe PinnedBufferMemoryStream(byte[] array)
    {
        _array = array;
        _pinningHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
        var num = array.Length;
        fixed (byte* pointer = &MemoryMarshal.GetReference<byte>(array))
        {
            Initialize(pointer, num, num, FileAccess.Read);
        }
    }

    ~PinnedBufferMemoryStream()
    {
        Dispose(disposing: false);
    }

    protected override void Dispose(bool disposing)
    {
        if (_pinningHandle.IsAllocated)
        {
            _pinningHandle.Free();
        }
        base.Dispose(disposing);
    }
}