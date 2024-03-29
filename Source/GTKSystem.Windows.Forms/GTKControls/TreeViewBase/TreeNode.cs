﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace System.Windows.Forms
{
    public class TreeNode: ICloneable, ISerializable, IEquatable<TreeNode>
    {
        private int index = 0;
        internal int Index { get { return index; } set { index = value; } }

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
        public bool Checked { get; set; }

        public string FullPath { get; set; }
        public bool IsSelected
        {
            get; set;
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
        public object Clone()
        {
            TreeNode newnode = new TreeNode(treeView);
            Array.ForEach(newnode.GetType().GetProperties(), o => { o.SetValue(this, o.GetValue(this)); });
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