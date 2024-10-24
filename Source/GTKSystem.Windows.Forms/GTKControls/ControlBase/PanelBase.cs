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
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnDrawnBackground(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
