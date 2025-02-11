/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gdk;
using GLib;
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class TreeView : ScrollableControl
    {
        public readonly TreeViewBase self = new TreeViewBase();
        public override object GtkControl => self;
        private Gtk.TreeStore _store;
        internal TreeNode root;
        internal Gtk.TreeStore Store { get { return _store; } }
        protected override void SetStyle(Widget widget)
        {
            base.SetStyle(self.TreeView);
        }
        private CellRendererToggle renderercheckbox;
        private CellRendererIcon rendererPixbuf;
        public TreeView() : base()
        {
            root = new TreeNode(this);
            root.Index = "-1";
            root.Name = "__root";
            _store = new Gtk.TreeStore(typeof(string), typeof(bool), typeof(int), typeof(string));
            self.TreeView.Model = _store;
            self.TreeView.Realized += TreeView_Realized;
            self.TreeView.Selection.Changed += Selection_Changed;
            self.TreeView.RowActivated += TreeView_RowActivated;
            self.TreeView.RowCollapsed += TreeView_RowCollapsed;
            self.TreeView.RowExpanded += TreeView_RowExpanded;
            this.BorderStyle = BorderStyle.Fixed3D;

            Gtk.TreeViewColumn column = new Gtk.TreeViewColumn();
            column.Title = "树目录";

            renderercheckbox = new CellRendererToggle();
            renderercheckbox.Activatable = true;
            renderercheckbox.IsExpanded = true;
            renderercheckbox.Toggled += CellName_Toggled;

            renderercheckbox.Visible = false;
            column.PackStart(renderercheckbox, false);
            column.AddAttribute(renderercheckbox, "active", 1);

            rendererPixbuf = new CellRendererIcon(this);
            rendererPixbuf.IsExpanded = true;
            rendererPixbuf.Visible = false;
            column.PackStart(rendererPixbuf, false);
            Gtk.CellRendererText renderertext = new Gtk.CellRendererText();
            renderertext.IsExpanded = true;
            renderertext.PlaceholderText = "---";
            column.PackStart(renderertext, true);
            column.AddAttribute(renderertext, "text", 0);
            self.TreeView.AppendColumn(column);
        }
        private bool Is_TreeView_Realized = false;
        private void TreeView_Realized(object sender, EventArgs e)
        {
            if (Is_TreeView_Realized == false)
            {
                Is_TreeView_Realized = true;
                if (ImageList != null)
                {
                    Gtk.TreeViewColumn column = ((Gtk.TreeView)sender).Columns[0];
                    if (string.IsNullOrWhiteSpace(ImageKey))
                    {
                        System.Drawing.Image image = ImageList.GetBitmap(ImageIndex);
                        rendererPixbuf.Pixbuf = image.Pixbuf;
                    }
                    else
                    {
                        System.Drawing.Image image = ImageList.GetBitmap(ImageKey);
                        rendererPixbuf.Pixbuf = image.Pixbuf;
                    }
                }
            }
        }

        private void TreeView_RowExpanded(object o, RowExpandedArgs args)
        {
            if (AfterExpand != null && ((Gtk.TreeView)o).IsVisible)
            {
                TreeNode result = null;
                GetNodeChild(root, args.Path.Indices, ref result);
                AfterExpand(this, new TreeViewEventArgs(result, TreeViewAction.Expand));
            }
        }

        private void TreeView_RowCollapsed(object o, RowCollapsedArgs args)
        {
            if (AfterCollapse != null && ((Gtk.TreeView)o).IsVisible)
            {
                TreeNode result = null;
                GetNodeChild(root, args.Path.Indices, ref result);
                AfterCollapse(this, new TreeViewEventArgs(result, TreeViewAction.Collapse));
            }
        }

        private void TreeView_RowActivated(object o, RowActivatedArgs args)
        {
            if (AfterSelect != null && ((Gtk.TreeView)o).IsVisible)
            {
                if (cancelEventArgs == null || cancelEventArgs.Cancel == false)
                {
                    TreeNode result = null;
                    GetNodeChild(root, args.Path.Indices, ref result);
                    AfterSelect(this, new TreeViewEventArgs(result));
                }
            }
        }
        private TreeViewCancelEventArgs cancelEventArgs = null;
        private void Selection_Changed(object sender, EventArgs e)
        {
            if (BeforeSelect != null)
            {
                if (self.TreeView.Selection.GetSelected(out TreeIter iter))
                {
                    TreePath[] paths = self.TreeView.Selection.GetSelectedRows();
                    TreeNode result = null; 
                    GetNodeChild(root, paths[0].Indices, ref result);
                    cancelEventArgs = new TreeViewCancelEventArgs(result, false, TreeViewAction.ByMouse);
                    BeforeSelect(this, cancelEventArgs);
                }
            }
        }

        public void Clear()
        {
            Store.Clear();
        }
        internal void LoadNodeValue(TreeNode node, TreeIter parent)
        {
            TreeIter iter = parent.Equals(TreeIter.Zero) ? Store.AppendValues(node.Text, node.Checked, node.ImageIndex, node.ImageKey) : Store.AppendValues(parent, node.Text, node.Checked, node.ImageIndex, node.ImageKey);
            TreePath path = Store.GetPath(iter);
            node.Index = string.Join(",", path.Indices);
            node.TreeIter = iter;
            foreach (TreeNode child in node.Nodes)
            {
                LoadNodeValue(child, iter);
            }
        }
        internal void NativeNodeChecked(TreeNode node, bool isChecked)
        {
            if (node != null)
            {
                _store.SetValue(node.TreeIter,1,isChecked);
            }
        }
        internal void NativeNodeSelected(TreeNode node, bool isSelected)
        {
            if (node != null)
            {
                this.SelectedNode = node;
            }
        }
        internal void NativeNodeText(TreeNode node, string text)
        {
            if (node != null)
            {
                _store.SetValue(node.TreeIter, 0, text);
            }
        }
        internal void NativeNodeImage(TreeNode node, int index)
        {
            if (node != null)
            {
                _store.SetValue(node.TreeIter, 2, index);
            }
        }
        internal void NativeNodeImage(TreeNode node, string key)
        {
            if (node != null)
            {
                _store.SetValue(node.TreeIter, 3, key);
            }
        }
        public TreeNodeCollection Nodes
        {
            get
            {
                return root.Nodes;
            }
        }
        private bool _checkBoxs;
        public bool CheckBoxes
        {
            get => _checkBoxs;
            set
            {
                _checkBoxs = value;
                renderercheckbox.Visible = _checkBoxs == true;
            }
        }
        private void CellName_Toggled(object o, ToggledArgs args)
        {
            //Console.WriteLine("CellRendererToggle CellName_Toggled");
            TreePath path = new TreePath(args.Path);
            var model = _store;
            model.GetIter(out TreeIter iter, path);
            bool val = (bool)(model.GetValue(iter, 1));
            model.SetValue(iter, 1, val == false);
        }
        private ImageList _imageList;
        public ImageList ImageList { get => _imageList; 
            set {
                _imageList = value;
                rendererPixbuf.Visible = _imageList != null;
                if (_imageList != null)
                {
                    Gtk.TreeViewColumn column = self.TreeView.Columns[0];
                    column.AddAttribute(rendererPixbuf, "pixbufkey", 3);
                    column.AddAttribute(rendererPixbuf, "pixbufindex", 2);
                }
            } 
        }
        public int ImageIndex { get; set; } = -1;
        public string ImageKey { get; set; }
        public int SelectedImageIndex { get; set; }
        public string SelectedImageKey { get; set; }
        public int StateImageIndex { get; set; }
        public string StateImageKey { get; set; }
        public void ExpandAll()
        {
            self.TreeView.ExpandAll();
        }
        public void CollapseAll()
        {
            self.TreeView.CollapseAll();
        }
        internal void SetExpandNode(TreeNode node, bool all)
        {
            self.TreeView.ExpandRow(_store.GetPath(node.TreeIter), all);
        }
        internal void SetCollapseNode(TreeNode node)
        {
            
            self.TreeView.CollapseRow(_store.GetPath(node.TreeIter));
        }
        internal bool GetNodeExpanded(TreeNode node)
        {
            return self.TreeView.GetRowExpanded(_store.GetPath(node.TreeIter));
        }
        internal void RemoveNode(TreeNode node)
        {
            _store.Remove(ref node.TreeIter);
        }
        public bool ShowLines { get=> self.TreeView.EnableTreeLines; set { self.TreeView.EnableTreeLines = true; self.TreeView.EnableGridLines = Gtk.TreeViewGridLines.Horizontal; } }
        public bool ShowNodeToolsTips { get; set; }
        public bool ShowPlusMinus { get; set; } = true;
        public bool ShowRootLines { get; set; } = true;
        public object SelectedItem
        {
            get
            {
                return SelectedNode.Text;
            }
        }
        public object SelectedValue
        {
            get
            {
                return SelectedNode.Text;
            }
        }

        [DefaultValue("\\")]
        public string PathSeparator
        {
            get;
            set;
        } = "\\";
        public TreeNode SelectedNode
        {
            get
            {
                if (self.TreeView.Selection.GetSelected(out TreeIter iter)) {
                    TreePath[] paths = self.TreeView.Selection.GetSelectedRows();
                    TreeNode result = null;
                    GetNodeChild(root, paths[0].Indices, ref result);
                    return result;
                }
                else { return null; }
            }
            set
            {
                if (value == null)
                    self.TreeView.Selection.UnselectAll();
                else
                {
                    self.TreeView.ExpandToPath(_store.GetPath(value.TreeIter));
                    self.TreeView.Selection.SelectIter(value.TreeIter);
                }
            }
        }
        public TreeNode TopNode
        {
            get
            {
                return root;
            }
            set
            {
               
            }
        }
        
        public event TreeViewCancelEventHandler BeforeSelect;
        public event TreeViewEventHandler AfterSelect;
        public event TreeViewEventHandler AfterCollapse;
        public event TreeViewEventHandler AfterExpand;
        private void GetNodeChild(TreeNode node, int[] indices, ref TreeNode result)
        {
            string nodeIndex= string.Join(",", indices);
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Index == nodeIndex)
                {
                    result = child;
                    return;
                }
                else if (nodeIndex.Length >= child.Index.Length)
                {
                    GetNodeChild(child, indices, ref result);
                }
            }
        }
        private class CellRendererIcon : Gtk.CellRendererPixbuf
        {
            public TreeView _treeView;
            public CellRendererIcon(TreeView treeView)
            {
                _treeView = treeView;
            }
            [Property("pixbufindex")]
            public int PixbufIndex
            {
                set
                {
                    if (value < _treeView.ImageList.Images.Count)
                        this.Pixbuf = _treeView.ImageList.Images[value].Pixbuf;
                }
            }
            [Property("pixbufkey")]
            public string PixbufKey
            {
                set
                {
                    if (string.IsNullOrWhiteSpace(value) == false)
                        this.Pixbuf = _treeView.ImageList.Images[value].Pixbuf;
                }
            }
        }
    }
}
