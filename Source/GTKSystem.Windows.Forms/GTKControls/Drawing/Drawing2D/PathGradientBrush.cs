using System.Collections;
using System.Linq;

namespace System.Drawing.Drawing2D
{
	/// <summary>Encapsulates a <see cref="T:System.Drawing.Brush" /> object that fills the interior of a <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object with a gradient. This class cannot be inherited.</summary>
	public sealed class PathGradientBrush : Brush
	{
		/// <summary>Gets or sets a <see cref="T:System.Drawing.Drawing2D.Blend" /> that specifies positions and factors that define a custom falloff for the gradient.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.Blend" /> that represents a custom falloff for the gradient.</returns>
		public Blend Blend
		{
            get;
            set;
        }

		/// <summary>Gets or sets the color at the center of the path gradient.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color at the center of the path gradient.</returns>
		public Color CenterColor
		{
			get;
			set;
		} = Color.Black;

		/// <summary>Gets or sets the center point of the path gradient.</summary>
		/// <returns>A <see cref="T:System.Drawing.PointF" /> that represents the center point of the path gradient.</returns>
		public PointF CenterPoint
		{
			get;
			set;
		}

		/// <summary>Gets or sets the focus point for the gradient falloff.</summary>
		/// <returns>A <see cref="T:System.Drawing.PointF" /> that represents the focus point for the gradient falloff.</returns>
		public PointF FocusScales
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

		/// <summary>Gets a bounding rectangle for this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.RectangleF" /> that represents a rectangular region that bounds the path this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> fills.</returns>
		public RectangleF Rectangle
		{
            get;
            private set;
        }

		/// <summary>Gets or sets an array of colors that correspond to the points in the path this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> fills.</summary>
		/// <returns>An array of <see cref="T:System.Drawing.Color" /> structures that represents the colors associated with each point in the path this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> fills.</returns>
		public Color[] SurroundColors
		{
			get;
			set;
		}

		/// <summary>Gets or sets a copy of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> that defines a local geometric transform for this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" />.</summary>
		/// <returns>A copy of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> that defines a geometric transform that applies only to fills drawn with this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" />.</returns>
		public Matrix Transform
		{
			get;
			set;
		}

		/// <summary>Gets or sets a <see cref="T:System.Drawing.Drawing2D.WrapMode" /> that indicates the wrap mode for this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.WrapMode" /> that specifies how fills drawn with this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> are tiled.</returns>
		public WrapMode WrapMode
		{
			get;
			set;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> class with the specified path.</summary>
		/// <param name="path">The <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> that defines the area filled by this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" />.</param>
		public PathGradientBrush(GraphicsPath path)
		{
			float left = path.SizePoints.Select(o => o.X).Min();
            float top = path.SizePoints.Select(o => o.Y).Min();
            float right = path.SizePoints.Select(o => o.X).Max();
            float bottom = path.SizePoints.Select(o => o.Y).Max();
			this.Rectangle = new RectangleF(left, top, right - left, bottom - top);
        }

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> class with the specified points.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that make up the vertices of the path.</param>
		public PathGradientBrush(PointF[] points) : this(points, WrapMode.Clamp)
        {

        }

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> class with the specified points and wrap mode.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points that make up the vertices of the path.</param>
		/// <param name="wrapMode">A <see cref="T:System.Drawing.Drawing2D.WrapMode" /> that specifies how fills drawn with this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> are tiled.</param>
		public PathGradientBrush(PointF[] points, WrapMode wrapMode)
        {
            float left = points.Select(o => o.X).Min();
            float top = points.Select(o => o.Y).Min();
            float right = points.Select(o => o.X).Max();
            float bottom = points.Select(o => o.Y).Max();
            this.Rectangle = new RectangleF(left, top, right - left, bottom - top);
        }

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> class with the specified points.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that make up the vertices of the path.</param>
		public PathGradientBrush(Point[] points) : this(points, WrapMode.Clamp)
        {
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> class with the specified points and wrap mode.</summary>
		/// <param name="points">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points that make up the vertices of the path.</param>
		/// <param name="wrapMode">A <see cref="T:System.Drawing.Drawing2D.WrapMode" /> that specifies how fills drawn with this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> are tiled.</param>
		public PathGradientBrush(Point[] points, WrapMode wrapMode)
        {
            float left = points.Select(o => o.X).Min();
            float top = points.Select(o => o.Y).Min();
            float right = points.Select(o => o.X).Max();
            float bottom = points.Select(o => o.Y).Max();
            this.Rectangle = new RectangleF(left, top, right - left, bottom - top);
        }

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.PathGradientBrush" /> this method creates, cast as an object.</returns>
		public override object Clone()
		{
			return null;

        }

		/// <summary>Updates the brush's transformation matrix with the product of brush's transformation matrix multiplied by another matrix.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> that will be multiplied by the brush's current transformation matrix.</param>
		public void MultiplyTransform(Matrix matrix)
		{
		}

		/// <summary>Updates the brush's transformation matrix with the product of the brush's transformation matrix multiplied by another matrix.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> that will be multiplied by the brush's current transformation matrix.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies in which order to multiply the two matrices.</param>
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
		}

		/// <summary>Resets the <see cref="P:System.Drawing.Drawing2D.PathGradientBrush.Transform" /> property to identity.</summary>
		public void ResetTransform()
		{
		}

		/// <summary>Rotates the local geometric transform by the specified amount. This method prepends the rotation to the transform.</summary>
		/// <param name="angle">The angle (extent) of rotation.</param>
		public void RotateTransform(float angle)
		{
		}

		/// <summary>Rotates the local geometric transform by the specified amount in the specified order.</summary>
		/// <param name="angle">The angle (extent) of rotation.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies whether to append or prepend the rotation matrix.</param>
		public void RotateTransform(float angle, MatrixOrder order)
		{
		}

		/// <summary>Scales the local geometric transform by the specified amounts. This method prepends the scaling matrix to the transform.</summary>
		/// <param name="sx">The transform scale factor in the x-axis direction.</param>
		/// <param name="sy">The transform scale factor in the y-axis direction.</param>
		public void ScaleTransform(float sx, float sy)
		{
		}

		/// <summary>Scales the local geometric transform by the specified amounts in the specified order.</summary>
		/// <param name="sx">The transform scale factor in the x-axis direction.</param>
		/// <param name="sy">The transform scale factor in the y-axis direction.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies whether to append or prepend the scaling matrix.</param>
		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
		}

