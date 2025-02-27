using System.Runtime.CompilerServices;

namespace System.Windows.Forms;

public class BindingManagerDataErrorEventArgs : EventArgs
{
    private readonly Exception exception;

    public Exception Exception
    {
        [CompilerGenerated]
        get => exception;
    }

    public BindingManagerDataErrorEventArgs(Exception exception)
    {
        this.exception = exception;
    }
}