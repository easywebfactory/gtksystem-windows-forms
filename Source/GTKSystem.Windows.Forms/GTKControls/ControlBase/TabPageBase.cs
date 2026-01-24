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

            Gtk.Viewport viewport = new Gtk.Viewport() { BorderWidth = 0 };
            viewport.Drawn += Viewport_Drawn;
            Content.Add(viewport);
            this.Add(Content);
        }
        private void Viewport_Drawn(object o, DrawnArgs args)
        {
            Override.OnPaint(args.Cr);
        }
    }
}
