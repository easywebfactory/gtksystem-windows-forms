﻿/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Forms.GtkRender;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class DataGridView : Control
    {
        public readonly DataGridViewBase self = new DataGridViewBase();
        public override object GtkControl => self;
        private Gtk.TreeView _treeView = new Gtk.TreeView();
        private DataGridViewColumnCollection _columns;
        private DataGridViewRowCollection _rows;
        private ControlBindingsCollection _collect;

        internal Gtk.TreeStore Store = new TreeStore(typeof(CellValue));
        internal Gtk.TreeView TreeView { get { return _treeView; } }
        
        public DataGridView():base()
        {
            _treeView.Valign = Gtk.Align.Fill;
            _treeView.Halign = Gtk.Align.Fill;
            _treeView.Selection.Mode = Gtk.SelectionMode.Multiple;
            _treeView.HeadersClickable = true;
            _treeView.HeadersVisible = true;
            _treeView.ActivateOnSingleClick = true;
            // _treeView.RowActivated += DataGridView_RowActivated;//此事件必须ActivateOnSingleClick = true;

            _columns = new DataGridViewColumnCollection(this);
            _rows = new DataGridViewRowCollection(this);
            _collect = new ControlBindingsCollection(this);

            Gtk.ScrolledWindow scroll = new Gtk.ScrolledWindow();
            scroll.Child = _treeView;
            self.Child = scroll;
        }

        public event EventHandler MultiSelectChanged
        {
            add { _treeView.RowActivated += (object sender, RowActivatedArgs e) => { if (self.IsRealized) { value.Invoke(this, e); } }; }
            remove { _treeView.RowActivated -= (object sender, RowActivatedArgs e) => { if (self.IsRealized) { value.Invoke(this, e); } }; }
        }
        public event DataGridViewCellEventHandler CellClick
        {
            add { _treeView.RowActivated += (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
            remove { _treeView.RowActivated -= (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
        }
        public event DataGridViewCellEventHandler CellEnter
        {
            add { _treeView.RowActivated += (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
            remove { _treeView.RowActivated -= (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
        }
        public event DataGridViewCellEventHandler CellLeave
        {
            add { _treeView.RowActivated += (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
            remove { _treeView.RowActivated -= (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
        }
        public event DataGridViewCellEventHandler CellValidated
        {
            add
            {
                _treeView.WidgetEventAfter += (object sender, WidgetEventAfterArgs e) =>
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
                _treeView.WidgetEventAfter -= (object sender, WidgetEventAfterArgs e) =>
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
                _treeView.RowActivated += (object sender, RowActivatedArgs e) =>
                {
                    if (self.IsRealized)
                    {
                        DataGridViewColumn column = e.Column as DataGridViewColumn;
                        var model = _treeView.Model;
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
                _treeView.RowActivated -= (object sender, RowActivatedArgs e) => {
                    if (self.IsRealized)
                    {
                        DataGridViewColumn column = e.Column as DataGridViewColumn;
                        var model = _treeView.Model;
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
            add { _treeView.RowActivated += (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
            remove { _treeView.RowActivated -= (object sender, RowActivatedArgs e) => { if (self.IsRealized) { DataGridViewColumn column = e.Column as DataGridViewColumn; value.Invoke(this, new DataGridViewCellEventArgs(column.Index, e.Path.Indices[0])); } }; }
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
        internal void BindDataSource(string propertyName, object datasource, string dataMember, int selectindex, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue, string formatString)
        {
            if (datasource == null || string.IsNullOrWhiteSpace(propertyName) || string.IsNullOrWhiteSpace(dataMember))
                return;

            this.TreeView.SelectionNotifyEvent += (object o, SelectionNotifyEventArgs args) => {
                datasource.GetType().GetProperty(propertyName).SetValue(datasource, dataMember);
            };

            this.TreeView.RowActivated += (object o, RowActivatedArgs args)
             => {
                 datasource.GetType().GetProperty(propertyName).SetValue(datasource, dataMember);
             };
        }

        public override ControlBindingsCollection DataBindings { get => _collect; }
        private object _DataSource;
        public object DataSource
        {
            get { return _DataSource; }
            set
            {
                _DataSource = value;
                Store.Clear();
                Store = new Gtk.TreeStore(Array.ConvertAll(_treeView.Columns, o => typeof(CellValue)));
               _treeView.Model = Store;
                updateListStore();
                _columns.Invalidate();
            }
        }

        private void updateListStore()
        {
            if (Store != null)
            {
                Store.Clear();
            }

            if (_DataSource == null)
            {
            }
            else if (_DataSource is DataTable)
            {
                loadDataTableSource();
            }
            else
            {
                loadListSource();
            }
        }
        private void loadDataTableSource()
        {
            DataTable dt = (DataTable)_DataSource;
            if (Columns.Count == 0)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.DataType.Name == "Boolean")
                        Columns.Add(new DataGridViewCheckBoxColumn() { Name = col.ColumnName, HeaderText = col.ColumnName, ValueType = col.DataType });
                    else
                        Columns.Add(new DataGridViewColumn() { Name = col.ColumnName, HeaderText = col.ColumnName, ValueType = col.DataType });
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
        private void loadListSource()
        {
            Type _type = _DataSource.GetType();
            Type[] _entityType = _type.GetGenericArguments();
            if (_entityType.Length == 1)
            {
                if (Columns.Count == 0)
                {
                    PropertyInfo[] pros = _entityType[0].GetProperties();
                    foreach (PropertyInfo pro in pros)
                    {
                        if (pro.PropertyType.Name == "Boolean")
                            Columns.Add(new DataGridViewCheckBoxColumn() { Name = pro.Name, HeaderText = pro.Name, ValueType = pro.PropertyType });
                        else
                            Columns.Add(new DataGridViewColumn() { Name = pro.Name, HeaderText = pro.Name, ValueType = pro.PropertyType });
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
        private Gtk.TreeView _treeview;
        public DataGridViewColumnCollection(DataGridView dataGridView)
        {
            __owner = dataGridView;
            _treeview = dataGridView.TreeView;
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
            _treeview.AppendColumn(column);
        }
        public new void AddRange(IEnumerable<DataGridViewColumn> columns)
        {
            foreach (DataGridViewColumn column in columns)
            {
                column.DataGridView = __owner;
                _treeview.AppendColumn(column);
            }
            base.AddRange(columns);
        }
        public new void Clear()
        {
            base.Clear();
            foreach (var wik in _treeview.Columns)
                _treeview.RemoveColumn(wik);

        }
        public void Invalidate()
        {
            if (__owner.TreeView.Columns.Length > __owner.Store.NColumns)
            {
                CellValue[] columnTypes = new CellValue[__owner.TreeView.Columns.Length];
                __owner.Store.Clear();
                __owner.Store = new TreeStore(Array.ConvertAll(columnTypes, o => typeof(CellValue)));
                __owner.TreeView.Model = __owner.Store;
            }
            else if (__owner.TreeView.Model == null)
            {
                __owner.TreeView.Model = __owner.Store;
            }
            if (__owner.TreeView.Columns.Length <= __owner.Store.NColumns)
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
