using GLib;
using Gtk;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;

 
namespace System.Windows.Forms
{

	[DefaultEvent("SelectedIndexChanged")]
	public class ListView : WidgetControl<Gtk.HBox>
    {
		private ListViewItemCollection _items;
		private ListViewGroupCollection _groups;
		private ColumnHeaderCollection _columns;
        internal Gtk.FlowBox _flow;
        public ListView():base()
        {
			_items = new ListViewItemCollection(this);
			_groups = new ListViewGroupCollection(this);
			_columns = new ColumnHeaderCollection(this);
			base.Control.StyleContext.AddClass("ListView");

            base.Control.Realized += Control_Realized;
            _flow = new Gtk.FlowBox();
            _flow.StyleContext.AddClass("FlowBox");
            _flow.MaxChildrenPerLine = 100u;
            _flow.MinChildrenPerLine = 0u;
            _flow.ColumnSpacing = 5;
            _flow.SortFunc = new FlowBoxSortFunc((fbc1, fbc2) => !this.Sorted ? 0 : fbc1.TooltipText.CompareTo(fbc2.TooltipText));
            _flow.Halign = Gtk.Align.Fill;
			_flow.Valign = Gtk.Align.Fill;
            _flow.Orientation = Gtk.Orientation.Vertical;
            _flow.SelectionMode = Gtk.SelectionMode.Multiple;
            _flow.ChildActivated += _flow_ChildActivated;
            _flow.SelectedChildrenChanged += _flow_SelectedChildrenChanged;
            Gtk.ScrolledWindow scrolledWindow = new Gtk.ScrolledWindow();
            //scrolledWindow.HscrollbarPolicy = PolicyType.Never;
            Gtk.Viewport viewport = new Gtk.Viewport();
            viewport.Add(_flow);
            scrolledWindow.Add(viewport);
            this.Control.Add(scrolledWindow);
        }

        private void _flow_SelectedChildrenChanged(object sender, EventArgs e)
        {
            _flow.SelectedForeach(new FlowBoxForeachFunc((fb, fbc) => {
                if (fbc.Data["type"].ToString() == "group")
                {
                    _flow.UnselectChild(fbc);
                }
            }));
        }

        private HashSet<int> selectedIndexes = new HashSet<int>();
        private void _flow_ChildActivated(object o, ChildActivatedArgs args)
        {
            if (args.Child.Data["type"].ToString() == "group")
            {
                _flow.UnselectChild(args.Child);
            }
            else
            {
                if (ItemActivate != null)
                    ItemActivate(this, args);

                if (selectedIndexes.Contains(args.Child.Index))
                {
                    if (args.Child.IsSelected)
                    {
                        selectedIndexes.Remove(args.Child.Index);
                        _flow.UnselectChild(args.Child);
                    }
                }
                else
                {
                    selectedIndexes.Add(args.Child.Index);
                    int rowIndex = args.Child.Index;
                    ListViewItem sender = this.Items[rowIndex];
                    if (SelectedIndexChanged != null)
                        SelectedIndexChanged(sender, args);

                    if (ItemSelectionChanged != null)
                        ItemSelectionChanged(this, new ListViewItemSelectionChangedEventArgs(sender, rowIndex, args.Child.IsSelected));
                }
                if (Click != null)
                    Click(this, args);
            }
        }

        private void Control_Realized(object sender, EventArgs e)
        {
            if (Groups.Count > 0)
            {
                _flow.Orientation = Gtk.Orientation.Horizontal;
                foreach (Gtk.FlowBoxChild child in _flow.Children)
                    child.WidthRequest = Width;
                foreach(ListViewItem item in Items)
                {
                    if (item.ItemType == "group")
                        AddGroup(item.Group, -1);
                    else
                        AddItem(item, -1);
                }
            }
            this.Control.ShowAll();
        }

		public bool Sorted
        {
            get; set;
        }
        public System.Windows.Forms.ListViewAlignment Alignment { get; set; }
		public bool AllowColumnReorder { get; set; }
		//public System.Windows.Forms.BorderStyle BorderStyle { get; set; }
		//listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3});
		//    flowLayoutPanel1.SetFlowBreak(listView2, true);
		public bool GridLines { get; set; } = true;
		public ImageList GroupImageList { get; set; }
		public System.Windows.Forms.ColumnHeaderStyle HeaderStyle { get; set; }
		public bool HideSelection { get; set; }
		public bool HoverSelection { get; set; }
		public bool LabelEdit { get; set; }
		public bool LabelWrap { get; set; }
		public ImageList LargeImageList { get; set; }

