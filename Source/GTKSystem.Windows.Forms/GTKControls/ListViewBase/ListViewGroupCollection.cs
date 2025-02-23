using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
        public new int Add(ListViewGroup group)
        {
            AddCore(group);
			return Count;
        }
        public ListViewGroup Add(string key, string headerText)
		{
			ListViewGroup group = new ListViewGroup(key, headerText);
            this.Add(group);
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
			if (this.Exists(m => m.Name == group.Name))
			{
				//Console.WriteLine("新增Group.Name重复");
				// throw new ArgumentException("Group.Name重复");
				return;
			}
            group.ListView = _listView;
            base.Add(group);

            _listView.NativeGroupAdd(group, -1);
		}
        public new void Clear()
		{
			_listView.NativeGroupsClear();
            base.Clear();
		}
		public bool Contains(string name)
		{
            return base.FindIndex(w => w.Name == name) > -1;
        }
    }
}
