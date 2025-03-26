using Cairo;

namespace System.Windows.Forms;

public sealed class LabelBase : Gtk.Label, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public LabelBase()
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("Label");
        Xalign = 0.0f;
        Yalign = 0.0f;
        Valign = Gtk.Align.Start;
        Halign = Gtk.Align.Start;
        Wrap = true;
        LineWrap = true;
        LineWrapMode = Pango.WrapMode.WordChar;
    }

    public LabelBase(string text) : base(text)
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("Label");
        Xalign = 0.0f;
        Yalign = 0.0f;
        Valign = Gtk.Align.Start;
        Halign = Gtk.Align.Start;
        Wrap = true;
        LineWrap = true;
        LineWrapMode = Pango.WrapMode.WordChar;
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