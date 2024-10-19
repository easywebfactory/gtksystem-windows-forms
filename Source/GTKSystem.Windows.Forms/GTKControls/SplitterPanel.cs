/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;


namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public sealed class SplitterPanel2 : Panel
    {
        internal SplitContainer Owner;
        public SplitterPanel2(SplitContainer owner) : base()
        {
            Owner = owner;
            self.BorderWidth = 1;
            self.ShadowType = Gtk.ShadowType.In;
            self.Margin = 0;
            self.Halign = Gtk.Align.Fill;
            self.Valign = Gtk.Align.Fill;
            self.Halign = Gtk.Align.Fill;
            self.Valign = Gtk.Align.Fill;
            self.Hexpand = false;
            self.Vexpand = false;
            this.contaner.Halign = Gtk.Align.Fill;
            this.contaner.Valign = Gtk.Align.Fill;
        }

        public override DockStyle Dock { get; set; } = DockStyle.Fill;
        public override Size Size { get; set; }

    }

    public sealed class SplitterPanel : ScrollableControl
    {
        public readonly ViewportBase self = new ViewportBase();
        public override object GtkControl => self;
        public Gtk.Overlay contaner = new Gtk.Overlay();
        private ControlCollection _controls;
        public SplitterPanel(SplitContainer owner) : base()
        {
            _controls = new ControlCollection(this, contaner);
            contaner.MarginStart = 0;
            contaner.MarginTop = 0;
            contaner.Halign = Align.Fill;
            contaner.Valign = Align.Fill;
            contaner.Expand = false;
            contaner.Expand = false;
            contaner.BorderWidth = 0;
            self.Halign = Align.Fill;
            self.Valign = Align.Fill;
            self.Add(contaner);
        }

        public override ControlCollection Controls => _controls;
        internal SplitContainer Owner;

        public override DockStyle Dock { get; set; } = DockStyle.Fill;
        public override Size Size { get; set; }

    }
}
