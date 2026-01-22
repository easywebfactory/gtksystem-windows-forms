namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ComboBoxBase : Gtk.ComboBoxText, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public ComboBoxBase() : base(true)
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("ComboBox");
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }
        public ComboBoxBase(bool hasEntry) : base(hasEntry)
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("ComboBox");
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }
    }
}
