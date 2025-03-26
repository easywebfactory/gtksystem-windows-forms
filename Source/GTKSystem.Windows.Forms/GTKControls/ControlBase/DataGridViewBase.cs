namespace System.Windows.Forms;

public sealed class DataGridViewBase : ScrollableBoxBase
{
    internal Gtk.TreeView GridView = new();
    public DataGridViewBase()
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("DataGridView");
        Override.BackColor = Drawing.Color.White;
        BorderWidth = 0;
        ShadowType = Gtk.ShadowType.Out;
        if (GridView == null)
        {
            return;
        }

        GridView.Valign = Gtk.Align.Start;
        GridView.Halign = Gtk.Align.Start;
        GridView.BorderWidth = 0;
        GridView.EnableGridLines = Gtk.TreeViewGridLines.Both;
        GridView.EnableTreeLines = true;
        AutoScroll = true;
        Add(GridView);
    }
}