using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
namespace System.Windows.Forms
{
	
	public class ListViewItem : ICloneable, ISerializable, IKeyboardToolTip
	{
		private ListViewSubItemCollection _subitems;
        public class ListViewSubItem
		{
			public Color BackColor
			{
				get;
				set;
			}

			
			public Rectangle Bounds
			{
                get;
                set;
            }

			public Font Font
			{
                get;
                set;
            }

			public Color ForeColor
			{
                get;
                set;
            }
			 
			public object Tag
			{
                get;
                set;
            }

			
			public string Text
			{
                get;
                set;
            }

			
			public string Name
			{
                get;
                set;
            }
            public ListViewItem ListViewItem
            {
                get;
                set;
            }
            public ListViewSubItem()
			{
				 
			}

			public ListViewSubItem(ListViewItem owner, string text)
			{
                ListViewItem = owner;
                this.Text = text;
			}

			public ListViewSubItem(ListViewItem owner, string text, Color foreColor, Color backColor, Font font)
			{
                ListViewItem = owner;
                this.Text= text;
				this.ForeColor = foreColor;
				this.BackColor = backColor;
				this.Font = font;
			}
			 
			public void ResetStyle()
			{
				 
			}
			 
		}

		public class ListViewSubItemCollection : List<ListViewSubItem>
		{
			private ListViewItem _owner;
            public virtual ListViewSubItem this[string key]
            {
				get
				{
					return base.Find(w => w.Name == key);
                }
			}

			public ListViewSubItemCollection(ListViewItem owner)
			{
				_owner = owner;
            }

			public ListViewSubItem Add(string text)
			{
                ListViewSubItem sub = new ListViewSubItem(_owner, text);
				Add(sub);
				return sub;
			}

			public ListViewSubItem Add(string text, Color foreColor, Color backColor, Font font)
			{
                ListViewSubItem sub = new ListViewSubItem(_owner, text, foreColor, backColor, font);
                Add(sub);
                return sub;
            }

			public void AddRange(ListViewSubItem[] items)
			{
				foreach(ListViewSubItem item in items)
					Add(item);
			}

			public void AddRange(string[] items)
			{
				 foreach(string item in items)
					Add(item);
			}

			public void AddRange(string[] items, Color foreColor, Color backColor, Font font)
			{
                foreach (string item in items)
                    Add(item, foreColor, backColor, font);
            }
			 
			public virtual bool ContainsKey(string key)
			{
				return base.FindIndex(w => w.Name == key) > -1;

            }

			public virtual int IndexOfKey(string key)
			{
				return base.FindIndex(w => w.Name == key);

            }

			public virtual void RemoveByKey(string key)
			{
                RemoveAt(base.FindIndex(w => w.Name == key));
            }
		}

		internal ListView _listView;

		internal ListViewGroup _group;

		internal int ID;

		internal virtual AccessibleObject AccessibilityObject
		{
			get;
		}


        internal string ItemType
        {
            get;
            set;
        }
        public Color BackColor
		{
            get;
            set;
        }

		
		public Rectangle Bounds
		{
			get;
		}

		
		
		
		public bool Checked
		{
            get;
            set;
        }

		
		
		public bool Focused
		{
            get;
            set;
        }

		
		
		
		public Font Font
		{
            get;
            set;
        }

		
		
		public Color ForeColor
		{
            get;
            set;
        }

		
		
		
		public ListViewGroup Group
		{
			get;
			set;
		}

		
		
		
		
		
		
		
		
		public int ImageIndex
		{
            get;
            set;
        }

		internal ListViewItemImageIndexer ImageIndexer
		{
			get;
		}

		
		
		
		
		
		
		
		public string ImageKey
		{
            get;
            set;
        }

		
		public ImageList ImageList
		{
            get;
            internal set;
        }

		
		
		
		public int IndentCount
		{
            get;
            set;
        }

		
		public int Index
		{
			get;
			internal set;
		}

		
		public ListView ListView
		{
            get;
            internal set;
        }

		
		
		
		public string Name
		{
			get;
			set;
		}

		
		
		
		public Point Position
		{
            get;
            set;
        }
		 
		public bool Selected
		{
            get;
            set;
        }

		
		
		
		
		
		
		
		
		public int StateImageIndex
		{
            get;
            set;
        }

		internal bool StateImageSet
		{
			get;
		}

		internal bool StateSelected
		{
            get;
            set;
        }

		
		
		
		
		public ListViewSubItemCollection SubItems
		{
            get;
            internal set;
        }

		
		
		
		
		
		
		public object Tag
		{
            get;
            set;
        }




		public string Text
		{
			get;
			set;
		} = string.Empty;

		
		
		public string ToolTipText
		{
            get;
            set;
        }

		
		
		public bool UseItemStyleForSubItems
		{
            get;
            set;
        }

		public ListViewItem()
		{
            InitListViewItem("", -1, "", Color.Black, Color.Black, null, null);
        }

		protected ListViewItem(SerializationInfo info, StreamingContext context)
		{
			 
		}

		public ListViewItem(string text)
		{
            InitListViewItem(text, -1, "", Color.Black, Color.Black, null, null);
        }

