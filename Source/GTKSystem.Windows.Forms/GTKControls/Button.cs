/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Button : Control
    {
        public readonly ButtonBase self = new ButtonBase();
        public override object GtkControl => self;
        public Button() : base()
        {
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
        
        public override event EventHandler Click
        {
            add { self.Clicked += value; }
            remove { self.Clicked -= value; }
        }
    }
}
