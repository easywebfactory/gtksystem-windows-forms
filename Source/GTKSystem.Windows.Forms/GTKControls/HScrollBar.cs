/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;
using Gtk;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class HScrollBar : ScrollBar
{
    public ScrollbarBase<HScrollbar>? self = new(Gtk.Orientation.Horizontal);
    public override object? GtkControl => self;
    public override Gtk.Adjustment Adjustment => self.Adjustment;
}