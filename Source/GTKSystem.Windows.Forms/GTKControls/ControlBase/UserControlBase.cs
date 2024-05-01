using Gtk;
using System;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class UserControlBase : Gtk.Viewport, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal UserControlBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("UserControl");
            //self.Override.AddClass("BackgroundTransparent");
            this.MarginStart = 0;
            this.MarginTop = 0;
            this.BorderWidth = 0;
            this.Halign = Align.Start;
            this.Valign = Align.Start;
            this.Expand = false;
            this.Hexpand = false;
            this.Vexpand = false;
        }
        public void AddClass(string cssClass)
        {
            this.Override.AddClass(cssClass);
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = this.Allocation;
            Override.OnDrawnBackground(cr, rec);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
