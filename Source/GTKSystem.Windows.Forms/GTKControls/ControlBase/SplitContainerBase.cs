namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class SplitContainerBase : Gtk.Paned, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public SplitContainerBase() : base(Gtk.Orientation.Vertical)
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("SplitContainer");
            this.BorderWidth = 0;
            this.WideHandle = true;
            this.Orientation = Gtk.Orientation.Horizontal;
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
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
