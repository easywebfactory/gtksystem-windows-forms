/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
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
    public DataGridViewRowCollection(DataGridView? dataGridView)
    {
        this.dataGridView = dataGridView;
    }

    public int Count => items.Count;

    private TreeIter? AddGtkStore(List<CellValue> values)
    {
        var columnscount = dataGridView?.store.NColumns??0;
        for (var i = values.Count; i < columnscount; i++)
            values.Add(new CellValue());
        var iter = dataGridView?.store.AppendValues(values.GetRange(0, columnscount).Cast<object>().ToArray() ?? []);
        return iter;
    }
    private TreeIter? AddGtkStore(TreeIter parent, List<CellValue> values)
    {
        var columnscount = dataGridView?.store.NColumns??0;
        for (var i = values.Count; i < columnscount; i++)
            values.Add(new CellValue());
        var iter = dataGridView?.store.AppendValues(parent, values.GetRange(0, columnscount).Cast<object>().ToArray());
        return iter;
    }
    private void AddGtkStore(params DataGridViewRow[] dataGridViewRows)
    {
        var rowindex = GetRowCount(DataGridViewElementStates.None);
        foreach (var row in dataGridViewRows)
        {
            row.DataGridView = dataGridView;
            row.Index = rowindex;
            var _cellStyle = row.DefaultCellStyle;
            if (dataGridView?.RowsDefaultCellStyle != null)
                _cellStyle = dataGridView.RowsDefaultCellStyle;
            if (rowindex % 2 != 0 && dataGridView?.AlternatingRowsDefaultCellStyle != null)
                _cellStyle = dataGridView.AlternatingRowsDefaultCellStyle;

            row.TreeIter = AddGtkStore(row.Cells.ConvertAll(c => new CellValue { Value = c.Value, Style = c.Style ?? _cellStyle }))??default;
            rowindex++;
            foreach (var child in row.Children)
            {
                items.Add(child);
                AddGtkStore(row.TreeIter, ref rowindex, child);
            }
        }
    }
    private void AddGtkStore(TreeIter parent, ref int rowindex, DataGridViewRow row)
    {
        row.Index = rowindex;
        row.DataGridView = dataGridView;
        var _cellStyle = row.DefaultCellStyle;
        if (dataGridView?.RowsDefaultCellStyle != null)
            _cellStyle = dataGridView.RowsDefaultCellStyle;
        if (rowindex % 2 != 0 && dataGridView?.AlternatingRowsDefaultCellStyle != null)
            _cellStyle = dataGridView.AlternatingRowsDefaultCellStyle;

        row.TreeIter = AddGtkStore(parent, row.Cells.ConvertAll(c => new CellValue { Value = c.Value, Style = c.Style ?? _cellStyle }))??default;
        rowindex++;
        foreach (var child in row.Children)
        {
            items.Add(child);
            AddGtkStore(row.TreeIter, ref rowindex, child);
        }

    }
    private TreeIter? InsertGtkStore(int rowIndex, List<CellValue> values)
    {
        var columnscount = dataGridView?.store.NColumns??0;
        for (var i = values.Count; i < columnscount; i++)
            values.Add(new CellValue());
        var iter = dataGridView?.store.InsertWithValues(rowIndex, values.GetRange(0, columnscount).Cast<object>().ToArray());
        return iter;
    }
    private void InsertGtkStore(int rowIndex, params DataGridViewRow[] dataGridViewRows)
    {
        var idx = rowIndex;
        foreach (var row in dataGridViewRows)
        {
            var rowindex = GetRowCount(DataGridViewElementStates.None);
            var _cellStyle = row.DefaultCellStyle;
            if (dataGridView?.RowsDefaultCellStyle != null)
                _cellStyle = dataGridView.RowsDefaultCellStyle;
            if (rowindex % 2 != 0 && dataGridView?.AlternatingRowsDefaultCellStyle != null)
                _cellStyle = dataGridView.AlternatingRowsDefaultCellStyle;

            row.TreeIter = InsertGtkStore(idx, row.Cells.ConvertAll(c => new CellValue { Value = c.Value, Style = c.Style ?? _cellStyle }))??default;
            idx++;
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

    bool IList.IsFixedSize => false;

    bool IList.IsReadOnly => false;

    int ICollection.Count => items.Count;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => this;

    object IList.this[int index] { get => items[index]; set => throw new NotSupportedException(); }
        
    public event CollectionChangeEventHandler? CollectionChanged;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int Add()
    {
        var row = new DataGridViewRow { Index = items.Count };
        AddGtkStore(row);
        items.Add(row);
        return 1;
    }
    public virtual int Add(DataGridViewRow dataGridViewRow)
    {
        AddGtkStore(dataGridViewRow);
        return items.Add(dataGridViewRow);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int Add(params object[] values)
    {
        var newRow = new DataGridViewRow
        {
            Index = items.Count
        };
        var cols = dataGridView?.Columns;
        var colleng = Math.Min(values.Length, cols?.Count??0);
        for (var i = 0; i < colleng; i++)
        {
            var cellvalue = values[i];
            var col = cols?[i];
            if (col is DataGridViewTextBoxColumn)
                newRow.Cells.Add(new DataGridViewTextBoxCell { Value = cellvalue });
            else if (col is DataGridViewImageColumn)
                newRow.Cells.Add(new DataGridViewImageCell { Value = cellvalue });
            else if (col is DataGridViewCheckBoxColumn)
                newRow.Cells.Add(new DataGridViewCheckBoxCell { Value = cellvalue });
            else if (col is DataGridViewButtonColumn)
                newRow.Cells.Add(new DataGridViewButtonCell { Value = cellvalue });
            else if (col is DataGridViewComboBoxColumn)
                newRow.Cells.Add(new DataGridViewComboBoxCell { Value = cellvalue });
            else if (col is DataGridViewLinkColumn)
                newRow.Cells.Add(new DataGridViewLinkCell { Value = cellvalue });
            else
                newRow.Cells.Add(new DataGridViewTextBoxCell { Value = cellvalue });
        }
        return Add(newRow);
    }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int Add(int count)
    {
        var start = items.Count;
        for (var i = 0; i < count; i++)
        {
            var row = new DataGridViewRow { Index = start + i };
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
        if (dataGridView?.store.NColumns < dataGridView?.GridView?.Columns.Length)
            dataGridView.Columns.Invalidate();

        AddGtkStore(dataGridViewRows);
        items.AddRange(dataGridViewRows);
    }
    public virtual void Clear()
    {
        dataGridView?.store.Clear();
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
            if (((DataGridViewRow)items[i]).State == includeFilter || ((DataGridViewRow)items[i]).State == excludeFilter)
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
    public int GetNextRow(int indexStart, DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
    {
        var idx = -1;
        for (var i = indexStart - 1; i < items.Count; i++)
        {
            if (((DataGridViewRow)items[i]).State == includeFilter || ((DataGridViewRow)items[i]).State == excludeFilter)
            {
                idx = i;
                break;
            }
        }
        return idx;
    }
    public int GetPreviousRow(int indexStart, DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
    {
        var idx = -1;
        for (var i = indexStart - 1; i > -1; i--)
        {
            if (((DataGridViewRow)items[i]).State == includeFilter || ((DataGridViewRow)items[i]).State == excludeFilter)
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
        for (var i = indexStart-1; i > -1; i--)
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
        //if (((uint)includeFilter & 0xFFFFFF90u) != 0)
        //{
        //    throw new ArgumentException("DataGridView_InvalidDataGridViewElementStateCombination", "includeFilter");
        //}
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
        //if (((uint)includeFilter & 0xFFFFFF90u) != 0)
        //{
        //    throw new ArgumentException("DataGridView_InvalidDataGridViewElementStateCombination", "includeFilter");
        //}
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
    public virtual void Insert(int rowIndex, params object[] values)
    {
        var row = new DataGridViewRow();
        foreach (var o in values)
            row.Cells.Add(new DataGridViewTextBoxCell { Value = o });
        InsertGtkStore(rowIndex, row);
        items.Insert(rowIndex, row);
    }
    public virtual void Insert(int rowIndex, DataGridViewRow dataGridViewRow)
    {
        InsertGtkStore(rowIndex, dataGridViewRow);
        items.Insert(rowIndex, dataGridViewRow);
    }

    public virtual void Insert(int rowIndex, int count) {
        var row = new DataGridViewRow();
        for (var i=0;i < DataGridView?.Columns.Count;i++)
            row.Cells.Add(new DataGridViewTextBoxCell());
        InsertGtkStore(rowIndex, row);
        items.Insert(rowIndex, row);
    }

    public virtual void InsertCopies(int indexSource, int indexDestination, int count) { }

    public virtual void InsertCopy(int indexSource, int indexDestination) { }
    public virtual void InsertRange(int rowIndex, params DataGridViewRow[] dataGridViewRows)
    {
        var i = rowIndex;
        foreach (var row in dataGridViewRows)
        {
            items.Insert(i, row);
            i++;
        }
        InsertGtkStore(rowIndex, dataGridViewRows);
    }
    public virtual void Remove(DataGridViewRow dataGridViewRow) {
        RemoveAt(dataGridViewRow.Index);
    }
    public virtual void RemoveAt(int index) {
        if (dataGridView?.store.GetIter(out var iter, new TreePath([index]))??false)
        {
            dataGridView.store.Remove(ref iter);
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
        TreeIter iter=default;
        var hasiter = dataGridView?.store.GetIter(out iter,new TreePath([rowIndex]))??false;
        if (hasiter)
        {
            foreach (DataGridViewRow item in items)
            {
                if(item.TreeIter.UserData.Equals(iter.UserData))
                    return item;
            }
        }
        return (DataGridViewRow)SharedList[rowIndex];
    }
    //**************************************
    protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
    {
        CollectionChanged?.Invoke(dataGridView, e);
    }

    int IList.Add(object? value)
    {
        var dataGridViewRow = value as DataGridViewRow;
        if (dataGridViewRow != null)
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
        var dataGridViewRow = value as DataGridViewRow;
        if (dataGridViewRow != null)
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