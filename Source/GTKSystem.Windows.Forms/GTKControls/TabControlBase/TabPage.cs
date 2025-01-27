/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Collections.Generic;
using System.Drawing;

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
        public override Padding Padding
        {
            get => base.Padding;
            set
            {
                base.Padding = value;
                self.Content.MarginStart = value.Left;
                self.Content.MarginTop = value.Top;
                self.Content.MarginEnd = value.Right;
                self.Content.MarginBottom = value.Bottom;
            }
        }
    }
}
