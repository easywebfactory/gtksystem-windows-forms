/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://github.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class RichTextBox : Control
    {
        public readonly RichTextBoxBase self = new RichTextBoxBase();
        public override object GtkControl => self;
        public RichTextBox():base()
        {

        }
        //protected override void SetStyle(Widget widget)
        //{
        //    base.SetStyle(self.TextView);
        //}
        public override string Text { get => self.TextView.Buffer.Text; set => self.TextView.Buffer.Text = value; }
        public virtual bool ReadOnly { get { return self.TextView.CanFocus; } set { self.TextView.CanFocus = value; } }

        public override event EventHandler TextChanged
        {
            add { self.TextView.Buffer.Changed += (object o, EventArgs args) => { value.Invoke(this, args); }; }
            remove { self.TextView.Buffer.Changed -= (object o, EventArgs args) => { value.Invoke(this, args); }; }
        }

        public void AppendText(string text)
        {
            var enditer = self.TextView.Buffer.EndIter;
            self.TextView.Buffer.Insert(ref enditer, text);
        }

        public string[] Lines
        {
            get { return self.TextView.Buffer.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None); }
        }
    }
}
