// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Gtk;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace System.Windows.Forms
{
    public partial class TreeNode: MarshalByRefObject, ICloneable, ISerializable
    {
        public int Index
        {
            get
            {
                if (parent == null)
                {
                    return -1;
                }
                else
                {
                    return parent.Nodes.IndexOf(this);
                }
            }
        }
        internal Gtk.TreeIter TreeIter = Gtk.TreeIter.Zero;
        private TreeNode parent;
        internal TreeView treeView;

        internal TreeView TreeView
        {
            get
            {
                if (treeView == null)
                { treeView = this.parent?.TreeView; }
                return treeView;
            }
        }
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
        public TreeNode(string text, int pImageIndex, int pSelectedImageIndex) : this(text)
        {
            ImageIndex = pImageIndex;
            SelectedImageIndex = pSelectedImageIndex;
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
            get { return parent; }
            internal set { parent = value; }
        }
        private string _text;
        public string Text
        {
            get=>_text;
            set {
                _text = value;
                TreeView?.NativeNodeText(this, value);
            }
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
            get {
                if (this.TreeIter.Equals(Gtk.TreeIter.Zero) || this.TreeView == null)
                    return _IsChecked;
                else
                {
                    return this.TreeView.GetNodeChecked(this);
                }
            } 
            set { _IsChecked = value; TreeView?.NativeNodeChecked(this, value); }
        }

        public string FullPath
        {
            get
            {
                List<string> paths=new List<string>();
                GetFullPath(this, paths);
                return string.Join("/", paths);
            }
            set { }
        }
        protected void GetFullPath(TreeNode node, List<string> paths)
        {
            if (node.Parent != null && node.Parent.Name != "__root__")
            {
                paths.Add(node.Text);
                GetFullPath(node.Parent, paths);
            }
        }
        private bool _IsSelected;
        public bool IsSelected
        {
            get {
                if (this.TreeIter.Equals(Gtk.TreeIter.Zero) || this.TreeView == null)
                    return _IsSelected;
                else
                {
                    return this.TreeView.GetNodeSelected(this);
                }
            }
            set { 
                _IsSelected = value; 
                TreeView?.NativeNodeSelected(this, value); 
            }
        }
        public bool IsExpanded
        {
            get
            {
                return TreeView?.GetNodeExpanded(this) == true;
            }
        }

        public int Level
        {
            get
            {
                if (parent == null || parent.Name == "__root__")
                {
                    return 0;
                }
                else
                {
                    return parent.Level + 1;
                }
            }
        }
        private int _imageIndex;
        public int ImageIndex { 
            get=>_imageIndex; 
            set { _imageIndex = value; TreeView?.NativeNodeImage(this, value); }
        }
        private string _imageKey;
        public string ImageKey { 
            get => _imageKey; 
            set { _imageKey = value; TreeView?.NativeNodeImage(this, value); }
        }
        public int SelectedImageIndex { get; set; }
        public string SelectedImageKey { get; set; }
        public int StateImageIndex { get; set; }
        public string StateImageKey { get; set; }
        public object Tag { get; set; }

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
            parent?.nodes?.Remove(this);
        }
        public object Clone()
        {
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //throw new NotImplementedException();
        }
    }
}