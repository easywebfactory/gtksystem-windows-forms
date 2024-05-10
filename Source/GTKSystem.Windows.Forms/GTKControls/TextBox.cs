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
    public partial class TextBox: Control
    {
        public readonly TextBoxBase self = new TextBoxBase();
        public override object GtkControl => self;
        public TextBox() : base()
        {
            self.HasFrame = false;
            self.MaxWidthChars = 1;
            self.WidthChars = 0;
            self.SupportMultidevice = true;
            self.TruncateMultiline = true;
            self.Valign = Gtk.Align.Start;
            self.Halign = Gtk.Align.Start;
        }
        public override string Text { get { return self.Text; } set { self.Text = value ?? ""; } }
        public virtual char PasswordChar { get => self.InvisibleChar; set { self.InvisibleChar = value; } }
        public override event EventHandler TextChanged
        {
            add { self.Changed += (object sender, EventArgs e) => { value.Invoke(this, e); }; }
            remove { self.Changed -= (object sender, EventArgs e) => { value.Invoke(this, e); }; }
        }
        public bool Multiline { get; set; }
    }
}
