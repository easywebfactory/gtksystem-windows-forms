
using Gtk;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Threading;


namespace System.Windows.Forms
{

	[DefaultEvent("SelectedIndexChanged")]
	public class ListView : WidgetControl<Gtk.Box>
    {
		private ListViewItemCollection _items;
		private ListViewGroupCollection _groups;
		private ColumnHeaderCollection _columns;
        internal Gtk.VBox flowBoxContainer = null;
        internal Gtk.StackSwitcher header = new Gtk.StackSwitcher();
        public ListView():base(Gtk.Orientation.Vertical,0)
        {
			_items = new ListViewItemCollection(this);
			_groups = new ListViewGroupCollection(this);
			_columns = new ColumnHeaderCollection(this);
			base.Control.StyleContext.AddClass("ListView");
            header.StyleContext.AddClass("ListViewHeader");
            this.Control.PackStart(header, false, true, 0);
            base.Control.Realized += Control_Realized;
            header.Halign = Gtk.Align.Fill;
            header.Valign = Gtk.Align.Fill;
            header.HeightRequest = 20;
            header.NoShowAll = true;
            header.Visible = false;
            header.Hide();
           

            flowBoxContainer = new Gtk.VBox();
            Gtk.ScrolledWindow scrolledWindow = new Gtk.ScrolledWindow();
            //scrolledWindow.HscrollbarPolicy = PolicyType.Never;
            scrolledWindow.Add(flowBoxContainer);
            this.Control.PackStart(scrolledWindow, true, true, 1);
        }
 
        private HashSet<int> selectedIndexes = new HashSet<int>();
        private void _flow_ChildActivated(object o, Gtk.ChildActivatedArgs args)
        {

            if (ItemActivate != null)
                ItemActivate(this, args);

            ListViewItem item = this.Items.FindAll(m => m.Group.Name == args.Child.Parent.Name)[args.Child.Index];
            if (selectedIndexes.Contains(item.Index))
            {
                if (args.Child.IsSelected)
                {
                    selectedIndexes.Remove(item.Index);
                    if (args.Child.Parent is Gtk.FlowBox flow)
                        flow.UnselectChild(args.Child);
                }
            }
            else
            {
                item.Selected = true;
                selectedIndexes.Add(item.Index);
                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(this, args);

                if (ItemSelectionChanged != null)
                    ItemSelectionChanged(this, new ListViewItemSelectionChangedEventArgs(item, item.Index, args.Child.IsSelected));
            }
            if (Click != null)
                Click(this, args);

        }

