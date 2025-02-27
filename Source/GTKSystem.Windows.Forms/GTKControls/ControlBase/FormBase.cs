using Gtk;
using GTKSystem.Windows.Forms.GTKControls;

namespace System.Windows.Forms;

public sealed class FormBase : Dialog, IGtkControl, IScrollableBoxBase, IWin32Window
{
    public new Window? Parent { get; }
    public readonly ScrolledWindow scrollView = new();
    public IGtkControlOverride Override { get; set; }
    public bool AutoScroll
    {
        get => scrollView.VscrollbarPolicy == PolicyType.Automatic;
        set
        {
            if (value)
            {
                if (VScroll)
                    scrollView.VscrollbarPolicy = PolicyType.Automatic;
                if (HScroll)
                    scrollView.HscrollbarPolicy = PolicyType.Automatic;
            }
            else
            {
                scrollView.VscrollbarPolicy = PolicyType.Never;
                scrollView.HscrollbarPolicy = PolicyType.Never;
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
        Parent = parent;
        DestroyWithParent = true;
        Override = new GtkFormsControlOverride(this);
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
        scrollView.BorderWidth = 0;
        scrollView.Valign = Align.Fill;
        scrollView.Halign = Align.Fill;
        scrollView.Hexpand = true;
        scrollView.Vexpand = true;
        scrollView.OverlayScrolling = true;
        scrollView.KineticScrolling = true;
        scrollView.HscrollbarPolicy = PolicyType.Automatic;
        scrollView.VscrollbarPolicy = PolicyType.Automatic;
        scrollView.Hadjustment.ValueChanged += Hadjustment_ValueChanged;
        scrollView.Vadjustment.ValueChanged += Vadjustment_ValueChanged;
        ContentArea.PackStart(scrollView, true, true, 0);
        //this.Decorated = false; //删除工具栏
        Drawn += FormBase_Drawn;
        Close += FormBase_Close;
    }

    private void FormBase_Close(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(this, "你正在关闭该窗口，确定要关闭吗？", "Esc按键操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

    private void FormBase_Response(object? o, ResponseArgs args)
    {
        if (args.ResponseId == ResponseType.DeleteEvent)
        {
            if (CloseWindowEvent?.Invoke(this, EventArgs.Empty)??false)
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
            Scroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.ThumbTrack, (int)(adj?.Value > adj?.StepIncrement ? adj.Value - adj.StepIncrement : adj?.Value ?? 0), (int)(adj?.Value ?? 0), ScrollOrientation.VerticalScroll));
        }
    }

    private void Hadjustment_ValueChanged(object? sender, EventArgs e)
    {
        if (Scroll != null)
        {
            var adj = (Adjustment?)sender;
            Scroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.ThumbTrack, (int)(adj?.Value > adj?.StepIncrement ? adj.Value - adj.StepIncrement : adj?.Value??0), (int)(adj?.Value??0), ScrollOrientation.HorizontalScroll));
        }
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
        scrollView.Child = child;
    }
}