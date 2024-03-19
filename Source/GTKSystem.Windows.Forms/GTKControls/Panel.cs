/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using GLib;
using Gtk;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Panel : WidgetContainerControl<Gtk.Viewport>
    {
        private Gtk.Fixed contaner = new Gtk.Fixed();
        private Gtk.ScrolledWindow scrolledwindow = new Gtk.ScrolledWindow();
        private ControlCollection _controls;

        public Panel() : base()
        {
            Widget.StyleContext.AddClass("Panel");
            base.Control.MarginStart = 0;
            base.Control.MarginTop = 0;
            base.Control.ShadowType = Gtk.ShadowType.In;
            base.Control.BorderWidth = 0;
            _controls = new ControlCollection(this, contaner);

            contaner.MarginStart = 0;
            contaner.MarginTop = 0;
            contaner.Halign = Align.Fill;
            contaner.Valign = Align.Fill;

            scrolledwindow.Halign = Align.Fill;
            scrolledwindow.Valign = Align.Fill;
            scrolledwindow.VscrollbarPolicy = PolicyType.Never;
            scrolledwindow.HscrollbarPolicy = PolicyType.Never;
            scrolledwindow.Child = contaner;
            base.Control.Child = scrolledwindow;
        }
        public override BorderStyle BorderStyle { get { return base.Control.ShadowType == Gtk.ShadowType.None ? BorderStyle.None : BorderStyle.FixedSingle; } set { base.Control.BorderWidth = 1; base.Control.ShadowType = Gtk.ShadowType.In; } }
        public override ControlCollection Controls => _controls;
        public override bool AutoScroll { 
            get => base.AutoScroll; 
            set { 
                base.AutoScroll = value;
                if (value == true)
                {
                    scrolledwindow.VscrollbarPolicy = PolicyType.Automatic;
                    scrolledwindow.HscrollbarPolicy = PolicyType.Automatic;
                }
            } 
        }
    }
}
