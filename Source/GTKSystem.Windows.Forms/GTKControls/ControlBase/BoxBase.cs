namespace System.Windows.Forms;

public sealed class BoxBase: Gtk.Box, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public BoxBase(Gtk.Orientation orientation, int spacing) : base(orientation, spacing)
    {
        Override = new GtkFormsControlOverride(this);
        Valign = Gtk.Align.Start;
        Halign = Gtk.Align.Start;
    }
    protected override bool OnDrawn(Cairo.Context cr)
    {
        var rec = new Gdk.Rectangle(0, 0, AllocatedWidth, AllocatedHeight);
        Override.DrawnBackColor(cr, rec);
        return base.OnDrawn(cr);
    }
}