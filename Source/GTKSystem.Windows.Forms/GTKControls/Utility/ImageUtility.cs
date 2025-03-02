using System.Drawing;
using Cairo;
using Gdk;

namespace System.Windows.Forms;

public class ImageUtility
{
    public static void ScaleImage(int width, int height, ref Pixbuf imagePixbuf, byte[] imagebytes, PictureBoxSizeMode sizeMode, ImageLayout backgroundMode)
    {
        if (imagebytes != null)
        {
            var pix = new Pixbuf(imagebytes);
            if (width > 0 && height > 0)
            {
                using var surface = new ImageSurface(Format.ARGB32, width, height);
                var showpix = new Pixbuf(surface, 0, 0, width, height);
                if (sizeMode == PictureBoxSizeMode.Normal && backgroundMode == ImageLayout.None)
                {
                    pix.CopyArea(0, 0, Math.Min(pix.Width, showpix.Width), Math.Min(pix.Height, showpix.Height), showpix, 0, 0);
                }
                else if (sizeMode == PictureBoxSizeMode.StretchImage || backgroundMode == ImageLayout.Stretch)
                { //缩放取全图铺满
                    var newpix = pix.ScaleSimple(width, height, InterpType.Tiles);
                    newpix.CopyArea(0, 0, newpix.Width, newpix.Height, showpix, 0, 0);
                }
                else if (sizeMode == PictureBoxSizeMode.CenterImage || backgroundMode == ImageLayout.Center)
                {
                    //取原图中间
                    var offsetx = (pix.Width - showpix.Width) / 2;
                    var offsety = (pix.Height - showpix.Height) / 2;
                    pix.CopyArea(offsetx > 0 ? offsetx : 0, offsety > 0 ? offsety : 0, Math.Min(pix.Width, showpix.Width), Math.Min(pix.Height, showpix.Height), showpix, offsetx < 0 ? -offsetx : 0, offsety < 0 ? -offsety : 0);
                }
                else if (sizeMode == PictureBoxSizeMode.Zoom || backgroundMode == ImageLayout.Zoom)
                {
                    if (pix.Width / width > pix.Height / height)
                    {
                        //图片的宽高比大于设置宽高比，以宽为准
                        var newpix = pix.ScaleSimple(width, width * pix.Height / pix.Width, InterpType.Tiles);
                        newpix.CopyArea(0, 0, newpix.Width, newpix.Height, showpix, (showpix.Width - newpix.Width) / 2, (showpix.Height - newpix.Height) / 2);
                    }
                    else
                    {
                        var newpix = pix.ScaleSimple(height * pix.Width / pix.Height, height, InterpType.Tiles);
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
                        using var surface2 = new ImageSurface(Format.ARGB32, width, height);
                        var backgroundpix = new Pixbuf(surface2, 0, 0, width, height);
                        for (var y = 0; y < height; y += pix.Height)
                        {
                            for (var x = 0; x < width; x += pix.Width)
                            {
                                pix.CopyArea(0, 0, width - x > pix.Width ? pix.Width : width - x, height - y > pix.Height ? pix.Height : height - y, backgroundpix, x, y);
                            }
                        }
                        imagePixbuf = backgroundpix;
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

    public static void DrawBackgroundImage(Context ctx, Pixbuf img, Gdk.Rectangle rec)
    {
        ctx.Save();
        ctx.ResetClip();
        ctx.Rectangle(rec.X, rec.Y, rec.Width, rec.Height);
        ctx.Clip();
        ctx.Translate(rec.Left, rec.Top);
        CairoHelper.SetSourcePixbuf(ctx, img, 0, 0);
        using (var p = ctx.GetSource())
        {
            if (p is SurfacePattern pattern)
            {
                pattern.Filter = Filter.Fast;
            }
        }
        ctx.Paint();
        ctx.Restore();
    }
    public static void DrawImage(Context? ctx, Pixbuf? img, Gdk.Rectangle rec, ContentAlignment imageAlign)
    {
        if (ctx != null && img != null)
        {
            ctx.Save();
            if (imageAlign == ContentAlignment.TopLeft)
                ctx.Translate(rec.X, rec.Y);
            else
            {
                var imgWidth = (img.Width - rec.Width) / 2;
                if (imageAlign == ContentAlignment.TopCenter)
                    ctx.Translate(imgWidth + rec.X, rec.Y);
                else if (imageAlign == ContentAlignment.TopRight)
                    ctx.Translate(img.Width - rec.Width + rec.X, rec.Y);
                else
                {
                    var imgHeight = (img.Height - rec.Height) / 2;
                    if (imageAlign == ContentAlignment.MiddleLeft)
                        ctx.Translate(rec.X, imgHeight + rec.Y);
                    else if (imageAlign == ContentAlignment.MiddleCenter)
                        ctx.Translate(imgWidth + rec.X, imgHeight + rec.Y);
                    else if (imageAlign == ContentAlignment.MiddleRight)
                        ctx.Translate(img.Width - rec.Width + rec.X, imgHeight + rec.Y);
                    else if (imageAlign == ContentAlignment.BottomLeft)
                        ctx.Translate(rec.X, img.Height - rec.Height + rec.Y);
                    else if (imageAlign == ContentAlignment.BottomCenter)
                        ctx.Translate(imgWidth + rec.X, img.Height - rec.Height + rec.Y);
                    else if (imageAlign == ContentAlignment.BottomRight)
                        ctx.Translate(img.Width - rec.Width + rec.X, img.Height - rec.Height + rec.Y);
                    else
                        ctx.Translate(rec.X, rec.Y);
                }
            }

            CairoHelper.SetSourcePixbuf(ctx, img, 0, 0);
            using (var p = ctx.GetSource())
            {
                if (p is SurfacePattern pattern)
                {
                    pattern.Filter = Filter.Fast;
                }
            }

            ctx.Paint();
            ctx.Restore();
        }
    }
    /// <summary>
    /// PictureBox图像显示模式
    /// </summary>
    /// <param name="srcImageBytes"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="destImage"></param>
    /// <param name="sizeMode"></param>
    public static void ScaleImageByPictureBoxSizeMode(byte[] srcImageBytes, int width, int height, out Pixbuf destImage, PictureBoxSizeMode sizeMode)
    {
        var srcPixbuf = new Pixbuf(srcImageBytes);
        ScaleImageByPictureBoxSizeMode(srcPixbuf, width, height, out destImage, sizeMode);

    }
    /// <summary>
    /// PictureBox图像显示模式
    /// </summary>
    /// <param name="srcPixbuf"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="destImage"></param>
    /// <param name="sizeMode"></param>
    public static void ScaleImageByPictureBoxSizeMode(Pixbuf? srcPixbuf, int width, int height, out Pixbuf destImage, PictureBoxSizeMode sizeMode)
    {
        using var surface = new ImageSurface(Format.ARGB32, width, height);
        destImage = new Pixbuf(surface, 0, 0, width, height);

        if (sizeMode == PictureBoxSizeMode.Normal)
        {
            //从左上角开始原图铺开，截剪多余
            srcPixbuf?.Scale(destImage, 0, 0, Math.Min(srcPixbuf.Width, destImage.Width), Math.Min(srcPixbuf.Height, destImage.Height), 0, 0, 1, 1, InterpType.Tiles);
        }
        else if (sizeMode == PictureBoxSizeMode.StretchImage)
        {
            //自由缩放取全图铺满
            if (srcPixbuf != null)
            {
                destImage = srcPixbuf.ScaleSimple(width, height, InterpType.Tiles);
            }
        }
        else if (sizeMode == PictureBoxSizeMode.CenterImage)
        {
            //取原图中间
            var offsetx = (destImage.Width - srcPixbuf?.Width??0) / 2;
            var offsety = (destImage.Height - srcPixbuf?.Height??0) / 2;
            srcPixbuf?.Scale(destImage, 0, 0, destImage.Width, destImage.Height, offsetx, offsety, 1, 1, InterpType.Tiles);
        }
        else if (sizeMode == PictureBoxSizeMode.Zoom)
        {
            //原图比例缩放，显示全图
            double scaleX = destImage.Width * 1f / (srcPixbuf?.Width??0);
            double scaleY = destImage.Height * 1f / (srcPixbuf?.Height??0);
            var scaleR = Math.Min(scaleX, scaleY);
            //按最小缩放
            var srcWidth = scaleX > scaleY ? (srcPixbuf?.Width??0) * scaleY : (srcPixbuf?.Width??0) * scaleX;
            var srcHeight = scaleX > scaleY ? (srcPixbuf?.Height ?? 0) * scaleY : (srcPixbuf?.Height ?? 0) * scaleX;

            var offsetx = (destImage.Width - (int)srcWidth) / 2;
            var offsety = (destImage.Height - (int)srcHeight) / 2;

            srcPixbuf?.Scale(destImage, offsetx > 0 ? offsetx : 0, offsety > 0 ? offsety : 0, (int)Math.Min(destImage.Width, srcWidth), Math.Min(destImage.Height, (int)srcHeight), offsetx > 0 ? offsetx : 0, offsety > 0 ? offsety : 0, scaleR, scaleR, InterpType.Tiles);
        }
        else if (sizeMode == PictureBoxSizeMode.AutoSize)
        {
            //原图不缩放，撑开PictureBox
            //destImage = srcPixbuf;
            srcPixbuf?.Scale(destImage, 0, 0, Math.Min(srcPixbuf.Width, destImage.Width), Math.Min(srcPixbuf.Height, destImage.Height), 0, 0, 1, 1, InterpType.Tiles);
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
    public static void ScaleImageByImageLayout(byte[]? srcImageBytes, int width, int height, out Pixbuf? destImage, ImageLayout layoutMode)
    {
        var srcPixbuf = new Pixbuf(srcImageBytes);
        ScaleImageByImageLayout(srcPixbuf, width, height, out destImage, layoutMode);
    }

    /// <summary>
    /// 背景图像显示模式
    /// </summary>
    /// <param name="srcPixbuf"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="destImage"></param>
    /// <param name="layoutMode"></param>
    public static void ScaleImageByImageLayout(Pixbuf srcPixbuf, int width, int height, out Pixbuf? destImage, ImageLayout layoutMode)
    {
        using var surface = new ImageSurface(Format.ARGB32, width, height);
        destImage = new Pixbuf(surface, 0, 0, width, height);

        if (layoutMode == ImageLayout.None)
        {
            //从左上角开始原图铺开，截剪多余
            srcPixbuf.Scale(destImage, 0, 0, Math.Min(srcPixbuf.Width, destImage.Width), Math.Min(srcPixbuf.Height, destImage.Height), 0, 0, 1, 1, InterpType.Tiles);
        }
        else if (layoutMode == ImageLayout.Stretch)
        { //自由缩放取全图铺满
            destImage = srcPixbuf.ScaleSimple(width, height, InterpType.Tiles);
        }
        else if (layoutMode == ImageLayout.Center)
        {
            //取原图中间
            var offsetx = (destImage.Width - srcPixbuf.Width) / 2;
            var offsety = (destImage.Height - srcPixbuf.Height) / 2;
            srcPixbuf.Scale(destImage, 0, 0, destImage.Width, destImage.Height, offsetx, offsety, 1, 1, InterpType.Tiles);
        }
        else if (layoutMode == ImageLayout.Zoom)
        {
            //原图比例缩放，显示全图
            double scaleX = destImage.Width * 1f / srcPixbuf.Width;
            double scaleY = destImage.Height * 1f / srcPixbuf.Height;
            var scaleR = Math.Min(scaleX, scaleY);
            //按最小缩放
            var srcWidth = scaleX > scaleY ? srcPixbuf.Width * scaleY : srcPixbuf.Width * scaleX;
            var srcHeight = scaleX > scaleY ? srcPixbuf.Height * scaleY : srcPixbuf.Height * scaleX;

            var offsetx = (destImage.Width - (int)srcWidth) / 2;
            var offsety = (destImage.Height - (int)srcHeight) / 2;

            srcPixbuf.Scale(destImage, offsetx > 0 ? offsetx : 0, offsety > 0 ? offsety : 0, (int)Math.Min(destImage.Width, srcWidth), Math.Min(destImage.Height, (int)srcHeight), offsetx > 0 ? offsetx : 0, offsety > 0 ? offsety : 0, scaleR, scaleR, InterpType.Tiles);
        }
        else if (layoutMode == ImageLayout.Tile)
        {
            //原图不缩放重复直到铺满
            //平铺背景图，原图铺满
            if (srcPixbuf.Width < width || srcPixbuf.Height < height)
            {
                using var surface2 = new ImageSurface(Format.ARGB32, width, height);
                var backgroundpix = new Pixbuf(surface2, 0, 0, width, height);
                for (var y = 0; y < height; y += srcPixbuf.Height)
                {
                    for (var x = 0; x < width; x += srcPixbuf.Width)
                    {
                        srcPixbuf.CopyArea(0, 0, width - x > srcPixbuf.Width ? srcPixbuf.Width : width - x, height - y > srcPixbuf.Height ? srcPixbuf.Height : height - y, backgroundpix, x, y);
                    }
                }
                destImage = backgroundpix;
            }
            else
            {
                srcPixbuf.Scale(destImage, 0, 0, Math.Min(srcPixbuf.Width, destImage.Width), Math.Min(srcPixbuf.Height, destImage.Height), 0, 0, 1, 1, InterpType.Tiles);
            }
        }
    }
}