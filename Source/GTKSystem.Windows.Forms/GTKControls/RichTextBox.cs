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
    public partial class RichTextBox : WidgetControl<Gtk.TextView>
    {
        public RichTextBox()
        {
            Widget.StyleContext.AddClass("RichTextBox");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
            base.Control.BorderWidth = 1;
        }

        public override string Text { get => base.Control.Buffer.Text; set => base.Control.Buffer.Text = value; }

        public override event EventHandler TextChanged
        {
            add { base.Control.Buffer.Changed += (object o, EventArgs args) => { value.Invoke(this, args); }; }
            remove { base.Control.Buffer.Changed -= (object o, EventArgs args) => { value.Invoke(this, args); }; }
        }

        public void AppendText(string text)
        {
            var enditer = base.Control.Buffer.EndIter;
            base.Control.Buffer.Insert(ref enditer, text);
        }

        public string[] Lines
        {
            get { return base.Control.Buffer.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None); }
        }
    }
}
