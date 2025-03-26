using Cairo;

namespace System.Windows.Forms;

public sealed class TextBoxBase : Gtk.Entry, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public TextBoxBase()
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("TextBox");
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