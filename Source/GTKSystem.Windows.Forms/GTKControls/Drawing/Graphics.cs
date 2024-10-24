
using Cairo;
using Gdk;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;

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
        internal Graphics(Cairo.Context context, Gdk.Rectangle rectangle)
        {
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

		public Drawing2D.Matrix Transform
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
		//internal void ContextTranslateWithDifference(double x,double y)
		//{
  //          this.context.Translate(diff_left + x, diff_top + y);
  //      }
        internal void SetTranslateWithDifference(double x, double y)
        {
            this.context.Translate(diff_left + x, diff_top + y);
        }
        internal void SetSourceColor(Pen pen)
		{
            if (pen.Brush is SolidBrush sbrush)
            {
                this.context.SetSourceRGBA(sbrush.Color.R / 255f, sbrush.Color.G / 255f, sbrush.Color.B / 255f, sbrush.Color.A / 255f);
            }
            else if (pen.Brush is LinearGradientBrush lbrush)
            {
                double maxsize = Math.Max(diff_left + lbrush.Rectangle.Right, diff_top + lbrush.Rectangle.Bottom); //渐变角度定为方形45度
                using Cairo.LinearGradient gradient = new Cairo.LinearGradient(diff_left + lbrush.Rectangle.Left, diff_top + lbrush.Rectangle.Top, maxsize, maxsize);
				int linearcount = lbrush.LinearColors.Length;
				int idx = 0;
				foreach (Color color in lbrush.LinearColors)
					gradient.AddColorStop((++idx) / linearcount, new Cairo.Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f));

				Cairo.Matrix matrix = new Cairo.Matrix(1, 0, 0, 1, 0, 0);
				matrix.Rotate(Math.PI * 45 / 180);//弧度
				gradient.Matrix = matrix;
                using Cairo.Pattern pattern = Cairo.Pattern.Lookup(gradient.Handle, false);
				this.context.SetSource(pattern);
			}
			else if (pen.Brush is HatchBrush hbrush)
            {
                this.context.SetSourceRGBA(hbrush.ForegroundColor.R / 255f, hbrush.ForegroundColor.G / 255f, hbrush.ForegroundColor.B / 255f, hbrush.ForegroundColor.A / 255f);
            }
            else if (pen.Brush is PathGradientBrush pbrush)
            {
                double maxsize = Math.Max(diff_left + pbrush.Rectangle.Right, diff_top + pbrush.Rectangle.Bottom); //渐变角度定为方形45度
                using Cairo.LinearGradient gradient = new Cairo.LinearGradient(diff_left + pbrush.Rectangle.Left, diff_top + pbrush.Rectangle.Top, maxsize, maxsize);
                int linearcount = pbrush.SurroundColors.Length;
                double centeridx = Math.Floor((double)linearcount / 2);
                int idx = 0;
				foreach (Color color in pbrush.SurroundColors)
				{
					if(idx == centeridx)
                        gradient.AddColorStop((++idx) / linearcount, new Cairo.Color(pbrush.CenterColor.R / 255f, pbrush.CenterColor.G / 255f, pbrush.CenterColor.B / 255f, pbrush.CenterColor.A / 255f));
					else
						gradient.AddColorStop((++idx) / linearcount, new Cairo.Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f));
				}
                Cairo.Matrix matrix = new Cairo.Matrix(1, 0, 0, 1, 0, 0);
                matrix.Rotate(Math.PI * 45 / 180);//弧度
                gradient.Matrix = matrix;
                using Cairo.Pattern pattern = Cairo.Pattern.Lookup(gradient.Handle, false);
                this.context.SetSource(pattern);
            }
			else
			{
                this.context.SetSourceRGBA(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f, pen.Color.A / 255f);
            }
        }
		public void Clear(Color color)
		{
            this.context.Save();
			this.context.SetSourceRGB(color.R / 255f, color.G / 255f, color.B / 255f);
            this.SetTranslateWithDifference(0, 0);
            this.context.Rectangle(this.rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
			this.context.Fill();
			//this.context.Paint();
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
            this.SetTranslateWithDifference(0, 0);
            this.SetSourceColor(pen);
            this.context.LineWidth = pen.Width;
            this.context.LineJoin = Cairo.LineJoin.Round;
            this.context.NewPath();
            this.context.Arc(x, y, Math.Min(width / 2, height / 2), Math.PI * startAngle / 180, Math.PI * (startAngle + sweepAngle) / 180);
            //this.context.ArcNegative(x, y, Math.Min(width / 2, height / 2), Math.PI * startAngle / 180, Math.PI * sweepAngle / 180); //相反位置
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

        #region 贝塞尔曲线
        /// <summary>
        /// 收集贝塞尔曲线坐标点全部点的位置集合
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private List<PointF> GetBezierPoints(List<PointF> points)
        {
            float seedNum = 0;
            for (int i = 1; i < points.Count; i++)
            {
                seedNum += Math.Abs(points[i].X - points[i - 1].X) + Math.Abs(points[i].Y - points[i - 1].Y);
            }
            seedNum += seedNum * 0.2f;
            float pStep = 1 / seedNum;
            List<PointF> rpoint = new List<PointF>();
            for (float pTime = 0; pTime <= 1; pTime += pStep)
            {
                List<PointF> lfpr = CalculateBezier(points, pTime);
                PointF fpr = lfpr[0];
                rpoint.Add(fpr);
            }
            return rpoint;
        }
        /// <summary>
        /// 计算贝塞尔曲线上坐标点单点位置
        /// </summary>
        /// <param name="points">贝塞尔条件坐标集合</param>
        /// <param name="time">时间因子</param>
        /// <returns></returns>
        private List<PointF> CalculateBezier(List<PointF> points, float time)
        {
            List<PointF> nList = new List<PointF> { };
            int listNum = points.Count;
            if (listNum < 2)
            {
                return points.ToList();
            }
            for (int n = 1; n < listNum; n++)
            {
                float nowX = (points[n].X - points[n - 1].X) * time + points[n - 1].X;
                float nowY = (points[n].Y - points[n - 1].Y) * time + points[n - 1].Y;
                PointF nowP = new PointF(nowX, nowY);
                nList.Add(nowP);
            }

            List<PointF> p = CalculateBezier(nList, time);
            return p;
        }

        private void DrawBeziersCore(Pen pen, PointF[] points)
        {
			List<PointF> data = GetBezierPoints(points.ToList());
            DrawLinesCore(pen, data.ToArray());
        }
        public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
		{
			DrawBeziersCore(pen, new PointF[] { new PointF(pt1.X, pt1.Y), new PointF(pt2.X, pt2.Y), new PointF(pt3.X, pt3.Y), new PointF(pt4.X, pt4.Y) });
        }

		public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
		{
            DrawBeziersCore(pen, new PointF[] { pt1, pt2, pt3, pt4 });
        }

		public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
		{
            DrawBeziersCore(pen, new PointF[] { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3), new PointF(x4, y4) });
        }

		public void DrawBeziers(Pen pen, PointF[] points)
		{
            DrawBeziersCore(pen, points);
        }

		public void DrawBeziers(Pen pen, Point[] points)
		{
            DrawBeziersCore(pen, Array.ConvertAll(points, p => new PointF(p.X, p.Y)));
        }
        #endregion

        private void DrawCurveCore(bool isClosePath, bool isfill, Pen pen, PointF[] points, int offset, int numberOfSegments, float tension, FillMode fillmode)
        {
            if (points.Length > 1)
            {
                this.context.Save();
                this.SetTranslateWithDifference(offset, offset);
                this.SetSourceColor(pen);
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
        private void DrawEllipseCore(Pen pen, float x, float y, float width, float height, bool isfill, FillMode fillmode)
        {
            this.context.Save();
            this.SetTranslateWithDifference(x + width / 2, y + height / 2);
            this.SetSourceColor(pen);
            this.context.LineWidth = pen.Width;
            this.context.LineJoin = Cairo.LineJoin.Round;
            this.context.NewPath();
			float r = (width + height) / 4;
			double rs = Math.Min(0.1, 2 / r);
            for (double t = 0; t < 2 * Math.PI; t += rs)
            {
                double x2_1 = width * Math.Cos(t) / 2;
                double y2_1 = height * Math.Sin(t) / 2;
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
            DrawImageScaledCore(image, new Rectangle(point.X, point.Y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, PointF point)
		{
            DrawImageScaledCore(image, new Rectangle((int)point.X, (int)point.Y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
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
				this.SetTranslateWithDifference(x, y);
				Gdk.CairoHelper.SetSourcePixbuf(this.context, newimg, 0, 0);

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
                this.SetTranslateWithDifference(destRect.X, destRect.Y);
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
            DrawImageScaledCore(image, new Rectangle(x, y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit)
		{
            DrawImageScaledCore(image, new Rectangle(x, y, srcRect.Width + x, srcRect.Height + y), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
        }

        public void DrawImage(Image image, int x, int y, int width, int height)
		{
            DrawImageScaledCore(image, new Rectangle(x, y, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, float x, float y)
		{
            DrawImageScaledCore(image, new Rectangle((int)x, (int)y, image.Width, image.Height), x, y, image.Width, image.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
		{
            DrawImageScaledCore(image, new Rectangle((int)x, (int)y, image.Width, image.Height), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
        }

		public void DrawImage(Image image, float x, float y, float width, float height)
        {
            DrawImageScaledCore(image, new Rectangle((int)x, (int)y, (int)width, (int)height), 0, 0, width, height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
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
        private void DrawLinesCore(Pen pen, PointF[] points)
        {
            if (points.Length > 0)
            {
                this.context.Save();
                this.SetTranslateWithDifference(0, 0);
                this.SetSourceColor(pen);
                this.context.LineWidth = pen.Width;
                this.context.NewPath();
                foreach (PointF p in points)
                {
                    this.context.LineTo(p.X, p.Y);
                }
                this.context.Stroke();
                this.context.Restore();

            }
        }
        public void DrawLine(Pen pen, Point pt1, Point pt2)
		{
			DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		public void DrawLine(Pen pen, PointF pt1, PointF pt2)
		{
            DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

		public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
		{
            DrawLinesCore(pen, new PointF[] { new PointF(x1, y1), new PointF(x2, y2) });
        }

		public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
		{
            DrawLinesCore(pen, new PointF[] { new PointF(x1, y1), new PointF(x2, y2) });
		}

		public void DrawLines(Pen pen, PointF[] points)
		{
            DrawLinesCore(pen, points);
        }

		public void DrawLines(Pen pen, Point[] points)
		{
            DrawLinesCore(pen, Array.ConvertAll<Point, PointF>(points, o => new PointF(o.X, o.Y)));
        }
        public void DrawPath(Pen pen, GraphicsPath path)
        {
			DrawPathCore(pen, path, false);
        }
        private void DrawPathCore(Pen pen, GraphicsPath path, bool isfill)
        {
            this.context.Save();
            path.Context = this.context;
            this.SetTranslateWithDifference(0, 0);
            this.SetSourceColor(pen);
            this.context.LineWidth = pen.Width;
            this.context.NewPath();
            foreach (object o in path.list)
            {
                if (o is GraphicsPath.FigureMode start && start.start == true)
                {
                    this.context.NewSubPath();
                }
                else if (o is GraphicsPath.ArcMode arc)
                {
                    double rw = arc.rect.Width / 2;
                    double rh = arc.rect.Height / 2;
                    double ra = Math.Min(rw, rh);
                    this.context.Arc(arc.rect.X + rw, arc.rect.Y + rh, ra, Math.PI * arc.startAngle / 180, Math.PI * (arc.startAngle + arc.sweepAngle) / 180);
                }
                else if (o is GraphicsPath.BezierMode bezier)
                {
                    this.context.MoveTo(bezier.pt1.X, bezier.pt1.Y);
                    List<PointF> data = GetBezierPoints(new List<PointF>() { bezier.pt1, bezier.pt2, bezier.pt3, bezier.pt4 });
                    foreach (PointF point in data)
                    {
                        this.context.LineTo(point.X, point.Y);
                    }
                }
                else if (o is GraphicsPath.BeziersMode beziers)
                {
                    List<PointF> data = GetBezierPoints(beziers.points.ToList());
                    foreach (PointF point in data)
                    {
                        this.context.LineTo(point.X, point.Y);
                    }
                }
                else if (o is GraphicsPath.ClosedCurveMode closedcurve)
                {
                    this.context.CurveTo(closedcurve.points[0].X, closedcurve.points[0].Y, closedcurve.points[1].X, closedcurve.points[1].Y, closedcurve.points[2].X, closedcurve.points[2].Y);
                    this.context.FillRule = closedcurve.fillmode == FillMode.Winding ? Cairo.FillRule.Winding : Cairo.FillRule.EvenOdd;
                    //this.context.Fill();
                    this.context.ClosePath();
                    this.context.NewSubPath();
                }
                else if (o is GraphicsPath.CurveMode curve)
                {
                    this.context.CurveTo(curve.points[0].X + curve.offset, curve.points[0].Y + curve.offset, curve.points[1].X + curve.offset, curve.points[1].Y + curve.offset, curve.points[2].X + curve.offset, curve.points[2].Y + curve.offset);
                }
                else if (o is GraphicsPath.EllipseMode ellipse)
                {
                    this.context.NewSubPath();
                    float r = (ellipse.rect.Width + ellipse.rect.Height) / 4;
                    double rs = Math.Min(0.1, 2 / r);
                    for (double t = 0; t < 2 * Math.PI; t += rs)
                    {
                        double x2_1 = ellipse.rect.Width * Math.Cos(t) / 2;
                        double y2_1 = ellipse.rect.Height * Math.Sin(t) / 2;
                        this.context.LineTo(x2_1 + ellipse.rect.X + ellipse.rect.Width / 2, y2_1 + ellipse.rect.Y + ellipse.rect.Height / 2);
                    }
                    this.context.ClosePath();
                    this.context.NewSubPath();
                }
                else if (o is GraphicsPath.LineMode line)
                {
                    this.context.LineTo(line.pt1.X, line.pt1.Y);
                    this.context.LineTo(line.pt2.X, line.pt2.Y);
                }
                else if (o is GraphicsPath.LinesMode lines)
                {
                    this.context.MoveTo(lines.points[0].X, lines.points[0].Y);
                    foreach (PointF p in lines.points)
                    {
                        this.context.LineTo(p.X, p.Y);
                    }
                }
                else if (o is GraphicsPath.PieMode pie)
                {
                    this.context.NewSubPath();
                    double rw = pie.rect.Width / 2;
                    double rh = pie.rect.Height / 2;
                    double ra = Math.Min(rw, rh);
                    this.context.Arc(pie.rect.X + rw, pie.rect.Y + rh, ra, Math.PI * pie.startAngle / 180, Math.PI * (pie.startAngle + pie.sweepAngle) / 180);
                    this.context.LineTo(pie.rect.X + rw, pie.rect.Y + rh);
                    this.context.ClosePath();
                    this.context.NewSubPath();
                }
                else if (o is GraphicsPath.PolygonMode polygon)
                {
                    this.context.NewSubPath();
                    foreach (PointF p in polygon.points)
                    {
                        this.context.LineTo(p.X, p.Y);
                    }
                    this.context.ClosePath();
                    this.context.NewSubPath();
                }
                else if (o is GraphicsPath.RectangleMode rectangle)
                {
                    this.context.Rectangle(rectangle.rect.X, rectangle.rect.Y, rectangle.rect.Width, rectangle.rect.Height);
                }
                else if (o is GraphicsPath.RectanglesMode rectangles)
                {
                    foreach (RectangleF rect in rectangles.rects)
                    {
                        this.context.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                    }

                }
                else if (o is GraphicsPath.StringMode str)
                {
                    string text = str.text;
                    if (str.layoutRect.Width > 0)
                    {
                        while (text.Length > 0 && this.context.TextExtents(text).Width > str.layoutRect.Width)
                            text = text.Substring(0, text.Length - 1);
                    }
                    float textSize = str.emSize < 1 ? 14f : str.emSize;
                    FontFamily font = str.family;
                    string family = font?.Name;
                    if (this.widget != null)
                    {
                        Pango.Context pangocontext = this.widget.PangoContext;
                        family = pangocontext.FontDescription.Family;
                        var pangoFamily = Array.Find(pangocontext.Families, f => f.Name == font.Name);
                        if (pangoFamily == null)
                            family = pangocontext.FontDescription.Family;
                    }
                    
                    this.context.SelectFontFace(family, str.style == 2 ? Cairo.FontSlant.Italic : Cairo.FontSlant.Normal, str.style == 1 ? Cairo.FontWeight.Bold : Cairo.FontWeight.Normal);
                    this.context.SetFontSize(textSize);
                    TextExtents textext = this.context.TextExtents(text);
                    this.context.MoveTo(str.layoutRect.X, str.layoutRect.Y + textext.Height);
                    this.context.ShowText(text);
                }
                else if (o is GraphicsPath.PathMode addpath)
                {
                    DrawPath(pen, addpath.path);
                }
                if (path.IsCloseAllFigures == true || (o is GraphicsPath.FigureMode close && close.close == true))
                {
                    this.context.ClosePath();
                }
            }
            if (isfill == true)
                this.context.Fill();
            else
                this.context.Stroke();

            if (path.matrix != null)
            {
                this.context.Matrix = ConvertToMatrix(path.matrix);
            }

            this.context.Restore();
        }
        private Cairo.Matrix ConvertToMatrix(Drawing2D.Matrix matrix)
        {
            Cairo.Matrix CairoMatrix = new Cairo.Matrix(matrix.m11, matrix.m12, matrix.m21, matrix.m22, matrix.dx, matrix.dy);
            CairoMatrix.Init(matrix.m11, matrix.m12, matrix.m21, matrix.m22, matrix.dx, matrix.dy);

            CairoMatrix.Translate(matrix.OffsetX, matrix.OffsetY);
            CairoMatrix.Scale(matrix.scaleX, matrix.scaleY);
            CairoMatrix.Rotate(matrix.angle);
            CairoMatrix.Multiply(ConvertToMatrix(matrix.multiply));
            if (matrix.invert)
                CairoMatrix.Invert();

			return CairoMatrix;
        }
        private void DrawPieCore(bool isFill, Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            this.context.Save();
            this.SetTranslateWithDifference(0, 0);
            this.SetSourceColor(pen);
            this.context.LineWidth = pen.Width;
            this.context.NewPath();
            this.context.MoveTo(x, y);
            this.context.Arc(x, y, Math.Min(width / 2, height / 2), Math.PI * startAngle / 180, Math.PI * (startAngle + sweepAngle) / 180);
            this.context.LineTo(x, y);
            this.context.ClosePath();
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
                this.SetTranslateWithDifference(0, 0);
                this.SetSourceColor(pen);
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
            this.SetTranslateWithDifference(0, 0);
            this.SetSourceColor(pen);
            this.context.NewPath();
            this.context.LineWidth = pen.Width;
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
			DrawString(s, font, brush, new RectangleF(point.X + this.rectangle.X, point.Y + this.rectangle.Y, this.rectangle.Width, this.rectangle.Height), new StringFormat());
		}

		public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format)
		{
			DrawString(s, font, brush, new RectangleF(point.X + this.rectangle.X, point.Y + this.rectangle.Y, this.rectangle.Width, this.rectangle.Height), format);
		}

		public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle)
		{
			DrawString(s, font, brush, layoutRectangle, new StringFormat());
		}

		public void DrawString(string text, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
		{
			if (string.IsNullOrEmpty(text) == false)
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
                
                string family = font?.Name;
                if (this.widget != null)
                {
                    Pango.Context pangocontext = this.widget.PangoContext;
                    family = pangocontext.FontDescription.Family;
                    var pangoFamily = Array.Find(pangocontext.Families, f => f.Name == font.Name);
                    if (pangoFamily == null)
                        family = pangocontext.FontDescription.Family;
                }
                this.context.SetFontSize(textSize);
                this.context.SelectFontFace(family, (font.Style & FontStyle.Italic) != 0 ? Cairo.FontSlant.Italic : Cairo.FontSlant.Normal, (font.Style & FontStyle.Bold) != 0 ? Cairo.FontWeight.Bold : Cairo.FontWeight.Normal);
                TextExtents textext = this.context.TextExtents(text);
                this.SetTranslateWithDifference(layoutRectangle.X, layoutRectangle.Y + textext.Height);
                this.SetSourceColor(new Pen(brush, 1));
				this.context.ShowText(text);
                this.context.Stroke();
				this.context.Restore();
			}
		}

		public void DrawString(string s, Font font, Brush brush, float x, float y)
        {
            DrawString(s, font, brush, new RectangleF(x + this.rectangle.X, y + this.rectangle.Y, this.rectangle.Width, this.rectangle.Height), new StringFormat());
        }
        public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format)
        {
			DrawString(s, font, brush, new RectangleF(x + this.rectangle.X, y + this.rectangle.Y, this.rectangle.Width, this.rectangle.Height), format);
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
            DrawPathCore(new Pen(brush, 1), path, true);
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
        private static Cairo.ImageSurface imagesurface;
        private static Cairo.Surface simisurface;
        private static Cairo.Context imagecontext;
        /// <summary>
        /// 使用此方法必须要执行Flush()方法输出Image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Graphics FromImage(Image image)
        {
            int _width = image.Width;
            int _height = image.Height;

            if (_width < 1)
                throw new ArgumentOutOfRangeException("Image.Width不能小于等于0");
            if (_height < 1)
                throw new ArgumentOutOfRangeException("Image.Height不能小于等于0");

            if (imagesurface == null)
                imagesurface = new Cairo.ImageSurface(Cairo.Format.Argb32, _width, _height);

            simisurface?.Dispose();
            simisurface = imagesurface.CreateSimilar(Cairo.Content.ColorAlpha, _width, _height);
            imagecontext?.Dispose();
            imagecontext = new Cairo.Context(simisurface);
            return new Drawing.Graphics(image, imagecontext, new Gdk.Rectangle(0, 0, _width, _height));
        }

        public void Flush()
        {
			Flush(FlushIntention.Flush);
        }

        public void Flush(FlushIntention intention)
        {
			try
			{
				if (this.widget is Image image && Graphics.simisurface != null && Graphics.simisurface.Status == Cairo.Status.Success)
				{
					image.Pixbuf = new Pixbuf(Graphics.simisurface, 0, 0, image.Width, image.Height);
				}
			}
			finally { }
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
		public object GetContextInfo()
		{
			return	this.context;
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
			return MeasureString(text, font, width, StringFormat.GenericDefault);
        }

		public SizeF MeasureString(string text, Font font, int width, StringFormat format)
        {
            float textSize = 14f;
            if (font != null)
            {
                textSize = font.Size;
                if (font.Unit == GraphicsUnit.Point)
                    textSize = font.Size * 1 / 72 * 96;
                if (font.Unit == GraphicsUnit.Inch)
                    textSize = font.Size * 96;
            }
            string family = font?.Name;
            if (this.widget != null)
            {
                Pango.Context pangocontext = this.widget.PangoContext;
                family = pangocontext.FontDescription.Family;
                var pangoFamily = Array.Find(pangocontext.Families, f => f.Name == font.Name);
                if (pangoFamily == null)
                    family = pangocontext.FontDescription.Family;
            }
            this.context.SelectFontFace(family, font.Italic ? Cairo.FontSlant.Italic : Cairo.FontSlant.Normal, font.Bold ? Cairo.FontWeight.Bold : Cairo.FontWeight.Normal);
            this.context.SetFontSize(textSize);
            var extents = this.context.TextExtents(text);
			return new SizeF((float)Math.Max(width,extents.Width), (float)extents.Height);
        }

		public void MultiplyTransform(Drawing2D.Matrix matrix)
		{
            this.context.Matrix?.Multiply(ConvertToMatrix(matrix));
        }

		public void MultiplyTransform(Drawing2D.Matrix matrix, MatrixOrder order)
		{
			this.context.Matrix?.Multiply(ConvertToMatrix(matrix));
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
			this.context.ResetClip();
		}

		public void ResetTransform()
		{
            this.context.Rotate(Math.PI / 180 * _angle * -1);
        }

		public void Restore(GraphicsState gstate)
		{
			this.context.Restore();
        }
		private float _angle = 0;
		public void RotateTransform(float angle)
		{
            this.context.Rotate(Math.PI / 180 * angle);
            _angle = angle;
        }

		public void RotateTransform(float angle, MatrixOrder order)
        {
            this.context.Rotate(Math.PI / 180 * angle);
            _angle = angle;
        }

		public GraphicsState Save()
		{
			return new GraphicsState();
		}

		public void ScaleTransform(float sx, float sy)
		{
			this.context.Scale(sx, sy);
		}

		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
            this.context.Scale(sx, sy);
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
            //TranslateTransform(dx, dy, MatrixOrder.Append);
        }

		public void TranslateClip(float dx, float dy)
		{
            //TranslateTransform(dx, dy, MatrixOrder.Append);
        }

		public void TranslateTransform(float dx, float dy)
		{
            TranslateTransform(dx, dy, MatrixOrder.Append);
        }

		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
            this.SetTranslateWithDifference(dx, dy);
        }
	}
}
