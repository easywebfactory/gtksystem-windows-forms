using Gtk;
using System;
using System.Drawing;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class LayoutBase: Gtk.Layout, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal LayoutBase(Adjustment hadjustment, Adjustment vadjustment) : base(hadjustment, vadjustment)
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
