using GLib;
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
			get
			{
				throw null;
			}
		}

		public float DpiY
		{
			get
			{
				throw null;
			}
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
			get
			{
				throw null;
			}
		}

		public float PageScale
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		public GraphicsUnit PageUnit
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		public PixelOffsetMode PixelOffsetMode
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		public Point RenderingOrigin
		{
			get
			{
				throw null;
			}
			set
			{
			}
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

		public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
		{
			DrawArc(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
		{
			DrawArc(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			DrawArc(pen, (float)x, (float)y, (float)width, (float)height, (float)startAngle, (float)sweepAngle);
		}

		public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			this.context.Save();
            this.ContextTranslateWithDifference(0, 0);
            this.context.LineWidth = pen.Width;
			this.context.LineJoin = Cairo.LineJoin.Round;
			this.context.SetSourceRGB(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f);
			this.context.Arc(x + width / 2, y + height / 2, Math.Min(width / 2, height / 2), 3, 300);
			this.context.Stroke();
			this.context.Restore();
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

		public void DrawClosedCurve(Pen pen, PointF[] points)
		{
		}

		public void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillmode)
		{
		}

		public void DrawClosedCurve(Pen pen, Point[] points)
		{
		}

		public void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillmode)
		{
		}

		public void DrawCurve(Pen pen, PointF[] points)
		{
		}

		public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments)
		{
		}

		public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension)
		{
		}

		public void DrawCurve(Pen pen, PointF[] points, float tension)
		{
		}

		public void DrawCurve(Pen pen, Point[] points)
		{
		}

		public void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension)
		{
		}

		public void DrawCurve(Pen pen, Point[] points, float tension)
		{
		}

		public void DrawEllipse(Pen pen, Rectangle rect)
		{
		}

		public void DrawEllipse(Pen pen, RectangleF rect)
		{
		}

		public void DrawEllipse(Pen pen, int x, int y, int width, int height)
		{
		}

		public void DrawEllipse(Pen pen, float x, float y, float width, float height)
		{
		}

		public void DrawIcon(Icon icon, Rectangle targetRect)
		{
		}

		public void DrawIcon(Icon icon, int x, int y)
		{
		}

		public void DrawIconUnstretched(Icon icon, Rectangle targetRect)
		{
		}

		public void DrawImage(Image image, Point point)
		{
			DrawImage(image, [new PointF(point.X, point.Y)], new RectangleF(0, 0, 0, 0), GraphicsUnit.Pixel, null, null, 0);
		}

		public void DrawImage(Image image, PointF point)
		{
			DrawImage(image, [point], new RectangleF(0, 0, 0, 0), GraphicsUnit.Pixel, null, null, 0);
		}

		public void DrawImage(Image image, PointF[] destPoints)
		{
			DrawImage(image, destPoints, new RectangleF(0, 0, image.Width, image.Height), GraphicsUnit.Pixel, null, null, 0);
		}

		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)
		{
			DrawImage(image, destPoints, srcRect, srcUnit, null, null, 0);
		}

		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
		{
			DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, null, 0);
		}

		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
		{
			DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback, 0);
		}

		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData)
		{
			Gdk.Pixbuf img = new Gdk.Pixbuf(image.PixbufData);
			this.context.Save();
            this.ContextTranslateWithDifference(destPoints[0].X, destPoints[0].Y);
			Gdk.CairoHelper.SetSourcePixbuf(this.context, img, srcRect.X, srcRect.Y);

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

		public void DrawImage(Image image, Point[] destPoints)
		{
			DrawImage(image, destPoints, new Rectangle(0, 0, 0, 0), GraphicsUnit.Pixel, null, null, 0);
		}

		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)
		{
			DrawImage(image, destPoints, srcRect, srcUnit, null, null, 0);
		}

		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
		{
			DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, null, 0);
		}

		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
		{
			DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback, 0);
		}

		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData)
		{
			DrawImage(image, Array.ConvertAll<Point, PointF>(destPoints, o => new PointF(o.X, o.Y)), srcRect, srcUnit, imageAttr, callback, callbackData);
		}

		public void DrawImage(Image image, Rectangle rect)
		{
			DrawImage(image, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)
		{
			DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit)
		{
			DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
		{
			DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback)
		{
			DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
		{
			DrawImage(image, destRect, (float)srcX, (float)srcY, (float)srcWidth, (float)srcHeight, srcUnit, imageAttrs, callback, callbackData);
		}

		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit)
		{
			DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
		{
			DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback)
		{
			DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, IntPtr.Zero);
		}

		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
		{
			Gdk.Pixbuf scaleimg = new Gdk.Pixbuf(new Cairo.ImageSurface(Cairo.Format.Argb32, destRect.Width, destRect.Height), 0, 0, destRect.Width, destRect.Height);
			Gdk.Pixbuf img = new Gdk.Pixbuf(image.PixbufData);
			img.Scale(scaleimg, destRect.X, destRect.Y, destRect.Width, destRect.Height, srcX, srcY, destRect.Width / srcWidth, destRect.Height / srcHeight, Gdk.InterpType.Tiles);
			this.context.Save();
			this.context.Translate(destRect.X, destRect.Y);
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

		public void DrawImage(Image image, RectangleF rect)
		{
			DrawImage(image, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height), 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
		{
			DrawImage(image, new Rectangle((int)destRect.X, (int)destRect.Y, (int)destRect.Width, (int)destRect.Height), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
		}

		public void DrawImage(Image image, int x, int y)
		{
			DrawImage(image, new Point(x, y));
		}

		public void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit)
		{
			DrawImage(image, new Rectangle(x, y, srcRect.Width, srcRect.Height), srcRect, srcUnit);
		}

		public void DrawImage(Image image, int x, int y, int width, int height)
		{
			DrawImage(image, new Rectangle(x, y, width, width));
		}

		public void DrawImage(Image image, float x, float y)
		{
			DrawImage(image, new PointF(x, y));
		}

		public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
		{
			DrawImage(image, new PointF[] { new PointF(x, y) }, srcRect, GraphicsUnit.Pixel);
		}

		public void DrawImage(Image image, float x, float y, float width, float height)
		{
			DrawImage(image, new RectangleF(x, y, width, width));
		}

		public void DrawImageUnscaled(Image image, Point point)
		{
		}

		public void DrawImageUnscaled(Image image, Rectangle rect)
		{
		}

		public void DrawImageUnscaled(Image image, int x, int y)
		{
		}

		public void DrawImageUnscaled(Image image, int x, int y, int width, int height)
		{
		}

		public void DrawImageUnscaledAndClipped(Image image, Rectangle rect)
		{
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
			this.context.Save();
            this.ContextTranslateWithDifference(0, 0);
            this.context.SetSourceRGB(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f);
			this.context.Arc(x + width / 2, y + height / 2, Math.Min(width / 2, height / 2), startAngle, sweepAngle);
			this.context.Fill();
			this.context.Restore();
		}

		public void DrawPolygon(Pen pen, PointF[] points)
		{
		}

		public void DrawPolygon(Pen pen, Point[] points)
		{
		}

		public void DrawRectangle(Pen pen, Rectangle rect)
		{
			this.context.Save();
            this.ContextTranslateWithDifference(0, 0);
            this.context.SetSourceRGB(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f);
			this.context.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
			this.context.Stroke();
			this.context.Restore();
		}

		public void DrawRectangle(Pen pen, int x, int y, int width, int height)
		{
			DrawRectangle(pen, new Rectangle(x, y, width, height));
		}

		public void DrawRectangle(Pen pen, float x, float y, float width, float height)
		{
			DrawRectangle(pen, new Rectangle((int)x, (int)y, (int)width, (int)height));
		}

		public void DrawRectangles(Pen pen, RectangleF[] rects)
		{
			foreach (RectangleF rec in rects)
				DrawRectangle(pen, new Rectangle((int)rec.X, (int)rec.Y, (int)rec.Width, (int)rec.Height));

		}

		public void DrawRectangles(Pen pen, Rectangle[] rects)
		{
			foreach (Rectangle rec in rects)
				DrawRectangle(pen, rec);
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
                
                float textSize = 15f;
				if (font != null)
				{
					textSize = font.Size;
					if (font.Unit == GraphicsUnit.Point)
						textSize = font.Size * 1 / 72 * 96;
					if (font.Unit == GraphicsUnit.Inch)
						textSize = font.Size * 96;
				}
                this.ContextTranslateWithDifference(layoutRectangle.X, layoutRectangle.Y+ textSize);
                if (brush is SolidBrush sbrush)
				{
					if (sbrush.Color.Name != "0")
						this.context.SetSourceRGBA(sbrush.Color.R / 255f, sbrush.Color.G / 255f, sbrush.Color.B / 255f, 1);
				}
				Pango.Context pangocontext = this.widget.PangoContext;
				string family = pangocontext.FontDescription.Family;
				if (string.IsNullOrWhiteSpace(font.Name) == false)
					family = font.Name;
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
		}

		public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode)
		{
		}

		public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, float tension)
		{
		}

		public void FillClosedCurve(Brush brush, Point[] points)
		{
		}

		public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode)
		{
		}

		public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, float tension)
		{
		}

		public void FillEllipse(Brush brush, Rectangle rect)
		{
		}

		public void FillEllipse(Brush brush, RectangleF rect)
		{
		}

		public void FillEllipse(Brush brush, int x, int y, int width, int height)
		{
		}

		public void FillEllipse(Brush brush, float x, float y, float width, float height)
		{
		}

		public void FillPath(Brush brush, GraphicsPath path)
		{
			if (brush is SolidBrush sbrush)
			{
				//if (path.PathPoints != null && path.PathPoints.Length > 0)
				//{
				//	this.context.Save();
				//	this.context.SetSourceRGB(sbrush.Color.R / 255f, sbrush.Color.G / 255f, sbrush.Color.B / 255f);
				//	this.context.Translate(0, 0);
				//	this.context.LineWidth = 2;
				//	this.context.LineJoin = Cairo.LineJoin.Bevel;
				//	this.context.LineCap = Cairo.LineCap.Butt;

				//	foreach (PointF p in path.PathPoints)
				//	{
				//                    this.context.LineTo(Convert.ToDouble(p.X), Convert.ToDouble(p.Y));
				//                }
				//	this.context.Stroke();
				//	this.context.Restore();
				//}
			}
		}

		public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle)
		{
			if (brush is SolidBrush sbrush)
			{
				this.context.Save();
                this.ContextTranslateWithDifference(0, 0);
                this.context.SetSourceRGB(sbrush.Color.R / 255f, sbrush.Color.G / 255f, sbrush.Color.B / 255f);
				this.context.Arc(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, Math.Min(rect.Width / 2, rect.Height / 2), startAngle, sweepAngle);
				this.context.Fill();
				this.context.Restore();
			}
		}

		public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			FillPie(brush, new Rectangle(x, y, width, height), startAngle, sweepAngle);
		}

		public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			FillPie(brush, new Rectangle((int)x, (int)y, (int)width, (int)height), startAngle, sweepAngle);
		}

		public void FillPolygon(Brush brush, PointF[] points)
		{
		}

		public void FillPolygon(Brush brush, PointF[] points, FillMode fillMode)
		{
		}

		public void FillPolygon(Brush brush, Point[] points)
		{
		}

		public void FillPolygon(Brush brush, Point[] points, FillMode fillMode)
		{
		}

		public void FillRectangle(Brush brush, Rectangle rect)
		{
			FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		public void FillRectangle(Brush brush, RectangleF rect)
		{
			FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		public void FillRectangle(Brush brush, int x, int y, int width, int height)
		{
			FillRectangle(brush, (float)x, (float)y, (float)width, (float)height);
		}

		public void FillRectangle(Brush brush, float x, float y, float width, float height)
		{
			if (brush is SolidBrush sbrush)
			{
				this.context.Save();
				this.ContextTranslateWithDifference(0, 0);
				this.context.SetSourceRGB(sbrush.Color.R / 255f, sbrush.Color.G / 255f, sbrush.Color.B / 255f);
				this.context.Rectangle(x, y, width, height);
				this.context.Fill();
				this.context.Restore();
			}
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
