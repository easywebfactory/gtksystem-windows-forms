
using Gtk;
using GTKSystem.Windows.Forms.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public delegate void PaintGraphicsEventHandler(Cairo.Context cr, Rectangle rec);
    public sealed class GtkControlOverride: IControlOverride
    {
        private Widget container;
        public object sender;
        public GtkControlOverride(Widget container)
        {
            this.container = container;
            this.sender = container;
        }
        public event PaintEventHandler Paint;
        public System.Drawing.Color? BackColor { get; set; }
        public System.Drawing.Image BackgroundImage { get; set; }
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
            if (BackgroundImage != null && BackgroundImage.Pixbuf != null)
            {
                SetDrawnBackground(cr, BackgroundImage.Pixbuf, BackgroundImageLayout, area.Width, area.Height);
            }
        }
        public void OnDrawnImage(Cairo.Context cr, Gdk.Rectangle area)
        {
            if (Image != null && Image.Pixbuf != null)
            {
                SetDrawImage(cr, Image.Pixbuf, area, ImageAlign);
            }
        }
        public event PaintGraphicsEventHandler PaintGraphics;
        public void OnPaint(Cairo.Context cr, Gdk.Rectangle area)
        {
            if (PaintGraphics != null) {
                PaintGraphics(cr, new Rectangle(area.X, area.Y, area.Width, area.Height));
            }
            if (Paint != null)
                Paint(sender, new PaintEventArgs(new Graphics(container, cr, area), new Rectangle(area.X, area.Y, area.Width, area.Height)));
        }

        public void SetDrawImage(Cairo.Context ctx, Gdk.Pixbuf img, Gdk.Rectangle rec, ContentAlignment ImageAlign)
        {
            ctx.Save();
            if (ImageAlign == ContentAlignment.TopLeft)
                Gdk.CairoHelper.SetSourcePixbuf(ctx, img, rec.X, rec.Y);
            else if (ImageAlign == ContentAlignment.TopCenter)
                Gdk.CairoHelper.SetSourcePixbuf(ctx, img, (img.Width - rec.Width) / 2 + rec.X, rec.Y);
            else if (ImageAlign == ContentAlignment.TopRight)
                Gdk.CairoHelper.SetSourcePixbuf(ctx, img, (img.Width - rec.Width) + rec.X, rec.Y);
            else if (ImageAlign == ContentAlignment.MiddleLeft)
                Gdk.CairoHelper.SetSourcePixbuf(ctx, img, rec.X, (img.Height - rec.Height) / 2 + rec.Y);
            else if (ImageAlign == ContentAlignment.MiddleCenter)
                Gdk.CairoHelper.SetSourcePixbuf(ctx, img, (img.Width - rec.Width) / 2 + rec.X, (img.Height - rec.Height) / 2 + rec.Y);
            else if (ImageAlign == ContentAlignment.MiddleRight)
                Gdk.CairoHelper.SetSourcePixbuf(ctx, img, (img.Width - rec.Width) + rec.X, (img.Height - rec.Height) / 2 + rec.Y);
            else if (ImageAlign == ContentAlignment.BottomLeft)
                Gdk.CairoHelper.SetSourcePixbuf(ctx, img, rec.X, (img.Height - rec.Height) + rec.Y);
            else if (ImageAlign == ContentAlignment.BottomCenter)
                Gdk.CairoHelper.SetSourcePixbuf(ctx, img, (img.Width - rec.Width) / 2 + rec.X, (img.Height - rec.Height) + rec.Y);
            else if (ImageAlign == ContentAlignment.BottomRight)
                Gdk.CairoHelper.SetSourcePixbuf(ctx, img, (img.Width - rec.Width) + rec.X, (img.Height - rec.Height) + rec.Y);
            else
                Gdk.CairoHelper.SetSourcePixbuf(ctx, img, rec.X, rec.Y);

            using (var p = ctx.GetSource())
            {
                if (p is Cairo.SurfacePattern pattern)
                {
                    pattern.Filter = Cairo.Filter.Fast;
                }
            }
            ctx.Paint();
            ctx.Restore();
        }
        public void SetDrawnBackground(Cairo.Context cr, Gdk.Pixbuf image, ImageLayout layoutMode, int width, int height)
        {
            if (layoutMode == ImageLayout.None)
            {
                //从左上角开始原图铺开
                Gdk.CairoHelper.SetSourcePixbuf(cr, image, 0, 0);
            }
            else if (layoutMode == ImageLayout.Stretch)
            { //自由缩放取全图铺满
                Gdk.CairoHelper.SetSourcePixbuf(cr, image.ScaleSimple(width, height, Gdk.InterpType.Nearest), 0, 0);
            }
            else if (layoutMode == ImageLayout.Center)
            {
                //取原图中间
                int offsetx = (width - image.Width) / 2;
                int offsety = (height - image.Height) / 2;
                Gdk.CairoHelper.SetSourcePixbuf(cr, image, offsetx, offsety);
            }
            else if (layoutMode == ImageLayout.Zoom)
            {
                //原图比例缩放，显示全图
                double scaleW = width * 1f / image.Width;
                double scaleH = height * 1f / image.Height;
                if (scaleW > scaleH)
                    Gdk.CairoHelper.SetSourcePixbuf(cr, image.ScaleSimple((int)(image.Width * scaleH), height, Gdk.InterpType.Nearest), 0, 0);
                else
                    Gdk.CairoHelper.SetSourcePixbuf(cr, image.ScaleSimple(width, (int)(image.Height * scaleW), Gdk.InterpType.Nearest), 0, 0);
            }
            else if (layoutMode == ImageLayout.Tile)
            {
                //平铺背景图，原图铺满
                for (int y = 0; y < height; y += image.Height)
                {
                    for (int x = 0; x < width; x += image.Width)
                    {
                        Gdk.CairoHelper.SetSourcePixbuf(cr, image, x, y);
                        cr.Paint();
                    }
                }
            }
            else
            {
                Gdk.CairoHelper.SetSourcePixbuf(cr, image, 0, 0);
            }
            cr.Paint();
        }
    }
}
