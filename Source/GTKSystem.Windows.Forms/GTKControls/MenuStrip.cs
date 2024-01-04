/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class MenuStrip : ToolStrip
    {
        public MenuStrip() : base()
        {
            Widget.StyleContext.AddClass("MenuStrip");
            base.Control.PackDirection = Gtk.PackDirection.Ltr;
            this.Dock = DockStyle.Top;
        }

    }
}
