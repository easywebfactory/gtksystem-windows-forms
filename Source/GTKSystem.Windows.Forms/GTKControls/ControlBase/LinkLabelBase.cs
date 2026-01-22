namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class LinkLabelBase : Gtk.LinkButton, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public LinkLabelBase() : base("")
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("LinkLabel");
            this.BorderWidth = 0;
            base.Valign = Gtk.Align.Start;
            base.Halign = Gtk.Align.Start;
            base.Drawn += LinkLabelBase_Drawn;
        }

        private void LinkLabelBase_Drawn(object o, Gtk.DrawnArgs args)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnPaint(args.Cr, rec);
        }
    }
}
