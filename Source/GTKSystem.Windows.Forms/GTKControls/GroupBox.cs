/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
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
    private readonly ControlCollection? _controls;
    public GroupBox()
    {
        _controls = new ControlCollection(this, contaner);
        _controls.offset.Offset(0, -20);
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
    public override string? Text { get => self.Label;
        set => self.Label = value;
    }
    public override ControlCollection? Controls => _controls;
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