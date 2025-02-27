using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class ListViewBase : Gtk.Viewport, IGtkControl
{
    public Gtk.Box box = new(Gtk.Orientation.Vertical, 0);
    public IGtkControlOverride Override { get; set; }
    public ListViewBase()
    {
        Override = new GtkFormsControlOverride(this);
        StyleContext.AddClass("view");
        Override.AddClass("ListView");
        BorderWidth = 1;
        Add(box);
    }
}