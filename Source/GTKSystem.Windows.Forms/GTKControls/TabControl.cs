//基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
//使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows和linux运行
//技术支持438865652@qq.com，https://www.cnblogs.com/easywebfactory
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class TabControl : WidgetControl<Gtk.Notebook>
    {
        private TabControl.ControlCollection _controls;
        public TabControl() : base()
        {
            Widget.StyleContext.AddClass("TabControl");
            _controls = new ControlCollection(this);
        }

        public int SelectedIndex { get { return base.Control.CurrentPage; } set { base.Control.CurrentPage = value; } }
     
        public TabPage SelectedTab { get { return _controls[base.Control.CurrentPage]; } set { } }

        public TabSizeMode SizeMode { get; set; }

        public bool ShowToolTips { get; set; }

        public int TabCount { get; }
 
        public new TabControl.ControlCollection Controls => _controls;
        public event EventHandler SelectedIndexChanged
        {
            add { base.Control.SwitchPage += (object sender, Gtk.SwitchPageArgs e) => { if (base.Control.IsRealized) { value.Invoke(sender, e); } }; }
            remove { base.Control.SwitchPage -= (object sender, Gtk.SwitchPageArgs e) => { if (base.Control.IsRealized) { value.Invoke(sender, e); } }; }
        }
        public class ControlCollection : List<TabPage>
        {
            TabControl _owner;
            public ControlCollection(TabControl owner)
            {
                _owner = owner;
            }
            public new int Add(TabPage item)
            {
                base.Add(item);
                return _owner.Control.AppendPage(item.Control, item.TabLabel);
            }
            public new void RemoveAt(int index)
            {
                base.RemoveAt(index);
                _owner.Control.RemovePage(index);
            }
            public new void Remove(TabPage value)
            {
                base.Remove(value);
                _owner.Control.Remove(((TabPage)value).Widget);
            }
        }
    }
}
