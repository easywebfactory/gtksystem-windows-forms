using Gtk;

namespace System.Windows.Forms;

public sealed class TreeViewBase : ScrollableBoxBase
{
    internal Gtk.TreeView treeView = new();

    public TreeViewBase()
    {
        Override.AddClass("TreeView");
        treeView.Valign = Align.Fill;
        treeView.Halign = Align.Fill;
        treeView.BorderWidth = 1;
        treeView.Margin = 0;
        treeView.EnableGridLines = TreeViewGridLines.None;
        treeView.EnableTreeLines = true;
        treeView.HeadersVisible = false;
        treeView.ActivateOnSingleClick = true;
            
        AutoScroll = true;
        Add(treeView);
    }
}