/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{

    [DesignerCategory("Component")]
    public partial class CheckedListBox : ContainerControl
    {
        public readonly CheckedListBoxBase self = new CheckedListBoxBase();
        public override object GtkControl => self;
        internal Gtk.FlowBox _flow = new Gtk.FlowBox();
        private ObjectCollection _items;
        public CheckedListBox() : base()
        {
            _items = new ObjectCollection(this);
            _flow.Orientation = Gtk.Orientation.Horizontal;
            _flow.MinChildrenPerLine = 1;
            _flow.MaxChildrenPerLine = 99999;
            _flow.Halign = Gtk.Align.Fill;
            _flow.Valign = Gtk.Align.Fill;
            _flow.ChildActivated += Control_ChildActivated;
            _flow.Realized += _flow_Realized;
            self.AutoScroll = true;
            self.Add(_flow);
            
            this.BorderStyle = BorderStyle.Fixed3D;
        }
        private bool is_flow_Realized;
        private void _flow_Realized(object sender, EventArgs e)
        {
            if (is_flow_Realized == false)
            {
                is_flow_Realized = true;
                foreach (var item in _items)
                {
                    NativeAdd(item, false, -1);
                }
                _flow.ShowAll();
            }
        }

        private void Control_ChildActivated(object o, Gtk.ChildActivatedArgs args)
        {
            int rowIndex = args.Child.Index;
            ObjectCollection.CheckedListBoxItem sender = this.Items.GetCheckedListBoxItem(rowIndex);
            sender.Selected = args.Child.IsSelected;
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, args);
            if (SelectedValueChanged != null)
                SelectedValueChanged(this, args);
            if (SelectedItemChanged != null)
                SelectedItemChanged(this, args);
        }

        public int ColumnWidth { get; set; }
        public bool MultiColumn { get { return _flow.Orientation == Gtk.Orientation.Vertical; } set { if (value == false) { _flow.Orientation = Gtk.Orientation.Horizontal; } else { _flow.Orientation = Gtk.Orientation.Vertical; } } }
        public bool HorizontalScrollbar { get; set; }
        public bool FormattingEnabled { get; set; }
        public int ItemHeight { get; set; }
        public SelectionMode SelectionMode { get; set; }
        public void ClearSelected() {
            foreach(Gtk.FlowBoxChild wi in _flow.Children) { 
                Gtk.Widget box = ((Gtk.Box)wi.Child).Children[0];
                if (box is Gtk.CheckButton check)
                {
                    check.Active = false;
                }
            };
            _flow.UnselectAll();
        }
        public bool CheckOnClick { get; set; }
        public ObjectCollection Items { 
            get {
                ArrayList list = new ArrayList();
                foreach (var item in _items)
                {
                    list.Add(item);
                }
                return _items;

            } 
        }
        public SelectedItemCollection SelectedItems
        {
            get
            {
                SelectedItemCollection items = new SelectedItemCollection();
                for (int i = 0; i < _items.Count; i++)
                {
                    ObjectCollection.CheckedListBoxItem item = _items.GetCheckedListBoxItem(i);
                    if (item.Selected == true)
                        items.Add(item.Item);
                }
                return items;
            }
        }
        public CheckedItemCollection CheckedItems { 
            get {
                CheckedItemCollection items = new CheckedItemCollection();
                for (int i = 0; i < _items.Count; i++)
                {
                    ObjectCollection.CheckedListBoxItem item = _items.GetCheckedListBoxItem(i);
                    if (item.Checked == true)
                        items.Add(item.Item);
                }
                return items; } 
        }

        public CheckedIndexCollection CheckedIndices
        {
            get
            {
                CheckedIndexCollection items = new CheckedIndexCollection();
                for (int i = 0; i < _items.Count; i++)
                {
                    ObjectCollection.CheckedListBoxItem item = _items.GetCheckedListBoxItem(i);
                    if (item.Checked == true)
                        items.Add(i);
                }
                return items;
            }
        }

        public event ItemCheckEventHandler ItemCheck;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler SelectedValueChanged;
        public event EventHandler SelectedItemChanged;
        internal void NativeAdd(object item, bool isChecked, int position)
        {
            if (_flow.IsRealized)
            {
                Gtk.CheckButton box = new Gtk.CheckButton();
                box.Label = item.ToString();
                box.Active = isChecked;
                box.Hexpand = false;
                box.Halign = Gtk.Align.Start;
                box.Valign = Gtk.Align.Center;
                box.Toggled += (object sender, EventArgs e) =>
                {
                    Gtk.CheckButton box = (Gtk.CheckButton)sender;
                    Gtk.FlowBoxChild item = box.Parent as Gtk.FlowBoxChild;
                    Items.GetCheckedListBoxItem(item.Index).Checked = box.Active;
                    if (this.ItemCheck != null)
                        this.ItemCheck(this, new ItemCheckEventArgs(item.Index, box.Active == true ? CheckState.Checked : CheckState.Unchecked, box.Active == false ? CheckState.Checked : CheckState.Unchecked));
                    if (this.CheckOnClick == true)
                        this._flow.SelectChild(item);
                };
                Gtk.FlowBoxChild boxitem = new Gtk.FlowBoxChild();
                boxitem.HeightRequest = this.ItemHeight;
                if (this.MultiColumn && this.ColumnWidth > 0)
                {
                    boxitem.WidthRequest = this.ColumnWidth;
                }
                boxitem.Valign = Gtk.Align.Start;
                boxitem.Halign = Gtk.Align.Start;
                boxitem.TooltipText = item.ToString();
                boxitem.Add(box);
                if (position < 0)
                {
                    this._flow.Add(boxitem);
                }
                else
                {
                    this._flow.Insert(boxitem, position);
                }
            }
        }
        internal void NativeRemove(int index)
        {
            if (_flow.IsRealized)
            {
                if (_flow.Children.Length > index)
                    _flow.Remove(_flow.Children[index]);
            }
        }
        internal void NativeClear()
        {
            if (_flow.IsRealized)
            {
                foreach (var item in _flow.Children)
                    _flow.Remove(item);
            }

        }

        public class ObjectCollection : ArrayList
        {
            public class CheckedListBoxItem
            {
                public object Item { get; set; }
                public bool Checked { get; set; }
                public bool Selected { get; set; }
                public override string ToString()
                {
                    return Item?.ToString();
                }
            }
            private CheckedListBox _owner;
            public ObjectCollection(CheckedListBox owner)
            {
                _owner = owner;
            }
            public override object this[int index]
            {
                get => ((CheckedListBoxItem)base[index]).Item;
                set => ((CheckedListBoxItem)base[index]).Item = value;
            }
            public CheckedListBoxItem GetCheckedListBoxItem(int index)
            {
                return base[index] as CheckedListBoxItem;
            }
            public override int Add(object value)
            {
                return AddCore(value, false, -1);
            }
            public int Add(object item, bool isChecked)
            {
                return AddCore(item, isChecked, -1);
            }

            public int AddCore(object item, bool isChecked, int position)
            {
                _owner.NativeAdd(item, isChecked, position);
                CheckedListBoxItem listBoxItem = new CheckedListBoxItem();
                listBoxItem.Item = item;
                listBoxItem.Checked = isChecked;
                listBoxItem.Selected = false;

                if (position < 0)
                {
                    return base.Add(listBoxItem);
                }
                else
                {
                    base.Insert(position, listBoxItem);
                    return position;
                }
            }

            public override void AddRange(ICollection c)
            {
                foreach (object o in c)
                    AddCore(o, false, -1);
            }
            public override void RemoveAt(int index)
            {
                _owner.NativeRemove(index);
                base.RemoveAt(index);
            }
            public override void Remove(object obj)
            {
                int index = base.IndexOf(obj);
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
}
