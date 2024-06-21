
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public abstract class ScrollableBoxBase : Gtk.Viewport, IControlGtk, IScrollableBoxBase
    {
        Gtk.ScrolledWindow scrolledWindow = new Gtk.ScrolledWindow();
        public event ScrollEventHandler Scroll;
        public GtkControlOverride Override { get; set; }
        internal ScrollableBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.ShadowType = Gtk.ShadowType.In;
            this.BorderWidth = 1;

            scrolledWindow.Halign = Gtk.Align.Fill;
            scrolledWindow.Valign = Gtk.Align.Fill;
            scrolledWindow.Hexpand = true;
            scrolledWindow.Vexpand = true;
            scrolledWindow.VscrollbarPolicy = Gtk.PolicyType.Never;
            scrolledWindow.HscrollbarPolicy = Gtk.PolicyType.Never;
            scrolledWindow.Hadjustment.ValueChanged += Hadjustment_ValueChanged;
            scrolledWindow.Vadjustment.ValueChanged += Vadjustment_ValueChanged;
            base.Child = scrolledWindow;
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

        public new Gtk.Widget Child { get => scrolledWindow.Child; }
        public void AddClass(string cssClass)
        {
            this.Override.AddClass(cssClass);
        }
        public new void Add(Gtk.Widget child)
        {
            scrolledWindow.Child = child;
        }
        public void Pack(Gtk.Widget child, Gtk.Align align, bool expand)
        {
            scrolledWindow.Halign = align;
            scrolledWindow.Valign = align;
            scrolledWindow.Hexpand = expand;
            scrolledWindow.Vexpand = expand;
            scrolledWindow.Child = child;
        }
        public bool VScroll { get; set; } = true;
        public bool HScroll { get; set; } = true;
        public virtual bool AutoScroll
        {
            get => scrolledWindow.VscrollbarPolicy == Gtk.PolicyType.Automatic;
            set
            {
                if (value == true)
                {
                    if (VScroll)
                        scrolledWindow.VscrollbarPolicy = Gtk.PolicyType.Automatic;
                    if (HScroll)
                        scrolledWindow.HscrollbarPolicy = Gtk.PolicyType.Automatic;
                }
                else
                {
                    scrolledWindow.VscrollbarPolicy = Gtk.PolicyType.Never;
                    scrolledWindow.HscrollbarPolicy = Gtk.PolicyType.Never;
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
            return new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = GetDrawRectangle();
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
