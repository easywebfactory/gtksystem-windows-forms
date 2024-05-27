/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using GLib;
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using Pango;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
	[DefaultEvent("SelectedIndexChanged")]
	[DefaultProperty("Items")]
	[DefaultBindingProperty("SelectedValue")]
	public partial class ListBox : ListControl
    {
        public readonly ListBoxBase self = new ListBoxBase();
        public override object GtkControl => self;
        private ControlBindingsCollection _collect;
        private ObjectCollection _items;

        internal Gtk.ListBox ListBoxView { get { return self.ListBox; } }

        public ListBox():base()
		{
            _collect = new ControlBindingsCollection(this);
            _items = new ObjectCollection(this);
            self.Realized += Self_Realized;
            ListBoxView.SelectedRowsChanged += ListBox_SelectedRowsChanged;
        }
        private void ListBox_SelectedRowsChanged(object sender, EventArgs e)
        {
            if (self.IsVisible)
            {
                ((EventHandler)Events["SelectedIndexChanged"])?.Invoke(this, e);
                ((EventHandler)Events["SelectedValueChanged"])?.Invoke(this, e);
                ((EventHandler)Events["SelectedItemChanged"])?.Invoke(this, e);
            }
        }

        private void Self_Realized(object sender, EventArgs e)
        {
            OnSetDataSource();
            foreach (Binding binding in DataBindings)
                ListBoxView.AddNotification(binding.PropertyName, propertyNotity);
        }
        private void propertyNotity(object o, NotifyArgs args)
        {
            Binding binding = DataBindings[args.Property];
            binding.WriteValue();
        }
        #region listcontrol
        private object _DataSource;
        public override object DataSource
        {
            get => _DataSource;
            set {
                _DataSource = value;
                if (self.IsVisible)
                {
                    OnSetDataSource();
                }
            }
        }
        private void OnSetDataSource()
        {
            if (_DataSource != null)
            {
                if (_DataSource is IListSource listSource)
                {
                    IEnumerator list = listSource.GetList().GetEnumerator();
                    SetDataSource(list);
                }
                else if (_DataSource is IEnumerable list1)
                {
                    SetDataSource(list1.GetEnumerator());
                }
            }
        }
        private void SetDataSource(IEnumerator enumerator)
        {
            _items.Clear();
            if (enumerator != null)
            {
                if (string.IsNullOrWhiteSpace(DisplayMember))
                {
                    while (enumerator.MoveNext())
                    {
                        var o = enumerator.Current;
                        if (o is DataRowView row)
                            _items.Add(row[0]);
                        else
                            _items.Add(enumerator.Current);
                    }
                }
                else
                {
                    while (enumerator.MoveNext())
                    {
                        var o = enumerator.Current;
                        if (o is DataRowView row)
                            _items.Add(row[DisplayMember]);
                        else
                            _items.Add(o.GetType().GetProperty(DisplayMember)?.GetValue(o));
                    }
                }
            }
        }

        public override int SelectedIndex
        {
            get => ListBoxView.SelectedRow == null ? -1 : ListBoxView.SelectedRow.Index; set => SelectedItems.SetSelected(value, true);
        }

        [DefaultValue(null)]
        [Browsable(false)]
        public override object SelectedValue
        {
            get
            {
                int index = SelectedIndex;
                return index == -1 ? null : _items[SelectedIndex];
            }
            set
            {
                ClearSelected();
                int index = _items.IndexOf(value);
                SelectedItems.SetSelected(index, true);
            }
        }

        [Browsable(false)]
        [Bindable(true)]
        public object SelectedItem
        {
            get
            {
                int index = SelectedIndex;
                return index == -1 ? null : _items[SelectedIndex];
            }
            set
            {
                ClearSelected();
                int index = _items.IndexOf(value);
                SelectedItems.SetSelected(index, true);
            }
        }
        public override string GetItemText(object item)
        {
            if(item is ItemArray.Entry entry)
            {
                return entry.Item?.ToString();
            }
            return item?.ToString();
        }
        protected void NativeInsert(int index, object item)
        {
            Gtk.ListBoxRow row = new Gtk.ListBoxRow();
            row.HeightRequest = ItemHeight > 0 ? ItemHeight : DefaultItemHeight;
            row.Add(new Gtk.Label(item.ToString()) { Valign = Align.Center, Halign = Align.Start, Expand = true });
            //row.Add(new LitorLabel() { Label = item.ToString(), Valign = Align.Center, Halign = Align.Start, Expand = true }); ;
            ListBoxView.Insert(row, index);
            if (self.IsVisible && !IsUpdateing)
            {
                ListBoxView.ShowAll();
            }
        }
        protected void NativeAdd(object item)
        {
            Gtk.ListBoxRow row = new Gtk.ListBoxRow();
            row.HeightRequest = ItemHeight > 0 ? ItemHeight : DefaultItemHeight;
            row.Add(new Gtk.Label(item.ToString()) { Valign = Align.Center, Halign = Align.Start, Expand = true });
            //row.Add(new LitorLabel() { Label = item.ToString(), Valign = Align.Center, Halign = Align.Start, Expand = true }); ;
            ListBoxView.Add(row);
            if (self.IsVisible && !IsUpdateing)
            {
                ListBoxView.ShowAll();
            }
        }
        protected void NativeClear()
        {
            int count = ListBoxView.Children.Length;
            while (count > 0)
            {
                ListBoxView.Remove(ListBoxView.GetRowAtIndex(count - 1));
                count--;
                //System.Threading.Thread.Sleep(3);
            }
        }
        protected void NativeRemoveAt(int index)
        {
            ListBoxView.Remove(ListBoxView.GetRowAtIndex(index));
        }
        protected string NativeGetItemText(int index)
        {
            Gtk.Label row = ListBoxView.GetRowAtIndex(index).Child as Gtk.Label;
            return row?.Text;
        }
        protected void OnSelectedIndexChanged(EventArgs e) {
            if (ListBoxView.SelectedRow != null)
                ListBoxView.SelectRow(ListBoxView.SelectedRow);
        }
        #endregion
        public override ControlBindingsCollection DataBindings { get => _collect; }
        internal bool ShowCheckBox { get; set; }
        internal bool ShowImage { get; set; }

        public const int NoMatches = -1;

		public const int DefaultItemHeight = 13;

        [DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		public override BorderStyle BorderStyle
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
                return indexs;
            }
        }

        [Browsable(false)]
        public SelectedObjectCollection SelectedItems
        {
            get
            {
                SelectedObjectCollection indexs = new SelectedObjectCollection(this);
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
                    ListBoxView.SelectionMode = Gtk.SelectionMode.None;
                }
                else if (value == SelectionMode.One)
                {
                    ListBoxView.SelectionMode = Gtk.SelectionMode.Single;
                }
                else if (value == SelectionMode.MultiSimple)
                {
                    ListBoxView.SelectionMode = Gtk.SelectionMode.Multiple;
                }
                else if (value == SelectionMode.MultiExtended)
                {
                    ListBoxView.SelectionMode = Gtk.SelectionMode.Multiple;
                }
            }
        }
        private void CheckNoDataSource()
        {
            //if (DataSource != null)
            //{
            //    throw new ArgumentException("SR.DataSourceLocksItems");
            //}
        }
        private bool _sorted;
        [DefaultValue(false)]
		public bool Sorted
        {
            get => _sorted; set => _sorted = value;
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
        public void ClearSelected()
        {
            ListBoxView.UnselectAll();
        }
        internal bool IsUpdateing = false;
        public void BeginUpdate()
		{
            IsUpdateing = true;
        }

		public void EndUpdate()
		{
            IsUpdateing = false;
            ListBoxView.ShowAll();
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
            return ListBoxView.GetRowAtIndex(index).IsSelected;
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
			self.ShowAll();
		}

		public override void ResetBackColor()
		{
			
		}

		public override void ResetForeColor()
		{
			
		}

		public void SetSelected(int index, bool value)
		{
            if (value == true)
                ListBoxView.SelectRow(ListBoxView.GetRowAtIndex(index));
            else
                ListBoxView.UnselectRow(ListBoxView.GetRowAtIndex(index));
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
    }
}
