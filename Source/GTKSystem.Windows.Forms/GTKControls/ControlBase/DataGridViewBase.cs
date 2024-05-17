namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class DataGridViewBase : Gtk.Viewport, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        private Gtk.ScrolledWindow scroll = new Gtk.ScrolledWindow();
        internal Gtk.TreeView GridView = new Gtk.TreeView();
        internal DataGridViewBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("DataGridView");
            this.Override.BackColor = System.Drawing.Color.White;

            GridView.Valign = Gtk.Align.Fill;
            GridView.Halign = Gtk.Align.Fill;
            GridView.Expand = true;
            scroll.Child = GridView;
            this.Child = scroll;
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
