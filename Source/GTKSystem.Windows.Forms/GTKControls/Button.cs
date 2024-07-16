/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://github.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 */

using Gdk;
using GLib;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class Button : Control
    {
        public readonly ButtonBase self = new ButtonBase();
        public override object GtkControl => self;
        public Button() : base()
        {
            self.Clicked += Self_Clicked;
        }

        private void Self_Clicked(object sender, EventArgs e)
        {
            if(Click!= null && self.IsVisible) { Click(this, EventArgs.Empty); }
        }

        public override string Text { get => self.Label; set => self.Label = value; }

        public override event EventHandler Click;
    }
}
