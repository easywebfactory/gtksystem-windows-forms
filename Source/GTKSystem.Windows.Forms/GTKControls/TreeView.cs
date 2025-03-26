/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GLib;
using Gtk;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class TreeView : ScrollableControl
    {
        public readonly TreeViewBase self = new();
        public override object GtkControl => self;
        private readonly TreeStore _store;
        internal TreeNode? root;
        internal TreeStore Store { get { return _store; } }
        protected override void SetStyle(Widget widget)
        {
            self.TreeView.Name = Name;
            base.SetStyle(self.TreeView);
        }
        private readonly CellRendererToggle renderercheckbox;
        private readonly CellRendererIcon rendererPixbuf;
        public TreeView() : base()
        {
            root = new TreeNode(this);
            root.Index = "-1";
            root.Name = "__root";
            _store = new TreeStore(typeof(string), typeof(bool), typeof(int), typeof(string));
            self.TreeView.Model = _store;
            self.TreeView.Realized += TreeView_Realized;
            self.TreeView.Selection.Changed += Selection_Changed;
            self.TreeView.RowActivated += TreeView_RowActivated;
            self.TreeView.RowCollapsed += TreeView_RowCollapsed;
            self.TreeView.RowExpanded += TreeView_RowExpanded;
            BorderStyle = BorderStyle.Fixed3D;

        var column = new TreeViewColumn();
        column.Title = "Tree Directory";

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
            var renderertext = new CellRendererText();
            renderertext.IsExpanded = true;
            renderertext.PlaceholderText = "---";
            column.PackStart(renderertext, true);
            column.AddAttribute(renderertext, "text", 0);
            self.TreeView.AppendColumn(column);
        }
        private void TreeView_Realized(object? sender, EventArgs e)
        {
            if (ImageList != null)
            {
                var column = ((Gtk.TreeView?)sender)?.Columns[0];
                if (string.IsNullOrWhiteSpace(ImageKey))
                {
                    var image = ImageList?.GetBitmap(ImageIndex);
                    if (image != null)
                    {
                        rendererPixbuf.Pixbuf = image.Pixbuf;
                    }
                }
                else
                {
                    System.Drawing.Image image = ImageList.GetBitmap(ImageKey);
                    rendererPixbuf.Pixbuf = image.Pixbuf;
                }
            }
        }

    private void TreeView_RowExpanded(object? o, RowExpandedArgs args)
    {
        if (AfterExpand != null && ((o as Gtk.TreeView)?.IsVisible ?? false))
        {
            TreeNode? result = null;
            GetNodeChild(root, args.Path.Indices, ref result);
            OnAfterExpand(new TreeViewEventArgs(result, TreeViewAction.Expand));
        }
    }

    protected virtual void OnAfterExpand(TreeViewEventArgs e)
    {
        AfterExpand?.Invoke(this, e);
    }

    private void TreeView_RowCollapsed(object? o, RowCollapsedArgs args)
    {
        if (AfterCollapse != null && ((o as Gtk.TreeView)?.IsVisible ?? false))
        {
            TreeNode? result = null;
            GetNodeChild(root, args.Path.Indices, ref result);
            var eventArgs = new TreeViewEventArgs(result, TreeViewAction.Collapse);
            OnAfterCollapse(eventArgs);
        }
    }

    protected virtual void OnAfterCollapse(TreeViewEventArgs e)
    {
        AfterCollapse?.Invoke(this, e);
    }

    private void TreeView_RowActivated(object? o, RowActivatedArgs args)
    {
        if (AfterSelect != null && ((o as Gtk.TreeView)?.IsVisible ?? false))
        {
            if (cancelEventArgs == null || cancelEventArgs.Cancel == false)
            {
                TreeNode? result = null;
                GetNodeChild(root, args.Path.Indices, ref result);
                var eventArgs = new TreeViewEventArgs(result);
                OnAfterSelect(eventArgs);
            }
        }
    }

    protected virtual void OnAfterSelect(TreeViewEventArgs e)
    {
        AfterSelect?.Invoke(this, e);
    }

    private TreeViewCancelEventArgs? cancelEventArgs;
    private void Selection_Changed(object? sender, EventArgs e)
    {
        if (BeforeSelect != null)
        {
            if (self.TreeView.Selection.GetSelected(out _))
            {
                var paths = self.TreeView.Selection.GetSelectedRows();
                TreeNode? result = null;
                GetNodeChild(root, paths[0].Indices, ref result);
                cancelEventArgs = new TreeViewCancelEventArgs(result, false, TreeViewAction.ByMouse);
                OnBeforeSelect(cancelEventArgs);
            }
        }
    }

    protected virtual void OnBeforeSelect(TreeViewCancelEventArgs? e)
    {
        BeforeSelect?.Invoke(this, e);
    }

    public void Clear()
        {
            Store.Clear();
        }
        internal void LoadNodeValue(TreeNode node, TreeIter parent)
        {
            var iter = parent.Equals(TreeIter.Zero) ? Store.AppendValues(node.Text, node.Checked, node.ImageIndex, node.ImageKey) : Store.AppendValues(parent, node.Text, node.Checked, node.ImageIndex, node.ImageKey);
            var path = Store.GetPath(iter);
            node.Index = string.Join(",", path.Indices);
            node.TreeIter = iter;
            foreach (var child in node.Nodes)
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
                SelectedNode = node;
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
        internal void NativeNodeImage(TreeNode node, string? key)
        {
            if (node != null)
            {
                _store.SetValue(node.TreeIter, 3, key);
            }
        }
        public TreeNodeCollection Nodes => root.Nodes;
        private bool _checkBoxs;
        public bool CheckBoxes
        {
            get => _checkBoxs;
            set
            {
                _checkBoxs = value;
                renderercheckbox.Visible = _checkBoxs;
            }
        }
        private void CellName_Toggled(object o, ToggledArgs args)
        {
            //Console.WriteLine("CellRendererToggle CellName_Toggled");
            var path = new TreePath(args.Path);
            var model = _store;
            model.GetIter(out var iter, path);
            var val = (bool)(model.GetValue(iter, 1));
            model.SetValue(iter, 1, val == false);
        }
        private ImageList? _imageList;
        public ImageList? ImageList { get => _imageList; 
            set {
                _imageList = value;
                rendererPixbuf.Visible = _imageList != null;
                if (_imageList != null)
                {
                    var column = self.TreeView.Columns[0];
                    column.AddAttribute(rendererPixbuf, "pixbufkey", 3);
                    column.AddAttribute(rendererPixbuf, "pixbufindex", 2);
                }
            } 
        }
        public int ImageIndex { get; set; } = -1;
        public string? ImageKey { get; set; }
        public int SelectedImageIndex { get; set; }
        public string? SelectedImageKey { get; set; }
        public int StateImageIndex { get; set; }
        public string? StateImageKey { get; set; }
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
        public bool ShowLines { get=> self.TreeView.EnableTreeLines; set { self.TreeView.EnableTreeLines = true; self.TreeView.EnableGridLines = TreeViewGridLines.Horizontal; } }
        public bool ShowNodeToolsTips { get; set; }
        public bool ShowPlusMinus { get; set; } = true;
        public bool ShowRootLines { get; set; } = true;
        public object? SelectedItem => SelectedNode?.Text;

        public object? SelectedValue => SelectedNode?.Text;

        [DefaultValue("\\")]
        public string PathSeparator
        {
            get;
            set;
        } = "\\";
        public TreeNode? SelectedNode
        {
            get
            {
                if (self.TreeView.Selection.GetSelected(out _)) {
                    TreePath[] paths = self.TreeView.Selection.GetSelectedRows();
                    TreeNode? result = null;
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
        public TreeNode? TopNode
        {
            get
            {
                return root;
            }
            set
            {
               
            }
        }
        
        public event TreeViewCancelEventHandler? BeforeSelect;
        public event TreeViewEventHandler? AfterSelect;
        public event TreeViewEventHandler? AfterCollapse;
        public event TreeViewEventHandler? AfterExpand;
        private void GetNodeChild(TreeNode? node, int[] indices, ref TreeNode? result)
        {
            var nodeIndex= string.Join(",", indices);
            foreach (var child in node.Nodes)
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
        private class CellRendererIcon : CellRendererPixbuf
        {
            public readonly TreeView _treeView;
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
                        Pixbuf = _treeView.ImageList.Images[value].Pixbuf;
                }
            }
            [Property("pixbufkey")]
            public string PixbufKey
            {
                set
                {
                    if (string.IsNullOrWhiteSpace(value) == false && _treeView.ImageList.Images.ContainsKey(value))
                        Pixbuf = _treeView.ImageList.Images[value].Pixbuf;
                }
            }
        }
    }
}