/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.Drawing;

namespace System.Windows.Forms;

public class TabPage : ContainerControl
{
    public readonly TabPageBase self = new();
    public override object GtkControl => self;
    internal Gtk.Label _tabLabel = new();
    private readonly ControlCollection _controls;
    public TabPage()
    {
        _controls = new ControlCollection(this, self.content);
        Dock = DockStyle.Fill;
    }

    public TabPage(string text):this()
    {
        _tabLabel.Text = text;
    }

    public override Point Location
    {
        get => new(0, 0);
        set
        {
        }
    }
    public new DockStyle Dock
    {
        get => DockStyle.Fill;
        set => base.Dock = DockStyle.Fill;
    }
    public override string Text { get => _tabLabel.Text;
        set => _tabLabel.Text = value;
    }
    public Gtk.Label TabLabel => _tabLabel;

    public new ControlCollection Controls => _controls;

    public int ImageIndex { get; set; }
    public string? ImageKey { get; set; }
    public List<object>? ImageList { get; set; }
    public override Padding Padding
    {
        get => base.Padding;
        set
        {
            base.Padding = value;
            if (self.content != null)
            {
                self.content.MarginStart = value.Left;
                self.content.MarginTop = value.Top;
                self.content.MarginEnd = value.Right;
                self.content.MarginBottom = value.Bottom;
            }
        }
    }
    private Size _size;
    public override Size Size { get => _size; set => _size = value;
    }
}