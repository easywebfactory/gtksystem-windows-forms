/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
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
    public TableLayoutPanel()
    {
        _controls=new TableLayoutControlCollection(this);
        _columnStyles = new TableLayoutColumnStyleCollection();
        _rowStyles = new TableLayoutRowStyleCollection();
    }

    public override void PerformLayout()
    {
        var size = Size;
        var ridx = 0;
        foreach (RowStyle rs in RowStyles)
        {
            if (rs.SizeType == SizeType.Absolute)
            {
                for (var c = 0; c < ColumnCount; c++)
                    self.GetChildAt(c, ridx).HeightRequest = Convert.ToInt32(rs.Height);
            }
            else if (rs.SizeType == SizeType.Percent)
            {
                for (var c = 0; c < ColumnCount; c++)
                    self.GetChildAt(c, ridx).HeightRequest = Convert.ToInt32(size.Height * rs.Height * 0.01);
            }
            ridx++;
        }
        var cidx = 0;
        foreach (ColumnStyle cs in ColumnStyles)
        {
            if (cs.SizeType == SizeType.Absolute)
            {
                for (var r = 0; r < RowCount; r++)
                    self.GetChildAt(cidx, r).WidthRequest = Convert.ToInt32(cs.Width);
            }
            else if (cs.SizeType == SizeType.Percent)
            {
                for (var r = 0; r < ColumnCount; r++)
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
    public new TableLayoutControlCollection Controls => _controls;

    private int columnCount;
    [DefaultValue(0)]
    [Localizable(true)]
    public int ColumnCount
    {
        get => columnCount;
        set
        {
            for (var r = 0; r < RowCount; r++)
            {
                for (var c = 0; c < value; c++)
                {
                    if (self.GetChildAt(c, r) == null)
                    {
                        self.Attach(new Gtk.Viewport { Vexpand = false, Hexpand = false }, c, r, 1, 1);
                    }
                }
            }
            for (var c = value; c < columnCount; c++)
            {
                self.RemoveColumn(value);
            }
            columnCount = value;
        }
    }

    public TableLayoutPanelGrowStyle GrowStyle
    {
        get;
        set;
    }
    private int rowCount;
    [DefaultValue(0)]
    [Localizable(true)]
    public int RowCount
    {
        get => rowCount;
        set
        {
            for (var r = 0; r < value; r++)
            {
                for (var c = 0; c < ColumnCount; c++)
                {
                    if (self.GetChildAt(c, r) == null)
                    {
                        self.Attach(new Gtk.Viewport { Vexpand = false, Hexpand = false }, c, r, 1, 1);
                    }
                }
            }
            for (var r = value; r < rowCount; r++)
            {
                self.RemoveRow(value);
            }
            rowCount = value;
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
            PerformLayout();
        }
    }
    bool IExtenderProvider.CanExtend(object? obj)
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
        return ((Gtk.Grid.GridChild)self[control.Widget.Parent]).TopAttach;
    }

    public void SetRow(Control control, int row)
    {
        ((Gtk.Grid.GridChild)self[control.Widget.Parent]).TopAttach = row;
    }

    [DisplayName("Cell")]
    public TableLayoutPanelCellPosition GetCellPosition(Control control)
    {
        var cellPosition = new TableLayoutPanelCellPosition(-1, -1);
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

    public Control? GetControlFromPosition(int column, int row)
    {
        return self.GetChildAt(column, row).Data["Control"] as Control;
    }

    public TableLayoutPanelCellPosition GetPositionFromControl(Control control)
    {
        var cellPosition = new TableLayoutPanelCellPosition(-1, -1);
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
        var list = new List<int>();
        if (RowCount > 0)
        {
            for (var c = 0; c < ColumnCount; c++)
                list.Add(self.GetChildAt(c, 0).WidthRequest);
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
                list.Add(self.GetChildAt(0, r).WidthRequest);
        }
        return list.ToArray();
    }
}