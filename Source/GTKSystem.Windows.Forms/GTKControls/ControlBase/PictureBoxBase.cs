
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class PictureBoxBase : Gtk.Viewport, IControlGtk
    {
        private Gdk.Pixbuf _drawImage;
        private Gdk.Pixbuf _image;
        public Gdk.Pixbuf Image
        {
            get => _image;
            set
            {
                _image = value;
                ScaleImage(); 
            }
        }
        private PictureBoxSizeMode _sizeMode = PictureBoxSizeMode.Normal;
        public PictureBoxSizeMode SizeMode { 
            get => _sizeMode; 
            set
            {
                _sizeMode = value;
                ScaleImage();
            }
        }
        private void ScaleImage()
        {
            if (_image != null && this.IsRealized)
                _drawImage = ScaleDrawnSource(_image, _sizeMode, this.AllocatedWidth, this.AllocatedHeight);
            else
                _drawImage = null;
        }
        private Gdk.Pixbuf _drawBackgroundImage;
        private Gdk.Pixbuf _backgroundImage;
        public Gdk.Pixbuf BackgroundImage
        {
            get => _backgroundImage;
            set
            {
                _backgroundImage = value;
                ScaleBackgroundImage();
            }
        }
        private ImageLayout _backgroundImageLayout = ImageLayout.None;
        public ImageLayout BackgroundImageLayout {
            get=> _backgroundImageLayout; 
            set {
                _backgroundImageLayout = value;
                ScaleBackgroundImage();
            }
        }
        private void ScaleBackgroundImage()
        {
            if (_backgroundImage != null && this.IsRealized)
                _drawBackgroundImage = ScaleDrawnBackground(_backgroundImage, _backgroundImageLayout, this.AllocatedWidth, this.AllocatedHeight);
            else
                _drawBackgroundImage = null;
        }
        public Size MaximumSize { get; set; }
        public Size MinimumSize { get; set; }
        public GtkControlOverride Override { get; set; }
        public PictureBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("PictureBox");
            this.BorderWidth = 0;
            this.ShadowType = Gtk.ShadowType.None;
            this.Events = Gdk.EventMask.AllEventsMask;
            this.Realized += PictureBoxBase_Realized;
        }

        private void PictureBoxBase_Realized(object sender, EventArgs e)
        {
            ScaleBackgroundImage();
            ScaleImage();
        }

        protected override void OnShown()
        {
            Override.OnAddClass();
            base.OnShown();
        }
        
        protected override bool OnDrawn(Cairo.Context cr)
        {
            int width = this.AllocatedWidth;
            int height = this.AllocatedHeight;
            Gdk.Rectangle rec = new Gdk.Rectangle(0, 0, width, height);
            if (_drawBackgroundImage != null)
            {
                cr.Save();
                Gdk.CairoHelper.SetSourcePixbuf(cr, _drawBackgroundImage, 0, 0);
                cr.Paint();
                cr.Restore();
            }
            bool returndrawn = base.OnDrawn(cr);
            Override.OnPaint(cr, rec);
            if (_drawImage != null)
            {
                cr.Save();
                Gdk.CairoHelper.SetSourcePixbuf(cr, _drawImage, 0, 0);
                cr.Paint();
                cr.Restore();
            }
            return returndrawn;
        }
        private Gdk.Pixbuf ScaleDrawnSource(Gdk.Pixbuf image, PictureBoxSizeMode sizeMode, int width, int height)
        {
            Gdk.Pixbuf result = null;
            if (sizeMode == PictureBoxSizeMode.Normal)
            {
                //从左上角开始原图铺开
                result = image;
            }
            else if (sizeMode == PictureBoxSizeMode.StretchImage)
            { 
                //自由缩放取全图铺满
                result = image.ScaleSimple(width, height, Gdk.InterpType.Nearest);
            }
            else if (sizeMode == PictureBoxSizeMode.CenterImage)
            {
                //取原图中间
                int offsetx = (width - image.Width) / 2;
                int offsety = (height - image.Height) / 2;
                Cairo.ImageSurface imageSurface = new Cairo.ImageSurface(Cairo.Format.Argb32, width, height);
                result = new Gdk.Pixbuf(imageSurface, 0, 0, width, height);
                int copy_width = offsetx > 0 ? image.Width : width;
                int copy_height = offsety > 0 ? image.Height : height;
                image.CopyArea(Math.Max(0, -offsetx), Math.Max(0, -offsety), copy_width, copy_height, result, Math.Max(0, offsetx), Math.Max(0, offsety));
                imageSurface.Dispose();
            }
            else if (sizeMode == PictureBoxSizeMode.Zoom)
            {
                //原图比例缩放，显示全图
                double scaleW = width * 1f / image.Width;
                double scaleH = height * 1f / image.Height;
                if (scaleW > scaleH)
                {
                    result = image.ScaleSimple((int)(image.Width * scaleH), height, Gdk.InterpType.Nearest);
                }
                else
                {
                    result = image.ScaleSimple(width, (int)(image.Height * scaleW), Gdk.InterpType.Nearest);
                }
            }
            else if (sizeMode == PictureBoxSizeMode.AutoSize)
            {
                //原图不缩放，撑开PictureBox
                if (MaximumSize.Width > 0 && MaximumSize.Height > 0 && image.Width > MaximumSize.Width && image.Height > MaximumSize.Height)
                    result = image.ScaleSimple(MaximumSize.Width, MaximumSize.Height, Gdk.InterpType.Nearest);
                else if (MaximumSize.Width > 0 && image.Width > MaximumSize.Width)
                    result = image.ScaleSimple(MaximumSize.Width, image.Height, Gdk.InterpType.Nearest);
                else if (MaximumSize.Width > 0 && image.Height > MaximumSize.Height)
                    result = image.ScaleSimple(image.Width, MaximumSize.Height, Gdk.InterpType.Nearest);
                else
                    result = image;

                if (MinimumSize.Width > 0 && MinimumSize.Height > 0 && result.Width < MinimumSize.Width && result.Height < MinimumSize.Height)
                    result = result.ScaleSimple(MinimumSize.Width, MinimumSize.Height, Gdk.InterpType.Nearest);
                else if (MinimumSize.Width > 0 && result.Width < MinimumSize.Width)
                    result = result.ScaleSimple(MinimumSize.Width, result.Height, Gdk.InterpType.Nearest);
                else if (MinimumSize.Height > 0 && result.Height < MinimumSize.Height)
                    result = result.ScaleSimple(result.Width, MinimumSize.Height, Gdk.InterpType.Nearest);

                if (this.WidthRequest < result.Width)
                    this.WidthRequest = result.Width;
                if (this.HeightRequest < result.Height)
                    this.HeightRequest = result.Height;
            }
            else if (sizeMode == PictureBoxSizeMode.Tile)
            {
                Cairo.ImageSurface imageSurface = new Cairo.ImageSurface(Cairo.Format.Argb32, width, height);
                result = new Gdk.Pixbuf(imageSurface, 0, 0, width, height);
                int copywidth = Math.Min(width, image.Width);
                int copyheight = Math.Min(height, image.Height);
                //平铺背景图，原图铺满
                for (int y = 0; y < height; y += image.Height)
                {
                    for (int x = 0; x < width; x += image.Width)
                    {
                        image.CopyArea(0, 0, Math.Min(copywidth, width - x), Math.Min(copyheight, height - y), result, x, y);
                    }
                }
                imageSurface.Dispose();
            }
            else
            {
                result = image;
            }

            return result;
        }

        public Gdk.Pixbuf ScaleDrawnBackground(Gdk.Pixbuf image, ImageLayout layoutMode, int width, int height)
        {
            Gdk.Pixbuf result = null;
            if (layoutMode == ImageLayout.None)
            {
                //从左上角开始原图铺开
                result = image;
            }
            else if (layoutMode == ImageLayout.Stretch)
            { //自由缩放取全图铺满
                result = image.ScaleSimple(width, height, Gdk.InterpType.Nearest);
            }
            else if (layoutMode == ImageLayout.Center)
            {
                //取原图中间
                int offsetx = (width - image.Width) / 2;
                int offsety = (height - image.Height) / 2;
                Cairo.ImageSurface imageSurface = new Cairo.ImageSurface(Cairo.Format.Argb32, width, height);
                result = new Gdk.Pixbuf(imageSurface, 0, 0, width, height);
                int copy_width = offsetx > 0 ? image.Width : width;
                int copy_height = offsety > 0 ? image.Height : height;
                image.CopyArea(Math.Max(0, -offsetx), Math.Max(0, -offsety), copy_width, copy_height, result, Math.Max(0, offsetx), Math.Max(0, offsety));
                imageSurface.Dispose();
            }
            else if (layoutMode == ImageLayout.Zoom)
            {
                //原图比例缩放，显示全图
                double scaleW = width * 1f / image.Width;
                double scaleH = height * 1f / image.Height;
                if (scaleW > scaleH)
                {
                    result = image.ScaleSimple((int)(image.Width * scaleH), height, Gdk.InterpType.Nearest);
                }
                else
                {
                    result = image.ScaleSimple(width, (int)(image.Height * scaleW), Gdk.InterpType.Nearest);
                }
            }
            else if (layoutMode == ImageLayout.Tile)
            {
                Cairo.ImageSurface imageSurface = new Cairo.ImageSurface(Cairo.Format.Argb32, width, height);
                result = new Gdk.Pixbuf(imageSurface, 0, 0, width, height);
                int copywidth = Math.Min(width, image.Width);
                int copyheight = Math.Min(height, image.Height);
                //平铺背景图，原图铺满
                for (int y = 0; y < height; y += image.Height)
                {
                    for (int x = 0; x < width; x += image.Width)
                    {
                        image.CopyArea(0, 0, Math.Min(copywidth, width - x), Math.Min(copyheight, height - y), result, x, y);
                    }
                }
                imageSurface.Dispose();
            }
            else
            {
                result = image;
            }

            return result;
        }
    }
}
