namespace System.Drawing;

/// <summary>
/// Defines methods for obtaining and releasing an existing handle to a Windows device
/// context.
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IDeviceContext : IDisposable
{
    /// <summary>
    /// Returns the handle to a Windows device context.
    /// </summary>
    /// <returns>An System.IntPtr representing the handle of a device context.</returns>
    IntPtr GetHdc();

    /// <summary>
    /// Releases the handle of a Windows device context.    
    /// </summary>
    void ReleaseHdc();
}