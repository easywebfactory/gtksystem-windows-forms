using Gtk;
using System;
using System.Collections.Generic;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class CheckBoxBase : Gtk.CheckButton, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal CheckBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("CheckBox");
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnDrawnBackground(cr, rec);
            Override.OnPaint(cr, rec);
            return base.OnDrawn(cr);
        }
    }
}
