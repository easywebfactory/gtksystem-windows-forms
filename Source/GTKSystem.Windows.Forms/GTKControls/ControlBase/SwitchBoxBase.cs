using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase;

public sealed class SwitchBoxBase : Gtk.Switch, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public SwitchBoxBase() : base()
    {
        Override = new GtkControlOverride(this);
        Override.AddClass("SwitchBox");
        Valign = Gtk.Align.Start;
        Halign = Gtk.Align.Start;
    }
    protected override void OnShown()
    {
        Override.OnAddClass();
        base.OnShown();
    }
}