        public virtual bool MultiSelect
        {
            get
            {
                return _flow.SelectionMode == Gtk.SelectionMode.Multiple;
            }
            set
            {
                if (value == true)
                {
                    _flow.SelectionMode = Gtk.SelectionMode.Multiple;
                }
                else
                {
                    _flow.SelectionMode = Gtk.SelectionMode.Single;
                }
            }
        }

        public bool OwnerDraw { get; set; }
		public bool Scrollable { get; set; }
		public bool ShowGroups { get; set; }
		public bool ShowItemToolTips { get; set; }

		public ImageList SmallImageList { get; set; }
		public System.Windows.Forms.SortOrder Sorting { get; set; }
		public ImageList StateImageList { get; set; }

		public bool UseCompatibleStateImageBehavior { get; set; }
		public System.Windows.Forms.View View { get; set; }


        public void AddItem(ListViewItem item, int position)
        {
            Gtk.HBox hBox = new Gtk.HBox();
            hBox.Valign = Gtk.Align.Start;
            hBox.Halign = Gtk.Align.Start;
            if (this.CheckBoxes == true)
            {
				Gtk.CheckButton checkbox = new Gtk.CheckButton() { WidthRequest = 20 };
                checkbox.Active = item.Checked;
                checkbox.Toggled += (object sender, EventArgs e) => {
                    Gtk.CheckButton box = sender as Gtk.CheckButton;
                    Gtk.FlowBoxChild boxitem = box.Parent.Parent as Gtk.FlowBoxChild;
                    ListViewItem thisitem = Items[boxitem.Index];
					thisitem.Checked = box.Active;
                    if (ItemCheck!=null)
					{
                        ItemCheck(item, new ItemCheckEventArgs(boxitem.Index, box.Active ? CheckState.Checked : CheckState.Unchecked, box.Active ? CheckState.Unchecked : CheckState.Checked));
					}
                    if (ItemChecked != null)
                    {
                        ItemChecked(item, new ItemCheckedEventArgs(thisitem));
                    }
                };
                hBox.Add(checkbox);
			}

			if (this.View == View.SmallIcon)
            {
                if (this.SmallImageList != null)
                {
                    this.SmallImageList.ImageSize = new Size(16, 16);
                    if (!string.IsNullOrWhiteSpace(item.ImageKey))
                    {
                        Drawing.Image img = this.SmallImageList.Images[item.ImageKey];
                        hBox.Add(new Gtk.Image(img.Pixbuf)) ;
                    }
                    else if (item.ImageIndex > -1)
                    {
                        Drawing.Image img = this.SmallImageList.Images[item.ImageIndex];
                        hBox.Add(new Gtk.Image(img.Pixbuf));
                    }

                }
                hBox.Add(new Gtk.Label(item.Text) { Xalign = 0, Halign = Gtk.Align.Start, Valign = Gtk.Align.Start }); 
            }
			else
			{
                Gtk.VBox vBox = new Gtk.VBox();
                if (this.LargeImageList != null)
                {
                    this.SmallImageList.ImageSize = new Size(100, 100);
                    if (!string.IsNullOrWhiteSpace(item.ImageKey))
                    {
                        Drawing.Image img = this.LargeImageList.Images[item.ImageKey];
                        int width = img.Pixbuf.Width;
                        int height = img.Pixbuf.Height;

                        vBox.Add(new Gtk.Image(new Gdk.Pixbuf(img.PixbufData)) { WidthRequest = Math.Min(50, width), HeightRequest = Math.Min(50, height) });
                    }
                    else if (item.ImageIndex > -1)
                    {
                        Drawing.Image img = this.LargeImageList.Images[item.ImageIndex];
                        int width = img.Pixbuf.Width;
                        int height = img.Pixbuf.Height;

                        vBox.Add(new Gtk.Image(img.Pixbuf) { WidthRequest = Math.Min(50, width), HeightRequest = Math.Min(50, height) });
                    }
                }
                vBox.Add(new Gtk.Label(item.Text) { Xalign = 0, Halign = Gtk.Align.Center, Valign = Gtk.Align.Center }); ;
                hBox.Add(vBox);
            }
            

            Gtk.FlowBoxChild boxitem = new Gtk.FlowBoxChild();
            boxitem.TooltipText = item.Text;
            boxitem.Data.Add("type", "item");
            boxitem.Add(hBox);
            if (position < 0)
            {
                this._flow.Add(boxitem);
            }
            else
            {
                this._flow.Insert(boxitem, position);
            }
        }
        public void AddGroup(ListViewGroup group, int position)
        {
            if (group == null)
                return;
            Gtk.HBox hBox = new Gtk.HBox();
            hBox.Valign = Gtk.Align.Fill;
            hBox.Halign = Gtk.Align.Fill;
            var bb = new Gtk.Label(group.Name) { Xalign = 0, Halign = Gtk.Align.Fill, Valign = Gtk.Align.Fill };
            bb.MarginTop = 10;
            bb.StyleContext.AddClass("listviewgroup");
            hBox.Add(bb);

            Gtk.FlowBoxChild boxitem = new Gtk.FlowBoxChild();
            boxitem.WidthRequest = Width;
            boxitem.Valign = Gtk.Align.Fill;
            boxitem.Halign = Gtk.Align.Fill;
            boxitem.TooltipText = group.Subtitle;
            boxitem.Data.Add("type", "group");
           // boxitem.StyleContext.AddClass("listviewgroup");
            boxitem.Add(hBox);

            if (position < 0)
            {
                this._flow.Add(boxitem);
            }
            else
            {
                this._flow.Insert(boxitem, position);
            }
        }
        public class CheckedIndexCollection : List<int>
		{
			 
