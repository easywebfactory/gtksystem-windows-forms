namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TableLayoutPanelBase : Gtk.Grid, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public TableLayoutPanelBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("TableLayoutPanel");
            this.RowHomogeneous = false;
            this.ColumnHomogeneous = false;
            this.BorderWidth = 1;
            this.BaselineRow = 0;
            this.ColumnSpacing = 0;
            this.RowSpacing = 0;
            base.Halign = Gtk.Align.Start;
            base.Valign = Gtk.Align.Start;
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
