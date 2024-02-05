/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class MenuStrip : ToolStrip
    {
        public MenuStrip() : base("MenuStrip")
        {
            Widget.StyleContext.RemoveClass("ToolStrip");
            Widget.StyleContext.AddClass("MenuStrip");
            this.Dock = DockStyle.Top;
        }

    }
}
