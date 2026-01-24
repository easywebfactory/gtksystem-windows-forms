using Gdk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ButtonBase: Gtk.Button, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public ButtonBase() : base(new Gtk.Label() { MaxWidthChars = 0, Wrap = true, LineWrap = true, LineWrapMode = Pango.WrapMode.WordChar })
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("Button");
            this.Valign = Gtk.Align.Start;
            this.Halign = Gtk.Align.Start;
            this.ButtonReleaseEvent += ButtonBase_ButtonReleaseEvent;
            this.Drawn += ButtonBase_Drawn;
        }

        private void ButtonBase_Drawn(object o, Gtk.DrawnArgs args)
        {
            Override.OnPaint(args.Cr);
        }

        private void ButtonBase_ButtonReleaseEvent(object o, Gtk.ButtonReleaseEventArgs args)
        {
            if (this.Toplevel is FormBase form)
            {
                ((System.Windows.Forms.Form)form.Data["Control"]).DialogResult = DialogResult;
            }
        }

        public System.Windows.Forms.DialogResult DialogResult { get; set; }
        public string Text { get => ((Gtk.Label)Child).Text; set => ((Gtk.Label)Child).Text = value; }
    }
}
