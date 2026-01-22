using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TableLayoutPanelBase : Gtk.Grid, IControlGtk
    {
        public GtkControlOverride Override { set; get; }

        public TableLayoutPanelBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("TableLayoutPanel");
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
    }
}
