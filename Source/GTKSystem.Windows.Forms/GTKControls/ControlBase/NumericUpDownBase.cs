using Cairo;

namespace System.Windows.Forms;

public sealed class NumericUpDownBase : Gtk.SpinButton, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public NumericUpDownBase() : base(0, 100, 1)
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("NumericUpDown");
        Value = 0;
        Orientation = Gtk.Orientation.Horizontal;
        Valign = Gtk.Align.Start;
        Halign = Gtk.Align.Start;
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