/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;


namespace System.Windows.Forms
{
	[DesignerCategory("Component")]
	[DefaultEvent("SelectedIndexChanged")]
	[DefaultProperty("Items")]
	[DefaultBindingProperty("SelectedValue")]
	public class ListBox : WidgetControl<Gtk.ListBox>
    {
        ControlBindingsCollection _collect;
        ObjectCollection _items;
        public ListBox():base()
		{
            _collect = new ControlBindingsCollection(this);
            _items = new ObjectCollection(this);
            Widget.StyleContext.AddClass("ListBox");
            base.Control.Realized += Control_Realized;
            base.Control.SortFunc = new ListBoxSortFunc((row1, row2) => {
                return !this.Sorted ? 0 : ((Gtk.Label)row1.Child).Text.CompareTo(((Gtk.Label)row2.Child).Text); 
            });
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
        [RefreshProperties(RefreshProperties.Repaint)]
        [AttributeProvider(typeof(IListSource))]
        public object? DataSource
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
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DefaultValue(null)]
        public IFormatProvider? FormatInfo
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

        public int SelectedIndex {
            get { return base.Control.SelectedRow == null ? -1 : base.Control.SelectedRow.Index; }
            set { base.Control.SelectRow(base.Control.GetRowAtIndex(value)); }
        }

        [DefaultValue(null)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Bindable(true)]
        public object? SelectedValue
        {
            get { return base.Control.SelectedRow.Children[0]; }
            set {
                int index = 0;
                foreach(object v in base.Control.AllChildren)
                {
                    if (((ListBoxItem)((ListBoxRow)v).Children[0]).ItemValue.Equals(value))
                    {
                        base.Control.SelectRow(base.Control.GetRowAtIndex(index));
                    }
                    index++;
                }
            }
        }

        public event EventHandler? DataSourceChanged;

        public event EventHandler? DisplayMemberChanged;

        public event ListControlConvertEventHandler? Format;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler? FormatInfoChanged;

        public event EventHandler? FormatStringChanged;

        public event EventHandler? FormattingEnabledChanged;

        public event EventHandler? ValueMemberChanged;

        public event EventHandler? SelectedValueChanged;

        public string? GetItemText(object? item)
        {
            if(item is ListBoxItem)
            {
                return ((ListBoxItem)item).DisplayText?.ToString();
            }
            return item?.ToString();
        }


        #endregion
        public override ControlBindingsCollection DataBindings { get => _collect; }


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
		[RefreshProperties(RefreshProperties.Repaint)]
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
		[RefreshProperties(RefreshProperties.Repaint)]
		public bool IntegralHeight
        {
            get; set;
        }

