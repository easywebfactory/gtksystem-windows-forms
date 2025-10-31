
using Gtk;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
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

        private Gtk.Button minimize = null;
        private Gtk.Button maximize = null;
        public FormBase() : base("title", null, DialogFlags.UseHeaderBar | DialogFlags.DestroyWithParent)
        {
            this.Override = new GtkControlOverride(this);
            this.WindowPosition = Gtk.WindowPosition.Center;
            this.BorderWidth = 0;
            if(Titlebar is Gtk.HeaderBar headerbar)
            {
                headerbar.DecorationLayout = "menu:close";
                maximize = new Gtk.Button("window-maximize-symbolic", IconSize.SmallToolbar)
                {
                    Name = "maximize",
                    Visible = true,
                    Relief = ReliefStyle.None,
                    Valign = Align.Center,
                    Halign = Align.Center,
                    AlwaysShowImage = true
                };
                maximize.StyleContext.AddClass("maximize");
                maximize.StyleContext.AddClass("titlebutton");
                maximize.Clicked += maximize_Clicked;
                headerbar.PackEnd(maximize);
                minimize = new Gtk.Button("window-minimize-symbolic", IconSize.SmallToolbar)
                {
                    Name = "minimize",
                    Visible = true,
                    Relief = ReliefStyle.None,
                    Valign = Align.Center,
                    Halign = Align.Center,
                    AlwaysShowImage = true
                };
                minimize.StyleContext.AddClass("minimize");
                minimize.StyleContext.AddClass("titlebutton");
                minimize.Clicked += minimize_Clicked;
                headerbar.PackEnd(minimize);
            }
          
            this.SetDefaultSize(100, 100);
            this.TypeHint = Gdk.WindowTypeHint.Normal;
            this.AppPaintable = false;
            this.Deletable = true;
            this.Decorated = true;
            this.Drawn += FormBase_Drawn;
            this.Close += FormBase_Close;
            this.Response += FormBase_Response;
            this.DeleteEvent += FormBase_DeleteEvent;
            this.WindowStateEvent += FormBase_WindowStateEvent;
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

        private void FormBase_WindowStateEvent(object o, WindowStateEventArgs args)
        {
            if (args.Event.NewWindowState.HasFlag(Gdk.WindowState.Maximized))
            {
                maximize.Image = Gtk.Image.NewFromIconName("window-restore-symbolic", IconSize.SmallToolbar);
                maximize.Name = "restore";
                maximize.StyleContext.AddClass("restore");
            }
            else
            {
                maximize.Image = Gtk.Image.NewFromIconName("window-maximize-symbolic", IconSize.SmallToolbar);
                maximize.Name = "maximize";
                maximize.StyleContext.RemoveClass("restore");
            }
        }
        private void maximize_Clicked(object? sender, EventArgs e)
        {
            var maximize = sender as Gtk.Button;
            if (maximize.Name == "restore")
            {
                this.Unmaximize();
                maximize.Name = "maximize";
            }
            else
            {
                this.Maximize();
                maximize.Name = "restore";
            }
        }

        private void minimize_Clicked(object? sender, EventArgs e)
        {
            this.Iconify();
        }
        public bool MaximizeBox
        {
            get => maximize.Visible;
            set
            {
                maximize.NoShowAll = true;
                maximize.Visible = value;
            }
        }
        public bool MinimizeBox
        {
            get => minimize.Visible;
            set
            {
                minimize.NoShowAll = true;
                minimize.Visible = value;
            }
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
