using Gtk;
using System;
using System.Drawing;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ScrollbarBase : Gtk.Scrollbar, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal ScrollbarBase(Orientation orientation) : base(orientation, new Gtk.Adjustment(0, 0, 100, 1, 10, 0))
        {
            this.Override = new GtkControlOverride(this);
        }
    }
}
