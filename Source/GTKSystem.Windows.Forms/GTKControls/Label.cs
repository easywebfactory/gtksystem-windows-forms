//基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
//使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows和linux运行
//技术支持438865652@qq.com，https://www.cnblogs.com/easywebfactory
using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Label : WidgetControl<Gtk.Label>
    {
        public Label() : base() {
            Widget.StyleContext.AddClass("Label");
        }

        public override string Text { get => base.Control.Text; set => base.Control.Text = value; }

    }
}
