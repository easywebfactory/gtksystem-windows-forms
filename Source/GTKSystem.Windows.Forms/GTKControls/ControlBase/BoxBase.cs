using Gtk;
using System;
using System.Drawing;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class BoxBase: Gtk.Box, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public BoxBase(Orientation orientation, int spacing) : base(orientation, spacing)
        {
            this.Override = new GtkControlOverride(this);
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.DrawnBackColor(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
