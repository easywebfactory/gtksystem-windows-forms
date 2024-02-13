/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using GLib;
using Gtk;
using Pango;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{

    [DesignerCategory("Component")]
    public partial class CheckedListBox : WidgetContainerControl<Gtk.Viewport>
    {
        ObjectCollection _items;
        internal Gtk.FlowBox _flow;
        public CheckedListBox() : base()
        {
            this.BackColor = Drawing.Color.White;
            Widget.StyleContext.AddClass("CheckedListBox");
            Widget.StyleContext.AddClass("BorderRadiusStyle");

            _flow = new Gtk.FlowBox();
            // _flow.MaxChildrenPerLine = 3u;
            _flow.Halign = Gtk.Align.Start;
            _flow.Valign = Gtk.Align.Start;
            _flow.Hexpand = true;
            _flow.Vexpand = true;
            _items = new ObjectCollection(this, _flow);
            _flow.ChildActivated += Control_ChildActivated;
            Gtk.ScrolledWindow scrolledWindow = new Gtk.ScrolledWindow();
            scrolledWindow.Halign = Gtk.Align.Fill;
            scrolledWindow.Valign = Gtk.Align.Fill;
            scrolledWindow.Hexpand = true;
            scrolledWindow.Vexpand = true;
            scrolledWindow.Child = _flow;
            this.Control.Child = scrolledWindow;
        }

        private void Control_ChildActivated(object o, Gtk.ChildActivatedArgs args)
        {
            int rowIndex = args.Child.Index;
            object sender = this.Items[rowIndex];
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(sender, args);
            if (SelectedValueChanged != null)
                SelectedValueChanged(sender, args);
        }

        public int ColumnWidth { get; set; } = 160;
        public bool MultiColumn { 
            get { return _flow.Orientation == Gtk.Orientation.Vertical; }
            set { _flow.Orientation = value == true ? Gtk.Orientation.Vertical : Gtk.Orientation.Horizontal;   } }
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
        public override Drawing.Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        public ObjectCollection Items { 
            get {
                return _items; 
            } 
        }

        public CheckedItemCollection CheckedItems { 
            get {
                CheckedItemCollection items = new CheckedItemCollection();
                foreach (object item in _items)
                {
                    CheckBox box= (CheckBox)item;
                    if (box.Checked == true)
                        items.Add(box.Text);
                }
                return items; } 
        }

        public CheckedIndexCollection CheckedIndices
        {
            get
            {
                CheckedIndexCollection items = new CheckedIndexCollection();
                int index = 0;
                foreach (object item in _items)
                {
                    if (((CheckBox)item).Checked == true)
                        items.Add(index);
                    index++;
                }
                return items;
            }
        }

        public event ItemCheckEventHandler ItemCheck;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler SelectedValueChanged;

        public class ObjectCollection : ArrayList
        {
            private CheckedListBox __owner;//CheckedListBox
            public ObjectCollection(CheckedListBox owner,Gtk.Container ownerContainer) //: base(owner, ownerContainer)
            {
                __owner = owner;
            }
            public override int Add(object value)
            {
                return AddCore(value, false, -1);
            }
            public int Add(object item, bool isChecked)
            {
                return AddCore(item, isChecked, -1);
            }

            public int Add(object item, CheckState state)
            {
                return AddCore(item, state== CheckState.Checked, -1);
            }

            public int AddCore(object item, bool isChecked, int position)
            {
                Gtk.CheckButton box = new Gtk.CheckButton();
                box.Label = " ";// item.ToString();
                box.Active = isChecked;
                box.WidthRequest = 20;
                box.Halign = Gtk.Align.Start;
                box.Valign = Gtk.Align.Start;
                box.Toggled += (object sender, EventArgs e) =>
                {
                    Gtk.CheckButton box = (Gtk.CheckButton)sender;
                    Gtk.FlowBoxChild item = box.Parent.Parent as Gtk.FlowBoxChild;
                    if (__owner.ItemCheck != null)
                        __owner.ItemCheck(item.TooltipText, new ItemCheckEventArgs(item.Index, box.Active == true ? CheckState.Checked : CheckState.Unchecked, box.Active == false ? CheckState.Checked : CheckState.Unchecked));
                    if (__owner.CheckOnClick == true)
                        __owner._flow.SelectChild(item);
                };
                Gtk.Box hBox = new Gtk.Box(Gtk.Orientation.Horizontal, 0);
                hBox.Add(box);
                hBox.Add(new Gtk.Label(item.ToString()) { Xalign = 0, Halign = Gtk.Align.Start, Valign = Gtk.Align.Start }); ;

                Gtk.FlowBoxChild boxitem = new Gtk.FlowBoxChild();
                boxitem.HeightRequest = __owner.ItemHeight;
                if (__owner.MultiColumn)
                {
                    boxitem.WidthRequest = __owner.ColumnWidth;
                    __owner._flow.MaxChildrenPerLine = Convert.ToUInt32(__owner.Width/ __owner.ColumnWidth);
                }
                boxitem.Valign = Gtk.Align.Fill;
                boxitem.Halign = Gtk.Align.Fill;
                boxitem.TooltipText = item.ToString();
                boxitem.Add(hBox);
                if (position < 0)
                {
                    __owner._flow.Add(boxitem);
                    if (__owner.Control.IsRealized)
                    {
                        __owner.Control.ShowAll();
                    }
                    return base.Add(item);
                }
                else
                {
                    __owner._flow.Insert(boxitem, position);
                    base.Insert(position, item);
                    if (__owner.Control.IsRealized)
                    {
                        __owner.Control.ShowAll();
                    }
                    return position;
                }
            }

            public override void AddRange(ICollection c)
            {
                foreach (object o in c)
                    AddCore(o, false, -1);
                base.AddRange(c);
            }
        }

        public class CheckedIndexCollection : List<int>
        {
        }

        public class CheckedItemCollection : List<object>
        {

        }

    }
}
