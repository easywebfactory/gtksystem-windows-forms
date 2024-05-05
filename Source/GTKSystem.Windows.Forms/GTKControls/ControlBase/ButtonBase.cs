using Gtk;
using System;
using System.ComponentModel;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ButtonBase: Gtk.Button, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal ButtonBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Button");
        }
        internal ButtonBase(Widget widget) : base(widget)
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("Button");
        }
        protected override void OnShown()
        {
            if (Override.BackgroundImage != null || Override.BackColor.HasValue)
                this.StyleContext.AddClass("BackgroundTransparent");
            else
                Override.OnAddClass();

            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(3, 3, this.WidthRequest - 3, this.HeightRequest - 3);
            Override.OnDrawnBackground(cr, rec);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
