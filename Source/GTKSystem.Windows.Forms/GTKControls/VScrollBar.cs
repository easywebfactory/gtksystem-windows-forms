/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public class VScrollBar : ScrollBar
    {
        public ScrollbarBase<Gtk.VScrollbar> self=new ScrollbarBase<Gtk.VScrollbar>(Gtk.Orientation.Vertical);
        public override object GtkControl => self;
        public override Gtk.Adjustment Adjustment { get => self.Adjustment; }
        public VScrollBar() : base()
        {

        }
    }
}
