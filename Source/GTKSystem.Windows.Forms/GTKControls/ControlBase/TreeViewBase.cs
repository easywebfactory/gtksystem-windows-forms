using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TreeViewBase : ScrollableBoxBase
    {
        internal Gtk.TreeView TreeView = new Gtk.TreeView();

        public TreeViewBase() : base()
        {
            this.Override.AddClass("TreeView");
            this.TreeView.Valign = Align.Fill;
            this.TreeView.Halign = Align.Fill;
            this.TreeView.BorderWidth = 1;
            this.TreeView.Margin = 0;
            this.TreeView.EnableGridLines = Gtk.TreeViewGridLines.None;
            this.TreeView.EnableTreeLines = true;
            this.TreeView.HeadersVisible = false;
            this.TreeView.ActivateOnSingleClick = true;
            
            this.AutoScroll = true;
            this.Add(this.TreeView);
        }
    }
}
