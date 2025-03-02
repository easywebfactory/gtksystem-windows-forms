namespace System.Windows.Forms;

internal class ListViewGroupItemCollection : List<ListViewItem>
{
	
    public ListViewGroupItemCollection(ListViewGroup group)
    {
			
    }

    public void AddRange(ListViewItem[] items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }
}