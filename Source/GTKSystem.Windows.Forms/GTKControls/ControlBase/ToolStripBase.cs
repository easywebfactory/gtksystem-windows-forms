namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ToolStripBase : Gtk.MenuBar, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public ToolStripBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("ToolStrip");
            this.Hexpand = false;
            this.Vexpand = false;
            this.Valign = Gtk.Align.Start;
            this.Halign = Gtk.Align.Start;
            this.HeightRequest = 20;
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
