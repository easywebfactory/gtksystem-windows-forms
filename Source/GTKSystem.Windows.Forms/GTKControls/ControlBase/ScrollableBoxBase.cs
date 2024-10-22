
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public abstract class ScrollableBoxBase : Gtk.ScrolledWindow, IControlGtk, IScrollableBoxBase
    {
        public event ScrollEventHandler Scroll;
        public GtkControlOverride Override { get; set; }
        public ScrollableBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.ShadowType = Gtk.ShadowType.None;
            this.BorderWidth = 1;
            this.Events = Gdk.EventMask.AllEventsMask;
            base.Halign = Gtk.Align.Start;
            base.Valign = Gtk.Align.Start;
            base.Hexpand = true;
            base.Vexpand = true;
            base.VscrollbarPolicy = Gtk.PolicyType.Never;
            base.HscrollbarPolicy = Gtk.PolicyType.Never;
            base.OverlayScrolling = false;
            base.Hadjustment.ValueChanged += Hadjustment_ValueChanged;
            base.Vadjustment.ValueChanged += Vadjustment_ValueChanged;
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

        public void AddClass(string cssClass)
        {
            this.Override.AddClass(cssClass);
        }
        public bool VScroll { get; set; } = true;
        public bool HScroll { get; set; } = true;
        public virtual bool AutoScroll
        {
            get => base.VscrollbarPolicy == Gtk.PolicyType.Automatic;
            set
            {
                if (value == true)
                {
                    if (VScroll)
                        base.VscrollbarPolicy = Gtk.PolicyType.Automatic;
                    if (HScroll)
                        base.HscrollbarPolicy = Gtk.PolicyType.Automatic;
                }
                else
                {
                    base.VscrollbarPolicy = Gtk.PolicyType.Never;
                    base.HscrollbarPolicy = Gtk.PolicyType.Never;
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