        private void Control_Realized(object sender, EventArgs e)
        {
            if (this.View == View.Details)
            {
                header.NoShowAll = false;
                header.Visible = true;
                header.ShowAll();
                foreach (ColumnHeader col in Columns)
                {
                    header.Add(new Gtk.Button(col.Text ?? "") { WidthRequest = col.Width, Halign = Gtk.Align.Fill, Valign = Gtk.Align.Fill });
                }
            }

            if (this.Sorting == SortOrder.Ascending)
                Items.Sort(new Comparison<ListViewItem>((x, y) => x.Text.CompareTo(y.Text)));
            else if (this.Sorting == SortOrder.Descending)
                Items.Sort(new Comparison<ListViewItem>((x, y) => y.Text.CompareTo(x.Text)));

            var group = Items.GroupBy(g => new { g.Group.Header,g.Group.Name }).OrderBy(o => o.Key.Name);
            foreach (var g in group)
            {
                if (g.Count() > 0)
                {
                    AddGroup(g.First().Group, -1);
                    foreach (ListViewItem item in g)
                    {
                        AddItem(item, -1);
                    }
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
        private bool _MultiSelect;
        public virtual bool MultiSelect
        {
            get
            {
                return _MultiSelect;
            }
            set
            {
                foreach (var flow in flowBoxContainer.AllChildren)
                {
                    if (flow is Gtk.FlowBox _flow)
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
            Gtk.Box hBox = new Gtk.Box(Gtk.Orientation.Horizontal,0);
            hBox.Valign = Gtk.Align.Fill;
            hBox.Halign = Gtk.Align.Fill;
            hBox.Spacing = 0;
            hBox.BorderWidth = 0;
            hBox.Homogeneous = false;
           // hBox.StyleContext.AddClass("listviewgrid");
            Gtk.FlowBoxChild boxitem = new Gtk.FlowBoxChild();
            boxitem.TooltipText = item.Text;
            boxitem.Halign = Gtk.Align.Fill;
            boxitem.Valign = Gtk.Align.Fill;
            boxitem.HeightRequest = 28;
            boxitem.BorderWidth = 0;
            boxitem.Margin = 0;
            boxitem.Add(hBox);

            if (Groups.Contains(item.Group?.Name) == false)
            {
                Groups.Add(item.Group);
            }
            Gtk.FlowBox flowBox = Groups[item.Group.Name].FlowBox;
            flowBox.Halign = Gtk.Align.Fill;
            flowBox.Valign = Gtk.Align.Fill;
            flowBox.Name = item.Group.Name;
            flowBox.ColumnSpacing = 0;
            flowBox.Add(boxitem);


            if (this.CheckBoxes == true)
            {
                if (this.View == View.SmallIcon || this.View == View.LargeIcon)
                {
                    Gtk.CheckButton checkbox = new Gtk.CheckButton() { Halign = Gtk.Align.Start, WidthRequest = 20 };
                    checkbox.Active = item.Checked;
                    checkbox.Toggled += (object sender, EventArgs e) =>
                    {
                        Gtk.CheckButton box = sender as Gtk.CheckButton;
                        Gtk.FlowBoxChild boxitem = box.Parent.Parent as Gtk.FlowBoxChild;
                        ListViewItem thisitem = this.Items.FindAll(m => m.Group.Name == boxitem.Parent.Name)[boxitem.Index];
                        thisitem.Checked = box.Active;
                        if (ItemCheck != null)
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
            }

            if (this.View == View.Details)
            {
                header.Visible = true;
                flowBox.MinChildrenPerLine = 1;
                flowBox.MaxChildrenPerLine = 1;
                Gtk.Box fistcell = new Gtk.Box(Gtk.Orientation.Horizontal, 0);
                fistcell.Homogeneous = false;

                fistcell.Halign = Gtk.Align.Fill;
                fistcell.Valign = Gtk.Align.Fill;
                fistcell.Spacing = 0;
                fistcell.BorderWidth = 0;
                fistcell.WidthRequest = Columns.Count > 0 ? Columns[0].Width-2 : 998;
                fistcell.Margin = 0;

                if (this.CheckBoxes == true)
                {
                    Gtk.CheckButton checkbox = new Gtk.CheckButton() { Halign = Gtk.Align.Start };

                    checkbox.Halign = Gtk.Align.Start;
                    checkbox.Valign = Gtk.Align.Fill;
                    checkbox.Active = item.Checked;
                    checkbox.Toggled += (object sender, EventArgs e) =>
                    {
                        Gtk.CheckButton box = sender as Gtk.CheckButton;
                        Gtk.FlowBoxChild boxitem = box.Parent.Parent as Gtk.FlowBoxChild;
                        ListViewItem thisitem = this.Items.FindAll(m => m.Group.Name == boxitem.Parent.Name)[boxitem.Index];
                        thisitem.Checked = box.Active;
                        if (ItemCheck != null)
                        {
                            ItemCheck(item, new ItemCheckEventArgs(boxitem.Index, box.Active ? CheckState.Checked : CheckState.Unchecked, box.Active ? CheckState.Unchecked : CheckState.Checked));
                        }
                        if (ItemChecked != null)
                        {
                            ItemChecked(item, new ItemCheckedEventArgs(thisitem));
                        }
                    };
                    fistcell.Add(checkbox);
                }
                if (this.SmallImageList != null)
                {
                    this.SmallImageList.ImageSize = new Size(16, 16);
                    if (!string.IsNullOrWhiteSpace(item.ImageKey))
                    {
                        Drawing.Image img = this.SmallImageList.Images[item.ImageKey];

                        fistcell.Add(new Gtk.Image(img.Pixbuf) { Halign=Gtk.Align.Start,Valign=Gtk.Align.Fill });
                    }
                    else if (item.ImageIndex > -1)
                    {
                        Drawing.Image img = this.SmallImageList.Images[item.ImageIndex];

                        fistcell.Add(new Gtk.Image(img.Pixbuf) { Halign = Gtk.Align.Start, Valign = Gtk.Align.Fill, WidthRequest = 20 }) ;
                    }

                }

                fistcell.Add(new Gtk.Label(item.Text) { MaxWidthChars= fistcell.WidthRequest/12, Halign = Gtk.Align.Fill, Valign = Gtk.Align.Fill, Ellipsize = Pango.EllipsizeMode.End });

                Gtk.Layout layout = new Gtk.Layout(new Adjustment(IntPtr.Zero), new Adjustment(IntPtr.Zero));
                layout.Halign = Gtk.Align.Start;
                layout.Valign = Gtk.Align.Fill;
                layout.Expand= true;
                layout.WidthRequest = fistcell.WidthRequest;
                layout.Add(fistcell);
                hBox.Add(layout);

                int index = 0;
                foreach(ColumnHeader col in Columns)
                {
                    if (index > 0 && item.SubItems != null && item.SubItems.Count > index)
                    {
                        Gtk.Label sublabel = new Gtk.Label(item.SubItems[index].Text) { MarginStart=2, WidthRequest = col.Width, MaxWidthChars = col.Width / 12, Halign = Gtk.Align.Fill, Valign = Gtk.Align.Fill, Ellipsize = Pango.EllipsizeMode.End };
                        Gtk.Layout sublayout = new Gtk.Layout(new Adjustment(IntPtr.Zero), new Adjustment(IntPtr.Zero));
                        sublayout.Halign = Gtk.Align.Start;
                        sublayout.Valign = Gtk.Align.Fill;
                        sublayout.Expand = true;
                        sublayout.WidthRequest = col.Width - 2;
                       // sublayout.HeightRequest = 30;
                        sublayout.Add(sublabel);
                        hBox.Add(sublayout);
                    }
                    index++;
                }
            }
            else if (this.View == View.SmallIcon)
            {
                header.Visible = false;

                flowBox.Orientation = Gtk.Orientation.Horizontal;
                flowBox.MinChildrenPerLine = 1;
                flowBox.MaxChildrenPerLine = 999;
                if (this.SmallImageList != null)
                {
                    this.SmallImageList.ImageSize = new Size(16, 16);
                    if (!string.IsNullOrWhiteSpace(item.ImageKey))
                    {
                        Drawing.Image img = this.SmallImageList.Images[item.ImageKey];
                        hBox.Add(new Gtk.Image(img.Pixbuf));
                    }
                    else if (item.ImageIndex > -1)
                    {
                        Drawing.Image img = this.SmallImageList.Images[item.ImageIndex];
                        hBox.Add(new Gtk.Image(img.Pixbuf));
                    }

                }
                hBox.Add(new Gtk.Label(item.Text) { MaxWidthChars = Columns.Count > 0 ? Columns[0].Width/12 : 100/2, Halign = Gtk.Align.Start, Valign = Gtk.Align.Start, Ellipsize = Pango.EllipsizeMode.End });
            }
            else if (this.View == View.LargeIcon)
            {
                flowBox.MinChildrenPerLine = 1;
                flowBox.MaxChildrenPerLine = 999;
       
                Gtk.Box vBox = new Gtk.Box(Gtk.Orientation.Vertical, 0);
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
                vBox.Add(new Gtk.Label(item.Text) { MaxWidthChars = Columns.Count > 0 ? Columns[0].Width / 12 : 100 / 2, Halign = Gtk.Align.Center, Valign = Gtk.Align.Center, Ellipsize = Pango.EllipsizeMode.End }); ;
                hBox.Add(vBox);
            }
            else
            {
                header.Visible = false;
                Gtk.Label label = new Gtk.Label(item.Text) { Halign = Gtk.Align.Start, Valign = Gtk.Align.Fill }; ;

                hBox.Add(label);
            }
        }

        public void AddGroup(ListViewGroup group, int position)
        {
            if (group == null)
                return;
            Gtk.VBox hBox = new Gtk.VBox();
            hBox.Valign = Gtk.Align.Start;
            hBox.Halign = Gtk.Align.Fill;
            Gtk.Viewport groupbox = new Gtk.Viewport();
            groupbox.StyleContext.AddClass("listviewgroup");
            var title = new Gtk.Label(group.Header) { Xalign = 0, Halign = Gtk.Align.Start, Valign = Gtk.Align.Start, Ellipsize = Pango.EllipsizeMode.End };
            title.MarginStart = 3;
            title.StyleContext.AddClass("listviewtitle");
            groupbox.Add(title);
            hBox.Add(groupbox);
            if (!string.IsNullOrWhiteSpace(group.Subtitle))
            {
                var subtitle = new Gtk.Label(group.Subtitle) { Xalign = 0, Halign = Gtk.Align.Start, Valign = Gtk.Align.Start, Ellipsize = Pango.EllipsizeMode.End };
                subtitle.MarginStart = 3;
                subtitle.StyleContext.AddClass("listviewsubtitle");
                hBox.Add(subtitle);
            }

            Gtk.FlowBox _flow = group.FlowBox;
            _flow.StyleContext.AddClass("FlowBox");
            _flow.MaxChildrenPerLine = 100u;
            _flow.MinChildrenPerLine = 0u;
            _flow.ColumnSpacing = 12;
            if (this.Sorted)
            {
                _flow.SortFunc = new Gtk.FlowBoxSortFunc((fbc1, fbc2) =>
                {
                    if(this.Sorting==SortOrder.Descending)
                        return fbc2.TooltipText.CompareTo(fbc1.TooltipText);
                    else if(this.Sorting == SortOrder.Ascending)
                        return fbc1.TooltipText.CompareTo(fbc2.TooltipText);
                    else
                        return 0;
                });
            }
            _flow.Halign = Gtk.Align.Fill;
            _flow.Valign = Gtk.Align.Fill;
            _flow.Orientation = Gtk.Orientation.Horizontal;
            _flow.SelectionMode = Gtk.SelectionMode.Single;
            _flow.ChildActivated += _flow_ChildActivated;
            hBox.Add(_flow);
            
            if (position < 0)
            {
                flowBoxContainer.PackStart(hBox,true,true,0);
            }
            else
            {
                flowBoxContainer.PackStart(hBox, true, true,Convert.ToUInt32(position));
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
		public class ListViewItemCollection : List<ListViewItem>, IList
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
                item.Index = Count;
                base.Add(item);
                if (_owner.Control.IsRealized)
                {
                    _owner.AddItem(item, position);
                    _owner.Control.ShowAll();
                }
            }
            public void AddRange(ListViewItem[] items)
            {
                foreach (ListViewItem item in items)
                {
                    AddCore(item, -1);
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
                foreach (ListViewItem item in CheckedItems)
                {
                    selecteditems.Add(item.Index);
                }
                return selecteditems;
            }
		}

		public CheckedListViewItemCollection CheckedItems
		{
			get
			{
                CheckedListViewItemCollection selecteditems = new CheckedListViewItemCollection(this);

                foreach (Gtk.VBox vbox in flowBoxContainer.AllChildren)
                {
                    foreach (var flow in vbox.AllChildren)
                    {
                        if (flow is Gtk.FlowBox _flow)
                        {
                            foreach (Gtk.FlowBoxChild child in _flow.AllChildren)
                            {
                                ListViewItem item = this.Items.FindAll(m => m.Group.Name == child.Parent.Name)[child.Index];
                                if (item.Checked)
                                {
                                    selecteditems.Add(item);
                                }
                            }
                        }
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
                foreach(ListViewItem item in SelectedItems)
                {
                    selecteditems.Add(item.Index);
                }
               
                return selecteditems;
            }
		}

		public SelectedListViewItemCollection SelectedItems
		{
			get
			{
				SelectedListViewItemCollection selecteditems = new SelectedListViewItemCollection();
                foreach (Gtk.VBox vbox in flowBoxContainer.AllChildren)
                {
                    foreach (var flow in vbox.AllChildren)
                    {
                        if (flow is Gtk.FlowBox _flow)
                        {
                            foreach (Gtk.FlowBoxChild child in _flow.AllChildren)
                            {
                                ListViewItem item = this.Items.FindAll(m => m.Group.Name == child.Parent.Name)[child.Index];
                                item.Selected = child.IsSelected;
                                if (child.IsSelected)
                                {
                                    selecteditems.Add(item);
                                }
                            }
                        }
                    }
                }

				return selecteditems;
            }
		}

		public void Clear()
		{
            foreach (Gtk.VBox vbox in flowBoxContainer.AllChildren)
            {
                foreach (var flow in vbox.AllChildren)
                {
                    if (flow is Gtk.FlowBox _flow)
                    {
                        foreach (Gtk.FlowBoxChild child in _flow.Children)
                            _flow.Remove(child);
                    }
                }
                flowBoxContainer.Remove(vbox);
            }


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
