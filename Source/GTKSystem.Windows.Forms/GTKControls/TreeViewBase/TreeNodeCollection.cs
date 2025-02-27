using Gtk;

namespace System.Windows.Forms;

public class TreeNodeCollection: List<TreeNode>
{
    private readonly TreeNode? owner;

    internal TreeNodeCollection(TreeNode? owner)
    {
        this.owner = owner;

    }
    public void AddRange(TreeNode[] nodes)
    {
        if (nodes == null)
        {
            throw new ArgumentNullException("nodes");
        }
        foreach (var node in nodes)
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
        foreach (var node in nodes)
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
        node.treeView = owner?.TreeView;
        base.Add(node);
        if (owner is { TreeView: not null })
        {
            owner.TreeView.LoadNodeValue(node, owner.treeIter);
        }
    }

    public new void Clear()
    {
        owner?.TreeView?.Clear();
        base.Clear();
    }
    public new void Remove(TreeNode node)
    {
        var indx = FindIndex(m => m.Index == node.Index);
        if (indx > -1)
            RemoveAt(indx);
    }
    public new void RemoveAt(int index)
    {
        if (owner is { TreeView: not null } && owner.TreeView.Store.GetIter(out var iter, new TreePath([index])))
        {
            owner.TreeView.Store.Remove(ref iter);
        }
        base.RemoveAt(index);
    }
}