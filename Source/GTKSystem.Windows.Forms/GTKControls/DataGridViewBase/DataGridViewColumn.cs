﻿/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

public class DataGridViewTextBoxColumn : DataGridViewColumn
{
    public DataGridViewTextBoxColumn() : base(new DataGridViewTextBoxCell())
    {
    }
    public DataGridViewTextBoxColumn(DataGridView? ownerGridView) : base(ownerGridView, new DataGridViewTextBoxCell())
    {
    }
}
public class DataGridViewCheckBoxColumn : DataGridViewColumn
{
    public DataGridViewCheckBoxColumn() : this(null)
    {
    }
    public DataGridViewCheckBoxColumn(DataGridView? ownerGridView) : base(0, ownerGridView, new DataGridViewCheckBoxCell())
    {
        ValueType = typeof(bool);

        var renderer = new CellRendererToggleValue(this);
        renderer.Editable = true;
        renderer.Mode = CellRendererMode.Activatable;
        renderer.Height = RowHeight;
        renderer.Toggled += CellName_Toggled;
        PackStart(renderer, true);
        SortMode = DataGridViewColumnSortMode.NotSortable;
        Sizing = TreeViewColumnSizing.GrowOnly;
        _cellRenderer = renderer;
    }

    private void CellName_Toggled(object o, ToggledArgs args)
    {
        var path = new TreePath(args.Path);
        var model = _treeView?.Model;
        if (model != null)
        {
            model.GetIter(out var iter, path);
            var cell = model.GetValue(iter, Index);
            if (cell is DataGridViewCell val)
            {
                var tggle = (CellRendererToggleValue)o;
                val.Value = tggle.Active == false;
                _gridview?.CellValueChanagedHandler(Index, path.Indices.Last());
            }
        }
    }
    internal override DataGridViewCell NewCell(object? value = null, Type? valueType = null)
    {
        var newcell = new DataGridViewCheckBoxCell();
        AtrributesClone(newcell);
        newcell.Value = value;
        newcell.ValueType = valueType;
        return newcell;
    }
}

public class DataGridViewRadioColumn : DataGridViewColumn
{
    public DataGridViewRadioColumn() : this(null)
    {
    }
    public DataGridViewRadioColumn(DataGridView? ownerGridView) : base(0, ownerGridView, new DataGridViewRadioCell())
    {
        ValueType = typeof(bool);
        var renderer = new CellRendererToggleValue(this);
        renderer.Editable = true;
        renderer.Mode = CellRendererMode.Activatable;
        renderer.Radio = true;
        renderer.Height = RowHeight;
        renderer.Toggled += CellName_Toggled;
        PackStart(renderer, true);
        SortMode = DataGridViewColumnSortMode.NotSortable;
        Sizing = TreeViewColumnSizing.GrowOnly;
        _cellRenderer = renderer;
    }

    private void CellName_Toggled(object o, ToggledArgs args)
    {
        var path = new TreePath(args.Path);
        var model = _treeView?.Model;
        if (model != null)
        {
            model.GetIter(out var iter, path);
            var cell = model.GetValue(iter, Index);
            if (cell is DataGridViewCell val)
            {
                var tggle = (CellRendererToggleValue)o;
                val.Value = tggle.Active == false;
                _gridview?.CellValueChanagedHandler(Index, path.Indices.Last());
            }
        }
    }
    internal override DataGridViewCell NewCell(object? value = null, Type? valueType = null)
    {
        var newcell = new DataGridViewRadioCell();
        AtrributesClone(newcell);
        newcell.Value = value;
        newcell.ValueType = valueType;
        return newcell;
    }
}

public class DataGridViewRadioCell : DataGridViewCell;

public class DataGridViewComboBoxColumn : DataGridViewColumn
{
    readonly ObjectCollection _items;
    internal ListStore modelInternal = new(typeof(string));
    public DataGridViewComboBoxColumn() : this(null)
    {
    }
    public DataGridViewComboBoxColumn(DataGridView? ownerGridView) : base(0, ownerGridView, new DataGridViewComboBoxCell())
    {
        _items = new ObjectCollection(this);
        var renderer = new CellRendererComboValue(this);
        renderer.Editable = true;
        renderer.Edited += Renderer_Edited;
        renderer.TextColumn = 0;
        renderer.Height = RowHeight;
        renderer.Model = modelInternal;
        PackStart(renderer, true);
        SortMode = DataGridViewColumnSortMode.NotSortable;
        Sizing = TreeViewColumnSizing.GrowOnly;
        _cellRenderer = renderer;
    }

