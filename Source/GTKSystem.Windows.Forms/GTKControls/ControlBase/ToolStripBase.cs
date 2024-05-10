using Gtk;
using System;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ToolStripBase : Gtk.MenuBar, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal ToolStripBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("ToolStrip");
            this.Hexpand = false;
            this.Vexpand = false;
            this.Valign = Gtk.Align.Start;
            this.Halign = Gtk.Align.Start;
            this.HeightRequest = 20;
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
        //protected override void OnResizeChecked()
        //{
        //    base.OnResizeChecked();
        //    Override.OnResizeChecked(this.Allocation);
        //}
        //protected override bool OnDrawn(Cairo.Context cr)
        //{
        //    Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
        //    Override.OnDrawnBackground(cr, rec);
        //    Override.OnPaint(cr, rec);
        //    return base.OnDrawn(cr);
        //}
    }
}
