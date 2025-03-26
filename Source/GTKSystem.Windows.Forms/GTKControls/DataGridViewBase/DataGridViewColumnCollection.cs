/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using System.ComponentModel;

namespace System.Windows.Forms;

public class DataGridViewColumnCollection : List<DataGridViewColumn>
{
    public event CollectionChangeEventHandler? CollectionChanged;
    private readonly DataGridView? owner;
    private readonly Gtk.TreeView? GridView;
    public DataGridViewColumnCollection(DataGridView? dataGridView)
    {
        owner = dataGridView;
        GridView = dataGridView?.GridView;
    }

    public virtual DataGridViewColumn this[string columnName] { get { return Find(m => m.Name == columnName); } }

    protected DataGridView? DataGridView => owner;

    public void Add(string columnName, string headerText)
    {
        var column = new DataGridViewColumn() { Name = columnName, HeaderText = headerText };
        Add(column);
    }
    public new void Add(DataGridViewColumn column)
    {
        column.DataGridView = owner;
        column.Index = Count;
        column.DisplayIndex = column.Index;
        column.SetGridViewDefaultStyle(owner?.DefaultCellStyle);

        var _cellStyle = column.DefaultCellStyle;
        if (owner?.ColumnHeadersDefaultCellStyle != null)
            _cellStyle = owner.ColumnHeadersDefaultCellStyle;

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

        base.Add(column);
        if (owner?.self.IsRealized??false)
        {
            Invalidate();
        }
        GridView?.AppendColumn(column);
        OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, column));
    }
    public new void AddRange(IEnumerable<DataGridViewColumn> columns)
    {
        foreach (var column in columns)
        {
            Add(column);
        }
    }
    public new bool Remove(DataGridViewColumn column)
    {
        if (owner != null)
        {
            owner.GridView?.RemoveColumn(column);
        }

        var ir= base.Remove(column);
        OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, column));
        return ir;
    }
    public new void RemoveAt(int index)
    {
        Remove(this[index]);
    }
    public new void Clear()
    {
        base.Clear();
        if (GridView != null)
        {
            foreach (var wik in GridView.Columns)
                GridView.RemoveColumn(wik);
        }

        OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, owner));
    }
    public void Invalidate()
    {
        if (Count > owner?.Store.NColumns)
        {
            var columnTypes = new object[Count];
            owner.Store.Clear();
            owner.Store = new Gtk.TreeStore(Array.ConvertAll(columnTypes, _ => typeof(DataGridViewCell)));
            if (owner.GridView != null)
            {
                owner.GridView.Model = owner.Store;
            }
        }
        else if (owner?.GridView?.Model == null)
        {
            if (owner?.GridView != null)
            {
                owner.GridView.Model = owner.Store;
            }

            if (owner != null)
            {
                owner.Store.DefaultSortFunc =
                    (_, _, _) => { return 0; };
                for (var i = 0; i < owner.Store.NColumns; i++)
                {
                    owner.Store.SetSortFunc(i, (m, t1, t2) =>
                    {
                        ((Gtk.TreeStore)m).GetSortColumnId(out var sortid, out _);
                        var v1 = m.GetValue(t1, sortid) as DataGridViewCell;
                        var v2 = m.GetValue(t2, sortid) as DataGridViewCell;
                        if (v1?.Value == null || v2?.Value == null)
                            return 0;
                        if (int.TryParse(v1.Value.ToString(), out var rv1) &&
                            int.TryParse(v2.Value.ToString(), out var rv2))
                            return (rv2 - rv1);
                        if (DateTime.TryParse(v1.Value.ToString(), out var rd1) &&
                            DateTime.TryParse(v2.Value.ToString(), out var rd2))
                            return (int)((rd2 - rd1).TotalSeconds);
                        return string.Compare(v2.Value.ToString(), v1.Value.ToString(), StringComparison.Ordinal);
                    });
                }
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
        return co?.Width ?? owner?.RowHeadersWidth??0;
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
        if (CollectionChanged != null)
            CollectionChanged(owner, e);
    }
}