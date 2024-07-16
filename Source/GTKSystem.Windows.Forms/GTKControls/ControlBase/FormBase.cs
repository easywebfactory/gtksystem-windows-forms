using Gtk;
using System;
using System.Linq;
using System.Windows.Forms;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class FormBase : Gtk.Dialog, IControlGtk, IScrollableBoxBase
    {
        public readonly Gtk.ScrolledWindow ScrollView = new Gtk.ScrolledWindow();
        public readonly Gtk.Layout StatusBar = new Gtk.Layout(new Gtk.Adjustment(1, 1, 100, 1, 0, 1), new Gtk.Adjustment(1, 1, 100, 1, 0, 1));
        public readonly Gtk.Viewport StatusBarView = new Gtk.Viewport();
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
        public FormBase() : base("title", Gtk.Window.ListToplevels().LastOrDefault(o => o is FormBase && o.IsActive), DialogFlags.DestroyWithParent)
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Form");
            this.WindowPosition = Gtk.WindowPosition.CenterAlways;
            this.BorderWidth = 0;
            this.ContentArea.BorderWidth = 0;
            this.ContentArea.Spacing = 0;
            this.SetDefaultSize(100, 100);
            this.TypeHint = Gdk.WindowTypeHint.Normal;
            this.AppPaintable = false;
            this.Deletable = true;
            this.Response += FormBase_Response;
            ScrollView.BorderWidth = 0;
            ScrollView.Valign = Gtk.Align.Fill;
            ScrollView.Halign = Gtk.Align.Fill;
            ScrollView.HscrollbarPolicy = PolicyType.Always;
            ScrollView.VscrollbarPolicy = PolicyType.Always;
            ScrollView.Hadjustment.ValueChanged += Hadjustment_ValueChanged;
            ScrollView.Vadjustment.ValueChanged += Vadjustment_ValueChanged;
            this.ContentArea.PackStart(ScrollView, true, true, 0);
            StatusBar.Hexpand = false;
            StatusBar.Vexpand = false;
            StatusBar.Halign = Gtk.Align.Fill;
            StatusBar.Valign = Gtk.Align.Fill;
            StatusBar.NoShowAll = true;
            StatusBar.Visible = false;
            StatusBar.BorderWidth = 0;
            StatusBarView.BorderWidth = 0;
            StatusBarView.StyleContext.AddClass("StatusStrip");
            StatusBarView.Child = StatusBar;
            this.ContentArea.PackEnd(StatusBarView, false, true, 0);
            //this.Decorated = false; //删除工具栏
        }
        private void FormBase_Response(object o, ResponseArgs args)
        {
            if (args.ResponseId == ResponseType.DeleteEvent)
            {
                if (CloseWindowEvent(this, EventArgs.Empty))
                    this.Destroy();
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
        public void ResizeControls(int widthIncrement, int heightIncrement, Gtk.Container parent)
        {
            foreach (Gtk.Widget control in parent.Children)
            {
                if (control != null)
                {
                    bool checksizing = false;
                    if (control.Data["Control"] != null)
                    {
                        Control con = (Control)control.Data["Control"];
                        DockStyle dock = con.Dock;
                        if (dock == DockStyle.None)
                        {
                            AnchorStyles anchor = con.Anchor;
                            if (anchor != AnchorStyles.None)
                            {
                                checksizing = true;
                                if ((anchor & AnchorStyles.Right) != 0)
                                {
                                    if ((anchor & AnchorStyles.Left) != 0)
                                    {
                                        control.WidthRequest = Math.Max(0, (int)control.Data["Width"] + widthIncrement);
                                    }
                                    else
                                    {
                                        if (parent[control] is Gtk.Layout.LayoutChild lc)
                                        {
                                            lc.X = (int)control.Data["Left"] + widthIncrement;
                                        }
                                        else if (parent[control] is Gtk.Fixed.FixedChild fc)
                                        {
                                            fc.X = (int)control.Data["Left"] + widthIncrement;
                                        }
                                    }
                                }
                                if ((anchor & AnchorStyles.Bottom) != 0)
                                {
                                    if ((anchor & AnchorStyles.Top) != 0)
                                    {
                                        control.HeightRequest = Math.Max(0, (int)control.Data["Height"] + heightIncrement);
                                    }
                                    else
                                    {
                                        if (parent[control] is Gtk.Layout.LayoutChild lc)
                                        {
                                            lc.Y = (int)control.Data["Top"] + heightIncrement;
                                        }
                                        else if (parent[control] is Gtk.Fixed.FixedChild fc)
                                        {
                                            fc.Y = (int)control.Data["Top"] + heightIncrement;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            checksizing = true;
                            if (dock == DockStyle.Top)
                            {
                                control.Valign = Gtk.Align.Start;
                                control.Hexpand = true;
                                if (control.WidthRequest > -1)
                                    control.WidthRequest = Math.Max(0, (int)control.Data["Width"] + widthIncrement);
                            }
                            else if (dock == DockStyle.Bottom)
                            {
                                control.Valign = Gtk.Align.End;
                                control.Halign = Gtk.Align.Fill;
                                control.Expand = true;
                                if (parent[control] is Gtk.Layout.LayoutChild lc)
                                {
                                    lc.Y = parent.HeightRequest - control.HeightRequest;
                                }
                                else if (parent[control] is Gtk.Fixed.FixedChild fc)
                                {
                                    fc.Y = parent.HeightRequest - control.HeightRequest;
                                }
                                if (control.WidthRequest > -1)
                                    control.WidthRequest = Math.Max(0, (int)control.Data["Width"] + widthIncrement);
                            }
                            else if (dock == DockStyle.Left)
                            {
                                control.Halign = Gtk.Align.Start;
                                control.Vexpand = true;
                                if (control.HeightRequest > -1)
                                    control.HeightRequest = Math.Max(0, (int)control.Data["Height"] + heightIncrement);
                            }
                            else if (dock == DockStyle.Right)
                            {
                                control.Halign = Gtk.Align.End;
                                if (parent[control] is Gtk.Layout.LayoutChild lc)
                                {
                                    lc.X = parent.WidthRequest - control.WidthRequest - 6;
                                }
                                else if (parent[control] is Gtk.Fixed.FixedChild fc)
                                {
                                    fc.X = parent.WidthRequest - control.WidthRequest - 6;
                                }
                                if (control.HeightRequest > -1)
                                    control.HeightRequest = Math.Max(0, (int)control.Data["Height"] + heightIncrement);
                            }
                            else if (dock == DockStyle.Fill)
                            {
                                control.Hexpand = true;
                                control.Vexpand = true;
                                if (control.HeightRequest > -1)
                                    control.HeightRequest = Math.Max(0, (int)control.Data["Height"] + heightIncrement);
                                if (control.WidthRequest > -1)
                                    control.WidthRequest = Math.Max(0, (int)control.Data["Width"] + widthIncrement);
                            }
                        }
                    }
                    if (control is Gtk.MenuBar)
                    {
                        //菜单不用处理
                    }
                    else if (control is Gtk.TreeView)
                    {
                        //树目录不用处理
                    }
                    else if (control is Gtk.Paned paned)
                    {//内部处理
                        paned.CheckResize();
                    }
                    else if (control is Gtk.Viewport || control is Gtk.ScrolledWindow || control is Gtk.Fixed || control is Gtk.Layout || control is Gtk.Box)
                    {
                        ResizeControls(widthIncrement, heightIncrement, (Gtk.Container)control);
                    }
                    else if (checksizing && control is Gtk.Container container)
                    {
                        ResizeControls(widthIncrement, heightIncrement, container);
                    }
                }
            }
        }

        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
        public void CloseWindow()
        {
            this.Destroy();
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
