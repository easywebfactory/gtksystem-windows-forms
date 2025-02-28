
using Cairo;
using Gdk;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms.Interfaces;
using Gtk;
using Matrix = System.Drawing.Drawing2D.Matrix;

namespace System.Drawing;

public sealed class Graphics : MarshalByRefObject, IDeviceContext
{
    private readonly Context? context;
    private readonly Gdk.Rectangle rectangle;
    private readonly IWidget? _widget;
    #region 用于输入与输出的数值调整差值
    internal double DiffLeft { get; set; }
    internal double DiffTop { get; set; }
    //internal int diff_right { get; set; }
    //internal int diff_bottom { get; set; }
    #endregion
    internal Graphics(IWidget? widget, Context? context, Gdk.Rectangle rectangle)
    {
        _widget = widget;
        this.context = context;
        this.rectangle = rectangle;
        Clip = new Region(new Rectangle(this.rectangle.X, this.rectangle.Y, this.rectangle.Width, this.rectangle.Height));
    }
    internal Graphics(Widget? widget, Context? context, Gdk.Rectangle rectangle)
    {
        this.widget = widget;
        this.context = context;
        this.rectangle = rectangle;
        Clip = new Region(new Rectangle(this.rectangle.X, this.rectangle.Y, this.rectangle.Width, this.rectangle.Height));
    }
    internal Graphics(Context? context, Gdk.Rectangle rectangle)
    {
        this.context = context;
        this.rectangle = rectangle;
        Clip = new Region(new Rectangle(this.rectangle.X, this.rectangle.Y, this.rectangle.Width, this.rectangle.Height));
    }
    public delegate bool DrawImageAbort(IntPtr callbackdata);

    public delegate bool EnumerateMetafileProc(EmfPlusRecordType recordType, int flags, int dataSize, IntPtr data, PlayRecordCallback callbackData);

    public Region Clip
    {
        get;
        set;
    }

    public RectangleF ClipBounds => new(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

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
    } = default;

    public float DpiY
    {
        get;
    } = default;

    public InterpolationMode InterpolationMode
    {
        get;
        set;
    }

    public bool IsClipEmpty => false;

    public bool IsVisibleClipEmpty
    {
        get;
    } = default;

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

    public int TextContrast => throw new NotImplementedException();

    public TextRenderingHint TextRenderingHint => throw new NotImplementedException();

    public Matrix Transform => throw new NotImplementedException();

