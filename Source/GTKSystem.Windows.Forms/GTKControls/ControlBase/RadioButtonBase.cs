namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class RadioButtonBase : Gtk.RadioButton, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public RadioButtonBase() : base(new Gtk.RadioButton("baseradio"))
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("RadioButton");
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }
        public RadioButtonBase(Gtk.RadioButton radio_group_member) : base(radio_group_member)
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("RadioButton");
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }
    }
}
