using Gtk;
using GTKSystem.Windows.Forms.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms
{
    public sealed class GtkControlOverride
    {
        private Gtk.Widget container;
        public GtkControlOverride(Gtk.Widget container) {
            this.container = container;
        }

        public event DrawnHandler DrawnBackground;
        public event PaintEventHandler Paint;
        public System.Drawing.Color? BackColor { get; set; }
        public System.Drawing.Image BackgroundImage { get; set; }
        public ImageLayout BackgroundImageLayout { get; set; }

        private List<string> cssList = new List<string>();
        public void AddClass(string cssClass)
        {
            cssList.Add(cssClass);
        }
        public void OnAddClass() {
            if (BackColor.HasValue == false && BackgroundImage == null)
            {
                foreach (string cssClass in cssList)
                {
                    this.container.StyleContext.AddClass(cssClass);
                }
            }
            else
            {
                this.container.StyleContext.AddClass("BackgroundTransparent");
            }
        }
        private Gdk.Pixbuf backgroundPixbuf;
        public void OnDrawnBackground(Cairo.Context cr, Gdk.Rectangle area)
        {
            if (BackColor.HasValue)
            {
                cr.Save();
                cr.SetSourceRGB(BackColor.Value.R / 255f, BackColor.Value.G / 255f, BackColor.Value.B / 255f);
                cr.Rectangle(2, 2, area.Width - 4, area.Height - 4);
                cr.Fill();
                cr.Restore();
            }
            if (BackgroundImage != null && BackgroundImage.PixbufData != null)
            {
                if (backgroundPixbuf == null || backgroundPixbuf.Width != area.Width - 4 || backgroundPixbuf.Height != area.Height)
                {
                    ImageUtility.ScaleImageByImageLayout(BackgroundImage.PixbufData, area.Width - 4, area.Height, out backgroundPixbuf, BackgroundImageLayout);
                }
                cr.Save();
                cr.Translate(2, 2);
                Gdk.CairoHelper.SetSourcePixbuf(cr, backgroundPixbuf, 0, 0);
                using (var p = cr.GetSource())
                {
                    if (p is Cairo.SurfacePattern pattern)
                    {
                        if (area.Width > backgroundPixbuf.Width || area.Height > backgroundPixbuf.Height)
                        {
                            pattern.Filter = Cairo.Filter.Fast;
                        }
                        else
                            pattern.Filter = Cairo.Filter.Good;
                    }
                }
                cr.Paint();
                cr.Restore();
            }

            DrawnArgs args = new DrawnArgs() { Args = new object[] { cr } };
            if (DrawnBackground != null)
            {
                DrawnBackground(this, args);
            }
        }
        public void OnPaint(Cairo.Context cr, Gdk.Rectangle area)
        {
            if (Paint != null)
                Paint(this, new PaintEventArgs(new Graphics(this.container, cr, area), new System.Drawing.Rectangle(area.X, area.Y, area.Width, area.Height)));
        }
    }
}
