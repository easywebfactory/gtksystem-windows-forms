
using Gtk;
using System.ComponentModel;
using System.Reflection;
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
                    ScrollView.VscrollbarPolicy = Gtk.PolicyType.External;
                    ScrollView.HscrollbarPolicy = Gtk.PolicyType.External;
                }
            }
        }
        public bool VScroll { get; set; } = true;
        public bool HScroll { get; set; } = true;

        public delegate bool CloseWindowHandler(object sender, EventArgs e);
        public event CloseWindowHandler CloseWindowEvent;
        public event System.Windows.Forms.ScrollEventHandler Scroll;
        public FormBase() : base("title", null, DialogFlags.UseHeaderBar | DialogFlags.DestroyWithParent)
        {
            this.Override = new GtkControlOverride(this);
            this.WindowPosition = Gtk.WindowPosition.Center;
            this.BorderWidth = 0;
            ((Gtk.HeaderBar)Titlebar).DecorationLayout = "menu:minimize,maximize,close";
            this.SetDefaultSize(100, 100);
            this.TypeHint = Gdk.WindowTypeHint.Normal;
            this.AppPaintable = false;
            this.Deletable = true;
            this.Decorated = true;
            this.Drawn += FormBase_Drawn;
            this.Close += FormBase_Close;
            this.Response += FormBase_Response;
            this.DeleteEvent += FormBase_DeleteEvent;
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
            this.ContentArea.BorderWidth = 0;
            this.ContentArea.Spacing = 0;
            this.ContentArea.Homogeneous = false;
            this.ContentArea.PackStart(ScrollView, true, true, 0);
            this.ContentArea.StyleContext.AddClass("Form");
        }
        public new Gdk.Pixbuf Icon
        {
            get => base.Icon;
            set
            {
                base.Icon = value;
                if (value != null && base.Titlebar != null && base.Titlebar is Gtk.HeaderBar titlebar)
                {
                    Gtk.Image flag = new Gtk.Image(value.ScaleSimple(24, 24, Gdk.InterpType.Bilinear));
                    flag.Visible = true;
                    titlebar.PackStart(flag);
                }
            }
        }
        private bool closingCancel;
        private void FormBase_DeleteEvent(object o, DeleteEventArgs args)
        {
            closingCancel = CloseWindowEvent(this, EventArgs.Empty);
            args.RetVal = closingCancel;
        }

        private void FormBase_Close(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "你正在关闭该窗口，确定要关闭吗？", "Esc按键操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                closingCancel = false;
                this.Respond(ResponseType.DeleteEvent);
            }
            else
            {
                closingCancel = true;
            }
        }

        private void FormBase_Drawn(object o, DrawnArgs args)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnPaint(args.Cr, rec);
        }

        private void FormBase_Response(object o, ResponseArgs args)
        {
            if (args.ResponseId == ResponseType.DeleteEvent && closingCancel == false)
            {
                ((Gtk.Window)this).Close();
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
            if (!CloseWindowEvent(this, EventArgs.Empty))
            {
                this.Dispose();
                this.Destroy();
            }
        }

        public void AddClass(string cssClass)
        {
            this.StyleContext.AddClass(cssClass);
        }
        public new void Add(Gtk.Widget child)
        {
            ScrollView.Child = child;
        }
    }

}
