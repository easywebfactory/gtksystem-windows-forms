using System;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TabPageBase : ScrollableBoxBase
    {
        public Gtk.Fixed Content = new Gtk.Fixed();
        internal TabPageBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("TabPage");
            this.BorderWidth = 0;
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
