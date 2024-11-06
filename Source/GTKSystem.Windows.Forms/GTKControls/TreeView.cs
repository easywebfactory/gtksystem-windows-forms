/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Atk;
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
        public TreeView() : base()
        {
            root = new TreeNode(this);
            root.Name = "root";
            _store = new Gtk.TreeStore(typeof(string), typeof(bool), typeof(Pixbuf));
            self.TreeView.Model = _store;
            self.TreeView.Realized += Control_Realized;
            self.TreeView.Selection.Changed += Selection_Changed;
            self.TreeView.RowActivated += TreeView_RowActivated;
            self.TreeView.RowCollapsed += TreeView_RowCollapsed;
            self.TreeView.RowExpanded += TreeView_RowExpanded;
            this.BorderStyle = BorderStyle.Fixed3D;
        }

        private void TreeView_RowExpanded(object o, RowExpandedArgs args)
        {
            if (AfterExpand != null && ((Gtk.TreeView)o).IsVisible)
            {
                TreeNode result = new TreeNode();
                GetNodeChild(root, args.Path.Indices, ref result);
                AfterExpand(this, new TreeViewEventArgs(result, TreeViewAction.Expand));
            }
        }

        private void TreeView_RowCollapsed(object o, RowCollapsedArgs args)
        {
            if (AfterCollapse != null && ((Gtk.TreeView)o).IsVisible)
            {
                TreeNode result = new TreeNode();
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
                    TreeNode result = new TreeNode();
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
                    TreeNode result = new TreeNode();
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
        private void Control_Realized(object sender, EventArgs e)
        {
            CellRendererToggle renderercheckbox = new CellRendererToggle();
            renderercheckbox.Activatable = true;
            renderercheckbox.IsExpanded = true;
            renderercheckbox.Toggled += CellName_Toggled;

            Gtk.CellRendererText renderertext = new Gtk.CellRendererText();
            renderertext.IsExpanded = true;
            renderertext.PlaceholderText = "---";

            Gtk.TreeViewColumn column = new Gtk.TreeViewColumn();
            column.Title = "树目录";
            if (CheckBoxes == true)
            {
                column.PackStart(renderercheckbox, false);
                column.AddAttribute(renderercheckbox, "active", 1);
            }
            if (ImageList != null)
            {
                Gtk.CellRendererPixbuf rendererPixbuf = new Gtk.CellRendererPixbuf();
                rendererPixbuf.IsExpanded = true;

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
                column.PackStart(rendererPixbuf, false);
                column.AddAttribute(rendererPixbuf, "pixbuf", 2);
            }
            column.PackStart(renderertext, true);
            column.AddAttribute(renderertext, "text", 0);
            self.TreeView.AppendColumn(column);
        }
        public class CellRendererIcon: Gtk.CellRendererPixbuf
        {
            public ImageList _imageList;
            public CellRendererIcon(ImageList imageList) {
                _imageList= imageList;
            }
            [Property("pixbuficon")]
            public string PixbufIcon
            {
                set {
                    if(string.IsNullOrWhiteSpace(value)==false)
                        this.Pixbuf = _imageList.Images[value].Pixbuf;
                    else
                        this.IconName = "image-missing";
                }
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
        internal void LoadNodeValue(TreeNode node, TreeIter parent)
        {
            Pixbuf pixbuf = null;
            if (ImageList != null)
            {
                if (string.IsNullOrWhiteSpace(node.ImageKey))
                {
                    var img = ImageList.GetBitmap(node.ImageIndex);
                    if (img != null)
                        pixbuf = img.Pixbuf;
                }
                else
                {
                    var img = ImageList.GetBitmap(node.ImageKey);
                    if (img != null)
                        pixbuf = img.Pixbuf;
                }
            }

            TreeIter iter = parent.Equals(TreeIter.Zero) ? Store.AppendValues(node.Text, node.Checked, pixbuf) : Store.AppendValues(parent, node.Text, node.Checked, pixbuf);
            TreePath path = Store.GetPath(iter);
            node.Index = string.Join(",", path.Indices);
            node.TreeIter = iter;
            foreach (TreeNode child in node.Nodes)
            {
                LoadNodeValue(child, iter);
            }
        }
        internal void SetChecked(TreeNode node, bool isChecked)
        {
            if (node != null)
            {
                _store.SetValue(node.TreeIter,1,isChecked);
            }
        }
        internal void SetSelected(TreeNode node, bool isSelected)
        {
            if (node != null)
            {
                this.SelectedNode = node;
            }
        }
        public TreeNodeCollection Nodes
        {
            get
            {
                return root.Nodes;
            }
        }
        public ImageList ImageList { get; set; }
        public int ImageIndex { get; set; }
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
        public bool CheckBoxes { get; set; }
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
        public string SelectedValuePath
        {
            get
            {
                string nodePath = string.Empty;
                if (self.TreeView.Selection.GetSelected(out TreeIter iter))
                {
                    TreePath[] paths = self.TreeView.Selection.GetSelectedRows();
                    List<string> nodeNames = new List<string>();
                    GetNodePath(root, paths[0].Indices, 0, ref nodeNames);
                    nodePath = string.Join(PathSeparator, nodeNames);
                }
                return nodePath;
            }
            set { }
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
                    TreeNode result = new TreeNode();
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
        private void GetNodeChilds(TreeNode node, int[] indices, int depth, ref TreeNode result, ref TreeNode lastNode)
        {
            TreeNode treeNode = new TreeNode();
            string nodeIndex = string.Join(",", indices.Take(depth + 1));
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Index == nodeIndex)
                {
                    treeNode = (TreeNode)child.Clone();
                    result.Nodes.Add(treeNode);
                    depth++;
                    if (depth < indices.Length)
                        GetNodeChilds(child, indices, depth, ref treeNode, ref lastNode);
                }
            }
            lastNode = treeNode;
        }
        private void GetNodePath(TreeNode node, int[] indices, int depth, ref List<string> nodePath)
        {
            string nodeIndex = string.Join(",", indices.Take(depth + 1));
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Index == nodeIndex)
                {
                    nodePath.Add(child.Text);
                    depth++;
                    if (depth < indices.Length)
                        GetNodePath(child, indices, depth, ref nodePath);
                }
            }
        }
    }
}
