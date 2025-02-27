using System.ComponentModel;

namespace System.Windows.Forms;

public class DataGridViewColumnCollection : List<DataGridViewColumn>
{
    private readonly DataGridView? _owner;
    private readonly Gtk.TreeView? gridView;
    public DataGridViewColumnCollection(DataGridView? dataGridView)
    {
        _owner = dataGridView;
        gridView = dataGridView?.GridView;
    }

    public virtual DataGridViewColumn this[string columnName] { get { return Find(m => m.Name == columnName); } }

    protected DataGridView? DataGridView => _owner;

    [Obsolete("此事件未实现，gtksystem.windows.forms提供vip开发服务")]
    public event CollectionChangeEventHandler? CollectionChanged;

    public void Add(string columnName, string headerText)
    {
        var column = new DataGridViewColumn { Name = columnName, HeaderText = headerText };
        Add(column);
    }
    public new void Add(DataGridViewColumn column)
    {
        column.DataGridView = _owner;
        var _cellStyle = column.DefaultCellStyle;
        if (_owner?.ColumnHeadersDefaultCellStyle != null)
            _cellStyle = _owner.ColumnHeadersDefaultCellStyle;

        if (_cellStyle != null)
        {
            var header = (Gtk.Button)column.Button;
            var style = "";
            if (_cellStyle.BackColor.Name != "0")
            {
                var backcolor = $"rgba({_cellStyle.BackColor.R},{_cellStyle.BackColor.G},{_cellStyle.BackColor.B},{_cellStyle.BackColor.A})";
                style += $".columnheaderbackcolor{{background-color:{backcolor};}} ";
                header.StyleContext.AddClass("columnheaderbackcolor");
            }
            if (_cellStyle.ForeColor.Name != "0")
            {
                var forecolor = $"rgba({_cellStyle.ForeColor.R},{_cellStyle.ForeColor.G},{_cellStyle.ForeColor.B},{_cellStyle.ForeColor.A})";
                style += $".columnheaderforecolor{{color:{forecolor};}} ";
                header.StyleContext.AddClass("columnheaderforecolor");
            }
            if (style.Length > 9)
            {
                var css = new Gtk.CssProvider();
                css.LoadFromData(style);
                header.StyleContext.AddProvider(css, 800);
            }
            switch (_cellStyle.Alignment)
            {
                case DataGridViewContentAlignment.TopLeft:
                case DataGridViewContentAlignment.MiddleLeft:
                case DataGridViewContentAlignment.BottomLeft:
                    column.Alignment = 0f;
                    break;

                case DataGridViewContentAlignment.TopCenter:
                case DataGridViewContentAlignment.MiddleCenter:
                case DataGridViewContentAlignment.BottomCenter:
                    column.Alignment = 0.5f;
                    break;

                case DataGridViewContentAlignment.TopRight:
                case DataGridViewContentAlignment.MiddleRight:
                case DataGridViewContentAlignment.BottomRight:
                    column.Alignment = 1.0f;
                    break;

                default:
                    column.Alignment = 0f;
                    break;
            }
        }
        gridView?.AppendColumn(column);
        base.Add(column);
    }
    public new void AddRange(IEnumerable<DataGridViewColumn> columns)
    {
        foreach (var column in columns)
        {
            Add(column);
        }
    }
    public new void Clear()
    {
        base.Clear();
        if (gridView?.Columns != null)
        {
            foreach (var wik in gridView?.Columns!)
                gridView.RemoveColumn(wik);
        }
    }
    public void Invalidate()
    {
        if (_owner?.GridView?.Columns.Length > _owner?.store.NColumns)
        {
            var columnTypes = new CellValue[_owner.GridView.Columns.Length];
            _owner.store.Clear();
            _owner.store = new Gtk.TreeStore(Array.ConvertAll(columnTypes, _ => typeof(CellValue)));
            _owner.GridView.Model = _owner.store;
        }
        else if (_owner?.GridView?.Model == null)
        {
            if (_owner is { GridView: not null })
            {
                _owner.GridView.Model = _owner.store;
            }
        }
        if (_owner?.GridView?.Columns.Length <= _owner?.store.NColumns)
        {
            var idx = 0;
            foreach (var column in this)
            {
                column.Index = idx;
                column.DisplayIndex = column.Index;
                column.DataGridView = _owner;
                column.Clear();
                column.Renderer();
                _owner.store.SetSortFunc(idx, (m, t1, t2) =>
                {
                    _owner.store.GetSortColumnId(out var sortid, out _);
                    if (m.GetValue(t1, sortid) == null || m.GetValue(t2, sortid) == null)
                        return 0;
                    return String.Compare(m.GetValue(t1, sortid).ToString(), m.GetValue(t2, sortid).ToString(), StringComparison.Ordinal);
                });
                idx++;
            }
        }
    }
    public int GetColumnCount(DataGridViewElementStates includeFilter)
    {
        return FindAll(m => m.State == includeFilter).Count;
    }

    public int GetColumnsWidth(DataGridViewElementStates includeFilter)
    {
        var co = Find(m => m.State == includeFilter);
        return co?.Width ?? _owner?.RowHeadersWidth??0;
    }

    public DataGridViewColumn GetFirstColumn(DataGridViewElementStates includeFilter)
    {
        return Find(m => m.State == includeFilter);
    }

    public DataGridViewColumn GetFirstColumn(DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
    {
        return Find(m => m.State == includeFilter && m.State == excludeFilter);
    }

    public DataGridViewColumn GetLastColumn(DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
    {
        return FindLast(m => m.State == includeFilter && m.State == excludeFilter);
    }

    public DataGridViewColumn? GetNextColumn(DataGridViewColumn dataGridViewColumnStart, DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
    {
        var ix = FindIndex(m => m.Name == dataGridViewColumnStart.Name && m.State == includeFilter && m.State == excludeFilter);
        return ix < Count ? base[ix] : null;
    }

    protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
    {
        CollectionChanged?.Invoke(_owner, e);
    }
}