			public CheckedIndexCollection(ListView owner)
			{
				 
			}

		}

		[ListBindable(false)]
		public class CheckedListViewItemCollection : List<ListViewItem>
		{
			
			public virtual ListViewItem this[string key]
			{
				get
				{
					return this.Find(w => w.Name == key);
				}
			}
			 
			public CheckedListViewItemCollection(ListView owner)
			{
				
			}

		}

		[ListBindable(false)]
		public class ColumnHeaderCollection : List<ColumnHeader>
        {
			public ColumnHeaderCollection(ListView owner)
            {
                 
            }

			public virtual ColumnHeader this[string key]
			{
				get
				{
					return base.Find(o => o.Name == key);
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}


			public virtual void RemoveByKey(string key)
			{
				base.Remove(base.Find(o=> o.Name == key));
			}

			public virtual int IndexOfKey(string key)
			{
				return base.FindIndex(o=> o.Name == key);
			}

			public virtual ColumnHeader Add(string text, int width, HorizontalAlignment textAlign)
			{
                return Add("", text, width, textAlign, "");
            }

			public virtual ColumnHeader Add(string text)
			{
                return Add("", text, 60, HorizontalAlignment.Left, "");
            }

			public virtual ColumnHeader Add(string text, int width)
			{
                return Add("", text, width, HorizontalAlignment.Left, "");
            }

			public virtual ColumnHeader Add(string key, string text)
			{
                return Add(key, text, 60, HorizontalAlignment.Left, "");
            }

			public virtual ColumnHeader Add(string key, string text, int width)
			{
				return Add(key, text, width, HorizontalAlignment.Left, "");
            }

			public virtual ColumnHeader Add(string key, string text, int width, HorizontalAlignment textAlign, string imageKey)
			{
                ColumnHeader header = new ColumnHeader();
                header.Name = key;
                header.Text = text;
                header.Width = width;
                header.TextAlign = textAlign;
                header.ImageKey = imageKey;
                header.ImageIndex = -1;
                base.Add(header);
                return header;
            }

			public virtual ColumnHeader Add(string key, string text, int width, HorizontalAlignment textAlign, int imageIndex)
			{
                ColumnHeader header = new ColumnHeader();
                header.Name = key;
                header.Text = text;
                header.Width = width;
                header.TextAlign = textAlign;
				header.ImageIndex = imageIndex;
                base.Add(header);
                return header;
            }

			public virtual void AddRange(ColumnHeader[] values)
			{
				base.AddRange(values);
			}

			public virtual bool ContainsKey(string key)
			{
				return base.Contains(base.Find(o=>o.Name==key));
			}


			public void Insert(int index, string text, int width, HorizontalAlignment textAlign)
			{
                Insert(index, "", text, width, textAlign, null);
            }

			public void Insert(int index, string text)
			{
                Insert(index, "", text, 0, HorizontalAlignment.Center, null);
            }

			public void Insert(int index, string text, int width)
			{
                Insert(index, "", text, width, HorizontalAlignment.Center, null);
            }

			public void Insert(int index, string key, string text)
			{
                Insert(index, key, text, 0, HorizontalAlignment.Center, null);

            }

			public void Insert(int index, string key, string text, int width)
			{
				Insert(index, key, text, width, HorizontalAlignment.Center, null);
            }

			public void Insert(int index, string key, string text, int width, HorizontalAlignment textAlign, string imageKey)
			{
                ColumnHeader header = new ColumnHeader();
                header._index = index;
                header.DisplayIndex = index;
                header.Name = key;
                header.Text = text;
                header.Width = width;
                header.TextAlign = textAlign;
                header.ImageKey = imageKey;
                base.Insert(index, header);
            }

			public void Insert(int index, string key, string text, int width, HorizontalAlignment textAlign, int imageIndex)
			{
                ColumnHeader header = new ColumnHeader();
				header._index = index;
                header.DisplayIndex = index;
                header.Name = key;
                header.Text = text;
                header.Width = width;
                header.TextAlign = textAlign;
                header.ImageIndex = imageIndex;
				base.Insert(index, header);
            }
		}

