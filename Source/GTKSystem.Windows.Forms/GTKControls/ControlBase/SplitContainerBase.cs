namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class SplitContainerBase : Gtk.Paned, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public SplitContainerBase() : base(Gtk.Orientation.Vertical)
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("SplitContainer");
            this.BorderWidth = 0;
            this.WideHandle = true;
            this.Orientation = Gtk.Orientation.Horizontal;
            base.Halign = Gtk.Align.Start;
            base.Valign = Gtk.Align.Start;
        }
    }
}
