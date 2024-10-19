using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TabControlBase : Gtk.Notebook, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal TabControlBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("TabControl");
            this.Scrollable = true;
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