		[ListBindable(false)]
		public class ListViewItemCollection : List<ListViewItem>
		{
			ListView _owner;
			public virtual ListViewItem this[string key]
			{
				get
				{
					return base.Find(w => w.Name == key);
				}
			}

            public ListViewItemCollection(ListView owner)
			{
				_owner = owner;
            }
            public new void Add(ListViewItem item)
            {
                AddCore(item, -1);
            }
			public virtual ListViewItem Add(string text)
			{
				return Add("", text, -1);
            }

			public virtual ListViewItem Add(string text, int imageIndex)
			{
                return Add("", text, imageIndex);
            }

			public virtual ListViewItem Add(string text, string imageKey)
			{
				return Add("", text, imageKey);

            }

			public virtual ListViewItem Add(string key, string text, string imageKey)
			{
				ListViewItem item = new ListViewItem(text,imageKey);
                item.Name = key;
				item.Text = text;
                AddCore(item, -1);
                return item;
            }

			public virtual ListViewItem Add(string key, string text, int imageIndex)
			{
                ListViewItem item = new ListViewItem(text,imageIndex);
                item.Name = key;
                AddCore(item, -1);
                return item;
            }

            public void AddCore(ListViewItem item, int position)
			{
                item.ItemType = "item";
                if (position < 0)
                {
                    item.Index = Count;
                    base.Add(item);
                    for (int i = 0; i < Count; i++)
                        this[i].Index = i;

                    
                    if (_owner.Control.IsRealized)
                    {
                        _owner.AddItem(item, position);
                        _owner.Control.ShowAll();
                    }
                }
                else
                {
                    item.Index = position;
                    base.Insert(position, item);
                  
                    if (_owner.Control.IsRealized)
                    {
                        _owner.AddItem(item, position);
                        _owner.Control.ShowAll();
                    }
                }
            }
            public void AddGroup(ListViewGroup group, int position)
            {
                if (_owner.Groups.Count > 0)
                {
                    ListViewItem item = new ListViewItem(group);
                    item.ItemType = "group";
                    if (position < 0)
                    {
                        item.Index = Count;
                        base.Add(item);
                        for (int i = 0; i < Count; i++)
                            this[i].Index = i;
                        
                        if (_owner.Control.IsRealized)
                        {
                            _owner.AddGroup(group, position);
                            _owner.Control.ShowAll();
                        }
                    }
                    else
                    {
                        item.Index = position;
                        base.Insert(position, item);
                       
                        if (_owner.Control.IsRealized)
                        {
                            _owner.AddGroup(group, position);
                            _owner.Control.ShowAll();
                        }
                    }

                }
            }
            public void AddRange(ListViewItem[] items)
			{
              var group =  items.GroupBy(g => g.Group);
                foreach (var g in group)
                {
                    AddGroup(g.Key,-1);
                    foreach (ListViewItem item in g)
                    {
                        AddCore(item,-1);
                    }
                }

                   
            }

			public void AddRange(ListViewItemCollection items)
			{
				 foreach(ListViewItem item in items)
                    AddCore(item,-1);
			}

			public virtual bool ContainsKey(string key)
			{
				return base.FindIndex(w => w.Name == key) > -1;
            }

			public void CopyTo(Array dest, int index)
			{
				throw null;
			}

			public ListViewItem[] Find(string key, bool searchAllSubItems)
			{
				if(searchAllSubItems)
					return base.FindAll(w => w.Name == key && w.SubItems.ContainsKey(key)).ToArray();
				else
                    return base.FindAll(w => w.Name == key).ToArray();
            }

			public virtual int IndexOfKey(string key)
			{
				return base.FindIndex(w => w.Name == key);
			}

			public ListViewItem Insert(int index, string text)
			{
                return Insert(index, "", text, -1);
            }

			public ListViewItem Insert(int index, string text, int imageIndex)
			{
                return Insert(index, "", text, imageIndex);
            }