        [Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public virtual int ItemHeight
        {
            get; set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[MergableProperty(false)]
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
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

  //      [Browsable(false)]
		//[Bindable(true)]
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		//public int SelectedIndex
  //      {
  //          get; set;
  //      }

        [Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SelectedIndexCollection SelectedIndices
        {
            get; 
        }

        [Browsable(false)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object? SelectedItem
        {
            get; set;
        }

        [Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SelectedObjectCollection SelectedItems
        {
            get; 
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
                    Control.SelectionMode = Gtk.SelectionMode.None;
                }
                else if (value == SelectionMode.One)
                {
                    Control.SelectionMode = Gtk.SelectionMode.Single;
                }
                else if (value == SelectionMode.MultiSimple) { 
                    Control.SelectionMode = Gtk.SelectionMode.Multiple;
                }
                else if (value == SelectionMode.MultiExtended)
                {
                    Control.SelectionMode = Gtk.SelectionMode.Multiple;
                }
            }
        }

        [DefaultValue(false)]
		public bool Sorted
        {
            get; set;
        }

        [Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Bindable(false)]
		public override string Text
        {
            get; set;
        }

        [Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int TopIndex
        {
            get; set;
        }

        [DefaultValue(true)]
		public bool UseTabStops
        {
            get; set;
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Browsable(false)]
		public IntegerCollection CustomTabOffsets
        {
            get; 
        }

        [Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding
		{
			get;set;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler? BackgroundImageChanged;
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler? BackgroundImageLayoutChanged;
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event EventHandler? TextChanged;
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler? Click;
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event MouseEventHandler? MouseClick;
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler? PaddingChanged;
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler? Paint;

		//public event DrawItemEventHandler? DrawItem;

		public event MeasureItemEventHandler? MeasureItem;

		public event EventHandler? SelectedIndexChanged;

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

            object? IList.this[int index]
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

            bool IList.Contains(object? item)
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

            int IList.IndexOf(object? item)
            {
                throw null;
            }

            public int Add(int item)
            {
                throw null;
            }

            int IList.Add(object? item)
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

            void IList.Insert(int index, object? value)
            {
                throw null;
            }

            void IList.Remove(object? value)
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
        public class ObjectCollection : IList, ICollection, IEnumerable
        {
            private ListBox _owner;
            private ArrayList _array = new ArrayList();
            public ObjectCollection(ListBox owner)
            {
                _owner = owner;
            }

            public ObjectCollection(ListBox owner, ObjectCollection value)
            {
                _owner = owner;
                foreach(object item in value)
                    Add(item);
            }

            public ObjectCollection(ListBox owner, object[] value)
            {
                _owner = owner;
                foreach (object item in value)
                    Add(item);
            }

            public int Count
            {
                get
                {
                    return _array.Count;
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

            public bool IsReadOnly
            {
                get
                {
                   return true;
                }
            }

            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public virtual object this[int index]
            {
                get
                {
                   return _array[index];
                }
                set
                {
                    Add(value);
                }
            }

            object? IList.this[int index]
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


            public int Add(object item)
            {
                Gtk.ListBoxRow row = new Gtk.ListBoxRow();
                row.Add((ListBoxItem)item);
                _owner.Control.Add(row);
                row.Valign = Align.Start;

                if (_owner.BorderStyle == BorderStyle.FixedSingle)
                    row.BorderWidth = 1;
                else if (_owner.BorderStyle == BorderStyle.Fixed3D)
                    row.BorderWidth = 2;
                else
                    row.BorderWidth = 1;
                
                if (_owner.Control.IsRealized)
                {
                    _owner.Control.ShowAll();
                }

                return _array.Add(item);
            }

            int IList.Add(object? item)
            {
                throw null;
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

            internal void AddRangeInternal(ICollection items)
            {
                throw null;
            }

            public virtual void Clear()
            {
                foreach (var wd in _owner.Control.Children)
                    _owner.Control.Remove(wd);
                _array.Clear();
            }

            internal void ClearInternal()
            {
                throw null;
            }

            public bool Contains(object value)
            {
                return _array.Contains(value);
            }

            bool IList.Contains(object? value)
            {
                throw null;
            }

            public void CopyTo(object[] destination, int arrayIndex)
            {
                _array.CopyTo(destination, arrayIndex);
            }

            void ICollection.CopyTo(Array destination, int index)
            {
                throw null;
            }

            public IEnumerator GetEnumerator()
            {
                return _array.GetEnumerator();
            }

            public int IndexOf(object value)
            {
                return _array.IndexOf(value);
            }

            int IList.IndexOf(object? value)
            {
                throw null;
            }

            internal int IndexOfIdentifier(object value)
            {
                throw null;
            }

            public void Insert(int index, object item)
            {
                _owner.Control.Insert((ListBoxItem)item, index);
                _array.Insert(index, item);
            }

            void IList.Insert(int index, object? item)
            {
                throw null;
            }

            public void Remove(object value)
            {
                _owner.Control.Remove((ListBoxItem)value);
                _array.Remove(value);
            }

            void IList.Remove(object? value)
            {
                throw null;
            }

            public void RemoveAt(int index)
            {
                _owner.Control.Remove(_owner.Control.GetRowAtIndex(index));
                _array.RemoveAt(index);
            }

            internal void SetItemInternal(int index, object value)
            {
                throw null;
            }
        }

        public class SelectedIndexCollection : IList, ICollection, IEnumerable
        {
            private class SelectedIndexEnumerator : IEnumerator
            {
                object IEnumerator.Current
                {
                    get
                    {
                        throw null;
                    }
                }

                public SelectedIndexEnumerator(SelectedIndexCollection items)
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

            public bool IsReadOnly
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
            }

            object? IList.this[int index]
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

            public SelectedIndexCollection(ListBox owner)
            {
                throw null;
            }

            public bool Contains(int selectedIndex)
            {
                throw null;
            }

            bool IList.Contains(object? selectedIndex)
            {
                throw null;
            }

            public int IndexOf(int selectedIndex)
            {
                throw null;
            }

            int IList.IndexOf(object? selectedIndex)
            {
                throw null;
            }

            int IList.Add(object? value)
            {
                throw null;
            }

            void IList.Clear()
            {
                throw null;
            }

            void IList.Insert(int index, object? value)
            {
                throw null;
            }

            void IList.Remove(object? value)
            {
                throw null;
            }

            void IList.RemoveAt(int index)
            {
                throw null;
            }

            public void CopyTo(Array destination, int index)
            {
                throw null;
            }

            public void Clear()
            {
                throw null;
            }

            public void Add(int index)
            {
                throw null;
            }

            public void Remove(int index)
            {
                throw null;
            }

            public IEnumerator GetEnumerator()
            {
                throw null;
            }
        }

        public class SelectedObjectCollection : IList, ICollection, IEnumerable
        {
            internal static int SelectedObjectMask;

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

            public bool IsReadOnly
            {
                get
                {
                    throw null;
                }
            }

            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public object? this[int index]
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

            public SelectedObjectCollection(ListBox owner)
            {
                throw null;
            }

            internal void Dirty()
            {
                throw null;
            }

            internal void EnsureUpToDate()
            {
                throw null;
            }

            public bool Contains(object? selectedObject)
            {
                throw null;
            }

            public int IndexOf(object? selectedObject)
            {
                throw null;
            }

            int IList.Add(object? value)
            {
                throw null;
            }

            void IList.Clear()
            {
                throw null;
            }

            void IList.Insert(int index, object? value)
            {
                throw null;
            }

            void IList.Remove(object? value)
            {
                throw null;
            }

            void IList.RemoveAt(int index)
            {
                throw null;
            }

            internal object GetObjectAt(int index)
            {
                throw null;
            }

            public void CopyTo(Array destination, int index)
            {
                throw null;
            }

            public IEnumerator GetEnumerator()
            {
                throw null;
            }

            internal bool GetSelected(int index)
            {
                throw null;
            }

            internal void PushSelectionIntoNativeListBox(int index)
            {
                throw null;
            }

            internal void SetSelected(int index, bool value)
            {
                throw null;
            }

            public void Clear()
            {
                throw null;
            }

            public void Add(object value)
            {
                throw null;
            }

            public void Remove(object value)
            {
                throw null;
            }
        }
    }
}
