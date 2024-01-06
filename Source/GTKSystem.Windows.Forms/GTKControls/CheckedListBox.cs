/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
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
    public partial class CheckedListBox : WidgetContainerControl<Gtk.HBox>
    {
        ObjectCollection _items;
        internal Gtk.FlowBox _flow;
        public CheckedListBox() : base()
        {
            Widget.StyleContext.AddClass("CheckedListBox");
            Widget.StyleContext.AddClass("BorderRadiusStyle");

            _flow = new Gtk.FlowBox();
            _flow.MaxChildrenPerLine = 3u;

            _items = new ObjectCollection(this, _flow);
            _flow.ChildActivated += Control_ChildActivated;
            _flow.UnselectedAll += _flow_UnselectedAll;
            _flow.Halign = Gtk.Align.Start;
            Gtk.ScrolledWindow scrolledWindow = new Gtk.ScrolledWindow();
            Gtk.Viewport viewport = new Gtk.Viewport();
            viewport.Add(_flow);
            scrolledWindow.Add(viewport);
            this.Control.Add(scrolledWindow);
        }

        private void _flow_UnselectedAll(object sender, EventArgs e)
        {


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
                Gtk.Widget box = ((Gtk.HBox)wi.Child).Children[0];
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
                Gtk.HBox hBox = new Gtk.HBox();
                hBox.Valign = Gtk.Align.Start;
                hBox.Halign = Gtk.Align.Start;
                hBox.Add(box);
                hBox.Add(new Gtk.Label(item.ToString()) { Xalign = 0, Halign = Gtk.Align.Start, Valign = Gtk.Align.Start }); ;

                Gtk.FlowBoxChild boxitem = new Gtk.FlowBoxChild();
                boxitem.HeightRequest = __owner.ItemHeight;
                boxitem.WidthRequest = __owner.ColumnWidth;
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
