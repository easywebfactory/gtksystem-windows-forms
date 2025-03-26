/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using System.ComponentModel;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class GroupBox : ContainerControl
{
    public readonly GroupBoxBase self = new();
    public override object GtkControl => self;
    private readonly Overlay? contaner = new();
    private readonly ControlCollection _controls = null!;
    public GroupBox()
    {
        _controls = new ControlCollection(this, contaner);
        _controls.Offset.Offset(0, -20);
        if (contaner != null)
        {
            contaner.MarginStart = 0;
            contaner.MarginTop = 0;
            contaner.Halign = Align.Fill;
            contaner.Valign = Align.Fill;
            contaner.Add(new Fixed { Halign = Align.Fill, Valign = Align.Fill });
            self.Child = contaner;
        }
    }
    public override string Text { get => self.Label;
        set => self.Label = value;
    }
    public override ControlCollection Controls => _controls;
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
    public override void SuspendLayout()
    {
        created = false;
    }
    public override void ResumeLayout(bool resume)
    {
        created = resume == false;
    }

    public override void PerformLayout()
    {
        created = true;
    }

}