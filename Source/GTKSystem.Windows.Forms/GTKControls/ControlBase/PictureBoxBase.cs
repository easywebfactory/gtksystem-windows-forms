using GLib;
using GTKSystem.Windows.Forms.Utility;
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
                Gdk.CairoHelper.SetSourcePixbuf(cr, _drawBackgroundImage, 0, 0);
                cr.Paint();
            }
            if (_drawImage != null)
            {
                Gdk.CairoHelper.SetSourcePixbuf(cr, _drawImage, 0, 0);
                cr.Paint();
            }

            Override.OnPaint(cr, rec);

            return base.OnDrawn(cr);
        }
        private void DrawnSource(Cairo.Context cr, Gdk.Pixbuf image, PictureBoxSizeMode sizeMode, int width,int height)
        {
            if (sizeMode == PictureBoxSizeMode.Normal)
            {
                //从左上角开始原图铺开
                Gdk.CairoHelper.SetSourcePixbuf(cr, image, 0, 0);
            }
            else if (sizeMode == PictureBoxSizeMode.StretchImage)
            { //自由缩放取全图铺满
                Gdk.CairoHelper.SetSourcePixbuf(cr, image.ScaleSimple(width, height, Gdk.InterpType.Nearest), 0, 0);
            }
            else if (sizeMode == PictureBoxSizeMode.CenterImage)
            {
                //取原图中间
                int offsetx = (width - image.Width) / 2;
                int offsety = (height - image.Height) / 2;
                Gdk.CairoHelper.SetSourcePixbuf(cr, image, offsetx, offsety);
            }
            else if (sizeMode == PictureBoxSizeMode.Zoom)
            {
                //原图比例缩放，显示全图
                double scaleW = width * 1f / image.Width;
                double scaleH = height * 1f / image.Height;
                if (scaleW > scaleH)
                    Gdk.CairoHelper.SetSourcePixbuf(cr, image.ScaleSimple((int)(image.Width * scaleH), height, Gdk.InterpType.Nearest), 0, 0);
                else
                    Gdk.CairoHelper.SetSourcePixbuf(cr, image.ScaleSimple(width, (int)(image.Height * scaleW), Gdk.InterpType.Nearest), 0, 0);
            }
            else if (sizeMode == PictureBoxSizeMode.AutoSize)
            {
                //原图不缩放，撑开PictureBox
                if (this.WidthRequest < image.Width)
                    this.WidthRequest = image.Width;
                if (this.HeightRequest < image.Height)
                    this.HeightRequest = image.Height;
                if (this.WidthRequest < MinimumSize.Width)
                    this.WidthRequest = MinimumSize.Width;
                if (this.HeightRequest < MinimumSize.Height)
                    this.HeightRequest = MinimumSize.Height;
                if (this.WidthRequest > MaximumSize.Width)
                    this.WidthRequest = MaximumSize.Width;
                if (this.HeightRequest > MaximumSize.Height)
                    this.HeightRequest = MaximumSize.Height;

                Gdk.CairoHelper.SetSourcePixbuf(cr, image, 0, 0);
            }
            else if (sizeMode == PictureBoxSizeMode.Tile)
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

        private Gdk.Pixbuf ScaleDrawnSource(Gdk.Pixbuf image, PictureBoxSizeMode sizeMode, int width, int height)
        {
            Gdk.Pixbuf result = null;
            if (sizeMode == PictureBoxSizeMode.Normal)
            {
                //从左上角开始原图铺开
                result = image;
            }
            else if (sizeMode == PictureBoxSizeMode.StretchImage)
            { //自由缩放取全图铺满

                result = image.ScaleSimple(width, height, Gdk.InterpType.Nearest);
            }
            else if (sizeMode == PictureBoxSizeMode.CenterImage)
            {
                //取原图中间
                int offsetx = (width - image.Width) / 2;
                int offsety = (height - image.Height) / 2;
                Cairo.ImageSurface imageSurface = new Cairo.ImageSurface(Cairo.Format.Argb32, width, height);
                Cairo.Surface imageSurface2 = imageSurface.CreateSimilar(Cairo.Content.ColorAlpha, width, height);
                result = new Gdk.Pixbuf(imageSurface2, 0, 0, width, height);
                image.CopyArea(Math.Max(0, -offsetx), Math.Max(0, -offsety), width, height, result, Math.Max(0, offsetx), Math.Max(0, offsety));
                imageSurface2.Dispose();
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
                if (this.WidthRequest < image.Width)
                    this.WidthRequest = image.Width;
                if (this.HeightRequest < image.Height)
                    this.HeightRequest = image.Height;
                if (this.WidthRequest < MinimumSize.Width)
                    this.WidthRequest = MinimumSize.Width;
                if (this.HeightRequest < MinimumSize.Height)
                    this.HeightRequest = MinimumSize.Height;
                if (this.WidthRequest > MaximumSize.Width)
                    this.WidthRequest = MaximumSize.Width;
                if (this.HeightRequest > MaximumSize.Height)
                    this.HeightRequest = MaximumSize.Height;

                result = image;
            }
            else if (sizeMode == PictureBoxSizeMode.Tile)
            {
                Cairo.ImageSurface imageSurface = new Cairo.ImageSurface(Cairo.Format.Argb32, width, height);
                Cairo.Surface imageSurface2 = imageSurface.CreateSimilar(Cairo.Content.ColorAlpha, width, height);
                result = new Gdk.Pixbuf(imageSurface2, 0, 0, width, height);

                //平铺背景图，原图铺满
                for (int y = 0; y < height; y += image.Height)
                {
                    for (int x = 0; x < width; x += image.Width)
                    {
                        image.CopyArea(0, 0, width, height, result, x, y);
                    }
                }
                imageSurface2.Dispose();
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
                Cairo.Surface imageSurface2 = imageSurface.CreateSimilar(Cairo.Content.ColorAlpha, width, height);
                result = new Gdk.Pixbuf(imageSurface2, 0, 0, width, height);
                image.CopyArea(Math.Max(0, -offsetx), Math.Max(0, -offsety), width, height, result, Math.Max(0, offsetx), Math.Max(0, offsety));
                imageSurface2.Dispose();
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
                Cairo.Surface imageSurface2 = imageSurface.CreateSimilar(Cairo.Content.ColorAlpha, width, height);
                result = new Gdk.Pixbuf(imageSurface2, 0, 0, width, height);

                //平铺背景图，原图铺满
                for (int y = 0; y < height; y += image.Height)
                {
                    for (int x = 0; x < width; x += image.Width)
                    {
                        image.CopyArea(0, 0, width, height, result, x, y);
                    }
                }
                imageSurface2.Dispose();
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
