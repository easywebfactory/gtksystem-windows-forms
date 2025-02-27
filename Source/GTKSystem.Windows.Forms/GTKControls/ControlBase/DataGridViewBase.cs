using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class DataGridViewBase : ScrollableBoxBase
{
    internal Gtk.TreeView? gridView = new();
    public DataGridViewBase()
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("DataGridView");
        Override.BackColor = Drawing.Color.White;
        BorderWidth = 0;
        ShadowType = Gtk.ShadowType.Out;
        if (gridView != null)
        {
            gridView.Valign = Gtk.Align.Start;
            gridView.Halign = Gtk.Align.Start;
            gridView.BorderWidth = 0;
            gridView.EnableGridLines = Gtk.TreeViewGridLines.Both;
            gridView.EnableTreeLines = true;
            AutoScroll = true;
            Add(gridView);
        }
    }
}