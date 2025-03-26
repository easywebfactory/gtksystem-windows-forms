/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.Drawing;

namespace System.Windows.Forms;

public class ToolStripTextBox : WidgetToolStrip<Gtk.MenuItem>
{
    public ToolStripTextBox() : base("ToolStripTextBox", [])
    {
            
    }
    public override Size Size { get => base.Size; set { entry.WidthRequest = value.Width;entry.HeightRequest = value.Height; base.Size = value; } }
    public int MaxLength { get => entry.MaxLength; set => entry.MaxLength = value; }
    public override string Text { get => entry.Text; set => entry.Text = value; }
}