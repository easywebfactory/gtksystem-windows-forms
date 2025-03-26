/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using System.ComponentModel;

namespace System.Windows.Forms;

[DesignerCategory("UserControl")]
[ToolboxItem(true)]
public class UserControl : ContainerControl
{
    public readonly UserControlBase self = new();
    public override object GtkControl => self;
    private readonly Overlay? contaner;
    private readonly ControlCollection _controls = null!;

    public UserControl()
    {
        contaner = new Overlay();
        contaner.MarginStart = 0;
        contaner.MarginTop = 0;
        contaner.BorderWidth = 0;
        contaner.Halign = Align.Fill;
        contaner.Valign = Align.Fill;
        contaner.Hexpand = false;
        contaner.Vexpand = false;
        contaner.Add(new Fixed { Halign = Align.Fill, Valign = Align.Fill });
        _controls = new ControlCollection(this, contaner);
        self.Add(contaner);
        //self.Override.Paint += Override_Paint;
        self.ParentSet += Self_ParentSet;
    }
    public override Padding Padding
    {
        get => base.Padding;
        set
        {
            base.Padding = value;
            if (contaner != null)
            {
                contaner.MarginStart = value.Left;
                contaner.MarginTop = value.Top;
                contaner.MarginEnd = value.Right;
                contaner.MarginBottom = value.Bottom;
            }
        }
    }
    private void Self_ParentSet(object? o, ParentSetArgs args)
    {
        OnParentChanged(EventArgs.Empty);
    }

    private void Override_Paint(object? sender, PaintEventArgs e)
    {
        OnPaint(e);
    }

    public Drawing.SizeF AutoScaleDimensions { get; set; }
    public AutoScaleMode AutoScaleMode { get; set; }
    public override ControlCollection Controls => _controls;

    public override void SuspendLayout()
    {

    }
    public override void ResumeLayout(bool performLayout)
    {

    }
}