/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{
	[ProvideProperty("ColumnSpan", typeof(Control))]
	[ProvideProperty("RowSpan", typeof(Control))]
	[ProvideProperty("Row", typeof(Control))]
	[ProvideProperty("Column", typeof(Control))]
	[ProvideProperty("CellPosition", typeof(Control))]
	[DefaultProperty("ColumnCount")]
    [DesignerCategory("Component")]
    public partial class TableLayoutPanel : WidgetContainerControl<Gtk.Grid>, IExtenderProvider
	{
		private TableLayoutControlCollection _controls;
		private TableLayoutColumnStyleCollection _columnStyles;
		private TableLayoutRowStyleCollection _rowStyles;
        public TableLayoutPanel():base()
        {
            Widget.StyleContext.AddClass("TableLayoutPanel");
            _controls=new TableLayoutControlCollection(this);
			_columnStyles = new TableLayoutColumnStyleCollection();
			_rowStyles = new TableLayoutRowStyleCollection();

			base.Control.RowHomogeneous = false;
			base.Control.ColumnHomogeneous= false;
			base.Control.BorderWidth = 1;
            base.Control.BaselineRow = 0;
            base.Control.ColumnSpacing = 0;
			base.Control.RowSpacing = 0;
            base.Control.Realized += Control_Realized;
        }

        public override void PerformLayout()
        {
            
        }
        private void Control_Realized(object sender, EventArgs e)
        {
            int[] colsWidth = GetColumnWidths();
            int[] rowsHeight = GetRowHeights();
            int colLeft = 0;
            int rowTop = 0;
            for (int col = 0; col< ColumnCount; col++)
            {
                rowTop = 0;
                colLeft += colsWidth[col];
                for (int row = 0; row < ColumnCount; row++)
                {
                    rowTop += rowsHeight[row];
                    Gtk.Viewport viewport = new Gtk.Viewport();
                    viewport.BorderWidth = 0;
                    if (CellBorderStyle == TableLayoutPanelCellBorderStyle.None)
						viewport.ShadowType = Gtk.ShadowType.None;
                    else if (CellBorderStyle == TableLayoutPanelCellBorderStyle.Single || CellBorderStyle == TableLayoutPanelCellBorderStyle.Inset)
                        viewport.ShadowType = Gtk.ShadowType.In;
                    else if (CellBorderStyle == TableLayoutPanelCellBorderStyle.Outset)
                        viewport.ShadowType = Gtk.ShadowType.Out;
                    else if (CellBorderStyle == TableLayoutPanelCellBorderStyle.InsetDouble)
					{
						viewport.ShadowType = Gtk.ShadowType.In;
                        viewport.BorderWidth = 1;
                    }
                    else if(CellBorderStyle == TableLayoutPanelCellBorderStyle.OutsetDouble)
                    {
                        viewport.ShadowType = Gtk.ShadowType.Out;
                        viewport.BorderWidth = 1;
					}
					else
					{
                        viewport.ShadowType = Gtk.ShadowType.In;
                    }

                    viewport.Margin = 0;
                    viewport.MarginStart = 0;
                    viewport.MarginTop = 0;
                    viewport.WidthRequest = colsWidth[col];
                    viewport.HeightRequest = rowsHeight[row];
                    Gtk.Layout layout = new Gtk.Layout(new Adjustment(0, 0, 0, 1, 0, 0), new Adjustment(0, 0, 0, 1, 0, 0));
                    layout.Vexpand = true;
                    layout.Hexpand = true;
                    viewport.Add(layout);
                    if (_controls.GridControls.ContainsKey($"{col},{row}"))
                    {
                        _controls.GridControls[$"{col},{row}"].Widget.MarginStart = 3;
                        _controls.GridControls[$"{col},{row}"].Widget.MarginTop = 3;

                        layout.Add(_controls.GridControls[$"{col},{row}"].Widget);
                    }

                    Control.Attach(viewport, colLeft, rowTop, 1, 1);
                }
            }
			Control.ShowAll();
        }

        [Browsable(false)]
		[Localizable(true)]
		public BorderStyle BorderStyle
		{
			get;
			set;
		}

 
		[Localizable(true)]
		public TableLayoutPanelCellBorderStyle CellBorderStyle
		{
            get;
            set;
        }

		[Browsable(false)]
		public override TableLayoutControlCollection Controls
		{
			get => _controls;
        }

		[DefaultValue(0)]
		[Localizable(true)]
		public int ColumnCount
		{
			get;
			set;
        }

		public TableLayoutPanelGrowStyle GrowStyle
		{
            get;
            set;
        }

		[DefaultValue(0)]
		[Localizable(true)]
		public int RowCount
		{
			get;
			set;
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

		bool IExtenderProvider.CanExtend(object obj)
		{
			throw null;
		}

		[DefaultValue(1)]
		[DisplayName("ColumnSpan")]
		public int GetColumnSpan(Control control)
		{
			throw null;
		}

		public void SetColumnSpan(Control control, int value)
		{
			throw null;
		}

		[DefaultValue(1)]
		[DisplayName("RowSpan")]
		public int GetRowSpan(Control control)
		{
			throw null;
		}

		public void SetRowSpan(Control control, int value)
		{
			throw null;
		}

		[DefaultValue(-1)]
		[DisplayName("Row")]
		public int GetRow(Control control)
		{
			throw null;
		}

		public void SetRow(Control control, int row)
		{
			
		}

		[DisplayName("Cell")]
		public TableLayoutPanelCellPosition GetCellPosition(Control control)
		{
			throw null;
		}

		public void SetCellPosition(Control control, TableLayoutPanelCellPosition position)
		{
			
		}

		[DefaultValue(-1)]
		[DisplayName("Column")]
		public int GetColumn(Control control)
		{
			throw null;
		}

		public void SetColumn(Control control, int column)
		{
			
        }

		public Control GetControlFromPosition(int column, int row)
		{
			throw null;
		}

		public TableLayoutPanelCellPosition GetPositionFromControl(Control control)
		{
			throw null;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int[] GetColumnWidths()
		{
			List<int> list = new List<int>();
			float gridWidth = this.Width;
			float absoluteWidth = 0;
			
			int col= 0;
            foreach (ColumnStyle style in ColumnStyles)
			{
				if(style.SizeType==SizeType.Absolute)
				{
					absoluteWidth += style.Width;
                }
            }
			float lastGridWidth = gridWidth - absoluteWidth;
            foreach (ColumnStyle style in ColumnStyles)
            {
                if (style.SizeType == SizeType.Absolute)
                {
                    list.Add((int)style.Width);
                }
                else if (style.SizeType == SizeType.Percent)
                {
                    list.Add((int)(style.Width*0.01* lastGridWidth));
                }
                else if (style.SizeType == SizeType.AutoSize)
                {
                    int colMaxWidth = 0;
                    int row = 0;
                    foreach (RowStyle rowStyle in RowStyles)
                    {
                        if (base.Control.GetChildAt(col, row) != null)
                            colMaxWidth = Math.Max(colMaxWidth, base.Control.GetChildAt(col, row).AllocatedWidth);
                        row++;
                    }
                    list.Add(colMaxWidth);
                }
                col++;
            }

            return list.ToArray();
		}

		[Browsable(false)]
		public int[] GetRowHeights()
		{
            List<int> list = new List<int>();
            float gridHeight = this.Height;
            float absoluteHeight = 0;
            int row = 0;
            int col = 0;
            foreach (RowStyle style in RowStyles)
            {
                if (style.SizeType == SizeType.Absolute)
                {
                    absoluteHeight += style.Height;
                }
            }
            float lastGridHeight = gridHeight - absoluteHeight;
            foreach (RowStyle style in RowStyles)
            {
                if (style.SizeType == SizeType.Absolute)
                {
                    list.Add((int)style.Height);
                }
                else if (style.SizeType == SizeType.Percent)
                {
                    list.Add((int)(style.Height * 0.01 * lastGridHeight));
                }
                else if (style.SizeType == SizeType.AutoSize)
                {
                    int colMaxHeight = 0;
                    foreach (ColumnStyle colStyle in ColumnStyles)
                    {
						if (base.Control.GetChildAt(col, row)!=null)
						{
							colMaxHeight = Math.Max(colMaxHeight, base.Control.GetChildAt(col, row).AllocatedHeight);
						}
                        row++;
                    }
                    list.Add(colMaxHeight);
                }
                col++;
            }

            return list.ToArray();
        }
	}
}
