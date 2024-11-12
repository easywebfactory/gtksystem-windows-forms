/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
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
    public partial class DataGridView : ScrollableControl
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
            GridView.Realized += GridView_Realized;
            GridView.RowActivated += GridView_RowActivated;
            GridView.Selection.Changed += Selection_Changed;
        }
        private List<int> _selectedBandIndexes = new List<int>();
        private void Selection_Changed(object sender, EventArgs e)
        {
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _selectedBandIndexes.Clear();
            TreePath[] treePaths = GridView.Selection.GetSelectedRows();
            foreach (TreePath path in treePaths)
            {
                int idx = path.Indices.Last();
                _selectedBandIndexes.Add(idx);
            }
            if (SelectionChanged != null && Created)
                SelectionChanged(this, e);
        }

        private void GridView_RowActivated(object o, RowActivatedArgs args)
        {
            //单行选择有效
            if (CellClick != null)
            {
                DataGridViewColumn column = args.Column as DataGridViewColumn;
                CellClick(this, new DataGridViewCellEventArgs(column.Index, args.Path.Indices[0]));
            }
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
        public event EventHandler SelectionChanged;
        public event DataGridViewCellEventHandler CellClick;
        internal void CellValueChanagedHandler(int column, int row, CellValue val)
        {
            var cells = _rows[row].Cells;
            if (cells.Count > column)
            {
                cells[column].Value = val?.Text;
            }

            if (CellValueChanged != null)
            {
                CellValueChanged(this, new DataGridViewCellEventArgs(column, row));
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
        public bool MultiSelect { get => !GridView.ActivateOnSingleClick; set { GridView.ActivateOnSingleClick = !value; } }
        public DataGridViewSelectionMode SelectionMode { get; set; }
        public string Markup { get; set; } = "...";
        public bool ReadOnly { get; set; }
        public int RowHeadersWidth { get; set; }
        public int ColumnHeadersHeight { get; set; }

        public DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
        {
            get;set;
        }
        public DataGridViewAutoSizeRowsMode AutoSizeRowsMode { get; set; }
        private DataGridViewRow _RowTemplate;
        public DataGridViewRow RowTemplate
        {
            get { return _RowTemplate ??= new DataGridViewRow(); }
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
                if (GridView.IsVisible)
                {
                    OnSetDataSource();
                }
            }
        }
        private void OnSetDataSource()
        {
            _Created = false;
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
            _Created = true;
        }
        public string DataMember { get; set; }
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
        [Browsable(false)]
        public DataGridViewSelectedCellCollection SelectedCells
        {
            get
            {
                DataGridViewSelectedCellCollection stcc = new DataGridViewSelectedCellCollection();
                switch (SelectionMode)
                {
                    case DataGridViewSelectionMode.CellSelect:
                        {

                            break;
                        }

                    case DataGridViewSelectionMode.FullColumnSelect:
                    case DataGridViewSelectionMode.ColumnHeaderSelect:
                        {
                            foreach (int columnIndex in _selectedBandIndexes)
                            {
                                foreach (DataGridViewRow dataGridViewRow in Rows)   // unshares all rows!
                                {
                                    stcc.Add(dataGridViewRow.Cells[columnIndex]);
                                }
                            }
                            break;
                        }

                    case DataGridViewSelectionMode.FullRowSelect:
                    case DataGridViewSelectionMode.RowHeaderSelect:
                        {
                            foreach (int rowIndex in _selectedBandIndexes)
                            {
                                DataGridViewRow dataGridViewRow = (DataGridViewRow)Rows[rowIndex];
                                foreach (DataGridViewCell dataGridViewCell in dataGridViewRow.Cells)
                                {
                                    stcc.Add(dataGridViewCell);
                                }
                            }
                            break;
                        }
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
                        foreach (int columnIndex in _selectedBandIndexes)
                        {
                            strc.Add(Columns[columnIndex]);
                        }

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
                        foreach (int rowIndex in _selectedBandIndexes)
                        {
                            strc.Add(Rows[rowIndex]);
                        }
                        break;
                }

                return strc;
            }
        }
        public bool NativeRowGetSelected(int rowindex)
        {
            switch (SelectionMode)
            {
                case DataGridViewSelectionMode.FullRowSelect:
                case DataGridViewSelectionMode.RowHeaderSelect:
                    return _selectedBandIndexes.Any(i => i == rowindex);
                    break;
                default:
                    return false;
            }
        }
        public void NativeRowSetSelected(int rowindex,bool selected)
        {
            Gtk.Application.Invoke(delegate
            {
                if (selected)
                    GridView.Selection.SelectIter(Rows[rowindex].TreeIter);
                else
                    GridView.Selection.UnselectIter(Rows[rowindex].TreeIter);
            });
        }

        public override void BeginInit()
        {
            _Created = false;
        }

        public override void EndInit()
        {
            _Created = true;
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
        [Obsolete("此事件未实现，自行开发")]
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
        [Obsolete("此事件未实现，自行开发")]
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
        [Obsolete("此事件未实现，自行开发")]
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
