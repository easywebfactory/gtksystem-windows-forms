/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.Collections;
using System.ComponentModel;
using Gtk;

namespace System.Windows.Forms;

    [DesignerCategory("Component")]
    public partial class CheckedListBox : ContainerControl
    {
        public readonly CheckedListBoxBase self = new();
        public override object GtkControl => self;
        internal FlowBox _flow = new();
        private readonly ObjectCollection _items;
        public CheckedListBox() : base()
        {
            _items = new ObjectCollection(this);
            _flow.Orientation = Gtk.Orientation.Horizontal;
            _flow.MinChildrenPerLine = 1;
            _flow.MaxChildrenPerLine = 99999;
            _flow.Halign = Align.Fill;
            _flow.Valign = Align.Fill;
            _flow.ChildActivated += Control_ChildActivated;
            self.AutoScroll = true;
            self.Add(_flow);
            
            BorderStyle = BorderStyle.Fixed3D;
        }
        private void Control_ChildActivated(object o, ChildActivatedArgs args)
        {
            if (CheckOnClick)
                args.Child.Child.SetProperty("active", new GLib.Value(true));

            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, args);
            if (SelectedValueChanged != null)
                SelectedValueChanged(this, args);
            if (SelectedItemChanged != null)
                SelectedItemChanged(this, args);
        }

        public int ColumnWidth { get; set; }
        public bool MultiColumn { get => _flow.Orientation == Gtk.Orientation.Vertical;
            set { if (value == false) { _flow.Orientation = Gtk.Orientation.Horizontal; } else { _flow.Orientation = Gtk.Orientation.Vertical; } } }
        public bool HorizontalScrollbar { get; set; }
        public bool FormattingEnabled { get; set; }
        public int ItemHeight { get; set; }
        public SelectionMode SelectionMode { get; set; }
        public void ClearSelected() {
            foreach(FlowBoxChild wi in _flow.Children) { 
                var box = ((Box)wi.Child).Children[0];
                if (box is CheckButton check)
                {
                    check.Active = false;
                }
            };
            _flow.UnselectAll();
        }
        public bool CheckOnClick { get; set; }
        public ObjectCollection Items => _items;

        public SelectedItemCollection SelectedItems
        {
            get
            {
                var items = new SelectedItemCollection();
                foreach (FlowBoxChild child in _flow.Children)
                {
                    if (child.IsSelected)
                        items.Add(child.Child.GetProperty("label").Val);
                }
                return items;
            }
        }
        public CheckedItemCollection CheckedItems { 
            get {
                var items = new CheckedItemCollection();
                foreach (FlowBoxChild child in _flow.Children)
                {
                    if ((bool)child.Child.GetProperty("active"))
                        items.Add(child.Child.GetProperty("label").Val);
                }
                return items; } 
        }

        public CheckedIndexCollection CheckedIndices
        {
            get
            {
                var items = new CheckedIndexCollection();
                foreach (FlowBoxChild child in _flow.Children)
                {
                    if ((bool)child.Child.GetProperty("active"))
                        items.Add(child.Index);
                }
                return items;
            }
        }

        public event ItemCheckEventHandler? ItemCheck;
        public event EventHandler? SelectedIndexChanged;
        public event EventHandler? SelectedValueChanged;
        public event EventHandler? SelectedItemChanged;
        internal void NativeAdd(CheckBox checkbox, bool isChecked, int position)
        {
            checkbox.self.Toggled += (sender, e) =>
            {
                var box = (CheckButton)sender;
                var item = box.Parent as FlowBoxChild;
                if (ItemCheck != null)
                {
                    var checkBox = _items.GetCheckedListBoxItem(item.Index);
                    ItemCheck(checkBox, new ItemCheckEventArgs(item.Index, box.Active ? CheckState.Checked : CheckState.Unchecked, box.Active == false ? CheckState.Checked : CheckState.Unchecked));
                }
            };
            var boxitem = new FlowBoxChild();
            boxitem.HeightRequest = ItemHeight;
            if (MultiColumn && ColumnWidth > 0)
            {
                boxitem.WidthRequest = ColumnWidth;
            }
            boxitem.Valign = Align.Start;
            boxitem.Halign = Align.Start;
            boxitem.TooltipText = checkbox.Text;
            boxitem.Add(checkbox.self);
            if (position < 0)
            {
                _flow.Add(boxitem);
            }
            else
            {
                _flow.Insert(boxitem, position);
            }
        }
        internal void NativeRemove(int index)
        {
            if (_flow.Children.Length > index)
                _flow.Remove(_flow.Children[index]);
        }
        internal void NativeClear()
        {
            foreach (var item in _flow.Children)
                _flow.Remove(item);
        }
        public void SetItemChecked(int index, bool value)
        {
            var item = _items.GetCheckedListBoxItem(index);
            if (item != null)
            {
                item.Checked = value;
            }
        }
        public void SetItemCheckState(int index, CheckState value)
        {
            var item = _items.GetCheckedListBoxItem(index);
            if (item != null)
            {
                item.Checked = value == CheckState.Checked;
            }
        }
        public class ObjectCollection : ArrayList
        {
            private readonly CheckedListBox _owner;
            public ObjectCollection(CheckedListBox owner)
            {
                _owner = owner;
            }
            public override object this[int index]
            {
                get => ((CheckBox)base[index]).Text;
                set => ((CheckBox)base[index]).Text = value?.ToString();
            }
            public CheckBox GetCheckedListBoxItem(int index)
            {
                if (index < Count)
                    return base[index] as CheckBox;
                return null;
            }

            public override int Add(object? value)
            {
                return AddCore(value, false, -1);
            }
            public int Add(object item, bool isChecked)
            {
                return AddCore(item, isChecked, -1);
            }

            public int AddCore(object item, bool isChecked, int position)
            {
                var listBoxItem = new CheckBox();
                listBoxItem.Text=item.ToString();
                listBoxItem.Checked = isChecked;
                _owner.NativeAdd(listBoxItem, isChecked, position);
                if (position < 0)
                {
                    return base.Add(listBoxItem);
                }

                base.Insert(position, listBoxItem);
                return position;
            }

        public override void AddRange(ICollection c)
        {
            foreach (var o in c)
                AddCore(o, false, -1);
        }
        public override void RemoveAt(int index)
        {
            _owner.NativeRemove(index);
            base.RemoveAt(index);
        }
        public override void Remove(object? obj)
        {
            var index = base.IndexOf(obj);
            if (index>-1)
            {
                RemoveAt(index);
            }
        }
        public override void Clear()
        {
            _owner.NativeClear();
            base.Clear();
        }
    }

    public class CheckedIndexCollection : List<int>
    {
    }

    public class CheckedItemCollection : List<object>
    {

    }
    public class SelectedItemCollection : List<object>
    {

    }
}