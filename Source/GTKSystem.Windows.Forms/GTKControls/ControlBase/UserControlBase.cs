using Gtk;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class UserControlBase : ScrollableBoxBase
    {
        public UserControlBase() : base()
        {
            this.Override.AddClass("UserControl");
            this.MarginStart = 0;
            this.MarginTop = 0;
            this.BorderWidth = 0;
            this.ShadowType = ShadowType.None;
            this.Events = Gdk.EventMask.AllEventsMask;
            this.Expand = false;
            this.Hexpand = false;
            this.Vexpand = false;
        }
    }
}
