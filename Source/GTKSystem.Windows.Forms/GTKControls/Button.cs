/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Button : Control
    {
        public readonly ButtonBase self = new ButtonBase();
        public override object GtkControl => self;
        public Button() : base()
        {
            self.Clicked += Self_Clicked;
        }
        private void Self_Clicked(object sender, EventArgs e)
        {
            if(Click!= null && self.IsVisible) { Click(this, EventArgs.Empty); }
        }

        public override string Text { get => ((Gtk.Label)self.Child).Text; set => ((Gtk.Label)self.Child).Text = value; }

        public override event EventHandler Click;
        public override RightToLeft RightToLeft { get { return self.Direction == Gtk.TextDirection.Rtl ? RightToLeft.Yes : RightToLeft.No; } set { self.Direction = value == RightToLeft.Yes ? Gtk.TextDirection.Rtl : Gtk.TextDirection.Ltr; } }
        public System.Drawing.ContentAlignment TextAlign
        {
            get { return textAlign; }
            set
            {
                textAlign = value;
                if (value == System.Drawing.ContentAlignment.TopLeft)
                {
                    self.Xalign = 0.0f;
                    self.Yalign = 0.0f;
                }
                else if (value == System.Drawing.ContentAlignment.TopCenter)
                {
                    self.Xalign = 0.5f;
                    self.Yalign = 0.0f;
                }
                else if (value == System.Drawing.ContentAlignment.TopRight)
                {
                    self.Xalign = 1.0f;
                    self.Yalign = 0.0f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleLeft)
                {
                    self.Xalign = 0.0f;
                    self.Yalign = 0.5f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleCenter)
                {
                    self.Xalign = 0.5f;
                    self.Yalign = 0.5f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleRight)
                {
                    self.Xalign = 1.0f;
                    self.Yalign = 0.5f;
                }
                else if (value == System.Drawing.ContentAlignment.BottomLeft)
                {
                    self.Xalign = 0.0f;
                    self.Yalign = 1.0f;
                }
                else if (value == System.Drawing.ContentAlignment.BottomCenter)
                {
                    self.Xalign = 0.5f;
                    self.Yalign = 1.0f;
                }
                else if (value == System.Drawing.ContentAlignment.BottomRight)
                {
                    self.Xalign = 1.0f;
                    self.Yalign = 1.0f;
                }

            }
        }
        private System.Drawing.ContentAlignment textAlign;
    }
}
