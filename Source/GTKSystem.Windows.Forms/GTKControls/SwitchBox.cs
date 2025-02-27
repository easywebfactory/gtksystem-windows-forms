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
    public partial class SwitchBox : Control
    {
        public readonly SwitchBoxBase self = new SwitchBoxBase();
        public override object GtkControl => self;
        public SwitchBox() {
            self.ButtonReleaseEvent += Self_ButtonReleaseEvent;
        }
        private void Self_ButtonReleaseEvent(object o, Gtk.ButtonReleaseEventArgs args)
        {
            if (CheckedChanged != null && self.IsVisible)
                CheckedChanged(this, EventArgs.Empty);
        }
        public override string Text { get { return self.TooltipText; } set { self.TooltipText = value; } }
        public  bool Checked { get { return self.Active; } 
            set { 
                self.Active = value;
                if (CheckedChanged != null && self.IsVisible)
                    CheckedChanged(this, EventArgs.Empty);
            } 
        }
        public event EventHandler CheckedChanged;
    }
}
