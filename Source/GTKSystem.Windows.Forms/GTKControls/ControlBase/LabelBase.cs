namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class LabelBase : Gtk.Label, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public LabelBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("Label");
            this.Xalign = 0.0f;
            this.Yalign = 0.0f;
            this.Valign = Gtk.Align.Start;
            this.Halign = Gtk.Align.Start;
            this.Wrap = true;
            this.LineWrap = true;
            this.LineWrapMode = Pango.WrapMode.WordChar;
        }

        public LabelBase(string text) : base(text)
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("Label");
            this.Xalign = 0.0f;
            this.Yalign = 0.0f;
            this.Valign = Gtk.Align.Start;
            this.Halign = Gtk.Align.Start;
            this.Wrap = true;
            this.LineWrap = true;
            this.LineWrapMode = Pango.WrapMode.WordChar;
            this.Drawn += LabelBase_Drawn;
        }

        private void LabelBase_Drawn(object o, Gtk.DrawnArgs args)
        {
            Override.OnPaint(args.Cr);
        }
    }
}
