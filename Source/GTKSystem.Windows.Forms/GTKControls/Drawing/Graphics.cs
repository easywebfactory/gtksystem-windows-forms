using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;


namespace System.Drawing
{
	public sealed class Graphics : MarshalByRefObject, IDeviceContext, IDisposable
	{
		private Cairo.Context context;
		private Gdk.Rectangle rectangle;
		private Gtk.Widget widget;
        #region 用于输入与输出的数值调整差值
        internal double diff_left { get; set; }
        internal double diff_top { get; set; }
        //internal int diff_right { get; set; }
        //internal int diff_bottom { get; set; }
        #endregion
        internal Graphics(Gtk.Widget widget, Cairo.Context context, Gdk.Rectangle rectangle)
		{
			this.widget = widget;
			this.context = context;
			this.rectangle = rectangle;
			this.Clip = new Region(new Rectangle(this.rectangle.X, this.rectangle.Y, this.rectangle.Width, this.rectangle.Height));
		}

		public delegate bool DrawImageAbort(IntPtr callbackdata);

		public delegate bool EnumerateMetafileProc(EmfPlusRecordType recordType, int flags, int dataSize, IntPtr data, PlayRecordCallback callbackData);

		public Region Clip
		{
			get;
			set;
		}

		public RectangleF ClipBounds
		{
			get
			{
				return new RectangleF(this.rectangle.X, this.rectangle.Y, this.rectangle.Width, this.rectangle.Height);
			}
		}

		public CompositingMode CompositingMode
		{
			get;
			set;
		}

		public CompositingQuality CompositingQuality
		{
			get;
			set;
		}

		public float DpiX
		{
			get;
		}

		public float DpiY
		{
			get;
		}

		public InterpolationMode InterpolationMode
		{
			get;
			set;
		}

		public bool IsClipEmpty
		{
			get
			{
				return false;
			}
		}

		public bool IsVisibleClipEmpty
		{
			get;
		}

		public float PageScale
		{
            get;
            set;
        }

		public GraphicsUnit PageUnit
		{
			get;
			set;
		}

		public PixelOffsetMode PixelOffsetMode
		{
			get;
			set;
		}

		public Point RenderingOrigin
		{
            get;
            set;
        }

		public SmoothingMode SmoothingMode
		{
			get;
			set;
		}

		public int TextContrast
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		public TextRenderingHint TextRenderingHint
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		public Matrix Transform
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		public RectangleF VisibleClipBounds
		{
            get
            {
                return new RectangleF(this.rectangle.X, this.rectangle.Y, this.rectangle.Width, this.rectangle.Height);
            }
        }

		public void AddMetafileComment(byte[] data)
		{
		}

		public GraphicsContainer BeginContainer()
		{
			throw null;
		}

		public GraphicsContainer BeginContainer(Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit)
		{
			throw null;
		}

		public GraphicsContainer BeginContainer(RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit)
		{
			throw null;
		}
		internal void ContextTranslateWithDifference(double x,double y)
		{
            this.context.Translate(diff_left + x, diff_top + y);
        }
		public void Clear(Color color)
		{
            this.context.Save();
			this.context.SetSourceRGB(color.R / 255f, color.G / 255f, color.B / 255f);
            this.ContextTranslateWithDifference(0, 0);
            this.context.Rectangle(this.rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
			this.context.Fill();
			this.context.Paint();
			this.context.Restore();
		}

		public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize)
		{
		}

		public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
		}

