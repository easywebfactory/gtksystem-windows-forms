using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Forms
{
	internal class ListViewGroupItemCollection : List<ListViewItem>
	{
	
		public ListViewGroupItemCollection(ListViewGroup group)
		{
			
		}

		public void AddRange(ListViewItem[] items)
		{
			foreach (ListViewItem item in items)
			{
				Add(item);
			}
		}
	}
}
