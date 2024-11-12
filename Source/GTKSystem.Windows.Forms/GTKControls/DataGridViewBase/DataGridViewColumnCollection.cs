using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms.GtkRender;

namespace System.Windows.Forms
{
    public class DataGridViewColumnCollection : List<DataGridViewColumn>
    {
        private DataGridView __owner;
        private Gtk.TreeView GridView;
        public DataGridViewColumnCollection(DataGridView dataGridView)
        {
            __owner = dataGridView;
            GridView = dataGridView.GridView;
        }

        public virtual DataGridViewColumn this[string columnName] { get { return base.Find(m => m.Name == columnName); } }

        protected DataGridView DataGridView { get { return __owner; } }
        [Obsolete("此事件未实现，gtksystem.windows.forms提供vip开发服务")]
        public event CollectionChangeEventHandler CollectionChanged;

        public void Add(string columnName, string headerText)
        {
            DataGridViewColumn column = new DataGridViewColumn() { Name = columnName, HeaderText = headerText };
            Add(column);
        }
        public new void Add(DataGridViewColumn column)
        {
            column.DataGridView = __owner;
            DataGridViewCellStyle _cellStyle = column.DefaultCellStyle;
            if (__owner.ColumnHeadersDefaultCellStyle != null)
                _cellStyle = __owner.ColumnHeadersDefaultCellStyle;

            if (_cellStyle != null)
            {
                Gtk.Button header = ((Gtk.Button)column.Button);
                string style = "";
                if (_cellStyle.BackColor.Name != "0")
                {
                    string backcolor = $"rgba({_cellStyle.BackColor.R},{_cellStyle.BackColor.G},{_cellStyle.BackColor.B},{_cellStyle.BackColor.A})";
                    style += $".columnheaderbackcolor{{background-color:{backcolor};}} ";
                    header.StyleContext.AddClass("columnheaderbackcolor");
                }
                if (_cellStyle.ForeColor.Name != "0")
                {
                    string forecolor = $"rgba({_cellStyle.ForeColor.R},{_cellStyle.ForeColor.G},{_cellStyle.ForeColor.B},{_cellStyle.ForeColor.A})";
                    style += $".columnheaderforecolor{{color:{forecolor};}} ";
                    header.StyleContext.AddClass("columnheaderforecolor");
                }
                if (style.Length > 9)
                {
                    Gtk.CssProvider css = new Gtk.CssProvider();
                    css.LoadFromData(style);
                    header.StyleContext.AddProvider(css, 800);
                }
            }
            GridView.AppendColumn(column);
            base.Add(column);
        }
        public new void AddRange(IEnumerable<DataGridViewColumn> columns)
        {
            foreach (DataGridViewColumn column in columns)
            {
                column.DataGridView = __owner;
                DataGridViewCellStyle _cellStyle = column.DefaultCellStyle;
                if (__owner.ColumnHeadersDefaultCellStyle != null)
                    _cellStyle = __owner.ColumnHeadersDefaultCellStyle;

                if (_cellStyle != null)
                {
                    Gtk.Button header = ((Gtk.Button)column.Button);
                    string style = "";
                    if (_cellStyle.BackColor.Name != "0")
                    {
                        string backcolor = $"rgba({_cellStyle.BackColor.R},{_cellStyle.BackColor.G},{_cellStyle.BackColor.B},{_cellStyle.BackColor.A})";
                        style += $".columnheaderbackcolor{{background-color:{backcolor};}} ";
                        header.StyleContext.AddClass("columnheaderbackcolor");
                    }
                    if (_cellStyle.ForeColor.Name != "0")
                    {
                        string forecolor = $"rgba({_cellStyle.ForeColor.R},{_cellStyle.ForeColor.G},{_cellStyle.ForeColor.B},{_cellStyle.ForeColor.A})";
                        style += $".columnheaderforecolor{{color:{forecolor};}} ";
                        header.StyleContext.AddClass("columnheaderforecolor");
                    }
                    if (style.Length > 9)
                    {
                        Gtk.CssProvider css = new Gtk.CssProvider();
                        css.LoadFromData(style);
                        header.StyleContext.AddProvider(css, 600);
                    }
                }
                GridView.AppendColumn(column);
            }
            base.AddRange(columns);
        }
        public new void Clear()
        {
            base.Clear();
            foreach (var wik in GridView.Columns)
                GridView.RemoveColumn(wik);

        }
        public void Invalidate()
        {
            if (__owner.GridView.Columns.Length > __owner.Store.NColumns)
            {
                CellValue[] columnTypes = new CellValue[__owner.GridView.Columns.Length];
                __owner.Store.Clear();
                __owner.Store = new Gtk.TreeStore(Array.ConvertAll(columnTypes, o => typeof(CellValue)));
                __owner.GridView.Model = __owner.Store;
            }
            else if (__owner.GridView.Model == null)
            {
                __owner.GridView.Model = __owner.Store;
            }
            if (__owner.GridView.Columns.Length <= __owner.Store.NColumns)
            {
                int idx = 0;
                foreach (DataGridViewColumn column in this)
                {
                    column.Index = idx;
                    column.DisplayIndex = column.Index;
                    column.DataGridView = __owner;
                    column.Clear();
                    column.Renderer();
                    __owner.Store.SetSortFunc(idx, new Gtk.TreeIterCompareFunc((Gtk.ITreeModel m, Gtk.TreeIter t1, Gtk.TreeIter t2) =>
                    {
                        __owner.Store.GetSortColumnId(out int sortid, out Gtk.SortType order);
                        if (m.GetValue(t1, sortid) == null || m.GetValue(t2, sortid) == null)
                            return 0;
                        else
                            return m.GetValue(t1, sortid).ToString().CompareTo(m.GetValue(t2, sortid).ToString());
                    }));
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
            DataGridViewColumn co = Find(m => m.State == includeFilter);
            return co == null ? __owner.RowHeadersWidth : co.Width;
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

        public DataGridViewColumn GetNextColumn(DataGridViewColumn dataGridViewColumnStart, DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
        {
            int ix = FindIndex(m => m.Name == dataGridViewColumnStart.Name && m.State == includeFilter && m.State == excludeFilter);
            return ix < Count ? base[ix] : null;
        }

        protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(__owner, e);
        }
    }
}
