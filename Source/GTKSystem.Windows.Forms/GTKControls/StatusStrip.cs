/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class StatusStrip : ToolStrip
    {
        public StatusStrip():base()
        {
            this.self.StyleContext.RemoveClass("ToolStrip");
            this.self.StyleContext.AddClass("StatusStrip");
            Dock = DockStyle.Bottom;
        }
        public override Point Location { get => base.Location; set => base.Location = new Point(value.X, value.Y - 12); }
        public override Size Size { get => base.Size; set => base.Size = new Size(value.Width, value.Height + 12); }
        [DefaultValue(false)]
        public bool ShowItemToolTips { get; set; }

        [DefaultValue(true)]
		public bool SizingGrip { get; set; }

        [DefaultValue(true)]
		public bool Stretch { get; set; }
    }
}
