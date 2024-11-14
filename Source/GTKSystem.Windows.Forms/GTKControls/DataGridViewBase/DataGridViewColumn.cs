using Gtk;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms.GtkRender;

namespace System.Windows.Forms
{
    public class DataGridViewTextBoxColumn : DataGridViewColumn
    {
        public DataGridViewTextBoxColumn():base(new DataGridViewTextBoxCell())
        {
        }
        public DataGridViewTextBoxColumn(DataGridView owningDataGridView) : base(owningDataGridView, new DataGridViewTextBoxCell())
        {
        }
    }
    public class DataGridViewCheckBoxColumn : DataGridViewColumn
    {
        public DataGridViewCheckBoxColumn() : base(new DataGridViewCheckBoxCell())
        {
            ValueType = typeof(bool);
            this.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        public DataGridViewCheckBoxColumn(DataGridView owningDataGridView) : base(owningDataGridView, new DataGridViewCheckBoxCell())
        {
            ValueType = typeof(bool);
            this.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        public override void Renderer()
        {
            var renderer = new CellRendererToggleValue(this);
            renderer.Activatable = this.ReadOnly == false;
            renderer.Mode = CellRendererMode.Activatable;
            renderer.Height = RowHeight;
            renderer.Width = Width;
            renderer.Toggled += CellName_Toggled;
            base.PackStart(renderer, true);
            base.AddAttribute(renderer, "cellvalue", this.DisplayIndex);
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            if (this.SortMode != DataGridViewColumnSortMode.NotSortable)
                base.SortColumnId = this.DisplayIndex;
        }

        private void CellName_Toggled(object o, ToggledArgs args)
        {
            TreePath path = new TreePath(args.Path);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object cell = model.GetValue(iter, this.DisplayIndex);
            if (cell is CellValue val)
            {
                CellRendererToggleValue tggle = (CellRendererToggleValue)o;
                val.Value = tggle.Active == false;
                model.SetValue(iter, this.DisplayIndex, val);
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices.Last(), val);
            }
        }
    }

    public class DataGridViewRadioColumn : DataGridViewColumn
    {
        public DataGridViewRadioColumn() : base(new DataGridViewCheckBoxCell())
        {
            ValueType = typeof(bool);
            this.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        public DataGridViewRadioColumn(DataGridView owningDataGridView) : base(owningDataGridView, new DataGridViewCheckBoxCell())
        {
            ValueType = typeof(bool);
            this.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        public override void Renderer()
        {
            var renderer = new CellRendererToggleValue(this);
            renderer.Activatable = this.ReadOnly == false;
            renderer.Mode = CellRendererMode.Activatable;
            renderer.Radio = true;
            renderer.Height = RowHeight;
            renderer.Width = Width;
            renderer.Toggled += CellName_Toggled;
            base.PackStart(renderer, true);
            base.AddAttribute(renderer, "cellvalue", this.DisplayIndex);
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            if (this.SortMode != DataGridViewColumnSortMode.NotSortable)
                base.SortColumnId = this.DisplayIndex;
        }

        private void CellName_Toggled(object o, ToggledArgs args)
        {
            TreePath path = new TreePath(args.Path);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object cell = model.GetValue(iter, this.DisplayIndex);
            if (cell is CellValue val)
            {
                CellRendererToggleValue tggle = (CellRendererToggleValue)o;
                val.Value = tggle.Active == false;
                model.SetValue(iter, this.DisplayIndex, val);
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices.Last(), val);
            }
        }
    }
    public class DataGridViewComboBoxColumn : DataGridViewColumn
    {
        ObjectCollection _items;
        public DataGridViewComboBoxColumn() : base(new DataGridViewComboBoxCell())
        {
            this.SortMode = DataGridViewColumnSortMode.NotSortable;
            _items = new ObjectCollection(this);
        }
        public DataGridViewComboBoxColumn(DataGridView owningDataGridView) : base(owningDataGridView, new DataGridViewComboBoxCell())
        {
            this.SortMode = DataGridViewColumnSortMode.NotSortable;
            _items = new ObjectCollection(this);
        }
        public override void Renderer()
        {
            CellRendererComboValue renderer = new CellRendererComboValue(this);
            renderer.Editable = this.ReadOnly == false && _gridview.ReadOnly == false;
            renderer.Edited += Renderer_Edited;
            renderer.TextColumn = 0;
            renderer.Height = RowHeight;
            renderer.Width = Width;
            Gtk.ListStore model = new Gtk.ListStore(typeof(string));
            foreach(var item in _items)
            {
                model.AppendValues(item);
            }

            renderer.Model = model;
            base.PackStart(renderer, true);
            base.AddAttribute(renderer, "cellvalue", this.DisplayIndex);
            
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            if (this.SortMode != DataGridViewColumnSortMode.NotSortable)
                base.SortColumnId = this.DisplayIndex;
        }

