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
            Override.OnDrawnBackground(cr, this.Allocation);
            return base.OnDrawn(cr);
        }
    }
}
