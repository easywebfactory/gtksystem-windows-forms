using Cairo;
using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class FlowLayoutPanelBase : Gtk.FlowBox, IGtkControl
{
    public IGtkControlOverride Override { get; set; }
    public FlowLayoutPanelBase()
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("FlowLayoutPanel");
        Valign = Gtk.Align.Start;
        Halign = Gtk.Align.Start;
    }
    protected override void OnShown()
    {
        Override.OnAddClass();
        base.OnShown();
    }
    protected override bool OnDrawn(Context? cr)
    {
        var rec = new Gdk.Rectangle(0, 0, AllocatedWidth, AllocatedHeight);
        Override.OnPaint(cr, rec);
        return base.OnDrawn(cr);
    }
}