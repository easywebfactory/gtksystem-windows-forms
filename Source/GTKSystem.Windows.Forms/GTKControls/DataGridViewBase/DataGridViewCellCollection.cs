﻿using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

[ListBindable(false)]
public class DataGridViewCellCollection : BaseCollection, IList
{
    private readonly ArrayList items = new();
    protected override ArrayList List => items;
    private DataGridViewRow? owner;

    public DataGridViewRow? Row
    {
        set => owner = value;
        get => owner;
    }

    public bool IsFixedSize => false;

    object IList.this[int index]
    {
        get => this[index];
        set
        {
            if (value is DataGridViewCell dataGridViewCell)
            {
                this[index] = dataGridViewCell;
            }
        }
    }

    public DataGridViewCell this[int index]
    {
        get => (DataGridViewCell)items[index];
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            items[index] = value;
        }
    }

    public DataGridViewCell this[string columnName]
    {
        get
        {
            DataGridViewColumn? dataGridViewColumn = null;
            if (owner?.DataGridView != null)
            {
                dataGridViewColumn = owner.DataGridView.Columns[columnName];
            }

            if (dataGridViewColumn == null)
            {
                throw new ArgumentException(@"DataGridViewColumnCollection_ColumnNotFound", "columnName");
            }

            return (DataGridViewCell)items[dataGridViewColumn.Index];
        }
        set
        {
            DataGridViewColumn? dataGridViewColumn = null;
            if (owner?.DataGridView != null)
            {
                dataGridViewColumn = owner.DataGridView.Columns[columnName];
            }

            if (dataGridViewColumn == null)
            {
                throw new ArgumentException(@"DataGridViewColumnCollection_ColumnNotFound", "columnName");
            }

            this[dataGridViewColumn.Index] = value;
        }
    }

    public DataGridViewCellCollection(DataGridViewRow? dataGridViewRow)
    {
        owner = dataGridViewRow;
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

    int IList.Add(object? value)
    {
        if (value is DataGridViewCell dataGridViewCell)
        {
            return Add(dataGridViewCell);
        }

        return -1;
    }

    void IList.Insert(int index, object value)
    {
        Insert(index, (DataGridViewCell)value);
    }

    void IList.Remove(object? value)
    {
        if (value is DataGridViewCell dataGridViewCell)
        {
            var gridViewCell = (DataGridViewCell?)value;
            if (gridViewCell != null)
            {
                Remove(gridViewCell);
            }
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

    public override IEnumerator GetEnumerator()
    {
        return items.GetEnumerator();
    }

    //*****************
    public virtual int Add(DataGridViewCell? dataGridViewCell)
    {
        if (owner?.DataGridView != null)
        {
            throw new InvalidOperationException("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView");
        }

        if (dataGridViewCell?.OwningRow != null)
        {
            throw new InvalidOperationException("DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow");
        }

        if (dataGridViewCell != null)
        {
            dataGridViewCell.OwningRowInternal = owner;
            return items.Add(dataGridViewCell);
        }

        return -1;
    }

    public virtual void AddRange(params DataGridViewCell[] dataGridViewCells)
    {
        if (dataGridViewCells == null)
        {
            throw new ArgumentNullException("dataGridViewCells");
        }

        items.AddRange(dataGridViewCells);
        foreach (var dataGridViewCell2 in dataGridViewCells)
        {
            dataGridViewCell2.OwningRowInternal = owner;
        }
    }

    public void AddRange(IEnumerable<DataGridViewCell> dataGridViewCells)
    {
        if (dataGridViewCells == null)
        {
            throw new ArgumentNullException("dataGridViewCells");
        }

        foreach (var dataGridViewCell2 in dataGridViewCells)
        {
            dataGridViewCell2.OwningRowInternal = owner;
            items.Add(dataGridViewCell2);
        }
    }

    public override int Count => items.Count;

    public virtual void Clear()
    {
        if (owner?.DataGridView != null)
        {
            throw new InvalidOperationException("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView");
        }

        foreach (DataGridViewCell item in items)
        {
            item.OwningRowInternal = null;
        }

        items.Clear();
    }

    public void CopyTo(DataGridViewCell[] array, int index)
    {
        items.CopyTo(array, index);
    }

    internal List<TOutput> ConvertAll<TOutput>(Converter<DataGridViewCell, TOutput> converter)
    {
        List<TOutput> data = [];
        foreach (DataGridViewCell cell in items)
            data.Add(converter(cell));
        return data;
    }

    public virtual bool Contains(DataGridViewCell dataGridViewCell)
    {
        var num = items.IndexOf(dataGridViewCell);
        return num != -1;
    }

    public int IndexOf(DataGridViewCell dataGridViewCell)
    {
        return items.IndexOf(dataGridViewCell);
    }

    public virtual void Insert(int index, DataGridViewCell dataGridViewCell)
    {
        if (owner?.DataGridView != null)
        {
            throw new InvalidOperationException("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView");
        }

        if (dataGridViewCell.OwningRow != null)
        {
            throw new InvalidOperationException("DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow");
        }

        items.Insert(index, dataGridViewCell);
        dataGridViewCell.OwningRowInternal = owner;
    }

    public virtual void Remove(DataGridViewCell cell)
    {
        if (owner?.DataGridView != null)
        {
            throw new InvalidOperationException("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView");
        }

        var num = -1;
        var count = items.Count;
        for (var i = 0; i < count; i++)
        {
            if (items[i] == cell)
            {
                num = i;
                break;
            }
        }

        if (num == -1)
        {
            throw new ArgumentException("DataGridViewCellCollection_CellNotFound");
        }

        RemoveAt(num);
    }

    public virtual void RemoveAt(int index)
    {
        if (owner?.DataGridView != null)
        {
            throw new InvalidOperationException("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView");
        }

        var dataGridViewCell = (DataGridViewCell)items[index];
        dataGridViewCell.OwningRowInternal = null;
        items.RemoveAt(index);
    }
}