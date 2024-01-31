/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System;
using System.Drawing;

namespace System.Windows.Forms
{

    public class ToolStripTextBox : WidgetToolStrip<Gtk.MenuItem>
    {
        public ToolStripTextBox() : base("ToolStripTextBox")
        {
            
        }
        public override Size Size { get => base.Size; set { this.entry.WidthRequest = value.Width;this.entry.HeightRequest = value.Height; base.Size = value; } }
        public int MaxLength { get => this.entry.MaxLength; set => this.entry.MaxLength = value; }
        public override string Text { get => this.entry.Text; set => this.entry.Text = value; }
    }

}