		public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize)
		{
		}

		public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
		}

		public void Dispose()
		{
		}
        private void DrawArcCore(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            this.context.Save();
            this.ContextTranslateWithDifference(0, 0);
            this.context.LineWidth = pen.Width;
            this.context.LineJoin = Cairo.LineJoin.Round;
            this.context.SetSourceRGB(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f);
            this.context.Arc(x + width / 2, y + height / 2, Math.Min(width / 2, height / 2), startAngle, sweepAngle);
            //this.context.ArcNegative(x + width / 2, y + height / 2, Math.Min(width / 2, height / 2), startAngle, sweepAngle); //相反位置
            this.context.Stroke();
            this.context.Restore();
        }
        public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
		{
            DrawArcCore(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
		{
            DrawArcCore(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
            DrawArcCore(pen, x, y, width, height, startAngle, sweepAngle);
        }

		public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
            DrawArcCore(pen, x, y, width, height, startAngle, sweepAngle);
        }

		public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
		{
		}

		public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
		{

		}

		public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
		{

		}

		public void DrawBeziers(Pen pen, PointF[] points)
		{
		}

		public void DrawBeziers(Pen pen, Point[] points)
		{
		}
        private void DrawCurveCore(bool isClosePath, bool isfill, Pen pen, PointF[] points, int offset, int numberOfSegments, float tension, FillMode fillmode)
        {
            if (points.Length > 1)
            {
                this.context.Save();
                this.ContextTranslateWithDifference(offset, offset);
                this.context.SetSourceRGB(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f);
                this.context.LineWidth = pen.Width;
                this.context.NewPath();
                this.context.CurveTo(points[0].X, points[0].Y, points[1].X, points[1].Y, points[2].X, points[2].Y);
                if (isClosePath)
                    this.context.ClosePath();
                if (isfill)
                {
                    this.context.FillRule = fillmode == FillMode.Winding ? Cairo.FillRule.Winding : Cairo.FillRule.EvenOdd;
                    this.context.Fill();
                }
                else
                    this.context.Stroke();

                this.context.Restore();
            }
        }
        public void DrawClosedCurve(Pen pen, PointF[] points)
		{
            DrawCurveCore(true, false, pen, points, 0, 0, 0, FillMode.Winding);
        }

		public void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillmode)
		{
            DrawCurveCore(true, false, pen, Array.ConvertAll(points, p => new PointF(p.X, p.Y)), 0, 0, tension, fillmode);
        }

		public void DrawClosedCurve(Pen pen, Point[] points)
		{
            DrawCurveCore(true, false, pen, Array.ConvertAll(points, p => new PointF(p.X, p.Y)), 0, 0, 0, FillMode.Winding);
        }

		public void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillmode)
        {
            DrawCurveCore(true, false, pen, Array.ConvertAll(points, p => new PointF(p.X, p.Y)), 0, 0, tension, fillmode);
        }

		public void DrawCurve(Pen pen, PointF[] points)
		{
            DrawCurveCore(false, false, pen, points, 0, 0, 0, FillMode.Winding);
        }

		public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments)
		{
            DrawCurveCore(false, false, pen, points, offset, numberOfSegments, 0, FillMode.Winding);
        }

		public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension)
        {
            DrawCurveCore(false,false, pen, points, offset, numberOfSegments, tension, FillMode.Winding);
        }
       
        public void DrawCurve(Pen pen, PointF[] points, float tension)
		{
            DrawCurveCore(false, false, pen, points, 0, 0, tension, FillMode.Winding);
        }

		public void DrawCurve(Pen pen, Point[] points)
		{
            DrawCurveCore(false, false, pen, Array.ConvertAll(points,p=> new PointF(p.X, p.Y)), 0, 0, 0, FillMode.Winding);
        }

		public void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension)
		{
            DrawCurveCore(false, false, pen, Array.ConvertAll(points, p => new PointF(p.X, p.Y)), offset, numberOfSegments, tension, FillMode.Winding);
        }

		public void DrawCurve(Pen pen, Point[] points, float tension)
		{
            DrawCurveCore(false, false, pen, Array.ConvertAll(points, p => new PointF(p.X, p.Y)), 0, 0, tension, FillMode.Winding);
        }

		public void DrawEllipse(Pen pen, Rectangle rect)
        {
            DrawEllipseCore(pen, rect.X, rect.Y, rect.Width, rect.Height, false, FillMode.Winding);
        }

		public void DrawEllipse(Pen pen, RectangleF rect)
		{
            DrawEllipseCore(pen, rect.X, rect.Y, rect.Width, rect.Height, false, FillMode.Winding);
        }

		public void DrawEllipse(Pen pen, int x, int y, int width, int height)
		{
            DrawEllipseCore(pen, x, y, width, height, false, FillMode.Winding);
        }

		public void DrawEllipse(Pen pen, float x, float y, float width, float height)
		{
			DrawEllipseCore(pen, x, y, width, height, false, FillMode.Winding);
        }
        public void DrawEllipseCore(Pen pen, float x, float y, float width, float height, bool isfill, FillMode fillmode)
        {
            this.context.Save();
            this.ContextTranslateWithDifference(x + width, y + height);
            this.context.SetSourceRGB(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f);
            this.context.LineWidth = pen.Width;
            this.context.LineJoin = Cairo.LineJoin.Round;
            this.context.NewPath();
			float r = (width + height) / 4;
			double rs = Math.Min(0.1, 2 / r);
            for (double t = 0; t < 2 * Math.PI; t += rs)
            {
                double x2_1 = width * Math.Cos(t);
                double y2_1 = height * Math.Sin(t);
                this.context.LineTo(x2_1, y2_1);
            }
            this.context.ClosePath();
			if (isfill)
			{
				this.context.FillRule = fillmode == FillMode.Winding ? Cairo.FillRule.Winding : Cairo.FillRule.EvenOdd;
				this.context.Fill();
			}
			else
			{
				this.context.Stroke();
			}
			this.context.Restore();

        }
        public void DrawIcon(Icon icon, Rectangle targetRect)
		{
            DrawImage(new Bitmap(icon.PixbufData), targetRect);
        }

		public void DrawIcon(Icon icon, int x, int y)
		{
            DrawImage(new Bitmap(icon.PixbufData), new Point(x,y));
        }

		public void DrawIconUnstretched(Icon icon, Rectangle targetRect)
		{
            DrawImage(new Bitmap(icon.PixbufData), targetRect);
        }

		public void DrawImage(Image image, Point point)
		{
            DrawImageScaledCore(image, new Rectangle(0, 0, image.Width, image.Height), point.X, point.Y, image.Width, image.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, PointF point)
		{
            DrawImageScaledCore(image, new Rectangle(0, 0, image.Width, image.Height), point.X, point.Y, image.Width, image.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, PointF[] destPoints)
		{
            DrawImageScaledCore(image, new Rectangle((int)destPoints[0].X, (int)destPoints[0].Y, (int)destPoints[1].X - (int)destPoints[0].X, (int)destPoints[2].Y - (int)destPoints[0].Y), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
		}
        private void DrawImageUnscaledCore(Image image, int x, int y, int width, int height, bool clipped = false)
        {
            Gdk.Pixbuf img = new Gdk.Pixbuf(image.PixbufData);
            if (width == 0)
                width = img.Width;
            if (height == 0)
                height = img.Height;
			using (var surface = new Cairo.ImageSurface(Cairo.Format.Argb32, width, height))
			{
				Gdk.Pixbuf newimg = new Gdk.Pixbuf(surface, 0, 0, width, height);
				img.CopyArea(x, y, width, height, newimg, 0, 0);
				this.context.Save();
				this.ContextTranslateWithDifference(x, y);
				Gdk.CairoHelper.SetSourcePixbuf(this.context, newimg, x, y);

				using (var p = this.context.GetSource())
				{
					if (p is Cairo.SurfacePattern pattern)
					{
						if (this.CompositingQuality == CompositingQuality.HighSpeed)
						{
							pattern.Filter = Cairo.Filter.Fast;
						}
						else if (this.CompositingQuality == CompositingQuality.HighQuality)
						{
							pattern.Filter = Cairo.Filter.Good;
						}
						else
							pattern.Filter = Cairo.Filter.Best;
					}
				}
				this.context.Paint();
				this.context.Restore();
			}
        }
        private void DrawImageScaledCore(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
        {
            Gdk.Pixbuf img = new Gdk.Pixbuf(image.PixbufData);
            if (srcWidth == 0)
                srcWidth = img.Width;
            if (srcHeight == 0)
                srcHeight = img.Height;
            if (destRect.Width == 0)
                destRect.Width = img.Width;
            if (destRect.Height == 0)
                destRect.Height = img.Height;
            using (var surface = new Cairo.ImageSurface(Cairo.Format.Argb32, destRect.Width, destRect.Height))
            {
                Gdk.Pixbuf scaleimg = new Gdk.Pixbuf(surface, 0, 0, destRect.Width, destRect.Height);

                img.Scale(scaleimg, 0, 0, destRect.Width, destRect.Height, srcX, srcY, destRect.Width / srcWidth, destRect.Height / srcHeight, Gdk.InterpType.Tiles);
                this.context.Save();
                this.ContextTranslateWithDifference(destRect.X, destRect.Y);
                Gdk.CairoHelper.SetSourcePixbuf(this.context, scaleimg, 0, 0);
                using (var p = this.context.GetSource())
                {
                    if (p is Cairo.SurfacePattern pattern)
                    {
                        if (this.CompositingQuality == CompositingQuality.HighSpeed)
                        {
                            pattern.Filter = Cairo.Filter.Fast;
                        }
                        else if (this.CompositingQuality == CompositingQuality.HighQuality)
                        {
                            pattern.Filter = Cairo.Filter.Good;
                        }
                        else
                            pattern.Filter = Cairo.Filter.Best;
                    }
                }

                this.context.Paint();
                this.context.Restore();

            }

        }
        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)
		{
            DrawImageScaledCore(image, new Rectangle((int)destPoints[0].X, (int)destPoints[0].Y, (int)destPoints[1].X - (int)destPoints[0].X, (int)destPoints[2].Y - (int)destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
		{
            DrawImageScaledCore(image, new Rectangle((int)destPoints[0].X, (int)destPoints[0].Y, (int)destPoints[1].X - (int)destPoints[0].X, (int)destPoints[2].Y - (int)destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
		{
            DrawImageScaledCore(image, new Rectangle((int)destPoints[0].X, (int)destPoints[0].Y, (int)destPoints[1].X - (int)destPoints[0].X, (int)destPoints[2].Y - (int)destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, callback, IntPtr.Zero);
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData)
		{
            DrawImageScaledCore(image, new Rectangle((int)destPoints[0].X, (int)destPoints[0].Y, (int)destPoints[1].X - (int)destPoints[0].X, (int)destPoints[2].Y - (int)destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, callback, new IntPtr(callbackData));
        }

		public void DrawImage(Image image, Point[] destPoints)
		{
            DrawImageScaledCore(image, new Rectangle(destPoints[0].X, destPoints[0].Y, destPoints[1].X - destPoints[0].X, destPoints[2].Y - destPoints[0].Y), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)
		{
            DrawImageScaledCore(image, new Rectangle(destPoints[0].X, destPoints[0].Y, destPoints[1].X - destPoints[0].X, destPoints[2].Y - destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
		{
            DrawImageScaledCore(image, new Rectangle(destPoints[0].X, destPoints[0].Y, destPoints[1].X - destPoints[0].X, destPoints[2].Y - destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
        {
            DrawImageScaledCore(image, new Rectangle(destPoints[0].X, destPoints[0].Y, destPoints[1].X - destPoints[0].X, destPoints[2].Y - destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, callback, IntPtr.Zero);
		}

		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData)
		{
            DrawImageScaledCore(image, new Rectangle(destPoints[0].X, destPoints[0].Y, destPoints[1].X- destPoints[0].X, destPoints[2].Y- destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, callback, new IntPtr(callbackData));
        }

		public void DrawImage(Image image, Rectangle rect)
		{
            DrawImageScaledCore(image, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)
		{
            DrawImageScaledCore(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit)
		{
            DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
		{
            DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback)
		{
            DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
		{
            DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, callbackData);
		}

		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit)
		{
            DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
		{
            DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback)
		{
            DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
        {
            DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, callbackData);
		}

		public void DrawImage(Image image, RectangleF rect)
        {
            DrawImageScaledCore(image, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height), rect.X, rect.Y, rect.Width, rect.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
		{
            DrawImageScaledCore(image, new Rectangle((int)destRect.X, (int)destRect.Y, (int)destRect.Width, (int)destRect.Height), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, int x, int y)
		{
            DrawImageScaledCore(image, new Rectangle(0, 0, image.Width, image.Height), x, y, image.Width, image.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit)
		{
            DrawImageScaledCore(image, new Rectangle(x, y, srcRect.Width + x, srcRect.Height + y), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
        }

        public void DrawImage(Image image, int x, int y, int width, int height)
		{
            DrawImageScaledCore(image, new Rectangle(0, 0, image.Width, image.Height), x, y, width, height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, float x, float y)
		{
            DrawImageScaledCore(image, new Rectangle(0, 0, image.Width, image.Height), x, y, image.Width, image.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
		{
            DrawImageScaledCore(image, new Rectangle((int)x, (int)y, image.Width, image.Height), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, float x, float y, float width, float height)
        {
            DrawImageScaledCore(image, new Rectangle(0, 0, image.Width, image.Height), x, y, width, height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
        }

		public void DrawImageUnscaled(Image image, Point point)
		{
            DrawImageUnscaledCore(image, point.X, point.Y, image.Width, image.Height);
        }

		public void DrawImageUnscaled(Image image, Rectangle rect)
		{
            DrawImageUnscaledCore(image, rect.X, rect.Y, rect.Width, rect.Height);
        }

		public void DrawImageUnscaled(Image image, int x, int y)
		{
            DrawImageUnscaledCore(image, x, y, image.Width, image.Height);
        }

		public void DrawImageUnscaled(Image image, int x, int y, int width, int height)
		{
            DrawImageUnscaledCore(image, x, y, width, height);
        }

		public void DrawImageUnscaledAndClipped(Image image, Rectangle rect)
		{
            DrawImageUnscaledCore(image, rect.X, rect.Y, rect.Width, rect.Height,true);
        }

		public void DrawLine(Pen pen, Point pt1, Point pt2)
		{
			DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		public void DrawLine(Pen pen, PointF pt1, PointF pt2)
		{
			DrawLines(pen, new PointF[] { pt1, pt2 });
		}

		public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
		{
			DrawLines(pen, new PointF[] { new PointF(x1, y1), new PointF(x2, y2) });
		}

		public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
		{
			DrawLines(pen, new PointF[] { new PointF(x1, y1), new PointF(x2, y2) });
		}

		public void DrawLines(Pen pen, PointF[] points)
		{
			if (points.Length > 0)
			{
				this.context.Save();
                this.ContextTranslateWithDifference(0, 0);
                this.context.SetSourceRGB(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f);
				this.context.LineWidth = pen.Width;
				foreach (PointF p in points)
				{
					this.context.LineTo(p.X, p.Y);
				}
				this.context.Stroke();
				this.context.Restore();

			}
		}

		public void DrawLines(Pen pen, Point[] points)
		{
			DrawLines(pen, Array.ConvertAll<Point, PointF>(points, o => new PointF(o.X, o.Y)));
        }
        public void DrawPath(Pen pen, GraphicsPath path)
        {
            DrawLines(pen, path.PathPoints);
        }

        private void DrawPieCore(bool isFill, Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            this.context.Save();
            this.ContextTranslateWithDifference(0, 0);
            this.context.SetSourceRGB(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f);
            this.context.LineWidth = pen.Width;
            this.context.Arc(x + width / 2, y + height / 2, Math.Min(width / 2, height / 2), startAngle, sweepAngle);
            if (isFill)
                this.context.Fill();
            else
                this.context.Stroke();
            this.context.Restore();
        }

		public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
		{
			DrawPie(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
		{
			DrawPie(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		public void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			DrawPie(pen, x, y, width, height, startAngle, sweepAngle);
		}

		public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
            DrawPieCore(false, pen, x, y, width, height, startAngle, sweepAngle);
        }

        private void DrawPolygonCore(bool isFill, Pen pen, PointF[] points, FillMode fillmode)
        {
            if (points.Length > 0)
            {
                this.context.Save();
                this.ContextTranslateWithDifference(0, 0);
                this.context.SetSourceRGB(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f);
                this.context.LineWidth = pen.Width;
                this.context.NewPath();
                foreach (PointF p in points)
                {
                    this.context.LineTo(p.X, p.Y);
                }
                this.context.ClosePath();
				if (isFill)
				{
                    this.context.FillRule = fillmode == FillMode.Winding ? Cairo.FillRule.Winding : Cairo.FillRule.EvenOdd;
                    this.context.Fill();
				}
				else
					this.context.Stroke();
                this.context.Restore();
            }
        }
        public void DrawPolygon(Pen pen, PointF[] points)
        {
            DrawPolygonCore(false, pen, points, FillMode.Winding);
        }

		public void DrawPolygon(Pen pen, Point[] points)
		{
            DrawPolygonCore(false, pen, Array.ConvertAll(points, p => new PointF(p.X, p.Y)), FillMode.Winding);
        }

        private void DrawRectangleCore(bool isFill, Pen pen, float x, float y, float width, float height)
        {
            this.context.Save();
            this.ContextTranslateWithDifference(0, 0);
            this.context.SetSourceRGB(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f);
            this.context.Rectangle(x, y, width, height);
			if(isFill)
				this.context.Fill();
			else
				this.context.Stroke();
            this.context.Restore();
        }
        public void DrawRectangle(Pen pen, Rectangle rect)
		{
            DrawRectangleCore(false, pen, rect.X, rect.Y, rect.Width, rect.Height);
        }
        public void DrawRectangle(Pen pen, RectangleF rect)
        {
            DrawRectangleCore(false, pen, rect.X, rect.Y, rect.Width, rect.Height);
        }
        public void DrawRectangle(Pen pen, int x, int y, int width, int height)
		{
            DrawRectangleCore(false, pen, x, y, width, height);
        }

		public void DrawRectangle(Pen pen, float x, float y, float width, float height)
		{
            DrawRectangleCore(false, pen, x, y, width, height);
        }

		public void DrawRectangles(Pen pen, RectangleF[] rects)
		{
			foreach (RectangleF rect in rects)
                DrawRectangle(pen, rect);

        }

		public void DrawRectangles(Pen pen, Rectangle[] rects)
		{
			foreach (Rectangle rect in rects)
                DrawRectangle(pen, rect);
        }

		public void DrawString(string s, Font font, Brush brush, PointF point)
		{
			DrawString(s, font, brush, new RectangleF(point.X, point.Y, this.widget.AllocatedWidth, this.widget.AllocatedHeight), new StringFormat());
		}

		public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format)
		{
			DrawString(s, font, brush, new RectangleF(point.X, point.Y, this.widget.AllocatedWidth, this.widget.AllocatedHeight), format);
		}

		public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle)
		{
			DrawString(s, font, brush, layoutRectangle, new StringFormat());
		}

		public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
		{
			if (string.IsNullOrEmpty(s) == false)
			{
				this.context.Save();
                
                float textSize = 14f;
				if (font != null)
				{
					textSize = font.Size;
					if (font.Unit == GraphicsUnit.Point)
						textSize = font.Size * 1 / 72 * 96;
					if (font.Unit == GraphicsUnit.Inch)
						textSize = font.Size * 96;
				}
                this.ContextTranslateWithDifference(layoutRectangle.X, layoutRectangle.Y + textSize);
                if (brush is SolidBrush sbrush)
				{
					if (sbrush.Color.Name != "0")
						this.context.SetSourceRGBA(sbrush.Color.R / 255f, sbrush.Color.G / 255f, sbrush.Color.B / 255f, 1);
				}
				Pango.Context pangocontext = this.widget.PangoContext;
				string family = pangocontext.FontDescription.Family;
				if (string.IsNullOrWhiteSpace(font.Name) == false)
				{
                    var pangoFamily = Array.Find(pangocontext.Families, f => f.Name == font.Name);
                    if (pangoFamily != null)
						family = pangoFamily.Name;
				}
				this.context.SelectFontFace(family, Cairo.FontSlant.Normal, Cairo.FontWeight.Normal);
				this.context.SetFontSize(textSize);
				this.context.ShowText(s);
                this.context.Stroke();
				this.context.Restore();
			}
		}

		public void DrawString(string s, Font font, Brush brush, float x, float y)
		{
			DrawString(s, font, brush, new RectangleF(x, y, this.widget.AllocatedWidth, this.widget.AllocatedHeight), new StringFormat());
		}

		public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format)
		{
			DrawString(s, font, brush, new RectangleF(x, y, this.widget.AllocatedWidth, this.widget.AllocatedHeight), format);
		}

		public void EndContainer(GraphicsContainer container)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
		{
		}

		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
		{
		}

		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
		}

		public void ExcludeClip(Rectangle rect)
		{
		}

		public void ExcludeClip(Region region)
		{
		}

		public void FillClosedCurve(Brush brush, PointF[] points)
		{
            DrawCurveCore(true, true, new Pen(brush,0), points, 0, 0, 0, FillMode.Winding);
        }

		public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode)
		{
            DrawCurveCore(true, true, new Pen(brush, 0), points, 0, 0, 0, fillmode);
        }

		public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, float tension)
		{
            DrawCurveCore(true, true, new Pen(brush, 0), points, 0, 0, tension, fillmode);
        }

		public void FillClosedCurve(Brush brush, Point[] points)
        {
            DrawCurveCore(true, true, new Pen(brush, 0), Array.ConvertAll(points, p => new PointF(p.X, p.Y)), 0, 0, 0, FillMode.Winding);
        }

		public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode)
		{
            DrawCurveCore(true, true, new Pen(brush, 0), Array.ConvertAll(points, p => new PointF(p.X, p.Y)), 0, 0, 0, fillmode);
        }

		public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, float tension)
		{
            DrawCurveCore(true, true, new Pen(brush, 0), Array.ConvertAll(points, p => new PointF(p.X, p.Y)), 0, 0, tension, fillmode);
        }

		public void FillEllipse(Brush brush, Rectangle rect)
        {
            DrawEllipseCore(new Pen(brush, 0), rect.X, rect.Y, rect.Width, rect.Height, true, FillMode.Winding);
        }

		public void FillEllipse(Brush brush, RectangleF rect)
        {
            DrawEllipseCore(new Pen(brush, 0), rect.X, rect.Y, rect.Width, rect.Height, true, FillMode.Winding);
        }

		public void FillEllipse(Brush brush, int x, int y, int width, int height)
		{
            DrawEllipseCore(new Pen(brush, 0), x, y, width, height, true, FillMode.Winding);
        }

		public void FillEllipse(Brush brush, float x, float y, float width, float height)
		{
            DrawEllipseCore(new Pen(brush, 0), x, y, width, height, true, FillMode.Winding);
        }

		public void FillPath(Brush brush, GraphicsPath path)
		{
			if (brush is SolidBrush sbrush)
			{
				if (path.PathPoints != null && path.PathPoints.Length > 0)
				{
					this.context.Save();
					this.context.SetSourceRGB(sbrush.Color.R / 255f, sbrush.Color.G / 255f, sbrush.Color.B / 255f);
                    this.ContextTranslateWithDifference(0, 0);
					this.context.LineJoin = Cairo.LineJoin.Bevel;
					this.context.LineCap = Cairo.LineCap.Butt;

					foreach (PointF p in path.PathPoints)
					{
                        this.context.LineTo(Convert.ToDouble(p.X), Convert.ToDouble(p.Y));
					}
					this.context.Fill();
					this.context.Restore();
				}
			}
		}

		public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle)
		{
            FillPie(brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

		public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
            FillPie(brush, x, y, width, height, startAngle, sweepAngle);
        }

		public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
            DrawPieCore(false, new Pen(brush, 0), x, y, width, height, startAngle, sweepAngle);
        }

		public void FillPolygon(Brush brush, PointF[] points)
		{
            FillPolygon(brush, points, FillMode.Winding);
        }

		public void FillPolygon(Brush brush, PointF[] points, FillMode fillMode)
		{
            DrawPolygonCore(true, new Pen(brush, 0), points, fillMode);
        }

		public void FillPolygon(Brush brush, Point[] points)
		{
            FillPolygon(brush, points,FillMode.Winding);
        }

		public void FillPolygon(Brush brush, Point[] points, FillMode fillMode)
		{
            DrawPolygonCore(true, new Pen(brush, 0), Array.ConvertAll(points, p => new PointF(p.X, p.Y)), fillMode);
        }

		public void FillRectangle(Brush brush, Rectangle rect)
		{
            DrawRectangleCore(true, new Pen(brush, 0), rect.X, rect.Y, rect.Width, rect.Height);
        }

		public void FillRectangle(Brush brush, RectangleF rect)
		{
            DrawRectangleCore(true, new Pen(brush, 0), rect.X, rect.Y, rect.Width, rect.Height);
        }

		public void FillRectangle(Brush brush, int x, int y, int width, int height)
		{
            DrawRectangleCore(true, new Pen(brush, 0), x, y, width, height);
        }

		public void FillRectangle(Brush brush, float x, float y, float width, float height)
		{
            DrawRectangleCore(true, new Pen(brush, 0), x, y, width, height);
        }
 
        public void FillRectangles(Brush brush, RectangleF[] rects)
		{
			foreach (RectangleF rect in rects)
				FillRectangle(brush, rect);
		}

		public void FillRectangles(Brush brush, Rectangle[] rects)
		{
			foreach (Rectangle rect in rects)
				FillRectangle(brush, rect);
		}

		public void FillRegion(Brush brush, Region region)
		{
		}

		~Graphics()
		{
		}

		public void Flush()
		{
		}

		public void Flush(FlushIntention intention)
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Graphics FromHdc(IntPtr hdc)
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Graphics FromHdc(IntPtr hdc, IntPtr hdevice)
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Graphics FromHdcInternal(IntPtr hdc)
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Graphics FromHwnd(IntPtr hwnd)
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Graphics FromHwndInternal(IntPtr hwnd)
		{
			throw null;
		}

		public static Graphics FromImage(Image image)
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public object GetContextInfo()
		{
			throw null;
		}

		public static IntPtr GetHalftonePalette()
		{
			throw null;
		}

		public IntPtr GetHdc()
		{
			throw null;
		}

		public Color GetNearestColor(Color color)
		{
			throw null;
		}

		public void IntersectClip(Rectangle rect)
		{
		}

		public void IntersectClip(RectangleF rect)
		{
		}

		public void IntersectClip(Region region)
		{
		}

		public bool IsVisible(Point point)
		{
			throw null;
		}

		public bool IsVisible(PointF point)
		{
			throw null;
		}

		public bool IsVisible(Rectangle rect)
		{
			throw null;
		}

		public bool IsVisible(RectangleF rect)
		{
			throw null;
		}

		public bool IsVisible(int x, int y)
		{
			throw null;
		}

		public bool IsVisible(int x, int y, int width, int height)
		{
			throw null;
		}

		public bool IsVisible(float x, float y)
		{
			throw null;
		}

		public bool IsVisible(float x, float y, float width, float height)
		{
			throw null;
		}

		public Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat)
		{
			throw null;
		}

		public SizeF MeasureString(string text, Font font)
		{
			throw null;
		}

		public SizeF MeasureString(string text, Font font, PointF origin, StringFormat stringFormat)
		{
			throw null;
		}

		public SizeF MeasureString(string text, Font font, SizeF layoutArea)
		{
			throw null;
		}

		public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat)
		{
			throw null;
		}

		public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled)
		{
			throw null;
		}

		public SizeF MeasureString(string text, Font font, int width)
		{
			throw null;
		}

		public SizeF MeasureString(string text, Font font, int width, StringFormat format)
		{
			throw null;
		}

		public void MultiplyTransform(Matrix matrix)
		{
		}

		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
		}

		public void ReleaseHdc()
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void ReleaseHdc(IntPtr hdc)
		{
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ReleaseHdcInternal(IntPtr hdc)
		{
		}

		public void ResetClip()
		{
		}

		public void ResetTransform()
		{
		}

		public void Restore(GraphicsState gstate)
		{
		}

		public void RotateTransform(float angle)
		{
		}

		public void RotateTransform(float angle, MatrixOrder order)
		{
		}

		public GraphicsState Save()
		{
			throw null;
		}

		public void ScaleTransform(float sx, float sy)
		{
		}

		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
		}

		public void SetClip(GraphicsPath path)
		{
		}

		public void SetClip(GraphicsPath path, CombineMode combineMode)
		{
		}

		public void SetClip(Graphics g)
		{
		}

		public void SetClip(Graphics g, CombineMode combineMode)
		{
		}

		public void SetClip(Rectangle rect)
		{
		}

		public void SetClip(Rectangle rect, CombineMode combineMode)
		{
		}

		public void SetClip(RectangleF rect)
		{
		}

		public void SetClip(RectangleF rect, CombineMode combineMode)
		{
		}

		public void SetClip(Region region, CombineMode combineMode)
		{
		}

		public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts)
		{
		}

		public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts)
		{
		}

		public void TranslateClip(int dx, int dy)
		{
		}

		public void TranslateClip(float dx, float dy)
		{
		}

		public void TranslateTransform(float dx, float dy)
		{
		}

		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
		}
	}
}