    public RectangleF VisibleClipBounds => new(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

    public void AddMetafileComment(byte[] data)
    {
    }

    public GraphicsContainer BeginContainer()
    {
        throw new NotImplementedException();
    }

    public GraphicsContainer BeginContainer(Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit)
    {
        throw new NotImplementedException();
    }

    public GraphicsContainer BeginContainer(RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit)
    {
        throw new NotImplementedException();
    }
    //internal void ContextTranslateWithDifference(double x,double y)
    //{
    //          this.context.Translate(diff_left + x, diff_top + y);
    //      }
    internal void SetTranslateWithDifference(double x, double y)
    {
        context?.Translate(DiffLeft + x, DiffTop + y);
    }
    internal void SetSourceColor(Pen pen)
    {
        if (pen.Brush is SolidBrush sbrush)
        {
            context?.SetSourceRGBA(sbrush.Color.R / 255f, sbrush.Color.G / 255f, sbrush.Color.B / 255f, sbrush.Color.A / 255f);
        }
        else if (pen.Brush is LinearGradientBrush lbrush)
        {
            var maxsize = Math.Max(DiffLeft + lbrush.Rectangle.Right, DiffTop + lbrush.Rectangle.Bottom); //渐变角度定为方形45度
            using var gradient = new LinearGradient(DiffLeft + lbrush.Rectangle.Left, DiffTop + lbrush.Rectangle.Top, maxsize, maxsize);
            var linearcount = lbrush.LinearColors.Length;
            var idx = 0;
            foreach (var color in lbrush.LinearColors)
            {
                var offset = ++idx / linearcount;
                gradient.AddColorStop(offset, new Cairo.Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f));
            }

            var matrix = new Cairo.Matrix(1, 0, 0, 1, 0, 0);
            matrix.Rotate(Math.PI * 45 / 180);//弧度
            gradient.Matrix = matrix;
            using var pattern = Pattern.Lookup(gradient.Handle, false);
            context?.SetSource(pattern);
        }
        else if (pen.Brush is HatchBrush hbrush)
        {
            context?.SetSourceRGBA(hbrush.ForegroundColor.R / 255f, hbrush.ForegroundColor.G / 255f, hbrush.ForegroundColor.B / 255f, hbrush.ForegroundColor.A / 255f);
        }
        else if (pen.Brush is PathGradientBrush pbrush)
        {
            var maxsize = Math.Max(DiffLeft + pbrush.Rectangle.Right, DiffTop + pbrush.Rectangle.Bottom); //渐变角度定为方形45度
            using var gradient = new LinearGradient(DiffLeft + pbrush.Rectangle.Left, DiffTop + pbrush.Rectangle.Top, maxsize, maxsize);
            var linearcount = pbrush.SurroundColors?.Length ?? 0;
            var centeridx = Math.Floor((double)linearcount / 2);
            var idx = 0;
            if (pbrush.SurroundColors != null)
            {
                foreach (var color in pbrush.SurroundColors)
                {
                    var offset = ++idx / linearcount;
                    if (Math.Abs(idx - centeridx) < 0.0)
                        gradient.AddColorStop(offset,
                            new Cairo.Color(pbrush.CenterColor.R / 255f, pbrush.CenterColor.G / 255f,
                                pbrush.CenterColor.B / 255f, pbrush.CenterColor.A / 255f));
                    else
                        gradient.AddColorStop(offset,
                            new Cairo.Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f));
                }
            }

            var matrix = new Cairo.Matrix(1, 0, 0, 1, 0, 0);
            matrix.Rotate(Math.PI * 45 / 180);//弧度
            gradient.Matrix = matrix;
            using var pattern = Pattern.Lookup(gradient.Handle, false);
            context?.SetSource(pattern);
        }
        else
        {
            context?.SetSourceRGBA(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f, pen.Color.A / 255f);
        }
    }
    public void Clear(Color color)
    {
        if (context != null)
        {
            context.Save();
            context.SetSourceRGB(color.R / 255f, color.G / 255f, color.B / 255f);
            SetTranslateWithDifference(0, 0);
            context.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            context.Fill();
            //this.context.Paint();
            context.Restore();
        }
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
        context.Save();
        SetTranslateWithDifference(0, 0);
        SetSourceColor(pen);
        context.LineWidth = pen.Width;
        context.LineJoin = Cairo.LineJoin.Round;
        context.NewPath();
        double radius = Math.Min(width / 2, height / 2);
        context.Arc(x + radius, y + radius, radius, Math.PI * startAngle / 180, Math.PI * (startAngle + sweepAngle) / 180);
        //this.context.ArcNegative(x, y, Math.Min(width / 2, height / 2), Math.PI * startAngle / 180, Math.PI * sweepAngle / 180); //相反位置
        context.Stroke();
        context.Restore();
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
        for (var i = 1; i < points.Count; i++)
        {
            seedNum += Math.Abs(points[i].X - points[i - 1].X) + Math.Abs(points[i].Y - points[i - 1].Y);
        }
        seedNum += seedNum * 0.2f;
        var pStep = 1 / seedNum;
        var rpoint = new List<PointF>();
        for (float pTime = 0; pTime <= 1; pTime += pStep)
        {
            var lfpr = CalculateBezier(points, pTime);
            var fpr = lfpr[0];
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
        var nList = new List<PointF>
        {
            Capacity = 0
        };
        var listNum = points.Count;
        if (listNum < 2)
        {
            return points.ToList();
        }
        for (var n = 1; n < listNum; n++)
        {
            var nowX = (points[n].X - points[n - 1].X) * time + points[n - 1].X;
            var nowY = (points[n].Y - points[n - 1].Y) * time + points[n - 1].Y;
            var nowP = new PointF(nowX, nowY);
            nList.Add(nowP);
        }

        var p = CalculateBezier(nList, time);
        return p;
    }

    private void DrawBeziersCore(Pen pen, PointF[] points)
    {
        var data = GetBezierPoints(points.ToList());
        DrawLinesCore(pen, data.ToArray());
    }
    public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
    {
        DrawBeziersCore(pen, [new(pt1.X, pt1.Y), new(pt2.X, pt2.Y), new(pt3.X, pt3.Y), new(pt4.X, pt4.Y)]);
    }

    public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
    {
        DrawBeziersCore(pen, [pt1, pt2, pt3, pt4]);
    }

    public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
    {
        DrawBeziersCore(pen, [new(x1, y1), new(x2, y2), new(x3, y3), new(x4, y4)]);
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
            if (context != null)
            {
                context.Save();
                SetTranslateWithDifference(offset, offset);
                SetSourceColor(pen);
                context.LineWidth = pen.Width;
                context.NewPath();
                context.CurveTo(points[0].X, points[0].Y, points[1].X, points[1].Y, points[2].X, points[2].Y);
                if (isClosePath)
                    context.ClosePath();
                if (isfill)
                {
                    context.FillRule = fillmode == FillMode.Winding ? FillRule.Winding : FillRule.EvenOdd;
                    context.Fill();
                }
                else
                    context.Stroke();

                context.Restore();
            }
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
        DrawCurveCore(false, false, pen, points, offset, numberOfSegments, tension, FillMode.Winding);
    }

    public void DrawCurve(Pen pen, PointF[] points, float tension)
    {
        DrawCurveCore(false, false, pen, points, 0, 0, tension, FillMode.Winding);
    }

    public void DrawCurve(Pen pen, Point[] points)
    {
        DrawCurveCore(false, false, pen, Array.ConvertAll(points, p => new PointF(p.X, p.Y)), 0, 0, 0, FillMode.Winding);
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
        if (context != null)
        {
            context.Save();
            SetTranslateWithDifference(x + width / 2, y + height / 2);
            SetSourceColor(pen);
            context.LineWidth = pen.Width;
            context.LineJoin = Cairo.LineJoin.Round;
            context.NewPath();
            var r = (width + height) / 4;
            var rs = Math.Min(0.1, 2 / r);
            for (double t = 0; t < 2 * Math.PI; t += rs)
            {
                var x21 = width * Math.Cos(t) / 2;
                var y21 = height * Math.Sin(t) / 2;
                context.LineTo(x21, y21);
            }

            context.ClosePath();
            if (isfill)
            {
                context.FillRule = fillmode == FillMode.Winding ? FillRule.Winding : FillRule.EvenOdd;
                context.Fill();
            }
            else
            {
                context.Stroke();
            }

            context.Restore();
        }
    }
    public void DrawIcon(Icon icon, Rectangle targetRect)
    {
        DrawImage(new Bitmap(icon.PixbufData), targetRect);
    }

    public void DrawIcon(Icon icon, int x, int y)
    {
        DrawImage(new Bitmap(icon.PixbufData), new Point(x, y));
    }

    public void DrawIconUnstretched(Icon icon, Rectangle targetRect)
    {
        DrawImage(new Bitmap(icon.PixbufData), targetRect);
    }

    public void DrawImage(Image? image, Point point)
    {
        DrawImageScaledCore(image, new Rectangle(point.X, point.Y, image?.Width ?? 0, image?.Height ?? 0), 0, 0, image?.Width ?? 0, image?.Height ?? 0, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, PointF point)
    {
        DrawImageScaledCore(image, new Rectangle((int)point.X, (int)point.Y, image?.Width ?? 0, image?.Height ?? 0), 0, 0, image?.Width ?? 0, image?.Height ?? 0, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, PointF[] destPoints)
    {
        DrawImageScaledCore(image, new Rectangle((int)destPoints[0].X, (int)destPoints[0].Y, (int)destPoints[1].X - (int)destPoints[0].X, (int)destPoints[2].Y - (int)destPoints[0].Y), 0, 0, image?.Width ?? 0, image?.Height ?? 0, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
    }
    private void DrawImageUnscaledCore(Image image, int x, int y, int width, int height, bool clipped = false)
    {
        var img = new Pixbuf(image.PixbufData);
        if (width == 0)
            width = img.Width;
        if (height == 0)
            height = img.Height;
        using var surface = new ImageSurface(Format.Argb32, width, height);
        var newimg = new Pixbuf(surface, 0, 0, width, height);
        img.CopyArea(x, y, width, height, newimg, 0, 0);
        if (context != null)
        {
            context.Save();
            SetTranslateWithDifference(x, y);
            Gdk.CairoHelper.SetSourcePixbuf(context, newimg, 0, 0);

            using (var p = context.GetSource())
            {
                if (p is SurfacePattern pattern)
                {
                    if (CompositingQuality == CompositingQuality.HighSpeed)
                    {
                        pattern.Filter = Filter.Fast;
                    }
                    else if (CompositingQuality == CompositingQuality.HighQuality)
                    {
                        pattern.Filter = Filter.Good;
                    }
                    else
                        pattern.Filter = Filter.Best;
                }
            }

            context.Paint();
            context.Restore();
        }
    }
    private void DrawImageScaledCore(Image? image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes? imageAttrs, DrawImageAbort? callback, IntPtr callbackData)
    {
        if (image != null)
        {
            var img = new Pixbuf(image.PixbufData);
            if (srcWidth == 0)
                srcWidth = img.Width;
            if (srcHeight == 0)
                srcHeight = img.Height;
            if (destRect.Width == 0)
                destRect.Width = img.Width;
            if (destRect.Height == 0)
                destRect.Height = img.Height;
            using var surface = new ImageSurface(Format.Argb32, destRect.Width, destRect.Height);
            var scaleimg = new Pixbuf(surface, 0, 0, destRect.Width, destRect.Height);

            img.Scale(scaleimg, 0, 0, destRect.Width, destRect.Height, srcX, srcY, destRect.Width / srcWidth, destRect.Height / srcHeight, InterpType.Tiles);
            if (context != null)
            {
                context.Save();
                SetTranslateWithDifference(destRect.X, destRect.Y);
                Gdk.CairoHelper.SetSourcePixbuf(context, scaleimg, 0, 0);
                using (var p = context.GetSource())
                {
                    if (p is SurfacePattern pattern)
                    {
                        if (CompositingQuality == CompositingQuality.HighSpeed)
                        {
                            pattern.Filter = Filter.Fast;
                        }
                        else if (CompositingQuality == CompositingQuality.HighQuality)
                        {
                            pattern.Filter = Filter.Good;
                        }
                        else
                            pattern.Filter = Filter.Best;
                    }
                }

                context.Paint();
                context.Restore();
            }
        }
    }
    public void DrawImage(Image? image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)
    {
        DrawImageScaledCore(image, new Rectangle((int)destPoints[0].X, (int)destPoints[0].Y, (int)destPoints[1].X - (int)destPoints[0].X, (int)destPoints[2].Y - (int)destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes? imageAttr)
    {
        DrawImageScaledCore(image, new Rectangle((int)destPoints[0].X, (int)destPoints[0].Y, (int)destPoints[1].X - (int)destPoints[0].X, (int)destPoints[2].Y - (int)destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes? imageAttr, DrawImageAbort? callback)
    {
        DrawImageScaledCore(image, new Rectangle((int)destPoints[0].X, (int)destPoints[0].Y, (int)destPoints[1].X - (int)destPoints[0].X, (int)destPoints[2].Y - (int)destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, callback, IntPtr.Zero);
    }

    public void DrawImage(Image? image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes? imageAttr, DrawImageAbort? callback, int callbackData)
    {
        DrawImageScaledCore(image, new Rectangle((int)destPoints[0].X, (int)destPoints[0].Y, (int)destPoints[1].X - (int)destPoints[0].X, (int)destPoints[2].Y - (int)destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, callback, new IntPtr(callbackData));
    }

    public void DrawImage(Image? image, Point[] destPoints)
    {
        DrawImageScaledCore(image, new Rectangle(destPoints[0].X, destPoints[0].Y, destPoints[1].X - destPoints[0].X, destPoints[2].Y - destPoints[0].Y), 0, 0, image?.Width ?? 0, image?.Height ?? 0, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)
    {
        DrawImageScaledCore(image, new Rectangle(destPoints[0].X, destPoints[0].Y, destPoints[1].X - destPoints[0].X, destPoints[2].Y - destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes? imageAttr)
    {
        DrawImageScaledCore(image, new Rectangle(destPoints[0].X, destPoints[0].Y, destPoints[1].X - destPoints[0].X, destPoints[2].Y - destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes? imageAttr, DrawImageAbort? callback)
    {
        DrawImageScaledCore(image, new Rectangle(destPoints[0].X, destPoints[0].Y, destPoints[1].X - destPoints[0].X, destPoints[2].Y - destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, callback, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes? imageAttr, DrawImageAbort? callback, int callbackData)
    {
        DrawImageScaledCore(image, new Rectangle(destPoints[0].X, destPoints[0].Y, destPoints[1].X - destPoints[0].X, destPoints[2].Y - destPoints[0].Y), 0, 0, srcRect.Width, srcRect.Height, srcUnit, imageAttr, callback, new IntPtr(callbackData));
    }

    public void DrawImage(Image? image, Rectangle rect)
    {
        DrawImageScaledCore(image, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)
    {
        DrawImageScaledCore(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit)
    {
        DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes? imageAttrs)
    {
        DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes? imageAttrs, DrawImageAbort? callback)
    {
        DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes? imageAttrs, DrawImageAbort? callback, IntPtr callbackData)
    {
        DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, callbackData);
    }

    public void DrawImage(Image? image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit)
    {
        DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes? imageAttrs)
    {
        DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes? imageAttrs, DrawImageAbort? callback)
    {
        DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, IntPtr.Zero);
    }

    public void DrawImage(Image? image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes? imageAttrs, DrawImageAbort? callback, IntPtr callbackData)
    {
        DrawImageScaledCore(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, callbackData);
    }

    public void DrawImage(Image? image, RectangleF rect)
    {
        DrawImageScaledCore(image, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height), rect.X, rect.Y, rect.Width, rect.Height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
    {
        DrawImageScaledCore(image, new Rectangle((int)destRect.X, (int)destRect.Y, (int)destRect.Width, (int)destRect.Height), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, int x, int y)
    {
        DrawImageScaledCore(image, new Rectangle(x, y, image?.Width ?? 0, image?.Height ?? 0), 0, 0, image?.Width ?? 0, image?.Height ?? 0, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit)
    {
        DrawImageScaledCore(image, new Rectangle(x, y, srcRect.Width + x, srcRect.Height + y), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, int x, int y, int width, int height)
    {
        DrawImageScaledCore(image, new Rectangle(x, y, width, height), 0, 0, image?.Width ?? 0, image?.Height ?? 0, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, float x, float y)
    {
        DrawImageScaledCore(image, new Rectangle((int)x, (int)y, image?.Width ?? 0, image?.Height ?? 0), x, y, image?.Width ?? 0, image?.Height ?? 0, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
    {
        DrawImageScaledCore(image, new Rectangle((int)x, (int)y, image?.Width ?? 0, image?.Height ?? 0), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit, null, null, IntPtr.Zero);
    }

    public void DrawImage(Image? image, float x, float y, float width, float height)
    {
        DrawImageScaledCore(image, new Rectangle((int)x, (int)y, (int)width, (int)height), 0, 0, width, height, GraphicsUnit.Pixel, null, null, IntPtr.Zero);
    }

    public void DrawImageUnscaled(Image image, Point point)
    {
        DrawImageUnscaledCore(image, point.X, point.Y, image?.Width ?? 0, image?.Height ?? 0);
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
        DrawImageUnscaledCore(image, rect.X, rect.Y, rect.Width, rect.Height, true);
    }
    private void DrawLinesCore(Pen pen, PointF[] points)
    {
        if (points.Length > 0)
        {
            if (context != null)
            {
                context.Save();
                SetTranslateWithDifference(0, 0);
                SetSourceColor(pen);
                context.LineWidth = pen.Width;
                context.NewPath();
                foreach (var p in points)
                {
                    context.LineTo(p.X, p.Y);
                }

                context.Stroke();
                context.Restore();
            }
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
        DrawLinesCore(pen, [new(x1, y1), new(x2, y2)]);
    }

    public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
    {
        DrawLinesCore(pen, [new(x1, y1), new(x2, y2)]);
    }

    public void DrawLines(Pen pen, PointF[] points)
    {
        DrawLinesCore(pen, points);
    }

    public void DrawLines(Pen pen, Point[] points)
    {
        DrawLinesCore(pen, Array.ConvertAll(points, o => new PointF(o.X, o.Y)));
    }
    public void DrawPath(Pen pen, GraphicsPath path)
    {
        DrawPathCore(pen, path, false);
    }
    private void DrawPathCore(Pen pen, GraphicsPath path, bool isfill)
    {
        if (context != null)
        {
            context.Save();
            path.Context = context;
            SetTranslateWithDifference(0, 0);
            SetSourceColor(pen);
            context.LineWidth = pen.Width;
            context.NewPath();
            foreach (var o in path.list)
            {
                if (o is GraphicsPath.FigureMode { Start: true })
                {
                    context.NewSubPath();
                }
                else if (o is GraphicsPath.ArcMode arc)
                {
                    double rw = arc.Rect.Width / 2;
                    double rh = arc.Rect.Height / 2;
                    var ra = Math.Min(rw, rh);
                    context.Arc(arc.Rect.X + rw, arc.Rect.Y + rh, ra, Math.PI * arc.StartAngle / 180,
                        Math.PI * (arc.StartAngle + arc.SweepAngle) / 180);
                }
                else if (o is GraphicsPath.BezierMode bezier)
                {
                    context.MoveTo(bezier.Pt1.X, bezier.Pt1.Y);
                    var data = GetBezierPoints([bezier.Pt1, bezier.Pt2, bezier.Pt3, bezier.Pt4]);
                    foreach (var point in data)
                    {
                        context.LineTo(point.X, point.Y);
                    }
                }
                else if (o is GraphicsPath.BeziersMode beziers)
                {
                    var data = GetBezierPoints(beziers.Points.ToList());
                    foreach (var point in data)
                    {
                        context.LineTo(point.X, point.Y);
                    }
                }
                else if (o is GraphicsPath.ClosedCurveMode closedcurve)
                {
                    context.CurveTo(closedcurve.Points?[0].X ?? 0, closedcurve.Points?[0].Y ?? 0, closedcurve.Points?[1].X ?? 0,
                        closedcurve.Points?[1].Y ?? 0, closedcurve.Points?[2].X ?? 0, closedcurve.Points?[2].Y ?? 0);
                    context.FillRule = closedcurve.Fillmode == FillMode.Winding ? FillRule.Winding : FillRule.EvenOdd;
                    //this.context.Fill();
                    context.ClosePath();
                    context.NewSubPath();
                }
                else if (o is GraphicsPath.CurveMode curve)
                {
                    context.CurveTo(curve.Points?[0].X ?? 0 + curve.Offset, curve.Points?[0].Y ?? 0 + curve.Offset,
                        curve.Points?[1].X ?? 0 + curve.Offset, curve.Points?[1].Y ?? 0 + curve.Offset,
                        curve.Points?[2].X ?? 0 + curve.Offset, curve.Points?[2].Y ?? 0 + curve.Offset);
                }
                else if (o is GraphicsPath.EllipseMode ellipse)
                {
                    context.NewSubPath();
                    var r = (ellipse.Rect.Width + ellipse.Rect.Height) / 4;
                    var rs = Math.Min(0.1, 2 / r);
                    for (double t = 0; t < 2 * Math.PI; t += rs)
                    {
                        var x21 = ellipse.Rect.Width * Math.Cos(t) / 2;
                        var y21 = ellipse.Rect.Height * Math.Sin(t) / 2;
                        context.LineTo(x21 + ellipse.Rect.X + ellipse.Rect.Width / 2,
                            y21 + ellipse.Rect.Y + ellipse.Rect.Height / 2);
                    }

                    context.ClosePath();
                    context.NewSubPath();
                }
                else if (o is GraphicsPath.LineMode line)
                {
                    context.LineTo(line.Pt1.X, line.Pt1.Y);
                    context.LineTo(line.Pt2.X, line.Pt2.Y);
                }
                else if (o is GraphicsPath.LinesMode lines)
                {
                    context.MoveTo(lines.Points?[0].X ?? 0, lines.Points?[0].Y ?? 0);
                    if (lines.Points != null)
                    {
                        foreach (var p in lines.Points)
                        {
                            context.LineTo(p.X, p.Y);
                        }
                    }
                }
                else if (o is GraphicsPath.PieMode pie)
                {
                    context.NewSubPath();
                    double rw = pie.Rect.Width / 2;
                    double rh = pie.Rect.Height / 2;
                    var ra = Math.Min(rw, rh);
                    context.Arc(pie.Rect.X + rw, pie.Rect.Y + rh, ra, Math.PI * pie.StartAngle / 180,
                        Math.PI * (pie.StartAngle + pie.SweepAngle) / 180);
                    context.LineTo(pie.Rect.X + rw, pie.Rect.Y + rh);
                    context.ClosePath();
                    context.NewSubPath();
                }
                else if (o is GraphicsPath.PolygonMode polygon)
                {
                    context.NewSubPath();
                    if (polygon.Points != null)
                    {
                        foreach (var p in polygon.Points)
                        {
                            context.LineTo(p.X, p.Y);
                        }
                    }

                    context.ClosePath();
                    context.NewSubPath();
                }
                else if (o is GraphicsPath.RectangleMode rectangleValue)
                {
                    context.Rectangle(rectangleValue.Rect.X, rectangleValue.Rect.Y, rectangleValue.Rect.Width, rectangleValue.Rect.Height);
                }
                else if (o is GraphicsPath.RectanglesMode rectangles)
                {
                    if (rectangles.Rects != null)
                    {
                        foreach (var rect in rectangles.Rects)
                        {
                            context.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                        }
                    }
                }
                else if (o is GraphicsPath.StringMode str)
                {
                    var text = str.Text;
                    if (str.LayoutRect.Width > 0)
                    {
                        while (text?.Length > 0 && context.TextExtents(text).Width > str.LayoutRect.Width)
                            text = text.Substring(0, text.Length - 1);
                    }

                    var textSize = str.EmSize < 1 ? 14f : str.EmSize;
                    var font = str.Family;
                    var family = font?.Name;
                    if (_widget != null)
                    {
                        var pangocontext = _widget.PangoContext;
                        family = pangocontext.FontDescription.Family;
                        var pangoFamily = Array.Find(pangocontext.Families, f => f.Name == font?.Name);
                        if (pangoFamily == null)
                            family = pangocontext.FontDescription.Family;
                    }
                    else if (widget != null)
                    {
                        var pangocontext = widget.PangoContext;
                        family = pangocontext.FontDescription.Family;
                        var pangoFamily = Array.Find(pangocontext.Families, f => f.Name == font?.Name);
                        if (pangoFamily == null)
                            family = pangocontext.FontDescription.Family;
                    }

                    context.SelectFontFace(family, str.Style == 2 ? FontSlant.Italic : FontSlant.Normal,
                        str.Style == 1 ? FontWeight.Bold : FontWeight.Normal);
                    context.SetFontSize(textSize);
                    var textext = context.TextExtents(text);
                    context.MoveTo(str.LayoutRect.X, str.LayoutRect.Y + textext.Height);
                    context.ShowText(text);
                }
                else if (o is GraphicsPath.PathMode { Path: not null } addpath)
                {
                    DrawPath(pen, addpath.Path);
                }

                if (path.IsCloseAllFigures || o is GraphicsPath.FigureMode { Close: true })
                {
                    context.ClosePath();
                }
            }

            if (isfill)
                context.Fill();
            else
                context.Stroke();

            if (path.Matrix != null)
            {
                context.Matrix = ConvertToMatrix(path.Matrix);
            }

            context.Restore();
        }
    }

    private Cairo.Matrix ConvertToMatrix(Matrix? matrix)
    {
        var cairoMatrix = new Cairo.Matrix(matrix?.M11 ?? 0, matrix?.M12 ?? 0, matrix?.M21 ?? 0, matrix?.M22 ?? 0, matrix?.Dx ?? 0, matrix?.Dy ?? 0);
        cairoMatrix.Init(matrix?.M11 ?? 0, matrix?.M12 ?? 0, matrix?.M21 ?? 0, matrix?.M22 ?? 0, matrix?.Dx ?? 0, matrix?.Dy ?? 0);

        cairoMatrix.Translate(matrix?.OffsetX ?? 0, matrix?.OffsetY ?? 0);
        cairoMatrix.Scale(matrix?.ScaleX ?? 0, matrix?.ScaleY ?? 0);
        cairoMatrix.Rotate(matrix?.Angle ?? 0);
        var multiplyValue = matrix?.MultiplyValue;
        if (multiplyValue != null)
        {
            cairoMatrix.Multiply(ConvertToMatrix(multiplyValue));
        }

        if (matrix?.InvertValue ?? false)
            cairoMatrix.Invert();

        return cairoMatrix;
    }
    private void DrawPieCore(bool isFill, Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
    {
        context.Save();
        SetTranslateWithDifference(0, 0);
        SetSourceColor(pen);
        context.LineWidth = pen.Width;
        context.NewPath();
        double radius = Math.Min(width / 2, height / 2);
        context.MoveTo(x + radius, y + radius);
        context.Arc(x + radius, y + radius, radius, Math.PI * startAngle / 180, Math.PI * (startAngle + sweepAngle) / 180);
        context.LineTo(x + radius, y + radius);
        context.ClosePath();
        if (isFill)
            context.Fill();
        else
            context.Stroke();
        context.Restore();
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
        DrawPie(pen, (float)x, y, width, height, startAngle, sweepAngle);
    }

    public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
    {
        DrawPieCore(false, pen, x, y, width, height, startAngle, sweepAngle);
    }

    private void DrawPolygonCore(bool isFill, Pen pen, PointF[] points, FillMode fillmode)
    {
        if (points.Length > 0)
        {
            if (context != null)
            {
                context.Save();
                SetTranslateWithDifference(0, 0);
                SetSourceColor(pen);
                context.LineWidth = pen.Width;
                context.NewPath();
                foreach (var p in points)
                {
                    context.LineTo(p.X, p.Y);
                }

                context.ClosePath();
                if (isFill)
                {
                    context.FillRule = fillmode == FillMode.Winding ? FillRule.Winding : FillRule.EvenOdd;
                    context.Fill();
                }
                else
                    context.Stroke();

                context.Restore();
            }
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
        if (context != null)
        {
            context.Save();
            SetTranslateWithDifference(0, 0);
            SetSourceColor(pen);
            context.NewPath();
            context.LineWidth = pen.Width;
            context.Rectangle(x, y, width, height);
            if (isFill)
                context.Fill();
            else
                context.Stroke();
            context.Restore();
        }
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
        foreach (var rect in rects)
            DrawRectangle(pen, rect);

    }

    public void DrawRectangles(Pen pen, Rectangle[] rects)
    {
        foreach (var rect in rects)
            DrawRectangle(pen, rect);
    }

    public void DrawString(string s, Font font, Brush? brush, PointF point)
    {
        DrawString(s, font, brush, new RectangleF(point.X + rectangle.X, point.Y + rectangle.Y, rectangle.Width, rectangle.Height), new StringFormat());
    }

    public void DrawString(string s, Font font, Brush? brush, PointF point, StringFormat format)
    {
        DrawString(s, font, brush, new RectangleF(point.X + rectangle.X, point.Y + rectangle.Y, rectangle.Width, rectangle.Height), format);
    }

    public void DrawString(string s, Font font, Brush? brush, RectangleF layoutRectangle)
    {
        DrawString(s, font, brush, layoutRectangle, new StringFormat());
    }

    public void DrawString(string text, Font font, Brush? brush, RectangleF layoutRectangle, StringFormat format)
    {
        if (string.IsNullOrEmpty(text) == false)
        {
            if (context != null)
            {
                context.Save();

                var textSize = 14f;
                if (font != null)
                {
                    textSize = font.Size;
                    if (font.Unit == GraphicsUnit.Point)
                        textSize = font.Size * 1 / 72 * 96;
                    if (font.Unit == GraphicsUnit.Inch)
                        textSize = font.Size * 96;
                }

                var family = font?.Name;
                if (_widget != null)
                {
                    var pangocontext = _widget.PangoContext;
                    family = pangocontext.FontDescription.Family;
                    var pangoFamily = Array.Find(pangocontext.Families, f => f.Name == font?.Name);
                    if (pangoFamily == null)
                        family = pangocontext.FontDescription.Family;
                }
                else if (widget != null)
                {
                    var pangocontext = widget.PangoContext;
                    family = pangocontext.FontDescription.Family;
                    var pangoFamily = Array.Find(pangocontext.Families, f => f.Name == font?.Name);
                    if (pangoFamily == null)
                        family = pangocontext.FontDescription.Family;
                }

                context.SetFontSize(textSize);
                context.SelectFontFace(family,
                    font != null && (font.Style & FontStyle.Italic) != 0 ? FontSlant.Italic : FontSlant.Normal,
                    font != null && (font.Style & FontStyle.Bold) != 0 ? FontWeight.Bold : FontWeight.Normal);
                var textext = context.TextExtents(text);
                SetTranslateWithDifference(layoutRectangle.X, layoutRectangle.Y + textext.Height);
                SetSourceColor(new Pen(brush, 1));
                context.ShowText(text);
                context.Stroke();
                context.Restore();
            }
        }
    }

    public void DrawString(string s, Font font, Brush? brush, float x, float y)
    {
        DrawString(s, font, brush, new RectangleF(x + rectangle.X, y + rectangle.Y, rectangle.Width, rectangle.Height), new StringFormat());
    }
    public void DrawString(string s, Font font, Brush? brush, float x, float y, StringFormat format)
    {
        DrawString(s, font, brush, new RectangleF(x + rectangle.X, y + rectangle.Y, rectangle.Width, rectangle.Height), format);
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

    public void FillClosedCurve(Brush? brush, PointF[] points)
    {
        DrawCurveCore(true, true, new Pen(brush, 0), points, 0, 0, 0, FillMode.Winding);
    }

    public void FillClosedCurve(Brush? brush, PointF[] points, FillMode fillmode)
    {
        DrawCurveCore(true, true, new Pen(brush, 0), points, 0, 0, 0, fillmode);
    }

    public void FillClosedCurve(Brush? brush, PointF[] points, FillMode fillmode, float tension)
    {
        DrawCurveCore(true, true, new Pen(brush, 0), points, 0, 0, tension, fillmode);
    }

    public void FillClosedCurve(Brush? brush, Point[] points)
    {
        DrawCurveCore(true, true, new Pen(brush, 0), Array.ConvertAll(points, p => new PointF(p.X, p.Y)), 0, 0, 0, FillMode.Winding);
    }

    public void FillClosedCurve(Brush? brush, Point[] points, FillMode fillmode)
    {
        DrawCurveCore(true, true, new Pen(brush, 0), Array.ConvertAll(points, p => new PointF(p.X, p.Y)), 0, 0, 0, fillmode);
    }

    public void FillClosedCurve(Brush? brush, Point[] points, FillMode fillmode, float tension)
    {
        DrawCurveCore(true, true, new Pen(brush, 0), Array.ConvertAll(points, p => new PointF(p.X, p.Y)), 0, 0, tension, fillmode);
    }

    public void FillEllipse(Brush? brush, Rectangle rect)
    {
        DrawEllipseCore(new Pen(brush, 0), rect.X, rect.Y, rect.Width, rect.Height, true, FillMode.Winding);
    }

    public void FillEllipse(Brush? brush, RectangleF rect)
    {
        DrawEllipseCore(new Pen(brush, 0), rect.X, rect.Y, rect.Width, rect.Height, true, FillMode.Winding);
    }

    public void FillEllipse(Brush? brush, int x, int y, int width, int height)
    {
        DrawEllipseCore(new Pen(brush, 0), x, y, width, height, true, FillMode.Winding);
    }

    public void FillEllipse(Brush? brush, float x, float y, float width, float height)
    {
        DrawEllipseCore(new Pen(brush, 0), x, y, width, height, true, FillMode.Winding);
    }

    public void FillPath(Brush? brush, GraphicsPath path)
    {
        DrawPathCore(new Pen(brush, 1), path, true);
    }

    public void FillPie(Brush? brush, Rectangle rect, float startAngle, float sweepAngle)
    {
        FillPie(brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
    }

    public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
    {
        DrawPieCore(true, new Pen(brush, 0), x, y, width, height, startAngle, sweepAngle);
    }

    public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
    {
        DrawPieCore(true, new Pen(brush, 0), x, y, width, height, startAngle, sweepAngle);
    }

    public void FillPolygon(Brush? brush, PointF[] points)
    {
        FillPolygon(brush, points, FillMode.Winding);
    }

    public void FillPolygon(Brush? brush, PointF[] points, FillMode fillMode)
    {
        DrawPolygonCore(true, new Pen(brush, 0), points, fillMode);
    }

    public void FillPolygon(Brush? brush, Point[] points)
    {
        FillPolygon(brush, points, FillMode.Winding);
    }

    public void FillPolygon(Brush? brush, Point[] points, FillMode fillMode)
    {
        DrawPolygonCore(true, new Pen(brush, 0), Array.ConvertAll(points, p => new PointF(p.X, p.Y)), fillMode);
    }

    public void FillRectangle(Brush? brush, Rectangle rect)
    {
        DrawRectangleCore(true, new Pen(brush, 0), rect.X, rect.Y, rect.Width, rect.Height);
    }

    public void FillRectangle(Brush? brush, RectangleF rect)
    {
        DrawRectangleCore(true, new Pen(brush, 0), rect.X, rect.Y, rect.Width, rect.Height);
    }

    public void FillRectangle(Brush? brush, int x, int y, int width, int height)
    {
        DrawRectangleCore(true, new Pen(brush, 0), x, y, width, height);
    }

    public void FillRectangle(Brush? brush, float x, float y, float width, float height)
    {
        DrawRectangleCore(true, new Pen(brush, 0), x, y, width, height);
    }

    public void FillRectangles(Brush? brush, RectangleF[] rects)
    {
        foreach (var rect in rects)
            FillRectangle(brush, rect);
    }

    public void FillRectangles(Brush? brush, Rectangle[] rects)
    {
        foreach (var rect in rects)
            FillRectangle(brush, rect);
    }

    public void FillRegion(Brush brush, Region region)
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Graphics FromHdc(IntPtr hdc)
    {
        throw new NotImplementedException();
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Graphics FromHdc(IntPtr hdc, IntPtr hdevice)
    {
        throw new NotImplementedException();
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Graphics FromHdcInternal(IntPtr hdc)
    {
        throw new NotImplementedException();
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Graphics FromHwnd(IntPtr hwnd)
    {
        throw new NotImplementedException();
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static Graphics FromHwndInternal(IntPtr hwnd)
    {
        throw new NotImplementedException();
    }

    private static ImageSurface? imagesurface;
    private static Surface? simisurface;
    private static Context? imagecontext;
    /// <summary>
    /// 使用此方法必须要执行Flush()方法输出Image
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Graphics? FromImage(Image image)
    {
        var _width = image.Width;
        var _height = image.Height;

        if (_width < 1)
            throw new ArgumentOutOfRangeException(nameof(image.Width));
        if (_height < 1)
            throw new ArgumentOutOfRangeException(nameof(image.Height));

        if (imagesurface == null)
            imagesurface = new ImageSurface(Format.Argb32, _width, _height);

        simisurface?.Dispose();
        simisurface = imagesurface.CreateSimilar(Content.ColorAlpha, _width, _height);
        imagecontext?.Dispose();
        imagecontext = new Context(simisurface);
        var o = (object)image;
        var widgetValue = o as IWidget;
        if (widgetValue != null)
        {
            return new Graphics(widgetValue, imagecontext, new Gdk.Rectangle(0, 0, _width, _height));
        }

        return null;
    }

    public void Flush()
    {
        Flush(FlushIntention.Flush);
    }

    public void Flush(FlushIntention intention)
    {
        if (widget is Image image && simisurface is { Status: Cairo.Status.Success })
        {
            image.Pixbuf = new Pixbuf(simisurface, 0, 0, image.Width, image.Height);
        }
    }


    [EditorBrowsable(EditorBrowsableState.Never)]
    public object? GetContextInfo()
    {
        return context;
    }

    public static IntPtr GetHalftonePalette()
    {
        throw new NotImplementedException();
    }

    public IntPtr GetHdc()
    {
        throw new NotImplementedException();
    }

    public Color GetNearestColor(Color color)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public bool IsVisible(PointF point)
    {
        throw new NotImplementedException();
    }

    public bool IsVisible(Rectangle rect)
    {
        throw new NotImplementedException();
    }

    public bool IsVisible(RectangleF rect)
    {
        throw new NotImplementedException();
    }

    public bool IsVisible(int x, int y)
    {
        throw new NotImplementedException();
    }

    public bool IsVisible(int x, int y, int width, int height)
    {
        throw new NotImplementedException();
    }

    public bool IsVisible(float x, float y)
    {
        throw new NotImplementedException();
    }

    public bool IsVisible(float x, float y, float width, float height)
    {
        throw new NotImplementedException();
    }

    public Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat)
    {
        throw new NotImplementedException();
    }

    public SizeF MeasureString(string text, Font font)
    {
        throw new NotImplementedException();
    }

    public SizeF MeasureString(string text, Font font, PointF origin, StringFormat stringFormat)
    {
        throw new NotImplementedException();
    }

    public SizeF MeasureString(string text, Font font, SizeF layoutArea)
    {
        throw new NotImplementedException();
    }

    public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat)
    {
        throw new NotImplementedException();
    }

    public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled)
    {
        throw new NotImplementedException();
    }

    public SizeF MeasureString(string text, Font font, int width)
    {
        return MeasureString(text, font, width, StringFormat.GenericDefault);
    }

    public SizeF MeasureString(string text, Font font, int width, StringFormat format)
    {
        var textSize = 14f;
        if (font != null)
        {
            textSize = font.Size;
            if (font.Unit == GraphicsUnit.Point)
                textSize = font.Size * 1 / 72 * 96;
            if (font.Unit == GraphicsUnit.Inch)
                textSize = font.Size * 96;
        }
        var family = font?.Name;
        if (_widget != null)
        {
            var pangocontext = _widget.PangoContext;
            family = pangocontext.FontDescription.Family;
            var pangoFamily = Array.Find(pangocontext.Families, f => f.Name == font?.Name);
            if (pangoFamily == null)
                family = pangocontext.FontDescription.Family;
        }
        if (widget != null)
        {
            var pangocontext = widget.PangoContext;
            family = pangocontext.FontDescription.Family;
            var pangoFamily = Array.Find(pangocontext.Families, f => f.Name == font?.Name);
            if (pangoFamily == null)
                family = pangocontext.FontDescription.Family;
        }

        if (context != null)
        {
            context.SelectFontFace(family, font?.Italic ?? false ? FontSlant.Italic : FontSlant.Normal,
                font?.Bold ?? false ? FontWeight.Bold : FontWeight.Normal);
            context.SetFontSize(textSize);
            var extents = context.TextExtents(text);
            return new SizeF((float)Math.Max(width, extents.Width), (float)extents.Height);
        }

        return default;
    }

    public void MultiplyTransform(Matrix? matrix)
    {
        context?.Matrix?.Multiply(ConvertToMatrix(matrix));
    }

    public void MultiplyTransform(Matrix? matrix, MatrixOrder order)
    {
        context?.Matrix?.Multiply(ConvertToMatrix(matrix));
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
        context?.ResetClip();
    }

    public void ResetTransform()
    {
        context?.Rotate(Math.PI / 180 * _angle * -1);
    }

    public void Restore(GraphicsState gstate)
    {
        context?.Restore();
    }
    private float _angle;
    private readonly Widget? widget;

    public void RotateTransform(float angle)
    {
        context?.Rotate(Math.PI / 180 * angle);
        _angle = angle;
    }

    public void RotateTransform(float angle, MatrixOrder order)
    {
        context?.Rotate(Math.PI / 180 * angle);
        _angle = angle;
    }

    public GraphicsState Save()
    {
        return new GraphicsState();
    }

    public void ScaleTransform(float sx, float sy)
    {
        context?.Scale(sx, sy);
    }

    public void ScaleTransform(float sx, float sy, MatrixOrder order)
    {
        context?.Scale(sx, sy);
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
        SetTranslateWithDifference(dx, dy);
    }
}