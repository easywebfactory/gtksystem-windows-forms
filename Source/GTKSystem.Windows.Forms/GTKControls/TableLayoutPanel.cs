using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	[ProvideProperty("ColumnSpan", typeof(Control))]
	[ProvideProperty("RowSpan", typeof(Control))]
	[ProvideProperty("Row", typeof(Control))]
	[ProvideProperty("Column", typeof(Control))]
	[ProvideProperty("CellPosition", typeof(Control))]
	[DefaultProperty("ColumnCount")]
	public class TableLayoutPanel : WidgetContainerControl<Gtk.Grid>, IExtenderProvider
	{
		public override LayoutEngine LayoutEngine
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TableLayoutSettings LayoutSettings
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Localizable(true)]
		public new BorderStyle BorderStyle
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

 
		[Localizable(true)]
		public TableLayoutPanelCellBorderStyle CellBorderStyle
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public new TableLayoutControlCollection Controls
		{
			get
			{
				throw null;
			}
		}

		[DefaultValue(0)]
		[Localizable(true)]
		public int ColumnCount
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		public TableLayoutPanelGrowStyle GrowStyle
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[DefaultValue(0)]
		[Localizable(true)]
		public int RowCount
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[DisplayName("Rows")]
		[MergableProperty(false)]
		[Browsable(false)]
		public TableLayoutRowStyleCollection RowStyles
		{
			get
			{
				throw null;
			}
		}


		[DisplayName("Columns")]
		[Browsable(false)]
		[MergableProperty(false)]
		public TableLayoutColumnStyleCollection ColumnStyles
		{
			get
			{
				throw null;
			}
		}

		public event TableLayoutCellPaintEventHandler? CellPaint
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		public TableLayoutPanel()
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected ControlCollection CreateControlsInstance()
		{
			throw null;
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
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DisplayName("Row")]
		public int GetRow(Control control)
		{
			throw null;
		}

		public void SetRow(Control control, int row)
		{
			throw null;
		}


		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DisplayName("Cell")]
		public TableLayoutPanelCellPosition GetCellPosition(Control control)
		{
			throw null;
		}

		public void SetCellPosition(Control control, TableLayoutPanelCellPosition position)
		{
			throw null;
		}

		[DefaultValue(-1)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DisplayName("Column")]
		public int GetColumn(Control control)
		{
			throw null;
		}

		public void SetColumn(Control control, int column)
		{
			throw null;
		}

		public Control? GetControlFromPosition(int column, int row)
		{
			throw null;
		}

		public TableLayoutPanelCellPosition GetPositionFromControl(Control? control)
		{
			throw null;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int[] GetColumnWidths()
		{
			throw null;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int[] GetRowHeights()
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void OnLayout(LayoutEventArgs levent)
		{
			throw null;
		}

		protected virtual void OnCellPaint(TableLayoutCellPaintEventArgs e)
		{
			throw null;
		}

		protected void OnPaintBackground(PaintEventArgs e)
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void ScaleCore(float dx, float dy)
		{
			throw null;
		}

		protected void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			throw null;
		}
	}
}
