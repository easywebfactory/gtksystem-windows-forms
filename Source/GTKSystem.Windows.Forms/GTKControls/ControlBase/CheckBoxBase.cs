using Gdk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class CheckBoxBase : Gtk.CheckButton, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public CheckBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("CheckBox");
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }
    }
}
