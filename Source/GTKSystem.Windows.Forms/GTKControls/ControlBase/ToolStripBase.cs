using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class ToolStripBase : Gtk.MenuBar, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public ToolStripBase()
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("ToolStrip");
        Hexpand = true;
        Vexpand = false;
        Valign = Gtk.Align.Start;
        Halign = Gtk.Align.Fill;
        HeightRequest = 20;
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
}