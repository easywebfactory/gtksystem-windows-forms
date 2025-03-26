using Gtk;

namespace System.Windows.Forms;

public sealed class TreeViewBase : ScrollableBoxBase
{
    internal Gtk.TreeView TreeView = new();

    public TreeViewBase()
    {
        Override.AddClass("TreeView");
        TreeView.Valign = Align.Fill;
        TreeView.Halign = Align.Fill;
        TreeView.BorderWidth = 1;
        TreeView.Margin = 0;
        TreeView.EnableGridLines = TreeViewGridLines.None;
        TreeView.EnableTreeLines = true;
        TreeView.HeadersVisible = false;
        TreeView.ActivateOnSingleClick = true;
            
        AutoScroll = true;
        Add(TreeView);
    }
}