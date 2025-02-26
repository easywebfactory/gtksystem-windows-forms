using System.Collections;

namespace System.Drawing.Drawing2D
{
	/// <summary>Encapsulates a <see cref="T:System.Drawing.Brush" /> with a linear gradient. This class cannot be inherited.</summary>
	public sealed class LinearGradientBrush : Brush
	{
		/// <summary>Gets or sets a <see cref="T:System.Drawing.Drawing2D.Blend" /> that specifies positions and factors that define a custom falloff for the gradient.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.Blend" /> that represents a custom falloff for the gradient.</returns>
		public Blend Blend
		{
            get;
            set;
        }

		/// <summary>Gets or sets a value indicating whether gamma correction is enabled for this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />.</summary>
		/// <returns>The value is <see langword="true" /> if gamma correction is enabled for this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />; otherwise, <see langword="false" />.</returns>
		public bool GammaCorrection
		{
			get;
			set;
		}

		/// <summary>Gets or sets a <see cref="T:System.Drawing.Drawing2D.ColorBlend" /> that defines a multicolor linear gradient.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.ColorBlend" /> that defines a multicolor linear gradient.</returns>
		public ColorBlend InterpolationColors
		{
            get;
            set;
        }

		/// <summary>Gets or sets the starting and ending colors of the gradient.</summary>
		/// <returns>An array of two <see cref="T:System.Drawing.Color" /> structures that represents the starting and ending colors of the gradient.</returns>
		public Color[] LinearColors
		{
			get;
			set;
		}

		/// <summary>Gets a rectangular region that defines the starting and ending points of the gradient.</summary>
		/// <returns>A <see cref="T:System.Drawing.RectangleF" /> structure that specifies the starting and ending points of the gradient.</returns>
		public RectangleF Rectangle
		{
            get;
            private set;
        }

		/// <summary>Gets or sets a copy <see cref="T:System.Drawing.Drawing2D.Matrix" /> that defines a local geometric transform for this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />.</summary>
		/// <returns>A copy of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> that defines a geometric transform that applies only to fills drawn with this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />.</returns>
		public Matrix Transform
		{
			get;
			set;
		}

		/// <summary>Gets or sets a <see cref="T:System.Drawing.Drawing2D.WrapMode" /> enumeration that indicates the wrap mode for this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.WrapMode" /> that specifies how fills drawn with this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> are tiled.</returns>
		public WrapMode WrapMode
		{
			get;
			set;
		} = WrapMode.Tile;
        public LinearGradientMode LinearGradientMode
        {
            get;
            set;
        }
        public float Angle
        {
            get;
            set;
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class with the specified points and colors.</summary>
        /// <param name="point1">A <see cref="T:System.Drawing.Point" /> structure that represents the starting point of the linear gradient.</param>
        /// <param name="point2">A <see cref="T:System.Drawing.Point" /> structure that represents the endpoint of the linear gradient.</param>
        /// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color of the linear gradient.</param>
        /// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color of the linear gradient.</param>
        public LinearGradientBrush(Point point1, Point point2, Color color1, Color color2)
		{
            this.Rectangle = new RectangleF(point1.X, point1.Y, Math.Abs(point2.X - point1.X), Math.Abs(point2.Y - point1.Y));
            this.LinearColors = new Color[2] { color1, color2 };
        }

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class with the specified points and colors.</summary>
		/// <param name="point1">A <see cref="T:System.Drawing.PointF" /> structure that represents the starting point of the linear gradient.</param>
		/// <param name="point2">A <see cref="T:System.Drawing.PointF" /> structure that represents the endpoint of the linear gradient.</param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color of the linear gradient.</param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color of the linear gradient.</param>
		public LinearGradientBrush(PointF point1, PointF point2, Color color1, Color color2)
        {
            this.Rectangle = new RectangleF(point1.X, point1.Y, Math.Abs(point2.X - point1.X), Math.Abs(point2.Y - point1.Y));
            this.LinearColors = new Color[2] { color1, color2 };
        }

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class based on a rectangle, starting and ending colors, and orientation.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that specifies the bounds of the linear gradient.</param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient.</param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient.</param>
		/// <param name="linearGradientMode">A <see cref="T:System.Drawing.Drawing2D.LinearGradientMode" /> enumeration element that specifies the orientation of the gradient. The orientation determines the starting and ending points of the gradient. For example, <see langword="LinearGradientMode.ForwardDiagonal" /> specifies that the starting point is the upper-left corner of the rectangle and the ending point is the lower-right corner of the rectangle.</param>
		public LinearGradientBrush(Rectangle rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
        {
            this.Rectangle = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            this.LinearColors = new Color[2] { color1, color2 };
			this.LinearGradientMode = linearGradientMode;
        }

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class based on a rectangle, starting and ending colors, and an orientation angle.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that specifies the bounds of the linear gradient.</param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient.</param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line.</param>
		public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle)
        {
            this.Rectangle = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            this.LinearColors = new Color[2] { color1, color2 };
            this.Angle = angle;
            this.WrapMode = WrapMode.Clamp;
        }

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class based on a rectangle, starting and ending colors, and an orientation angle.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that specifies the bounds of the linear gradient.</param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient.</param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line.</param>
		/// <param name="isAngleScaleable">Set to <see langword="true" /> to specify that the angle is affected by the transform associated with this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />; otherwise, <see langword="false" />.</param>
		public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle, bool isAngleScaleable)
        {
            this.Rectangle = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            this.LinearColors = new Color[2] { color1, color2 };
            this.Angle = angle;
            this.WrapMode = WrapMode.Clamp;
        }

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> based on a rectangle, starting and ending colors, and an orientation mode.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> structure that specifies the bounds of the linear gradient.</param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient.</param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient.</param>
		/// <param name="linearGradientMode">A <see cref="T:System.Drawing.Drawing2D.LinearGradientMode" /> enumeration element that specifies the orientation of the gradient. The orientation determines the starting and ending points of the gradient. For example, <see langword="LinearGradientMode.ForwardDiagonal" /> specifies that the starting point is the upper-left corner of the rectangle and the ending point is the lower-right corner of the rectangle.</param>
		public LinearGradientBrush(RectangleF rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
        {
            this.Rectangle = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            this.LinearColors = new Color[2] { color1, color2 };
            this.LinearGradientMode = linearGradientMode;
        }

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class based on a rectangle, starting and ending colors, and an orientation angle.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> structure that specifies the bounds of the linear gradient.</param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient.</param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line.</param>
		public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle)
        {
            this.Rectangle = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            this.LinearColors = new Color[2] { color1, color2 };
            this.Angle = angle;
            this.WrapMode = WrapMode.Clamp;
        }

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class based on a rectangle, starting and ending colors, and an orientation angle.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> structure that specifies the bounds of the linear gradient.</param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient.</param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line.</param>
		/// <param name="isAngleScaleable">Set to <see langword="true" /> to specify that the angle is affected by the transform associated with this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />; otherwise, <see langword="false" />.</param>
		public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle, bool isAngleScaleable)
        {
            this.Rectangle = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            this.LinearColors = new Color[2] { color1, color2 };
            this.Angle = angle;
            this.WrapMode = WrapMode.Clamp;
        }

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> this method creates, cast as an object.</returns>
		public override object Clone()
		{
            return null;
        }

