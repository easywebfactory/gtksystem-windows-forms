/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Xml.Linq;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Panel : ScrollableControl
    {
        public readonly PanelBase self = new PanelBase();
        public override object GtkControl => self;
        private ControlCollection _controls;
        public Panel() : base()
        {
            self.Override.sender = this;
            _controls = new ControlCollection(this, self.contaner);
            this.AutoScroll = false;
        }
        internal Panel(string type) : base()
        {
            self.Override.sender = this;
            self.StyleContext.AddClass(type);
            this.AutoScroll = false;
        }
        public override ControlCollection Controls => _controls;
        public override Padding Padding
        {
            get => base.Padding;
            set
            {
                base.Padding = value;
                self.contaner.MarginStart = value.Left;
                self.contaner.MarginTop = value.Top;
                self.contaner.MarginEnd = value.Right;
                self.contaner.MarginBottom = value.Bottom;
            }
        }
    }
}
