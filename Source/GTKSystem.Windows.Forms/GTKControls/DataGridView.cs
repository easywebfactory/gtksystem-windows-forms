/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gdk;
using GLib;
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class DataGridView : ScrollableControl
    {
        public readonly DataGridViewBase self = new DataGridViewBase();
        public override object GtkControl => self;
        private DataGridViewColumnCollection _columns;
        private DataGridViewRowCollection _rows;
        private ControlBindingsCollection _collect;
        internal Gtk.TreeStore Store = new TreeStore(typeof(DataGridViewRow));
        public Gtk.TreeView GridView { get { return self.GridView; } }
        public DataGridView() : base()
        {
            self.Override.sender = this;
            this.BorderStyle = BorderStyle.FixedSingle;
            GridView.Margin = 0;
            GridView.MarginStart = 0;
            GridView.MarginEnd = 0;
            GridView.Selection.Mode = Gtk.SelectionMode.Multiple;
            GridView.HeadersClickable = true;
            GridView.HeadersVisible = true;
            GridView.ActivateOnSingleClick = false;

            _columns = new DataGridViewColumnCollection(this);
            _rows = new DataGridViewRowCollection(this);
            _collect = new ControlBindingsCollection(this);
            self.Realized += Self_Realized;
            GridView.Selection.Changed += Selection_Changed;
            GridView.ColumnsChanged += GridView_ColumnsChanged;
            GridView.ButtonReleaseEvent += GridView_ButtonReleaseEvent;
            GridView.WidgetEvent += GridView_WidgetEvent;
        }

        private void GridView_WidgetEvent(object o, WidgetEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.ButtonPress && args.Event is EventButton eb)
            {
                args.RetVal = eb.Button > 1;
            }
        }

        private void GridView_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
        {
            if (CellClick != null)
            {
                Gtk.Widget widget = o as Gtk.Widget;
                if (widget.Window.Handle.Equals(args.Event.Window.Parent.Handle) && GridView.GetPathAtPos((int)args.Event.X, (int)args.Event.Y, out TreePath path, out TreeViewColumn column))
                {
                    if(Store.GetIter(out TreeIter iter, path))
                    {
                        DataGridViewRow row = GetRowByIter(Rows, iter, path.Depth);
                        if (row != null) {
                            CellClick(this, new DataGridViewCellEventArgs(((DataGridViewColumn)column).Index, row.Index));
                        }
                    }
                }
            }
        }
        private DataGridViewRow GetRowByIter(DataGridViewRowCollection rows, TreeIter iter, int depth)
        {
            foreach (DataGridViewRow row in rows.SharedList)
            {
                if (row.TreeIter.Equals(iter))
                {
                    return row;
                    break;
                }
                else if (depth > 1 && row.Children.Count > 0)
                {
                    return GetRowByIter(row.Children, iter, --depth);
                }
            }
            return null;
        }

        private void GridView_ColumnsChanged(object sender, EventArgs e)
        {
            foreach (var item in GridView.Columns)
            {
                item.Clicked -= Item_Clicked;
                item.Clicked += Item_Clicked;
            }
        }

        private void Item_Clicked(object sender, EventArgs e)
        {
            _selectedColumn = ((DataGridViewColumn)sender);
        }
        private DataGridViewColumn _selectedColumn;
        private void Selection_Changed(object sender, EventArgs e)
        {
            if (SelectionChanged != null && Created)
                SelectionChanged(this, e);
        }
        private bool Is_GridView_Realized;
        private void Self_Realized(object sender, EventArgs e)
        {
            if (!Is_GridView_Realized)
            {
                Is_GridView_Realized = true;
                OnSetDataSource();
                _columns.Invalidate();
                AllowUserToOrderColumnsChanged?.Invoke(this, EventArgs.Empty);
                AllowUserToResizeColumnsChanged?.Invoke(this, EventArgs.Empty);
                foreach (Binding binding in DataBindings)
                    GridView.AddNotification(binding.PropertyName, propertyNotity);

                if (_sortedColumn != null)
                {
                    Store.SetSortColumnId(_sortedColumn.SortColumnId, _sortedColumn.SortOrder);
                }
            }
            this.Invalidate(false);
        }
        private void propertyNotity(object o, NotifyArgs args)
        {
            Binding binding = DataBindings[args.Property];
            binding.WriteValue();
        }
        public event EventHandler SelectionChanged;
        public event DataGridViewCellEventHandler CellClick;
        internal void CellValueChanagedHandler(int column, TreeIter iter, TreePath path)
        {
            if (CellValueChanged != null)
            {
                DataGridViewRow row = GetRowByIter(Rows, iter, path.Depth);
                if (row != null)
                {
                    CellValueChanged(this, new DataGridViewCellEventArgs(column, row.Index));
                }
            }
        }
        public event DataGridViewCellEventHandler CellValueChanged;
        public void SetExpandRow(DataGridViewRow row, bool all)
        {
            GridView.ExpandRow(Store.GetPath(row.TreeIter), all);
        }
        public void SetCollapseRow(DataGridViewRow row)
        {
            GridView.CollapseRow(Store.GetPath(row.TreeIter));
        }
        private bool _MultiSelect = true;
        public bool MultiSelect
        {
            get => _MultiSelect;
            set
            {
                _MultiSelect = value;
                GridView.ActivateOnSingleClick = !_MultiSelect;
                if (_SelectionMode == DataGridViewSelectionMode.CellSelect)
                {
                    GridView.Selection.Mode = Gtk.SelectionMode.None;
                }
                else
                {
                    if (_MultiSelect == true)
                        GridView.Selection.Mode = Gtk.SelectionMode.Multiple;
                    else
                        GridView.Selection.Mode = Gtk.SelectionMode.Single;
                }
            }
        }

        private DataGridViewSelectionMode _SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
        public DataGridViewSelectionMode SelectionMode
        {
            get => _SelectionMode;
            set
            {
                _SelectionMode = value;
                switch (_SelectionMode)
                {
                    case DataGridViewSelectionMode.CellSelect:
                        GridView.Selection.Mode = Gtk.SelectionMode.None;
                        break;
                    case DataGridViewSelectionMode.FullColumnSelect:
                    case DataGridViewSelectionMode.ColumnHeaderSelect:
                        GridView.Selection.Mode = Gtk.SelectionMode.None;
                        break;
                    case DataGridViewSelectionMode.FullRowSelect:
                    case DataGridViewSelectionMode.RowHeaderSelect:
                        GridView.Selection.Mode = Gtk.SelectionMode.Multiple;
                        break;
                }

            }
        }
        public bool VirtualMode { get => true; set { } }
        public string Markup { get; set; } = "...";
        public bool ReadOnly { get; set; }
        public int RowHeadersWidth { get; set; }
        public int ColumnHeadersHeight { get; set; }
        public DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
        {
            get; set;
        }
        public DataGridViewAutoSizeRowsMode AutoSizeRowsMode { get; set; } = DataGridViewAutoSizeRowsMode.None;
        private DataGridViewRow _RowTemplate;
        public DataGridViewRow RowTemplate
        {
            get { _RowTemplate ??= new DataGridViewRow(); 
                _RowTemplate.DefaultCellStyle = new DataGridViewCellStyle(); 
                return _RowTemplate; 
            }
            set { _RowTemplate = value; }
        }
        public DataGridViewCellStyle DefaultCellStyle { get; set; }
        public DataGridViewCellStyle ColumnHeadersDefaultCellStyle { get; set; }
        public DataGridViewCellStyle AlternatingRowsDefaultCellStyle { get; set; }
        public DataGridViewCellStyle RowsDefaultCellStyle { get; set; }
        public DataGridViewCellStyle RowHeadersDefaultCellStyle { get; set; }
        public override ControlBindingsCollection DataBindings { get => _collect; }
        private object _DataSource;
        public object DataSource
        {
            get { return _DataSource; }
            set
            {
                _DataSource = value;
                if (Is_GridView_Realized == true)
                {
                    OnSetDataSource();
                }
            }
        }
        private void OnSetDataSource()
        {
            _Created = false;
            _rows.Clear();
            if (_DataSource != null)
            {
                if (_DataSource is DataTable dtable)
                {
                    LoadDataTableSource(dtable);
                }
                else if (_DataSource is DataSet dset)
                {
                    if (string.IsNullOrEmpty(DataMember))
                        LoadDataTableSource(dset.Tables[0]);
                    else
                        LoadDataTableSource(dset.Tables[DataMember]);
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
            _Created = true;
        }
        public string DataMember { get; set; }
        private void LoadDataTableSource(DataTable dt)
        {
            foreach (DataColumn col in dt.Columns)
            {
                DataGridViewColumn column = _columns.Find(m => m.DataPropertyName == col.ColumnName);
                if (column == null)
                {
                    if (AutoGenerateColumns)
                    {
                        if (col.DataType.Name == "Boolean")
                            _columns.Add(new DataGridViewCheckBoxColumn(this) { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
                        else if (col.DataType.Name == "Image" || col.DataType.Name == "Bitmap")
                            _columns.Add(new DataGridViewImageColumn(this) { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
                        else
                            _columns.Add(new DataGridViewColumn(this) { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
                    }
                }
                else
                {
                    column.ValueType = col.DataType;
                }
            }
            if (_columns.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataGridViewRow newRow = new DataGridViewRow(dr);
                    foreach (DataGridViewColumn col in _columns)
                    {
                        object cellvalue = dt.Columns.Contains(col.DataPropertyName) ? dr[col.DataPropertyName] : null;
                        newRow.Cells.Add(col.NewCell(cellvalue, col.ValueType));
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
                PropertyInfo[] pros = _entityType[0].GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo pro in pros)
                {
                    DataGridViewColumn column = _columns.Find(m => m.DataPropertyName == pro.Name);
                    if (column == null)
                    {
                        if (AutoGenerateColumns)
                        {
                            if (pro.PropertyType.Name == "Boolean")
                                _columns.Add(new DataGridViewCheckBoxColumn(this) { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                            else if (pro.PropertyType.Name == "Image" || pro.PropertyType.Name == "Bitmap")
                                _columns.Add(new DataGridViewImageColumn(this) { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                            else
                                _columns.Add(new DataGridViewColumn(this) { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                        }
                    }
                    else
                    {
                        column.ValueType = pro.PropertyType;
                    }
                }
                if (_columns.Count > 0)
                {
                    IEnumerator reader = ((IEnumerable)_DataSource).GetEnumerator();
                    while (reader.MoveNext())
                    {
                        object obj = reader.Current;
                        Type type = obj.GetType();
                        DataGridViewRow newRow = new DataGridViewRow(obj);
                        foreach (DataGridViewColumn col in _columns)
                        {
                            object cellvalue = type.GetProperty(col.DataPropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)?.GetValue(obj);
                            newRow.Cells.Add(col.NewCell(cellvalue, col.ValueType));
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
        public int RowCount { 
            get => _rows.Count; 
            set { 
                while (value > _rows.Count) { _rows.RemoveAt(_rows.Count - 1); }  
            }
        }
        public DataGridViewRow CurrentRow
        {
            get
            {
                var selectedrows = SelectedRows;
                return selectedrows.Count > 0 ? selectedrows[selectedrows.Count - 1] : null;
            }
        }
        [Browsable(false)]
        public DataGridViewSelectedCellCollection SelectedCells
        {
            get
            {
                DataGridViewSelectedCellCollection stcc = new DataGridViewSelectedCellCollection();
                switch (SelectionMode)
                {
                    case DataGridViewSelectionMode.CellSelect:
                        break;
                    case DataGridViewSelectionMode.FullColumnSelect:
                    case DataGridViewSelectionMode.ColumnHeaderSelect:
                        break;
                    case DataGridViewSelectionMode.FullRowSelect:
                    case DataGridViewSelectionMode.RowHeaderSelect:
                        foreach (DataGridViewRow dataGridViewRow in SelectedRows)
                        {
                            foreach (DataGridViewCell dataGridViewCell in dataGridViewRow.Cells)
                            {
                                stcc.Add(dataGridViewCell);
                            }
                        }
                        break;
                }

                return stcc;
            }
        }

        [Browsable(false)]
        public DataGridViewSelectedColumnCollection SelectedColumns
        {
            get
            {
                DataGridViewSelectedColumnCollection strc = new DataGridViewSelectedColumnCollection();
                switch (SelectionMode)
                {
                    case DataGridViewSelectionMode.CellSelect:
                    case DataGridViewSelectionMode.FullRowSelect:
                    case DataGridViewSelectionMode.RowHeaderSelect:
                        break;
                    case DataGridViewSelectionMode.FullColumnSelect:
                    case DataGridViewSelectionMode.ColumnHeaderSelect:
                        if (_selectedColumn != null)
                            strc.Add(_selectedColumn);
                        break;
                }

                return strc;
            }
        }

        [Browsable(false)]
        public DataGridViewSelectedRowCollection SelectedRows
        {
            get
            {
                DataGridViewSelectedRowCollection strc = new DataGridViewSelectedRowCollection();
                switch (SelectionMode)
                {
                    case DataGridViewSelectionMode.CellSelect:
                    case DataGridViewSelectionMode.FullColumnSelect:
                    case DataGridViewSelectionMode.ColumnHeaderSelect:
                        break;
                    case DataGridViewSelectionMode.FullRowSelect:
                    case DataGridViewSelectionMode.RowHeaderSelect:
                        TreePath[] treePaths = GridView.Selection.GetSelectedRows();
                        foreach (TreePath path in treePaths)
                        {
                            if (Store.GetIter(out TreeIter iter, path))
                            {
                                DataGridViewRow row = GetRowByIter(Rows, iter, path.Depth);
                                if (row != null)
                                    strc.Add(row);
                            }
                        }
                        break;
                }

                return strc;
            }
        }
        internal bool NativeRowGetSelected(TreeIter rowiter)
        {
            switch (SelectionMode)
            {
                case DataGridViewSelectionMode.FullRowSelect:
                case DataGridViewSelectionMode.RowHeaderSelect:
                    return (GridView.Selection.GetSelected(out TreeIter iter) && iter.Equals(rowiter));
                    break;
                default:
                    return false;
            }
        }
        internal void NativeRowSetSelected(TreeIter rowiter, bool selected)
        {
            Gtk.Application.Invoke(delegate
            {
                if (selected)
                    GridView.Selection.SelectIter(rowiter);
                else
                    GridView.Selection.UnselectIter(rowiter);
            });
        }
        public bool AutoGenerateColumns { get; set; } = true;
        /// <summary>
        /// 是否起用过滤功能，用于row.visible功能，同时禁用列排序
        /// </summary>
        public bool UseModelFilter { get; set; }
        public bool AllowUserToAddRows { get; set; }
        public bool AllowUserToDeleteRows { get; set; }

        private bool _AllowUserToOrderColumns = true;
        public bool AllowUserToOrderColumns
        {
            get => _AllowUserToOrderColumns;
            set
            {
                _AllowUserToOrderColumns = value;
                if (AllowUserToOrderColumnsChanged == null)
                    AllowUserToOrderColumnsChanged += DataGridView_AllowUserToOrderColumnsChanged;
                else
                    AllowUserToOrderColumnsChanged(this, EventArgs.Empty);
            }
        }

        private void DataGridView_AllowUserToOrderColumnsChanged(object? sender, EventArgs e)
        {
            foreach (var item in GridView.Columns)
            {
                item.Reorderable = _AllowUserToOrderColumns;
            }
        }

        private bool _AllowUserToResizeColumns = true;
        public bool AllowUserToResizeColumns
        {
            get => AllowUserToResizeColumns;
            set
            {
                _AllowUserToResizeColumns = value;
                if (AllowUserToResizeColumnsChanged == null)
                    AllowUserToResizeColumnsChanged += DataGridView_AllowUserToResizeColumnsChanged;
                else
                    AllowUserToResizeColumnsChanged(this, EventArgs.Empty);
            }
        }

        private void DataGridView_AllowUserToResizeColumnsChanged(object? sender, EventArgs e)
        {
            foreach (var item in GridView.Columns)
            {
                item.Resizable = _AllowUserToResizeColumns;
            }
        }

        public bool AllowUserToResizeRows { get => GridView.Reorderable; set => GridView.Reorderable = value; }
        private DataGridViewColumn _sortedColumn;
        public DataGridViewColumn SortedColumn => _sortedColumn;

        private SortOrder _sortOrder;
        public SortOrder SortOrder => _sortOrder;
        public virtual void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction)
        {
            _sortedColumn= dataGridViewColumn;
            if (direction == ListSortDirection.Ascending)
            {
                _sortOrder = SortOrder.Ascending;
                _sortedColumn.SortOrder = SortType.Ascending;
            }
            else if (direction == ListSortDirection.Descending)
            {
                _sortOrder = SortOrder.Descending;
                _sortedColumn.SortOrder = SortType.Descending;
            }
            if(self.IsRealized)
                Store.SetSortColumnId(_sortedColumn.SortColumnId, _sortedColumn.SortOrder);
        }
        public override void BeginInit()
        {
            _Created = false;
        }

        public override void EndInit()
        {
            _Created = true;
        }
        public void ClearSelection()
        {
            GridView.Selection.UnselectAll();
        }
        public void OnCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (CellPainting != null)
                CellPainting(sender, e);
        }
        public void EndEdit()
        {
            foreach (DataGridViewColumn column in Columns)
            {
                column.EditableEditingDone();
            }
        }

        private int _FirstDisplayedScrollingRowIndex;
        public int FirstDisplayedScrollingRowIndex
        {
            get => _FirstDisplayedScrollingRowIndex;
            set
            {
                _FirstDisplayedScrollingRowIndex = value;
                var grid = GridView;
                if (grid.NColumns > 0 && Rows.Count > value)
                {
                    grid.ScrollToCell(new TreePath([value]), null, false, 0, 0);
                }
            }
        }
        private int _FirstDisplayedScrollingColumnIndex;
        public int FirstDisplayedScrollingColumnIndex
        {
            get => _FirstDisplayedScrollingColumnIndex;
            set
            {
                _FirstDisplayedScrollingColumnIndex = value;
                var grid = GridView;
                if (_FirstDisplayedScrollingColumnIndex > -1 && grid.NColumns > _FirstDisplayedScrollingColumnIndex)
                {
                    int x_offset = grid.GetColumn(_FirstDisplayedScrollingColumnIndex).XOffset;
                    self.ScrollView(x_offset, -1);
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            _rows.Clear();
            _columns.Clear();
            _collect.Clear();
            _DataSource = null;
            base.Dispose(disposing);
        }

        //[Obsolete("此事件未实现，自行开发")]
        //public event EventHandler BackgroundImageChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler BackgroundColorChanged;
        //[Obsolete("此事件未实现，自行开发")]
        //public event EventHandler BackColorChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewAutoSizeModeEventHandler AutoSizeRowsModeChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewAutoSizeColumnsModeEventHandler AutoSizeColumnsModeChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler AutoGenerateColumnsChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler AlternatingRowsDefaultCellStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler AllowUserToResizeRowsChanged;

        public event EventHandler AllowUserToResizeColumnsChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler AllowUserToDeleteRowsChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler AllowUserToAddRowsChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler Sorted;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewSortCompareEventHandler SortCompare;
        //[Obsolete("此事件未实现，自行开发")]
        //public event EventHandler SelectionChanged;

        public event EventHandler AllowUserToOrderColumnsChanged;
        //[Obsolete("此事件未实现，自行开发")]
        //public event EventHandler StyleChanged;
        //[Obsolete("此事件未实现，自行开发")]
        //public event EventHandler BackgroundImageLayoutChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler CellBorderStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewDataErrorEventHandler DataError;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewBindingCompleteEventHandler DataBindingComplete;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler CurrentCellDirtyStateChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler CurrentCellChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnWidthChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnToolTipTextChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnStateChangedEventHandler ColumnStateChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnSortModeChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnRemoved;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnNameChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler DefaultValuesNeeded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnMinimumWidthChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellMouseEventHandler ColumnHeaderMouseDoubleClick;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellMouseEventHandler ColumnHeaderMouseClick;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnDividerWidthChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnDividerDoubleClickEventHandler ColumnDividerDoubleClick;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnDisplayIndexChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnDefaultCellStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnDataPropertyNameChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnContextMenuStripChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnAdded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellValueEventHandler CellValuePushed;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewColumnEventHandler ColumnHeaderCellChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellValueEventHandler CellValueNeeded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewEditingControlShowingEventHandler EditingControlShowing;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler RowContextMenuStripChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler RowUnshared;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowStateChangedEventHandler RowStateChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowsRemovedEventHandler RowsRemoved;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowsAddedEventHandler RowsAdded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowPrePaintEventHandler RowPrePaint;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowPostPaintEventHandler RowPostPaint;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler RowMinimumHeightChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler RowLeave;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowHeightInfoPushedEventHandler RowHeightInfoPushed;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowHeightInfoNeededEventHandler RowHeightInfoNeeded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler NewRowNeeded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler RowHeightChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellMouseEventHandler RowHeaderMouseDoubleClick;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellMouseEventHandler RowHeaderMouseClick;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowErrorTextNeededEventHandler RowErrorTextNeeded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler RowErrorTextChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler RowEnter;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler RowDividerHeightChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowDividerDoubleClickEventHandler RowDividerDoubleClick;
        [Obsolete("此事件未实现，自行开发")]
        public event QuestionEventHandler RowDirtyStateNeeded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler RowDefaultCellStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowContextMenuStripNeededEventHandler RowContextMenuStripNeeded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler RowHeaderCellChanged;
        //[Obsolete("此事件未实现，自行开发")]
        //public event DataGridViewCellEventHandler CellValueChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellValidatingEventHandler CellValidating;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellValidated;
        [Obsolete("此事件未实现，自行开发")]
        public event QuestionEventHandler CancelRowEdit;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewAutoSizeColumnModeEventHandler AutoSizeColumnModeChanged;
        //[Obsolete("此事件未实现，自行开发")]
        //public event EventHandler TextChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler RowsDefaultCellStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewAutoSizeModeEventHandler RowHeadersWidthSizeModeChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler RowHeadersWidthChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler RowHeadersDefaultCellStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler RowHeadersBorderStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler ReadOnlyChanged;
        //[Obsolete("此事件未实现，自行开发")]
        //public event EventHandler PaddingChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellCancelEventHandler CellBeginEdit;
        [Obsolete("此事件未实现，此事件未实现，自行开发")]
        public event EventHandler MultiSelectChanged;
        //[Obsolete("此事件未实现，自行开发")]
        //public event EventHandler FontChanged;
        //[Obsolete("此事件未实现，自行开发")]
        //public event EventHandler ForeColorChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler EditModeChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler DefaultCellStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler DataSourceChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler DataMemberChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewAutoSizeModeEventHandler ColumnHeadersHeightSizeModeChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler ColumnHeadersHeightChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler ColumnHeadersDefaultCellStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler ColumnHeadersBorderStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler GridColorChanged;
        //[Obsolete("此事件未实现，自行开发")]
        //public event DataGridViewCellEventHandler CellClick;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellContentClick;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellContentDoubleClick;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellToolTipTextNeededEventHandler CellToolTipTextNeeded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellToolTipTextChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellStyleContentChangedEventHandler CellStyleContentChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellStateChangedEventHandler CellStateChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellParsingEventHandler CellParsing;
        public event DataGridViewCellPaintingEventHandler CellPainting;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellMouseEventHandler CellMouseUp;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellMouseEventHandler CellMouseMove;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellMouseLeave;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellMouseEnter;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellMouseEventHandler CellMouseDown;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellMouseEventHandler CellMouseDoubleClick;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellMouseEventHandler CellMouseClick;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellLeave;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellFormattingEventHandler CellFormatting;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellErrorTextNeededEventHandler CellErrorTextNeeded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellErrorTextChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellEnter;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellEndEdit;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellDoubleClick;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellContextMenuStripNeededEventHandler CellContextMenuStripNeeded;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler CellContextMenuStripChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event EventHandler BorderStyleChanged;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellEventHandler RowValidated;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewCellCancelEventHandler RowValidating;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowCancelEventHandler UserDeletingRow;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler UserDeletedRow;
        [Obsolete("此事件未实现，自行开发")]
        public event DataGridViewRowEventHandler UserAddedRow;
    }
}
