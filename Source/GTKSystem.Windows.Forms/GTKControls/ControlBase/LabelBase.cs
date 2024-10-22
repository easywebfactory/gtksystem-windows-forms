namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class LabelBase : Gtk.Label, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public LabelBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Label");
            this.Xalign = 0.0f;
            this.Yalign = 0.0f;
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
        }

        public LabelBase(string text) : base(text)
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Label");
            this.Xalign = 0.0f;
            this.Yalign = 0.0f;
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
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
