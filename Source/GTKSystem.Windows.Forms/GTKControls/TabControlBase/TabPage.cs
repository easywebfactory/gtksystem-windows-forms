/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms
{
    public class TabPage : ContainerControl
    {
        public readonly TabPageBase self = new TabPageBase();
        public override object GtkControl => self;
        internal Gtk.Label _tabLabel = new Gtk.Label();
        private ControlCollection _controls;
        public TabPage() : base()
        {
            _controls = new ControlCollection(this, self.Content);
            this.Dock = DockStyle.Fill;
        }

        public TabPage(string text):this()
        {
            _tabLabel.Text = text;
        }

        public override Point Location
        {
            get
            {
                return new Point(self.MarginStart, self.MarginTop);
            }
            set
            {
                self.MarginStart = 0;
                self.MarginTop = 0;
            }
        }
        public new DockStyle Dock
        {
            get
            {
                return DockStyle.Fill;
            }
            set { base.Dock = DockStyle.Fill; }
        }
        public override string Text { get { return _tabLabel.Text; } set { _tabLabel.Text = value; } }
        public Gtk.Label TabLabel { get { return _tabLabel; } }

        public new ControlCollection Controls => _controls;

        public int ImageIndex { get; set; }
        public string ImageKey { get; set; }
        public List<object> ImageList { get; set; }
    }
}
