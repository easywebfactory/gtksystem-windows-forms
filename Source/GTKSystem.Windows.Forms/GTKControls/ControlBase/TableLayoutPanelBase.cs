using Cairo;
using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class TableLayoutPanelBase : Gtk.Grid, IGtkControl
{
    public IGtkControlOverride Override { get; set; }
    public TableLayoutPanelBase()
    {
        Override = new GtkFormsControlOverride(this);
        Override.AddClass("TableLayoutPanel");
        RowHomogeneous = false;
        ColumnHomogeneous = false;
        BorderWidth = 1;
        BaselineRow = 0;
        ColumnSpacing = 0;
        RowSpacing = 0;
        Halign = Gtk.Align.Start;
        Valign = Gtk.Align.Start;
        Vexpand = false;
        Hexpand = false;
    }
    public void AddClass(string cssClass)
    {
        Override.AddClass(cssClass);
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