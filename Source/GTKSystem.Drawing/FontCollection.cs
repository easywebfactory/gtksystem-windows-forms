namespace System.Drawing.Text;

//
// 摘要:
//     Provides a base class for installed and private font collections.
public abstract class FontCollection : IDisposable
{
    //
    // 摘要:
    //     Gets the array of System.Drawing.FontFamily objects associated with this System.Drawing.Text.FontCollection.
    //
    // 返回结果:
    //     An array of System.Drawing.FontFamily objects.
    public FontFamily[]? Families { get; } = default;

    //
    // 摘要:
    //     Releases all resources used by this System.Drawing.Text.FontCollection.
    public void Dispose() { }
    //
    // 摘要:
    //     Releases the unmanaged resources used by the System.Drawing.Text.FontCollection
    //     and optionally releases the managed resources.
    //
    // 参数:
    //   disposing:
    //     true to release both managed and unmanaged resources; false to release only unmanaged
    //     resources.
    protected virtual void Dispose(bool disposing) { }
}