namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class FlowLayoutPanelBase : Gtk.FlowBox, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public FlowLayoutPanelBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("FlowLayoutPanel");
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }
    }
}
