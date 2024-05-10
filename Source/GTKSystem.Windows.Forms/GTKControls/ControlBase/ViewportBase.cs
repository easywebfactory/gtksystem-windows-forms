using Gtk;
using System;
using System.ComponentModel;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ViewportBase : Gtk.Viewport, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal ViewportBase() : base()
        {
            this.Override = new GtkControlOverride(this);
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnDrawnBackground(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
