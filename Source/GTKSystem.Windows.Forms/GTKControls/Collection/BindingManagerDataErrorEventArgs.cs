using System.Runtime.CompilerServices;

namespace System.Windows.Forms;

public class BindingManagerDataErrorEventArgs : EventArgs
{
    private readonly Exception _exception;

    public Exception Exception
    {
        [CompilerGenerated]
        get => _exception;
    }

    public BindingManagerDataErrorEventArgs(Exception exception)
    {
        _exception = exception;
    }
}