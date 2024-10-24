/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections.Generic;
using System.ComponentModel;
using static System.Windows.Forms.TableLayoutControlCollection;

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
			return _controls.GetCellControl(column, row) as Control;
		}

        public TableLayoutPanelCellPosition GetPositionFromControl(Control control)
        {
            foreach (TableLayoutControllCell item in _controls)
            {
                if (item.Control.Equals(control))
                    return item.TableLayoutPanelCellPosition;
            }
            return default(TableLayoutPanelCellPosition);
        }

        [Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int[] GetColumnWidths()
		{
			List<int> list = new List<int>();
			float gridWidth = self.WidthRequest;
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
                        if (self.GetChildAt(col, row) != null)
                            colMaxWidth = Math.Max(colMaxWidth, self.GetChildAt(col, row).AllocatedWidth);
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
            float gridHeight = self.HeightRequest;
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
						if (self.GetChildAt(col, row)!=null)
						{
							colMaxHeight = Math.Max(colMaxHeight, self.GetChildAt(col, row).AllocatedHeight);
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
