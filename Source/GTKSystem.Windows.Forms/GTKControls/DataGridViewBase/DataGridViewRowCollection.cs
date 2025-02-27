/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using System.Collections;
using System.ComponentModel;


namespace System.Windows.Forms
{
    [ListBindable(false)]
    public class DataGridViewRowCollection : ICollection, IEnumerable, IList
    {
        private ArrayList items = new ArrayList();
        private DataGridView dataGridView;
        private DataGridViewRow parentRow;
        public DataGridViewRowCollection(DataGridView dataGridView)
        {
            this.dataGridView = dataGridView;
        }
        public DataGridViewRowCollection(DataGridView dataGridView, DataGridViewRow parentRow)
        {
            this.dataGridView = dataGridView;
            this.parentRow = parentRow;
        }
        private TreeIter AddGtkStore(DataGridViewCellCollection cells)
        {
            int ncolumns = dataGridView.Store.NColumns;
            int columnscount = dataGridView.Columns.Count;
            for (int i = cells.Count; i < columnscount; i++)
                cells.Add(dataGridView.Columns[i].NewCell());
            for (int i = cells.Count; i < ncolumns; i++)
                cells.Add(new DataGridViewTextBoxCell());
            for (int i = 0; i < ncolumns; i++)
                cells[i].ColumnIndex = i;
            
            if (ncolumns == cells.Count)
                return dataGridView.Store.AppendValues(cells.ConvertAll(o => o).ToArray());
            else
                return dataGridView.Store.AppendValues(cells.ConvertAll(o => o).Take(ncolumns).ToArray());
        }
        private TreeIter AddGtkStore(TreeIter parent, DataGridViewCellCollection cells)
        {
            int ncolumns = dataGridView.Store.NColumns;
            int columnscount = dataGridView.Columns.Count;
            for (int i = cells.Count; i < columnscount; i++)
                cells.Add(dataGridView.Columns[i].NewCell());
            for (int i = cells.Count; i < ncolumns; i++)
                cells.Add(new DataGridViewTextBoxCell());
            for (int i = 0; i < ncolumns; i++)
                cells[i].ColumnIndex = i;
            if (ncolumns == cells.Count)
                return dataGridView.Store.AppendValues(parent, cells.ConvertAll(o => o).ToArray());
            else
                return dataGridView.Store.AppendValues(parent, cells.ConvertAll(o => o).Take(ncolumns).ToArray());
        }
        private void AddGtkStore(params DataGridViewRow[] dataGridViewRows)
        {
            int rowindex = items.Count;
            foreach (DataGridViewRow row in dataGridViewRows)
            {
                row.DataGridView = dataGridView;
                row.Index = rowindex;
                DataGridViewCellStyle _cellStyle = dataGridView.DefaultCellStyle;
                if (dataGridView.RowsDefaultCellStyle != null)
                    _cellStyle = dataGridView.RowsDefaultCellStyle;
                if (rowindex % 2 != 0 && dataGridView.AlternatingRowsDefaultCellStyle != null)
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
                    Gtk.TreeIter piter = AddGtkStore(row.TreeIter, row.Cells);
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
            DataGridViewCellStyle _cellStyle = dataGridView.DefaultCellStyle;
            if (dataGridView.RowsDefaultCellStyle != null)
                _cellStyle = dataGridView.RowsDefaultCellStyle;
            if (rowindex % 2 != 0 && dataGridView.AlternatingRowsDefaultCellStyle != null)
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
            int ncolumns = dataGridView.Store.NColumns;
            int columnscount = dataGridView.Columns.Count;
            for (int i = cells.Count; i < columnscount; i++)
                cells.Add(dataGridView.Columns[i].NewCell());
            for (int i = cells.Count; i < ncolumns; i++)
                cells.Add(new DataGridViewTextBoxCell());
            for (int i = 0; i < ncolumns; i++)
                cells[i].ColumnIndex = i;
            if (ncolumns == cells.Count)
                return dataGridView.Store.InsertWithValues(rowIndex, cells.ConvertAll(o => o).ToArray());
            else
                return dataGridView.Store.InsertWithValues(rowIndex, cells.ConvertAll(o => o).Take(ncolumns).ToArray());
        }
        private void InsertGtkStore(int rowIndex, params DataGridViewRow[] dataGridViewRows)
        {
            int idx = rowIndex;
            foreach (DataGridViewRow row in dataGridViewRows)
            {
                DataGridViewCellStyle _cellStyle = dataGridView.DefaultCellStyle;
                if (dataGridView.RowsDefaultCellStyle != null)
                    _cellStyle = dataGridView.RowsDefaultCellStyle;
                if (idx % 2 != 0 && dataGridView.AlternatingRowsDefaultCellStyle != null)
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
            int newindex = idx;
            for (int i = idx; i < items.Count; i++)
            {
                ((DataGridViewRow)items[i]).Index = idx;
            }
        }


        public DataGridViewRow this[int index]
        {
            get
            {
                DataGridViewRow dataGridViewRow = SharedRow(index);
                return dataGridViewRow;
            }
        }
        protected DataGridView DataGridView { get { return dataGridView; } }
        internal ArrayList SharedList => items;
        protected ArrayList List { get { return items; } }
        public int Count => items.Count;
        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => false;


        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        object IList.this[int index] { get => items[index]; set => throw new NotSupportedException(); }
        
        public event CollectionChangeEventHandler CollectionChanged;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int Add()
        {
            DataGridViewRow row = new DataGridViewRow();
            AddGtkStore(row);
            return row.Index;
        }
        public virtual int Add(DataGridViewRow dataGridViewRow)
        {
            int rowindex = items.Count;
            if (parentRow == null)
                AddGtkStore(dataGridViewRow);
            else
                AddGtkStore(parentRow.TreeIter, ref rowindex, dataGridViewRow);

            return dataGridViewRow.Index;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int Add(params object[] values)
        {
            DataGridViewRow newRow = new DataGridViewRow();
            var cols = dataGridView.Columns;
            int valueslength = values.Length;
            for (int i = 0; i < cols.Count; i++)
            {
                if (i >= valueslength)
                    newRow.Cells.Add(dataGridView.Columns[i].NewCell());
                else if (values[i] is DataGridViewCell cellval)
                    newRow.Cells.Add(cellval);
                else
                    newRow.Cells.Add(dataGridView.Columns[i].NewCell(values[i]));
            }
            return Add(newRow);
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int Add(int count)
        {
            int start = items.Count;
            for (int i = 0; i < count; i++)
            {
                DataGridViewRow row = new DataGridViewRow() { Index = start + i };
                AddGtkStore(row);
                items.Add(row);
            }

            return start + count;
        }
        public virtual int AddCopies(int indexSource, int count)
        {
            DataGridViewRow[] arr = new DataGridViewRow[count];
            items.CopyTo(0, arr, indexSource, count);
            this.AddRange(arr);
            return count;
        }
        public virtual int AddCopy(int indexSource)
        {
            return AddCopies(indexSource, 1);
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual void AddRange(params DataGridViewRow[] dataGridViewRows)
        {
            if (this.dataGridView.Store.NColumns < this.dataGridView.GridView.Columns.Length)
                this.dataGridView.Columns.Invalidate();
            if (parentRow == null)
                AddGtkStore(dataGridViewRows);
            else
            {
                int rowindex = items.Count;
                foreach (DataGridViewRow row in dataGridViewRows)
                {
                    AddGtkStore(parentRow.TreeIter, ref rowindex, row);
                }
            }
        }
        public virtual void Clear()
        {
            dataGridView.Store.Clear();
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
            int idx = -1;
            for (int i = 0; i < items.Count; i++)
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
            int idx = -1;
            for (int i = 0; i < items.Count; i++)
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
            int idx = -1;
            for (int i = 0; i < items.Count; i++)
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
            int idx = -1;
            for (int i = indexStart - 1; i < items.Count; i++)
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
            int idx = -1;
            for (int i = indexStart - 1; i < items.Count; i++)
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
            int idx = -1;
            for (int i = indexStart - 1; i > -1; i--)
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
            int idx = -1;
            for (int i = indexStart-1; i > -1; i--)
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
            else
            {
                int num = 0;
                for (int i = 0; i < items.Count; i++)
                {
                    if ((GetRowState(i) & includeFilter) == includeFilter)
                    {
                        num++;
                    }
                }
                return num;
            }
        }
        public int GetRowsHeight(DataGridViewElementStates includeFilter)
        {
            int num = 0;
            for (int i = 0; i < items.Count; i++)
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
            DataGridViewRow newRow = new DataGridViewRow();
            var cols = dataGridView.Columns;
            int valueslength = values.Length;
            for (int i = 0; i < cols.Count; i++)
            {
                if (i >= valueslength)
                    newRow.Cells.Add(dataGridView.Columns[i].NewCell());
                else if (values[i] is DataGridViewCell cellval)
                    newRow.Cells.Add(cellval);
                else
                    newRow.Cells.Add(dataGridView.Columns[i].NewCell(values[i]));
            }

            InsertGtkStore(rowIndex, newRow);
        }
        public virtual void Insert(int rowIndex, DataGridViewRow dataGridViewRow)
        {
            InsertGtkStore(rowIndex, dataGridViewRow);
        }

        public virtual void Insert(int rowIndex, int count) {
            DataGridViewRow[] rows = new DataGridViewRow[count];
            for (int r = 0; r < count; r++)
            {
                foreach (var col in dataGridView.Columns)
                {
                    var cell = col.CellTemplate;
                    rows[r].Cells.Add(cell);
                }
            }
            InsertRange(rowIndex, rows);
        }

        public virtual void InsertCopies(int indexSource, int indexDestination, int count) { }

        public virtual void InsertCopy(int indexSource, int indexDestination) { }
        public virtual void InsertRange(int rowIndex, params DataGridViewRow[] dataGridViewRows)
        {
            InsertGtkStore(rowIndex, dataGridViewRows);
        }
        public virtual void Remove(DataGridViewRow dataGridViewRow) {
            RemoveAt(dataGridViewRow.Index);
        }
        public virtual void RemoveAt(int index) {
            if (dataGridView.Store.GetIter(out TreeIter iter, new TreePath(new int[] { index })))
            {
                dataGridView.Store.Remove(ref iter);
            }
            items.RemoveAt(index);
            //reset id
            for (int i = 0; i < items.Count; i++)
            {
                ((DataGridViewRow)items[i]).Index = i;
            }
        }
        public DataGridViewRow SharedRow(int rowIndex)
        {
            bool hasiter = dataGridView.Store.GetIter(out TreeIter iter,new TreePath(new int[] { rowIndex }));
            if (hasiter)
            {
                foreach (DataGridViewRow item in items)
                {
                    if(item.TreeIter.Equals(iter.UserData))
                        return item;
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

        int IList.Add(object value)
        {
            return Add((DataGridViewRow)value);
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

        void IList.Insert(int index, object value)
        {
            Insert(index, (DataGridViewRow)value);
        }

        void IList.Remove(object value)
        {
            Remove((DataGridViewRow)value);
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
}