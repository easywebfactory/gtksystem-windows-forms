using Gtk;
using System;
using System.Drawing;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class BoxBase: Gtk.Box, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal BoxBase(Orientation orientation, int spacing) : base(orientation, spacing)
        {
            this.Override = new GtkControlOverride(this);
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Override.DrawnBackColor(cr, this.Allocation);
            return base.OnDrawn(cr);
        }
    }
}
