/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Label : WidgetControl<Gtk.Label>
    {
        public Label() : base() {
            Widget.StyleContext.AddClass("Label");
            this.Control.Xalign = 0.08f;
            this.Control.Yalign = 0.08f;
        }

        public override string Text { get => base.Control.Text; set => base.Control.Text = value; }
        public override RightToLeft RightToLeft { get { return this.Control.Direction == Gtk.TextDirection.Rtl ? RightToLeft.Yes : RightToLeft.No; } set { this.Control.Direction = value == RightToLeft.Yes ?  Gtk.TextDirection.Rtl : Gtk.TextDirection.Ltr; } }
        public System.Drawing.ContentAlignment TextAlign { 
            get { return textAlign; } 
            set { 
                textAlign = value;
                if (value == System.Drawing.ContentAlignment.TopLeft)
                {
                    this.Control.Xalign = 0.08f;
                    this.Control.Yalign = 0.08f;
                }
                else if (value == System.Drawing.ContentAlignment.TopCenter)
                {
                    this.Control.Xalign = 0.5f; 
                    this.Control.Yalign = 0.08f;
                }
                else if (value == System.Drawing.ContentAlignment.TopRight)
                {
                    this.Control.Xalign = 0.92f;
                    this.Control.Yalign = 0.08f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleLeft)
                {
                    this.Control.Xalign = 0.08f;
                    this.Control.Yalign = 0.5f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleCenter)
                {
                    this.Control.Xalign = 0.5f;
                    this.Control.Yalign = 0.5f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleRight)
                {
                    this.Control.Xalign = 0.92f;
                    this.Control.Yalign = 0.5f;
                }
                else if (value == System.Drawing.ContentAlignment.BottomLeft)
                {
                    this.Control.Xalign = 0.08f;
                    this.Control.Yalign = 0.08f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleCenter)
                {
                    this.Control.Xalign = 0.5f;
                    this.Control.Yalign = 0.08f;
                }
                else if (value == System.Drawing.ContentAlignment.MiddleRight)
                {
                    this.Control.Xalign = 0.92f;
                    this.Control.Yalign = 0.08f;
                }

            }
        }
        private System.Drawing.ContentAlignment textAlign;
    }
}