    private void Renderer_Edited(object o, EditedArgs args)
    {
        var path = new TreePath(args.Path);
        var model = _treeView?.Model;
        if (model != null)
        {
            model.GetIter(out var iter, path);
            var cell = model.GetValue(iter, Index);
            if (cell is DataGridViewCell val)
            {
                val.Value = args.NewText;
                _gridview?.CellValueChanagedHandler(Index, path.Indices.Last());
            }
        }
    }
    internal override DataGridViewCell NewCell(object? value = null, Type? valueType = null)
    {
        var newcell = new DataGridViewComboBoxCell();
        AtrributesClone(newcell);
        newcell.Value = value;
        newcell.ValueType = valueType;
        return newcell;
    }
    public ObjectCollection Items => _items;
    public class ObjectCollection : ArrayList
    {
        private readonly DataGridViewComboBoxColumn _owner;
        public ObjectCollection(DataGridViewComboBoxColumn owner)
        {
            _owner = owner;
        }
        public void AddRange(object[] items)
        {
            foreach (var item in items)
                Add(item);
        }
        public override int Add(object? value)
        {
            _owner.modelInternal.AppendValues(value);
            return base.Add(value);
        }
    }
}

public class DataGridViewButtonColumn : DataGridViewColumn
{
    public DataGridViewButtonColumn() : this(null)
    {
    }
    public DataGridViewButtonColumn(DataGridView? ownerGridView) : base(0, ownerGridView, new DataGridViewButtonCell())
    {
        var renderer = new CellRendererButtonValue(this);
        renderer.Editable = false;
        renderer.Mode = CellRendererMode.Activatable;
        renderer.Height = RowHeight;
        renderer.SetAlignment(0.5f, 0.5f);
        PackStart(renderer, false);
        SortMode = DataGridViewColumnSortMode.NotSortable;
        Sizing = TreeViewColumnSizing.GrowOnly;
        _cellRenderer = renderer;
    }
    private void TreeView_RowActivated(object o, RowActivatedArgs args)
    {
        if (args.Column.Handle == Handle && args.Column.Cells[0] is CellRendererText)
        {
            var path = args.Path;
            _gridview?.CellValueChanagedHandler(Index, path.Indices.Last());
        }
    }
    internal override DataGridViewCell NewCell(object? value = null, Type? valueType = null)
    {
        var newcell = new DataGridViewButtonCell();
        AtrributesClone(newcell);
        newcell.Value = value;
        newcell.ValueType = valueType;
        return newcell;
    }
}
public class DataGridViewImageColumn : DataGridViewColumn
{
    public DataGridViewImageColumn() : this(null)
    {
    }
    public DataGridViewImageColumn(DataGridView? ownerGridView) : base(0, ownerGridView, new DataGridViewImageCell())
    {
        ValueType = typeof(Drawing.Image);
        var renderer = new CellRendererPixbufValue(this);
        //renderer.IconName = "face-smile";
        renderer.Height = RowHeight;
        PackStart(renderer, false);
        SortMode = DataGridViewColumnSortMode.NotSortable;
        Sizing = TreeViewColumnSizing.GrowOnly;
        _cellRenderer = renderer;
    }
    internal override DataGridViewCell NewCell(object? value = null, Type? valueType = null)
    {
        var newcell = new DataGridViewImageCell();
        AtrributesClone(newcell);
        newcell.Value = value;
        newcell.ValueType = valueType;
        return newcell;
    }
}
public class DataGridViewLinkColumn : DataGridViewColumn
{
    public DataGridViewLinkColumn() : this(null)
    {
    }
    public DataGridViewLinkColumn(DataGridView? ownerGridView) : base(0, ownerGridView, new DataGridViewLinkCell())
    {

    }
    internal override DataGridViewCell NewCell(object? value = null, Type? valueType = null)
    {
        var newcell = new DataGridViewLinkCell();
        AtrributesClone(newcell);
        newcell.Value = value;
        newcell.ValueType = valueType;
        return newcell;
    }
}

