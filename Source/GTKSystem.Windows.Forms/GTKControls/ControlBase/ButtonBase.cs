using Gtk;
using System;


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
