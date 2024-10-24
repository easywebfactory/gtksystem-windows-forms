using System;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class PictureBoxBase : Gtk.Image, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public PictureBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("PictureBox");
            base.Valign = Gtk.Align.Fill;
            base.Halign = Gtk.Align.Fill;
            base.Expand = true;
            base.Xalign = 0;
            base.Yalign = 0;
        }
        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        protected override bool OnDrawn(Cairo.Context cr)
        {
            if (this.Pixbuf != null)
            {
                cr.Scale(this.AllocatedWidth * 1f / this.Pixbuf.Width * 1f, this.AllocatedHeight * 1f / this.Pixbuf.Height * 1f);
            }
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, this.AllocatedWidth, this.AllocatedHeight);
            Override.OnDrawnBackground(cr, rec);
            Override.OnPaint(cr, rec);
            base.OnDrawn(cr);
            return true;
        }
    }
}
