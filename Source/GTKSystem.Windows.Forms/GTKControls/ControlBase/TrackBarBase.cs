namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TrackBarBase : Gtk.Viewport, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public TrackBarBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("TrackBar");
            base.Halign = Gtk.Align.Start;
            base.Valign = Gtk.Align.Start;
        }
    }
}
