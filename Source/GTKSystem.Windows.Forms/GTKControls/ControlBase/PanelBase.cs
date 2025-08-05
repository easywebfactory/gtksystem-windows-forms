using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class PanelBase: ScrollableBoxBase
    {
        public PanelBase() : base()
        {
            this.Override.AddClass("Panel");
            this.ShadowType = Gtk.ShadowType.None;
            this.BorderWidth = 0;
        }
    }
}
