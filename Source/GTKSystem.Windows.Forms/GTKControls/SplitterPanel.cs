/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://github.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;
using System.Drawing;


namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public sealed class SplitterPanel : Panel
    {
        internal SplitContainer Owner;
        public SplitterPanel(SplitContainer owner) : base()
        {
            Owner = owner;
            self.BorderWidth = 1;
            self.ShadowType = Gtk.ShadowType.In;
            self.Margin = 0;
            this.Dock = DockStyle.Fill;
        }

        public override DockStyle Dock { get; set; } = DockStyle.Fill;
        public override Size Size { get; set; }

    }
}
