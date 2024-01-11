/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
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
            base.Control.SupportMultidevice = true;
            base.Control.TruncateMultiline = true;
            base.Control.Valign = Gtk.Align.Start;
            base.Control.Halign = Gtk.Align.Start;
        }
        public override string Text { get { return base.Control.Text; } set { base.Control.Text = value; } }
        public virtual char PasswordChar { get => base.Control.InvisibleChar; set { base.Control.InvisibleChar = value; } }
        public override event EventHandler TextChanged
        {
            add { base.Control.Changed += (object sender, EventArgs e) => { value.Invoke(this, e); }; }
            remove { base.Control.Changed -= (object sender, EventArgs e) => { value.Invoke(this, e); }; }
        }
        public bool Multiline { get; set; }
    }
}
