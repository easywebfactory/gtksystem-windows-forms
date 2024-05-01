using Gtk;
using GTKSystem.Windows.Forms.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class GtkControlOverride: IControlOverride
    {
        private Widget container;
        public GtkControlOverride(Widget container)
        {
            this.container = container;
        }

        public event DrawnHandler DrawnBackground;
        public event PaintEventHandler Paint;
        public Color? BackColor { get; set; }
        public System.Drawing.Image BackgroundImage { get; set; }
        public ImageLayout BackgroundImageLayout { get; set; }

        private List<string> cssList = new List<string>();
        public void AddClass(string cssClass)
        {
            cssList.Add(cssClass);
        }
        public void OnAddClass()
        {
            foreach (string cssClass in cssList)
            {
                container.StyleContext.AddClass(cssClass);
            }
            if (BackgroundImage != null)
            {
                container.StyleContext.AddClass("BackgroundTransparent");
            }
        }
        private Gdk.Pixbuf backgroundPixbuf;
        public void OnDrawnBackground(Cairo.Context cr, Gdk.Rectangle area)
        {
            if (BackgroundImage != null && BackgroundImage.PixbufData != null)
            {
                if (backgroundPixbuf == null || backgroundPixbuf.Width != area.Width || backgroundPixbuf.Height != area.Height)
                {
                    ImageUtility.ScaleImageByImageLayout(BackgroundImage.PixbufData, area.Width, area.Height, out backgroundPixbuf, BackgroundImageLayout);
                }
                cr.Save();
                //Console.WriteLine($"{area.Width},{area.Height}");
                cr.ResetClip();
                cr.Rectangle(area.Left, area.Top, area.Width, area.Height);
                cr.Clip();
                if (BackColor.HasValue)
                    cr.SetSourceRGBA(BackColor.Value.R / 255f, BackColor.Value.G / 255f, BackColor.Value.B / 255f, BackColor.Value.A / 255f);
                else
                    cr.SetSourceRGBA(0.98, 0.97, 0.97, 1);

                cr.Fill();
                cr.Translate(area.Left, area.Top);
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
                Paint(this, new PaintEventArgs(new Graphics(container, cr, area), new Rectangle(area.X, area.Y, area.Width, area.Height)));
        }
    }
}
