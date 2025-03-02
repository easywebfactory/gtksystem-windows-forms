using Cairo;
using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class TrackBarBase : Gtk.Viewport, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public TrackBarBase()
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("TrackBar");
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
        Override.OnDrawnBackground(cr, rec);
        Override.OnPaint(cr, rec);
        return base.OnDrawn(cr);
    }
}