		public ListViewItem(string text, int imageIndex)
		{
            InitListViewItem(text, imageIndex, "", Color.Black, Color.Black, null, null);
        }

		public ListViewItem(string[] items)
		{
            InitListViewItem(items, -1, "", Color.Black, Color.Black, null, null);
        }

		public ListViewItem(string[] items, int imageIndex)
		{
            InitListViewItem(items, imageIndex, "", Color.Black, Color.Black, null, null);
        }

		public ListViewItem(string[] items, int imageIndex, Color foreColor, Color backColor, Font font)
		{
            InitListViewItem(items, imageIndex, "", foreColor, backColor, font, null);
        }

		public ListViewItem(ListViewSubItem[] subItems, int imageIndex)
		{
            foreach (ListViewSubItem item in subItems)
                _subitems.Add(item);
            InitListViewItem("", imageIndex, "", Color.Black, Color.Black, null, null);
        }

		public ListViewItem(ListViewGroup group)
		{
            InitListViewItem("", -1, "", Color.Black, Color.Black, null, group);
        }

		public ListViewItem(string text, ListViewGroup group)
		{
            InitListViewItem(text, -1, null, Color.Black, Color.Black, null, group);
        }

		public ListViewItem(string text, int imageIndex, ListViewGroup group)
		{
            InitListViewItem(text, imageIndex, null, Color.Black, Color.Black, null, group);
        }

		public ListViewItem(string[] items, ListViewGroup group)
		{
            InitListViewItem(items, -1, "", Color.Black, Color.Black, null, group);
        }

		public ListViewItem(string[] items, int imageIndex, ListViewGroup group)
		{
            InitListViewItem(items, imageIndex, "", Color.Black, Color.Black, null, group);
        }

		public ListViewItem(string[] items, int imageIndex, Color foreColor, Color backColor, Font font, ListViewGroup group)
		{
            InitListViewItem(items, imageIndex, "", foreColor, backColor, font, group);
        }

		public ListViewItem(ListViewSubItem[] subItems, int imageIndex, ListViewGroup group)
		{
            foreach (ListViewSubItem item in subItems)
                _subitems.Add(item);
            InitListViewItem(new string[0], imageIndex, "", Color.Black, Color.Black, null, group);
        }

		public ListViewItem(string text, string imageKey)
		{
            InitListViewItem(text, -1, imageKey, Color.Black, Color.Black, null, null);
        }

		public ListViewItem(string[] items, string imageKey) 
		{
            InitListViewItem(items, -1, imageKey, Color.Black, Color.Black, null, null);
        }

		public ListViewItem(string[] items, string imageKey, Color foreColor, Color backColor, Font font)
        {
            InitListViewItem(items, -1, imageKey, foreColor, backColor, font, null);
        }

		public ListViewItem(ListViewSubItem[] subItems, string imageKey)
		{
            foreach (ListViewSubItem item in subItems)
                _subitems.Add(item);
            InitListViewItem(new string[0], -1, imageKey, Color.Black, Color.Black, null, null);
        }

		public ListViewItem(string text, string imageKey, ListViewGroup group)
		{
            InitListViewItem(text, -1, imageKey, Color.Black, Color.Black, null, group);
        }

		public ListViewItem(string[] items, string imageKey, ListViewGroup group)
        {
            InitListViewItem(items, -1, imageKey, Color.Black, Color.Black, null, group);
        }

		public ListViewItem(string[] items, string imageKey, Color foreColor, Color backColor, Font font, ListViewGroup group)
		{
			InitListViewItem(items, -1, imageKey, foreColor, backColor, font, group);
        }

		public ListViewItem(ListViewSubItem[] subItems, string imageKey, ListViewGroup group)
		{
			 foreach(ListViewSubItem item in subItems)
				_subitems.Add(item);
			InitListViewItem("", -1, imageKey, Color.Black, Color.Black, null, group);
        }
        internal void InitListViewItem(string text, int imageIndex, string imageKey, Color foreColor, Color backColor, Font font, ListViewGroup group)
        {
            _subitems = new ListViewSubItemCollection(this);
			SubItems = _subitems;
            this.Text = text;
			this.ImageIndex = imageIndex;
            this.ImageKey = imageKey;
            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Font = font;
            this.Group = group;
			if (group == null)
			{
				this.Group = ListViewGroup.GetDefaultListViewGroup();
			}
        }
        internal void InitListViewItem(string[] items, int imageIndex, string imageKey, Color foreColor, Color backColor, Font font, ListViewGroup group)
        {
			InitListViewItem(items[0], imageIndex, imageKey, foreColor, backColor, font, group);
            foreach (string item in items)
                _subitems.Add(item);

        }
		//internal string RelateFlowBoxChildKey { get; set; }
        public void BeginEdit()
		{
			 
		}

		public virtual object Clone()
		{
			throw null;
		}

		public virtual void EnsureVisible()
		{
			 
		}
		 
		public ListViewSubItem GetSubItemAt(int x, int y)
		{
			return _subitems[x];
		}

		internal void Host(ListView parent, int id, int index)
		{
			
		}
		  
		public virtual void Remove()
		{
			
		}

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
           
        }
    }
}