public class DataGridViewColumn : TreeViewColumn
{
    public int Column { get; }
    internal DataGridViewCell? _cellTemplate;
    internal Gtk.TreeView? _treeView;
    internal DataGridView? _gridview;
    internal ICellRenderer? _cellRenderer;
    protected DataGridViewColumn(int column, DataGridView? ownerGridView, DataGridViewCell? cellTemplate)
    {
        Column = column;
        if (ownerGridView != null)
        {
            _gridview = ownerGridView;
            _treeView = ownerGridView.GridView;
        }
        _cellTemplate = cellTemplate;
    }
    public DataGridViewColumn(DataGridView? ownerGridView) : this(ownerGridView, new DataGridViewTextBoxCell())
    {
    }

    public DataGridViewColumn() : this(null, new DataGridViewTextBoxCell())
    {
    }
    public DataGridViewColumn(DataGridViewCell? cellTemplate) : this(null, cellTemplate)
    {
    }

    public DataGridViewColumn(DataGridView? ownerGridView, DataGridViewCell? cellTemplate) : this(0, ownerGridView, cellTemplate)
    {
        var renderer = new CellRendererValue(this);
        renderer.Editable = true;
        renderer.Mode = CellRendererMode.Editable;
        renderer.Edited += Renderer_Edited;
        renderer.PlaceholderText = "---";
        renderer.Height = RowHeight;
        PackStart(renderer, true);
        Sizing = TreeViewColumnSizing.GrowOnly;
        _cellRenderer = renderer;
    }

    private void Renderer_Edited(object o, EditedArgs args)
    {
        var path = new TreePath(args.Path);
        var model = _treeView?.Model;
        if (model != null)
        {
            model.GetIter(out var iter, path);
            var cell = model.GetValue(iter, Index);
            if (cell is DataGridViewCell val)
            {
                val.Value = args.NewText;
                _gridview?.CellValueChanagedHandler(Index, path.Indices.Last());
            }
        }
    }

    internal virtual DataGridViewCell NewCell(object? value = null, Type? valueType = null)
    {
        var newcell = new DataGridViewTextBoxCell();
        AtrributesClone(newcell);
        newcell.Value = value;
        newcell.ValueType = valueType;
        return newcell;
    }
    internal void AtrributesClone(DataGridViewCell newcell)
    {
        if (_cellTemplate != null)
        {
            newcell.Style = _cellTemplate.Style?.Clone();
            newcell.ReadOnly = _cellTemplate.ReadOnly;
        }
    }
    public void SetGridViewDefaultStyle(DataGridViewCellStyle? cellStyle)
    {
        if (cellStyle is { WrapMode: DataGridViewTriState.True })
        {
            if (_cellRenderer != null)
            {
                _cellRenderer.WrapMode = Pango.WrapMode.WordChar;
                _cellRenderer.WrapWidth = 0;
                _cellRenderer.WidthChars = 0;
            }
        }
        if (_gridview != null)
        {
            if (_gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.AllCells || _gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.DisplayedCells || _gridview.RowTemplate?.Resizable == DataGridViewTriState.True)
            {

            }
            else
            {
                if (_cellRenderer != null)
                {
                    _cellRenderer.Height = RowHeight;
                }
            }
        }
    }

    private int DefaultHeight
    {
        get
        {
            if (_treeView != null)
            {
                var size = _treeView.PangoContext.FontDescription.Size / Pango.Scale.PangoScale;
                return (int)size + 16;
            }
            return 28;
        }
    }
    public int RowHeight
    {
        get
        {
            if (_gridview != null)
            {
                return _gridview.RowTemplate?.Height ?? 0;
            }
            return DefaultHeight;
        }
    }
    public DataGridView? DataGridView
    {
        get => _gridview;
        set
        {
            _gridview = value;
            if (value != null)
            {
                _treeView = value.GridView;
            }
        }
    }
    private string? _markup;
    public string? Markup
    {
        get => _markup; set
        {
            _markup = value;
            if (_cellRenderer != null)
            {
                _cellRenderer.Markup = value;
            }
        }
    }

    public DataGridViewElementStates State
    {
        get => DataGridViewElementStates.None;
        internal set { }
    }
    [DefaultValue("")]
    public string HeaderText
    {
        get => Title;
        set => Title = value;
    }
    private int _DisplayIndex;
    [Browsable(false)]
    public int DisplayIndex
    {
        get => _DisplayIndex; set => _DisplayIndex = value;
    }
    [Localizable(true)]
    [RefreshProperties(RefreshProperties.Repaint)]
    public new int Width
    {
        get => FixedWidth;
        set => FixedWidth = value;
    }

