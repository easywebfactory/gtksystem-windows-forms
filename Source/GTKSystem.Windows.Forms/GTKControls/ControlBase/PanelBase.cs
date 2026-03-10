using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class PanelBase: ScrollableBoxBase
    {
        public Gtk.Overlay contaner = new Gtk.Overlay();
        public PanelBase() : base()
        {
            this.StyleContext.AddClass("Panel");
            this.ShadowType = Gtk.ShadowType.None;
            this.BorderWidth = 0;
            contaner.Margin = 0;
            contaner.Halign = Align.Fill;
            contaner.Valign = Align.Fill;
            contaner.Hexpand = false;
            contaner.Vexpand = false;
            contaner.BorderWidth = 0;
            Gtk.DrawingArea background = new Gtk.DrawingArea();
            background.Events = Gdk.EventMask.EnterNotifyMask;
            background.Drawn += Background_Drawn;
            contaner.Add(background);
            this.Add(contaner);
        }
        private void Background_Drawn(object o, DrawnArgs args)
        {
            Override.OnPaint(args.Cr);
        }
    }
}
