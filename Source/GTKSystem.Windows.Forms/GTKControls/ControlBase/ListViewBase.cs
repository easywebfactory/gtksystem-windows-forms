namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ListViewBase : Gtk.Viewport, IControlGtk
    {
        public Gtk.Box box = new Gtk.Box(Gtk.Orientation.Vertical, 0);
        public GtkControlOverride Override { get; set; }
        public ListViewBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("view");
            this.Override.AddClass("ListView");
            this.BorderWidth = 1;
            this.Add(box);
        }
    }
}
