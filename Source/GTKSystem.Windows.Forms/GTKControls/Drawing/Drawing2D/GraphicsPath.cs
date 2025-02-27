using System.Collections;
using Cairo;

namespace System.Drawing.Drawing2D;

/// <summary>Represents a series of connected lines and curves. This class cannot be inherited.</summary>
public sealed class GraphicsPath : MarshalByRefObject, ICloneable, IDisposable
{
    /// <summary>Gets or sets a <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines how the interiors of shapes in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> are filled.</summary>
    /// <returns>A <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that specifies how the interiors of shapes in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> are filled.</returns>
    public FillMode FillMode
    {
        get;
        set;
    }

    /// <summary>Gets a <see cref="T:System.Drawing.Drawing2D.PathData" /> that encapsulates arrays of points (<paramref name="points" />) and types (<paramref name="types" />) for this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <returns>A <see cref="T:System.Drawing.Drawing2D.PathData" /> that encapsulates arrays for both the points and types for this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</returns>
    public PathData? PathData
    {
        get;
        private set;
    }

    /// <summary>Gets the points in the path.</summary>
    /// <returns>An array of <see cref="T:System.Drawing.PointF" /> objects that represent the path.</returns>
    public PointF[]? PathPoints
    {
        get;
        private set;
    }

    /// <summary>Gets the types of the corresponding points in the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> array.</summary>
    /// <returns>An array of bytes that specifies the types of the corresponding points in the path.</returns>
    public byte[]? PathTypes
    {
        get;
        private set;
    }

