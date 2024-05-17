/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using GLib;
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms.GtkRender;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class DataGridView : Control
    {
        public readonly DataGridViewBase self = new DataGridViewBase();
        public override object GtkControl => self;
        private DataGridViewColumnCollection _columns;
        private DataGridViewRowCollection _rows;
        private ControlBindingsCollection _collect;
        internal Gtk.TreeStore Store = new TreeStore(typeof(CellValue));
        internal Gtk.TreeView GridView { get { return self.GridView; } }
        public DataGridView():base()
        {
            GridView.Selection.Mode = Gtk.SelectionMode.Multiple;
            GridView.HeadersClickable = true;
            GridView.HeadersVisible = true;
            GridView.ActivateOnSingleClick = true;
            // GridView.RowActivated += DataGridView_RowActivated;//此事件必须ActivateOnSingleClick = true;

            _columns = new DataGridViewColumnCollection(this);
            _rows = new DataGridViewRowCollection(this);
            _collect = new ControlBindingsCollection(this);
            GridView.Realized += GridView_Realized;

        }
        private void GridView_Realized(object sender, EventArgs e)
        {
            OnSetDataSource();
            foreach (Binding binding in DataBindings)
                GridView.AddNotification(binding.PropertyName, propertyNotity);
        }
        private void propertyNotity(object o, NotifyArgs args)
        {
            Binding binding = DataBindings[args.Property];
            binding.WriteValue();
        }

        public event EventHandler MultiSelectChanged
        {
            add { GridView.RowActivated += (object sender, RowActivatedArgs e) => { if (self.IsRealized) { value.Invoke(this, e); } }; }
            remove { GridView.RowActivated -= (object sender, RowActivatedArgs e) => { if (self.IsRealized) { value.Invoke(this, e); } }; }
        }
        public event DataGridViewCellEventHandler CellClick
        {
            add { GridView.RowActivated += (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
            remove { GridView.RowActivated -= (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
        }
        public event DataGridViewCellEventHandler CellEnter
        {
            add { GridView.RowActivated += (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
            remove { GridView.RowActivated -= (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
        }
        public event DataGridViewCellEventHandler CellLeave
        {
            add { GridView.RowActivated += (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
            remove { GridView.RowActivated -= (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
        }
        public event DataGridViewCellEventHandler CellValidated
        {
            add
            {
                GridView.WidgetEventAfter += (object sender, WidgetEventAfterArgs e) =>
                {
                     if (self.IsRealized) {
                        if (isCellValidating)
                        {
                            isCellValidating = false;
                            value.Invoke(this, new DataGridViewCellEventArgs(columnid, rowid));
                        }
                    }
                };
            }
            remove
            {
                GridView.WidgetEventAfter -= (object sender, WidgetEventAfterArgs e) =>
                {
                    if (self.IsRealized)
                    {
                        if (isCellValidating)
                        {
                            isCellValidating = false;
                            value.Invoke(this, new DataGridViewCellEventArgs(columnid, rowid));
                        }
                    }
                };
            }
        }
        int columnid = 0;
        int rowid = 0;
        bool isCellValidating = false;
        public event DataGridViewCellValidatingEventHandler CellValidating
        {
            add
            {
                GridView.RowActivated += (object sender, RowActivatedArgs e) =>
                {
                    if (self.IsRealized)
                    {
                        DataGridViewColumn column = e.Column as DataGridViewColumn;
                        var model = GridView.Model;
                        model.GetIter(out TreeIter iter, e.Path);
                        CellValue val = (CellValue)(model.GetValue(iter, column.Index));
                        value.Invoke(this, new DataGridViewCellValidatingEventArgs(column.Index, e.Path.Indices[0], val?.Text));
                        columnid = column.Index;
                        rowid = e.Path.Indices[0];
                        isCellValidating = true;
                    }
                };
            }
            remove
            {
                GridView.RowActivated -= (object sender, RowActivatedArgs e) => {
                    if (self.IsRealized)
                    {
                        DataGridViewColumn column = e.Column as DataGridViewColumn;
                        var model = GridView.Model;
                        model.GetIter(out TreeIter iter, e.Path);
                        CellValue val = (CellValue)(model.GetValue(iter, column.Index));

                        value.Invoke(this, new DataGridViewCellValidatingEventArgs(column.Index, e.Path.Indices[0], val?.Text));
                        columnid = column.Index;
                        rowid = e.Path.Indices[0];
                        isCellValidating = true;
                    }
                };
            }
        }

        internal void CellValueChanagedHandler(int column, int row, CellValue val)
        {
            var cells = _rows[row].Cells;
            if(cells.Count>column)
            {
                cells[column].Value = val?.Text;
            }

            if (CellValueChanged != null)
            {
                CellValueChanged(this, new DataGridViewCellEventArgs(column, row));
            }
        }
        public event DataGridViewCellEventHandler CellValueChanged;

        public event DataGridViewCellEventHandler RowEnter;
        public event DataGridViewCellEventHandler RowLeave;
        public event EventHandler SelectionChanged
        {
            add
            {
                GridView.Selection.Changed += (object sender, EventArgs e) => {
                    if (self.IsVisible)
                    {
                        value.Invoke(this, e);
                    }
                };
            }
            remove
            {
                GridView.Selection.Changed -= (object sender, EventArgs e) => {
                    if (self.IsVisible)
                    {
                        value.Invoke(this, e);
                    }
                };
            }
        }

        public string Markup { get; set; } = "...";
       
        public int RowHeadersWidth { get; set; }
        public int ColumnHeadersHeight { get; set; }

        public DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
        {
            get;set;
        }
        public DataGridViewRow RowTemplate
        {
            get { return new DataGridViewRow(); }
            set { }
        }
        public override ControlBindingsCollection DataBindings { get => _collect; }
        private object _DataSource;
        public object DataSource
        {
            get { return _DataSource; }
            set
            {
                _DataSource = value;
                if (GridView.IsVisible)
                {
                    OnSetDataSource();
                }
            }
        }
        private void OnSetDataSource()
        {
            if (_DataSource != null)
            {
                Store.Clear();
                if (_DataSource != null)
                {
                    Store = new Gtk.TreeStore(Array.ConvertAll(GridView.Columns, o => typeof(CellValue)));
                    GridView.Model = Store;
                    if (_DataSource is DataTable dtable)
                    {
                        LoadDataTableSource(dtable);
                    }
                    else if (_DataSource is DataView dview)
                    {
                        LoadDataTableSource(dview.Table);
                    }
                    else
                    {
                        LoadListSource();
                    }
                    _columns.Invalidate();
                }
            }
        }
        public string DataMember { get; set; }
        private void UpdateListStore()
        {
            if (Store != null)
            {
                Store.Clear();
            }

            if (_DataSource == null)
            {
            }
            else if (_DataSource is DataTable dtable)
            {
                LoadDataTableSource(dtable);
            }
            else if (_DataSource is DataView dview)
            {
                LoadDataTableSource(dview.Table);
            }
            else
            {
                LoadListSource();
            }
        }
        private void LoadDataTableSource(DataTable dt)
        {
            if (Columns.Count == 0)
            {
                string[] _DataMembers = string.IsNullOrWhiteSpace(DataMember) ? new string[0] : DataMember.Split(",");
                foreach (DataColumn col in dt.Columns)
                {
                    if (_DataMembers.Length == 0 || _DataMembers.Contains(col.ColumnName))
                    {
                        if (col.DataType.Name == "Boolean")
                            Columns.Add(new DataGridViewCheckBoxColumn() { Name = col.ColumnName, HeaderText = col.ColumnName, ValueType = col.DataType });
                        else
                            Columns.Add(new DataGridViewColumn() { Name = col.ColumnName, HeaderText = col.ColumnName, ValueType = col.DataType });
                    }
                }
                _columns.Invalidate();
            }
            int ncolumns = Columns.Count;
            if (ncolumns > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    foreach (DataGridViewColumn col in Columns)
                    {
                        string cellvalue = dt.Columns.Contains(col.Name) ? dr[col.Name].ToString() : string.Empty;
                        if (col is DataGridViewTextBoxColumn)
                            newRow.Cells.Add(new DataGridViewTextBoxCell() { Value = cellvalue });
                        else if (col is DataGridViewImageColumn)
                            newRow.Cells.Add(new DataGridViewImageCell() { Value = cellvalue });
                        else if (col is DataGridViewCheckBoxColumn)
                            newRow.Cells.Add(new DataGridViewCheckBoxCell() { Value = cellvalue });
                        else if (col is DataGridViewButtonColumn)
                            newRow.Cells.Add(new DataGridViewButtonCell() { Value = cellvalue });
                        else if (col is DataGridViewComboBoxColumn)
                            newRow.Cells.Add(new DataGridViewComboBoxCell() { Value = cellvalue });
                        else if (col is DataGridViewLinkColumn)
                            newRow.Cells.Add(new DataGridViewLinkCell() { Value = cellvalue });
                        else
                            newRow.Cells.Add(new DataGridViewTextBoxCell() { Value = cellvalue });
                    }
                    _rows.Add(newRow);
                }
            }
        }
        private void LoadListSource()
        {
            Type _type = _DataSource.GetType();
            Type[] _entityType = _type.GetGenericArguments();
            if (_entityType.Length == 1)
            {
                if (Columns.Count == 0)
                {
                    string[] _DataMembers = string.IsNullOrWhiteSpace(DataMember) ? new string[0] : DataMember.Split(",");
                    PropertyInfo[] pros = _entityType[0].GetProperties();
                    foreach (PropertyInfo pro in pros)
                    {
                        if (_DataMembers.Length == 0 || _DataMembers.Contains(pro.Name))
                        {
                            if (pro.PropertyType.Name == "Boolean")
                                Columns.Add(new DataGridViewCheckBoxColumn() { Name = pro.Name, HeaderText = pro.Name, ValueType = pro.PropertyType });
                            else
                                Columns.Add(new DataGridViewColumn() { Name = pro.Name, HeaderText = pro.Name, ValueType = pro.PropertyType });
                        }
                    }
                    _columns.Invalidate();
                }

                int ncolumns = Columns.Count;
                if (ncolumns > 0)
                {
                    IEnumerator reader = ((IEnumerable)_DataSource).GetEnumerator();
                    while (reader.MoveNext())
                    {
                        object obj = reader.Current;
                        Dictionary<string, string> values = new Dictionary<string, string>();
                        Array.ForEach(obj.GetType().GetProperties(), o => { values.Add(o.Name, Convert.ToString(o.GetValue(obj))); });
                        DataGridViewRow newRow = new DataGridViewRow();
                        foreach (DataGridViewColumn col in Columns)
                        {
                            string cellvalue = values.ContainsKey(col.Name) ? values[col.Name] : string.Empty;
                            if (col is DataGridViewTextBoxColumn)
                                newRow.Cells.Add(new DataGridViewTextBoxCell() { Value = cellvalue });
                            else if (col is DataGridViewImageColumn)
                                newRow.Cells.Add(new DataGridViewImageCell() { Value = cellvalue });
                            else if (col is DataGridViewCheckBoxColumn)
                                newRow.Cells.Add(new DataGridViewCheckBoxCell() { Value = cellvalue });
                            else if (col is DataGridViewButtonColumn)
                                newRow.Cells.Add(new DataGridViewButtonCell() { Value = cellvalue });
                            else if (col is DataGridViewComboBoxColumn)
                                newRow.Cells.Add(new DataGridViewComboBoxCell() { Value = cellvalue });
                            else if (col is DataGridViewLinkColumn)
                                newRow.Cells.Add(new DataGridViewLinkCell() { Value = cellvalue });
                            else
                                newRow.Cells.Add(new DataGridViewTextBoxCell() { Value = cellvalue });
                        }
                        _rows.Add(newRow);
                    }
                }
            }
        }
        public DataGridViewColumnCollection Columns
        {
            get
            {
                return _columns;
            }
        }

        public DataGridViewRowCollection Rows { get { return _rows; } }

       

        public override void BeginInit()
        {

        }

        public override void EndInit()
        {

        }
    }
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
        public event CollectionChangeEventHandler CollectionChanged;

        public void Add(string columnName, string headerText)
        {
            DataGridViewColumn column = new DataGridViewColumn() { Name = columnName, HeaderText = headerText };
            Add(column);
        }
        public new void Add(DataGridViewColumn column)
        {
            column.DataGridView = __owner;
            base.Add(column);
            GridView.AppendColumn(column);
        }
        public new void AddRange(IEnumerable<DataGridViewColumn> columns)
        {
            foreach (DataGridViewColumn column in columns)
            {
                column.DataGridView = __owner;
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
                __owner.Store = new TreeStore(Array.ConvertAll(columnTypes, o => typeof(CellValue)));
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
