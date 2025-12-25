using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [ListBindable(false)]
    public class DataGridViewCellCollection : BaseCollection, IList, ICollection, IEnumerable
    {
        private ArrayList items = new ArrayList();
        protected override ArrayList List => items;
        private DataGridViewRow owner;
        public DataGridViewRow Row { set => owner = value; get => owner; }
        public bool IsFixedSize => false;

        object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DataGridViewCell this[int index]
        {
            get
            {
                if (items.Count > index && index >= 0)
                {
                    return (DataGridViewCell)items[index];
                }
                else if (owner.DataGridView != null)
                {
                    if (owner.DataGridView.Columns.Count > index && index >= 0)
                    {
                        for (int i = items.Count; i < owner.DataGridView.Columns.Count; i++)
                        {
                            var cell = owner.DataGridView.Columns[i].NewCell();
                            cell.ColumnIndex = i;
                            cell.OwningRow = owner;
                            items.Add(cell);
                        }
                        return (DataGridViewCell)items[index];
                    }
                    else
                        throw new IndexOutOfRangeException();
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (items.Count > index && index >= 0)
                {
                    items[index] = value;
                }
                else if (owner.DataGridView != null)
                {
                    if (owner.DataGridView.Columns.Count > index && index >= 0)
                    {
                        for (int i = items.Count; i < owner.DataGridView.Columns.Count; i++)
                        {
                            if (i == index)
                            {
                                value.ColumnIndex = i;
                                value.OwningRow = owner;
                                items.Add(value);
                            }
                            else
                            {
                                var cell = owner.DataGridView.Columns[i].NewCell();
                                cell.ColumnIndex = i;
                                cell.OwningRow = owner;
                                items.Add(cell);
                            }
                        }
                    }
                    else
                        throw new IndexOutOfRangeException();
                }
            }
        }
        public DataGridViewCell this[string columnName]
        {
            get
            {
                DataGridViewColumn dataGridViewColumn = null;
                if (owner.DataGridView != null)
                {
                    dataGridViewColumn = owner.DataGridView.Columns[columnName];
                }

                if (dataGridViewColumn == null)
                {
                    throw new ArgumentException("DataGridViewColumnCollection_ColumnNotFound", "columnName");
                }

                return (DataGridViewCell)this[dataGridViewColumn.Index];
            }
            set
            {
                DataGridViewColumn dataGridViewColumn = null;
                if (owner.DataGridView != null)
                {
                    dataGridViewColumn = owner.DataGridView.Columns[columnName];
                }

                if (dataGridViewColumn == null)
                {
                    throw new ArgumentException("DataGridViewColumnCollection_ColumnNotFound", "columnName");
                }

                this[dataGridViewColumn.Index] = value;
            }
        }
        public DataGridViewCellCollection(DataGridViewRow dataGridViewRow)
        {
            owner = dataGridViewRow;
        }
        void IList.Clear()
        {
            Clear();
        }
        bool IList.Contains(object value)
        {
            return items.Contains(value);
        }
        int IList.IndexOf(object value)
        {
            return items.IndexOf(value);
        }
        int IList.Add(object value)
        {
            return Add((DataGridViewCell)value);
        }
        void IList.Insert(int index, object value)
        {
            Insert(index, (DataGridViewCell)value);
        }
        void IList.Remove(object value)
        {
            Remove((DataGridViewCell)value);
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
        public virtual int Add(DataGridViewCell dataGridViewCell)
        {
            dataGridViewCell.ColumnIndex = items.Count;
            dataGridViewCell.OwningRow = owner;
            return items.Add(dataGridViewCell);
        }

        public virtual void AddRange(params DataGridViewCell[] dataGridViewCells)
        {
            AddRangeInternal(dataGridViewCells);
        }
        public void AddRange(IEnumerable<DataGridViewCell> dataGridViewCells)
        {
            AddRangeInternal(dataGridViewCells);
        }
        private void AddRangeInternal(IEnumerable<DataGridViewCell> dataGridViewCells)
        {
            int idx = items.Count;
            foreach (DataGridViewCell dataGridViewCell in dataGridViewCells)
            {
                dataGridViewCell.ColumnIndex = idx;
                dataGridViewCell.OwningRow = owner;
                items.Add(dataGridViewCell);
                idx++;
            }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public virtual void Clear()
        {
            foreach (DataGridViewCell item in items)
            {
                item.OwningRow = null;
            }

            items.Clear();
        }
        public void CopyTo(DataGridViewCell[] array, int index)
        {
            items.CopyTo(array, index);
        }
        internal List<TOutput> ConvertAll<TOutput>(Converter<DataGridViewCell, TOutput> converter)
        {
            List <TOutput> data = new List<TOutput>();
            foreach (DataGridViewCell cell in items)
                data.Add(converter(cell));
            return data;
        }
        public virtual bool Contains(DataGridViewCell dataGridViewCell)
        {
            int num = items.IndexOf(dataGridViewCell);
            return num != -1;
        }
        public int IndexOf(DataGridViewCell dataGridViewCell)
        {
            return items.IndexOf(dataGridViewCell);
        }

        public virtual void Insert(int index, DataGridViewCell dataGridViewCell)
        {
            if (dataGridViewCell.OwningRow != null)
            {
                throw new InvalidOperationException("DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow");
            }
            dataGridViewCell.ColumnIndex = index;
            dataGridViewCell.OwningRow = owner;
            items.Insert(index, dataGridViewCell);
            for (int i = index; i < items.Count; i++)
                ((DataGridViewCell)items[i]).ColumnIndex = i;
        }

        public virtual void Remove(DataGridViewCell cell)
        {
            int num = -1;
            int count = items.Count;
            for (int i = 0; i < count; i++)
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
            DataGridViewCell dataGridViewCell = (DataGridViewCell)items[index];
            dataGridViewCell.OwningRow = null;
            items.RemoveAt(index);
        }
    }
}