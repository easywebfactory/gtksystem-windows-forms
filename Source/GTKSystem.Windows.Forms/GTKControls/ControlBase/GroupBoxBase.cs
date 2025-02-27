using Cairo;
using Gtk;
using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class GroupBoxBase : Frame, IGtkControl, IScrollableBoxBase
{
    public IGtkControlOverride Override { get; set; }
    public bool AutoScroll { get; set; }
    public bool HScroll { get; set; } = false;
    public bool VScroll { get; set; } = false;

    public GroupBoxBase()
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("GroupBox");
        LabelXalign = 0.03f;
        Valign = Align.Start;
        Halign = Align.Start;
    }

    public event ScrollEventHandler? Scroll;

    protected override void OnShown()
    {
        Override.OnAddClass();
        base.OnShown();
    }
    protected override bool OnDrawn(Context? cr)
    {
        var rec = new Gdk.Rectangle(0, 0, AllocatedWidth, AllocatedHeight);
        Override.OnPaint(cr, rec);
        return base.OnDrawn(cr);
    }

    public void AddClass(string cssClass)
    {
        Override.AddClass(cssClass);
    }

    public void Pack(Widget child, Align align, bool expand)
    {
        child.Valign = align;
        child.Halign = align;
        child.Expand = expand;
        Add(child);
    }
}