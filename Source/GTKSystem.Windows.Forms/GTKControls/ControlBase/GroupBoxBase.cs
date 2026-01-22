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

        public GroupBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("GroupBox");
            this.LabelXalign = 0.03f;
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
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
