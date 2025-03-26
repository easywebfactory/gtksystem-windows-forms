namespace System.Drawing.Drawing2D;

/// <summary>Encapsulates a 3-by-3 affine matrix that represents a geometric transform. This class cannot be inherited.</summary>
public sealed class Matrix : MarshalByRefObject, IDisposable
{
    /// <summary>Gets an array of floating-point values that represents the elements of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
    /// <returns>An array of floating-point values that represents the elements of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</returns>
    public float[] Elements
    {
        get { throw new NotImplementedException(); }
    }

    /// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is the identity matrix.</summary>
    /// <returns>This property is <see langword="true" /> if this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is identity; otherwise, <see langword="false" />.</returns>
    public bool IsIdentity => throw new NotImplementedException();

    /// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is invertible.</summary>
    /// <returns>This property is <see langword="true" /> if this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is invertible; otherwise, <see langword="false" />.</returns>
    public bool IsInvertible => throw new NotImplementedException();

    /// <summary>Gets the x translation value (the dx value, or the element in the third row and first column) of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
    /// <returns>The x translation value of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</returns>
    public float OffsetX { get; internal set; }

    /// <summary>Gets the y translation value (the dy value, or the element in the third row and second column) of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
    /// <returns>The y translation value of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</returns>
    public float OffsetY { get; internal set; }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> class as the identity matrix.</summary>
    public Matrix()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> class to the geometric transform defined by the specified rectangle and array of points.</summary>
    /// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle to be transformed.</param>
    /// <param name="plgpts">An array of three <see cref="T:System.Drawing.Point" /> structures that represents the points of a parallelogram to which the upper-left, upper-right, and lower-left corners of the rectangle is to be transformed. The lower-right corner of the parallelogram is implied by the first three corners.</param>
    public Matrix(Rectangle rect, Point[]? plgpts)
    {
        RectF = rect;
        Plgpts = plgpts;
    }

    public Point[]? Plgpts { get; set; }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> class to the geometric transform defined by the specified rectangle and array of points.</summary>
    /// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle to be transformed.</param>
    /// <param name="plgpts">An array of three <see cref="T:System.Drawing.PointF" /> structures that represents the points of a parallelogram to which the upper-left, upper-right, and lower-left corners of the rectangle is to be transformed. The lower-right corner of the parallelogram is implied by the first three corners.</param>
    public Matrix(RectangleF rect, PointF[] plgpts)
    {
        RectF = rect;
        Plgptsf = plgpts;
    }

    public PointF[]? Plgptsf { get; set; }

    public RectangleF? RectF { get; set; }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> class with the specified elements.</summary>
    /// <param name="m11">The value in the first row and first column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    /// <param name="m12">The value in the first row and second column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    /// <param name="m21">The value in the second row and first column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    /// <param name="m22">The value in the second row and second column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    /// <param name="dx">The value in the third row and first column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    /// <param name="dy">The value in the third row and second column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    public Matrix(float m11, float m12, float m21, float m22, float dx, float dy)
    {
        //[m11,m12,0]
        //[m21,m22,0]
        //[dx,dy,1]

        M11 = m11;
        M12 = m12;
        M21 = m21;
        M22 = m22;
        Dx = dx;
        Dy = dy;
    }

    public float M11 { get; set; } = 1;
    public float M12 { get; set; }
    public float M21 { get; set; }
    public float M22 { get; set; } = 1;
    public float Dx { get; set; }
    public float Dy { get; set; }
    public MatrixOrder Order { get; set; } = MatrixOrder.Prepend;

