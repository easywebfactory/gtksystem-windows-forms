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
            if (owner.TreeView != null)
            {
                foreach (TreeNode node in this)
                    owner.TreeView.RemoveNode(node);
            }
            base.Clear();
        }
        public new void Remove(TreeNode node)
        {
            if (owner.TreeView != null)
                this.owner.TreeView.RemoveNode(node);
            base.Remove(node);
        }
        public new void RemoveAt(int index)
        {
            if (index < this.Count)
                Remove(this[index]);
        }
    }
}
