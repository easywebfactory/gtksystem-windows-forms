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
    }
}
