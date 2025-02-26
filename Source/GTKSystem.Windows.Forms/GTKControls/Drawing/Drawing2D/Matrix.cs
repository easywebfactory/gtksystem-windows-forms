using Gtk;
using System.Collections;

namespace System.Drawing.Drawing2D
{
	/// <summary>Encapsulates a 3-by-3 affine matrix that represents a geometric transform. This class cannot be inherited.</summary>
	public sealed class Matrix : MarshalByRefObject, IDisposable
	{
		/// <summary>Gets an array of floating-point values that represents the elements of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <returns>An array of floating-point values that represents the elements of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</returns>
		public float[] Elements
		{
			get
			{
				throw null;
			}
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is the identity matrix.</summary>
		/// <returns>This property is <see langword="true" /> if this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is identity; otherwise, <see langword="false" />.</returns>
		public bool IsIdentity
		{
			get
			{
				throw null;
			}
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is invertible.</summary>
		/// <returns>This property is <see langword="true" /> if this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is invertible; otherwise, <see langword="false" />.</returns>
		public bool IsInvertible
		{
			get
			{
				throw null;
			}
		}

		/// <summary>Gets the x translation value (the dx value, or the element in the third row and first column) of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <returns>The x translation value of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</returns>
		public float OffsetX
		{
			get;
			internal set;
		}

		/// <summary>Gets the y translation value (the dy value, or the element in the third row and second column) of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <returns>The y translation value of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</returns>
		public float OffsetY
		{
            get;
            internal set;
        }

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> class as the identity matrix.</summary>
		public Matrix()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> class to the geometric transform defined by the specified rectangle and array of points.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle to be transformed.</param>
		/// <param name="plgpts">An array of three <see cref="T:System.Drawing.Point" /> structures that represents the points of a parallelogram to which the upper-left, upper-right, and lower-left corners of the rectangle is to be transformed. The lower-right corner of the parallelogram is implied by the first three corners.</param>
		public Matrix(Rectangle rect, Point[] plgpts)
        {
            //plgpts：一个由三个 PointF 结构构成的数组，该数组表示矩形的左上角、右上角和左下角将变换为的平行四边形的三个点。 平行四边形的右下角的位置可从前三个角的位置导出
            //此方法初始化新的 Matrix ，使其表示几何转换，该转换由 rect 参数指定的矩形映射到参数中 plgpts 三个点定义的并行四边形。 矩形的左上角映射到数组中的 plgpts 第一个点，右上角映射到第二个点，左下角映射到第三个点。 前三个平行四边形右下角点隐含。

        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> class to the geometric transform defined by the specified rectangle and array of points.</summary>
        /// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle to be transformed.</param>
        /// <param name="plgpts">An array of three <see cref="T:System.Drawing.PointF" /> structures that represents the points of a parallelogram to which the upper-left, upper-right, and lower-left corners of the rectangle is to be transformed. The lower-right corner of the parallelogram is implied by the first three corners.</param>
        public Matrix(RectangleF rect, PointF[] plgpts)
        {
            //plgpts：一个由三个 PointF 结构构成的数组，该数组表示矩形的左上角、右上角和左下角将变换为的平行四边形的三个点。 平行四边形的右下角的位置可从前三个角的位置导出
            //此方法初始化新的 Matrix ，使其表示几何转换，该转换由 rect 参数指定的矩形映射到参数中 plgpts 三个点定义的并行四边形。 矩形的左上角映射到数组中的 plgpts 第一个点，右上角映射到第二个点，左下角映射到第三个点。 前三个平行四边形右下角点隐含。

        }

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
            // 矩阵的第三列是[0,0,1]，固定值
            //其中M11,M22影响缩放，dx=OffsetX,dy=OffsetY影响平移，M12,M21(和M11，M22)一起影响旋转等。

            this.m11 = m11;
			this.m12 = m12;
			this.m21 = m21;
			this.m22 = m22;
			this.dx = dx;
			this.dy = dy;
		}
        public float m11 { get; set; } = 1;
        public float m12 { get; set; } = 0;
        public float m21 { get; set; } = 0;
        public float m22 { get; set; } = 1;
        public float dx { get; set; } = 0;
        public float dy { get; set; } = 0;
        public MatrixOrder order { get; set; } = MatrixOrder.Prepend;

        public float angle { get; set; }
        public PointF rotateAtPoint { get; set; }
        public float scaleX { get; set; }
        public float scaleY { get; set; }
        public float shearX { get; set; }
        public float shearY { get; set; }
        public PointF[] transformPoints { get; set; }
        public PointF[] transformVectors{ get; set; }
        public Point[] vectorTransformPoints{ get; set; }
        public RectangleF initRectangle { get; set; }
        public PointF[] initPointF { get; set; }
        public Matrix initMatrix { get; set; }
        public bool invert { get; set; }
        public Matrix multiply { get; set; }

        /// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
        /// <returns>The <see cref="T:System.Drawing.Drawing2D.Matrix" /> that this method creates.</returns>
        public Matrix Clone()
		{
            return null;
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
            this.initMatrix = null;
            this.initRectangle = new RectangleF();
            this.initPointF = new PointF[0];
            this.vectorTransformPoints = new Point[0];
            this.transformVectors = new PointF[0];
            this.transformPoints = new PointF[0];
            this.shearX = 0;
            this.shearY = 0;
            this.angle = 0;
            this.scaleX = 0;
            this.scaleY = 0;
            this.rotateAtPoint = new PointF();
            this.order = MatrixOrder.Prepend;
            this.dx = 0;
            this.dy = 0;
            this.m11 = 1;
            this.m12 = 0;
            this.m21 = 0;
            this.m22 = 1;
            this.multiply = null;
        }

		/// <summary>Tests whether the specified object is a <see cref="T:System.Drawing.Drawing2D.Matrix" /> and is identical to this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <param name="obj">The object to test.</param>
		/// <returns>This method returns <see langword="true" /> if <paramref name="obj" /> is the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> identical to this <see cref="T:System.Drawing.Drawing2D.Matrix" />; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			return this.GetHashCode() == obj.GetHashCode();  
		}

		/// <summary>Inverts this <see cref="T:System.Drawing.Drawing2D.Matrix" />, if it is invertible.</summary>
		public void Invert()
		{
			this.invert = true;
		}

		/// <summary>Multiplies this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by the matrix specified in the <paramref name="matrix" /> parameter, by prepending the specified <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is to be multiplied.</param>
		public void Multiply(Matrix matrix)
		{
			this.initMatrix = matrix;
		}

		/// <summary>Multiplies this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by the matrix specified in the <paramref name="matrix" /> parameter, and in the order specified in the <paramref name="order" /> parameter.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is to be multiplied.</param>
		/// <param name="order">The <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that represents the order of the multiplication.</param>
		public void Multiply(Matrix matrix, MatrixOrder order)
		{
			this.multiply = matrix;
			this.order = order;
		}

		/// <summary>Resets this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to have the elements of the identity matrix.</summary>
		public void Reset()
		{
			this.initMatrix = null;
            this.initRectangle = new RectangleF();
            this.initPointF=new PointF[0];
            this.vectorTransformPoints = new Point[0];
            this.transformVectors = new PointF[0];
            this.transformPoints = new PointF[0];
            this.shearX = 0;
            this.shearY = 0;
            this.angle = 0;
            this.scaleX = 0;
            this.scaleY = 0;
            this.rotateAtPoint = new PointF();
            this.order = MatrixOrder.Prepend;
			this.dx = 0;
			this.dy = 0;
			this.m11 = 1;
            this.m12 = 0;
            this.m21 = 0;
            this.m22 = 1;
			this.multiply = null;
        }

		/// <summary>Prepend to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> a clockwise rotation, around the origin and by the specified angle.</summary>
		/// <param name="angle">The angle of the rotation, in degrees.</param>
		public void Rotate(float angle)
        {
            //this.m11 = (float)Math.Cos(angle * Math.PI / 180.0);
            //this.m12 = -(float)Math.Sin(angle * Math.PI / 180.0);

            //this.m21 = (float)Math.Sin(angle * Math.PI / 180.0);
            //this.m22 = (float)Math.Cos(angle * Math.PI / 180.0);

            this.angle = angle;
        }

		/// <summary>Applies a clockwise rotation of an amount specified in the <paramref name="angle" /> parameter, around the origin (zero x and y coordinates) for this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <param name="angle">The angle (extent) of the rotation, in degrees.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the rotation is applied to this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
		public void Rotate(float angle, MatrixOrder order)
        {
			this.order = order;
			Rotate(angle);
        }

		/// <summary>Applies a clockwise rotation to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> around the point specified in the <paramref name="point" /> parameter, and by prepending the rotation.</summary>
		/// <param name="angle">The angle (extent) of the rotation, in degrees.</param>
		/// <param name="point">A <see cref="T:System.Drawing.PointF" /> that represents the center of the rotation.</param>
		public void RotateAt(float angle, PointF point)
		{
			this.rotateAtPoint = point;
            Rotate(angle);
        }

		/// <summary>Applies a clockwise rotation about the specified point to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
		/// <param name="angle">The angle of the rotation, in degrees.</param>
		/// <param name="point">A <see cref="T:System.Drawing.PointF" /> that represents the center of the rotation.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the rotation is applied.</param>
		public void RotateAt(float angle, PointF point, MatrixOrder order)
        {
            this.rotateAtPoint = point;
            this.order = order;
            Rotate(angle);
        }

		/// <summary>Applies the specified scale vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by prepending the scale vector.</summary>
		/// <param name="scaleX">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the x-axis direction.</param>
		/// <param name="scaleY">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the y-axis direction.</param>
		public void Scale(float scaleX, float scaleY)
		{
            this.scaleX = scaleX;
			this.scaleY = scaleY;
        }

		/// <summary>Applies the specified scale vector (<paramref name="scaleX" /> and <paramref name="scaleY" />) to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> using the specified order.</summary>
		/// <param name="scaleX">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the x-axis direction.</param>
		/// <param name="scaleY">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the y-axis direction.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the scale vector is applied to this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
		public void Scale(float scaleX, float scaleY, MatrixOrder order)
        {
            this.order = order;
            Scale(scaleX, scaleY);
        }

		/// <summary>Applies the specified shear vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by prepending the shear transformation.</summary>
		/// <param name="shearX">The horizontal shear factor.</param>
		/// <param name="shearY">The vertical shear factor.</param>
		public void Shear(float shearX, float shearY)
		{
			this.shearX = shearX;
			this.shearY = shearY;
		}

		/// <summary>Applies the specified shear vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
		/// <param name="shearX">The horizontal shear factor.</param>
		/// <param name="shearY">The vertical shear factor.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the shear is applied.</param>
		public void Shear(float shearX, float shearY, MatrixOrder order)
        {
            this.shearX = shearX;
            this.shearY = shearY; 
			this.order = order;
        }

		/// <summary>Applies the geometric transform represented by this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to a specified array of points.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points to transform.</param>
		public void TransformPoints(PointF[] pts)
		{
			this.transformPoints = pts;
        }

		/// <summary>Applies the geometric transform represented by this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to a specified array of points.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform.</param>
		public void TransformPoints(Point[] pts)
        {
            this.transformPoints = Array.ConvertAll(pts, p => new PointF(p.X, p.Y));
        }

		/// <summary>Multiplies each vector in an array by the matrix. The translation elements of this matrix (third row) are ignored.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform.</param>
		public void TransformVectors(PointF[] pts)
        {
			//将数组中的每个矢量与矩阵相乘。 该矩阵的转换元素（第三行）被忽略。
			//         this.m11 = this.m11 * pts[0].X + this.m11 * pts[1].X;
			//this.m12 = this.m12 * pts[0].Y + this.m12 * pts[1].Y;

			//this.m21 = this.m21 * pts[0].X + this.m21 * pts[1].X;
			//this.m22 = this.m22 * pts[0].Y + this.m22 * pts[1].Y;

			this.transformVectors = pts;
		}

        /// <summary>Applies only the scale and rotate components of this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to the specified array of points.</summary>
        /// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform.</param>
        public void TransformVectors(Point[] pts)
		{
            this.transformVectors = Array.ConvertAll(pts, p => new PointF(p.X, p.Y));
        }

		/// <summary>Applies the specified translation vector (<paramref name="offsetX" /> and <paramref name="offsetY" />) to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by prepending the translation vector.</summary>
		/// <param name="offsetX">The x value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
		/// <param name="offsetY">The y value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
		public void Translate(float offsetX, float offsetY)
		{
			this.OffsetX = offsetX;
			this.OffsetY = offsetY;
        }

		/// <summary>Applies the specified translation vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
		/// <param name="offsetX">The x value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
		/// <param name="offsetY">The y value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the translation is applied to this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</param>
		public void Translate(float offsetX, float offsetY, MatrixOrder order)
        {
			this.order = order;
			Translate(offsetX, offsetY);
        }

		/// <summary>Multiplies each vector in an array by the matrix. The translation elements of this matrix (third row) are ignored.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform.</param>
		public void VectorTransformPoints(Point[] pts)
		{
            //将数组中的每个矢量与矩阵相乘。 该矩阵的转换元素（第三行）被忽略。
   //         this.m11 = this.m11 * pts[0].X + this.m11 * pts[1].X;
			//this.m12 = this.m12 * pts[0].Y + this.m12 * pts[1].Y;

			//this.m21 = this.m21 * pts[0].X + this.m21 * pts[1].X;
			//this.m22 = this.m22 * pts[0].Y + this.m22 * pts[1].Y;
			this.vectorTransformPoints = pts;
		}
    }
}
