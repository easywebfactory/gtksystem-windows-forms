using Cairo;
using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class ButtonBase: Gtk.Button, IGtkControl
{
    public IGtkControlOverride Override { get; set; }
    public ButtonBase() : base(new Gtk.Label { Wrap = true, SingleLineMode = false, LineWrap = true, LineWrapMode = Pango.WrapMode.WordChar })
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("Button");
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