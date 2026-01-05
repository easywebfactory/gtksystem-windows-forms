
using System.Collections;
using System.ComponentModel;

namespace System.Drawing.Drawing2D
{
	public sealed class LinearGradientBrush : Brush
	{

		public Blend Blend
		{
            get;
            set;
        }

		public bool GammaCorrection
		{
			get;
			set;
		}

		public ColorBlend InterpolationColors
		{
            get;
            set;
        }

		public Color[] LinearColors
		{
			get;
			set;
		}

		public RectangleF Rectangle
		{
            get;
            private set;
        }

		public Matrix Transform
		{
			get;
			set;
		}
		public WrapMode WrapMode
		{
			get;
			set;
		} = WrapMode.Tile;
        
        //internal float angle;
        internal LinearGradientMode linearGradientMode;
        //internal bool isAngleScaleable;
        //internal PointF translate = new PointF();
        //internal MatrixOrder order = MatrixOrder.Prepend;
        //internal PointF scale = new PointF();
        internal Cairo.Matrix cairomatrix = new Cairo.Matrix();
        public LinearGradientBrush(PointF point1, PointF point2, Color color1, Color color2)
        {
            this.Rectangle = new RectangleF(point1, new SizeF(point2.X - point1.X, point2.Y - point1.Y));
            this.LinearColors = new Color[2] { color1, color2 };
            this.cairomatrix.InitRotate(-Math.PI * 6 / 180);
        }

        public LinearGradientBrush(Point point1, Point point2, Color color1, Color color2)
            : this((PointF)point1, (PointF)point2, color1, color2)
        {
        }

        public LinearGradientBrush(RectangleF rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
        {
            if (linearGradientMode is < LinearGradientMode.Horizontal or > LinearGradientMode.BackwardDiagonal)
                throw new InvalidEnumArgumentException(nameof(linearGradientMode), (int)linearGradientMode, typeof(LinearGradientMode));

            if (rect.Width == 0.0 || rect.Height == 0.0)
                throw new ArgumentException($"Invalid Rectangle {rect.ToString()}");

            this.Rectangle = rect;
            this.LinearColors = new Color[2] { color1, color2 };
            this.linearGradientMode = linearGradientMode;
            this.cairomatrix.InitRotate(-Math.PI * 6 / 180);
        }

        public LinearGradientBrush(Rectangle rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
            : this((RectangleF)rect, color1, color2, linearGradientMode)
        {
        }

        public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle)
            : this(rect, color1, color2, angle, isAngleScaleable: false)
        {
        }

        public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle, bool isAngleScaleable)
        {
            if (rect.Width == 0.0 || rect.Height == 0.0)
                throw new ArgumentException($"Invalid Rectangle {rect.ToString()}");
            this.Rectangle = rect;
            this.LinearColors = new Color[2] { color1, color2 };
            this.cairomatrix.InitRotate(-Math.PI * 6 / 180);
            this.cairomatrix.Rotate((22 - angle) * Math.PI / 180);
        }

        public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle)
            : this(rect, color1, color2, angle, isAngleScaleable: false)
        {
        }

        public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle, bool isAngleScaleable)
            : this((RectangleF)rect, color1, color2, angle, isAngleScaleable)
        {
        }

        public override object Clone()
		{
            return null;
        }

		public void MultiplyTransform(Matrix matrix)
		{
            MultiplyTransform(matrix, MatrixOrder.Append);

        }

		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
        {
            this.cairomatrix.Multiply(new Cairo.Matrix(matrix.m11, matrix.m12, matrix.m21, matrix.m22, matrix.dx, matrix.dy));

        }

		public void ResetTransform()
		{
            this.cairomatrix.InitRotate(Math.PI * 6 / 180);
        }

		public void RotateTransform(float angle)
		{
            RotateTransform(angle, MatrixOrder.Append);
        }

        public void RotateTransform(float angle, MatrixOrder order)
		{
            this.cairomatrix.Rotate((22 - angle) * Math.PI / 180);
        }

		public void ScaleTransform(float sx, float sy)
		{
            ScaleTransform(sx, sy, MatrixOrder.Append);
        }

		public void ScaleTransform(float sx, float sy, MatrixOrder order)
        {
            this.cairomatrix.Scale(sx, sy);
        }

		public void SetBlendTriangularShape(float focus)
		{
		}

		public void SetBlendTriangularShape(float focus, float scale)
		{
		}

		public void SetSigmaBellShape(float focus)
		{
		}

		public void SetSigmaBellShape(float focus, float scale)
		{
		}

        public void TranslateTransform(float dx, float dy)
		{
            TranslateTransform(dx, dy, MatrixOrder.Append);
        }

		public void TranslateTransform(float dx, float dy, MatrixOrder order)
        {
            this.cairomatrix.Translate(dx, dy);
        }
	}
}
