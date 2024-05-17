namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ListBoxBase : Gtk.Viewport, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal Gtk.ListBox ListBox = new Gtk.ListBox();
        internal ListBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("ListBox");

            Gtk.ScrolledWindow scrolledWindow = new Gtk.ScrolledWindow();
            scrolledWindow.Add(ListBox);
            this.Child = scrolledWindow;
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
