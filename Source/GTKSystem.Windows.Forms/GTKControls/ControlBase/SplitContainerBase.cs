using Cairo;
using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class SplitContainerBase : Gtk.Paned, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public SplitContainerBase() : base(Gtk.Orientation.Vertical)
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("SplitContainer");
        BorderWidth = 0;
        WideHandle = true;
        Orientation = Gtk.Orientation.Horizontal;
        Halign = Gtk.Align.Start;
        Valign = Gtk.Align.Start;
    }

    public void AddClass(string cssClass)
    {
        Override.AddClass(cssClass);
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