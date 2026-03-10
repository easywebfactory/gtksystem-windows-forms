using Gtk;
using System;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class GroupBoxBase : Gtk.Frame, IControlGtk, IScrollableBoxBase
    {
        public GtkControlOverride Override { get; set; }
        public bool AutoScroll { get; set; }
        public bool HScroll { get; set; }
        public bool VScroll { get; set; }
        public Gtk.Overlay contaner = new Gtk.Overlay();
        public Gtk.ScrolledWindow fixedcontaner = new Gtk.ScrolledWindow();
        public GroupBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("GroupBox");
            this.LabelXalign = 0.03f;
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
            contaner.Halign = Align.Fill;
            contaner.Valign = Align.Fill;
            Gtk.DrawingArea background = new Gtk.DrawingArea();
            background.Events = Gdk.EventMask.EnterNotifyMask;
            background.Drawn += Background_Drawn;
            contaner.Add(background);

            fixedcontaner.SizeAllocated += Fixedcontaner_SizeAllocated;
            fixedcontaner.WidgetEvent += Fixedcontaner_WidgetEvent;
            fixedcontaner.HscrollbarPolicy = PolicyType.Never;
            fixedcontaner.VscrollbarPolicy = PolicyType.External;
            fixedcontaner.Add(contaner);
            this.Add(fixedcontaner);
        }
        private void Background_Drawn(object o, DrawnArgs args)
        {
            Override.OnPaint(args.Cr);
        }

        private void Fixedcontaner_WidgetEvent(object o, WidgetEventArgs args)
        {
            fixedcontaner.Vadjustment.Value = 20;
            args.RetVal = true;
        }

        private void Fixedcontaner_SizeAllocated(object o, SizeAllocatedArgs args)
        {
            fixedcontaner.Vadjustment.Value = 20;
        }
        public event System.Windows.Forms.ScrollEventHandler Scroll;
        public void Pack(Widget child, Align align, bool expand)
        {
            child.Valign = align;
            child.Halign = align;
            child.Expand = expand;
            base.Add(child);
        }
        public Gtk.ScrolledWindow ScrolledWindow { get; }
        public bool ScrollView(double hscrollValue, double vscrollValue)
        {
            return false;
        }
    }
}
