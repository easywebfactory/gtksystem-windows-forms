/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
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
    public DataGridViewTextBoxColumn(DataGridView? owningDataGridView) : base(owningDataGridView, new DataGridViewTextBoxCell())
    {
    }
}
public class DataGridViewCheckBoxColumn : DataGridViewColumn
{
    public DataGridViewCheckBoxColumn() : base(new DataGridViewCheckBoxCell())
    {
        ValueType = typeof(bool);
        SortMode = DataGridViewColumnSortMode.NotSortable;
    }
    public DataGridViewCheckBoxColumn(DataGridView? owningDataGridView) : base(owningDataGridView, new DataGridViewCheckBoxCell())
    {
        ValueType = typeof(bool);
        SortMode = DataGridViewColumnSortMode.NotSortable;
    }
    public override void Renderer()
    {
        var renderer = new CellRendererToggleValue(this);
        renderer.Activatable = ReadOnly == false;
        renderer.Mode = CellRendererMode.Activatable;
        renderer.Height = RowHeight;
        renderer.Width = Width;
        renderer.Toggled += CellName_Toggled;
        PackStart(renderer, true);
        AddAttribute(renderer, "cellvalue", DisplayIndex);
        Sizing = TreeViewColumnSizing.GrowOnly;
        if (SortMode != DataGridViewColumnSortMode.NotSortable)
            SortColumnId = DisplayIndex;
    }

    private void CellName_Toggled(object? o, ToggledArgs args)
    {
        var path = new TreePath(args.Path);
        var model = _treeView?.Model;
        TreeIter iter = default;
        model?.GetIter(out iter, path);
        var cell = model?.GetValue(iter, DisplayIndex);
        if (cell is CellValue val)
        {
            var tggle = o as CellRendererToggleValue;
            val.Value = tggle?.Active == false;
            model?.SetValue(iter, DisplayIndex, val);
            _gridview?.CellValueChanagedHandler(DisplayIndex, path.Indices.Last(), val);
        }
    }
}

public class DataGridViewRadioColumn : DataGridViewColumn
{
    public DataGridViewRadioColumn() : base(new DataGridViewCheckBoxCell())
    {
        ValueType = typeof(bool);
        SortMode = DataGridViewColumnSortMode.NotSortable;
    }
    public DataGridViewRadioColumn(DataGridView? owningDataGridView) : base(owningDataGridView, new DataGridViewCheckBoxCell())
    {
        ValueType = typeof(bool);
        SortMode = DataGridViewColumnSortMode.NotSortable;
    }
    public override void Renderer()
    {
        var renderer = new CellRendererToggleValue(this);
        renderer.Activatable = ReadOnly == false;
        renderer.Mode = CellRendererMode.Activatable;
        renderer.Radio = true;
        renderer.Height = RowHeight;
        renderer.Width = Width;
        renderer.Toggled += CellName_Toggled;
        PackStart(renderer, true);
        AddAttribute(renderer, "cellvalue", DisplayIndex);
        Sizing = TreeViewColumnSizing.GrowOnly;
        if (SortMode != DataGridViewColumnSortMode.NotSortable)
            SortColumnId = DisplayIndex;
    }

    private void CellName_Toggled(object? o, ToggledArgs args)
    {
        var path = new TreePath(args.Path);
        var model = _treeView?.Model;
        TreeIter iter=default;
        model?.GetIter(out iter, path);
        var cell = model?.GetValue(iter, DisplayIndex);
        if (cell is CellValue val)
        {
            var tggle = o as CellRendererToggleValue;
            val.Value = tggle?.Active == false;
            model?.SetValue(iter, DisplayIndex, val);
            _gridview?.CellValueChanagedHandler(DisplayIndex, path.Indices.Last(), val);
        }
    }
}
public class DataGridViewComboBoxColumn : DataGridViewColumn
{
    readonly ObjectCollection _items;
    public DataGridViewComboBoxColumn() : base(new DataGridViewComboBoxCell())
    {
        SortMode = DataGridViewColumnSortMode.NotSortable;
        _items = new ObjectCollection(this);
    }
    public DataGridViewComboBoxColumn(DataGridView? owningDataGridView) : base(owningDataGridView, new DataGridViewComboBoxCell())
    {
        SortMode = DataGridViewColumnSortMode.NotSortable;
        _items = new ObjectCollection(this);
    }
    public override void Renderer()
    {
        var renderer = new CellRendererComboValue(this);
        renderer.Editable = ReadOnly == false && _gridview?.ReadOnly == false;
        renderer.Edited += Renderer_Edited;
        renderer.TextColumn = 0;
        renderer.Height = RowHeight;
        renderer.Width = Width;
        var model = new ListStore(typeof(string));
        foreach (var item in _items)
        {
            model.AppendValues(item);
        }

        renderer.Model = model;
        PackStart(renderer, true);
        AddAttribute(renderer, "cellvalue", DisplayIndex);

        Sizing = TreeViewColumnSizing.GrowOnly;
        if (SortMode != DataGridViewColumnSortMode.NotSortable)
            SortColumnId = DisplayIndex;
    }

