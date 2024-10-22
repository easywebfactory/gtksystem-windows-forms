namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TextBoxBase : Gtk.Entry, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public TextBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("TextBox");
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
