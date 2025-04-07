using Cairo;
using Gtk;

namespace System.Windows.Forms;

public abstract class ScrollableBoxBase : ScrolledWindow, IControlGtk, IScrollableBoxBase
{
    public event ScrollEventHandler? Scroll;
    public IGtkControlOverride Override { get; set; }
    public ScrollableBoxBase()
    {
        Override = new GtkFormsControlOverride(this);
        ShadowType = ShadowType.None;
        BorderWidth = 1;
        Events = Gdk.EventMask.AllEventsMask;
        Halign = Align.Start;
        Valign = Align.Start;
        Hexpand = false;
        Vexpand = false;
        VscrollbarPolicy = PolicyType.External;
        HscrollbarPolicy = PolicyType.External;
        OverlayScrolling = false;
        Hadjustment.ValueChanged += Hadjustment_ValueChanged;
        Vadjustment.ValueChanged += Vadjustment_ValueChanged;
    }
 
    private void Vadjustment_ValueChanged(object? sender, EventArgs e)
    {
        if (Scroll != null)
        {
            var adj = (Adjustment?)sender;
            OnScroll(new ScrollEventArgs(ScrollEventType.ThumbTrack, (int)(adj?.Value > adj?.StepIncrement ? adj.Value - adj.StepIncrement : adj?.Value??0), (int)(adj?.Value??0), ScrollOrientation.VerticalScroll));
        }
    }

    protected virtual void OnScroll(ScrollEventArgs e)
    {
        Scroll?.Invoke(this, e);
    }

    private void Hadjustment_ValueChanged(object? sender, EventArgs e)
    {
        if (Scroll != null)
        {
            var adj = (Adjustment?)sender;
            OnScroll(new ScrollEventArgs(ScrollEventType.ThumbTrack, (int)(adj?.Value > adj?.StepIncrement ? adj.Value - adj.StepIncrement : adj?.Value??0), (int)(adj?.Value??0), ScrollOrientation.HorizontalScroll));
        }
    }

    public void AddClass(string cssClass)
    {
        Override.AddClass(cssClass);
    }
    public bool VScroll { get; set; } = true;
    public bool HScroll { get; set; } = true;
    public virtual bool AutoScroll
    {
        get => VscrollbarPolicy == PolicyType.Automatic;
        set
        {
            if (value)
            {
                if (VScroll)
                    VscrollbarPolicy = PolicyType.Automatic;
                if (HScroll)
                    HscrollbarPolicy = PolicyType.Automatic;
            }
            else
            {
                VscrollbarPolicy = PolicyType.External;
                HscrollbarPolicy = PolicyType.External;
            }
        }
    }
    protected override void OnShown()
    {
        Override.OnAddClass();
        base.OnShown();
    }
    protected virtual Gdk.Rectangle GetDrawRectangle()
    {
        return new Gdk.Rectangle(0, 0, AllocatedWidth, AllocatedHeight);
    }
    protected override bool OnDrawn(Context? cr)
    {
        var rec = GetDrawRectangle();
        Override.OnPaint(cr, rec);
        return base.OnDrawn(cr);
    }
}