    private void Renderer_Edited(object? o, EditedArgs args)
    {
        var path = new TreePath(args.Path);
        var model = _treeView?.Model;
        TreeIter iter = default;
        model?.GetIter(out iter, path);
        var cell = model?.GetValue(iter, DisplayIndex);
        if (cell is CellValue val)
        {
            val.Value = args.NewText;
            model?.SetValue(iter, DisplayIndex, val);
            _gridview?.CellValueChanagedHandler(DisplayIndex, path.Indices.Last(), val);
        }
    }
    public ObjectCollection Items => _items;
    public class ObjectCollection : ArrayList
    {
        private DataGridViewColumn _owner;
        public ObjectCollection(DataGridViewColumn owner)
        {
            _owner = owner;
        }
        public void AddRange(object[] items)
        {
            foreach (var item in items)
                Add(item);
        }
    }
}

public class DataGridViewButtonColumn : DataGridViewColumn
{
    public DataGridViewButtonColumn() : base(new DataGridViewButtonCell())
    {
        SortMode = DataGridViewColumnSortMode.NotSortable;
    }
    public DataGridViewButtonColumn(DataGridView? owningDataGridView) : base(owningDataGridView, new DataGridViewButtonCell())
    {
        SortMode = DataGridViewColumnSortMode.NotSortable;
    }
    public override void Renderer()
    {
        var renderer = new CellRendererButtonValue(this);
        renderer.Editable = false;
        renderer.Height = RowHeight;
        renderer.Width = Width;
        if (DefaultCellStyle == null)
        {
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter };
        }
        else if (DefaultCellStyle.Alignment == DataGridViewContentAlignment.NotSet)
        {
            DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        PackStart(renderer, false);
        AddAttribute(renderer, "cellvalue", DisplayIndex);
        Sizing = TreeViewColumnSizing.GrowOnly;
        if (SortMode != DataGridViewColumnSortMode.NotSortable)
            SortColumnId = DisplayIndex;
        if (DataGridView is { GridView: not null })
        {
            DataGridView.GridView.RowActivated += TreeView_RowActivated;
        }
    }
    private void TreeView_RowActivated(object? o, RowActivatedArgs args)
    {
        if (args.Column.Handle == Handle)
        {
            if (args.Column.Cells[0] is CellRendererText cell)
            {
                var path = args.Path;
                _gridview?.CellValueChanagedHandler(DisplayIndex, path.Indices.Last(), new CellValue { Value = cell.Text });
            }
        }
    }

}
public class DataGridViewImageColumn : DataGridViewColumn
{
    public DataGridViewImageColumn() : base(new DataGridViewImageCell())
    {
        ValueType = typeof(Image);
        SortMode = DataGridViewColumnSortMode.NotSortable;
    }
    public DataGridViewImageColumn(DataGridView? owningDataGridView) : base(owningDataGridView, new DataGridViewImageCell())
    {
        ValueType = typeof(Image);
        SortMode = DataGridViewColumnSortMode.NotSortable;
    }
    public override void Renderer()
    {
        var renderer = new CellRendererPixbufValue(this);
        //renderer.IconName = "face-smile";
        renderer.Height = RowHeight;
        renderer.Width = Width;
        PackStart(renderer, false);
        AddAttribute(renderer, "cellvalue", DisplayIndex);
        Sizing = TreeViewColumnSizing.GrowOnly;
        if (SortMode != DataGridViewColumnSortMode.NotSortable)
            SortColumnId = DisplayIndex;
    }
}
public class DataGridViewLinkColumn : DataGridViewColumn
{
    public DataGridViewLinkColumn() : base(new DataGridViewLinkCell())
    {

    }
    public DataGridViewLinkColumn(DataGridView? owningDataGridView) : base(owningDataGridView, new DataGridViewLinkCell())
    {

    }
}

public class DataGridViewColumn : TreeViewColumn
{
    public int Key { get; }
    internal DataGridViewCell? _cellTemplate;
    internal Gtk.TreeView? _treeView;
    internal DataGridView? _gridview;
    public DataGridViewColumn(IntPtr intPtr) : base(intPtr)
    {
    }
    public DataGridViewColumn() : this(0, null, null)
    {
    }
    public DataGridViewColumn(DataGridViewCell? cellTemplate) : this(0, null, cellTemplate)
    {
    }
    public DataGridViewColumn(DataGridView? owningDataGridView) : this(0, owningDataGridView, null)
    {
    }
    public DataGridViewColumn(DataGridView? owningDataGridView, DataGridViewCell? cellTemplate) : this(0, owningDataGridView, cellTemplate)
    {
    }
    protected DataGridViewColumn(int key, DataGridView? owningDataGridView, DataGridViewCell? cellTemplate)
    {
        Key = key;
        _treeView = owningDataGridView?.GridView;
        _gridview = owningDataGridView;
        _cellTemplate = cellTemplate;
        base.Resizable = true;
        SortMode = DataGridViewColumnSortMode.Automatic;
    }