        private void Renderer_Edited(object o, EditedArgs args)
        {
            TreePath path = new TreePath(args.Path);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object cell = model.GetValue(iter, this.DisplayIndex);
            if (cell is CellValue val)
            {
                val.Value = args.NewText;
                model.SetValue(iter, this.DisplayIndex, val);
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices.Last(), val);
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
                foreach (object item in items)
                    Add(item);
            }
        }
    }
    
    public class DataGridViewButtonColumn : DataGridViewColumn
    {
        public DataGridViewButtonColumn():base(new DataGridViewButtonCell())
        {
            this.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        public DataGridViewButtonColumn(DataGridView owningDataGridView) : base(owningDataGridView, new DataGridViewButtonCell())
        {
            this.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        public override void Renderer()
        {
            var renderer = new CellRendererButtonValue(this);
            renderer.Editable = false;
            renderer.Height = RowHeight;
            renderer.Width = Width;
            if (base.DefaultCellStyle == null)
            {
                base.DefaultCellStyle = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleCenter };
            }
            else if (base.DefaultCellStyle.Alignment == null || base.DefaultCellStyle.Alignment == DataGridViewContentAlignment.NotSet)
            {
                base.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            base.PackStart(renderer, false);
            base.AddAttribute(renderer, "cellvalue", this.DisplayIndex);
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            if (this.SortMode != DataGridViewColumnSortMode.NotSortable)
                base.SortColumnId = this.DisplayIndex;
            if (this.DataGridView != null)
                this.DataGridView.GridView.RowActivated += TreeView_RowActivated;
        }
        private void TreeView_RowActivated(object o, RowActivatedArgs args)
        {
            if (args.Column.Handle == this.Handle)
            {
                if (args.Column.Cells[0] is CellRendererText cell)
                {
                    TreePath path = args.Path;
                    _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices.Last(), new CellValue() { Value = cell.Text });
                }
            }
        }

    }
    public class DataGridViewImageColumn : DataGridViewColumn
    {
        public DataGridViewImageColumn() : base(new DataGridViewImageCell())
        {
            this.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        public DataGridViewImageColumn(DataGridView owningDataGridView) : base(owningDataGridView, new DataGridViewImageCell())
        {
            this.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        public override void Renderer()
        {
            var renderer = new CellRendererPixbufValue(this);
            //renderer.IconName = "face-smile";
            renderer.Height = RowHeight;
            renderer.Width = Width;
            base.PackStart(renderer, false);
            base.AddAttribute(renderer, "cellvalue", this.DisplayIndex);
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            if (this.SortMode != DataGridViewColumnSortMode.NotSortable)
                base.SortColumnId = this.DisplayIndex;
        }

    }
    public class DataGridViewLinkColumn : DataGridViewColumn
    {
        public DataGridViewLinkColumn() : base(new DataGridViewLinkCell())
        {
          
        }
        public DataGridViewLinkColumn(DataGridView owningDataGridView) : base(owningDataGridView, new DataGridViewLinkCell())
        {
            
        }
    }

    public class DataGridViewColumn : TreeViewColumn
    {
        internal DataGridViewCell _cellTemplate;
        internal Gtk.TreeView _treeView;
        internal DataGridView _gridview;
        public DataGridViewColumn(IntPtr intPtr) : base(intPtr)
        {
        }
        public DataGridViewColumn() : this(0, null, null)
        {
        }
        public DataGridViewColumn(DataGridViewCell cellTemplate) : this(0, null, cellTemplate)
        {
        }
        public DataGridViewColumn(DataGridView owningDataGridView) : this(0, owningDataGridView, null)
        {
        }
        public DataGridViewColumn(DataGridView owningDataGridView, DataGridViewCell cellTemplate) : this(0, owningDataGridView, cellTemplate)
        {
        }
        protected DataGridViewColumn(int key,DataGridView owningDataGridView, DataGridViewCell cellTemplate) : base()
        {
            _treeView = owningDataGridView?.GridView;
            _gridview = owningDataGridView;
            _cellTemplate = cellTemplate;
            base.Resizable = this.Resizable == DataGridViewTriState.True;
            this.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        public DataGridViewColumn(string title, CellRenderer cell, params object[] attrs) : base(title, cell, attrs)
        {
            _cellTemplate = new DataGridViewTextBoxCell();
            base.Resizable = this.Resizable == DataGridViewTriState.True;
        }
        public virtual void Renderer()
        {
            var renderer = new CellRendererValue(this);
            renderer.Editable = this.ReadOnly == false && _gridview.ReadOnly == false;
            renderer.Edited += Renderer_Edited;
            renderer.Mode = CellRendererMode.Editable;
            renderer.PlaceholderText = "---";
            renderer.Markup = this.Markup;
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
                if (_gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.AllCells || _gridview.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.DisplayedCells || _gridview.RowTemplate.Resizable == DataGridViewTriState.True)
                {
                }
                else
                {
                    renderer.Height = RowHeight;
                }
            }
            base.PackStart(renderer, true);
            base.AddAttribute(renderer, "cellvalue", this.DisplayIndex);
            base.Sizing = TreeViewColumnSizing.Fixed;
            if (this.SortMode != DataGridViewColumnSortMode.NotSortable)
                base.SortColumnId = this.DisplayIndex;
        }

        private void Renderer_Edited(object o, EditedArgs args)
        {
            TreePath path = new TreePath(args.Path);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object cell = model.GetValue(iter, this.DisplayIndex);
            if (cell is CellValue val)
            {
                val.Value = args.NewText;
                model.SetValue(iter, this.DisplayIndex, val);
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices.Last(), val);
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
        public string Markup { get; set; }
        public DataGridViewElementStates State { get { return DataGridViewElementStates.None; } internal set { } }
        [DefaultValue("")]
        public string HeaderText { get { return this.Title; } set { this.Title = value; } }
        [Browsable(false)]
        public int DisplayIndex { get; set; }
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

        public DataGridViewColumnSortMode SortMode { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISite Site { get; set; }
        public new DataGridViewTriState Resizable {
            get { return base.Resizable == true ? DataGridViewTriState.True : DataGridViewTriState.False; } 
            set { base.Resizable = value == DataGridViewTriState.True; } }

        public bool ReadOnly { get; set; }
        [DefaultValue("")]
        public string Name { get; set; }
        [DefaultValue(5)]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]

        public int MinimumWidth { get; set; }
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

        [Browsable(true)]
        public DataGridViewCellStyle DefaultCellStyle { get; set; }
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
        public virtual DataGridViewCell CellTemplate { get; set; }
        [DefaultValue(DataGridViewAutoSizeColumnMode.NotSet)]
        [RefreshProperties(RefreshProperties.Repaint)]

        public DataGridViewAutoSizeColumnMode AutoSizeMode { get; set; }


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

        public int Index { get; internal set; }
    }


}
