﻿/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GLib;
using Gtk;
using System.ComponentModel;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class TreeView : ScrollableControl
{
    public readonly TreeViewBase self = new();
    public override object GtkControl => self;
    private readonly TreeStore _store;
    internal TreeNode root;
    internal TreeStore Store => _store;

    protected override void SetStyle(Widget widget)
    {
        base.SetStyle(self.treeView);
    }
    private readonly CellRendererToggle renderercheckbox;
    private readonly CellRendererIcon rendererPixbuf;
    public TreeView()
    {
        root = new TreeNode(this)
        {
            Index = "-1",
            Name = "__root"
        };
        _store = new TreeStore(typeof(string), typeof(bool), typeof(int), typeof(string));
        self.treeView.Model = _store;
        self.treeView.Realized += TreeView_Realized;
        self.treeView.Selection.Changed += Selection_Changed;
        self.treeView.RowActivated += TreeView_RowActivated;
        self.treeView.RowCollapsed += TreeView_RowCollapsed;
        self.treeView.RowExpanded += TreeView_RowExpanded;
        BorderStyle = BorderStyle.Fixed3D;

        var column = new TreeViewColumn();
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
        var renderertext = new CellRendererText();
        renderertext.IsExpanded = true;
        renderertext.PlaceholderText = "---";
        column.PackStart(renderertext, true);
        column.AddAttribute(renderertext, "text", 0);
        self.treeView.AppendColumn(column);
    }
    private bool isTreeViewRealized;
    private void TreeView_Realized(object? sender, EventArgs e)
    {
        if (isTreeViewRealized == false)
        {
            isTreeViewRealized = true;
            if (ImageList != null)
            {
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

    private void TreeView_RowExpanded(object? o, RowExpandedArgs args)
    {
        if (AfterExpand != null && ((o as Gtk.TreeView)?.IsVisible ?? false))
        {
            TreeNode? result = null;
            GetNodeChild(root, args.Path.Indices, ref result);
            AfterExpand?.Invoke(this, new TreeViewEventArgs(result, TreeViewAction.Expand));
        }
    }

    private void TreeView_RowCollapsed(object? o, RowCollapsedArgs args)
    {
        if (AfterCollapse != null && ((o as Gtk.TreeView)?.IsVisible ?? false))
        {
            TreeNode? result = null;
            GetNodeChild(root, args.Path.Indices, ref result);
            AfterCollapse?.Invoke(this, new TreeViewEventArgs(result, TreeViewAction.Collapse));
        }
    }

    private void TreeView_RowActivated(object? o, RowActivatedArgs args)
    {
        if (AfterSelect != null && ((o as Gtk.TreeView)?.IsVisible ?? false))
        {
            if (cancelEventArgs == null || cancelEventArgs.Cancel == false)
            {
                TreeNode? result = null;
                GetNodeChild(root, args.Path.Indices, ref result);
                AfterSelect?.Invoke(this, new TreeViewEventArgs(result));
            }
        }
    }
    private TreeViewCancelEventArgs? cancelEventArgs;
    private void Selection_Changed(object? sender, EventArgs e)
    {
        if (BeforeSelect != null)
        {
            if (self.treeView.Selection.GetSelected(out _))
            {
                var paths = self.treeView.Selection.GetSelectedRows();
                TreeNode? result = null;
                GetNodeChild(root, paths[0].Indices, ref result);
                cancelEventArgs = new TreeViewCancelEventArgs(result, false, TreeViewAction.ByMouse);
                BeforeSelect?.Invoke(this, cancelEventArgs);
            }
        }
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
        node.treeIter = iter;
        foreach (var child in node.Nodes)
        {
            LoadNodeValue(child, iter);
        }
    }
    internal void NativeNodeChecked(TreeNode node, bool isChecked)
    {
        if (node != null)
        {
            _store.SetValue(node.treeIter, 1, isChecked);
        }
    }
    internal void NativeNodeSelected(TreeNode? node, bool isSelected)
    {
        if (node != null)
        {
            SelectedNode = node;
        }
    }
    internal void NativeNodeText(TreeNode node, string? text)
    {
        if (node != null)
        {
            _store.SetValue(node.treeIter, 0, text);
        }
    }
    internal void NativeNodeImage(TreeNode node, int index)
    {
        if (node != null)
        {
            _store.SetValue(node.treeIter, 2, index);
        }
    }
    internal void NativeNodeImage(TreeNode node, string? key)
    {
        if (node != null)
        {
            _store.SetValue(node.treeIter, 3, key);
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
    private void CellName_Toggled(object? o, ToggledArgs args)
    {
        //Console.WriteLine("CellRendererToggle CellName_Toggled");
        var path = new TreePath(args.Path);
        var model = _store;
        model.GetIter(out var iter, path);
        var val = (bool)model.GetValue(iter, 1);
        model.SetValue(iter, 1, val == false);
    }
    private ImageList? _imageList;
    public ImageList? ImageList
    {
        get => _imageList;
        set
        {
            _imageList = value;
            rendererPixbuf.Visible = _imageList != null;
            if (_imageList != null)
            {
                var column = self.treeView.Columns[0];
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
        self.treeView.ExpandAll();
    }
    public void CollapseAll()
    {
        self.treeView.CollapseAll();
    }
    internal void SetExpandNode(TreeNode node, bool all)
    {
        self.treeView.ExpandRow(_store.GetPath(node.treeIter), all);
    }
    internal void SetCollapseNode(TreeNode node)
    {

        self.treeView.CollapseRow(_store.GetPath(node.treeIter));
    }
    internal bool GetNodeExpanded(TreeNode node)
    {
        return self.treeView.GetRowExpanded(_store.GetPath(node.treeIter));
    }
    public bool ShowLines { get => self.treeView.EnableTreeLines; set { self.treeView.EnableTreeLines = true; self.treeView.EnableGridLines = TreeViewGridLines.Horizontal; } }
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
            if (self.treeView.Selection.GetSelected(out _))
            {
                var paths = self.treeView.Selection.GetSelectedRows();
                TreeNode? result = null;
                GetNodeChild(root, paths[0].Indices, ref result);
                return result;
            }

            return null;
        }
        set
        {
            if (value == null)
                self.treeView.Selection.UnselectAll();
            else
            {
                self.treeView.ExpandToPath(_store.GetPath(value.treeIter));
                self.treeView.Selection.SelectIter(value.treeIter);
            }
        }
    }
    public TreeNode TopNode
    {
        get => root;
        set
        {

        }
    }

    public event TreeViewCancelEventHandler? BeforeSelect;
    public event TreeViewEventHandler? AfterSelect;
    public event TreeViewEventHandler? AfterCollapse;
    public event TreeViewEventHandler? AfterExpand;
    private void GetNodeChild(TreeNode node, int[] indices, ref TreeNode? result)
    {
        var nodeIndex = string.Join(",", indices);
        foreach (var child in node.Nodes)
        {
            if (child.Index == nodeIndex)
            {
                result = child;
                return;
            }

            if (nodeIndex.Length >= child.Index.Length)
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
                if (value < (_treeView.ImageList?.Images.Count ?? 0))
                    Pixbuf = _treeView.ImageList?.Images[value].Pixbuf;
            }
        }
        [Property("pixbufkey")]
        public string? PixbufKey
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                    Pixbuf = _treeView.ImageList?.Images[value]?.Pixbuf;
            }
        }
    }
}