using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.Utility
{
    public class ImageUtility
    {
        public static void ScaleImage(int width, int height, ref Gdk.Pixbuf imagePixbuf, byte[] imagebytes, PictureBoxSizeMode sizeMode, ImageLayout backgroundMode)
        {
            if (imagebytes != null)
            {
                Gdk.Pixbuf pix = new Gdk.Pixbuf(imagebytes);
                if (width > 0 && height > 0)
                {
                    using (var surface = new Cairo.ImageSurface(Cairo.Format.ARGB32, width, height))
                    {
                        Gdk.Pixbuf showpix = new Gdk.Pixbuf(surface, 0, 0, width, height);
                        if (sizeMode == PictureBoxSizeMode.Normal && backgroundMode == ImageLayout.None)
                        {
                            pix.CopyArea(0, 0, Math.Min(pix.Width, showpix.Width), Math.Min(pix.Height, showpix.Height), showpix, 0, 0);
                        }
                        else if (sizeMode == PictureBoxSizeMode.StretchImage || backgroundMode == ImageLayout.Stretch)
                        { //缩放取全图铺满
                            Gdk.Pixbuf newpix = pix.ScaleSimple(width, height, Gdk.InterpType.Tiles);
                            newpix.CopyArea(0, 0, newpix.Width, newpix.Height, showpix, 0, 0);
                        }
                        else if (sizeMode == PictureBoxSizeMode.CenterImage || backgroundMode == ImageLayout.Center)
                        {
                            //取原图中间
                            int offsetx = (pix.Width - showpix.Width) / 2;
                            int offsety = (pix.Height - showpix.Height) / 2;
                            pix.CopyArea(offsetx > 0 ? offsetx : 0, offsety > 0 ? offsety : 0, Math.Min(pix.Width, showpix.Width), Math.Min(pix.Height, showpix.Height), showpix, offsetx < 0 ? -offsetx : 0, offsety < 0 ? -offsety : 0);
                        }
                        else if (sizeMode == PictureBoxSizeMode.Zoom || backgroundMode == ImageLayout.Zoom)
                        {
                            if (pix.Width / width > pix.Height / height)
                            {
                                //图片的宽高比大于设置宽高比，以宽为准
                                Gdk.Pixbuf newpix = pix.ScaleSimple(width, width * pix.Height / pix.Width, Gdk.InterpType.Tiles);
                                newpix.CopyArea(0, 0, newpix.Width, newpix.Height, showpix, (showpix.Width - newpix.Width) / 2, (showpix.Height - newpix.Height) / 2);
                            }
                            else
                            {
                                Gdk.Pixbuf newpix = pix.ScaleSimple(height * pix.Width / pix.Height, height, Gdk.InterpType.Tiles);
                                newpix.CopyArea(0, 0, newpix.Width, newpix.Height, showpix, (showpix.Width - newpix.Width) / 2, (showpix.Height - newpix.Height) / 2);
                            }
                        }
                        else if (sizeMode == PictureBoxSizeMode.AutoSize)
                        {
                            pix.CopyArea(0, 0, Math.Min(pix.Width, showpix.Width), Math.Min(pix.Height, showpix.Height), showpix, 0, 0);
                        }

                        if (backgroundMode == ImageLayout.Tile)
                        {
                            //平铺背景图，原图铺满
                            if (pix.Width < width || pix.Height < height)
                            {
                                using (var surface2 = new Cairo.ImageSurface(Cairo.Format.ARGB32, width, height))
                                {
                                    Gdk.Pixbuf backgroundpix = new Gdk.Pixbuf(surface2, 0, 0, width, height);
                                    for (int y = 0; y < height; y += pix.Height)
                                    {
                                        for (int x = 0; x < width; x += pix.Width)
                                        {
                                            pix.CopyArea(0, 0, width - x > pix.Width ? pix.Width : width - x, height - y > pix.Height ? pix.Height : height - y, backgroundpix, x, y);
                                        }
                                    }
                                    imagePixbuf = backgroundpix;
                                }
                            }
                            else
                            {
                                imagePixbuf = showpix;
                            }
                        }
                        else
                        {
                            imagePixbuf = showpix;
                        }
                    }
                }
            }
        }

        public static void DrawBackgroundImage(Cairo.Context ctx, Gdk.Pixbuf img, Gdk.Rectangle rec)
        {
            Gdk.Size size = new Gdk.Size(rec.Width, rec.Height);
            ctx.Save();
            ctx.Translate(4, 4);
            Gdk.CairoHelper.SetSourcePixbuf(ctx, img, 0, 0);
            using (var p = ctx.GetSource())
            {
                if (p is Cairo.SurfacePattern pattern)
                {
                    if (size.Width > img.Width || size.Height > img.Height)
                    {
                        pattern.Filter = Cairo.Filter.Fast;
                    }
                    else
                        pattern.Filter = Cairo.Filter.Good;
                }
            }
            ctx.Paint();
            ctx.Restore();
        }

        /// <summary>
        /// PictureBox图像显示模式
        /// </summary>
        /// <param name="srcImageBytes"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="destImage"></param>
        /// <param name="sizeMode"></param>
        public static void ScaleImageByPictureBoxSizeMode(byte[] srcImageBytes, int width, int height, out Gdk.Pixbuf destImage, PictureBoxSizeMode sizeMode)
        {
            Gdk.Pixbuf srcPixbuf = new Gdk.Pixbuf(srcImageBytes);
            using (var surface = new Cairo.ImageSurface(Cairo.Format.ARGB32, width, height))
            {
                destImage = new Gdk.Pixbuf(surface, 0, 0, width, height);

                if (sizeMode == PictureBoxSizeMode.Normal)
                {
                    //从左上角开始原图铺开，截剪多余
                    srcPixbuf.Scale(destImage, 0, 0, Math.Min(srcPixbuf.Width, destImage.Width), Math.Min(srcPixbuf.Height, destImage.Height), 0, 0, 1, 1, Gdk.InterpType.Tiles);
                }
                else if (sizeMode == PictureBoxSizeMode.StretchImage)
                { //自由缩放取全图铺满
                    destImage = srcPixbuf.ScaleSimple(width, height, Gdk.InterpType.Tiles);
                }
                else if (sizeMode == PictureBoxSizeMode.CenterImage)
                {
                    //取原图中间
                    int offsetx = (destImage.Width- srcPixbuf.Width) / 2;
                    int offsety = (destImage.Height- srcPixbuf.Height) / 2;
                    srcPixbuf.Scale(destImage, 0, 0, destImage.Width, destImage.Height, offsetx, offsety, 1, 1, Gdk.InterpType.Tiles);
                }
                else if (sizeMode == PictureBoxSizeMode.Zoom)
                {
                    //原图比例缩放，显示全图
                    double scaleX = destImage.Width * 1f / srcPixbuf.Width;
                    double scaleY = destImage.Height * 1f / srcPixbuf.Height;
                    double scaleR = Math.Min(scaleX, scaleY);
                    //按最小缩放
                    double srcWidth = scaleX > scaleY ? srcPixbuf.Width * scaleY : srcPixbuf.Width * scaleX;
                    double srcHeight = scaleX > scaleY ? srcPixbuf.Height * scaleY : srcPixbuf.Height * scaleX;

                    int offsetx = (destImage.Width - (int)srcWidth) / 2;
                    int offsety = (destImage.Height - (int)srcHeight) / 2;

                    srcPixbuf.Scale(destImage, offsetx > 0 ? offsetx: 0, offsety > 0 ? offsety : 0, (int)Math.Min(destImage.Width, srcWidth), Math.Min(destImage.Height, (int)srcHeight), offsetx > 0 ? offsetx : 0, offsety > 0 ? offsety : 0, scaleR, scaleR, Gdk.InterpType.Tiles);
                }
                else if (sizeMode == PictureBoxSizeMode.AutoSize)
                {
                    //原图不缩放，撑开PictureBox
                    //destImage = srcPixbuf;
                    srcPixbuf.Scale(destImage, 0, 0, Math.Min(srcPixbuf.Width, destImage.Width), Math.Min(srcPixbuf.Height, destImage.Height), 0, 0, 1, 1, Gdk.InterpType.Tiles);
                }
            }

        }
        /// <summary>
        /// 背景图像显示模式
        /// </summary>
        /// <param name="srcImageBytes"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="destImage"></param>
        /// <param name="layoutMode"></param>
        public static void ScaleImageByImageLayout(byte[] srcImageBytes, int width, int height, out Gdk.Pixbuf destImage, ImageLayout layoutMode)
        {
            Gdk.Pixbuf srcPixbuf = new Gdk.Pixbuf(srcImageBytes);
            using (var surface = new Cairo.ImageSurface(Cairo.Format.ARGB32, width, height))
            {
                destImage = new Gdk.Pixbuf(surface, 0, 0, width, height);

                if (layoutMode == ImageLayout.None)
                {
                    //从左上角开始原图铺开，截剪多余
                    srcPixbuf.Scale(destImage, 0, 0, Math.Min(srcPixbuf.Width, destImage.Width), Math.Min(srcPixbuf.Height, destImage.Height), 0, 0, 1, 1, Gdk.InterpType.Tiles);
                }
                else if (layoutMode == ImageLayout.Stretch)
                { //自由缩放取全图铺满
                    destImage = srcPixbuf.ScaleSimple(width, height, Gdk.InterpType.Tiles);
                }
                else if (layoutMode == ImageLayout.Center)
                {
                    //取原图中间
                    int offsetx = (destImage.Width - srcPixbuf.Width) / 2;
                    int offsety = (destImage.Height - srcPixbuf.Height) / 2;
                    srcPixbuf.Scale(destImage, 0, 0, destImage.Width, destImage.Height, offsetx, offsety, 1, 1, Gdk.InterpType.Tiles);
                }
                else if (layoutMode == ImageLayout.Zoom)
                {
                    //原图比例缩放，显示全图
                    double scaleX = destImage.Width * 1f / srcPixbuf.Width;
                    double scaleY = destImage.Height * 1f / srcPixbuf.Height;
                    double scaleR = Math.Min(scaleX, scaleY);
                    //按最小缩放
                    double srcWidth = scaleX > scaleY ? srcPixbuf.Width * scaleY : srcPixbuf.Width * scaleX;
                    double srcHeight = scaleX > scaleY ? srcPixbuf.Height * scaleY : srcPixbuf.Height * scaleX;

                    int offsetx = (destImage.Width - (int)srcWidth) / 2;
                    int offsety = (destImage.Height - (int)srcHeight) / 2;

                    srcPixbuf.Scale(destImage, offsetx > 0 ? offsetx : 0, offsety > 0 ? offsety : 0, (int)Math.Min(destImage.Width, srcWidth), Math.Min(destImage.Height, (int)srcHeight), offsetx > 0 ? offsetx : 0, offsety > 0 ? offsety : 0, scaleR, scaleR, Gdk.InterpType.Tiles);
                }
                else if (layoutMode == ImageLayout.Tile)
                {
                    //原图不缩放重复直到铺满
                    //平铺背景图，原图铺满
                    if (srcPixbuf.Width < width || srcPixbuf.Height < height)
                    {
                        using (var surface2 = new Cairo.ImageSurface(Cairo.Format.ARGB32, width, height))
                        {
                            Gdk.Pixbuf backgroundpix = new Gdk.Pixbuf(surface2, 0, 0, width, height);
                            for (int y = 0; y < height; y += srcPixbuf.Height)
                            {
                                for (int x = 0; x < width; x += srcPixbuf.Width)
                                {
                                    srcPixbuf.CopyArea(0, 0, width - x > srcPixbuf.Width ? srcPixbuf.Width : width - x, height - y > srcPixbuf.Height ? srcPixbuf.Height : height - y, backgroundpix, x, y);
                                }
                            }
                            destImage = backgroundpix;
                        }
                    }
                    else
                    {
                        srcPixbuf.Scale(destImage, 0, 0, Math.Min(srcPixbuf.Width, destImage.Width), Math.Min(srcPixbuf.Height, destImage.Height), 0, 0, 1, 1, Gdk.InterpType.Tiles);
                    }
                }
            }
        }
    }
}
