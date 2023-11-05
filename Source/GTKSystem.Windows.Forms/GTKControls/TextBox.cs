//基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
//使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows和linux运行
//技术支持438865652@qq.com，https://www.cnblogs.com/easywebfactory
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class TextBox: WidgetControl<Gtk.Entry>
    {
        public TextBox() : base()
        {
            Control.StyleContext.AddClass("TextBox");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
            base.Control.HasFrame = false;
            base.Control.MaxWidthChars = 1;
            base.Control.WidthChars = 0;

        }
        public override string Text { get { return base.Control.Text; } set { base.Control.Text = value; } }
        public virtual char PasswordChar { get => base.Control.InvisibleChar; set { base.Control.InvisibleChar = value; } }
        public override event EventHandler TextChanged
        {
            add { base.Control.Changed += (object sender, EventArgs e) => { value.Invoke(sender, e); }; }
            remove { base.Control.Changed -= (object sender, EventArgs e) => { value.Invoke(sender, e); }; }
        }

    }
}
