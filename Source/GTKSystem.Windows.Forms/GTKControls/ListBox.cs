/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using GLib;
using Gtk;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
 
namespace System.Windows.Forms
{
	[DesignerCategory("Component")]
	[DefaultEvent("SelectedIndexChanged")]
	[DefaultProperty("Items")]
	[DefaultBindingProperty("SelectedValue")]
	public partial class ListBox : WidgetControl<Gtk.HBox>
    {
        ControlBindingsCollection _collect;
        ObjectCollection _items;
        internal Gtk.FlowBox _flow;
        public ListBox():base()
		{
            _collect = new ControlBindingsCollection(this);
            _items = new ObjectCollection(this);
            Widget.StyleContext.AddClass("ListBox");
            base.Control.Realized += Control_Realized;
            _flow = new Gtk.FlowBox();
           // _flow.MaxChildrenPerLine = 9u;
            _flow.SortFunc = new FlowBoxSortFunc((fbc1, fbc2) => !this.Sorted ? 0 : fbc1.TooltipText.CompareTo(fbc2.TooltipText));

            _items = new ObjectCollection(this);
            _flow.ChildActivated += Control_ChildActivated;
            _flow.Halign = Gtk.Align.Start;
            Gtk.ScrolledWindow scrolledWindow = new Gtk.ScrolledWindow();
            Gtk.Viewport viewport = new Gtk.Viewport();
            viewport.Add(_flow);
            scrolledWindow.Add(viewport);
            this.Control.Add(scrolledWindow);
        }

        private HashSet<int> selectedIndexes = new HashSet<int>();
        private void Control_ChildActivated(object o, Gtk.ChildActivatedArgs args)
        {
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
                object sender = this.Items[rowIndex];
                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(sender, args);
                if (SelectedValueChanged != null)
                    SelectedValueChanged(sender, args);
            }
        }
        private void Control_Realized(object sender, EventArgs e)
        {
            foreach (Binding binding in _collect)
            {
                BindDataSource(binding.DataSource, binding.DataMember, binding.DataMember, SelectedIndex, binding.FormattingEnabled, binding.DataSourceUpdateMode, binding.NullValue, binding.FormatString);
            }
            if (DataSource != null)
            {
                BindDataSource(DataSource, DisplayMember, ValueMember, SelectedIndex, FormattingEnabled, DataSourceUpdateMode.OnPropertyChanged, string.Empty, FormatString);
            }
            foreach (object item in _items)
            {
                AddItem(item, -1);
            }

            this.Control.ShowAll();
        }
        internal void BindDataSource(object datasource,string displaymember,string valuemember,int selectindex, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue, string formatString)
        {
            IListSource source = datasource as IListSource;
            if (string.IsNullOrWhiteSpace(valuemember))
                valuemember = displaymember;

            if (source == null)
            {
                IEnumerable iesource = datasource as IEnumerable;

                foreach (var row in iesource)
                {
                    string display = row.GetType().GetProperty(displaymember).GetValue(row).ToString();
                    string value = row.GetType().GetProperty(valuemember).GetValue(row).ToString();
                    ListBoxItem item = new ListBoxItem();
                    if (formattingEnabled && string.IsNullOrWhiteSpace(formatString) == false)
                        item.DisplayText = string.Format(formatString,display);
                    else
                        item.DisplayText = display;

                    item.ItemValue = value;
                    item.CheckValue = value;
                    _items.Add(item);
                }
            }
            else
            {
                if (source.ContainsListCollection)
                {
                    DataSet ds = datasource as DataSet;
                    foreach (DataTable dtb in ds.Tables)
                    {
                        foreach (DataRow row in dtb.Rows)
                        {
                            ListBoxItem item = new ListBoxItem();
                            if (formattingEnabled && string.IsNullOrWhiteSpace(formatString) == false)
                                item.DisplayText = string.Format(formatString, row[displaymember]);
                            else
                                item.DisplayText = row[displaymember];
                            item.ItemValue = row[valuemember];
                            item.CheckValue = row[valuemember];
                            _items.Add(item);
                        }
                    }
                }
                else
                {
                    IList list = source.GetList();
                    foreach (object row in list)
                    {
                        string display = row.GetType().GetProperty(displaymember).GetValue(row).ToString();
                        string value = row.GetType().GetProperty(valuemember).GetValue(row).ToString();
                        ListBoxItem item = new ListBoxItem();
                        if (formattingEnabled && string.IsNullOrWhiteSpace(formatString) == false)
                            item.DisplayText = string.Format(formatString, display);
                        else
                            item.DisplayText = display;
                        item.ItemValue = value;
                        item.CheckValue = value;
                        _items.Add(item);
                    }
                }
            }
        }
        #region listcontrol

