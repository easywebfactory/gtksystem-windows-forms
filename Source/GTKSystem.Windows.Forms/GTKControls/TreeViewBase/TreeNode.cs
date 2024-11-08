// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using GLib;

namespace System.Windows.Forms
{
    public class TreeNode: ICloneable, ISerializable, IEquatable<TreeNode>
    {
        //格式，各级索引并集：1,2,3....
        private string index = "";
        internal string Index { get { return index; } set { index = value; } }
        internal Gtk.TreeIter TreeIter = Gtk.TreeIter.Zero;
        private TreeNode parent;
        internal TreeView treeView;
        internal TreeView TreeView { get { return treeView; } }
        private TreeNodeCollection nodes;
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
        public TreeNode(TreeView view) : this()
        {
            this.treeView = view;
        }
        public TreeNode(TreeNode node) : this()
        {
            this.parent = node;
            this.treeView = node.TreeView;
        }
        public TreeNodeCollection Nodes
        {
            get
            {
                return nodes;
            }
        }

        public TreeNode Parent
        {
            get
            {
                TreeView treeView = TreeView;
                if (treeView != null && parent == treeView.root)
                {
                    return null;
                }

                return parent;
            }
            internal set { parent = value; }
        }
        public string Text
        {
            get;set;
        }


        public string ToolTipText
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }
        private bool _IsChecked;
        public bool Checked
        {
            get => _IsChecked; set { _IsChecked = value; if (this.treeView != null) { this.treeView.SetChecked(this, value); } }
        }

        public string FullPath { get; set; }
        private bool _IsSelected;
        public bool IsSelected
        {
            get=> _IsSelected; set { _IsSelected = value; if (this.treeView != null) { this.treeView.SetSelected(this, value); } }
        }
        public bool IsExpanded
        {
            get
            {
                if (this.treeView != null)
                {
                    return this.treeView.GetNodeExpanded(this);
                }
                else
                    return false;
            }
        }

        public int Level
        {
            get
            {
                if (Parent == null)
                {
                    return 0;
                }

                return Parent.Level + 1;
            }
        }
        public int ImageIndex { get; set; }
        public string ImageKey { get; set; }
        public int SelectedImageIndex { get; set; }
        public string SelectedImageKey { get; set; }
        public int StateImageIndex { get; set; }
        public string StateImageKey { get; set; }
        public void Expand(){
            if (this.treeView != null) { this.treeView.SetExpandNode(this, false); }
        }
        public void ExpandAll()
        {
            if (this.treeView != null) { this.treeView.SetExpandNode(this, true); }
        }
        public void Collapse()
        {
            if (this.treeView != null) { this.treeView.SetCollapseNode(this); }
        }
        public object Clone()
        {
            TreeNode newnode = new TreeNode(treeView);
            Reflection.PropertyInfo[] props = newnode.GetType().GetProperties(Reflection.BindingFlags.Public | Reflection.BindingFlags.Instance);
            foreach(var pro in props)
            {
                if (pro.GetSetMethod()!=null)
                    pro.SetValue(newnode, pro.GetValue(this));
            }
            return newnode;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //throw new NotImplementedException();
        }

        public bool Equals([AllowNull] TreeNode other)
        {
            return other.Index == this.Index && other.Text == this.Text && other.Level == this.Level;
        }
    }
}