namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class DataGridViewBase : ScrollableBoxBase
    {
        internal Gtk.TreeView GridView = new Gtk.TreeView();
        internal DataGridViewBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("DataGridView");
            this.Override.BackColor = System.Drawing.Color.White;
            this.BorderWidth = 0;
            this.ShadowType = Gtk.ShadowType.Out;
            GridView.Valign = Gtk.Align.Fill;
            GridView.Halign = Gtk.Align.Fill;
            GridView.Expand = true;
            GridView.BorderWidth = 0;
            GridView.EnableGridLines = Gtk.TreeViewGridLines.Both;
            GridView.EnableTreeLines = true;
            this.AutoScroll = true;
            this.Add(GridView);
        }
    }
}
