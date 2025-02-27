/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using GLib;
using Gtk;
using System.Collections;
using System.ComponentModel;
using System.Data;

#pragma warning disable CS0067 // Event is never used

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class DataGridView : ScrollableControl
{
    public readonly DataGridViewBase self = new();
    public override object GtkControl => self;
    private readonly DataGridViewColumnCollection _columns;
    private readonly DataGridViewRowCollection _rows;
    private readonly ControlBindingsCollection? _collect;
    internal TreeStore store = new(typeof(CellValue));
    public Gtk.TreeView? GridView => self.gridView;

    public DataGridView()
    {
        BorderStyle = BorderStyle.FixedSingle;
        if (GridView != null)
        {
            GridView.Margin = 0;
            GridView.MarginStart = 0;
            GridView.MarginEnd = 0;
            GridView.Selection.Mode = Gtk.SelectionMode.Multiple;
            GridView.HeadersClickable = true;
            GridView.HeadersVisible = true;
            GridView.ActivateOnSingleClick = false;
        }

        _columns = new DataGridViewColumnCollection(this);
        _rows = new DataGridViewRowCollection(this);
        _collect = new ControlBindingsCollection(this);
        if (GridView != null)
        {
            GridView.Realized += GridView_Realized;
            GridView.RowActivated += GridView_RowActivated;
            GridView.Selection.Changed += Selection_Changed;
        }
    }
    private readonly List<int> _selectedBandIndexes = [];
    private void Selection_Changed(object? sender, EventArgs e)
    {
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _selectedBandIndexes.Clear();
        if (GridView != null)
        {
            var treePaths = GridView.Selection.GetSelectedRows();
            foreach (var path in treePaths)
            {
                var idx = path.Indices.Last();
                _selectedBandIndexes.Add(idx);
            }
        }

        if (SelectionChanged != null && Created)
            SelectionChanged?.Invoke(this, e);
    }

    private void GridView_RowActivated(object? o, RowActivatedArgs args)
    {
        //单行选择有效
        if (CellClick != null)
        {
            var column = args.Column as DataGridViewColumn;
            CellClick?.Invoke(this, new DataGridViewCellEventArgs(column?.Index??-1, args.Path.Indices[0]));
        }
    }

    private void GridView_Realized(object? sender, EventArgs e)
    {
        OnSetDataSource();
        _columns.Invalidate();
        if (DataBindings != null)
        {
            foreach (Binding binding in DataBindings)
            {
                GridView?.AddNotification(binding.PropertyName, PropertyNotity);
            }
        }
    }
    private void PropertyNotity(object? o, NotifyArgs args)
    {
        var binding = DataBindings?[args.Property];
        binding?.WriteValue();
    }
    public event EventHandler? SelectionChanged;
    public event DataGridViewCellEventHandler? CellClick;
    internal void CellValueChanagedHandler(int column, int row, CellValue val)
    {
        var cells = _rows[row].Cells;
        if (cells.Count > column)
        {
            cells[column].Value = val?.Text;
        }

        CellValueChanged?.Invoke(this, new DataGridViewCellEventArgs(column, row));
    }
    public event DataGridViewCellEventHandler? CellValueChanged;
    public void SetExpandRow(DataGridViewRow row, bool all)
    {
        GridView?.ExpandRow(store.GetPath(row.TreeIter), all);
    }
    public void SetCollapseRow(DataGridViewRow row)
    {
        GridView?.CollapseRow(store.GetPath(row.TreeIter));
    }
    public bool MultiSelect
    {
        get => !(GridView?.ActivateOnSingleClick??true);
        set
        {
            var gridView = GridView;
            if (gridView != null)
            {
                gridView.ActivateOnSingleClick = !value;
            }
        }
    }

    public DataGridViewSelectionMode SelectionMode { get; set; }
    public string Markup { get; set; } = "...";
    public bool ReadOnly { get; set; }
    public int RowHeadersWidth { get; set; }
    public int ColumnHeadersHeight { get; set; }
    public DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
    {
        get; set;
    }
    public DataGridViewAutoSizeRowsMode AutoSizeRowsMode { get; set; }
    private DataGridViewRow? rowTemplate;
    public DataGridViewRow? RowTemplate
    {
        get { return rowTemplate ??= new DataGridViewRow(); }
        set => rowTemplate = value;
    }
    public DataGridViewCellStyle? DefaultCellStyle { get; set; }
    public DataGridViewCellStyle? ColumnHeadersDefaultCellStyle { get; set; }
    public DataGridViewCellStyle? AlternatingRowsDefaultCellStyle { get; set; }
    public DataGridViewCellStyle? RowsDefaultCellStyle { get; set; }
    public DataGridViewCellStyle? RowHeadersDefaultCellStyle { get; set; }
    public override ControlBindingsCollection? DataBindings => _collect;
    private object? dataSource;
    public object? DataSource
    {
        get => dataSource;
        set
        {
            dataSource = value;
            if (GridView?.IsVisible??false)
            {
                OnSetDataSource();
            }
        }
    }
    private void OnSetDataSource()
    {
        created = false;
        _rows.Clear();
        if (dataSource != null)
        {
            store = new TreeStore(Array.ConvertAll(GridView?.Columns ?? [], _ => typeof(CellValue)));
            if (GridView != null)
            {
                GridView.Model = store;
            }

            if (dataSource is DataTable dtable)
            {
                LoadDataTableSource(dtable);
            }
            else if (dataSource is DataSet dset)
            {
                if (string.IsNullOrEmpty(DataMember))
                    LoadDataTableSource(dset.Tables[0]);
                else
                    LoadDataTableSource(dset.Tables[DataMember]);
            }
            else if (dataSource is DataView dview)
            {
                LoadDataTableSource(dview.Table);
            }
            else
            {
                LoadListSource();
            }
        }
        created = true;
    }
    public string? DataMember { get; set; }
    private void LoadDataTableSource(DataTable dt)
    {
        foreach (DataColumn col in dt.Columns)
        {
            if (_columns.Exists(m => m.DataPropertyName == col.ColumnName) == false)
            {
                if (col.DataType.Name == "Boolean")
                    _columns.Add(new DataGridViewCheckBoxColumn { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
                else if (col.DataType.Name == "Image" || col.DataType.Name == "Bitmap")
                    _columns.Add(new DataGridViewImageColumn { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
                else
                    _columns.Add(new DataGridViewColumn { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
            }
        }
        _columns.Invalidate();

        if (_columns.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                var newRow = new DataGridViewRow();
                foreach (var col in _columns)
                {
                    var cellvalue = dt.Columns.Contains(col.DataPropertyName ?? string.Empty) ? dr[col.DataPropertyName??string.Empty] : null;
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
                _rows.Add(newRow);
            }
        }
    }
    private void LoadListSource()
    {
        var _type = dataSource?.GetType();
        var _entityType = _type?.GetGenericArguments();
        if (_entityType?.Length == 1)
        {
            var pros = _entityType[0].GetProperties();
            foreach (var pro in pros)
            {
                if (_columns.Exists(m => m.DataPropertyName == pro.Name) == false)
                {
                    if (pro.PropertyType.Name == "Boolean")
                        _columns.Add(new DataGridViewCheckBoxColumn { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                    else if (pro.PropertyType.Name == "Image" || pro.PropertyType.Name == "Bitmap")
                        _columns.Add(new DataGridViewImageColumn { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                    else
                        _columns.Add(new DataGridViewColumn { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                }
            }
            _columns.Invalidate();

            if (_columns.Count > 0)
            {
                var readerValue = (dataSource as IEnumerable)?.GetEnumerator();
                using var disposable = readerValue as IDisposable;
                while (readerValue != null && readerValue.MoveNext())
                {
                    var obj = readerValue.Current;
                    Dictionary<string, object> values = new();
                    Array.ForEach(obj?.GetType().GetProperties() ?? [], o => { values.Add(o.Name, o.GetValue(obj)); });
                    var newRow = new DataGridViewRow();
                    foreach (var col in _columns)
                    {
                        values.TryGetValue(col.DataPropertyName ?? string.Empty, out var cellvalue);
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
                    _rows.Add(newRow);
                }
            }
        }
    }
    public DataGridViewColumnCollection Columns => _columns;
    public DataGridViewRowCollection Rows => _rows;

    [Browsable(false)]
    public DataGridViewSelectedCellCollection SelectedCells
    {
        get
        {
            var stcc = new DataGridViewSelectedCellCollection();
            switch (SelectionMode)
            {
                case DataGridViewSelectionMode.CellSelect:
                    {

                        break;
                    }

                case DataGridViewSelectionMode.FullColumnSelect:
                case DataGridViewSelectionMode.ColumnHeaderSelect:
                    {
                        foreach (var columnIndex in _selectedBandIndexes)
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
                        foreach (var rowIndex in _selectedBandIndexes)
                        {
                            var dataGridViewRow = Rows[rowIndex];
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
            var strc = new DataGridViewSelectedColumnCollection();
            switch (SelectionMode)
            {
                case DataGridViewSelectionMode.CellSelect:
                case DataGridViewSelectionMode.FullRowSelect:
                case DataGridViewSelectionMode.RowHeaderSelect:
                    break;
                case DataGridViewSelectionMode.FullColumnSelect:
                case DataGridViewSelectionMode.ColumnHeaderSelect:
                    foreach (var columnIndex in _selectedBandIndexes)
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
            var strc = new DataGridViewSelectedRowCollection();
            switch (SelectionMode)
            {
                case DataGridViewSelectionMode.CellSelect:
                case DataGridViewSelectionMode.FullColumnSelect:
                case DataGridViewSelectionMode.ColumnHeaderSelect:
                    break;
                case DataGridViewSelectionMode.FullRowSelect:
                case DataGridViewSelectionMode.RowHeaderSelect:
                    foreach (var rowIndex in _selectedBandIndexes)
                    {
                        strc.Add(Rows[rowIndex]);
                    }
                    break;
            }

            return strc;
        }
    }

    public int ColumnCount => Columns.Count;
    public int RowCount => Rows.SharedList.Count;

    public bool NativeRowGetSelected(int rowindex)
    {
        switch (SelectionMode)
        {
            case DataGridViewSelectionMode.FullRowSelect:
            case DataGridViewSelectionMode.RowHeaderSelect:
                return _selectedBandIndexes.Any(i => i == rowindex);
            default:
                return false;
        }
    }
    public void NativeRowSetSelected(int rowindex, bool selected)
    {
        Gtk.Application.Invoke(delegate
        {
            if (selected)
                GridView?.Selection.SelectIter(Rows[rowindex].TreeIter);
            else
                GridView?.Selection.UnselectIter(Rows[rowindex].TreeIter);
        });
    }

    public override void BeginInit()
    {
        created = false;
    }

    public override void EndInit()
    {
        created = true;
    }
    public void ClearSelection()
    {
        GridView?.Selection.UnselectAll();
    }

    //[Obsolete("此事件未实现，自行开发")]
    //public event EventHandler? BackgroundImageChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? BackgroundColorChanged;
    //[Obsolete("此事件未实现，自行开发")]
    //public event EventHandler? BackColorChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewAutoSizeModeEventHandler? AutoSizeRowsModeChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewAutoSizeColumnsModeEventHandler? AutoSizeColumnsModeChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? AutoGenerateColumnsChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? AlternatingRowsDefaultCellStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? AllowUserToResizeRowsChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? AllowUserToResizeColumnsChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? AllowUserToDeleteRowsChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? AllowUserToAddRowsChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? Sorted;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewSortCompareEventHandler? SortCompare;
    //[Obsolete("此事件未实现，自行开发")]
    //public event EventHandler? SelectionChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? AllowUserToOrderColumnsChanged;
    //[Obsolete("此事件未实现，自行开发")]
    //public event EventHandler? StyleChanged;
    //[Obsolete("此事件未实现，自行开发")]
    //public event EventHandler? BackgroundImageLayoutChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? CellBorderStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewDataErrorEventHandler? DataError;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewBindingCompleteEventHandler? DataBindingComplete;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? CurrentCellDirtyStateChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? CurrentCellChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnWidthChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnToolTipTextChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnStateChangedEventHandler? ColumnStateChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnSortModeChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnRemoved;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnNameChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? DefaultValuesNeeded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnMinimumWidthChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellMouseEventHandler? ColumnHeaderMouseDoubleClick;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellMouseEventHandler? ColumnHeaderMouseClick;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnDividerWidthChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnDividerDoubleClickEventHandler? ColumnDividerDoubleClick;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnDisplayIndexChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnDefaultCellStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnDataPropertyNameChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnContextMenuStripChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnAdded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellValueEventHandler? CellValuePushed;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewColumnEventHandler? ColumnHeaderCellChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellValueEventHandler? CellValueNeeded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewEditingControlShowingEventHandler? EditingControlShowing;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? RowContextMenuStripChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? RowUnshared;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowStateChangedEventHandler? RowStateChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowsRemovedEventHandler? RowsRemoved;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowsAddedEventHandler? RowsAdded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowPrePaintEventHandler? RowPrePaint;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowPostPaintEventHandler? RowPostPaint;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? RowMinimumHeightChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? RowLeave;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowHeightInfoPushedEventHandler? RowHeightInfoPushed;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowHeightInfoNeededEventHandler? RowHeightInfoNeeded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? NewRowNeeded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? RowHeightChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellMouseEventHandler? RowHeaderMouseDoubleClick;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellMouseEventHandler? RowHeaderMouseClick;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowErrorTextNeededEventHandler? RowErrorTextNeeded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? RowErrorTextChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? RowEnter;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? RowDividerHeightChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowDividerDoubleClickEventHandler? RowDividerDoubleClick;
    [Obsolete("此事件未实现，自行开发")]
    public event QuestionEventHandler? RowDirtyStateNeeded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? RowDefaultCellStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowContextMenuStripNeededEventHandler? RowContextMenuStripNeeded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? RowHeaderCellChanged;
    //[Obsolete("此事件未实现，自行开发")]
    //public event DataGridViewCellEventHandler? CellValueChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellValidatingEventHandler? CellValidating;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellValidated;
    [Obsolete("此事件未实现，自行开发")]
    public event QuestionEventHandler? CancelRowEdit;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewAutoSizeColumnModeEventHandler? AutoSizeColumnModeChanged;
    //[Obsolete("此事件未实现，自行开发")]
    //public event EventHandler? TextChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? RowsDefaultCellStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewAutoSizeModeEventHandler? RowHeadersWidthSizeModeChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? RowHeadersWidthChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? RowHeadersDefaultCellStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? RowHeadersBorderStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? ReadOnlyChanged;
    //[Obsolete("此事件未实现，自行开发")]
    //public event EventHandler? PaddingChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellCancelEventHandler? CellBeginEdit;
    [Obsolete("此事件未实现，此事件未实现，自行开发")]
    public event EventHandler? MultiSelectChanged;
    //[Obsolete("此事件未实现，自行开发")]
    //public event EventHandler? FontChanged;
    //[Obsolete("此事件未实现，自行开发")]
    //public event EventHandler? ForeColorChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? EditModeChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? DefaultCellStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? DataSourceChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? DataMemberChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewAutoSizeModeEventHandler? ColumnHeadersHeightSizeModeChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? ColumnHeadersHeightChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? ColumnHeadersDefaultCellStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? ColumnHeadersBorderStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? GridColorChanged;
    //[Obsolete("此事件未实现，自行开发")]
    //public event DataGridViewCellEventHandler? CellClick;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellContentClick;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellContentDoubleClick;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellToolTipTextNeededEventHandler? CellToolTipTextNeeded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellToolTipTextChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellStyleContentChangedEventHandler? CellStyleContentChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellStateChangedEventHandler? CellStateChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellParsingEventHandler? CellParsing;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellPaintingEventHandler? CellPainting;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellMouseEventHandler? CellMouseUp;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellMouseEventHandler? CellMouseMove;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellMouseLeave;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellMouseEnter;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellMouseEventHandler? CellMouseDown;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellMouseEventHandler? CellMouseDoubleClick;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellMouseEventHandler? CellMouseClick;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellLeave;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellFormattingEventHandler? CellFormatting;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellErrorTextNeededEventHandler? CellErrorTextNeeded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellErrorTextChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellEnter;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellEndEdit;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellDoubleClick;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellContextMenuStripNeededEventHandler? CellContextMenuStripNeeded;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? CellContextMenuStripChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event EventHandler? BorderStyleChanged;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellEventHandler? RowValidated;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewCellCancelEventHandler? RowValidating;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowCancelEventHandler? UserDeletingRow;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? UserDeletedRow;
    [Obsolete("此事件未实现，自行开发")]
    public event DataGridViewRowEventHandler? UserAddedRow;

}