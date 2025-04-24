using GTKSystem.Windows.Forms.Utility;
using System.Drawing;
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class PictureBoxBase : Gtk.DrawingArea, IControlGtk
    {
        public Gdk.Pixbuf Image;
        public PictureBoxSizeMode SizeMode;
        public Gdk.Pixbuf BackgroundImage;
        public ImageLayout BackgroundImageLayout;
        public Size MaximumSize { get; set; }
        public Size MinimumSize { get; set; }
        public GtkControlOverride Override { get; set; }
        public PictureBoxBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("PictureBox");
            this.Events = Gdk.EventMask.AllEventsMask;
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
            if (this.BackgroundImage != null)
            {
                Override.SetDrawnBackground(cr, this.BackgroundImage, BackgroundImageLayout, width, height);
            }

            if (this.Image != null)
            {
                DrawnSource(cr, this.Image, SizeMode, width, height);
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
    }
}
