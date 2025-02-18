namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TableLayoutPanelBase : ScrollableBoxBase, IControlGtk
    {
        public Gtk.Grid grid = new Gtk.Grid();
        public TableLayoutPanelBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("TableLayoutPanel");
            grid.RowHomogeneous = false;
            grid.ColumnHomogeneous = false;
            grid.BaselineRow = 0;
            grid.ColumnSpacing = 0;
            grid.RowSpacing = 0;
            grid.BorderWidth = 0;
            grid.Halign = Gtk.Align.Fill;
            grid.Valign = Gtk.Align.Fill;
            this.BorderWidth = 0;
            this.Vexpand = false;
            this.Hexpand = false;
            this.HscrollbarPolicy = Gtk.PolicyType.External;
            this.VscrollbarPolicy = Gtk.PolicyType.External;

            this.Add(this.grid);
        }
        public void AddClass(string cssClass)
        {
            this.Override.AddClass(cssClass);
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
