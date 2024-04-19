using Gtk;
using System.Collections;
using System.ComponentModel;
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
            var renderer = new CellRendererToggleValue();
            renderer.Activatable = this.ReadOnly == false;
            renderer.Mode = CellRendererMode.Activatable;
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
                Boolean.TryParse(val.Text, out bool result);
                val.Text = result == true ? "False" : "True";
                model.SetValue(iter, this.DisplayIndex, val);
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices[0], val);
            }
            else
            {
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices[0], new CellValue() { Text = cell?.ToString() });
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
            var renderer = new CellRendererToggleValue();
            renderer.Activatable = this.ReadOnly == false;
            renderer.Mode = CellRendererMode.Activatable;
            renderer.Radio = true;
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
                Boolean.TryParse(val.Text, out bool result);
                val.Text = result == true ? "False" : "True";
                model.SetValue(iter, this.DisplayIndex, val);
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices[0], val);
            }
            else
            {
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices[0], new CellValue() { Text = cell?.ToString() });
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
            CellRendererComboValue renderer = new CellRendererComboValue();
            renderer.Editable = this.ReadOnly == false;
            renderer.Edited += Renderer_Edited;
            //renderer.Changed += Renderer_Changed;
            //renderer.WidthChars = 10;
            renderer.TextColumn = 0;
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

        private void Renderer_Changed(object o, ChangedArgs args)
        {
            CellRendererCombo combo = (CellRendererCombo)o;
            TreePath path = new TreePath(args.Args[0].ToString());
            TreeIter iter = (TreeIter)args.Args[1];
            object val = combo.Model.GetValue(iter, 0);
            _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices[0], new CellValue() { Text = val?.ToString() });
        }

        private void Renderer_Edited(object o, EditedArgs args)
        {
            TreePath path = new TreePath(args.Path);
            var model = _treeView.Model;
            model.GetIter(out TreeIter iter, path);
            object cell = model.GetValue(iter, this.DisplayIndex);
            if (cell is CellValue val)
            {
                val.Text = args.NewText;
                model.SetValue(iter, this.DisplayIndex, val);
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices[0], val);
            }
            else
            {
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices[0], new CellValue() { Text = cell?.ToString() });
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

        private void TreeView_RowActivated(object o, RowActivatedArgs args)
        {
            if (args.Column.Handle == this.Handle)
            {
                TreePath path = args.Path;
                if(args.Column.Cells[0] is CellRendererText cell)
                {
                    _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices[0], new CellValue() { Text = cell.Text?.ToString() });
                }
            }
        }

        public override void Renderer()
        {
            var renderer = new CellRendererButtonValue();
            renderer.Editable = false;
            base.PackStart(renderer, true);
            base.AddAttribute(renderer, "cellvalue", this.DisplayIndex);
            base.Sizing = TreeViewColumnSizing.GrowOnly;
            if (this.SortMode != DataGridViewColumnSortMode.NotSortable)
                base.SortColumnId = this.DisplayIndex;
            if (this.DataGridView != null)
                this.DataGridView.TreeView.RowActivated += TreeView_RowActivated;
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
            var renderer = new CellRendererPixbufValue();
            renderer.IconName = "face-smile";
            base.PackStart(renderer, true);
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
            _treeView = owningDataGridView?.TreeView;
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
            var renderer = new CellRendererValue();
            renderer.Editable = this.ReadOnly == false;
            renderer.Edited += Renderer_Edited;
            renderer.Mode = CellRendererMode.Editable;
            renderer.PlaceholderText = "---";
            renderer.Markup = this.Markup;
            renderer.Alignment=Pango.Alignment.Center;
            //renderer.Xalign = 0f;
            //base.Alignment = 0;
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
                val.Text = args.NewText;
                model.SetValue(iter, this.DisplayIndex, val);
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices[0], val);
            }
            else
            {
                _gridview.CellValueChanagedHandler(this.DisplayIndex, path.Indices[0], new CellValue() { Text = cell?.ToString() });
            }

        }

        public DataGridView DataGridView { get { return _gridview; } set { _gridview = value; _treeView = value.TreeView; } }
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
            return this;
        }
        public virtual int GetPreferredWidth(DataGridViewAutoSizeColumnMode autoSizeColumnMode, bool fixedHeight)
        {
            return 10;
        }
        public override string ToString() { return this.GetType().Name; }
        //protected override void Dispose(bool disposing) {  }

        public int Index { get; internal set; }
    }


}
