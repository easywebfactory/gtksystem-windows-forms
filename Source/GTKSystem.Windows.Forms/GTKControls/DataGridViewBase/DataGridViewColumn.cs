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
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Windows.Forms.GtkRender;
using System.Xml.Serialization;

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
            base.SortMode = DataGridViewColumnSortMode.NotSortable;
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            _cellRenderer = renderer;
        }

        private void CellName_Toggled(object o, ToggledArgs args)
        {
            TreePath path = new TreePath(args.Path);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object cell = model.GetValue(iter, this.Index);
            if (cell is DataGridViewCell val)
            {
                CellRendererToggleValue tggle = (CellRendererToggleValue)o;
                val.Value = tggle.Active == false;
                _gridview.CellValueChanagedHandler(this.Index, path.Indices.Last());
            }
        }
    }

    public class DataGridViewRadioColumn : DataGridViewColumn
    {
        public DataGridViewRadioColumn() : this(null)
        {
        }
        public DataGridViewRadioColumn(DataGridView ownerGridView) : base(0, ownerGridView, new DataGridViewCheckBoxCell())
        {
            ValueType = typeof(bool);
            var renderer = new CellRendererToggleValue(this);
            renderer.Editable = true;
            renderer.Mode = CellRendererMode.Activatable;
            renderer.Radio = true;
            renderer.Height = RowHeight;
            renderer.Toggled += CellName_Toggled;
            base.PackStart(renderer, true);
            base.SortMode = DataGridViewColumnSortMode.NotSortable;
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            _cellRenderer = renderer;
        }

        private void CellName_Toggled(object o, ToggledArgs args)
        {
            TreePath path = new TreePath(args.Path);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object cell = model.GetValue(iter, this.Index);
            if (cell is DataGridViewCell val)
            {
                CellRendererToggleValue tggle = (CellRendererToggleValue)o;
                val.Value = tggle.Active == false;
                _gridview.CellValueChanagedHandler(this.Index, path.Indices.Last());
            }
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
            renderer.Edited += Renderer_Edited;
            renderer.TextColumn = 0;
            renderer.Height = RowHeight;
            renderer.Model = model;
            base.PackStart(renderer, true);
            base.SortMode = DataGridViewColumnSortMode.NotSortable;
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            _cellRenderer = renderer;
        }

        private void Renderer_Edited(object o, EditedArgs args)
        {
            TreePath path = new TreePath(args.Path);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object cell = model.GetValue(iter, this.Index);
            if (cell is DataGridViewCell val)
            {
                val.Value = args.NewText;
                _gridview.CellValueChanagedHandler(this.Index, path.Indices.Last());
            }
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
        private void TreeView_RowActivated(object o, RowActivatedArgs args)
        {
            if (args.Column.Handle == this.Handle)
            {
                if (args.Column.Cells[0] is CellRendererText cell)
                {
                    TreePath path = args.Path;
                    _gridview.CellValueChanagedHandler(this.Index, path.Indices.Last());
                }
            }
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
            base.PackStart(renderer, false);
            base.SortMode = DataGridViewColumnSortMode.NotSortable;
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            _cellRenderer = renderer;
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
    }

    public class DataGridViewColumn : TreeViewColumn
    {
        internal DataGridViewCell _cellTemplate;
        internal Gtk.TreeView _treeView;
        internal DataGridView _gridview;
        internal ICellRenderer _cellRenderer;

        protected DataGridViewColumn(int column, DataGridView ownerGridView, DataGridViewCell cellTemplate) : base()
        {
            if (ownerGridView != null)
            {
                _gridview = ownerGridView;
                _treeView = ownerGridView.GridView;
            }
            _cellTemplate = cellTemplate;
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
            renderer.PlaceholderText = "---";
            renderer.Height = RowHeight;
            base.PackStart(renderer, true);
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            _cellRenderer = renderer as ICellRenderer;
        }
        private void Renderer_Edited(object o, EditedArgs args)
        {
            TreePath path = new TreePath(args.Path);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object cell = model.GetValue(iter, this.Index);
            if (cell is DataGridViewCell val)
            {
                val.Value = args.NewText;
                _gridview.CellValueChanagedHandler(this.Index, path.Indices.Last());
            }
        }
        public void SetGridViewDefaultStyle(DataGridViewCellStyle cellStyle)
        {
            if (cellStyle != null)
            {
                if (cellStyle.WrapMode == DataGridViewTriState.True)
                {
                    _cellRenderer.WrapMode = Pango.WrapMode.WordChar;
                    _cellRenderer.WrapWidth = 0;
                    _cellRenderer.WidthChars = 0;
                }
            }
            if (_gridview != null)
            {
                if (_gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.AllCells || _gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.DisplayedCells || _gridview.RowTemplate.Resizable == DataGridViewTriState.True)
                {
                    
                }
                else
                {
                    _cellRenderer.Height = RowHeight;
                }
            }
        }
        private int DefaultHeight
        {
            get
            {
                if (_treeView !=null)
                {
                    double size = _treeView.PangoContext.FontDescription.Size / Pango.Scale.PangoScale;
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
                    return _gridview.RowTemplate.Height;
                }
                return DefaultHeight;
            }
        }
        public DataGridView DataGridView { get { return _gridview; } set { _gridview = value; _treeView = value.GridView; } }
        private string _markup;
        public string Markup { get => _markup; set { _markup = value; _cellRenderer.Markup = value; } }

        public DataGridViewElementStates State { get { return DataGridViewElementStates.None; } internal set { } }
        [DefaultValue("")]
        public string HeaderText { get { return this.Title; } set { this.Title = value; } }
        private int _DisplayIndex;
        [Browsable(false)]
        public int DisplayIndex { get => _DisplayIndex; set { _DisplayIndex = value; } }
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public new int Width { get { return base.FixedWidth; } set { base.FixedWidth = value; } }

        //[DefaultValue(true)]
        //[Localizable(true)]
        //public bool Visible { get; set; }
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type ValueType { get; set; } = typeof(string);
        [DefaultValue("")]
        [Localizable(true)]

        public string ToolTipText { get; set; }
        private DataGridViewColumnSortMode _SortMode;
        public DataGridViewColumnSortMode SortMode
        {
            get => _SortMode;
            set
            {
                _SortMode = value;
                if (value == DataGridViewColumnSortMode.Automatic)
                    base.SortIndicator = true;
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [DefaultValue("")]
        public string Name { get => base.Button.Name; set { base.Button.Name = value; } }
        [DefaultValue(5)]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]

        public int MinimumWidth { get => base.MinWidth; set { base.MinWidth = value; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDataBound { get; }
        [Browsable(false)]
        public DataGridViewCellStyle InheritedStyle { get; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DataGridViewAutoSizeColumnMode InheritedAutoSizeMode { get; }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewColumnHeaderCell HeaderCell { get; set; }
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]

        public bool Frozen { get; set; }
        [DefaultValue(100)]

        public float FillWeight { get; set; }
        [DefaultValue(0)]

        public int DividerWidth { get; set; }

        private DataGridViewCellStyle _DefaultCellStyle;
        [Browsable(true)]
        public DataGridViewCellStyle DefaultCellStyle { 
            get=> _DefaultCellStyle; 
            set { _DefaultCellStyle = value; _cellRenderer.ColumnStyle = value; }
        }
        [Browsable(true)]
        [DefaultValue("")]
        public string DataPropertyName { get; set; }
        [DefaultValue(null)]

        public ContextMenuStrip ContextMenuStrip { get; set; }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual Type CellType { get { return _cellTemplate.GetType(); } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual DataGridViewCell CellTemplate {
            get
            {
                Type celltype = _cellTemplate.GetType();
                DataGridViewCell newcell=Activator.CreateInstance(celltype) as DataGridViewCell;
                var propertys = celltype.GetProperties(Reflection.BindingFlags.Public | Reflection.BindingFlags.Instance);
                foreach (PropertyInfo property in propertys)
                    if (property.CanRead && property.CanWrite)
                        property.SetValue(newcell, property.GetValue(_cellTemplate));

                return newcell;
            }
            set => _cellTemplate = value;
        }


        [DefaultValue(DataGridViewAutoSizeColumnMode.NotSet)]
        [RefreshProperties(RefreshProperties.Repaint)]

        public DataGridViewAutoSizeColumnMode AutoSizeMode { 
            get=> _AutoSizeMode; 
            set {
                if (value == DataGridViewAutoSizeColumnMode.None) { _AutoSizeMode = value; base.Resizable = false; base.Sizing = TreeViewColumnSizing.Fixed; } else { base.Resizable = true; base.Sizing = TreeViewColumnSizing.GrowOnly; }
            }
        }
        private DataGridViewAutoSizeColumnMode _AutoSizeMode;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler Disposed;

        public object Clone()
        {
            return ((ArrayList)(new ArrayList() { this }).Clone())[0]; 
        }
        public virtual int GetPreferredWidth(DataGridViewAutoSizeColumnMode autoSizeColumnMode, bool fixedHeight)
        {
            return RowHeight;
        }
        public override string ToString() { return this.GetType().Name; }
        //protected override void Dispose(bool disposing) {  }
        private int _index;
        public int Index { get => _index; internal set { _index = value; base.SortColumnId = value; foreach (var cell in base.Cells) { base.AddAttribute(cell, "cellvalue", _index); } } }

    }


}
