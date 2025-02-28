using Cairo;
using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class RadioButtonBase : Gtk.RadioButton, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public RadioButtonBase() : base(new Gtk.RadioButton("baseradio"))
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("RadioButton");
        Valign = Gtk.Align.Start;
        Halign = Gtk.Align.Start;
    }
    public RadioButtonBase(Gtk.RadioButton radioGroupMember) : base(radioGroupMember)
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("RadioButton");
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