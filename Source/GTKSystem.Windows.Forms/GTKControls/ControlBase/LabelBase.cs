namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class LabelBase : Gtk.Label, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal LabelBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Label");
            this.Xalign = 0.08f;
            this.Yalign = 0.08f;
        }

        internal LabelBase(string text) : base(text)
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Label");
            this.Xalign = 0.08f;
            this.Yalign = 0.08f;
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnDrawnBackground(cr, rec);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
