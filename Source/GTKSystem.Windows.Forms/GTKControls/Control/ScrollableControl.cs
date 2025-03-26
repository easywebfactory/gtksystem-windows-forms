using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;

[Designer("System.Windows.Forms.Design.ScrollableControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public class ScrollableControl : Control
{

    protected const int scrollStateAutoScrolling = 1;

    protected const int scrollStateHScrollVisible = 2;

    protected const int scrollStateVScrollVisible = 4;

    protected const int scrollStateUserHasScrolled = 8;

    protected const int scrollStateFullDrag = 16;

    private IScrollableBoxBase? scrollbase;
    public ScrollableControl()
    {
        scrollbase = GtkControl as IScrollableBoxBase;
    }
    public void SetScrolledWindow(IScrollableBoxBase? scrolledwindow)
    {
        scrollbase = scrolledwindow;
    }
    public override Rectangle DisplayRectangle { get; set; }
    public Size AutoScrollMinSize { get; set; }
    public Point AutoScrollPosition { get; set; }
    public Size AutoScrollMargin { get; set; }
    private bool autoScroll;
    public virtual bool AutoScroll
    {
        get => autoScroll;
        set
        {
            autoScroll = value;
            if (scrollbase != null) { scrollbase.AutoScroll = value; }
        }
    }
    //public VScrollProperties VerticalScroll { get; }
    //public DockPaddingEdges DockPadding { get; }
    //public HScrollProperties HorizontalScroll { get; }
    protected bool VScroll
    {
        get => scrollbase?.VScroll ?? false;
        set
        {
            if (scrollbase != null)
            {
                scrollbase.VScroll = value;
            }
        }
    }

    protected bool HScroll
    {
        get => scrollbase?.HScroll??false;
        set
        {
            if (scrollbase != null)
            {
                scrollbase.HScroll = value;
            }
        }
    }

    public virtual event ScrollEventHandler? Scroll
    {
        add { if (scrollbase != null) { scrollbase.Scroll += value; } }
        remove { if (scrollbase != null) { scrollbase.Scroll -= value; } }
    }
}