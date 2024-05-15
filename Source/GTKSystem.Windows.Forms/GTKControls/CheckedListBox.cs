/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using Pango;
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
            _flow.Orientation = Gtk.Orientation.Horizontal;
            _flow.Halign = Gtk.Align.Start;
            _flow.Valign = Gtk.Align.Start;
            _items = new ObjectCollection(_flow);
            _flow.ChildActivated += Control_ChildActivated;
            _flow.Realized += _flow_Realized;
            Gtk.ScrolledWindow scrolledWindow = new Gtk.ScrolledWindow();
            scrolledWindow.Halign = Gtk.Align.Fill;
            scrolledWindow.Valign = Gtk.Align.Fill;
            scrolledWindow.Hexpand = true;
            scrolledWindow.Vexpand = true;
            scrolledWindow.Child = _flow;
            self.Child = scrolledWindow;
        }

        private void _flow_Realized(object sender, EventArgs e)
        {
            foreach(var item in _items)
            {
                AddItem(item, false, -1);
            }
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
        public bool MultiColumn { get; set; }
        //public bool MultiColumn1
        //{
        //    get { return _flow.Orientation == Gtk.Orientation.Vertical; }
        //    set { _flow.Orientation = value == true ? Gtk.Orientation.Vertical : Gtk.Orientation.Horizontal; }
        //}

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
        internal void AddItem(object item, bool isChecked, int position)
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
                if (this.ItemCheck != null)
                    this.ItemCheck(item.TooltipText, new ItemCheckEventArgs(item.Index, box.Active == true ? CheckState.Checked : CheckState.Unchecked, box.Active == false ? CheckState.Checked : CheckState.Unchecked));
                if (this.CheckOnClick == true)
                    this._flow.SelectChild(item);
            };
            Gtk.Box hBox = new Gtk.Box(Gtk.Orientation.Horizontal, 0);
            hBox.Add(box);
            hBox.Add(new Gtk.Label(item.ToString()) { Xalign = 0, Halign = Gtk.Align.Start, Valign = Gtk.Align.Start, WidthRequest = this.ColumnWidth, Ellipsize=EllipsizeMode.End }); ;

            Gtk.FlowBoxChild boxitem = new Gtk.FlowBoxChild();
            boxitem.HeightRequest = this.ItemHeight;
            if (this.MultiColumn)
            {
                boxitem.WidthRequest = this.ColumnWidth;
                this._flow.MinChildrenPerLine = Convert.ToUInt32(this.Width / this.ColumnWidth);
                this._flow.MaxChildrenPerLine = Convert.ToUInt32(this.Width / this.ColumnWidth);
            }
            boxitem.Valign = Gtk.Align.Fill;
            boxitem.Halign = Gtk.Align.Fill;
            boxitem.TooltipText = item.ToString();
            boxitem.Add(hBox);
            if (position < 0)
            {
                this._flow.Add(boxitem);
                if (self.IsRealized)
                {
                    self.ShowAll();
                }
            }
            else
            {
                this._flow.Insert(boxitem, position);
                if (self.IsRealized)
                {
                    self.ShowAll();
                }
            }
        }
        public class ObjectCollection : ArrayList
        {
            public ObjectCollection(Gtk.Container ownerContainer)
            {

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
                if (position < 0)
                {
                    return base.Add(item);
                }
                else
                {
                    base.Insert(position, item);
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
