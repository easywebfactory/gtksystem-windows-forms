/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
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
        public Gtk.TreeView GridView { get { return self.GridView; } }
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
            _columns.Invalidate();
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
            _rows.Clear();
            if (_DataSource != null)
            {
                Store = new Gtk.TreeStore(Array.ConvertAll(GridView.Columns, o => typeof(CellValue)));
                GridView.Model = Store;
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
                if (_columns.Exists(m => m.DataPropertyName == col.ColumnName) == false)
                {
                    if (col.DataType.Name == "Boolean")
                        _columns.Add(new DataGridViewCheckBoxColumn() { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
                    else if (col.DataType.Name == "Image" || col.DataType.Name == "Bitmap")
                        _columns.Add(new DataGridViewImageColumn() { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
                    else
                        _columns.Add(new DataGridViewColumn() { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
                }
            }
            _columns.Invalidate();

            if (_columns.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    foreach (DataGridViewColumn col in _columns)
                    {
                        object cellvalue = dt.Columns.Contains(col.DataPropertyName ?? string.Empty) ? dr[col.DataPropertyName] : null;
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
                PropertyInfo[] pros = _entityType[0].GetProperties();
                foreach (PropertyInfo pro in pros)
                {
                    if (_columns.Exists(m => m.DataPropertyName == pro.Name) == false)
                    {
                        if (pro.PropertyType.Name == "Boolean")
                            _columns.Add(new DataGridViewCheckBoxColumn() { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                        else if (pro.PropertyType.Name == "Image" || pro.PropertyType.Name == "Bitmap")
                            _columns.Add(new DataGridViewImageColumn() { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                        else
                            _columns.Add(new DataGridViewColumn() { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                    }
                }
                _columns.Invalidate();

                if (_columns.Count > 0)
                {
                    IEnumerator reader = ((IEnumerable)_DataSource).GetEnumerator();
                    while (reader.MoveNext())
                    {
                        object obj = reader.Current;
                        Dictionary<string, object> values = new Dictionary<string, object>();
                        Array.ForEach(obj.GetType().GetProperties(), o => { values.Add(o.Name, o.GetValue(obj)); });
                        DataGridViewRow newRow = new DataGridViewRow();
                        foreach (DataGridViewColumn col in _columns)
                        {
                            values.TryGetValue(col.DataPropertyName ?? string.Empty, out object cellvalue);
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
        public void ClearSelection()
        {
            GridView.Selection.UnselectAll();
        }

        //[Obsolete("This event is not implemented and is developed by ourselves.")]
        //public event EventHandler BackgroundImageChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler BackgroundColorChanged;
        //[Obsolete("This event is not implemented and is developed by ourselves.")]
        //public event EventHandler BackColorChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewAutoSizeModeEventHandler AutoSizeRowsModeChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewAutoSizeColumnsModeEventHandler AutoSizeColumnsModeChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler AutoGenerateColumnsChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler AlternatingRowsDefaultCellStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler AllowUserToResizeRowsChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler AllowUserToResizeColumnsChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler AllowUserToDeleteRowsChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler AllowUserToAddRowsChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler Sorted;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewSortCompareEventHandler SortCompare;
        //[Obsolete("This event is not implemented and is developed by ourselves.")]
        //public event EventHandler SelectionChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler AllowUserToOrderColumnsChanged;
        //[Obsolete("This event is not implemented and is developed by ourselves.")]
        //public event EventHandler StyleChanged;
        //[Obsolete("This event is not implemented and is developed by ourselves.")]
        //public event EventHandler BackgroundImageLayoutChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler CellBorderStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewDataErrorEventHandler DataError;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewBindingCompleteEventHandler DataBindingComplete;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler CurrentCellDirtyStateChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler CurrentCellChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnWidthChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnToolTipTextChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnStateChangedEventHandler ColumnStateChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnSortModeChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnRemoved;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnNameChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler DefaultValuesNeeded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnMinimumWidthChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellMouseEventHandler ColumnHeaderMouseDoubleClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellMouseEventHandler ColumnHeaderMouseClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnDividerWidthChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnDividerDoubleClickEventHandler ColumnDividerDoubleClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnDisplayIndexChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnDefaultCellStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnDataPropertyNameChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnContextMenuStripChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnAdded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellValueEventHandler CellValuePushed;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewColumnEventHandler ColumnHeaderCellChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellValueEventHandler CellValueNeeded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewEditingControlShowingEventHandler EditingControlShowing;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler RowContextMenuStripChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler RowUnshared;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowStateChangedEventHandler RowStateChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowsRemovedEventHandler RowsRemoved;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowsAddedEventHandler RowsAdded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowPrePaintEventHandler RowPrePaint;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowPostPaintEventHandler RowPostPaint;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler RowMinimumHeightChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler RowLeave;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowHeightInfoPushedEventHandler RowHeightInfoPushed;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowHeightInfoNeededEventHandler RowHeightInfoNeeded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler NewRowNeeded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler RowHeightChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellMouseEventHandler RowHeaderMouseDoubleClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellMouseEventHandler RowHeaderMouseClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowErrorTextNeededEventHandler RowErrorTextNeeded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler RowErrorTextChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler RowEnter;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler RowDividerHeightChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowDividerDoubleClickEventHandler RowDividerDoubleClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event QuestionEventHandler RowDirtyStateNeeded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler RowDefaultCellStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowContextMenuStripNeededEventHandler RowContextMenuStripNeeded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler RowHeaderCellChanged;
        //[Obsolete("This event is not implemented and is developed by ourselves.")]
        //public event DataGridViewCellEventHandler CellValueChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellValidatingEventHandler CellValidating;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellValidated;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event QuestionEventHandler CancelRowEdit;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewAutoSizeColumnModeEventHandler AutoSizeColumnModeChanged;
        //[Obsolete("This event is not implemented and is developed by ourselves.")]
        //public event EventHandler TextChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler RowsDefaultCellStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewAutoSizeModeEventHandler RowHeadersWidthSizeModeChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler RowHeadersWidthChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler RowHeadersDefaultCellStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler RowHeadersBorderStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler ReadOnlyChanged;
        //[Obsolete("This event is not implemented and is developed by ourselves.")]
        //public event EventHandler PaddingChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellCancelEventHandler CellBeginEdit;
        [Obsolete("此事件未实现，This event is not implemented and is developed by ourselves.")]
        public event EventHandler MultiSelectChanged;
        //[Obsolete("This event is not implemented and is developed by ourselves.")]
        //public event EventHandler FontChanged;
        //[Obsolete("This event is not implemented and is developed by ourselves.")]
        //public event EventHandler ForeColorChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler EditModeChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler DefaultCellStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler DataSourceChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler DataMemberChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewAutoSizeModeEventHandler ColumnHeadersHeightSizeModeChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler ColumnHeadersHeightChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler ColumnHeadersDefaultCellStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler ColumnHeadersBorderStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler GridColorChanged;
        //[Obsolete("This event is not implemented and is developed by ourselves.")]
        //public event DataGridViewCellEventHandler CellClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellContentClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellContentDoubleClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellToolTipTextNeededEventHandler CellToolTipTextNeeded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellToolTipTextChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellStyleContentChangedEventHandler CellStyleContentChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellStateChangedEventHandler CellStateChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellParsingEventHandler CellParsing;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellPaintingEventHandler CellPainting;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellMouseEventHandler CellMouseUp;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellMouseEventHandler CellMouseMove;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellMouseLeave;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellMouseEnter;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellMouseEventHandler CellMouseDown;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellMouseEventHandler CellMouseDoubleClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellMouseEventHandler CellMouseClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellLeave;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellFormattingEventHandler CellFormatting;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellErrorTextNeededEventHandler CellErrorTextNeeded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellErrorTextChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellEnter;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellEndEdit;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellDoubleClick;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellContextMenuStripNeededEventHandler CellContextMenuStripNeeded;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler CellContextMenuStripChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event EventHandler BorderStyleChanged;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellEventHandler RowValidated;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewCellCancelEventHandler RowValidating;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowCancelEventHandler UserDeletingRow;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler UserDeletedRow;
        [Obsolete("This event is not implemented and is developed by ourselves.")]
        public event DataGridViewRowEventHandler UserAddedRow;

    }
}
