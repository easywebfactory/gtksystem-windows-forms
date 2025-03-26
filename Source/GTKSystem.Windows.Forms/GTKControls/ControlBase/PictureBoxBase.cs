using Cairo;

namespace System.Windows.Forms;

public sealed class PictureBoxBase : Gtk.Image, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public PictureBoxBase()
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("PictureBox");
        Valign = Gtk.Align.Fill;
        Halign = Gtk.Align.Fill;
        Expand = true;
        Xalign = 0;
        Yalign = 0;
    }
    protected override void OnShown()
    {
        Override.OnAddClass();
        base.OnShown();
    }
    protected override bool OnDrawn(Context? cr)
    {
        if (Pixbuf != null)
        {
            cr?.Scale(AllocatedWidth * 1f / Pixbuf.Width * 1f, AllocatedHeight * 1f / Pixbuf.Height * 1f);
        }
        var rec = new Gdk.Rectangle(0, 0, AllocatedWidth, AllocatedHeight);
        Override.OnDrawnBackground(cr, rec);
        Override.OnPaint(cr, rec);
        base.OnDrawn(cr);
        return true;
    }
}