    public DataGridViewColumn(string title, CellRenderer cell, params object[] attrs) : base(title, cell, attrs)
    {
        _cellTemplate = new DataGridViewTextBoxCell();
        base.Resizable = Resizable == DataGridViewTriState.True;
    }
    public virtual void Renderer()
    {
        var renderer = new CellRendererValue(this);
        renderer.Editable = ReadOnly == false && _gridview?.ReadOnly == false;
        renderer.Edited += Renderer_Edited;
        renderer.Mode = CellRendererMode.Editable;
        renderer.PlaceholderText = "---";
        renderer.Markup = Markup;
        renderer.Width = Width;
        renderer.Height = RowHeight;
        if (_gridview != null)
        {
            if (_gridview.DefaultCellStyle?.WrapMode == DataGridViewTriState.True)
            {
                renderer.WrapMode = Pango.WrapMode.WordChar;
                renderer.WrapWidth = 0;
                renderer.WidthChars = 0;
            }
            if (_gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.AllCells || _gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.DisplayedCells || _gridview.RowTemplate?.Resizable == DataGridViewTriState.True)
            {
            }
            else
            {
                renderer.Height = RowHeight;
            }
        }
        PackStart(renderer, true);
        AddAttribute(renderer, "cellvalue", DisplayIndex);
        Sizing = TreeViewColumnSizing.Fixed;
        if (SortMode != DataGridViewColumnSortMode.NotSortable)
            SortColumnId = DisplayIndex;
    }

    private void Renderer_Edited(object? o, EditedArgs args)
    {
        var path = new TreePath(args.Path);
        var model = _treeView?.Model;
        TreeIter iter=default;
        model?.GetIter(out iter, path);
        var cell = model?.GetValue(iter, DisplayIndex);
        if (cell is CellValue val)
        {
            val.Value = args.NewText;
            model?.SetValue(iter, DisplayIndex, val);
            _gridview?.CellValueChanagedHandler(DisplayIndex, path.Indices.Last(), val);
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
                return _gridview.RowTemplate?.Height??0;
            }
            return DefaultHeight;
        }
    }
    public DataGridView? DataGridView
    {
        get => _gridview;
        set { _gridview = value; _treeView = value?.GridView; }
    }
    public string? Markup { get; set; }
    public DataGridViewElementStates State => DataGridViewElementStates.None;

    [DefaultValue("")]
    public string HeaderText
    {
        get => Title;
        set => Title = value;
    }
    [Browsable(false)]
    public int DisplayIndex { get; set; }
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
    public Type ValueType { get; set; } = typeof(string);
    [DefaultValue("")]
    [Localizable(true)]

    public string? ToolTipText { get; set; }

    public DataGridViewColumnSortMode SortMode { get; set; }
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ISite? Site { get; set; }
    public new DataGridViewTriState Resizable
    {
        get => base.Resizable ? DataGridViewTriState.True : DataGridViewTriState.False;
        set => base.Resizable = value == DataGridViewTriState.True;
    }

    public bool ReadOnly { get; set; }
    [DefaultValue("")] public string Name { get; set; } = string.Empty;
    [DefaultValue(5)]
    [Localizable(true)]
    [RefreshProperties(RefreshProperties.Repaint)]

    public int MinimumWidth { get; set; }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsDataBound => default;

    [Browsable(false)] public DataGridViewCellStyle? InheritedStyle => default;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public DataGridViewAutoSizeColumnMode InheritedAutoSizeMode => default;

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

    [Browsable(true)]
    public DataGridViewCellStyle? DefaultCellStyle { get; set; }
    [Browsable(true)]
    [DefaultValue("")]
    public string? DataPropertyName { get; set; }
    [DefaultValue(null)]

    public ContextMenuStrip? ContextMenuStrip { get; set; }
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public virtual Type? CellType => _cellTemplate?.GetType();

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public virtual DataGridViewCell? CellTemplate { get; set; }
    [DefaultValue(DataGridViewAutoSizeColumnMode.NotSet)]
    [RefreshProperties(RefreshProperties.Repaint)]

    public DataGridViewAutoSizeColumnMode AutoSizeMode { get; set; }


    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public virtual event EventHandler? Disposed;

    public object Clone()
    {
        return ((ArrayList)new ArrayList { this }.Clone())[0];
    }
    public virtual int GetPreferredWidth(DataGridViewAutoSizeColumnMode autoSizeColumnMode, bool fixedHeight)
    {
        return RowHeight;
    }
    public override string ToString() { return GetType().Name; }
    //protected override void Dispose(bool disposing) {  }

    public int Index { get; internal set; }
}