using Gtk;
using System;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TabPageBase : ScrollableBoxBase
    {
        public Gtk.Overlay contaner = new Gtk.Overlay();
        public TabPageBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("TabPage");
            this.BorderWidth = 0;
            this.contaner.Margin = 0;
            this.contaner.Halign = Align.Fill;
            this.contaner.Valign = Align.Fill;
            this.contaner.Expand = false;
            this.Halign = Align.Fill;
            this.Valign = Align.Fill;

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
