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
public class SplitContainer : ContainerControl
{
    public readonly SplitContainerBase self = new();
    public override object GtkControl => self;
    public SplitContainer()
    {
        _panel1 = new SplitterPanel(this);
        _panel1.contaner.Name = "Child1";
        _panel2 = new SplitterPanel(this);
        _panel2.contaner.Name = "Child2";
        if (_panel1.Widget is Widget panel1Widget) self.Pack1(panel1Widget, false, true);
        if (_panel2.Widget is Widget panel2Widget) self.Pack2(panel2Widget, true, true);
    }
    private SplitterPanel _panel1;
    private SplitterPanel _panel2;
    public SplitterPanel Panel1
    {
        get => _panel1;
        set => _panel1 = value;
    }
    public SplitterPanel Panel2 {
        get => _panel2;
        set => _panel2 = value;
    }
    public int splitterDistance;
    public int SplitterDistance { get => self.Position + 5; set { splitterDistance = Math.Max(1, value - 5); self.Position = splitterDistance; } }
    private int splitterWidth;
    public int SplitterWidth { get => splitterWidth;
        set { splitterWidth = value; self.WideHandle = value > 2; } }
    public int SplitterIncrement { get; set; }
    public Orientation Orientation {
        get => self.Orientation == Gtk.Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
        set => self.Orientation = value == Orientation.Horizontal ? Gtk.Orientation.Vertical : Gtk.Orientation.Horizontal;
    }
    private FixedPanel _fixedPanel = FixedPanel.Panel1;
    public FixedPanel FixedPanel {
        get => _fixedPanel;
        set {
            _fixedPanel = value;
            var resize = value == FixedPanel.Panel2;
            if (_panel1.Widget is Widget panel1Widget) ((Paned.PanedChild)self[panel1Widget]).Resize = resize;
            if (_panel2.Widget is Widget panel2Widget) ((Paned.PanedChild)self[panel2Widget]).Resize = !resize;
        } 
    }
}