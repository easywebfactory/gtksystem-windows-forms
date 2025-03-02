using Gtk;
using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class LayoutBase: Layout, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public LayoutBase(Adjustment hadjustment, Adjustment vadjustment) : base(hadjustment, vadjustment)
    {
        Override = new GtkFormsControlOverride(this);
        Valign = Align.Start;
        Halign = Align.Start;
    }
    protected override bool OnDrawn(Cairo.Context cr)
    {
        var rec = new Gdk.Rectangle(0, 0, AllocatedWidth, AllocatedHeight);
        Override.DrawnBackColor(cr, rec);
        return base.OnDrawn(cr);
    }
}