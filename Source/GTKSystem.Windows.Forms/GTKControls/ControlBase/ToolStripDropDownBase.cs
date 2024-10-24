namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ToolStripDropDownBase : Gtk.Menu, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public ToolStripDropDownBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            base.Halign = Gtk.Align.Start;
            base.Valign = Gtk.Align.Start;
        }
        public void AddClass(string cssClass)
        {
            this.Override.AddClass(cssClass);
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
    }
}
