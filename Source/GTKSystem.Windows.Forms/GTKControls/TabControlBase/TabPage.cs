/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms
{
    public class TabPage : WidgetControl<Gtk.Layout>
    {
        internal Gtk.Label _tabLabel = new Gtk.Label();
        private ControlCollection _controls;
        public TabPage() : base(new Gtk.Adjustment(IntPtr.Zero), new Gtk.Adjustment(IntPtr.Zero))
        {
            Widget.StyleContext.AddClass("TabPage");
            Control.BorderWidth = 0;
            _controls = new ControlCollection(this, this.Control);

            Widget.Data["Dock"] = DockStyle.Fill;
        }

        public TabPage(string text)
        {
            _tabLabel.Text = text;
            _controls = new ControlCollection(this, this.Control);
        }

        public override Point Location
        {
            get
            {
                return new Point(Widget.MarginStart, Widget.MarginTop);
            }
            set
            {
                Widget.MarginStart = 0;
                Widget.MarginTop = 0;
                Widget.Data["InitMarginStart"] = 0;
                Widget.Data["InitMarginTop"] = 0;
            }
        }
        public new DockStyle Dock
        {
            get
            {
                return DockStyle.Fill;
            }
            set { Widget.Data["Dock"] = DockStyle.Fill; }
        }
        public override string Text { get { return _tabLabel.Text; } set { _tabLabel.Text = value; } }
        public Gtk.Label TabLabel { get { return _tabLabel; } }

        public new ControlCollection Controls => _controls;

        public int ImageIndex { get; set; }
        public string ImageKey { get; set; }
        public List<object> ImageList { get; set; }
    }
}
