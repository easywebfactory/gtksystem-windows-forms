//基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
//使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows和linux运行
//技术支持438865652@qq.com，https://www.cnblogs.com/easywebfactory
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;


namespace System.Windows.Forms
{

    [DesignerCategory("Component")]
    public partial class CheckedListBox : WidgetControl<Gtk.ListBox>
    {
        ObjectCollection _items;
        public CheckedListBox() : base()//base(Gtk.Orientation.Vertical, 0)
        {
            Widget.StyleContext.AddClass("CheckedListBox");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
            _items = new ObjectCollection(this);

        }
         

        public bool MultiColumn { get; set; }
        public bool HorizontalScrollbar { get; set; }
        public bool FormattingEnabled { get; set; }
        public int ItemHeight { get; set; }
        public SelectionMode SelectionMode { get; set; }
        private bool isClearSelected;
        public void ClearSelected() {
            isClearSelected = true;
            base.Control.UnselectAll();
            foreach (object item in _items)
            {
                CheckBox box = (CheckBox)item;
                if (box.Checked == true)
                    box.Checked = false;
            }
            isClearSelected = false;
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
        public void SendEvent(object sender, EventArgs e)
        {
            if (isClearSelected == false)
            {
                CheckBox checkbox = (CheckBox)sender;
                base.Control.SelectRow(base.Control.GetRowAtIndex(Convert.ToInt32(checkbox.Name)));

                if (ItemCheck != null)
                    ItemCheck(sender, new ItemCheckEventArgs(Convert.ToInt32(checkbox.Name), checkbox.CheckState, checkbox.CheckState == CheckState.Unchecked ? CheckState.Checked : CheckState.Unchecked));
                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(checkbox, e);
                if (SelectedValueChanged != null)
                    SelectedValueChanged(checkbox, e);
            }
        }

        public event ItemCheckEventHandler ItemCheck;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler SelectedValueChanged;

        public class ObjectCollection : ControlCollection
        {
            private CheckedListBox __owner;//CheckedListBox
            public ObjectCollection(CheckedListBox owner) : base(owner,typeof(CheckBox))
            {
                __owner = owner;
            }
            public override void Add(Type itemType, object item)
            {
                CheckBox box = new CheckBox();
                box.Control.Label = item.ToString();
                box.Control.Name = Count.ToString();

                box.Control.Toggled += (object sender, EventArgs e) =>
                {
                    __owner.SendEvent(box, e); 
                };
                base.Add(box);
            }

            /// <summary>
            ///  Lets the user add an item to the listbox with the given initial value
            ///  for the Checked portion of the item.
            /// </summary>
            public int Add(CheckBox item, bool isChecked)
            {
                item.Checked = isChecked;
                return Add(item, isChecked ? CheckState.Checked : CheckState.Unchecked);
            }

            /// <summary>
            ///  Lets the user add an item to the listbox with the given initial value
            ///  for the Checked portion of the item.
            /// </summary>
            public int Add(CheckBox item, CheckState state)
            {
                item.CheckState = state;
                int index = base.Add(item);
                return index;
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
