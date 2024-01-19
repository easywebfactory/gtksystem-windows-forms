using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{
	[ListBindable(false)]
	public class ListViewGroupCollection : List<ListViewGroup>
	{
		ListView _listView;
		public ListViewGroup this[string key]
		{
			get
			{
				return base.Find(w=>w.Name == key);
			}
			set
			{
                if (base.FindIndex(w => w.Name == key) > -1)
                {
                    base[FindIndex(w => w.Name == key)] = value;
                }
            }
		}

		internal ListViewGroupCollection(ListView listView)
		{
            _listView = listView;

        }

		public ListViewGroup Add(string key, string headerText)
		{
			ListViewGroup group = new ListViewGroup(key, headerText);

            AddCore(group);
			return group;
		}


		public void AddRange(ListViewGroup[] groups)
		{
            foreach (ListViewGroup group in groups)
            {
                AddCore(group);
            }
        }

		public void AddRange(ListViewGroupCollection groups)
		{
			foreach (ListViewGroup group in groups)
			{
                AddCore(group);
			}
		}
		private void AddCore(ListViewGroup group)
		{
			Add(group);
		}
		public bool Contains(string name)
		{
            return base.FindIndex(w => w.Name == name) > -1;
        }

    }
}
