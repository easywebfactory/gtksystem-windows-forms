using Gtk;

namespace System.Windows.Forms;

public class DialogEventArgs : EventArgs
{
    public DialogEventArgs(Dialog dia)
    {
        Dialog = dia;
    }

    public Dialog Dialog { get; }
}