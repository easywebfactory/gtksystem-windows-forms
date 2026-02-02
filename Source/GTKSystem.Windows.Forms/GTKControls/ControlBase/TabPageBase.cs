using Gtk;
using System;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TabPageBase : ScrollableBoxBase
    {
        public Gtk.Overlay Content = new Gtk.Overlay();
        public TabPageBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("TabPage");
            this.BorderWidth = 0;
            this.Content.Margin = 0;
            this.Content.Halign = Align.Fill;
            this.Content.Valign = Align.Fill;
            this.Content.Expand = false;
            this.Halign = Align.Fill;
            this.Valign = Align.Fill;

            Gtk.DrawingArea background = new Gtk.DrawingArea();
            background.Events = Gdk.EventMask.EnterNotifyMask;
            background.Drawn += Background_Drawn;
            Content.Add(background);
            this.Add(Content);
        }
        private void Background_Drawn(object o, DrawnArgs args)
        {
            Override.OnPaint(args.Cr);
        }
    }
}