        [DefaultValue(null)]
        [AttributeProvider(typeof(IListSource))]
        public object DataSource
        {
            get;
            set;
        }

        [DefaultValue("")]
        public string DisplayMember
        {
            get; set;
        }

        [Browsable(false)]
        [DefaultValue(null)]
        public IFormatProvider FormatInfo
        {
            get; set;
        }

        [DefaultValue("")]
        [MergableProperty(false)]
        public string FormatString
        {
            get; set;
        }

        [DefaultValue(false)]
        public bool FormattingEnabled
        {
            get; set;
        }

        [DefaultValue("")]
        public string ValueMember
        {
            get; set;
        }
        public int SelectedIndex
        {
            get {
                int index = -1;
                _flow.SelectedForeach(new FlowBoxForeachFunc((fb, fbc) => { index = fbc.Index; }));
                return index; 
            }
            set { _flow.SelectChild(_flow.GetChildAtIndex(value)); }
        }
 
        [DefaultValue(null)]
        [Browsable(false)]
        public object SelectedValue
        {
            get
            {
                object text = null;
                _flow.SelectedForeach(new FlowBoxForeachFunc((fb, fbc) => {
                    if (Items[fbc.Index] is ListBoxItem item)
                    {
                        text = item.ItemValue;
                    }
                    else
                    {
                        text = Items[fbc.Index];
                    }
                }));
                return text;
            }
            set {
                int idx = 0;
                foreach(var item in Items)
                {
                    if(item is ListBoxItem item2)
                        if(item2.ItemValue==value)
                            _flow.SelectChild(_flow.GetChildAtIndex(idx));
                    else if(item.Equals(value))
                            _flow.SelectChild(_flow.GetChildAtIndex(idx));

                    idx++;
                }
            }
        }

        public event EventHandler DataSourceChanged;

        public event EventHandler DisplayMemberChanged;

        public event ListControlConvertEventHandler Format;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler FormatInfoChanged;

        public event EventHandler FormatStringChanged;

        public event EventHandler FormattingEnabledChanged;

        public event EventHandler ValueMemberChanged;

        public event EventHandler SelectedValueChanged;

        public string GetItemText(object item)
        {
            if(item is ListBoxItem)
            {
                return ((ListBoxItem)item).DisplayText?.ToString();
            }
            return item?.ToString();
        }


        #endregion
        public override ControlBindingsCollection DataBindings { get => _collect; }

        internal bool ShowCheckBox { get; set; }
        internal bool ShowImage { get; set; }

        public const int NoMatches = -1;

		public const int DefaultItemHeight = 13;

