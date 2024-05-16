/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;

namespace System.Windows.Forms
{

    [DesignerCategory("Component")]
    public partial class ComboBox: Control
    {
        public readonly ComboBoxBase self = new ComboBoxBase();
        public override object GtkControl => self;
        private ObjectCollection __itemsData;
        public ComboBox():base()
        {
            self.Entry.HasFrame = false;
            self.Entry.WidthChars = 0;
            __itemsData = new ObjectCollection(this);
        }
        public bool FormattingEnabled { get; set; }
        private ComboBoxStyle _DropDownStyle;
        public ComboBoxStyle DropDownStyle { 
            get=> _DropDownStyle; 
            set {
                _DropDownStyle = value; 
                if(value==ComboBoxStyle.DropDown) {
                    self.StyleContext.AddClass("DropDown");
                }
                else if (value == ComboBoxStyle.DropDownList)
                {
                    self.StyleContext.AddClass("DropDownList");
                    self.Entry.IsEditable = false;
                    self.Entry.CanFocus = false;
                }
                else
                {
                    self.StyleContext.RemoveClass("DropDown");
                    self.StyleContext.RemoveClass("DropDownList");
                }
            } 
        }
        public override string Text { get => self.Entry.Text; set { self.Entry.Text = value; } }
        public object SelectedItem { 
            get { return __itemsData[SelectedIndex]; } 
            set { int _index = __itemsData.IndexOf(value); if (_index != -1) { SelectedIndex = _index; } } 
        }
        internal int _selectedIndex;
        public int SelectedIndex { get { return self.Active; } set { self.Active = value; _selectedIndex = value; } }
        public ObjectCollection Items { get { return __itemsData; } }
        public string GetItemText(object item)
        {
            return item.ToString();
        }
        public string NativeGetItemText(int index)
        {
            return __itemsData[index].ToString();
        }
        private bool _sorted;
        public bool Sorted { get=> _sorted; set=> _sorted = value; }
        public event EventHandler SelectedIndexChanged
        {
            add { self.Changed += (object sender, EventArgs e) => { value.Invoke(this, e);  }; }
            remove { self.Changed -= (object sender, EventArgs e) => { value.Invoke(this, e); }; }
        }
        public event EventHandler SelectedValueChanged
        {
            add { self.Changed += (object sender, EventArgs e) => { value.Invoke(this, e); }; }
            remove { self.Changed -= (object sender, EventArgs e) => { value.Invoke(this, e); }; }
        }
    }

}
