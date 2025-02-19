
using System.Collections;
using System.Drawing;
using System.Runtime.Serialization;
namespace System.Windows.Forms
{

    public class ListViewItem : ICloneable, ISerializable, IKeyboardToolTip
	{
		private ListViewSubItemCollection _subitems;
        public class ListViewSubItem
		{
			internal Gtk.Label _label;
			public Color? BackColor
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

			public Color? ForeColor
			{
                get;
                set;
            }
			 
			public object Tag
			{
                get;
                set;
            }


            internal string _text = string.Empty;
            public string Text
            {
                get => _text;
                set
                {
                    _text = value;
                    if (_label != null)
                        _label.Text = value;
                }
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
		internal Gtk.FlowBoxChild _flowBoxChild;
		internal virtual AccessibleObject AccessibilityObject
		{
			get;
		}


        internal string ItemType
        {
            get;
            set;
        }
        public Color? BackColor
		{
            get;
            set;
        }

		
		public Rectangle Bounds
		{
			get;
		}

		
		
		internal bool _checked;
		public bool Checked
		{
			get => _checked;

            set {
				_checked = value;
                if (_listView != null)
                    _listView.NativeCheckItem(this, value);
            }
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

		
		
		public Color? ForeColor
		{
            get;
            set;
        }

        public ListViewGroup Group
        {
            get { return _group; }
            set { _group = value;}
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
		internal bool _selected;
        public bool Selected
        {
            get => _selected;
			set { 
				_selected = value;
                if (_listView != null)
                    _listView.NativeSelectItem(this, value);
            }
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
            get => _subitems;
        }

		
		
		
		
		
		
		public object Tag
		{
            get;
            set;
        }

        internal string _text = string.Empty;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                if (_listView != null)
                    _listView.NativeUpdateText(this, value);

            }
        }

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
            InitListViewItem("", -1, "", null, null, null, null);
        }

		protected ListViewItem(SerializationInfo info, StreamingContext context)
		{
			 
		}

		public ListViewItem(string text)
		{
            InitListViewItem(text, -1, "", null, null, null, null);
        }

		public ListViewItem(string text, int imageIndex)
		{
            InitListViewItem(text, imageIndex, "", null, null, null, null);
        }

		public ListViewItem(string[] items)
		{
            InitListViewItem(items, -1, "", null, null, null, null);
        }

		public ListViewItem(string[] items, int imageIndex)
		{
            InitListViewItem(items, imageIndex, "", null, null, null, null);
        }

		public ListViewItem(string[] items, int imageIndex, Color foreColor, Color backColor, Font font)
		{
            InitListViewItem(items, imageIndex, "", foreColor, backColor, font, null);
        }

		public ListViewItem(ListViewSubItem[] subItems, int imageIndex)
		{
            InitListViewItem(subItems, imageIndex, "", null, null, null, null);
        }

		public ListViewItem(ListViewGroup group)
		{
            InitListViewItem("", -1, "", null, null, null, group);
        }

		public ListViewItem(string text, ListViewGroup group)
		{
            InitListViewItem(text, -1, null, null, null, null, group);
        }

		public ListViewItem(string text, int imageIndex, ListViewGroup group)
		{
            InitListViewItem(text, imageIndex, null, null, null, null, group);
        }

		public ListViewItem(string[] items, ListViewGroup group)
		{
            InitListViewItem(items, -1, "", null, null, null, group);
        }

		public ListViewItem(string[] items, int imageIndex, ListViewGroup group)
		{
            InitListViewItem(items, imageIndex, "", null, null, null, group);
        }

		public ListViewItem(string[] items, int imageIndex, Color foreColor, Color backColor, Font font, ListViewGroup group)
		{
            InitListViewItem(items, imageIndex, "", foreColor, backColor, font, group);
        }

		public ListViewItem(ListViewSubItem[] subItems, int imageIndex, ListViewGroup group)
		{
            InitListViewItem(subItems, imageIndex, "", null, null, null, group);
        }

		public ListViewItem(string text, string imageKey)
		{
            InitListViewItem(text, -1, imageKey, null, null, null, null);
        }

		public ListViewItem(string[] items, string imageKey) 
		{
            InitListViewItem(items, -1, imageKey, null, null, null, null);
        }

		public ListViewItem(string[] items, string imageKey, Color foreColor, Color backColor, Font font)
        {
            InitListViewItem(items, -1, imageKey, foreColor, backColor, font, null);
        }

		public ListViewItem(ListViewSubItem[] subItems, string imageKey)
		{
            InitListViewItem(subItems, -1, imageKey, null, null, null, null);
        }

		public ListViewItem(string text, string imageKey, ListViewGroup group)
		{
            InitListViewItem(text, -1, imageKey, null, null, null, group);
        }

		public ListViewItem(string[] items, string imageKey, ListViewGroup group)
        {
            InitListViewItem(items, -1, imageKey, null, null, null, group);
        }

		public ListViewItem(string[] items, string imageKey, Color foreColor, Color backColor, Font font, ListViewGroup group)
		{
			InitListViewItem(items, -1, imageKey, foreColor, backColor, font, group);
        }

		public ListViewItem(ListViewSubItem[] subItems, string imageKey, ListViewGroup group)
		{
            InitListViewItem(subItems, -1, imageKey, null, null, null, group);
        }
        internal void InitListViewItem(string text, int imageIndex, string imageKey, Color? foreColor, Color? backColor, Font font, ListViewGroup group)
        {
            _subitems = new ListViewSubItemCollection(this);
            _subitems.Add(text);
            this.Text = text;
			this.ImageIndex = imageIndex;
            this.ImageKey = imageKey;
            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Font = font;
            this.Group = group;
        }
        internal void InitListViewItem(string[] items, int imageIndex, string imageKey, Color? foreColor, Color? backColor, Font font, ListViewGroup group)
        {
			InitListViewItem(items.Length > 0 ? items[0] : "", imageIndex, imageKey, foreColor, backColor, font, group);
            foreach (string item in items)
                _subitems.Add(item);

        }
        internal void InitListViewItem(ListViewSubItem[] subItems, int imageIndex, string imageKey, Color? foreColor, Color? backColor, Font font, ListViewGroup group)
        {
            InitListViewItem(subItems.Length > 0 ? subItems[0].Text : "", imageIndex, imageKey, foreColor, backColor, font, group);
            foreach (ListViewSubItem item in subItems)
                _subitems.Add(item);
        }
        //internal string RelateFlowBoxChildKey { get; set; }
        public void BeginEdit()
		{
			 
		}

		public virtual object Clone()
		{
            return ((ArrayList)(new ArrayList() { this }).Clone())[0];
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