			public ListViewItem Insert(int index, string text, string imageKey)
			{
				return Insert(index, "", text, imageKey);
            }

			public virtual ListViewItem Insert(int index, string key, string text, string imageKey)
			{
                ListViewItem item = new ListViewItem(text, imageKey);
                item.Name = key;
                base.Insert(index, item);
                return item;
            }

			public virtual ListViewItem Insert(int index, string key, string text, int imageIndex)
			{
                ListViewItem item = new ListViewItem(text, imageIndex);
                item.Name = key;
				base.Insert(index, item);
				return item;
			}

			public virtual void RemoveByKey(string key)
			{
				base.Remove(base.Find(w => w.Name == key));
			}

		}

		[ListBindable(false)]
		public class SelectedIndexCollection : List<int>
		{
			
		}

		[ListBindable(false)]
		public class SelectedListViewItemCollection : List<ListViewItem>
        {
		
		}


		public ItemActivation Activation { get; set; }

        public bool CheckBoxes { get; set; }

        public CheckedIndexCollection CheckedIndices
		{
			get
			{
                CheckedIndexCollection selecteditems = new CheckedIndexCollection(this);
                foreach (Gtk.FlowBoxChild child in _flow.AllChildren)
                {
                    ListViewItem itm = Items[child.Index];
                    itm.Selected = child.IsSelected;
                    if (itm.Checked)
                    {
                        selecteditems.Add(child.Index);
                    }
                }
                return selecteditems;
            }
		}

		public CheckedListViewItemCollection CheckedItems
		{
			get
			{
                CheckedListViewItemCollection selecteditems = new CheckedListViewItemCollection(this);
                foreach (Gtk.FlowBoxChild child in _flow.AllChildren)
                {
					ListViewItem itm = Items[child.Index];
					itm.Selected = child.IsSelected;
                    if (itm.Checked)
                    {
                        selecteditems.Add(itm);
                    }
                }
                return selecteditems;
            }
		}
		public ColumnHeaderCollection Columns
		{
			get
			{
				return _columns;
			}
		}

		public bool FullRowSelect { get; set; }

        public ListViewGroupCollection Groups
		{
			get
			{
                return _groups;
            }
		}

		public ListViewItemCollection Items
		{
			get
			{
                return _items;
            }
		}

		public IComparer ListViewItemSorter
		{
			get;
			set;
		}

		public SelectedIndexCollection SelectedIndices
		{
			get
			{
                SelectedIndexCollection selecteditems = new SelectedIndexCollection();
                foreach (Gtk.FlowBoxChild child in _flow.AllChildren)
                {
                    Items[child.Index].Selected = child.IsSelected;
                    if (child.IsSelected)
                    {
                        selecteditems.Add(child.Index);
                    }
                }
                return selecteditems;
            }
		}

		public SelectedListViewItemCollection SelectedItems
		{
			get
			{
				SelectedListViewItemCollection selecteditems = new SelectedListViewItemCollection();
                foreach (Gtk.FlowBoxChild child in _flow.AllChildren)
                {
                    ListViewItem itm = Items[child.Index];
                    itm.Selected = child.IsSelected;
                    if (child.IsSelected)
                    {
                        selecteditems.Add(itm);
                    }
                }
				return selecteditems;
            }
		}

		public void Clear()
		{
            foreach (Gtk.FlowBoxChild child in _flow.Children)
                _flow.Remove(child);

            Items.Clear();
		}


		public ListViewItem FindItemWithText(string text)
		{
            return Items.Find(w => w.Text == text);
        }

		public ListViewItem FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex)
		{
            int idx = Items.FindIndex(startIndex, w => w.Text == text);
            return idx == -1 ? null : Items[idx];
        }

		public ListViewItem FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex, bool isPrefixSearch)
		{
            int idx = Items.FindIndex(startIndex, w => w.Text == text);
            return idx == -1 ? null : Items[idx];
        }

		public ListViewItem GetItemAt(int x, int y)
		{
			throw null;
		}

		public Rectangle GetItemRect(int index)
		{
			throw null;
		}

		public void Sort()
		{
			 
		}

		public event ColumnClickEventHandler ColumnClick;
		public event ItemCheckEventHandler ItemCheck;
        public event ItemCheckedEventHandler ItemChecked;
        public event ListViewItemSelectionChangedEventHandler ItemSelectionChanged;
		public event EventHandler SelectedIndexChanged;
		public override event EventHandler Click;
		public event EventHandler ItemActivate;
    }
}
