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
            this.Valign = Gtk.Align.Start;
            this.Halign = Gtk.Align.Start;
            this.Drawn += LinkLabelBase_Drawn;
        }

        private void LinkLabelBase_Drawn(object o, Gtk.DrawnArgs args)
        {
            Override.OnPaint(args.Cr);
        }
    }
}
