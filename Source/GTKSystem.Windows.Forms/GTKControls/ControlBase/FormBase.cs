using Gtk;
using GtkSystem.Resources.Extensions;

namespace System.Windows.Forms;

public sealed class FormBase : Dialog, IControlGtk, IScrollableBoxBase, IWin32Window
{
    public new Window? Parent { get; set; }
    public readonly ScrolledWindow ScrollView = new();
    public IGtkControlOverride Override { get; set; }
    public bool AutoScroll
    {
        get => ScrollView.VscrollbarPolicy == PolicyType.Automatic;
        set
        {
            if (value)
            {
                if (VScroll)
                    ScrollView.VscrollbarPolicy = PolicyType.Automatic;
                if (HScroll)
                    ScrollView.HscrollbarPolicy = PolicyType.Automatic;
            }
            else
            {
                ScrollView.VscrollbarPolicy = PolicyType.External;
                ScrollView.HscrollbarPolicy = PolicyType.External;
            }
        }
    }

    public bool VScroll { get; set; } = true;
    public bool HScroll { get; set; } = true;

        public delegate bool CloseWindowHandler(object? sender, EventArgs e);
        public event CloseWindowHandler? CloseWindowEvent;
        public event ScrollEventHandler? Scroll;
        public FormBase(Window? parent = null) : base("title", ListToplevels().LastOrDefault(o => o is FormBase && o.IsActive), DialogFlags.UseHeaderBar)
        {
            Override = new GtkControlOverride(this);
            Override.AddClass("Form");
            WindowPosition = WindowPosition.Center;
            BorderWidth = 0;
            ContentArea.BorderWidth = 0;
            ContentArea.Spacing = 0;
            ContentArea.Homogeneous = false;

        SetDefaultSize(100, 100);
        TypeHint = Gdk.WindowTypeHint.Normal;
        AppPaintable = false;
        Deletable = true;
        Response += FormBase_Response;
        ScrollView.BorderWidth = 0;
        ScrollView.Valign = Align.Fill;
        ScrollView.Halign = Align.Fill;
        ScrollView.Hexpand = true;
        ScrollView.Vexpand = true;
        ScrollView.OverlayScrolling = true;
        ScrollView.KineticScrolling = true;
        ScrollView.HscrollbarPolicy = PolicyType.Automatic;
        ScrollView.VscrollbarPolicy = PolicyType.Automatic;
        ScrollView.Hadjustment.ValueChanged += Hadjustment_ValueChanged;
        ScrollView.Vadjustment.ValueChanged += Vadjustment_ValueChanged;
        ContentArea.PackStart(ScrollView, true, true, 0);
        //this.Decorated = false; // Delete toolbar
        Drawn += FormBase_Drawn;
        Close += FormBase_Close;
    }

    private void FormBase_Close(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(this, SystemResources.FormBase_FormBase_Close_1, SystemResources.FormBase_FormBase_Close_2, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            Respond(ResponseType.DeleteEvent);
        }
    }

    private void FormBase_Drawn(object? o, DrawnArgs args)
    {
        var rec = new Gdk.Rectangle(0, 0, AllocatedWidth, AllocatedHeight);
        Override.OnPaint(args.Cr, rec);
    }

        private void FormBase_Response(object o, ResponseArgs args)
        {
            if (args.ResponseId == ResponseType.DeleteEvent)
            {
                if (CloseWindowEvent(this, EventArgs.Empty))
                {
                    OnClose();
                    Group.CurrentGrab?.Destroy();
                    Destroy();
                }
                else
                    Run();
            }
        }
        private void Vadjustment_ValueChanged(object? sender, EventArgs e)
        {
            if (Scroll != null)
            {
                var adj = (Adjustment?)sender;
                Scroll(this, new ScrollEventArgs(ScrollEventType.ThumbTrack, (int)(adj.Value > adj.StepIncrement ? (adj.Value - adj.StepIncrement) : adj.Value), (int)adj.Value, ScrollOrientation.VerticalScroll));
            }
        }

    private void Hadjustment_ValueChanged(object? sender, EventArgs e)
    {
        if (Scroll != null)
        {
            var adj = (Adjustment?)sender;
            OnScroll(new ScrollEventArgs(ScrollEventType.ThumbTrack, (int)(adj?.Value > adj?.StepIncrement ? adj.Value - adj.StepIncrement : adj?.Value??0), (int)(adj?.Value??0), ScrollOrientation.HorizontalScroll));
        }
    }

    private void OnScroll(ScrollEventArgs e)
    {
        Scroll?.Invoke(this, e);
    }

    public void CloseWindow()
    {
        Respond(ResponseType.DeleteEvent);
    }

    public void AddClass(string cssClass)
    {
        Override.AddClass(cssClass);
    }
    public new void Add(Widget child)
    {
        ScrollView.Child = child;
    }
}