using Cairo;
using Gtk;

namespace System.Windows.Forms;

public sealed class TabPageBase : ScrollableBoxBase
{
    public Overlay? content = new();
    public TabPageBase()
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("TabPage");
        BorderWidth = 0;
        content!.Margin = 0;
        content.Halign = Align.Fill;
        content.Valign = Align.Fill;
        content.Expand = false;
        content.Add(new Fixed { Halign = Align.Fill, Valign = Align.Fill });
        Halign = Align.Fill;
        Valign = Align.Fill;
        Add(content);
    }
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
}