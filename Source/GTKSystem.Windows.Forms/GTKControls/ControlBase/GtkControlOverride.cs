
using Gtk;
using GTKSystem.Windows.Forms.Utility;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public delegate void PaintGraphicsEventHandler(Cairo.Context cr, Rectangle rec);
    public sealed class GtkControlOverride: IControlOverride
    {
        private Widget container;
        public GtkControlOverride(Widget container)
        {
            this.container = container;
        }
        public event DrawnHandler DrawnBackground;
        public event PaintEventHandler Paint;
        public System.Drawing.Color? BackColor { get; set; }
        private System.Drawing.Image _BackgroundImage;
        public System.Drawing.Image BackgroundImage { get { return _BackgroundImage; } set { _BackgroundImage = value; backgroundPixbuf = null; } }
        public ImageLayout BackgroundImageLayout { get; set; }
        public System.Drawing.Image Image { get; set; }
        public System.Drawing.ContentAlignment ImageAlign { get; set; }

        private List<string> cssList = new List<string>();
        public void AddClass(string cssClass)
        {
            cssList.Add(cssClass);
        }
        public void RemoveClass(string cssClass)
        {
            cssList.Remove(cssClass);
        }
        public void OnAddClass()
        {
            foreach (string cssClass in cssList)
            {
                if(container.StyleContext.HasClass(cssClass))
                    container.StyleContext.RemoveClass(cssClass);
                container.StyleContext.AddClass(cssClass);
            }
            ClearNativeBackground();
        }
        public void ClearNativeBackground()
        {
        }
        private Gdk.Pixbuf backgroundPixbuf;
        public void DrawnBackColor(Cairo.Context cr, Gdk.Rectangle area)
        {
            if (BackColor.HasValue)
            {
                cr.Save();
                cr.SetSourceRGBA(BackColor.Value.R / 255f, BackColor.Value.G / 255f, BackColor.Value.B / 255f, BackColor.Value.A / 255f);
                cr.Paint();
                cr.Restore();
            }
        }
        public void OnDrawnBackground(Cairo.Context cr, Gdk.Rectangle area)
        {
            if (BackColor.HasValue)
            {
                cr.Save();
                cr.SetSourceRGBA(BackColor.Value.R / 255f, BackColor.Value.G / 255f, BackColor.Value.B / 255f, BackColor.Value.A / 255f);
                cr.Paint();
                cr.Restore();
            }
            if (BackgroundImage != null && BackgroundImage.PixbufData != null)
            {
                if (backgroundPixbuf == null || backgroundPixbuf.Width != area.Width || backgroundPixbuf.Height != area.Height)
                {
                    ImageUtility.ScaleImageByImageLayout(BackgroundImage.PixbufData, area.Width, area.Height, out backgroundPixbuf, BackgroundImageLayout);
                }
                ImageUtility.DrawImage(cr, backgroundPixbuf, area, ContentAlignment.TopLeft);
            }

            if (DrawnBackground != null)
            {
                DrawnArgs args = new DrawnArgs() { Args = new object[] { cr } };
                DrawnBackground(this.container, args);
            }
        }
        private Gdk.Pixbuf imagePixbuf;
        public void OnDrawnImage(Cairo.Context cr, Gdk.Rectangle area)
        {
            if (Image != null && Image.PixbufData != null)
            {
                if (imagePixbuf == null || imagePixbuf.Width != area.Width || imagePixbuf.Height != area.Height)
                {
                    Gdk.Pixbuf imagepixbuf = new Gdk.Pixbuf(Image.PixbufData);
                    imagePixbuf = imagepixbuf.ScaleSimple(area.Width, area.Height, Gdk.InterpType.Nearest);
                }
                ImageUtility.DrawImage(cr, imagePixbuf, area, ImageAlign);
            }
        }
        
        public event PaintGraphicsEventHandler PaintGraphics;
        public void OnPaint(Cairo.Context cr, Gdk.Rectangle area)
        {
            if (PaintGraphics != null) {
                PaintGraphics(cr, new Rectangle(area.X, area.Y, area.Width, area.Height));
            }
            if (Paint != null)
                Paint(this.container, new PaintEventArgs(new Graphics(container, cr, area), new Rectangle(area.X, area.Y, area.Width, area.Height)));
        }
    }
}
