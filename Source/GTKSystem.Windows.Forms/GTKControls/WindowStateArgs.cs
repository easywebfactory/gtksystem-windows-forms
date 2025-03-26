using System.ComponentModel;

namespace System.Windows.Forms;

public class WindowStateArgs : CancelEventArgs
{
    public FormWindowState State
    {
        get;
    }

    public WindowStateArgs(FormWindowState state)
    {
        State = state;
    }
}