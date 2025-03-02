/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
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
    private readonly ControlCollection? _controls;

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
        self.Override.Paint += Override_Paint;
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

    public override event EventHandler? Load;
    public Drawing.SizeF AutoScaleDimensions { get; set; }
    public AutoScaleMode AutoScaleMode { get; set; }
    public override ControlCollection? Controls => _controls;

    protected override void OnPaint(PaintEventArgs e)
    {
    }
    protected override void OnParentChanged(EventArgs e)
    {
    }
    public override void SuspendLayout()
    {

    }
    public override void ResumeLayout(bool performLayout)
    {

    }
}