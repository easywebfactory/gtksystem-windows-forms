 
namespace System.Windows.Forms;

public class ContainerControl : ScrollableControl, IContainerControl
{
    private Control? activeControl;
    public Control? ActiveControl
    {
        get
        {
            foreach (var control in Controls)
            {
                if (control is Control con)
                {
                    if ((con.Widget.StateFlags & Gtk.StateFlags.Active) != 0 || con.Widget.IsFocus)
                        return con;
                }
            }
            return activeControl;
        }
        set => ActivateControl(value);
    }

    public bool ActivateControl(Control? active)
    {
        activeControl = active;
        if (active?.GtkControl is Gtk.Widget widget)
        {
            return widget.Activate();
        }
        return false;
    }
    public virtual StatusStrip? StatusStrip { get; set; }
}