        [DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		public BorderStyle BorderStyle
        {
            get; set;
        }

        [Localizable(true)]
		[DefaultValue(0)]
		public int ColumnWidth
        {
            get; set;
        }

        [DefaultValue(false)]
		[Browsable(false)]
		public bool UseCustomTabOffsets
        {
            get; set;
        }

        [DefaultValue(DrawMode.Normal)]
		public virtual DrawMode DrawMode
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

        [DefaultValue(0)]
		[Localizable(true)]
		public int HorizontalExtent
        {
            get; set;
        }

        [DefaultValue(false)]
		[Localizable(true)]
		public bool HorizontalScrollbar
        {
            get; set;
        }

        [DefaultValue(true)]
		[Localizable(true)]
		public bool IntegralHeight
        {
            get; set;
        }

        [Localizable(true)]
		public virtual int ItemHeight
        {
            get; set;
        }

		[Localizable(true)]
		public ObjectCollection Items
        {
            get => _items;
        }

        [DefaultValue(false)]
		public bool MultiColumn
        {
            get; set;
        }

        [Browsable(false)]
		public int PreferredHeight
        {
            get; 
        }

        [DefaultValue(false)]
		[Localizable(true)]
		public bool ScrollAlwaysVisible
        {
            get; set;
        }

        [Browsable(false)]
		public SelectedIndexCollection SelectedIndices
        {
            get {
                SelectedIndexCollection indexs = new SelectedIndexCollection(this);
                _flow.SelectedForeach(new FlowBoxForeachFunc((fb, fbc) => {
                    indexs.Add(fbc.Index);
                }));
                return indexs;
            }
        }

        [Browsable(false)]
		[Bindable(true)]
		public object SelectedItem
        {
            get
            {
                object item = null;
                _flow.SelectedForeach(new FlowBoxForeachFunc((fb, fbc) => {
                    item = Items[fbc.Index];
                }));
                return item;
            }
            set {
                _flow.SelectChild(_flow.GetChildAtIndex(Items.IndexOf(value)));
            }
        }

        [Browsable(false)]
		public SelectedObjectCollection SelectedItems
        {
            get
            {
                SelectedObjectCollection indexs = new SelectedObjectCollection(this);
                _flow.SelectedForeach(new FlowBoxForeachFunc((fb, fbc) => {
                    indexs.Add(Items[fbc.Index]);
                }));
                return indexs;
            }
        }
        public SelectionMode _SelectionMode;
        [DefaultValue(SelectionMode.One)]
		public virtual SelectionMode SelectionMode
        {
            get {
                return _SelectionMode;
            }
            set {
                if (value == SelectionMode.None)
                {
                    _flow.SelectionMode = Gtk.SelectionMode.None;
                }
                else if (value == SelectionMode.One)
                {
                    _flow.SelectionMode = Gtk.SelectionMode.Single;
                }
                else if (value == SelectionMode.MultiSimple) {
                    _flow.SelectionMode = Gtk.SelectionMode.Multiple;
                }
                else if (value == SelectionMode.MultiExtended)
                {
                    _flow.SelectionMode = Gtk.SelectionMode.Multiple;
                }
            }
        }

        [DefaultValue(false)]
		public bool Sorted
        {
            get; set;
        }

        [Browsable(false)]
		public override string Text
        {
            get; set;
        }

		public int TopIndex
        {
            get; set;
        }

        [DefaultValue(true)]
		public bool UseTabStops
        {
            get; set;
        }

		public IntegerCollection CustomTabOffsets
        {
            get; 
        }

		public new Padding Padding
		{
			get;set;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged;
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged;
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event EventHandler TextChanged;
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler Click;
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event MouseEventHandler MouseClick;
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler PaddingChanged;
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint;

		//public event DrawItemEventHandler DrawItem;

		public event MeasureItemEventHandler MeasureItem;

		public event EventHandler SelectedIndexChanged;

		public void BeginUpdate()
		{
			throw null;
		}
		public void ClearSelected()
		{
			throw null;
		}

		public void EndUpdate()
		{
			throw null;
		}

		public int FindString(string s)
		{
			throw null;
		}

		public int FindString(string s, int startIndex)
		{
			throw null;
		}

		public int FindStringExact(string s)
		{
			throw null;
		}

		public int FindStringExact(string s, int startIndex)
		{
			throw null;
		}

		public int GetItemHeight(int index)
		{
			throw null;
		}

		public Drawing.Rectangle GetItemRectangle(int index)
		{
			throw null;
		}
		public bool GetSelected(int index)
		{
			throw null;
		}

		public int IndexFromPoint(Drawing.Point p)
		{
			throw null;
		}

		public int IndexFromPoint(int x, int y)
		{
			throw null;
		}

		public override void Refresh()
		{
			this.Control.ShowAll();
		}

		public override void ResetBackColor()
		{
			throw null;
		}

		public override void ResetForeColor()
		{
			throw null;
		}

		public void SetSelected(int index, bool value)
		{
			throw null;
		}
        public void AddItem(object item, int position)
        {
            Gtk.HBox hBox = new Gtk.HBox();
            hBox.Valign = Gtk.Align.Fill;
            hBox.Halign = Gtk.Align.Fill;
            hBox.Add(new Gtk.Label(item.ToString()) { Xalign = 0, Halign = Gtk.Align.Start, Valign = Gtk.Align.Start }); ;

            Gtk.FlowBoxChild boxitem = new Gtk.FlowBoxChild();
            boxitem.HeightRequest = this.ItemHeight;
            boxitem.WidthRequest = this.ColumnWidth;
            boxitem.WidthRequest = Width;
            boxitem.Valign = Gtk.Align.Fill;
            boxitem.Halign = Gtk.Align.Fill;
            boxitem.TooltipText = item.ToString();
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
        public class ListBoxItem: Gtk.Label
        {
            public ListBoxItem() { 
               base.Xalign = 0;
            }
            public object DisplayText { get { return base.Text; } set { base.Text = value?.ToString(); } }
            public object ItemValue { get; set; }
            public object CheckValue { get; set; }

            public override string ToString()
            {
                return DisplayText?.ToString();
            }
        }

        public class IntegerCollection : IList, ICollection, IEnumerable
        {
            private class CustomTabOffsetsEnumerator : IEnumerator
            {
                object IEnumerator.Current
                {
                    get
                    {
                        throw null;
                    }
                }

                public CustomTabOffsetsEnumerator(IntegerCollection items)
                {
                    throw null;
                }

                bool IEnumerator.MoveNext()
                {
                    throw null;
                }

                void IEnumerator.Reset()
                {
                    throw null;
                }
            }

            [Browsable(false)]
            public int Count
            {
                get
                {
                    throw null;
                }
            }

            object ICollection.SyncRoot
            {
                get
                {
                    throw null;
                }
            }

            bool ICollection.IsSynchronized
            {
                get
                {
                    throw null;
                }
            }

            bool IList.IsFixedSize
            {
                get
                {
                    throw null;
                }
            }

            bool IList.IsReadOnly
            {
                get
                {
                    throw null;
                }
            }

            public int this[int index]
            {
                get
                {
                    throw null;
                }
                set
                {
                    throw null;
                }
            }

            object IList.this[int index]
            {
                get
                {
                    throw null;
                }
                set
                {
                    throw null;
                }
            }

            public IntegerCollection(ListBox owner)
            {
                throw null;
            }

            public bool Contains(int item)
            {
                throw null;
            }

            bool IList.Contains(object item)
            {
                throw null;
            }

            public void Clear()
            {
                throw null;
            }

            public int IndexOf(int item)
            {
                throw null;
            }

            int IList.IndexOf(object item)
            {
                throw null;
            }

            public int Add(int item)
            {
                throw null;
            }

            int IList.Add(object item)
            {
                throw null;
            }

            public void AddRange(int[] items)
            {
                throw null;
            }

            public void AddRange(IntegerCollection value)
            {
                throw null;
            }

            void IList.Clear()
            {
                throw null;
            }

            void IList.Insert(int index, object value)
            {
                throw null;
            }

            void IList.Remove(object value)
            {
                throw null;
            }

            void IList.RemoveAt(int index)
            {
                throw null;
            }

            public void Remove(int item)
            {
                throw null;
            }

            public void RemoveAt(int index)
            {
                throw null;
            }

            public void CopyTo(Array destination, int index)
            {
                throw null;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw null;
            }
        }

        [ListBindable(false)]
        public class ObjectCollection : ArrayList
        {
            private ListBox _owner;
            public ObjectCollection(ListBox owner)
            {
                _owner = owner;
            }
            public ObjectCollection(ListBox owner, ObjectCollection value)
            {
                _owner = owner;
                foreach (object item in value)
                    Add(item);
            }

            public ObjectCollection(ListBox owner, object[] value)
            {
                _owner = owner;
                foreach (object item in value)
                    Add(item);
            }
            public int AddCore(object item, int position)
            {
                if (position < 0)
                {
                    int idx = base.Add(item);
                    if (_owner.Control.IsRealized)
                    {
                        _owner.AddItem(item, position);
                        _owner.Control.ShowAll();
                    }
                    return idx;
                }
                else
                {
                    base.Insert(position, item);
                    if (_owner.Control.IsRealized)
                    {
                        _owner.AddItem(item, position);
                        _owner.Control.ShowAll();
                    }
                    return position;
                }

            }
            public override int Add(object item)
            {
                return AddCore(item, -1);
            }

            public void AddRange(ObjectCollection value)
            {
                foreach (object item in value)
                    Add(item);
            }

            public void AddRange(object[] items)
            {
                foreach (object item in items)
                    Add(item);
            }
             
            public override void Clear()
            {
                foreach (var wd in _owner._flow.Children)
                    _owner._flow.Remove(wd);
                base.Clear();
            }
 
            public override void Insert(int index, object item)
            {
                AddCore(item, -1);
            }

            public override void Remove(object value)
            {
                int idx = base.IndexOf(value);
                if (idx > -1)
                    RemoveAt(idx);
            }

            public override void RemoveAt(int index)
            {
                _owner._flow.Remove(_owner._flow.GetChildAtIndex(index));
                base.RemoveAt(index);
            }

        }
        

        public class SelectedIndexCollection : List<int>
        {
            public SelectedIndexCollection(ListBox owner)
            {
                
            }
        }

        public class SelectedObjectCollection : List<object>
        {
            public SelectedObjectCollection(ListBox owner)
            {
                
            }

        }
    }
}
