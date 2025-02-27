using Cairo;
using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public abstract class ScrollableBoxBase : Gtk.ScrolledWindow, IGtkControl, IScrollableBoxBase
{
    public event ScrollEventHandler? Scroll;
    public IGtkControlOverride Override { get; set; }
    public ScrollableBoxBase()
    {
        Override = new GtkFormsControlOverride(this);
        ShadowType = Gtk.ShadowType.None;
        BorderWidth = 1;
        Events = Gdk.EventMask.AllEventsMask;
        Halign = Gtk.Align.Start;
        Valign = Gtk.Align.Start;
        Hexpand = false;
        Vexpand = false;
        VscrollbarPolicy = Gtk.PolicyType.Never;
        HscrollbarPolicy = Gtk.PolicyType.Never;
        OverlayScrolling = false;
        Hadjustment.ValueChanged += Hadjustment_ValueChanged;
        Vadjustment.ValueChanged += Vadjustment_ValueChanged;
    }
 
    private void Vadjustment_ValueChanged(object? sender, EventArgs e)
    {
        if (Scroll != null)
        {
            var adj = (Gtk.Adjustment?)sender;
            Scroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.ThumbTrack, (int)(adj?.Value > adj?.StepIncrement ? adj.Value - adj.StepIncrement : adj?.Value??0), (int)(adj?.Value??0), ScrollOrientation.VerticalScroll));
        }
    }

    private void Hadjustment_ValueChanged(object? sender, EventArgs e)
    {
        if (Scroll != null)
        {
            var adj = (Gtk.Adjustment?)sender;
            Scroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.ThumbTrack, (int)(adj?.Value > adj?.StepIncrement ? adj.Value - adj.StepIncrement : adj?.Value??0), (int)(adj?.Value??0), ScrollOrientation.HorizontalScroll));
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
        get => VscrollbarPolicy == Gtk.PolicyType.Automatic;
        set
        {
            if (value)
            {
                if (VScroll)
                    VscrollbarPolicy = Gtk.PolicyType.Automatic;
                if (HScroll)
                    HscrollbarPolicy = Gtk.PolicyType.Automatic;
            }
            else
            {
                VscrollbarPolicy = Gtk.PolicyType.Never;
                HscrollbarPolicy = Gtk.PolicyType.Never;
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