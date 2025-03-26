﻿/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

[ListBindable(false)]
public class DataGridViewRowCollection : IList
{
    private readonly ArrayList items = new();
    private readonly DataGridView? dataGridView;
    private readonly DataGridViewRow? parentRow;

    public DataGridViewRowCollection(DataGridView? dataGridView)
    {
        this.dataGridView = dataGridView;
    }

    public DataGridViewRowCollection(DataGridView? dataGridView, DataGridViewRow? parentRow)
    {
        this.dataGridView = dataGridView;
        this.parentRow = parentRow;
    }

    private TreeIter AddGtkStore(DataGridViewCellCollection cells)
    {
        var ncolumns = dataGridView?.Store.NColumns ?? 0;
        var columnscount = dataGridView?.Columns.Count ?? 0;
        for (var i = cells.Count; i < columnscount; i++)
            cells.Add(dataGridView!.Columns[i].NewCell());
        for (var i = cells.Count; i < ncolumns; i++)
            cells.Add(new DataGridViewTextBoxCell());
        for (var i = 0; i < ncolumns; i++)
            cells[i].ColumnIndex = i;

        if (ncolumns == cells.Count)
            return dataGridView!.Store.AppendValues(cells.ConvertAll(o => o).ToArray<object>());
        return dataGridView!.Store.AppendValues(cells.ConvertAll(o => o).Take(ncolumns).ToArray<object>());
    }

    private TreeIter AddGtkStore(TreeIter parent, DataGridViewCellCollection cells)
    {
        var ncolumns = dataGridView?.Store.NColumns ?? 0;
        var columnscount = dataGridView?.Columns.Count ?? 0;
        for (var i = cells.Count; i < columnscount; i++)
            cells.Add(dataGridView!.Columns[i].NewCell());
        for (var i = cells.Count; i < ncolumns; i++)
            cells.Add(new DataGridViewTextBoxCell());
        for (var i = 0; i < ncolumns; i++)
            cells[i].ColumnIndex = i;
        if (ncolumns == cells.Count)
            return dataGridView!.Store.AppendValues(parent, cells.ConvertAll(o => o).ToArray<object>());
        return dataGridView!.Store.AppendValues(parent, cells.ConvertAll(o => o).Take(ncolumns).ToArray<object>());
    }

