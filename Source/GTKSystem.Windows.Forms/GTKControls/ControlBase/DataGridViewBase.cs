namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class DataGridViewBase : ScrollableBoxBase
    {
        internal Gtk.TreeView GridView = new Gtk.TreeView();
        public DataGridViewBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("DataGridView");
            this.Override.BackColor = System.Drawing.Color.White;
            this.BorderWidth = 0;
            this.ShadowType = Gtk.ShadowType.Out;
            GridView.Valign = Gtk.Align.Start;
            GridView.Halign = Gtk.Align.Start;
            GridView.BorderWidth = 0;
            GridView.EnableGridLines = Gtk.TreeViewGridLines.Both;
            GridView.EnableTreeLines = true;
            this.AutoScroll = true;
            this.Add(GridView);
        }
    }
}
