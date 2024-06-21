using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TreeViewBase : ScrollableBoxBase
    {
        internal Gtk.TreeView TreeView = new Gtk.TreeView();
        internal TreeViewBase() : base()
        {
            this.Override.AddClass("TreeView");
            this.TreeView.BorderWidth = 0;
            this.TreeView.Expand = true;
            this.TreeView.HeadersVisible = false;
            this.TreeView.ActivateOnSingleClick = true;
            this.AutoScroll = true;
            this.Add(this.TreeView);
        }
    }
}
