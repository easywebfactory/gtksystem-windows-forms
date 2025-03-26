/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GLib;
using Gtk;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class DataGridView : ScrollableControl
{
    public readonly DataGridViewBase self = new();
    public override object GtkControl => self;
    private DataGridViewColumnCollection _columns = null!;
    private DataGridViewRowCollection _rows = null!;
    private ControlBindingsCollection _collect = null!;
    internal TreeStore Store = new(typeof(DataGridViewCell));

    public Gtk.TreeView GridView => self.GridView;

    public DataGridView()
    {
        Init();
    }

    private void Init()
    {
        BorderStyle = BorderStyle.FixedSingle;
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
        if (GridView != null)
        {
            GridView.Realized += GridView_Realized;
            GridView.RowActivated += GridView_RowActivated;
            GridView.Selection.Changed += Selection_Changed;
        }
    }

    private readonly List<int> _selectedBandIndexes = new();
    private void Selection_Changed(object? sender, EventArgs e)
    {
        _selectedBandIndexes.Clear();
        var treePaths = GridView.Selection.GetSelectedRows();
        foreach (var path in treePaths)
        {
            var idx = path.Indices.Last();
            _selectedBandIndexes.Add(idx);
        }
        if (SelectionChanged != null && Created)
            OnSelectionChanged(e);
    }

    protected virtual void OnSelectionChanged(EventArgs e)
    {
        SelectionChanged?.Invoke(this, e);
    }

    private void GridView_RowActivated(object o, RowActivatedArgs args)
    {
        // Single row selection is valid
        if (CellClick != null)
        {
            if (args.Column is DataGridViewColumn column)
            {
                CellClick(this, new DataGridViewCellEventArgs(column.Index, args.Path.Indices.Last()));
            }
        }
    }

    private void GridView_Realized(object? sender, EventArgs e)
    {
        OnSetDataSource();
        _columns.Invalidate();
        if (DataBindings != null)
        {
            foreach (Binding binding in DataBindings)
                GridView.AddNotification(binding.PropertyName, propertyNotity);
        }
    }

    private void propertyNotity(object o, NotifyArgs args)
    {
        var binding = DataBindings[args.Property];
        binding?.WriteValue();
    }
    public event EventHandler? SelectionChanged;
    public event DataGridViewCellEventHandler? CellClick;
    internal void CellValueChanagedHandler(int column, int row)
    {
        if (CellValueChanged != null)
        {
            CellValueChanged(this, new DataGridViewCellEventArgs(column, row));
        }
    }
    public event DataGridViewCellEventHandler? CellValueChanged;
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
                if (_MultiSelect)
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
                {
                    GridView.Selection.Mode = Gtk.SelectionMode.None;
                    break;
                }

                case DataGridViewSelectionMode.FullColumnSelect:
                case DataGridViewSelectionMode.ColumnHeaderSelect:
                {
                    GridView.Selection.Mode = Gtk.SelectionMode.Multiple;
                    break;
                }

                case DataGridViewSelectionMode.FullRowSelect:
                case DataGridViewSelectionMode.RowHeaderSelect:
                {
                    GridView.Selection.Mode = Gtk.SelectionMode.Multiple;
                    break;
                }
            }

        }
    }
    public string Markup { get; set; } = "...";
    public bool ReadOnly { get; set; }
    public int RowHeadersWidth { get; set; }
    public int ColumnHeadersHeight { get; set; }
    public DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
    {
        get; set;
    }
    public DataGridViewAutoSizeRowsMode AutoSizeRowsMode { get; set; }
    private DataGridViewRow? _RowTemplate;
    public DataGridViewRow? RowTemplate
    {
        get { return _RowTemplate ??= new DataGridViewRow(); }
        set => _RowTemplate = value;
    }
    public DataGridViewCellStyle? DefaultCellStyle { get; set; }
    public DataGridViewCellStyle? ColumnHeadersDefaultCellStyle { get; set; }
    public DataGridViewCellStyle? AlternatingRowsDefaultCellStyle { get; set; }
    public DataGridViewCellStyle? RowsDefaultCellStyle { get; set; }
    public DataGridViewCellStyle? RowHeadersDefaultCellStyle { get; set; }
    public override ControlBindingsCollection DataBindings => _collect;
    private object? _DataSource;