    public float Angle { get; set; }
    public PointF RotateAtPoint { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public float ShearX { get; set; }
    public float ShearY { get; set; }
    public PointF[]? TransformPointsValue { get; set; }
    public PointF[]? TransformVectorsValue { get; set; }
    public Point[]? VectorTransformPointsValue { get; set; }
    public RectangleF InitRectangle { get; set; }
    public PointF[]? InitPointF { get; set; }
    public Matrix? InitMatrix { get; set; }
    public bool InvertValue { get; set; }
    public Matrix? MultiplyValue { get; set; }

    /// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
    /// <returns>The <see cref="T:System.Drawing.Drawing2D.Matrix" /> that this method creates.</returns>
    public Matrix Clone()
    {
        return null!;
        //         Matrix m = new Matrix();
        //m.initMatrix = this.initMatrix;
        //m.initRectangle = this.initRectangle;
        //m.initPointF = this.initPointF;
        //m.vectorTransformPoints = this.vectorTransformPoints;
        //m.transformVectors = this.transformVectors;
        //m.transformPoints = this.transformPoints;
        //m.shearX = this.shearX;
        //m.shearY = this.shearY;
        //m.angle = this.angle;
        //m.scaleX = this.scaleX;
        //m.scaleY = this.scaleY;
        //m.rotateAtPoint = this.rotateAtPoint;
        //m.order = this.order;
        //m.dx = this.dx;
        //m.dy = this.dy;
        //m.m11 = this.m11;
        //m.m12 = this.m12;
        //m.m21 = this.m21;
        //m.m22 = this.m22;

        //return m;
    }

    /// <summary>Releases all resources used by this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
    public void Dispose()
    {
        InitMatrix = null;
        InitRectangle = new RectangleF();
        InitPointF = [];
        VectorTransformPointsValue = [];
        TransformVectorsValue = [];
        TransformPointsValue = [];
        ShearX = 0;
        ShearY = 0;
        Angle = 0;
        ScaleX = 0;
        ScaleY = 0;
        RotateAtPoint = new PointF();
        Order = MatrixOrder.Prepend;
        Dx = 0;
        Dy = 0;
        M11 = 1;
        M12 = 0;
        M21 = 0;
        M22 = 1;
        MultiplyValue = null;
    }

    /// <summary>Tests whether the specified object is a <see cref="T:System.Drawing.Drawing2D.Matrix" /> and is identical to this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>This method returns <see langword="true" /> if <paramref name="obj" /> is the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> identical to this <see cref="T:System.Drawing.Drawing2D.Matrix" />; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj)
    {
        return GetHashCode() == obj?.GetHashCode();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>Inverts this <see cref="T:System.Drawing.Drawing2D.Matrix" />, if it is invertible.</summary>
    public void Invert()
    {
        InvertValue = true;
    }

    /// <summary>Multiplies this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by the matrix specified in the <paramref name="matrix" /> parameter, by prepending the specified <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
    /// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is to be multiplied.</param>
    public void Multiply(Matrix matrix)
    {
        InitMatrix = matrix;
    }

    /// <summary>Multiplies this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by the matrix specified in the <paramref name="matrix" /> parameter, and in the order specified in the <paramref name="order" /> parameter.</summary>
    /// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is to be multiplied.</param>
    /// <param name="order">The <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that represents the order of the multiplication.</param>
    public void Multiply(Matrix matrix, MatrixOrder order)
    {
        MultiplyValue = matrix;
        Order = order;
    }

    /// <summary>Resets this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to have the elements of the identity matrix.</summary>
    public void Reset()
    {
        InitMatrix = null;
        InitRectangle = new RectangleF();
        InitPointF = [];
        VectorTransformPointsValue = [];
        TransformVectorsValue = [];
        TransformPointsValue = [];
        ShearX = 0;
        ShearY = 0;
        Angle = 0;
        ScaleX = 0;
        ScaleY = 0;
        RotateAtPoint = new PointF();
        Order = MatrixOrder.Prepend;
        Dx = 0;
        Dy = 0;
        M11 = 1;
        M12 = 0;
        M21 = 0;
        M22 = 1;
        MultiplyValue = null;
    }

    /// <summary>Prepend to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> a clockwise rotation, around the origin and by the specified angle.</summary>
    /// <param name="angle">The angle of the rotation, in degrees.</param>
    public void Rotate(float angle)
    {
        //this.m11 = (float)Math.Cos(angle * Math.PI / 180.0);
        //this.m12 = -(float)Math.Sin(angle * Math.PI / 180.0);

        //this.m21 = (float)Math.Sin(angle * Math.PI / 180.0);
        //this.m22 = (float)Math.Cos(angle * Math.PI / 180.0);

        Angle = angle;
    }

    /// <summary>Applies a clockwise rotation of an amount specified in the <paramref name="angle" /> parameter, around the origin (zero x and y coordinates) for this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
    /// <param name="angle">The angle (extent) of the rotation, in degrees.</param>
    /// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the rotation is applied to this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    public void Rotate(float angle, MatrixOrder order)
    {
        Order = order;
        Rotate(angle);
    }

    /// <summary>Applies a clockwise rotation to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> around the point specified in the <paramref name="point" /> parameter, and by prepending the rotation.</summary>
    /// <param name="angle">The angle (extent) of the rotation, in degrees.</param>
    /// <param name="point">A <see cref="T:System.Drawing.PointF" /> that represents the center of the rotation.</param>
    public void RotateAt(float angle, PointF point)
    {
        RotateAtPoint = point;
        Rotate(angle);
    }

    /// <summary>Applies a clockwise rotation about the specified point to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
    /// <param name="angle">The angle of the rotation, in degrees.</param>
    /// <param name="point">A <see cref="T:System.Drawing.PointF" /> that represents the center of the rotation.</param>
    /// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the rotation is applied.</param>
    public void RotateAt(float angle, PointF point, MatrixOrder order)
    {
        RotateAtPoint = point;
        Order = order;
        Rotate(angle);
    }

    /// <summary>Applies the specified scale vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by prepending the scale vector.</summary>
    /// <param name="scaleX">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the x-axis direction.</param>
    /// <param name="scaleY">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the y-axis direction.</param>
    public void Scale(float scaleX, float scaleY)
    {
        ScaleX = scaleX;
        ScaleY = scaleY;
    }

    /// <summary>Applies the specified scale vector (<paramref name="scaleX" /> and <paramref name="scaleY" />) to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> using the specified order.</summary>
    /// <param name="scaleX">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the x-axis direction.</param>
    /// <param name="scaleY">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the y-axis direction.</param>
    /// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the scale vector is applied to this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    public void Scale(float scaleX, float scaleY, MatrixOrder order)
    {
        Order = order;
        Scale(scaleX, scaleY);
    }

    /// <summary>Applies the specified shear vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by prepending the shear transformation.</summary>
    /// <param name="shearX">The horizontal shear factor.</param>
    /// <param name="shearY">The vertical shear factor.</param>
    public void Shear(float shearX, float shearY)
    {
        ShearX = shearX;
        ShearY = shearY;
    }

    /// <summary>Applies the specified shear vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
    /// <param name="shearX">The horizontal shear factor.</param>
    /// <param name="shearY">The vertical shear factor.</param>
    /// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the shear is applied.</param>
    public void Shear(float shearX, float shearY, MatrixOrder order)
    {
        ShearX = shearX;
        ShearY = shearY;
        Order = order;
    }

    /// <summary>Applies the geometric transform represented by this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to a specified array of points.</summary>
    /// <param name="pts">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points to transform.</param>
    public void TransformPoints(PointF[] pts)
    {
        TransformPointsValue = pts;
    }

    /// <summary>Applies the geometric transform represented by this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to a specified array of points.</summary>
    /// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform.</param>
    public void TransformPoints(Point[] pts)
    {
        TransformPointsValue = Array.ConvertAll(pts, p => new PointF(p.X, p.Y));
    }

    /// <summary>Multiplies each vector in an array by the matrix. The translation elements of this matrix (third row) are ignored.</summary>
    /// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform.</param>
    public void TransformVectors(PointF[] pts)
    {
        //         this.m11 = this.m11 * pts[0].X + this.m11 * pts[1].X;
        //this.m12 = this.m12 * pts[0].Y + this.m12 * pts[1].Y;

        //this.m21 = this.m21 * pts[0].X + this.m21 * pts[1].X;
        //this.m22 = this.m22 * pts[0].Y + this.m22 * pts[1].Y;

        TransformVectorsValue = pts;
    }

    /// <summary>Applies only the scale and rotate components of this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to the specified array of points.</summary>
    /// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform.</param>
    public void TransformVectors(Point[] pts)
    {
        TransformVectorsValue = Array.ConvertAll(pts, p => new PointF(p.X, p.Y));
    }

    /// <summary>Applies the specified translation vector (<paramref name="offsetX" /> and <paramref name="offsetY" />) to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by prepending the translation vector.</summary>
    /// <param name="offsetX">The x value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    /// <param name="offsetY">The y value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    public void Translate(float offsetX, float offsetY)
    {
        OffsetX = offsetX;
        OffsetY = offsetY;
    }

    /// <summary>Applies the specified translation vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
    /// <param name="offsetX">The x value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    /// <param name="offsetY">The y value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    /// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the translation is applied to this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
    public void Translate(float offsetX, float offsetY, MatrixOrder order)
    {
        Order = order;
        Translate(offsetX, offsetY);
    }

    /// <summary>Multiplies each vector in an array by the matrix. The translation elements of this matrix (third row) are ignored.</summary>
    /// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform.</param>
    public void VectorTransformPoints(Point[] pts)
    {
        //         this.m11 = this.m11 * pts[0].X + this.m11 * pts[1].X;
        //this.m12 = this.m12 * pts[0].Y + this.m12 * pts[1].Y;

        //this.m21 = this.m21 * pts[0].X + this.m21 * pts[1].X;
        //this.m22 = this.m22 * pts[0].Y + this.m22 * pts[1].Y;
        VectorTransformPointsValue = pts;
    }
}