    /// <summary>Gets the number of elements in the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> or the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathTypes" /> array.</summary>
    /// <returns>An integer that specifies the number of elements in the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> or the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathTypes" /> array.</returns>
    public int PointCount
    {
        get;
        private set;
    }
    public List<PointF> sizePoints = [];
    private void AddPathPoints(params PointF[] points) {
        foreach (var p in points)
            sizePoints.Add(p);
    }
    public class ArcMode
    {
        public RectangleF Rect { get; set; }
        public float StartAngle { get; set; }
        public float SweepAngle { get; set; }
    }
    public class BezierMode
    {
        public PointF Pt1 { get; set; }
        public PointF Pt2 { get; set; }
        public PointF Pt3 { get; set; }
        public PointF Pt4 { get; set; }
    }
    public class BeziersMode
    {
        public PointF[]? Points { get; set; }
    }
    public class ClosedCurveMode
    {
        public PointF[]? Points { get; set; }
        public float Tension { get; set; }
        public FillMode Fillmode { get; set; }
    }
    public class CurveMode
    {
        public PointF[]? Points { get; set; }
        public int Offset { get; set; }
        public int NumberOfSegments { get; set; }
        public float Tension { get; set; }
    }
    public class EllipseMode
    {
        public RectangleF Rect { get; set; }
    }
    public class LineMode
    {
        public PointF Pt1 { get; set; }
        public PointF Pt2 { get; set; }
    }
    public class LinesMode
    {
        public PointF[]? Points { get; set; }
    }
    public class PathMode
    {
        public GraphicsPath? Path { get; set; }
        public bool Connect { get; set; }
    }
    public class PieMode
    {
        public RectangleF Rect { get; set; }
        public float StartAngle { get; set; }
        public float SweepAngle { get; set; }
    }
    public class PolygonMode
    {
        public PointF[]? Points { get; set; }
    }
    public class RectangleMode
    {
        public RectangleF Rect { get; set; }
    }
    public class RectanglesMode
    {
        public RectangleF[]? Rects { get; set; }
    }
    public class StringMode
    {
        public string Text { get; set; } = string.Empty;
        public FontFamily? Family { get; set; }
        public int Style { get; set; }
        public float EmSize { get; set; }
        public RectangleF LayoutRect { get; set; }
        public StringFormat? Format { get; set; }
    }
    public class FigureMode
    {
        public bool Start { get; set; }
        public bool Close { get; set; }
    }
    internal Context? Context { get; set; }
    internal ArrayList list = new();

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> class with a <see cref="P:System.Drawing.Drawing2D.GraphicsPath.FillMode" /> value of <see cref="F:System.Drawing.Drawing2D.FillMode.Alternate" />.</summary>
    public GraphicsPath()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> class with the specified <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration.</summary>
    /// <param name="fillMode">The <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines how the interior of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> is filled.</param>
    public GraphicsPath(FillMode fillMode) : this(new PointF[0], null, fillMode)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> array with the specified <see cref="T:System.Drawing.Drawing2D.PathPointType" /> and <see cref="T:System.Drawing.PointF" /> arrays.</summary>
    /// <param name="pts">An array of <see cref="T:System.Drawing.PointF" /> structures that defines the coordinates of the points that make up this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</param>
    /// <param name="types">An array of <see cref="T:System.Drawing.Drawing2D.PathPointType" /> enumeration elements that specifies the type of each corresponding point in the <paramref name="pts" /> array.</param>
    public GraphicsPath(PointF[]? pts, byte[]? types) : this(pts, types, FillMode.Alternate)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> array with the specified <see cref="T:System.Drawing.Drawing2D.PathPointType" /> and <see cref="T:System.Drawing.PointF" /> arrays and with the specified <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration element.</summary>
    /// <param name="pts">An array of <see cref="T:System.Drawing.PointF" /> structures that defines the coordinates of the points that make up this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</param>
    /// <param name="types">An array of <see cref="T:System.Drawing.Drawing2D.PathPointType" /> enumeration elements that specifies the type of each corresponding point in the <paramref name="pts" /> array.</param>
    /// <param name="fillMode">A <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that specifies how the interiors of shapes in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> are filled.</param>
    public GraphicsPath(PointF[]? pts, byte[]? types, FillMode fillMode)
    {
        PathPoints = pts;
        PointCount = pts?.Length??0;
        PathTypes = types;
        FillMode = fillMode;
        PathData = new PathData { Points = pts, Types = types };
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> class with the specified <see cref="T:System.Drawing.Drawing2D.PathPointType" /> and <see cref="T:System.Drawing.Point" /> arrays.</summary>
    /// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that defines the coordinates of the points that make up this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</param>
    /// <param name="types">An array of <see cref="T:System.Drawing.Drawing2D.PathPointType" /> enumeration elements that specifies the type of each corresponding point in the <paramref name="pts" /> array.</param>
    public GraphicsPath(Point[] pts, byte[]? types) : this(pts, types, FillMode.Alternate)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> class with the specified <see cref="T:System.Drawing.Drawing2D.PathPointType" /> and <see cref="T:System.Drawing.Point" /> arrays and with the specified <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration element.</summary>
    /// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that defines the coordinates of the points that make up this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</param>
    /// <param name="types">An array of <see cref="T:System.Drawing.Drawing2D.PathPointType" /> enumeration elements that specifies the type of each corresponding point in the <paramref name="pts" /> array.</param>
    /// <param name="fillMode">A <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that specifies how the interiors of shapes in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> are filled.</param>
    public GraphicsPath(Point[] pts, byte[]? types, FillMode fillMode) : this(Array.ConvertAll(pts, o => new PointF(o.X, o.Y)), types, fillMode)
    {
    }

    /// <summary>Appends an elliptical arc to the current figure.</summary>
    /// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangular bounds of the ellipse from which the arc is taken.</param>
    /// <param name="startAngle">The starting angle of the arc, measured in degrees clockwise from the x-axis.</param>
    /// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the arc.</param>
    public void AddArc(Rectangle rect, float startAngle, float sweepAngle)
    {
        AddPathPoints(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Bottom));
        list.Add(new ArcMode { Rect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), StartAngle = startAngle, SweepAngle = sweepAngle });
    }

    /// <summary>Appends an elliptical arc to the current figure.</summary>
    /// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangular bounds of the ellipse from which the arc is taken.</param>
    /// <param name="startAngle">The starting angle of the arc, measured in degrees clockwise from the x-axis.</param>
    /// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the arc.</param>
    public void AddArc(RectangleF rect, float startAngle, float sweepAngle)
    {
        AddPathPoints(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Bottom));
        list.Add(new ArcMode { Rect = rect, StartAngle = startAngle, SweepAngle = sweepAngle });
    }

    /// <summary>Appends an elliptical arc to the current figure.</summary>
    /// <param name="x">The x-coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
    /// <param name="width">The width of the rectangular region that defines the ellipse from which the arc is drawn.</param>
    /// <param name="height">The height of the rectangular region that defines the ellipse from which the arc is drawn.</param>
    /// <param name="startAngle">The starting angle of the arc, measured in degrees clockwise from the x-axis.</param>
    /// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the arc.</param>
    public void AddArc(int x, int y, int width, int height, float startAngle, float sweepAngle)
    {
        AddPathPoints(new PointF(x, y), new PointF(x + width, y + height));
        list.Add(new ArcMode { Rect = new RectangleF(x, y, width, height), StartAngle = startAngle, SweepAngle = sweepAngle });
    }

    /// <summary>Appends an elliptical arc to the current figure.</summary>
    /// <param name="x">The x-coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
    /// <param name="width">The width of the rectangular region that defines the ellipse from which the arc is drawn.</param>
    /// <param name="height">The height of the rectangular region that defines the ellipse from which the arc is drawn.</param>
    /// <param name="startAngle">The starting angle of the arc, measured in degrees clockwise from the x-axis.</param>
    /// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the arc.</param>
    public void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
    {
        AddPathPoints(new PointF(x, y), new PointF(x + width, y + height));
        list.Add(new ArcMode { Rect = new RectangleF(x, y, width, height), StartAngle = startAngle, SweepAngle = sweepAngle });
    }

    /// <summary>Adds a cubic Bézier curve to the current figure.</summary>
    /// <param name="pt1">A <see cref="T:System.Drawing.Point" /> that represents the starting point of the curve.</param>
    /// <param name="pt2">A <see cref="T:System.Drawing.Point" /> that represents the first control point for the curve.</param>
    /// <param name="pt3">A <see cref="T:System.Drawing.Point" /> that represents the second control point for the curve.</param>
    /// <param name="pt4">A <see cref="T:System.Drawing.Point" /> that represents the endpoint of the curve.</param>
    public void AddBezier(Point pt1, Point pt2, Point pt3, Point pt4)
    {
        AddPathPoints(new PointF(pt1.X, pt1.Y), new PointF(pt2.X, pt2.Y), new PointF(pt3.X, pt3.Y), new PointF(pt4.X, pt4.Y));
        list.Add(new BezierMode { Pt1 = new PointF(pt1.X, pt1.Y), Pt2 = new PointF(pt2.X, pt2.Y), Pt3 = new PointF(pt3.X, pt3.Y), Pt4 = new PointF(pt4.X, pt4.Y) });
    }

    /// <summary>Adds a cubic Bézier curve to the current figure.</summary>
    /// <param name="pt1">A <see cref="T:System.Drawing.PointF" /> that represents the starting point of the curve.</param>
    /// <param name="pt2">A <see cref="T:System.Drawing.PointF" /> that represents the first control point for the curve.</param>
    /// <param name="pt3">A <see cref="T:System.Drawing.PointF" /> that represents the second control point for the curve.</param>
    /// <param name="pt4">A <see cref="T:System.Drawing.PointF" /> that represents the endpoint of the curve.</param>
    public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
    {
        AddPathPoints(new PointF(pt1.X, pt1.Y), new PointF(pt2.X, pt2.Y), new PointF(pt3.X, pt3.Y), new PointF(pt4.X, pt4.Y));
        list.Add(new BezierMode { Pt1 = new PointF(pt1.X, pt1.Y), Pt2 = new PointF(pt2.X, pt2.Y), Pt3 = new PointF(pt3.X, pt3.Y), Pt4 = new PointF(pt4.X, pt4.Y) });
    }

    /// <summary>Adds a cubic Bézier curve to the current figure.</summary>
    /// <param name="x1">The x-coordinate of the starting point of the curve.</param>
    /// <param name="y1">The y-coordinate of the starting point of the curve.</param>
    /// <param name="x2">The x-coordinate of the first control point for the curve.</param>
    /// <param name="y2">The y-coordinate of the first control point for the curve.</param>
    /// <param name="x3">The x-coordinate of the second control point for the curve.</param>
    /// <param name="y3">The y-coordinate of the second control point for the curve.</param>
    /// <param name="x4">The x-coordinate of the endpoint of the curve.</param>
    /// <param name="y4">The y-coordinate of the endpoint of the curve.</param>
    public void AddBezier(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
    {
        AddPathPoints(new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3), new PointF(x4, y4));
        list.Add(new BezierMode { Pt1 = new PointF(x1, y1), Pt2 = new PointF(x2, y2), Pt3 = new PointF(x3, y3), Pt4 = new PointF(x4, y4) });
    }

    /// <summary>Adds a cubic Bézier curve to the current figure.</summary>
    /// <param name="x1">The x-coordinate of the starting point of the curve.</param>
    /// <param name="y1">The y-coordinate of the starting point of the curve.</param>
    /// <param name="x2">The x-coordinate of the first control point for the curve.</param>
    /// <param name="y2">The y-coordinate of the first control point for the curve.</param>
    /// <param name="x3">The x-coordinate of the second control point for the curve.</param>
    /// <param name="y3">The y-coordinate of the second control point for the curve.</param>
    /// <param name="x4">The x-coordinate of the endpoint of the curve.</param>
    /// <param name="y4">The y-coordinate of the endpoint of the curve.</param>
    public void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
    {
        AddPathPoints(new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3), new PointF(x4, y4));
        list.Add(new BezierMode { Pt1 = new PointF(x1, y1), Pt2 = new PointF(x2, y2), Pt3 = new PointF(x3, y3), Pt4 = new PointF(x4, y4) });
    }

    /// <summary>Adds a sequence of connected cubic Bézier curves to the current figure.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curves.</param>
    public void AddBeziers(PointF[] points)
    {
        AddPathPoints(points);
        list.Add(new BeziersMode { Points = points });
    }

    /// <summary>Adds a sequence of connected cubic Bézier curves to the current figure.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curves.</param>
    public void AddBeziers(params Point[] points)
    {
        AddPathPoints(Array.ConvertAll(points, p => new PointF(p.X, p.Y)));
        list.Add(new BeziersMode { Points = Array.ConvertAll(points, p => new PointF(p.X, p.Y)) });
    }

    /// <summary>Adds a closed curve to this path. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curve.</param>
    public void AddClosedCurve(PointF[] points)
    {
        AddPathPoints(points);
        list.Add(new ClosedCurveMode { Points = points });
    }

    /// <summary>Adds a closed curve to this path. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curve.</param>
    /// <param name="tension">A value between from 0 through 1 that specifies the amount that the curve bends between points, with 0 being the smallest curve (sharpest corner) and 1 being the smoothest curve.</param>
    public void AddClosedCurve(PointF[] points, float tension)
    {
        AddPathPoints(points);
        list.Add(new ClosedCurveMode { Points = points, Tension = tension });
    }

    /// <summary>Adds a closed curve to this path. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curve.</param>
    public void AddClosedCurve(Point[] points)
    {
        AddPathPoints(Array.ConvertAll(points, p => new PointF(p.X, p.Y)));
        list.Add(new ClosedCurveMode { Points = Array.ConvertAll(points, p => new PointF(p.X, p.Y)) });
    }

    /// <summary>Adds a closed curve to this path. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curve.</param>
    /// <param name="tension">A value between from 0 through 1 that specifies the amount that the curve bends between points, with 0 being the smallest curve (sharpest corner) and 1 being the smoothest curve.</param>
    public void AddClosedCurve(Point[] points, float tension)
    {
        AddPathPoints(Array.ConvertAll(points, p => new PointF(p.X, p.Y)));
        list.Add(new ClosedCurveMode { Points = Array.ConvertAll(points, p => new PointF(p.X, p.Y)), Tension = tension });
    }

    /// <summary>Adds a spline curve to the current figure. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curve.</param>
    public void AddCurve(PointF[] points)
    {
        AddPathPoints(points);
        list.Add(new CurveMode { Points = points });
    }

    /// <summary>Adds a spline curve to the current figure.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curve.</param>
    /// <param name="offset">The index of the element in the <paramref name="points" /> array that is used as the first point in the curve.</param>
    /// <param name="numberOfSegments">The number of segments used to draw the curve. A segment can be thought of as a line connecting two points.</param>
    /// <param name="tension">A value that specifies the amount that the curve bends between control points. Values greater than 1 produce unpredictable results.</param>
    public void AddCurve(PointF[] points, int offset, int numberOfSegments, float tension)
    {
        AddPathPoints(points);
        list.Add(new CurveMode { Points = points, Offset = offset, NumberOfSegments = numberOfSegments, Tension = tension });
    }

    /// <summary>Adds a spline curve to the current figure.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the curve.</param>
    /// <param name="tension">A value that specifies the amount that the curve bends between control points. Values greater than 1 produce unpredictable results.</param>
    public void AddCurve(PointF[] points, float tension)
    {
        AddPathPoints(points);
        list.Add(new CurveMode { Points = points, Tension = tension });
    }

    /// <summary>Adds a spline curve to the current figure. A cardinal spline curve is used because the curve travels through each of the points in the array.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curve.</param>
    public void AddCurve(Point[] points)
    {
        AddPathPoints(Array.ConvertAll(points, p => new PointF(p.X, p.Y)));
        list.Add(new CurveMode { Points = Array.ConvertAll(points, p => new PointF(p.X, p.Y)) });
    }

    /// <summary>Adds a spline curve to the current figure.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curve.</param>
    /// <param name="offset">The index of the element in the <paramref name="points" /> array that is used as the first point in the curve.</param>
    /// <param name="numberOfSegments">A value that specifies the amount that the curve bends between control points. Values greater than 1 produce unpredictable results.</param>
    /// <param name="tension">A value that specifies the amount that the curve bends between control points. Values greater than 1 produce unpredictable results.</param>
    public void AddCurve(Point[] points, int offset, int numberOfSegments, float tension)
    {
        AddPathPoints(Array.ConvertAll(points, p => new PointF(p.X, p.Y)));
        list.Add(new CurveMode { Points = Array.ConvertAll(points, p => new PointF(p.X, p.Y)), Offset = offset, NumberOfSegments = numberOfSegments, Tension = tension });
    }

    /// <summary>Adds a spline curve to the current figure.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the curve.</param>
    /// <param name="tension">A value that specifies the amount that the curve bends between control points. Values greater than 1 produce unpredictable results.</param>
    public void AddCurve(Point[] points, float tension)
    {
        AddPathPoints(Array.ConvertAll(points, p => new PointF(p.X, p.Y)));
        list.Add(new CurveMode { Points = Array.ConvertAll(points, p => new PointF(p.X, p.Y)), Tension = tension });
    }

    /// <summary>Adds an ellipse to the current path.</summary>
    /// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> that represents the bounding rectangle that defines the ellipse.</param>
    public void AddEllipse(Rectangle rect)
    {
        AddPathPoints(new PointF(rect.X, rect.Y), new PointF(rect.Right, rect.Bottom));
        list.Add(new EllipseMode { Rect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height) });
    }

    /// <summary>Adds an ellipse to the current path.</summary>
    /// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> that represents the bounding rectangle that defines the ellipse.</param>
    public void AddEllipse(RectangleF rect)
    {
        AddPathPoints(new PointF(rect.X, rect.Y), new PointF(rect.Right, rect.Bottom));
        list.Add(new EllipseMode { Rect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height) });
    }

    /// <summary>Adds an ellipse to the current path.</summary>
    /// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
    /// <param name="width">The width of the bounding rectangle that defines the ellipse.</param>
    /// <param name="height">The height of the bounding rectangle that defines the ellipse.</param>
    public void AddEllipse(int x, int y, int width, int height)
    {
        AddPathPoints(new PointF(x, y), new PointF(width, height));
        list.Add(new EllipseMode { Rect = new RectangleF(x, y, width, height) });
    }

    /// <summary>Adds an ellipse to the current path.</summary>
    /// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
    /// <param name="y">The y-coordinate of the upper left corner of the bounding rectangle that defines the ellipse.</param>
    /// <param name="width">The width of the bounding rectangle that defines the ellipse.</param>
    /// <param name="height">The height of the bounding rectangle that defines the ellipse.</param>
    public void AddEllipse(float x, float y, float width, float height)
    {
        AddPathPoints(new PointF(x, y), new PointF(width, height));
        list.Add(new EllipseMode { Rect = new RectangleF(x, y, width, height) });
    }

    /// <summary>Appends a line segment to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="pt1">A <see cref="T:System.Drawing.Point" /> that represents the starting point of the line.</param>
    /// <param name="pt2">A <see cref="T:System.Drawing.Point" /> that represents the endpoint of the line.</param>
    public void AddLine(Point pt1, Point pt2)
    {
        AddPathPoints(new PointF(pt1.X, pt1.Y), new PointF(pt2.X, pt2.Y));
        list.Add(new LineMode { Pt1 = new PointF(pt1.X, pt1.Y), Pt2 = new PointF(pt2.X, pt2.Y) });
    }

    /// <summary>Appends a line segment to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="pt1">A <see cref="T:System.Drawing.PointF" /> that represents the starting point of the line.</param>
    /// <param name="pt2">A <see cref="T:System.Drawing.PointF" /> that represents the endpoint of the line.</param>
    public void AddLine(PointF pt1, PointF pt2)
    {
        AddPathPoints(new PointF(pt1.X, pt1.Y), new PointF(pt2.X, pt2.Y));
        list.Add(new LineMode { Pt1 = new PointF(pt1.X, pt1.Y), Pt2 = new PointF(pt2.X, pt2.Y) });
    }

    /// <summary>Appends a line segment to the current figure.</summary>
    /// <param name="x1">The x-coordinate of the starting point of the line.</param>
    /// <param name="y1">The y-coordinate of the starting point of the line.</param>
    /// <param name="x2">The x-coordinate of the endpoint of the line.</param>
    /// <param name="y2">The y-coordinate of the endpoint of the line.</param>
    public void AddLine(int x1, int y1, int x2, int y2)
    {
        AddPathPoints(new PointF(x1, y1), new PointF(x2, y2));
        list.Add(new LineMode { Pt1 = new PointF(x1, y1), Pt2 = new PointF(x2, y2) });
    }

    /// <summary>Appends a line segment to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="x1">The x-coordinate of the starting point of the line.</param>
    /// <param name="y1">The y-coordinate of the starting point of the line.</param>
    /// <param name="x2">The x-coordinate of the endpoint of the line.</param>
    /// <param name="y2">The y-coordinate of the endpoint of the line.</param>
    public void AddLine(float x1, float y1, float x2, float y2)
    {
        AddPathPoints(new PointF(x1, y1), new PointF(x2, y2));
        list.Add(new LineMode { Pt1 = new PointF(x1, y1), Pt2 = new PointF(x2, y2) });
    }

    /// <summary>Appends a series of connected line segments to the end of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that define the line segments to add.</param>
    public void AddLines(PointF[] points)
    {
        AddPathPoints(points);
        list.Add(new LinesMode { Points = points });
    }

    /// <summary>Appends a series of connected line segments to the end of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that define the line segments to add.</param>
    public void AddLines(Point[] points)
    {
        AddPathPoints(Array.ConvertAll(points, p => new PointF(p.X, p.Y)));
        list.Add(new LinesMode { Points = Array.ConvertAll(points, p => new PointF(p.X, p.Y)) });
    }

    /// <summary>Appends the specified <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> to this path.</summary>
    /// <param name="addingPath">The <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> to add.</param>
    /// <param name="connect">A Boolean value that specifies whether the first figure in the added path is part of the last figure in this path. A value of <see langword="true" /> specifies that (if possible) the first figure in the added path is part of the last figure in this path. A value of <see langword="false" /> specifies that the first figure in the added path is separate from the last figure in this path.</param>
    public void AddPath(GraphicsPath addingPath, bool connect)
    {
        list.Add(new PathMode { Path = addingPath, Connect= connect });
    }

    /// <summary>Adds the outline of a pie shape to this path.</summary>
    /// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> that represents the bounding rectangle that defines the ellipse from which the pie is drawn.</param>
    /// <param name="startAngle">The starting angle for the pie section, measured in degrees clockwise from the x-axis.</param>
    /// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the pie section, measured in degrees clockwise from <paramref name="startAngle" />.</param>
    public void AddPie(Rectangle rect, float startAngle, float sweepAngle)
    {
        AddPathPoints(new PointF(rect.Left, rect.Top),new PointF(rect.Right, rect.Bottom));
        list.Add(new PieMode { Rect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), StartAngle = startAngle, SweepAngle = sweepAngle });
    }

    /// <summary>Adds the outline of a pie shape to this path.</summary>
    /// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie is drawn.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie is drawn.</param>
    /// <param name="width">The width of the bounding rectangle that defines the ellipse from which the pie is drawn.</param>
    /// <param name="height">The height of the bounding rectangle that defines the ellipse from which the pie is drawn.</param>
    /// <param name="startAngle">The starting angle for the pie section, measured in degrees clockwise from the x-axis.</param>
    /// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the pie section, measured in degrees clockwise from <paramref name="startAngle" />.</param>
    public void AddPie(int x, int y, int width, int height, float startAngle, float sweepAngle)
    {
        AddPathPoints(new PointF(x, y), new PointF(x + width, y + height));
        list.Add(new PieMode { Rect = new RectangleF(x, y, width, height), StartAngle = startAngle, SweepAngle = sweepAngle });
    }

    /// <summary>Adds the outline of a pie shape to this path.</summary>
    /// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie is drawn.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie is drawn.</param>
    /// <param name="width">The width of the bounding rectangle that defines the ellipse from which the pie is drawn.</param>
    /// <param name="height">The height of the bounding rectangle that defines the ellipse from which the pie is drawn.</param>
    /// <param name="startAngle">The starting angle for the pie section, measured in degrees clockwise from the x-axis.</param>
    /// <param name="sweepAngle">The angle between <paramref name="startAngle" /> and the end of the pie section, measured in degrees clockwise from <paramref name="startAngle" />.</param>
    public void AddPie(float x, float y, float width, float height, float startAngle, float sweepAngle)
    {
        AddPathPoints(new PointF(x, y), new PointF(x + width, y + height));
        list.Add(new PieMode { Rect = new RectangleF(x, y, width, height), StartAngle = startAngle, SweepAngle = sweepAngle });
    }

    /// <summary>Adds a polygon to this path.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that defines the polygon to add.</param>
    public void AddPolygon(PointF[] points)
    {
        AddPathPoints(points);
        list.Add(new PolygonMode { Points = Array.ConvertAll(points, r => new PointF(r.X, r.Y)) });
    }

    /// <summary>Adds a polygon to this path.</summary>
    /// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that defines the polygon to add.</param>
    public void AddPolygon(Point[] points)
    {
        AddPathPoints(Array.ConvertAll(points, r => new PointF(r.X, r.Y)));
        list.Add(new PolygonMode { Points = Array.ConvertAll(points, r => new PointF(r.X, r.Y)) });
    }

    /// <summary>Adds a rectangle to this path.</summary>
    /// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle to add.</param>
    public void AddRectangle(Rectangle rect)
    {
        AddPathPoints(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Bottom));
        list.Add(new RectangleMode { Rect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height) });
    }

    /// <summary>Adds a rectangle to this path.</summary>
    /// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle to add.</param>
    public void AddRectangle(RectangleF rect)
    {
        AddPathPoints(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Bottom));
        list.Add(new RectangleMode { Rect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height) });
    }

    /// <summary>Adds a series of rectangles to this path.</summary>
    /// <param name="rects">An array of <see cref="T:System.Drawing.RectangleF" /> structures that represents the rectangles to add.</param>
    public void AddRectangles(RectangleF[] rects)
    {
        foreach(var rect in rects)
            AddPathPoints(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Bottom));

        list.Add(new RectanglesMode { Rects = rects });
    }

    /// <summary>Adds a series of rectangles to this path.</summary>
    /// <param name="rects">An array of <see cref="T:System.Drawing.Rectangle" /> structures that represents the rectangles to add.</param>
    public void AddRectangles(Rectangle[] rects)
    {
        foreach (RectangleF rect in rects)
            AddPathPoints(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Bottom));
        list.Add(new RectanglesMode { Rects = Array.ConvertAll(rects,r=>new RectangleF(r.X, r.Y, r.Width, r.Height)) });
    }

    /// <summary>Adds a text string to this path.</summary>
    /// <param name="s">The <see cref="T:System.String" /> to add.</param>
    /// <param name="family">A <see cref="T:System.Drawing.FontFamily" /> that represents the name of the font with which the test is drawn.</param>
    /// <param name="style">A <see cref="T:System.Drawing.FontStyle" /> enumeration that represents style information about the text (bold, italic, and so on). This must be cast as an integer (see the example code later in this section).</param>
    /// <param name="emSize">The height of the em square box that bounds the character.</param>
    /// <param name="origin">A <see cref="T:System.Drawing.Point" /> that represents the point where the text starts.</param>
    /// <param name="format">A <see cref="T:System.Drawing.StringFormat" /> that specifies text formatting information, such as line spacing and alignment.</param>
    public void AddString(string s, FontFamily family, int style, float emSize, Point origin, StringFormat format)
    {
        AddPathPoints(new PointF(0, 0), new PointF(emSize * s.Length, emSize));
        list.Add(new StringMode { Text = s, Family = family, Style = style, EmSize = emSize, LayoutRect = new RectangleF(origin.X, origin.Y, 0, 0), Format = format });
    }

    /// <summary>Adds a text string to this path.</summary>
    /// <param name="s">The <see cref="T:System.String" /> to add.</param>
    /// <param name="family">A <see cref="T:System.Drawing.FontFamily" /> that represents the name of the font with which the test is drawn.</param>
    /// <param name="style">A <see cref="T:System.Drawing.FontStyle" /> enumeration that represents style information about the text (bold, italic, and so on). This must be cast as an integer (see the example code later in this section).</param>
    /// <param name="emSize">The height of the em square box that bounds the character.</param>
    /// <param name="origin">A <see cref="T:System.Drawing.PointF" /> that represents the point where the text starts.</param>
    /// <param name="format">A <see cref="T:System.Drawing.StringFormat" /> that specifies text formatting information, such as line spacing and alignment.</param>
    public void AddString(string s, FontFamily family, int style, float emSize, PointF origin, StringFormat format)
    {
        AddPathPoints(new PointF(0, 0), new PointF(emSize * s.Length, emSize));
        list.Add(new StringMode { Text = s, Family = family, Style = style, EmSize = emSize, LayoutRect = new RectangleF(origin.X, origin.Y, 0, 0), Format = format });
    }

    /// <summary>Adds a text string to this path.</summary>
    /// <param name="s">The <see cref="T:System.String" /> to add.</param>
    /// <param name="family">A <see cref="T:System.Drawing.FontFamily" /> that represents the name of the font with which the test is drawn.</param>
    /// <param name="style">A <see cref="T:System.Drawing.FontStyle" /> enumeration that represents style information about the text (bold, italic, and so on). This must be cast as an integer (see the example code later in this section).</param>
    /// <param name="emSize">The height of the em square box that bounds the character.</param>
    /// <param name="layoutRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the text.</param>
    /// <param name="format">A <see cref="T:System.Drawing.StringFormat" /> that specifies text formatting information, such as line spacing and alignment.</param>
    public void AddString(string s, FontFamily family, int style, float emSize, Rectangle layoutRect, StringFormat format)
    {
        AddPathPoints(new PointF(layoutRect.Left, layoutRect.Top), new PointF(layoutRect.Right, layoutRect.Bottom));
        list.Add(new StringMode { Text = s, Family = family, Style = style, EmSize = emSize, LayoutRect = new RectangleF(layoutRect.X, layoutRect.Y, layoutRect.Width, layoutRect.Height), Format = format });
    }

    /// <summary>Adds a text string to this path.</summary>
    /// <param name="s">The <see cref="T:System.String" /> to add.</param>
    /// <param name="family">A <see cref="T:System.Drawing.FontFamily" /> that represents the name of the font with which the test is drawn.</param>
    /// <param name="style">A <see cref="T:System.Drawing.FontStyle" /> enumeration that represents style information about the text (bold, italic, and so on). This must be cast as an integer (see the example code later in this section).</param>
    /// <param name="emSize">The height of the em square box that bounds the character.</param>
    /// <param name="layoutRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the text.</param>
    /// <param name="format">A <see cref="T:System.Drawing.StringFormat" /> that specifies text formatting information, such as line spacing and alignment.</param>
    public void AddString(string s, FontFamily family, int style, float emSize, RectangleF layoutRect, StringFormat format)
    {
        AddPathPoints(new PointF(layoutRect.Left, layoutRect.Top), new PointF(layoutRect.Right, layoutRect.Bottom));
        list.Add(new StringMode { Text = s, Family = family, Style = style, EmSize = emSize, LayoutRect = new RectangleF(layoutRect.X, layoutRect.Y, layoutRect.Width, layoutRect.Height), Format = format });
    }

    /// <summary>Clears all markers from this path.</summary>
    public void ClearMarkers()
    {
    }

    /// <summary>Creates an exact copy of this path.</summary>
    /// <returns>The <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> this method creates, cast as an object.</returns>
    public object Clone()
    {
        return ((ArrayList)new ArrayList { this }.Clone())[0];
    }

    /// <summary>Starts a new figure without closing the current figure. All subsequent points added to the path are added to this new figure.</summary>
    public void StartFigure()
    {
        list.Add(new FigureMode { Start = true, Close = false });
    }
    internal bool IsCloseAllFigures { get; set; }
    /// <summary>Closes all open figures in this path and starts a new figure. It closes each open figure by connecting a line from its endpoint to its starting point.</summary>
    public void CloseAllFigures()
    {
        IsCloseAllFigures = true;
    }

    /// <summary>Closes the current figure and starts a new figure. If the current figure contains a sequence of connected lines and curves, the method closes the loop by connecting a line from the endpoint to the starting point.</summary>
    public void CloseFigure()
    {
        list.Add(new FigureMode { Start = false, Close = true });
    }

    /// <summary>Releases all resources used by this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
    ~GraphicsPath()
    {
        Dispose();
    }

    /// <summary>Converts each curve in this path into a sequence of connected line segments.</summary>
    public void Flatten()
    {
    }

    /// <summary>Applies the specified transform and then converts each curve in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> into a sequence of connected line segments.</summary>
    /// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which to transform this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> before flattening.</param>
    public void Flatten(Matrix matrix)
    {
    }

    /// <summary>Converts each curve in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> into a sequence of connected line segments.</summary>
    /// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which to transform this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> before flattening.</param>
    /// <param name="flatness">Specifies the maximum permitted error between the curve and its flattened approximation. A value of 0.25 is the default. Reducing the flatness value will increase the number of line segments in the approximation.</param>
    public void Flatten(Matrix matrix, float flatness)
    {
    }

    /// <summary>Returns a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <returns>A <see cref="T:System.Drawing.RectangleF" /> that represents a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</returns>
    public RectangleF GetBounds()
    {
        throw new NotImplementedException();
    }

    /// <summary>Returns a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when this path is transformed by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
    /// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a transformation to be applied to this path before the bounding rectangle is calculated. This path is not permanently transformed; the transformation is used only during the process of calculating the bounding rectangle.</param>
    /// <returns>A <see cref="T:System.Drawing.RectangleF" /> that represents a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</returns>
    public RectangleF GetBounds(Matrix matrix)
    {
        throw new NotImplementedException();
    }

    /// <summary>Returns a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when the current path is transformed by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> and drawn with the specified <see cref="T:System.Drawing.Pen" />.</summary>
    /// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a transformation to be applied to this path before the bounding rectangle is calculated. This path is not permanently transformed; the transformation is used only during the process of calculating the bounding rectangle.</param>
    /// <param name="pen">The <see cref="T:System.Drawing.Pen" /> with which to draw the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</param>
    /// <returns>A <see cref="T:System.Drawing.RectangleF" /> that represents a rectangle that bounds this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</returns>
    public RectangleF GetBounds(Matrix matrix, Pen pen)
    {
        throw new NotImplementedException();
    }

    /// <summary>Gets the last point in the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> array of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <returns>A <see cref="T:System.Drawing.PointF" /> that represents the last point in this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</returns>
    public PointF GetLastPoint()
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />.</summary>
    /// <param name="point">A <see cref="T:System.Drawing.Point" /> that specifies the location to test.</param>
    /// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
    public bool IsOutlineVisible(Point point, Pen pen)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" /> and using the specified <see cref="T:System.Drawing.Graphics" />.</summary>
    /// <param name="pt">A <see cref="T:System.Drawing.Point" /> that specifies the location to test.</param>
    /// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test.</param>
    /// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> as drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
    public bool IsOutlineVisible(Point pt, Pen pen, Graphics graphics)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />.</summary>
    /// <param name="point">A <see cref="T:System.Drawing.PointF" /> that specifies the location to test.</param>
    /// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
    public bool IsOutlineVisible(PointF point, Pen pen)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" /> and using the specified <see cref="T:System.Drawing.Graphics" />.</summary>
    /// <param name="pt">A <see cref="T:System.Drawing.PointF" /> that specifies the location to test.</param>
    /// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test.</param>
    /// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> as drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
    public bool IsOutlineVisible(PointF pt, Pen pen, Graphics graphics)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />.</summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
    public bool IsOutlineVisible(int x, int y, Pen pen)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" /> and using the specified <see cref="T:System.Drawing.Graphics" />.</summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test.</param>
    /// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> as drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
    public bool IsOutlineVisible(int x, int y, Pen pen, Graphics graphics)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />.</summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
    public bool IsOutlineVisible(float x, float y, Pen pen)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> when drawn with the specified <see cref="T:System.Drawing.Pen" /> and using the specified <see cref="T:System.Drawing.Graphics" />.</summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <param name="pen">The <see cref="T:System.Drawing.Pen" /> to test.</param>
    /// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within (under) the outline of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> as drawn with the specified <see cref="T:System.Drawing.Pen" />; otherwise, <see langword="false" />.</returns>
    public bool IsOutlineVisible(float x, float y, Pen pen, Graphics graphics)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="point">A <see cref="T:System.Drawing.Point" /> that represents the point to test.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
    public bool IsVisible(Point point)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="pt">A <see cref="T:System.Drawing.Point" /> that represents the point to test.</param>
    /// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
    public bool IsVisible(Point pt, Graphics graphics)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="point">A <see cref="T:System.Drawing.PointF" /> that represents the point to test.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
    public bool IsVisible(PointF point)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="pt">A <see cref="T:System.Drawing.PointF" /> that represents the point to test.</param>
    /// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within this; otherwise, <see langword="false" />.</returns>
    public bool IsVisible(PointF pt, Graphics graphics)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
    public bool IsVisible(int x, int y)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />, using the specified <see cref="T:System.Drawing.Graphics" />.</summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
    public bool IsVisible(int x, int y, Graphics graphics)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
    public bool IsVisible(float x, float y)
    {
        throw new NotImplementedException();
    }

    /// <summary>Indicates whether the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> in the visible clip region of the specified <see cref="T:System.Drawing.Graphics" />.</summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> for which to test visibility.</param>
    /// <returns>This method returns <see langword="true" /> if the specified point is contained within this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />; otherwise, <see langword="false" />.</returns>
    public bool IsVisible(float x, float y, Graphics graphics)
    {
        throw new NotImplementedException();
    }

    /// <summary>Empties the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> and <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathTypes" /> arrays and sets the <see cref="T:System.Drawing.Drawing2D.FillMode" /> to <see cref="F:System.Drawing.Drawing2D.FillMode.Alternate" />.</summary>
    public void Reset()
    {
        PathPoints = null;
        PathTypes = null;
        FillMode = FillMode.Alternate;
    }

    /// <summary>Reverses the order of points in the <see cref="P:System.Drawing.Drawing2D.GraphicsPath.PathPoints" /> array of this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    public void Reverse()
    {
    }

    /// <summary>Sets a marker on this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    public void SetMarkers()
    {
    }

    /// <summary>Applies a transform matrix to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that represents the transformation to apply.</param>
    public void Transform(Matrix? matrix)
    {
        Matrix = matrix;
    }

    /// <summary>Applies a warp transform, defined by a rectangle and a parallelogram, to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="destPoints">An array of <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram to which the rectangle defined by <paramref name="srcRect" /> is transformed. The array can contain either three or four elements. If the array contains three elements, the lower-right corner of the parallelogram is implied by the first three points.</param>
    /// <param name="srcRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that is transformed to the parallelogram defined by <paramref name="destPoints" />.</param>
    public void Warp(PointF[]? destPoints, RectangleF srcRect)
    {
        DestPoints = destPoints;
        SrcRect = srcRect;
    }

    /// <summary>Applies a warp transform, defined by a rectangle and a parallelogram, to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="destPoints">An array of <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram to which the rectangle defined by <paramref name="srcRect" /> is transformed. The array can contain either three or four elements. If the array contains three elements, the lower-right corner of the parallelogram is implied by the first three points.</param>
    /// <param name="srcRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that is transformed to the parallelogram defined by <paramref name="destPoints" />.</param>
    /// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a geometric transform to apply to the path.</param>
    public void Warp(PointF[]? destPoints, RectangleF srcRect, Matrix? matrix)
    {
        DestPoints = destPoints;
        SrcRect = srcRect;
        Matrix = matrix;
    }

    /// <summary>Applies a warp transform, defined by a rectangle and a parallelogram, to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="destPoints">An array of <see cref="T:System.Drawing.PointF" /> structures that defines a parallelogram to which the rectangle defined by <paramref name="srcRect" /> is transformed. The array can contain either three or four elements. If the array contains three elements, the lower-right corner of the parallelogram is implied by the first three points.</param>
    /// <param name="srcRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that is transformed to the parallelogram defined by <paramref name="destPoints" />.</param>
    /// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a geometric transform to apply to the path.</param>
    /// <param name="warpMode">A <see cref="T:System.Drawing.Drawing2D.WarpMode" /> enumeration that specifies whether this warp operation uses perspective or bilinear mode.</param>
    public void Warp(PointF[]? destPoints, RectangleF srcRect, Matrix? matrix, WarpMode warpMode)
    {
        DestPoints = destPoints;
        SrcRect = srcRect;
        Matrix = matrix;
        WarpMode = warpMode;
    }

    /// <summary>Applies a warp transform, defined by a rectangle and a parallelogram, to this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="destPoints">An array of <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram to which the rectangle defined by <paramref name="srcRect" /> is transformed. The array can contain either three or four elements. If the array contains three elements, the lower-right corner of the parallelogram is implied by the first three points.</param>
    /// <param name="srcRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that is transformed to the parallelogram defined by <paramref name="destPoints" />.</param>
    /// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a geometric transform to apply to the path.</param>
    /// <param name="warpMode">A <see cref="T:System.Drawing.Drawing2D.WarpMode" /> enumeration that specifies whether this warp operation uses perspective or bilinear mode.</param>
    /// <param name="flatness">A value from 0 through 1 that specifies how flat the resulting path is. For more information, see the <see cref="M:System.Drawing.Drawing2D.GraphicsPath.Flatten" /> methods.</param>
    public void Warp(PointF[]? destPoints, RectangleF srcRect, Matrix? matrix, WarpMode warpMode, float flatness)
    {
        DestPoints = destPoints;
        SrcRect = srcRect;
        Matrix = matrix;
        WarpMode = warpMode;
        Flatness = flatness;
    }

    /// <summary>Adds an additional outline to the path.</summary>
    /// <param name="pen">A <see cref="T:System.Drawing.Pen" /> that specifies the width between the original outline of the path and the new outline this method creates.</param>
    public void Widen(Pen? pen)
    {
        Pen = pen;
    }

    /// <summary>Adds an additional outline to the <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
    /// <param name="pen">A <see cref="T:System.Drawing.Pen" /> that specifies the width between the original outline of the path and the new outline this method creates.</param>
    /// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a transform to apply to the path before widening.</param>
    public void Widen(Pen? pen, Matrix? matrix)
    {
        Pen = pen;
        Matrix = matrix;
    }

    /// <summary>Replaces this <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> with curves that enclose the area that is filled when this path is drawn by the specified pen.</summary>
    /// <param name="pen">A <see cref="T:System.Drawing.Pen" /> that specifies the width between the original outline of the path and the new outline this method creates.</param>
    /// <param name="matrix">A <see cref="T:System.Drawing.Drawing2D.Matrix" /> that specifies a transform to apply to the path before widening.</param>
    /// <param name="flatness">A value that specifies the flatness for curves.</param>
    public void Widen(Pen? pen, Matrix? matrix, float flatness)
    {
        Pen= pen;
        Matrix= matrix;
        Flatness= flatness;
    }
    internal PointF[]? DestPoints { get; set; }
    internal RectangleF SrcRect { get; set; }
    internal WarpMode WarpMode { get; set; }
    internal Pen? Pen { get; set; }
    internal Matrix? Matrix { get; set; }
    internal float Flatness { get; set; }
}