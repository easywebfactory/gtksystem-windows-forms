
using System;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class PanelBase: Gtk.Viewport, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal PanelBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Panel");
            this.ShadowType = Gtk.ShadowType.In;
            this.BorderWidth = 0;
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnDrawnBackground(cr, rec);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
