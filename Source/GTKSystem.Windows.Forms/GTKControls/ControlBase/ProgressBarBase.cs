namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ProgressBarBase : Gtk.LevelBar, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public ProgressBarBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("ProgressBar");
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }
    }
}