		/// <summary>Creates a gradient with a center color and a linear falloff to one surrounding color.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies where, along any radial from the center of the path to the path's boundary, the center color will be at its highest intensity. A value of 1 (the default) places the highest intensity at the center of the path.</param>
		public void SetBlendTriangularShape(float focus)
		{
		}

		/// <summary>Creates a gradient with a center color and a linear falloff to each surrounding color.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies where, along any radial from the center of the path to the path's boundary, the center color will be at its highest intensity. A value of 1 (the default) places the highest intensity at the center of the path.</param>
		/// <param name="scale">A value from 0 through 1 that specifies the maximum intensity of the center color that gets blended with the boundary color. A value of 1 causes the highest possible intensity of the center color, and it is the default value.</param>
		public void SetBlendTriangularShape(float focus, float scale)
		{
		}

		/// <summary>Creates a gradient brush that changes color starting from the center of the path outward to the path's boundary. The transition from one color to another is based on a bell-shaped curve.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies where, along any radial from the center of the path to the path's boundary, the center color will be at its highest intensity. A value of 1 (the default) places the highest intensity at the center of the path.</param>
		public void SetSigmaBellShape(float focus)
		{
		}

		/// <summary>Creates a gradient brush that changes color starting from the center of the path outward to the path's boundary. The transition from one color to another is based on a bell-shaped curve.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies where, along any radial from the center of the path to the path's boundary, the center color will be at its highest intensity. A value of 1 (the default) places the highest intensity at the center of the path.</param>
		/// <param name="scale">A value from 0 through 1 that specifies the maximum intensity of the center color that gets blended with the boundary color. A value of 1 causes the highest possible intensity of the center color, and it is the default value.</param>
		public void SetSigmaBellShape(float focus, float scale)
		{
		}

		/// <summary>Applies the specified translation to the local geometric transform. This method prepends the translation to the transform.</summary>
		/// <param name="dx">The value of the translation in x.</param>
		/// <param name="dy">The value of the translation in y.</param>
		public void TranslateTransform(float dx, float dy)
		{
		}

		/// <summary>Applies the specified translation to the local geometric transform in the specified order.</summary>
		/// <param name="dx">The value of the translation in x.</param>
		/// <param name="dy">The value of the translation in y.</param>
		/// <param name="order">The order (prepend or append) in which to apply the translation.</param>
		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
		}
	}
}
