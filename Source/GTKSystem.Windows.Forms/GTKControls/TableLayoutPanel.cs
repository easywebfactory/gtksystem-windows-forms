/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
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
    public partial class TableLayoutPanel : Panel, IExtenderProvider
    {
        private TableLayoutControlCollection _controls;
		private TableLayoutColumnStyleCollection _columnStyles;
		private TableLayoutRowStyleCollection _rowStyles;
        public TableLayoutPanelBase layoutEngine = new TableLayoutPanelBase();
        public TableLayoutPanel():base("TableLayoutPanel")
        {
            self.Override.sender = this;
            layoutEngine.Halign = Gtk.Align.Fill;
            layoutEngine.Valign = Gtk.Align.Fill;
            layoutEngine.SizeAllocated += Self_SizeAllocated;
            layoutEngine.Mapped += Self_Mapped;
            _controls =new TableLayoutControlCollection(this);
			_columnStyles = new TableLayoutColumnStyleCollection();
			_rowStyles = new TableLayoutRowStyleCollection();           
            self.Remove(self.contaner);
            self.contaner.Destroy();
            self.Add(layoutEngine);
        }

        private bool Is_Self_Mapped;
        private void Self_Mapped(object sender, EventArgs e)
        {
            if (!Is_Self_Mapped)
            {
                Is_Self_Mapped = true;
                PerformLayout();
            }
        }
        private int allocationwidth = 0;
        private int allocationheight = 0;
        private void Self_SizeAllocated(object o, Gtk.SizeAllocatedArgs args)
        {
            if (args.Allocation.Width != allocationwidth || args.Allocation.Height != allocationheight)
            {
                PerformLayout();
                allocationwidth = args.Allocation.Width;
                allocationheight = args.Allocation.Height;
            }
        }

        public override void PerformLayout()
        {
            SetColumnsStyles();
            SetRowsStyles();
            self.QueueResize();
        }
        private TableLayoutPanelCellBorderStyle _CellBorderStyle;

        public TableLayoutPanelCellBorderStyle CellBorderStyle
		{
            get => _CellBorderStyle;
            set
            {
                _CellBorderStyle = value;
                if (value == TableLayoutPanelCellBorderStyle.None)
                    self.StyleContext.RemoveClass("TableCellBorder");
                else
                    self.StyleContext.AddClass("TableCellBorder");
            }
        }
		public new TableLayoutControlCollection Controls
		{
			get => _controls;
        }
		private int _ColumnCount;
		public int ColumnCount
		{
            get => _ColumnCount;
            set
            {
                for (int r = 0; r < _RowCount; r++)
                {
                    for (int c = 0; c < value; c++)
                    {
                        if (layoutEngine.GetChildAt(c, r) == null)
                        {
                            layoutEngine.Attach(new Gtk.Viewport() { Vexpand = true, Hexpand = true, BorderWidth = 0, Valign = Gtk.Align.Fill, Halign = Gtk.Align.Fill }, c, r, 1, 1);
                        }
                    }
                }
                for (int c = value; c < _ColumnCount; c++)
                {
                    layoutEngine.RemoveColumn(value);
                }
                _ColumnCount = value;
                SetColumnsStyles();
            }
        }

		public TableLayoutPanelGrowStyle GrowStyle
		{
            get;
            set;
        }
        private int _RowCount;
		public int RowCount
		{
            get => _RowCount;
            set
            {
                for (int r = 0; r < value; r++)
                {
                    for (int c = 0; c < _ColumnCount; c++)
                    {
                        if (layoutEngine.GetChildAt(c, r) == null)
                        {
                            layoutEngine.Attach(new Gtk.Viewport() { Vexpand = true, Hexpand = true, BorderWidth = 0, Valign = Gtk.Align.Fill, Halign = Gtk.Align.Fill }, c, r, 1, 1);
                        }
                    }
                }
                for (int r = value; r < _RowCount; r++)
                {
                    layoutEngine.RemoveRow(value);
                }
                _RowCount = value;
                SetRowsStyles();
            }
        }
		public TableLayoutRowStyleCollection RowStyles
		{
			get => _rowStyles;
        }
		public TableLayoutColumnStyleCollection ColumnStyles
		{
			get => _columnStyles;
        }
        private void SetColumnsStyles()
        {
            Size size = this.Size;
            int panelWidth = size.Width;
            int cidx = 0;
            foreach (ColumnStyle cs in ColumnStyles)
            {
                if (cidx < ColumnCount)
                {
                    if (cs.SizeType == SizeType.Absolute)
                    {
                        for (int r = 0; r < RowCount; r++)
                        {
                            layoutEngine.GetChildAt(cidx, r).WidthRequest = Convert.ToInt32(cs.Width);
                        }
                    }
                    else if (cs.SizeType == SizeType.Percent)
                    {
                        for (int r = 0; r < RowCount; r++)
                        {
                            layoutEngine.GetChildAt(cidx, r).WidthRequest = Convert.ToInt32(panelWidth * cs.Width * 0.01);
                        }
                    }
                    cidx++;
                }
            }
        }
        private void SetRowsStyles()
        {
            Size size = this.Size;
            int panelHeight = size.Height;
            int ridx = 0;
            foreach (RowStyle rs in RowStyles)
            {
                if (ridx < RowCount)
                {
                    if (rs.SizeType == SizeType.Absolute)
                    {
                        for (int c = 0; c < ColumnCount; c++)
                            layoutEngine.GetChildAt(c, ridx).HeightRequest = Convert.ToInt32(rs.Height);
                    }
                    else if (rs.SizeType == SizeType.Percent)
                    {
                        for (int c = 0; c < ColumnCount; c++)
                            layoutEngine.GetChildAt(c, ridx).HeightRequest = Convert.ToInt32(panelHeight * rs.Height * 0.01);
                    }
                    ridx++;
                }
            }
        }
        bool IExtenderProvider.CanExtend(object obj)
		{
			throw null;
		}
		public int GetColumnSpan(Control control)
		{
			return control.Widget.MarginStart;
		}

		public void SetColumnSpan(Control control, int value)
		{
            control.Widget.MarginStart = value;
            control.Widget.MarginEnd = value;
        }
		public int GetRowSpan(Control control)
		{
            return control.Widget.MarginTop;
        }

		public void SetRowSpan(Control control, int value)
		{
            control.Widget.MarginTop = value;
            control.Widget.MarginBottom = value;
        }
		public int GetRow(Control control)
		{
            return ((Gtk.Grid.GridChild)layoutEngine[control.Widget.Parent]).TopAttach;
        }

		public void SetRow(Control control, int row)
		{
            ((Gtk.Grid.GridChild)layoutEngine[control.Widget.Parent]).TopAttach = row;
        }
		public TableLayoutPanelCellPosition GetCellPosition(Control control)
		{
            TableLayoutPanelCellPosition cellPosition = new TableLayoutPanelCellPosition(-1, -1);
            if (layoutEngine[control.Widget.Parent] is Gtk.Grid.GridChild cell)
            {
                cellPosition.Column = cell.LeftAttach;
                cellPosition.Row = cell.TopAttach;
            }
            return cellPosition;
        }

		public void SetCellPosition(Control control, TableLayoutPanelCellPosition position)
		{
            if (layoutEngine[control.Widget.Parent] is Gtk.Grid.GridChild cell)
            {
                cell.LeftAttach = position.Column;
                cell.TopAttach = position.Row;
            }
        }
		public int GetColumn(Control control)
		{
            return ((Gtk.Grid.GridChild)layoutEngine[control.Widget.Parent]).LeftAttach;
        }

		public void SetColumn(Control control, int column)
		{
            if (layoutEngine[control.Widget.Parent] is Gtk.Grid.GridChild child)
                child.LeftAttach = column;

        }

		public Control GetControlFromPosition(int column, int row)
		{
            return layoutEngine.GetChildAt(column, row).Data["Control"] as Control;
		}

        public TableLayoutPanelCellPosition GetPositionFromControl(Control control)
        {
            TableLayoutPanelCellPosition cellPosition = new TableLayoutPanelCellPosition(-1, -1);
            if (layoutEngine[control.Widget.Parent] is Gtk.Grid.GridChild cell)
            {
                cellPosition.Column = cell.LeftAttach;
                cellPosition.Row = cell.TopAttach;
            }
            return cellPosition;
        }
		public int[] GetColumnWidths()
		{
            List<int> list = new List<int>();
            if (RowCount > 0)
            {
                for (int c = 0; c < ColumnCount; c++)
                    list.Add(layoutEngine.GetChildAt(c, 0).WidthRequest);
            }
            return list.ToArray();
		}
		public int[] GetRowHeights()
		{
            List<int> list = new List<int>();
            if (ColumnCount > 0)
            {
                for (int r = 0; r < RowCount; r++)
                    list.Add(layoutEngine.GetChildAt(0, r).HeightRequest);
            }
            return list.ToArray();
        }
	}
}
