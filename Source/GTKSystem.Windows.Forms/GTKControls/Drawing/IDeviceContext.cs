namespace System.Drawing
{
    //
    // summary:
    //     Defines methods for obtaining and releasing an existing handle to a Windows device
    //     context.
    public interface IDeviceContext : IDisposable
    {
        //
        // summary:
        //     Returns the handle to a Windows device context.
        //
        // Return results:
        //     An System.IntPtr representing the handle of a device context.
        IntPtr GetHdc();
        //
        // summary:
        //     Releases the handle of a Windows device context.
        void ReleaseHdc();
    }
}
