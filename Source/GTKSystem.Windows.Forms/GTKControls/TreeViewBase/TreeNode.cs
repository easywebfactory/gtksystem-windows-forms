﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Windows.Forms;

public class TreeNode : ICloneable, ISerializable, IEquatable<TreeNode>
{
    // Format, union of indexes at all levels： 0,1,2,3....
    private string index = "";
    public string Index { get => index;
        internal set => index = value ?? "";
    }
    internal Gtk.TreeIter TreeIter = Gtk.TreeIter.Zero;
    private TreeNode? parent;
    internal TreeView? treeView;

    internal TreeView? TreeView
    {
        get
        {
            if (treeView == null)
            { treeView = parent?.TreeView; }
            return treeView;
        }
    }
    private readonly TreeNodeCollection nodes;
    public TreeNode()
    {
        nodes = new TreeNodeCollection(this);
    }
    public TreeNode(string text) : this()
    {
        Text = text;
    }
    public TreeNode(string text, TreeNode[] children) : this()
    {
        Text = text;
        Nodes.AddRange(children);
    }
    public TreeNode(TreeView? view) : this()
    {
        treeView = view;
    }
    public TreeNode(TreeNode? node) : this()
    {
        parent = node;
        treeView = node?.TreeView;
    }
    public TreeNode(string text, int pImageIndex, int pSelectedImageIndex) : this(text)
    {
        ImageIndex = pImageIndex;
        SelectedImageIndex = pSelectedImageIndex;
    }

    public TreeNodeCollection Nodes => nodes;

    public TreeNode? Parent
    {
        get => parent;
        internal set => parent = value;
    }
    private string _text = string.Empty;
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            TreeView?.NativeNodeText(this, value);
        }
    }

    public string? ToolTipText
    {
        get; set;
    }

    public string Name
    {
        get;
        set;
    } = string.Empty;

    private bool isChecked;
    public bool Checked
    {
        get => isChecked; set { isChecked = value; TreeView?.NativeNodeChecked(this, value); }
    }

    public string FullPath
    {
        get
        {
            List<string?> paths = [];
            GetFullPath(this, paths);
            return string.Join("/", paths);
        }
        set { }
    }
    protected void GetFullPath(TreeNode? node, List<string?> paths)
    {
        paths.Add(node?.Text);
        if (node?.Parent != null && node.Parent.index != "-1")
        {
            GetFullPath(node.Parent, paths);
        }
    }
    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            isSelected = value;
            TreeView?.NativeNodeSelected(this, value);
        }
    }
    public bool IsExpanded => TreeView?.GetNodeExpanded(this) == true;

    public int Level
    {
        get
        {
            if (parent == null)
                return 0;
            if (TreeView != null && parent.Equals(TreeView.root))
                return 0;
            return parent.Level + 1;
        }
    }
    private int _imageIndex;
    public int ImageIndex
    {
        get => _imageIndex;
        set { _imageIndex = value; TreeView?.NativeNodeImage(this, value); }
    }
    private string? _imageKey;
    public string? ImageKey
    {
        get => _imageKey;
        set { _imageKey = value; TreeView?.NativeNodeImage(this, value); }
    }
    public int SelectedImageIndex { get; set; }
    public string? SelectedImageKey { get; set; }
    public int StateImageIndex { get; set; }
    public string? StateImageKey { get; set; }
    public object? Tag { get; set; }

    public void Expand()
    {
        TreeView?.SetExpandNode(this, false);
    }
    public void ExpandAll()
    {
        TreeView?.SetExpandNode(this, true);
    }
    public void Collapse()
    {
        TreeView?.SetCollapseNode(this);
    }
    public void Remove()
    {
        TreeView?.RemoveNode(this);
    }
    public object Clone()
    {
        return null!;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        //throw new NotImplementedException();
    }

    public bool Equals(TreeNode? other)
    {
        return other != null && other.Index == Index && other.Name == Name && other.Text == Text && other.Level == Level;
    }
}