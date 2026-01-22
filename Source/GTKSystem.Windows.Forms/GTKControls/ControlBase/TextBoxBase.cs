namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TextBoxBase : Gtk.Entry, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public TextBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("TextBox");
            base.Halign = Gtk.Align.Start;
            base.Valign = Gtk.Align.Start;
        }
        public void AddClass(string cssClass)
        {
            this.StyleContext.AddClass(cssClass);
        }
    }
}
