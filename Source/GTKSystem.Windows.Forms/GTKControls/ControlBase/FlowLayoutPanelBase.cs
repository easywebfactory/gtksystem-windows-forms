namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class FlowLayoutPanelBase : Gtk.FlowBox, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal FlowLayoutPanelBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("FlowLayoutPanel");
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
