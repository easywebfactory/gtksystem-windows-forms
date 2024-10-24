namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ViewportBase : Gtk.Viewport, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public ViewportBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            base.Halign = Gtk.Align.Start;
            base.Valign = Gtk.Align.Start;
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnDrawnBackground(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
