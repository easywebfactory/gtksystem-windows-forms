/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
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
