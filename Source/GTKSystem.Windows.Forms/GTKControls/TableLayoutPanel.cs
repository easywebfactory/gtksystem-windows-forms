/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    //[ProvideProperty("ColumnSpan", typeof(Control))]
    //[ProvideProperty("RowSpan", typeof(Control))]
    //[ProvideProperty("Row", typeof(Control))]
    //[ProvideProperty("Column", typeof(Control))]
    //[ProvideProperty("CellPosition", typeof(Control))]
    //[DefaultProperty("ColumnCount")]
    [DesignerCategory("Component")]
    public partial class TableLayoutPanel : ContainerControl, IExtenderProvider
    {
        public readonly TableLayoutPanelBase self = new TableLayoutPanelBase();
        public override object GtkControl => self;
        private TableLayoutControlCollection _controls;
		private TableLayoutColumnStyleCollection _columnStyles;
		private TableLayoutRowStyleCollection _rowStyles;
        public TableLayoutPanel():base()
        {
            _controls=new TableLayoutControlCollection(this);
			_columnStyles = new TableLayoutColumnStyleCollection();
			_rowStyles = new TableLayoutRowStyleCollection();
        }

        public override void PerformLayout()
        {
            Size size = this.Size;
            int ridx = 0;
            foreach (RowStyle rs in RowStyles)
            {
                if (rs.SizeType == SizeType.Absolute)
                {
                    for (int c = 0; c < ColumnCount; c++)
                        self.GetChildAt(c, ridx).HeightRequest = Convert.ToInt32(rs.Height);
                }
                else if (rs.SizeType == SizeType.Percent)
                {
                    for (int c = 0; c < ColumnCount; c++)
                        self.GetChildAt(c, ridx).HeightRequest = Convert.ToInt32(size.Height * rs.Height * 0.01);
                }
                ridx++;
            }
            int cidx = 0;
            foreach (ColumnStyle cs in ColumnStyles)
            {
                if (cs.SizeType == SizeType.Absolute)
                {
                    for (int r = 0; r < RowCount; r++)
                        self.GetChildAt(cidx, r).WidthRequest = Convert.ToInt32(cs.Width);
                }
                else if (cs.SizeType == SizeType.Percent)
                {
                    for (int r = 0; r < ColumnCount; r++)
                    {
                        self.GetChildAt(cidx, r).WidthRequest = Convert.ToInt32(size.Width * cs.Width * 0.01);
                    }
                }
                cidx++;
            }
        }

        [Browsable(false)]
        public override BorderStyle BorderStyle { get => base.BorderStyle; set => base.BorderStyle = value; }



        [Localizable(true)]
		public TableLayoutPanelCellBorderStyle CellBorderStyle
		{
            get;
            set;
        }

		[Browsable(false)]
		public new TableLayoutControlCollection Controls
		{
			get => _controls;
        }
		private int _ColumnCount;
		[DefaultValue(0)]
		[Localizable(true)]
		public int ColumnCount
		{
            get => _ColumnCount;
            set
            {
                for (int r = 0; r < RowCount; r++)
                {
                    for (int c = 0; c < value; c++)
                    {
                        if (self.GetChildAt(c, r) == null)
                        {
                            self.Attach(new Gtk.Viewport() { Vexpand = false, Hexpand = false }, c, r, 1, 1);
                        }
                    }
                }
                for (int c = value; c < _ColumnCount; c++)
                {
                    self.RemoveColumn(value);
                }
                _ColumnCount = value;
            }
        }

		public TableLayoutPanelGrowStyle GrowStyle
		{
            get;
            set;
        }
        private int _RowCount;
        [DefaultValue(0)]
		[Localizable(true)]
		public int RowCount
		{
            get => _RowCount;
            set
            {
                for (int r = 0; r < value; r++)
                {
                    for (int c = 0; c < ColumnCount; c++)
                    {
                        if (self.GetChildAt(c, r) == null)
                        {
                            self.Attach(new Gtk.Viewport() { Vexpand = false, Hexpand = false }, c, r, 1, 1);
                        }
                    }
                }
                for (int r = value; r < _RowCount; r++)
                {
                    self.RemoveRow(value);
                }
                _RowCount = value;
            }
        }

		[DisplayName("Rows")]
		[Browsable(false)]
		public TableLayoutRowStyleCollection RowStyles
		{
			get => _rowStyles;
        }


		[DisplayName("Columns")]
		[Browsable(false)]
		public TableLayoutColumnStyleCollection ColumnStyles
		{
			get => _columnStyles;
        }
        public override Size Size { 
            get => base.Size;
            set {
                base.Size = value;
                PerformLayout();
            }
        }
        bool IExtenderProvider.CanExtend(object obj)
		{
			throw null;
		}

		[DefaultValue(1)]
		[DisplayName("ColumnSpan")]
		public int GetColumnSpan(Control control)
		{
			return control.Widget.MarginStart;
		}

		public void SetColumnSpan(Control control, int value)
		{
            control.Widget.MarginStart = value;
            control.Widget.MarginEnd = value;
        }

		[DefaultValue(1)]
		[DisplayName("RowSpan")]
		public int GetRowSpan(Control control)
		{
            return control.Widget.MarginTop;
        }

		public void SetRowSpan(Control control, int value)
		{
            control.Widget.MarginTop = value;
            control.Widget.MarginBottom = value;
        }

		[DefaultValue(-1)]
		[DisplayName("Row")]
		public int GetRow(Control control)
		{
            return ((Gtk.Grid.GridChild)self[control.Widget.Parent]).TopAttach;
        }

		public void SetRow(Control control, int row)
		{
            ((Gtk.Grid.GridChild)self[control.Widget.Parent]).TopAttach = row;
        }

		[DisplayName("Cell")]
		public TableLayoutPanelCellPosition GetCellPosition(Control control)
		{
            TableLayoutPanelCellPosition cellPosition = new TableLayoutPanelCellPosition(-1, -1);
            if (self[control.Widget.Parent] is Gtk.Grid.GridChild cell)
            {
                cellPosition.Column = cell.LeftAttach;
                cellPosition.Row = cell.TopAttach;
            }
            return cellPosition;
        }

		public void SetCellPosition(Control control, TableLayoutPanelCellPosition position)
		{
            if (self[control.Widget.Parent] is Gtk.Grid.GridChild cell)
            {
                cell.LeftAttach = position.Column;
                cell.TopAttach = position.Row;
            }
        }

		[DefaultValue(-1)]
		[DisplayName("Column")]
		public int GetColumn(Control control)
		{
            return ((Gtk.Grid.GridChild)self[control.Widget.Parent]).LeftAttach;
        }

		public void SetColumn(Control control, int column)
		{
            if (self[control.Widget.Parent] is Gtk.Grid.GridChild child)
                child.LeftAttach = column;

        }

		public Control GetControlFromPosition(int column, int row)
		{
            return self.GetChildAt(column, row).Data["Control"] as Control;
		}

        public TableLayoutPanelCellPosition GetPositionFromControl(Control control)
        {
            TableLayoutPanelCellPosition cellPosition = new TableLayoutPanelCellPosition(-1, -1);
            if (self[control.Widget.Parent] is Gtk.Grid.GridChild cell)
            {
                cellPosition.Column = cell.LeftAttach;
                cellPosition.Row = cell.TopAttach;
            }
            return cellPosition;
        }

        [Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int[] GetColumnWidths()
		{
            List<int> list = new List<int>();
            if (RowCount > 0)
            {
                for (int c = 0; c < ColumnCount; c++)
                    list.Add(self.GetChildAt(c, 0).WidthRequest);
            }
            return list.ToArray();
		}

		[Browsable(false)]
		public int[] GetRowHeights()
		{
            List<int> list = new List<int>();
            if (ColumnCount > 0)
            {
                for (int r = 0; r < RowCount; r++)
                    list.Add(self.GetChildAt(0, r).WidthRequest);
            }
            return list.ToArray();
        }
	}
}