#pragma warning disable CS0414 // Field is assigned but its value is never used
    private bool _Created;
#pragma warning restore CS0414 // Field is assigned but its value is never used

    public object? DataSource
    {
        get => _DataSource;
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
    public string? DataMember { get; set; }
    private void LoadDataTableSource(DataTable dt)
    {
        foreach (DataColumn col in dt.Columns)
        {
            if (_columns.Exists(m => m.DataPropertyName == col.ColumnName) == false)
            {
                if (col.DataType.Name == "Boolean")
                    _columns.Add(new DataGridViewCheckBoxColumn(this) { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
                else if (col.DataType.Name == "Image" || col.DataType.Name == "Bitmap")
                    _columns.Add(new DataGridViewImageColumn(this) { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
                else
                    _columns.Add(new DataGridViewColumn(this) { Name = col.ColumnName, HeaderText = col.ColumnName, DataPropertyName = col.ColumnName, ValueType = col.DataType });
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
                        var cellvalue = dt.Columns.Contains(col.DataPropertyName) ? dr[col.DataPropertyName] : null;
                        newRow.Cells.Add(col.NewCell(cellvalue, col.ValueType));
                    }
                    _rows.Add(newRow);
                }
            }
        }
        private void LoadListSource()
        {
            var _type = _DataSource.GetType();
            var _entityType = _type.GetGenericArguments();
            if (_entityType.Length == 1)
            {
                var pros = _entityType[0].GetProperties(BindingFlags.Public|BindingFlags.Instance);
                foreach (var pro in pros)
                {
                    if (_columns.Exists(m => m.DataPropertyName == pro.Name) == false)
                    {
                        if (pro.PropertyType.Name == "Boolean")
                            _columns.Add(new DataGridViewCheckBoxColumn(this) { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                        else if (pro.PropertyType.Name == "Image" || pro.PropertyType.Name == "Bitmap")
                            _columns.Add(new DataGridViewImageColumn(this) { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                        else
                            _columns.Add(new DataGridViewColumn(this) { Name = pro.Name, HeaderText = pro.Name, DataPropertyName = pro.Name, ValueType = pro.PropertyType });
                    }
                }
                _columns.Invalidate();

            if (_columns.Count > 0)
            {
                var enumerator = ((IEnumerable?)_DataSource)?.GetEnumerator();
                using var disposable = enumerator as IDisposable;
                while (enumerator != null && enumerator.MoveNext())
                {
                    var obj = enumerator.Current;
                    var type = obj?.GetType();
                    var newRow = new DataGridViewRow();
                    foreach (var col in _columns)
                    {
                        var cellvalue = type?.GetProperty(col.DataPropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)?.GetValue(obj);
                        newRow.Cells.Add(col.NewCell(cellvalue, col.ValueType));
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
                    var cols = Store.NColumns;
                    Store.Foreach((model, _, iter) =>
                    {
                        for (var i = 0; i < cols; i++)
                        {
                            var cell = (DataGridViewCell)model.GetValue(iter, i);
                            if (cell.Selected)
                                stcc.Add(cell);
                        }
                        return false;
                    });
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
        _Created = true;
    }
    public void ClearSelection()
    {
        GridView.Selection.UnselectAll();
    }
    public void OnCellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (CellPainting != null)
            CellPainting(sender, e);
    }
#pragma warning disable CS0067 // Event is never used
    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellValueEventHandler? CellValuePushed;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewColumnEventHandler? ColumnHeaderCellChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellValueEventHandler? CellValueNeeded;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewEditingControlShowingEventHandler? EditingControlShowing;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowEventHandler? RowContextMenuStripChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowEventHandler? RowUnshared;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowStateChangedEventHandler? RowStateChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowsRemovedEventHandler? RowsRemoved;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowsAddedEventHandler? RowsAdded;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowPrePaintEventHandler? RowPrePaint;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowPostPaintEventHandler? RowPostPaint;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowEventHandler? RowMinimumHeightChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? RowLeave;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowHeightInfoPushedEventHandler? RowHeightInfoPushed;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowHeightInfoNeededEventHandler? RowHeightInfoNeeded;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowEventHandler? NewRowNeeded;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowEventHandler? RowHeightChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellMouseEventHandler? RowHeaderMouseDoubleClick;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellMouseEventHandler? RowHeaderMouseClick;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowErrorTextNeededEventHandler? RowErrorTextNeeded;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowEventHandler? RowErrorTextChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? RowEnter;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowEventHandler? RowDividerHeightChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowDividerDoubleClickEventHandler? RowDividerDoubleClick;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event QuestionEventHandler? RowDirtyStateNeeded;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowEventHandler? RowDefaultCellStyleChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowContextMenuStripNeededEventHandler? RowContextMenuStripNeeded;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowEventHandler? RowHeaderCellChanged;

    //[Obsolete("This event is not implemented and is developed by ourselves.")]
    //public event DataGridViewCellEventHandler CellValueChanged;
    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellValidatingEventHandler? CellValidating;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellValidated;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event QuestionEventHandler? CancelRowEdit;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewAutoSizeColumnModeEventHandler? AutoSizeColumnModeChanged;

    //[Obsolete("This event is not implemented and is developed by ourselves.")]
    //public event EventHandler TextChanged;
    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? RowsDefaultCellStyleChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewAutoSizeModeEventHandler? RowHeadersWidthSizeModeChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? RowHeadersWidthChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? RowHeadersDefaultCellStyleChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? RowHeadersBorderStyleChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? ReadOnlyChanged;

    //[Obsolete("This event is not implemented and is developed by ourselves.")]
    //public event EventHandler PaddingChanged;
    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellCancelEventHandler? CellBeginEdit;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? MultiSelectChanged;

    //[Obsolete("This event is not implemented and is developed by ourselves.")]
    //public event EventHandler FontChanged;
    //[Obsolete("This event is not implemented and is developed by ourselves.")]
    //public event EventHandler ForeColorChanged;
    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? EditModeChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? DefaultCellStyleChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? DataSourceChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? DataMemberChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewAutoSizeModeEventHandler? ColumnHeadersHeightSizeModeChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? ColumnHeadersHeightChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? ColumnHeadersDefaultCellStyleChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? ColumnHeadersBorderStyleChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? GridColorChanged;

    //[Obsolete("This event is not implemented and is developed by ourselves.")]
    //public event DataGridViewCellEventHandler CellClick;
    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellContentClick;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellContentDoubleClick;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellToolTipTextNeededEventHandler? CellToolTipTextNeeded;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellToolTipTextChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellStyleContentChangedEventHandler? CellStyleContentChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellStyleChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellStateChangedEventHandler? CellStateChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellParsingEventHandler? CellParsing;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellPaintingEventHandler? CellPainting;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellMouseEventHandler? CellMouseUp;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellMouseEventHandler? CellMouseMove;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellMouseLeave;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellMouseEnter;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellMouseEventHandler? CellMouseDown;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellMouseEventHandler? CellMouseDoubleClick;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellMouseEventHandler? CellMouseClick;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellLeave;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellFormattingEventHandler? CellFormatting;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellErrorTextNeededEventHandler? CellErrorTextNeeded;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellErrorTextChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellEnter;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellEndEdit;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellDoubleClick;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellContextMenuStripNeededEventHandler? CellContextMenuStripNeeded;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? CellContextMenuStripChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event EventHandler? BorderStyleChanged;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellEventHandler? RowValidated;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewCellCancelEventHandler? RowValidating;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowCancelEventHandler? UserDeletingRow;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowEventHandler? UserDeletedRow;

    [Obsolete("This event is not implemented and is developed by ourselves.")]
    public event DataGridViewRowEventHandler? UserAddedRow;

    public event EventHandler<DataGridViewColumnEventArgs>? ColumnNameChanged;
}