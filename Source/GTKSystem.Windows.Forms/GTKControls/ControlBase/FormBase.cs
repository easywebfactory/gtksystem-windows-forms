using Gtk;
using System;
using System.Linq;
using System.Windows.Forms;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class FormBase : Gtk.Dialog, IControlGtk, IScrollableBoxBase, IWin32Window
    {
        public readonly Gtk.ScrolledWindow ScrollView = new Gtk.ScrolledWindow();
        public GtkControlOverride Override { get; set; }
        public bool AutoScroll
        {
            get => ScrollView.VscrollbarPolicy == Gtk.PolicyType.Automatic;
            set
            {
                if (value == true)
                {
                    if (VScroll)
                        ScrollView.VscrollbarPolicy = Gtk.PolicyType.Automatic;
                    if (HScroll)
                        ScrollView.HscrollbarPolicy = Gtk.PolicyType.Automatic;
                }
                else
                {
                    ScrollView.VscrollbarPolicy = Gtk.PolicyType.Never;
                    ScrollView.HscrollbarPolicy = Gtk.PolicyType.Never;
                }
            }
        }
        public bool VScroll { get; set; } = true;
        public bool HScroll { get; set; } = true;

        public delegate bool CloseWindowHandler(object sender, EventArgs e);
        public event CloseWindowHandler CloseWindowEvent;
        public event System.Windows.Forms.ScrollEventHandler Scroll;
        public FormBase(Gtk.Window parent = null) : base("title", Gtk.Window.ListToplevels().LastOrDefault(o => o is FormBase && o.IsActive), DialogFlags.UseHeaderBar)
        {
            this.DestroyWithParent = true;
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Form");
            this.WindowPosition = Gtk.WindowPosition.Center;
            this.BorderWidth = 0;
            this.ContentArea.BorderWidth = 0;
            this.ContentArea.Spacing = 0;
            this.ContentArea.Homogeneous = false;

            this.SetDefaultSize(100, 100);
            this.TypeHint = Gdk.WindowTypeHint.Normal;
            this.AppPaintable = false;
            this.Deletable = true;
            this.Response += FormBase_Response;
            ScrollView.BorderWidth = 0;
            ScrollView.Valign = Gtk.Align.Fill;
            ScrollView.Halign = Gtk.Align.Fill;
            ScrollView.Hexpand = true;
            ScrollView.Vexpand = true;
            ScrollView.OverlayScrolling = true;
            ScrollView.KineticScrolling = true;
            ScrollView.HscrollbarPolicy = PolicyType.Automatic;
            ScrollView.VscrollbarPolicy = PolicyType.Automatic;
            ScrollView.Hadjustment.ValueChanged += Hadjustment_ValueChanged;
            ScrollView.Vadjustment.ValueChanged += Vadjustment_ValueChanged;
            this.ContentArea.PackStart(ScrollView, true, true, 0);
            //this.Decorated = false; //删除工具栏
            this.Drawn += FormBase_Drawn;
            this.Close += FormBase_Close;
        }
        private bool IsNoEscFormClose = false;
        private void FormBase_Close(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "你正在关闭该窗口，确定要关闭吗？", "Esc按键操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                IsNoEscFormClose = false;
                this.Respond(ResponseType.DeleteEvent);
            }
            else
            {
                IsNoEscFormClose = true;
            }
        }

        private void FormBase_Drawn(object o, DrawnArgs args)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnPaint(args.Cr, rec);
        }

        private void FormBase_Response(object o, ResponseArgs args)
        {
            if (IsNoEscFormClose)
            {
                IsNoEscFormClose = false;
            }
            else if (args.ResponseId == ResponseType.DeleteEvent)
            {
                if (CloseWindowEvent(this, EventArgs.Empty))
                    this.HideOnDelete();
                else
                    this.Run();
            }
        }
        private void Vadjustment_ValueChanged(object sender, EventArgs e)
        {
            if (Scroll != null)
            {
                Gtk.Adjustment adj = (Gtk.Adjustment)sender;
                Scroll(this, new System.Windows.Forms.ScrollEventArgs(ScrollEventType.ThumbTrack, (int)(adj.Value > adj.StepIncrement ? (adj.Value - adj.StepIncrement) : adj.Value), (int)adj.Value, ScrollOrientation.VerticalScroll));
            }
        }

        private void Hadjustment_ValueChanged(object sender, EventArgs e)
        {
            if (Scroll != null)
            {
                Gtk.Adjustment adj = (Gtk.Adjustment)sender;
                Scroll(this, new System.Windows.Forms.ScrollEventArgs(ScrollEventType.ThumbTrack, (int)(adj.Value > adj.StepIncrement ? (adj.Value - adj.StepIncrement) : adj.Value), (int)adj.Value, ScrollOrientation.HorizontalScroll));
            }
        }
        public void CloseWindow()
        {
            this.HideOnDelete();
        }

        public void AddClass(string cssClass)
        {
            this.Override.AddClass(cssClass);
        }
        public new void Add(Gtk.Widget child)
        {
            ScrollView.Child = child;
        }
    }
}
