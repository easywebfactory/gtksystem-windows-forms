/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms.GtkRender;

namespace System.Windows.Forms
{
    public class DataGridViewTextBoxColumn : DataGridViewColumn
    {
        public DataGridViewTextBoxColumn():base(new DataGridViewTextBoxCell())
        {
        }
        public DataGridViewTextBoxColumn(DataGridView ownerGridView) : base(ownerGridView, new DataGridViewTextBoxCell())
        {
        }
    }
    public class DataGridViewCheckBoxColumn : DataGridViewColumn
    {
        public DataGridViewCheckBoxColumn() : this(null)
        {
        }
        public DataGridViewCheckBoxColumn(DataGridView ownerGridView) : base(0, ownerGridView, new DataGridViewCheckBoxCell())
        {
            ValueType = typeof(bool);

            var renderer = new CellRendererToggleValue(this);
            renderer.Editable = true;
            renderer.Mode = CellRendererMode.Activatable;
            renderer.Height = RowHeight;
            renderer.Toggled += CellName_Toggled;
            base.PackStart(renderer, true);
            _cellRenderer = renderer;
        }

        private void CellName_Toggled(object o, ToggledArgs args)
        {
            TreePath path = new TreePath(args.Path);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object row = model.GetValue(iter, 0);
            if (row is DataGridViewRow val)
            {
                CellRendererToggleValue tggle = (CellRendererToggleValue)o;
                val.Cells[Index].Value = tggle.Active == false;
                _gridview.CellValueChanagedHandler(Index, iter, path);
            }
        }
        internal override DataGridViewCell NewCell(object value = null, Type valueType = null)
        {
            DataGridViewCheckBoxCell newcell = new DataGridViewCheckBoxCell();
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
        public DataGridViewRadioColumn(DataGridView ownerGridView) : base(0, ownerGridView, new DataGridViewRadioCell())
        {
            ValueType = typeof(bool);
            var renderer = new CellRendererToggleValue(this);
            renderer.Editable = true;
            renderer.Mode = CellRendererMode.Activatable;
            renderer.Radio = true;
            renderer.Height = RowHeight;
            renderer.Toggled += CellName_Toggled;
            base.PackStart(renderer, true);
            _cellRenderer = renderer;
        }

        private void CellName_Toggled(object o, ToggledArgs args)
        {
            TreePath path = new TreePath(args.Path);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object row = model.GetValue(iter, 0);
            if (row is DataGridViewRow val)
            {
                CellRendererToggleValue tggle = (CellRendererToggleValue)o;
                val.Cells[Index].Value = tggle.Active == false;
                _gridview.CellValueChanagedHandler(Index, iter, path);
            }
        }
        internal override DataGridViewCell NewCell(object value = null, Type valueType = null)
        {
            DataGridViewRadioCell newcell = new DataGridViewRadioCell();
            AtrributesClone(newcell);
            newcell.Value = value;
            newcell.ValueType = valueType;
            return newcell;
        }
    }
    public class DataGridViewComboBoxColumn : DataGridViewColumn
    {
        ObjectCollection _items;
        internal Gtk.ListStore model = new Gtk.ListStore(typeof(string));
        public DataGridViewComboBoxColumn() : this(null)
        {
        }
        public DataGridViewComboBoxColumn(DataGridView ownerGridView) : base(0, ownerGridView, new DataGridViewComboBoxCell())
        {
            _items = new ObjectCollection(this);
            CellRendererComboValue renderer = new CellRendererComboValue(this);
            renderer.Editable = true;
            renderer.Mode = CellRendererMode.Editable;
            renderer.Edited += Renderer_Edited;
            renderer.EditingStarted += Renderer_EditingStarted;
            renderer.TextColumn = 0;
            renderer.Height = RowHeight;
            renderer.Model = model;
            base.PackStart(renderer, true);
            _cellRenderer = renderer;
        }
        private void Renderer_EditingStarted(object o, EditingStartedArgs args)
        {
            _cellEditing = true;
            _cellEditablePath = args.Path;
            CellEditable = args.Editable;
        }
        private void Renderer_Edited(object o, EditedArgs args)
        {
            RendererEdited(args.Path, args.NewText);
        }
        internal override DataGridViewCell NewCell(object value = null, Type valueType = null)
        {
            DataGridViewComboBoxCell newcell = new DataGridViewComboBoxCell();
            AtrributesClone(newcell);
            newcell.Value = value;
            newcell.ValueType = valueType;
            return newcell;
        }
        public ObjectCollection Items => _items;
        public class ObjectCollection : ArrayList
        {
            private DataGridViewComboBoxColumn _owner;
            public ObjectCollection(DataGridViewComboBoxColumn owner)
            {
                _owner = owner;
            }
            public void AddRange(object[] items)
            {
                foreach (object item in items)
                    Add(item);
            }
            public override int Add(object value)
            {
                _owner.model.AppendValues(value);
                return base.Add(value);
            }
        }
    }
    
