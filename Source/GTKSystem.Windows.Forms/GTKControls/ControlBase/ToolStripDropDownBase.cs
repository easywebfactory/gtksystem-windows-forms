namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ToolStripDropDownBase : Gtk.Menu, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal ToolStripDropDownBase() : base()
        {
            this.Override = new GtkControlOverride(this);
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
