
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
        public new void Add(TreeNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            node.Parent = node;
            node.treeView = owner.TreeView;
            base.Add(node);
        }
    }
}