    public class DataGridViewButtonColumn : DataGridViewColumn
    {
        public DataGridViewButtonColumn() : this(null)
        {
        }
        public DataGridViewButtonColumn(DataGridView ownerGridView) :base(0, ownerGridView, new DataGridViewButtonCell())
        {
            var renderer = new CellRendererButtonValue(this);
            renderer.Editable = false;
            renderer.Mode = CellRendererMode.Activatable;
            renderer.Height = RowHeight;
            renderer.SetAlignment(0.5f, 0.5f);
            base.PackStart(renderer, false);
            base.SortMode = DataGridViewColumnSortMode.NotSortable;
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            _cellRenderer = renderer;
        }
        internal override DataGridViewCell NewCell(object value = null, Type valueType = null)
        {
            DataGridViewButtonCell newcell = new DataGridViewButtonCell();
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
        public DataGridViewImageColumn(DataGridView ownerGridView) : base(0, ownerGridView, new DataGridViewImageCell())
        {
            ValueType = typeof(Drawing.Image);
            var renderer = new CellRendererPixbufValue(this);
            //renderer.IconName = "face-smile";
            renderer.Height = RowHeight;
            renderer.Editable = false;
            base.PackStart(renderer, false);
            _cellRenderer = renderer;
        }
        internal override DataGridViewCell NewCell(object value = null, Type valueType = null)
        {
            DataGridViewImageCell newcell = new DataGridViewImageCell();
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
        public DataGridViewLinkColumn(DataGridView ownerGridView) : base(0, ownerGridView, new DataGridViewLinkCell())
        {
          
        }
        internal override DataGridViewCell NewCell(object value = null, Type valueType = null)
        {
            DataGridViewLinkCell newcell = new DataGridViewLinkCell();
            AtrributesClone(newcell);
            newcell.Value = value;
            newcell.ValueType = valueType;
            return newcell;
        }
    }

    public class DataGridViewColumn : TreeViewColumn
    {
        internal DataGridViewCell _cellTemplate;
        internal Gtk.TreeView _treeView;
        internal DataGridView _gridview;
        internal ICellRenderer _cellRenderer;
        internal string _cellEditablePath;
        internal bool _cellEditing;
        public ICellEditable CellEditable;
        protected DataGridViewColumn(int column, DataGridView ownerGridView, DataGridViewCell cellTemplate) : base()
        {
            if (ownerGridView != null)
            {
                _gridview = ownerGridView;
                _treeView = ownerGridView.GridView;
            }
            _cellTemplate = cellTemplate;
            base.Resizable = true;
            base.Sizing = TreeViewColumnSizing.GrowOnly;
        }
        public DataGridViewColumn(DataGridView ownerGridView) : this(ownerGridView, new DataGridViewTextBoxCell())
        {
        }

        public DataGridViewColumn() : this(null, new DataGridViewTextBoxCell())
        {
        }
        public DataGridViewColumn(DataGridViewCell cellTemplate) : this(null, cellTemplate)
        {
        }

        public DataGridViewColumn(DataGridView ownerGridView, DataGridViewCell cellTemplate) : this(0, ownerGridView, cellTemplate)
        {
            var renderer = new CellRendererValue(this);
            renderer.Editable = true;
            renderer.Mode = CellRendererMode.Editable;
            renderer.Edited += Renderer_Edited;
            renderer.EditingStarted += Renderer_EditingStarted;
            renderer.PlaceholderText = "";
            renderer.Height = RowHeight;
            base.PackStart(renderer, true);
            _cellRenderer = renderer as ICellRenderer; 
        }

        private void Renderer_EditingStarted(object o, EditingStartedArgs args)
        {
            _cellEditing = true;
            _cellEditablePath = args.Path;
            CellEditable = args.Editable;
        }
        public void EditableEditingDone()
        {
            if (CellEditable != null)
            {
                try
                {
                    CellEditable.EditingDone += Editable_EditingDone;
                    CellEditable.FinishEditing();
                    CellEditable.EditingDone -= Editable_EditingDone;
                }
                catch { }
                finally {
                    CellEditable?.RemoveWidget();
                    CellEditable = null;
                    _cellEditing = false;
                }
            }
        }
        private void Editable_EditingDone(object sender, EventArgs e)
        {
            if (_cellEditing == true)
            {
                if (sender is Gtk.Entry enry)
                {
                    RendererEdited(_cellEditablePath, enry.Text);
                }
                else if (sender is Gtk.ComboBox com)
                {
                    RendererEdited(_cellEditablePath, com.Entry.Text);
                }
            }
        }

        private void Renderer_Edited(object o, EditedArgs args)
        {
            RendererEdited(args.Path, args.NewText);
        }
        public void RendererEdited(string treepath, string value)
        {
            _cellEditing = false;
            TreePath path = new TreePath(treepath);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object row = model.GetValue(iter, 0);
            if (row is DataGridViewRow val)
            {
                val.Cells[Index].Value = value;
                _gridview.CellValueChanagedHandler(Index, iter, path);
            }
            this.QueueResize();
        }
        internal virtual DataGridViewCell NewCell(object value = null, Type valueType = null)
        {
            DataGridViewTextBoxCell newcell = new DataGridViewTextBoxCell();
            AtrributesClone(newcell);
            newcell.Value = value;
            newcell.ValueType = valueType ?? typeof(string);
            return newcell;
        }
        internal void AtrributesClone(DataGridViewCell newcell)
        {
            newcell.Style = _cellTemplate.Style?.Clone();
            newcell.OwningRow = _cellTemplate.OwningRow;
            newcell.ReadOnly = _cellTemplate.ReadOnly;
        }
        public void SetDefaultStyle(DataGridViewCellStyle cellStyle)
        {
            if (cellStyle != null)
            {
                if (cellStyle.WrapMode == DataGridViewTriState.True)
                {
                    _cellRenderer.WrapMode = Pango.WrapMode.WordChar;
                    _cellRenderer.WrapWidth = Width;
                }
            }
        }
        Gtk.CssProvider css = new Gtk.CssProvider();
        public void UpdateDefaultStyle()
        {
            if (_gridview != null)
            {
                if (_ReadOnly || _gridview.ReadOnly)
                {
                    _cellRenderer.Editable = false;
                    _cellRenderer.Activatable = false;
                }
                else
                {
                    _cellRenderer.Editable = true;
                    _cellRenderer.Activatable = true;
                }
                if (_gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.AllCells || _gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.DisplayedCells || _gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders || _gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders)
                {
                    _cellRenderer.Height = -1;
                }
                else
                {
                    _cellRenderer.Height = RowHeight;
                }
                if (_gridview.DefaultCellStyle != null && _gridview.DefaultCellStyle.WrapMode == DataGridViewTriState.True)
                {
                    _cellRenderer.WrapMode = Pango.WrapMode.WordChar;
                    _cellRenderer.WrapWidth = Width;
                }
                if (_gridview.GridView.IsRealized)
                    this.QueueResize();

                DataGridViewCellStyle _cellStyle = this.DefaultCellStyle;
                if (_cellStyle == null && _gridview.ColumnHeadersDefaultCellStyle != null)
                    _cellStyle = _gridview.ColumnHeadersDefaultCellStyle;

                if (_cellStyle != null)
                {
                    Gtk.Button header = ((Gtk.Button)this.Button);
                    StringBuilder style = new StringBuilder();
                    if (_cellStyle.BackColor.Name != "0")
                    {
                        Color backColor = _cellStyle.BackColor;
                        string color = $"rgba({backColor.R},{backColor.G},{backColor.B},{backColor.A})";
                        style.AppendFormat("background-color:{0};background:{0};", color);
                    }

                    if (_cellStyle.ForeColor.Name != "0")
                    {
                        Color foreColor = _cellStyle.ForeColor;
                        string color = $"rgba({foreColor.R},{foreColor.G},{foreColor.B},{foreColor.A})";
                        style.AppendFormat("color:{0};", color);
                    }
                    if (_cellStyle.Font != null)
                    {
                        Font font = _cellStyle.Font;
                        if (font.Unit == GraphicsUnit.Pixel)
                            style.AppendFormat("font-size:{0}px;", font.Size);
                        else if (font.Unit == GraphicsUnit.Inch)
                            style.AppendFormat("font-size:{0}in;", font.Size);
                        else if (font.Unit == GraphicsUnit.Point)
                            style.AppendFormat("font-size:{0}pt;", font.Size);
                        else if (font.Unit == GraphicsUnit.Millimeter)
                            style.AppendFormat("font-size:{0}mm;", font.Size);
                        else if (font.Unit == GraphicsUnit.Document)
                            style.AppendFormat("font-size:{0}cm;", font.Size);
                        else if (font.Unit == GraphicsUnit.Display)
                            style.AppendFormat("font-size:{0}pc;", font.Size);
                        else
                            style.AppendFormat("font-size:{0}pt;", font.Size);

                        if (string.IsNullOrWhiteSpace(font.FontFamily?.Name) == false)
                        {
                            style.AppendFormat("font-family:\"{0}\";", font.FontFamily.Name);
                        }
                        if (font.Bold)
                        {
                            style.Append("font-weight:bold;");
                        }
                        if (font.Italic)
                        {
                            style.Append("font-style:italic;");
                        }
                        if (font.Underline)
                        {
                            style.Append("text-decoration:underline;");
                        }
                        if (font.Strikeout)
                        {
                            style.Append("text-decoration:line-through;");
                        }
                    }

                    if (style.Length > 9)
                    {
                        css.LoadFromData($".columnheadercell{{{style.ToString()}}}");
                        if (header.StyleContext.HasClass("columnheadercell"))
                        {
                            header.StyleContext.RemoveClass("columnheadercell");
                            header.StyleContext.RemoveProvider(css);
                        }
                        header.StyleContext.AddProvider(css, StyleProviderPriority.User);
                        header.StyleContext.AddClass("columnheadercell");
                    }
                    switch (_cellStyle.Alignment)
                    {
                        case DataGridViewContentAlignment.TopLeft:
                        case DataGridViewContentAlignment.MiddleLeft:
                        case DataGridViewContentAlignment.BottomLeft:
                            this.Alignment = 0f;
                            break;

                        case DataGridViewContentAlignment.TopCenter:
                        case DataGridViewContentAlignment.MiddleCenter:
                        case DataGridViewContentAlignment.BottomCenter:
                            this.Alignment = 0.5f;
                            break;

                        case DataGridViewContentAlignment.TopRight:
                        case DataGridViewContentAlignment.MiddleRight:
                        case DataGridViewContentAlignment.BottomRight:
                            this.Alignment = 1.0f;
                            break;

                        default:
                            this.Alignment = 0f;
                            break;
                    }
                }
            }
            SetDefaultStyle(this.DefaultCellStyle);
        }
        public int RowHeight
        {
            get
            {
                if (_gridview != null)
                {
                    return _gridview.RowTemplate.Height;
                }
                return 28;
            }
        }
        public DataGridView DataGridView { get { return _gridview; } set { _gridview = value; _treeView = value.GridView; } }
        private string _markup;
        public string Markup { get => _markup; set { _markup = value; _cellRenderer.Markup = value; } }

        public DataGridViewElementStates State { get { return DataGridViewElementStates.None; } internal set { } }
        public string HeaderText { get { return this.Title; } set { this.Title = value; } }
        public int DisplayIndex
        {
            get => Array.IndexOf(_treeView.Columns, (TreeViewColumn)this);
            set
            {
                if (_gridview != null && value >= 0 &&_gridview.Columns.Count > value && this.Handle != _treeView.Columns[value].Handle)
                {
                    _treeView.MoveColumnAfter((TreeViewColumn)this, _treeView.Columns[value]);
                }
            }
        }
        public new int Width { get { return base.FixedWidth; } set { base.FixedWidth = value; _cellRenderer.WidthChars = value; } }
        public Type ValueType { get; set; } = typeof(string);
        public string ToolTipText { get; set; }
        private DataGridViewColumnSortMode _SortMode = DataGridViewColumnSortMode.Automatic;
        public DataGridViewColumnSortMode SortMode
        {
            get => _SortMode;
            set
            {
                _SortMode = value;
                if (value == DataGridViewColumnSortMode.NotSortable)
                    base.SortColumnId = -1;
                else
                    base.SortColumnId = _index;
            }
        }
        public ISite Site { get; set; }
        public new DataGridViewTriState Resizable {
            get { return base.Resizable == true ? DataGridViewTriState.True : DataGridViewTriState.False; } 
            set { base.Resizable = value == DataGridViewTriState.True; }
        }
        private bool _ReadOnly;
        public bool ReadOnly {
            get=> _ReadOnly; 
            set
            {
                _ReadOnly = value;
                _cellRenderer.Editable = !_ReadOnly;
                _cellRenderer.Activatable = !_ReadOnly;
            }
        }
        public string Name { get => base.Button.Name; set { base.Button.Name = value; } }
        public int MinimumWidth { get => base.MinWidth; set { base.MinWidth = value; } }
        public bool IsDataBound { get; }
        public DataGridViewCellStyle InheritedStyle { get; }
        public DataGridViewAutoSizeColumnMode InheritedAutoSizeMode { get; }
        public DataGridViewColumnHeaderCell HeaderCell { get; set; }
        public bool Frozen { get; set; }
        public float FillWeight { get; set; } = 100;
        public int DividerWidth { get; set; }

        private DataGridViewCellStyle _DefaultCellStyle;
        public DataGridViewCellStyle DefaultCellStyle { 
            get=> _DefaultCellStyle; 
            set { _DefaultCellStyle = value; _cellRenderer.ColumnStyle = value; SetDefaultStyle(_DefaultCellStyle); }
        }
        private string _DataPropertyName = string.Empty;
        public string DataPropertyName { get => _DataPropertyName; set => _DataPropertyName = value; }
        public ContextMenuStrip ContextMenuStrip { get; set; }
        public virtual Type CellType { get { return _cellTemplate.GetType(); } }
        public virtual DataGridViewCell CellTemplate {
            get => _cellTemplate;
            set => _cellTemplate = value;
        }
        public DataGridViewAutoSizeColumnMode AutoSizeMode { 
            get=> _AutoSizeMode; 
            set {
                if (value == DataGridViewAutoSizeColumnMode.None) { _AutoSizeMode = value; base.Resizable = false; base.Sizing = TreeViewColumnSizing.Fixed; } else { base.Resizable = true; base.Sizing = TreeViewColumnSizing.GrowOnly; }
            }
        }
        private DataGridViewAutoSizeColumnMode _AutoSizeMode;
        public event EventHandler Disposed;

        public object Clone()
        {
            return null;
        }
        public virtual int GetPreferredWidth(DataGridViewAutoSizeColumnMode autoSizeColumnMode, bool fixedHeight)
        {
            return RowHeight;
        }
        private int _index = -1;
        public int Index
        {
            get => _index;
            internal set
            {
                if (_index == -1)
                    base.AddAttribute(base.Cells[0], "cellvalue", 0);
                _index = value;
                if (_SortMode != DataGridViewColumnSortMode.NotSortable)
                {
                    base.SortColumnId = _index;
                }
            }
        }
    }


}
