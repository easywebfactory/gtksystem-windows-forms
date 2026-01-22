namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class NumericUpDownBase : Gtk.SpinButton, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public NumericUpDownBase() : base(0, 100, 1)
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("NumericUpDown");
            this.Value = 0;
            this.Orientation = Gtk.Orientation.Horizontal;
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }
    }
}
