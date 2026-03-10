using Gtk;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class UserControlBase : ScrollableBoxBase
    {
        public Gtk.Overlay contaner = new Gtk.Overlay();
        public UserControlBase() : base()
        {
            this.StyleContext.AddClass("UserControl");
            this.MarginStart = 0;
            this.MarginTop = 0;
            this.BorderWidth = 0;
            this.ShadowType = ShadowType.None;
            this.Events = Gdk.EventMask.AllEventsMask;
            this.Expand = false;
            this.Hexpand = false;
            this.Vexpand = false;

            contaner.Halign = Align.Fill;
            contaner.Valign = Align.Fill;
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
