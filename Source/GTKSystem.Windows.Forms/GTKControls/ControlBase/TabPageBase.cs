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
            this.Override.AddClass("TabPage");
            this.BorderWidth = 0;
            this.Content.MarginStart = 0;
            this.Content.MarginTop = 0;
            this.Content.Halign = Align.Fill;
            this.Content.Valign = Align.Fill;
            this.Content.Expand = true;
            this.Content.Add(new Gtk.Fixed() { Halign = Align.Fill, Valign = Align.Fill });
            base.Halign = Align.Fill;
            base.Valign = Align.Fill;
            base.Add(Content);
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
