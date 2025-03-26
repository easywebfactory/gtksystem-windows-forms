namespace System.Windows.Forms;

public sealed class ToolStripDropDownBase : Gtk.Menu, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public ToolStripDropDownBase()
    {
        Override = new GtkFormsControlOverride(this);
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
}