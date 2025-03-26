/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;

//[ProvideProperty("ColumnSpan", typeof(Control))]
//[ProvideProperty("RowSpan", typeof(Control))]
//[ProvideProperty("Row", typeof(Control))]
//[ProvideProperty("Column", typeof(Control))]
//[ProvideProperty("CellPosition", typeof(Control))]
//[DefaultProperty("ColumnCount")]
[DesignerCategory("Component")]
public class TableLayoutPanel : ContainerControl, IExtenderProvider
{
    public readonly TableLayoutPanelBase self = new();
    public override object GtkControl => self;
    private readonly TableLayoutControlCollection _controls;
    private readonly TableLayoutColumnStyleCollection _columnStyles;
    private readonly TableLayoutRowStyleCollection _rowStyles;
    public Gtk.Grid grid => self.grid;
    public TableLayoutPanel()
    {
        _controls=new TableLayoutControlCollection(this);
        _columnStyles = new TableLayoutColumnStyleCollection();
        _rowStyles = new TableLayoutRowStyleCollection();
    }

    public override void PerformLayout()
    {
        SetColumnsStyles();
        SetRowsStyles();
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
    public new TableLayoutControlCollection Controls => _controls;

    private int _ColumnCount;
    [DefaultValue(0)]
    [Localizable(true)]
    public int ColumnCount
    {
        get => _ColumnCount;
        set
        {
            for (var r = 0; r < _RowCount; r++)
            {
                for (var c = 0; c < value; c++)
                {
                    if (grid.GetChildAt(c, r) == null)
                    {
                        grid.Attach(new Gtk.Viewport() { Vexpand = true, Hexpand = true, BorderWidth = 0, Valign = Gtk.Align.Fill, Halign = Gtk.Align.Fill }, c, r, 1, 1);
                    }
                }
            }
            for (var c = value; c < _ColumnCount; c++)
            {
                grid.RemoveColumn(value);
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
    [DefaultValue(0)]
    [Localizable(true)]
    public int RowCount
    {
        get => _RowCount;
        set
        {
            for (var r = 0; r < value; r++)
            {
                for (var c = 0; c < _ColumnCount; c++)
                {
                    if (grid.GetChildAt(c, r) == null)
                    {
                        grid.Attach(new Gtk.Viewport() { Vexpand = true, Hexpand = true, BorderWidth = 0, Valign = Gtk.Align.Fill, Halign = Gtk.Align.Fill }, c, r, 1, 1);
                    }
                }
            }
            for (var r = value; r < _RowCount; r++)
            {
                grid.RemoveRow(value);
            }
            _RowCount = value;
            SetRowsStyles();
        }
    }

    [DisplayName("Rows")]
    [Browsable(false)]
    public TableLayoutRowStyleCollection RowStyles => _rowStyles;

    [DisplayName("Columns")]
    [Browsable(false)]
    public TableLayoutColumnStyleCollection ColumnStyles => _columnStyles;

    public override Size Size { 
        get => base.Size;
        set {
            base.Size = value;
            SetColumnsStyles();
            SetRowsStyles();
        }
    }
    private void SetColumnsStyles()
    {
        var size = Size;
        var cidx = 0;
        foreach (ColumnStyle cs in ColumnStyles)
        {
            if (cidx < ColumnCount)
            {
                if (cs.SizeType == SizeType.Absolute)
                {
                    for (var r = 0; r < RowCount; r++)
                        grid.GetChildAt(cidx, r).WidthRequest = Convert.ToInt32(cs.Width);
                }
                else if (cs.SizeType == SizeType.Percent)
                {
                    for (var r = 0; r < RowCount; r++)
                    {
                        grid.GetChildAt(cidx, r).WidthRequest = Convert.ToInt32(size.Width * cs.Width * 0.01);
                    }
                }
                cidx++;
            }
        }
    }
    private void SetRowsStyles()
    {
        var size = Size;
        var ridx = 0;
        foreach (RowStyle rs in RowStyles)
        {
            if (ridx < RowCount)
            {
                if (rs.SizeType == SizeType.Absolute)
                {
                    Console.WriteLine(ColumnCount);
                    Console.WriteLine(Convert.ToInt32(rs.Height));
                    for (var c = 0; c < ColumnCount; c++)
                        grid.GetChildAt(c, ridx).HeightRequest = Convert.ToInt32(rs.Height);
                }
                else if (rs.SizeType == SizeType.Percent)
                {
                    for (var c = 0; c < ColumnCount; c++)
                        grid.GetChildAt(c, ridx).HeightRequest = Convert.ToInt32(size.Height * rs.Height * 0.01);
                }
                ridx++;
            }
        }
    }
    bool IExtenderProvider.CanExtend(object obj)
    {
        throw new NotImplementedException();
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
        return ((Gtk.Grid.GridChild)grid[control.Widget.Parent]).TopAttach;
    }

    public void SetRow(Control control, int row)
    {
        ((Gtk.Grid.GridChild)grid[control.Widget.Parent]).TopAttach = row;
    }

    [DisplayName("Cell")]
    public TableLayoutPanelCellPosition GetCellPosition(Control control)
    {
        var cellPosition = new TableLayoutPanelCellPosition(-1, -1);
        if (grid[control.Widget.Parent] is Gtk.Grid.GridChild cell)
        {
            cellPosition.Column = cell.LeftAttach;
            cellPosition.Row = cell.TopAttach;
        }
        return cellPosition;
    }

    public void SetCellPosition(Control control, TableLayoutPanelCellPosition position)
    {
        if (grid[control.Widget.Parent] is Gtk.Grid.GridChild cell)
        {
            cell.LeftAttach = position.Column;
            cell.TopAttach = position.Row;
        }
    }

    [DefaultValue(-1)]
    [DisplayName("Column")]
    public int GetColumn(Control control)
    {
        return ((Gtk.Grid.GridChild)grid[control.Widget.Parent]).LeftAttach;
    }

    public void SetColumn(Control control, int column)
    {
        if (grid[control.Widget.Parent] is Gtk.Grid.GridChild child)
            child.LeftAttach = column;

    }

    public Control? GetControlFromPosition(int column, int row)
    {
        return grid.GetChildAt(column, row).Data["Control"] as Control;
    }

    public TableLayoutPanelCellPosition GetPositionFromControl(Control control)
    {
        var cellPosition = new TableLayoutPanelCellPosition(-1, -1);
        if (grid[control.Widget.Parent] is Gtk.Grid.GridChild cell)
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
        var list = new List<int>();
        if (RowCount > 0)
        {
            for (var c = 0; c < ColumnCount; c++)
                list.Add(grid.GetChildAt(c, 0).WidthRequest);
        }
        return list.ToArray();
    }

    [Browsable(false)]
    public int[] GetRowHeights()
    {
        var list = new List<int>();
        if (ColumnCount > 0)
        {
            for (var r = 0; r < RowCount; r++)
                list.Add(grid.GetChildAt(0, r).WidthRequest);
        }
        return list.ToArray();
    }
}