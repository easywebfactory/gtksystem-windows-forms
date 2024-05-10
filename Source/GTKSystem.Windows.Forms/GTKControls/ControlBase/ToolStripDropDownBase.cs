using Gtk;
using System;


namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ToolStripDropDownBase : Gtk.Menu, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal ToolStripDropDownBase() : base()
        {
            this.Override = new GtkControlOverride(this);
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