    private void AddGtkStore(params DataGridViewRow[] dataGridViewRows)
    {
        var rowindex = items.Count;
        foreach (var row in dataGridViewRows)
        {
            row.DataGridView = dataGridView;
            row.Index = rowindex;
            var _cellStyle = dataGridView?.DefaultCellStyle;
            if (dataGridView?.RowsDefaultCellStyle != null)
                _cellStyle = dataGridView.RowsDefaultCellStyle;
            if (rowindex % 2 != 0 && dataGridView?.AlternatingRowsDefaultCellStyle != null)
                _cellStyle = dataGridView.AlternatingRowsDefaultCellStyle;
            if (row.DefaultCellStyle != null)
                _cellStyle = row.DefaultCellStyle;

            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.RowIndex = rowindex;
                cell.RowStyle = _cellStyle;
            }

            items.Add(row);
            rowindex++;
            if (row.TreeIter.Equals(TreeIter.Zero))
            {
                row.TreeIter = AddGtkStore(row.Cells);
                foreach (DataGridViewRow child in row.Children)
                {
                    items.Add(child);
                    AddGtkStore(row.TreeIter, ref rowindex, child);
                }
            }
            else
            {
                var piter = AddGtkStore(row.TreeIter, row.Cells);
                foreach (DataGridViewRow child in row.Children)
                {
                    items.Add(child);
                    AddGtkStore(piter, ref rowindex, child);
                }
            }
        }
    }

    private void AddGtkStore(TreeIter parent, ref int rowindex, DataGridViewRow row)
    {
        row.Index = rowindex;
        row.DataGridView = dataGridView;
        var _cellStyle = dataGridView?.DefaultCellStyle;
        if (dataGridView?.RowsDefaultCellStyle != null)
            _cellStyle = dataGridView.RowsDefaultCellStyle;
        if (rowindex % 2 != 0 && dataGridView?.AlternatingRowsDefaultCellStyle != null)
            _cellStyle = dataGridView.AlternatingRowsDefaultCellStyle;
        if (row.DefaultCellStyle != null)
            _cellStyle = row.DefaultCellStyle;

        foreach (DataGridViewCell cell in row.Cells)
            cell.RowStyle = _cellStyle;

        items.Add(row);
        row.TreeIter = AddGtkStore(parent, row.Cells);
        rowindex++;
        foreach (DataGridViewRow child in row.Children)
        {
            items.Add(child);
            AddGtkStore(row.TreeIter, ref rowindex, child);
        }
    }

    private TreeIter InsertGtkStore(int rowIndex, DataGridViewCellCollection cells)
    {
        var ncolumns = dataGridView?.Store.NColumns;
        var columnscount = dataGridView?.Columns.Count;
        for (var i = cells.Count; i < columnscount; i++)
            cells.Add(dataGridView?.Columns[i].NewCell());
        for (var i = cells.Count; i < ncolumns; i++)
            cells.Add(new DataGridViewTextBoxCell());
        for (var i = 0; i < ncolumns; i++)
            cells[i].ColumnIndex = i;
        if (ncolumns == cells.Count)
            return dataGridView!.Store.InsertWithValues(rowIndex, cells.ConvertAll(o => o).ToArray<object>());
        return dataGridView!.Store.InsertWithValues(rowIndex, cells.ConvertAll(o => o).Take(ncolumns ?? 0).ToArray<object>());
    }

    private void InsertGtkStore(int rowIndex, params DataGridViewRow[] dataGridViewRows)
    {
        var idx = rowIndex;
        foreach (var row in dataGridViewRows)
        {
            var _cellStyle = dataGridView?.DefaultCellStyle;
            if (dataGridView?.RowsDefaultCellStyle != null)
                _cellStyle = dataGridView.RowsDefaultCellStyle;
            if (idx % 2 != 0 && dataGridView?.AlternatingRowsDefaultCellStyle != null)
                _cellStyle = dataGridView.AlternatingRowsDefaultCellStyle;
            if (row.DefaultCellStyle != null)
                _cellStyle = row.DefaultCellStyle;

            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.RowIndex = idx;
                cell.RowStyle = _cellStyle;
            }

            items.Insert(idx, row);
            row.TreeIter = InsertGtkStore(idx, row.Cells);
            idx++;
        }

        for (var i = idx; i < items.Count; i++)
        {
            ((DataGridViewRow)items[i]).Index = idx;
        }
    }


    public DataGridViewRow this[int index]
    {
        get
        {
            var dataGridViewRow = SharedRow(index);
            return dataGridViewRow;
        }
    }

    protected DataGridView? DataGridView => dataGridView;

    internal ArrayList SharedList => items;

    protected ArrayList List => items;

    public int Count => items.Count;
    bool IList.IsFixedSize => false;

    bool IList.IsReadOnly => false;


    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => this;

    object IList.this[int index]
    {
        get => items[index];
        set => throw new NotSupportedException();
    }

    public event CollectionChangeEventHandler? CollectionChanged;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int Add()
    {
        var row = new DataGridViewRow();
        AddGtkStore(row);
        return row.Index;
    }

    public virtual int Add(DataGridViewRow dataGridViewRow)
    {
        var rowindex = items.Count;
        if (parentRow == null)
            AddGtkStore(dataGridViewRow);
        else
            AddGtkStore(parentRow.TreeIter, ref rowindex, dataGridViewRow);

        return dataGridViewRow.Index;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int Add(params object?[] values)
    {
        var newRow = new DataGridViewRow();
        var cols = dataGridView?.Columns;
        var valueslength = values.Length;
        for (var i = 0; i < cols?.Count; i++)
        {
            if (i >= valueslength)
                newRow.Cells.Add(dataGridView?.Columns[i].NewCell());
            else if (values[i] is DataGridViewCell cellval)
                newRow.Cells.Add(cellval);
            else
                newRow.Cells.Add(dataGridView?.Columns[i].NewCell(values[i]));
        }

        return Add(newRow);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int Add(int count)
    {
        var start = items.Count;
        for (var i = 0; i < count; i++)
        {
            var row = new DataGridViewRow() { Index = start + i };
            AddGtkStore(row);
            items.Add(row);
        }

        return start + count;
    }

    public virtual int AddCopies(int indexSource, int count)
    {
        var arr = new DataGridViewRow[count];
        items.CopyTo(0, arr, indexSource, count);
        AddRange(arr);
        return count;
    }

    public virtual int AddCopy(int indexSource)
    {
        return AddCopies(indexSource, 1);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual void AddRange(params DataGridViewRow[] dataGridViewRows)
    {
        if (dataGridView?.Store.NColumns < dataGridView?.GridView?.Columns.Length)
            dataGridView.Columns.Invalidate();
        if (parentRow == null)
            AddGtkStore(dataGridViewRows);
        else
        {
            var rowindex = items.Count;
            foreach (var row in dataGridViewRows)
            {
                AddGtkStore(parentRow.TreeIter, ref rowindex, row);
            }
        }
    }

    public virtual void Clear()
    {
        dataGridView?.Store.Clear();
        items.Clear();
    }

    public virtual bool Contains(DataGridViewRow dataGridViewRow)
    {
        return items.IndexOf(dataGridViewRow) != -1;
    }

    public void CopyTo(DataGridViewRow[] array, int index)
    {
        items.CopyTo(array, index);
    }

    public int GetFirstRow(DataGridViewElementStates includeFilter)
    {
        var idx = -1;
        for (var i = 0; i < items.Count; i++)
        {
            if (((DataGridViewRow)items[i]).State == includeFilter)
            {
                idx = i;
                break;
            }
        }

        return idx;
    }

    public int GetFirstRow(DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
    {
        var idx = -1;
        for (var i = 0; i < items.Count; i++)
        {
            if (((DataGridViewRow)items[i]).State == includeFilter ||
                ((DataGridViewRow)items[i]).State == excludeFilter)
            {
                idx = i;
                break;
            }
        }

        return idx;
    }

    public int GetLastRow(DataGridViewElementStates includeFilter)
    {
        var idx = -1;
        for (var i = 0; i < items.Count; i++)
        {
            if (((DataGridViewRow)items[i]).State == includeFilter)
            {
                idx = i;
            }
        }

        return idx;
    }

    public int GetNextRow(int indexStart, DataGridViewElementStates includeFilter)
    {
        var idx = -1;
        for (var i = indexStart - 1; i < items.Count; i++)
        {
            if (((DataGridViewRow)items[i]).State == includeFilter)
            {
                idx = i;
                break;
            }
        }

        return idx;
    }

    public int GetNextRow(int indexStart, DataGridViewElementStates includeFilter,
        DataGridViewElementStates excludeFilter)
    {
        var idx = -1;
        for (var i = indexStart - 1; i < items.Count; i++)
        {
            if (((DataGridViewRow)items[i]).State == includeFilter ||
                ((DataGridViewRow)items[i]).State == excludeFilter)
            {
                idx = i;
                break;
            }
        }

        return idx;
    }

    public int GetPreviousRow(int indexStart, DataGridViewElementStates includeFilter,
        DataGridViewElementStates excludeFilter)
    {
        var idx = -1;
        for (var i = indexStart - 1; i > -1; i--)
        {
            if (((DataGridViewRow)items[i]).State == includeFilter ||
                ((DataGridViewRow)items[i]).State == excludeFilter)
            {
                idx = i;
                break;
            }
        }

        return idx;
    }

    public int GetPreviousRow(int indexStart, DataGridViewElementStates includeFilter)
    {
        var idx = -1;
        for (var i = indexStart - 1; i > -1; i--)
        {
            if (((DataGridViewRow)items[i]).State == includeFilter)
            {
                idx = i;
                break;
            }
        }

        return idx;
    }

    public int GetRowCount(DataGridViewElementStates includeFilter)
    {

        if (includeFilter == DataGridViewElementStates.None)
            return items.Count;
        var num = 0;
        for (var i = 0; i < items.Count; i++)
        {
            if ((GetRowState(i) & includeFilter) == includeFilter)
            {
                num++;
            }
        }

        return num;
    }

    public int GetRowsHeight(DataGridViewElementStates includeFilter)
    {
        var num = 0;
        for (var i = 0; i < items.Count; i++)
        {
            if ((GetRowState(i) & includeFilter) == includeFilter)
            {
                num += ((DataGridViewRow)items[i]).GetHeight(i);
            }
        }

        return num;
    }

    public virtual DataGridViewElementStates GetRowState(int rowIndex)
    {
        return ((DataGridViewRow)items[rowIndex]).State;
    }

    public int IndexOf(DataGridViewRow dataGridViewRow)
    {
        return items.IndexOf(dataGridViewRow);
    }

    public virtual void Insert(int rowIndex, params object?[] values)
    {
        var newRow = new DataGridViewRow();
        var cols = dataGridView?.Columns;
        var valueslength = values.Length;
        for (var i = 0; i < cols?.Count; i++)
        {
            if (i >= valueslength)
                newRow.Cells.Add(dataGridView?.Columns[i].NewCell());
            else if (values[i] is DataGridViewCell cellval)
                newRow.Cells.Add(cellval);
            else
                newRow.Cells.Add(dataGridView?.Columns[i].NewCell(values[i]));
        }

        InsertGtkStore(rowIndex, newRow);
    }

    public virtual void Insert(int rowIndex, DataGridViewRow dataGridViewRow)
    {
        InsertGtkStore(rowIndex, dataGridViewRow);
    }

    public virtual void Insert(int rowIndex, int count)
    {
        var rows = new DataGridViewRow[count];
        for (var r = 0; r < count; r++)
        {
            if (dataGridView != null)
            {
                foreach (var col in dataGridView.Columns)
                {
                    var cell = col.CellTemplate;
                    rows[r].Cells.Add(cell);
                }
            }
        }

        InsertRange(rowIndex, rows);
    }

    public virtual void InsertCopies(int indexSource, int indexDestination, int count)
    {
    }

    public virtual void InsertCopy(int indexSource, int indexDestination)
    {
    }

    public virtual void InsertRange(int rowIndex, params DataGridViewRow[] dataGridViewRows)
    {
        InsertGtkStore(rowIndex, dataGridViewRows);
    }

    public virtual void Remove(DataGridViewRow dataGridViewRow)
    {
        RemoveAt(dataGridViewRow.Index);
    }

    public virtual void RemoveAt(int index)
    {
        if (dataGridView?.Store.GetIter(out var iter, new TreePath(new int[] { index })) ?? false)
        {
            dataGridView.Store.Remove(ref iter);
        }

        items.RemoveAt(index);
        //reset id
        for (var i = 0; i < items.Count; i++)
        {
            ((DataGridViewRow)items[i]).Index = i;
        }
    }

    public DataGridViewRow SharedRow(int rowIndex)
    {
        if (dataGridView != null)
        {
            var hasiter = dataGridView.Store.GetIter(out var iter, new TreePath([rowIndex]));
            if (hasiter)
            {
                foreach (DataGridViewRow item in items)
                {
                    if (item.TreeIter.Equals(iter.UserData))
                        return item;
                }
            }
        }

        return (DataGridViewRow)SharedList[rowIndex];
    }

    //**************************************
    protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
    {
        if (CollectionChanged != null)
            CollectionChanged(dataGridView, e);
    }

    int IList.Add(object? value)
    {
        if (value is DataGridViewRow dataGridViewRow)
        {
            return Add(dataGridViewRow);
        }

        return -1;
    }

    void IList.Clear()
    {
        Clear();
    }

    bool IList.Contains(object? value)
    {
        return items.Contains(value);
    }

    int IList.IndexOf(object? value)
    {
        return items.IndexOf(value);
    }

    void IList.Insert(int index, object value)
    {
        Insert(index, (DataGridViewRow)value);
    }

    void IList.Remove(object? value)
    {
        if (value is DataGridViewRow dataGridViewRow)
        {
            Remove(dataGridViewRow);
        }
    }

    void IList.RemoveAt(int index)
    {
        RemoveAt(index);
    }

    void ICollection.CopyTo(Array array, int index)
    {
        items.CopyTo(array, index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return items.GetEnumerator();
    }

}