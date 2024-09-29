/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
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
