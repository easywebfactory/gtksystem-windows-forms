﻿namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class PictureBoxBase : Gtk.Image, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        internal PictureBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("PictureBox");
            this.Halign = Gtk.Align.Center;
            this.Valign = Gtk.Align.Center;
            this.Xalign = 0.5f;
            this.Yalign = 0.5f;
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
