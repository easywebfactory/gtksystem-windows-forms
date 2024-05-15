namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ProgressBarBase : Gtk.LevelBar, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal ProgressBarBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("ProgressBar");
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
            Override.OnDrawnBackground(cr, rec);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
