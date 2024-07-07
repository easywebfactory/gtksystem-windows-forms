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
    public partial class TextBox: Control
    {
        public readonly TextBoxBase self = new TextBoxBase();
        public override object GtkControl => self;
        public TextBox() : base()
        {
            //self.HasFrame = false;
            self.ShadowType = Gtk.ShadowType.In;
            //self.HasDefault = false;
            self.MaxWidthChars = 1;
            self.WidthChars = 0;

            self.Valign = Gtk.Align.Start;
            self.Halign = Gtk.Align.Start;
        }
        public string PlaceholderText { get { return self.PlaceholderText; } set { self.PlaceholderText = value ?? ""; } }
        public override string Text { get { return self.Text; } set { self.Text = value ?? ""; } }
        public virtual char PasswordChar { get => self.InvisibleChar; set { self.InvisibleChar = value; self.Visibility = false; } }
        public virtual bool ReadOnly { get { return self.IsEditable; } set { self.IsEditable = value;  } }
        public override event EventHandler TextChanged
        {
            add { self.Changed += (object sender, EventArgs e) => { value.Invoke(this, e); }; }
            remove { self.Changed -= (object sender, EventArgs e) => { value.Invoke(this, e); }; }
        }
        public bool Multiline { get; set; }
    }
}
