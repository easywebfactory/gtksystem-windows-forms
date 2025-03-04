using Gtk;

namespace System.Windows.Forms;

public sealed class UserControlBase : ScrollableBoxBase
{
    public UserControlBase()
    {
        Override.AddClass("UserControl");
        MarginStart = 0;
        MarginTop = 0;
        BorderWidth = 0;
        ShadowType = ShadowType.None;
        Events = Gdk.EventMask.AllEventsMask;
        Expand = false;
        Hexpand = false;
        Vexpand = false;
    }
}