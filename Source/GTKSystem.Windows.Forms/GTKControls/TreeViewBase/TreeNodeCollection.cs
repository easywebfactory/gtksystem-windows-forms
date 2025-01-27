
using Atk;
using Gtk;
using Pango;
using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    public class TreeNodeCollection: List<TreeNode>
    {
        private TreeNode owner;

        internal TreeNodeCollection(TreeNode owner)
        {
            this.owner = owner;

        }
        public void AddRange(TreeNode[] nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }
            foreach (TreeNode node in nodes)
            {
                Add(node);
            }
        }
        public new void AddRange(IEnumerable<TreeNode> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }
            foreach (TreeNode node in nodes)
            {
                Add(node);
            }
        }
        public new void Add(TreeNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            node.Parent = owner;
            node.treeView = owner.TreeView;
            base.Add(node);
            if (owner != null && owner.TreeView != null)
            {
                owner.TreeView.LoadNodeValue(node, owner.TreeIter);
            }
        }

        public new void Clear()
        {
            this.owner?.TreeView?.Clear();
            base.Clear();
        }
        public new void Remove(TreeNode node)
        {
            int indx = base.FindIndex(m => m.Index == node.Index);
            if (indx > -1)
                RemoveAt(indx);
        }
        public new void RemoveAt(int index)
        {
            if (this.owner.TreeView.Store.GetIter(out TreeIter iter, new TreePath(new int[] { index })))
            {
                this.owner.TreeView.Store.Remove(ref iter);
            }
            base.RemoveAt(index);
        }
    }
}
