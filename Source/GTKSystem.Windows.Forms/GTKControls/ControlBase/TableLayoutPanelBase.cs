using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TableLayoutPanelBase : Gtk.Grid
    {
        public TableLayoutPanelBase() : base()
        {
            this.RowHomogeneous = false;
            this.ColumnHomogeneous = false;
            this.BaselineRow = 0;
            this.ColumnSpacing = 0;
            this.RowSpacing = 0;
            this.BorderWidth = 0;
        }
    }
}
