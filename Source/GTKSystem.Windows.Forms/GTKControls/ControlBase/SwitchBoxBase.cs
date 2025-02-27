using GLib;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class SwitchBoxBase : Gtk.Switch, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public SwitchBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("SwitchBox");
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
    }
}
