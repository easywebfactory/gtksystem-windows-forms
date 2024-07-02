using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TreeViewBase : ScrollableBoxBase
    {
        internal Gtk.TreeView TreeView = new Gtk.TreeView();

        internal TreeViewBase() : base()
        {
            this.Override.AddClass("TreeView");
            this.TreeView.Valign = Align.Fill;
            this.TreeView.Halign = Align.Fill;
            this.TreeView.MarginTop = 3;
            this.TreeView.MarginStart = 1;
            this.TreeView.MarginEnd = 1;
            this.TreeView.MarginBottom = 1;
            this.TreeView.EnableGridLines = Gtk.TreeViewGridLines.Horizontal;
            this.TreeView.EnableTreeLines = true;
            this.TreeView.HeadersVisible = false;
            this.TreeView.ActivateOnSingleClick = true;
            this.TreeView.BorderWidth = 1;
            this.AutoScroll = true;
            this.Add(this.TreeView);
        }
    }
}
