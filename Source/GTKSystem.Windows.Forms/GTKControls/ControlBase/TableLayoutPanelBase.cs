using Cairo;
using Gtk;

namespace System.Windows.Forms;

public sealed class TableLayoutPanelBase : Grid, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public Grid grid = new();
    public TableLayoutPanelBase() : base()
    {
        Override = new GtkControlOverride(this);
        Override.AddClass("TableLayoutPanel");
        grid.RowHomogeneous = false;
        grid.ColumnHomogeneous = false;
        grid.BaselineRow = 0;
        grid.ColumnSpacing = 0;
        grid.RowSpacing = 0;
        grid.BorderWidth = 0;
        grid.Halign = Align.Fill;
        grid.Valign = Align.Fill;
        BorderWidth = 0;
        Vexpand = false;
        Hexpand = false;
        HscrollbarPolicy = PolicyType.External;
        VscrollbarPolicy = PolicyType.External;

        Add(grid);
    }

    public PolicyType VscrollbarPolicy { get; set; }

    public PolicyType HscrollbarPolicy { get; set; }

    public void AddClass(string cssClass)
    {
        Override.AddClass(cssClass);
    }
    protected override void OnShown()
    {
        Override.OnAddClass();
        base.OnShown();
    }
    protected override bool OnDrawn(Context cr)
    {
        var rec = new Gdk.Rectangle(0, 0, AllocatedWidth, AllocatedHeight);
        Override.OnPaint(cr, rec);
        return base.OnDrawn(cr);
    }
}