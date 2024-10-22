namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class CheckBoxBase : Gtk.CheckButton, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public CheckBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("CheckBox");
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
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
