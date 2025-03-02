using Cairo;

namespace System.Windows.Forms;

public sealed class PanelBase: ScrollableBoxBase
{
    public PanelBase()
    {
        Override.AddClass("Panel");
        ShadowType = Gtk.ShadowType.None;
        BorderWidth = 0;
    }
    protected override bool OnDrawn(Context? cr)
    {
        var rec = new Gdk.Rectangle(0, 0, AllocatedWidth, AllocatedHeight);
        Override.OnDrawnBackground(cr, rec);
        return base.OnDrawn(cr);
    }
}