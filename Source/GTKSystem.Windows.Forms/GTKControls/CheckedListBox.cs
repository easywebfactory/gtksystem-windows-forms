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
public class CheckedListBox : ContainerControl
{
    public readonly CheckedListBoxBase self = new();
    public override object GtkControl => self;
    internal FlowBox _flow = new();
    private readonly ObjectCollection _items;
    public CheckedListBox()
    {
        _items = new ObjectCollection(this);
        _flow.Orientation = Gtk.Orientation.Horizontal;
        _flow.MinChildrenPerLine = 1;
        _flow.MaxChildrenPerLine = 99999;
        _flow.Halign = Align.Fill;
        _flow.Valign = Align.Fill;
        _flow.ChildActivated += Control_ChildActivated;
        _flow.Realized += _flow_Realized;
        self.AutoScroll = true;
        self.Add(_flow);
            
        BorderStyle = BorderStyle.Fixed3D;
    }
    private bool isFlowRealized;
    private void _flow_Realized(object? sender, EventArgs e)
    {
        if (isFlowRealized == false)
        {
            isFlowRealized = true;
            foreach (var item in _items)
            {
                NativeAdd(item, false, -1);
            }
            _flow.ShowAll();
        }
    }

    private void Control_ChildActivated(object? o, ChildActivatedArgs args)
    {
        var rowIndex = args.Child.Index;
        var sender = Items.GetCheckedListBoxItem(rowIndex);
        if (sender != null)
        {
            sender.Selected = args.Child.IsSelected;
        }

        SelectedIndexChanged?.Invoke(this, args);
        SelectedValueChanged?.Invoke(this, args);
        SelectedItemChanged?.Invoke(this, args);
    }

    public int ColumnWidth { get; set; }
    public bool MultiColumn { get => _flow.Orientation == Gtk.Orientation.Vertical;
        set { if (value == false) { _flow.Orientation = Gtk.Orientation.Horizontal; } else { _flow.Orientation = Gtk.Orientation.Vertical; } } }
    public bool HorizontalScrollbar { get; set; }
    public bool FormattingEnabled { get; set; }
    public int ItemHeight { get; set; }
    public SelectionMode SelectionMode { get; set; }
    public void ClearSelected() {
        foreach(var wi in _flow.Children.Cast<FlowBoxChild>()) { 
            var box = ((Box)wi.Child).Children[0];
            if (box is CheckButton check)
            {
                check.Active = false;
            }
        }
        _flow.UnselectAll();
    }
    public bool CheckOnClick { get; set; }
    public ObjectCollection Items { 
        get {
            var list = new ArrayList();
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
            var items = new SelectedItemCollection();
            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items.GetCheckedListBoxItem(i);
                if (item is { Selected: true })
                {
                    var itemItem = item.Item;
                    if (itemItem != null)
                    {
                        items.Add(itemItem);
                    }
                }
            }
            return items;
        }
    }
    public CheckedItemCollection CheckedItems { 
        get {
            var items = new CheckedItemCollection();
            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items.GetCheckedListBoxItem(i);
                if (item is { Checked: true })
                {
                    var itemItem = item.Item;
                    if (itemItem != null)
                    {
                        items.Add(itemItem);
                    }
                }
            }
            return items; } 
    }

    public CheckedIndexCollection CheckedIndices
    {
        get
        {
            var items = new CheckedIndexCollection();
            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items.GetCheckedListBoxItem(i);
                if (item is { Checked: true })
                    items.Add(i);
            }
            return items;
        }
    }

    public event ItemCheckEventHandler? ItemCheck;
    public event EventHandler? SelectedIndexChanged;
    public event EventHandler? SelectedValueChanged;
    public event EventHandler? SelectedItemChanged;
    internal void NativeAdd(object? item, bool isChecked, int position)
    {
        if (_flow.IsRealized)
        {
            var box = new CheckButton();
            box.Label = item?.ToString();
            box.Active = isChecked;
            box.Hexpand = false;
            box.Halign = Align.Start;
            box.Valign = Align.Center;
            box.Toggled += (sender, _) =>
            {
                var checkButton = (CheckButton?)sender;
                if (checkButton?.Parent is FlowBoxChild parent)
                {
                    var checkedListBoxItem = Items.GetCheckedListBoxItem(parent.Index);
                    if (checkedListBoxItem != null)
                    {
                        checkedListBoxItem.Checked = checkButton.Active;
                    }

                    ItemCheck?.Invoke(this,
                        new ItemCheckEventArgs(parent.Index,
                            checkButton.Active ? CheckState.Checked : CheckState.Unchecked,
                            checkButton.Active == false ? CheckState.Checked : CheckState.Unchecked));

                    if (CheckOnClick)
                        _flow.SelectChild(parent);
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
            boxitem.TooltipText = item?.ToString();
            boxitem.Add(box);
            if (position < 0)
            {
                _flow.Add(boxitem);
            }
            else
            {
                _flow.Insert(boxitem, position);
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
            public object? Item { get; set; }
            public bool Checked { get; set; }
            public bool Selected { get; set; }
            public override string? ToString()
            {
                return Item?.ToString();
            }
        }
        private readonly CheckedListBox _owner;
        public ObjectCollection(CheckedListBox owner)
        {
            _owner = owner;
        }
        public override object? this[int index]
        {
            get => ((CheckedListBoxItem)base[index]).Item;
            set => ((CheckedListBoxItem)base[index]).Item = value;
        }
        public CheckedListBoxItem? GetCheckedListBoxItem(int index)
        {
            return base[index] as CheckedListBoxItem;
        }
        public override int Add(object? value)
        {
            return AddCore(value, false, -1);
        }
        public int Add(object? item, bool isChecked)
        {
            return AddCore(item, isChecked, -1);
        }

        public int AddCore(object? item, bool isChecked, int position)
        {
            _owner.NativeAdd(item, isChecked, position);
            var listBoxItem = new CheckedListBoxItem
            {
                Item = item,
                Checked = isChecked,
                Selected = false
            };

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