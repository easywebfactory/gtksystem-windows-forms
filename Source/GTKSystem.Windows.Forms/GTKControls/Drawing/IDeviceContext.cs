namespace System.Drawing;

//
// 摘要:
//     Defines methods for obtaining and releasing an existing handle to a Windows device
//     context.
public interface IDeviceContext : IDisposable
{
    //
    // 摘要:
    //     Returns the handle to a Windows device context.
    //
    // 返回结果:
    //     An System.IntPtr representing the handle of a device context.
    IntPtr GetHdc();
    //
    // 摘要:
    //     Releases the handle of a Windows device context.
    void ReleaseHdc();
}