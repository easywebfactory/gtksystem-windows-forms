using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TableLayoutPanelBase : Gtk.Grid, IControlGtk
    {
        public GtkControlOverride Override { set; get; }

        public TableLayoutPanelBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("TableLayoutPanel");
            this.RowHomogeneous = false;
            this.ColumnHomogeneous = false;
            this.BaselineRow = 0;
            this.ColumnSpacing = 0;
            this.RowSpacing = 0;
            this.Halign = Gtk.Align.Start;
            this.Valign = Gtk.Align.Start;
            this.BorderWidth = 0;
            this.Vexpand = false;
            this.Hexpand = false;
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
