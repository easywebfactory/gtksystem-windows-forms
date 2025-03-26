/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using System.ComponentModel;

namespace System.Windows.Forms;

[ProvideProperty("FlowBreak", typeof(Control))]
[DefaultProperty("FlowDirection")]
[DesignerCategory("Component")]
public class FlowLayoutPanel : Control, IExtenderProvider
{
    public readonly FlowLayoutPanelBase self = new();
    public override object GtkControl => self;
    private readonly ObjectCollection? _controls;
    public FlowLayoutPanel()
    {
        self.Orientation = Gtk.Orientation.Horizontal;
        self.Halign = Align.Start;
        self.Valign = Align.Start;
        self.MinChildrenPerLine = 1;
        self.MaxChildrenPerLine = 999;
        self.ColumnSpacing = 0;
        self.BorderWidth = 0;
        _controls = new ObjectCollection(this);
    }

    private readonly FlowDirection flowDirection = default;
    public FlowDirection FlowDirection
    {
        get => flowDirection;
        set
        {
            if (value == FlowDirection.LeftToRight || value == FlowDirection.RightToLeft) { self.Orientation = Gtk.Orientation.Horizontal; }
            else if (value == FlowDirection.TopDown || value == FlowDirection.BottomUp) { self.Orientation = Gtk.Orientation.Vertical; }
        }
    }

    public bool WrapContents { get; set; }

    public bool GetFlowBreak(Control control)
    {
        return false;
    }

    public void SetFlowBreak(Control control, bool value)
    {

    }
    public override ControlCollection Controls => _controls!;
    public bool CanExtend(object? extendee)
    {
        return true;
    }
    public class ObjectCollection : ControlCollection
    {
        private readonly FlowLayoutPanel? _owner;
        public ObjectCollection(FlowLayoutPanel? owner) : base(owner)
        {
            _owner = owner;
        }
        public override void Add(Control? control)
        {
            var box = new FlowBoxChild();
            box.Valign = Align.Start;
            box.Halign = Align.Start;
            box.Expand = false;
            if (control != null)
            {
                control.Location = new Drawing.Point(0, 0);
                control.LockLocation = true;
                control.Parent = _owner;
                var widg = control.Widget;
                widg.Valign = Align.Start;
                widg.Halign = Align.Start;
                widg.Expand = false;
                if (widg is Widget widget) box.Add(widget);
                _owner?.self.Add(box);
                AddWidget(box, control);
            }
        }
    }
}