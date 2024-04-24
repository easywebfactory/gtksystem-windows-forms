/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using Gtk;
using GTKSystem.Windows.Forms;
using GTKSystem.Windows.Forms.Utility;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Button : Control // WidgetControl<Button.GtkButton>
    {
        public GtkButton self = new GtkButton();
        public override Widget Widget => self;
        public Button() : base()
        {
            self.Override.AddClass("Button");
            //self.Override.DrawnBackground += Control_DrawnBackground;
        }

        private void Control_DrawnBackground(object o, DrawnArgs args)
        {
            Gdk.Rectangle rec = Widget.Allocation;


            Graphics g = new Graphics(this.Widget, args.Cr, rec);
            using SolidBrush brush = new SolidBrush(Color.Yellow);
            g.FillRectangle(brush, new Rectangle(0, 0, rec.Width, rec.Height));
            using SolidBrush brush2 = new SolidBrush(Color.White);
            g.DrawEllipse(new Pen(brush2, 5), new Rectangle(10, 10, 30, 20));
        }

        public override string Text { get => self.Label; set => self.Label = value; }

        public override ImageLayout BackgroundImageLayout { get => self.Override.BackgroundImageLayout; set => self.Override.BackgroundImageLayout = value; }
        public override Drawing.Image BackgroundImage { get => self.Override.BackgroundImage; set => self.Override.BackgroundImage = value; }
        public override Color BackColor { get => self.Override.BackColor.HasValue ? self.Override.BackColor.Value : Color.Transparent; set => self.Override.BackColor = value; }

        public override event PaintEventHandler Paint
        {
            add { self.Override.Paint += value; }
            remove { self.Override.Paint -= value; }
        }
        public sealed class GtkButton : Gtk.Button
        {
            internal Button owner { set; get; }
            internal GtkButton() : base()
            {
                this.Override = new GtkControlOverride(this);
            }
            public GtkControlOverride Override { get; set; }
 
            protected override void OnShown()
            {
                Override.OnAddClass();
                base.OnShown();
            }
            protected override bool OnDrawn(Cairo.Context cr)
            {
                Gdk.Rectangle rec = this.Allocation;
                Override.OnDrawnBackground(cr, rec);
                Override.OnPaint(cr, rec);
                return base.OnDrawn(cr);
            }
        }
    }
}