    //[DefaultValue(true)]
    //[Localizable(true)]
    //public bool Visible { get; set; }
    [Browsable(false)]
    [DefaultValue(null)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Type? ValueType { get; set; } = typeof(string);
    [DefaultValue("")]
    [Localizable(true)]

    public string? ToolTipText { get; set; }
    private DataGridViewColumnSortMode _SortMode = DataGridViewColumnSortMode.Automatic;
    public DataGridViewColumnSortMode SortMode
    {
        get => _SortMode;
        set
        {
            _SortMode = value;
            if (value == DataGridViewColumnSortMode.NotSortable)
                SortColumnId = -1;
            else
                SortColumnId = _index;
        }
    }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ISite? Site { get; set; }
    public new DataGridViewTriState Resizable
    {
        get => base.Resizable ? DataGridViewTriState.True : DataGridViewTriState.False;
        set => base.Resizable = value == DataGridViewTriState.True;
    }
    private bool _ReadOnly;
    public bool ReadOnly
    {
        get => _ReadOnly;
        set
        {
            _ReadOnly = value;
            if (_cellRenderer != null)
            {
                _cellRenderer.Editable = !_ReadOnly;
                _cellRenderer.Activatable = !_ReadOnly;
            }
        }
    }

    [DefaultValue("")]
    public string Name
    {
        get => Button.Name; set => Button.Name = value;
    }
    [DefaultValue(5)]
    [Localizable(true)]
    [RefreshProperties(RefreshProperties.Repaint)]

    public int MinimumWidth
    {
        get => MinWidth; set => MinWidth = value;
    }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsDataBound { get; set; }
    [Browsable(false)]
    public DataGridViewCellStyle? InheritedStyle { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public DataGridViewAutoSizeColumnMode InheritedAutoSizeMode { get; set; }


    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DataGridViewColumnHeaderCell? HeaderCell { get; set; }
    [DefaultValue(false)]
    [RefreshProperties(RefreshProperties.All)]

    public bool Frozen { get; set; }
    [DefaultValue(100)]

    public float FillWeight { get; set; }
    [DefaultValue(0)]

    public int DividerWidth { get; set; }

    private DataGridViewCellStyle? _DefaultCellStyle;
    [Browsable(true)]
    public DataGridViewCellStyle? DefaultCellStyle
    {
        get => _DefaultCellStyle;
        set
        {
            _DefaultCellStyle = value;
            if (_cellRenderer != null)
            {
                _cellRenderer.ColumnStyle = value;
            }
        }
    }
    private string _DataPropertyName = string.Empty;
    [Browsable(true)]
    [DefaultValue("")]
    public string DataPropertyName { get => _DataPropertyName; set => _DataPropertyName = value; }
    [DefaultValue(null)]

    public ContextMenuStrip? ContextMenuStrip { get; set; }
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public virtual Type? CellType => _cellTemplate?.GetType();

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public virtual DataGridViewCell? CellTemplate
    {
        get => _cellTemplate;
        set => _cellTemplate = value;
    }


    [DefaultValue(DataGridViewAutoSizeColumnMode.NotSet)]
    [RefreshProperties(RefreshProperties.Repaint)]

    public DataGridViewAutoSizeColumnMode AutoSizeMode
    {
        get => _AutoSizeMode;
        set
        {
            if (value == DataGridViewAutoSizeColumnMode.None) { _AutoSizeMode = value; base.Resizable = false; Sizing = TreeViewColumnSizing.Fixed; } else { base.Resizable = true; Sizing = TreeViewColumnSizing.GrowOnly; }
        }
    }
    private DataGridViewAutoSizeColumnMode _AutoSizeMode;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public virtual event EventHandler? Disposed;

    public object? Clone()
    {
        return null;
    }
    public virtual int GetPreferredWidth(DataGridViewAutoSizeColumnMode autoSizeColumnMode, bool fixedHeight)
    {
        return RowHeight;
    }
    public override string ToString() { return GetType().Name; }
    //protected override void Dispose(bool disposing) {  }
    private int _index;
    public int Index
    {
        get => _index;
        internal set
        {
            _index = value; if (_SortMode != DataGridViewColumnSortMode.NotSortable) { SortColumnId = value; }
            foreach (var cell in Cells) { AddAttribute(cell, "cellvalue", _index); }
        }
    }
}