		/// <summary>Multiplies the <see cref="T:System.Drawing.Drawing2D.Matrix" /> that represents the local geometric transform of this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> by prepending the specified <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which to multiply the geometric transform.</param>
		public void MultiplyTransform(Matrix matrix)
		{
		}

		/// <summary>Multiplies the <see cref="T:System.Drawing.Drawing2D.Matrix" /> that represents the local geometric transform of this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which to multiply the geometric transform.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies in which order to multiply the two matrices.</param>
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
		}

		/// <summary>Resets the <see cref="P:System.Drawing.Drawing2D.LinearGradientBrush.Transform" /> property to identity.</summary>
		public void ResetTransform()
		{
		}

		/// <summary>Rotates the local geometric transform by the specified amount. This method prepends the rotation to the transform.</summary>
		/// <param name="angle">The angle of rotation.</param>
		public void RotateTransform(float angle)
		{
		}

		/// <summary>Rotates the local geometric transform by the specified amount in the specified order.</summary>
		/// <param name="angle">The angle of rotation.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies whether to append or prepend the rotation matrix.</param>
		public void RotateTransform(float angle, MatrixOrder order)
		{
		}

		/// <summary>Scales the local geometric transform by the specified amounts. This method prepends the scaling matrix to the transform.</summary>
		/// <param name="sx">The amount by which to scale the transform in the x-axis direction.</param>
		/// <param name="sy">The amount by which to scale the transform in the y-axis direction.</param>
		public void ScaleTransform(float sx, float sy)
		{
		}

		/// <summary>Scales the local geometric transform by the specified amounts in the specified order.</summary>
		/// <param name="sx">The amount by which to scale the transform in the x-axis direction.</param>
		/// <param name="sy">The amount by which to scale the transform in the y-axis direction.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies whether to append or prepend the scaling matrix.</param>
		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
		}

		/// <summary>Creates a linear gradient with a center color and a linear falloff to a single color on both ends.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies the center of the gradient (the point where the gradient is composed of only the ending color).</param>
		public void SetBlendTriangularShape(float focus)
		{
		}

		/// <summary>Creates a linear gradient with a center color and a linear falloff to a single color on both ends.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies the center of the gradient (the point where the gradient is composed of only the ending color).</param>
		/// <param name="scale">A value from 0 through1 that specifies how fast the colors falloff from the starting color to <paramref name="focus" /> (ending color)</param>
		public void SetBlendTriangularShape(float focus, float scale)
		{
		}

		/// <summary>Creates a gradient falloff based on a bell-shaped curve.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies the center of the gradient (the point where the starting color and ending color are blended equally).</param>
		public void SetSigmaBellShape(float focus)
		{
		}

		/// <summary>Creates a gradient falloff based on a bell-shaped curve.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies the center of the gradient (the point where the gradient is composed of only the ending color).</param>
		/// <param name="scale">A value from 0 through 1 that specifies how fast the colors falloff from the <paramref name="focus" />.</param>
		public void SetSigmaBellShape(float focus, float scale)
		{
		}

		/// <summary>Translates the local geometric transform by the specified dimensions. This method prepends the translation to the transform.</summary>
		/// <param name="dx">The value of the translation in x.</param>
		/// <param name="dy">The value of the translation in y.</param>
		public void TranslateTransform(float dx, float dy)
		{
		}

		/// <summary>Translates the local geometric transform by the specified dimensions in the specified order.</summary>
		/// <param name="dx">The value of the translation in x.</param>
		/// <param name="dy">The value of the translation in y.</param>
		/// <param name="order">The order (prepend or append) in which to apply the translation.</param>
		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
		}
	}
}
