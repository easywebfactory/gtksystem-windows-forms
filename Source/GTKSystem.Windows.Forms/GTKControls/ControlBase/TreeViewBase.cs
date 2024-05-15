namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TreeViewBase : Gtk.TreeView, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal TreeViewBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("TreeView");
            this.BorderWidth = 0;
            this.Expand = true;
            this.HeadersVisible = false;
            this.ActivateOnSingleClick = true;
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
