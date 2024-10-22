using Gtk;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class LayoutBase: Gtk.Layout, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public LayoutBase(Adjustment hadjustment, Adjustment vadjustment) : base(hadjustment, vadjustment)
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
