using System.Collections;
using System.Drawing.Drawing2D;

namespace System.Drawing
{
	/// <summary>Defines an object used to draw lines and curves. This class cannot be inherited.</summary>
	public sealed class Pen : MarshalByRefObject, ICloneable, IDisposable
	{
		/// <summary>Gets or sets the alignment for this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.PenAlignment" /> that represents the alignment for this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not a member of <see cref="T:System.Drawing.Drawing2D.PenAlignment" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.Alignment" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public PenAlignment Alignment
		{
			get;
			set;
		}

		/// <summary>Gets or sets the <see cref="T:System.Drawing.Brush" /> that determines attributes of this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> that determines attributes of this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.Brush" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public Brush Brush
		{
			get;
			set;
		}

		/// <summary>Gets or sets the color of this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure that represents the color of this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.Color" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public Color Color
		{
			get;
			set;
		}

		/// <summary>Gets or sets an array of values that specifies a compound pen. A compound pen draws a compound line made up of parallel lines and spaces.</summary>
		/// <returns>An array of real numbers that specifies the compound array. The elements in the array must be in increasing order, not less than 0, and not greater than 1.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.CompoundArray" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public float[] CompoundArray
		{
			get;
			set;
		}

		/// <summary>Gets or sets a custom cap to use at the end of lines drawn with this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> that represents the cap used at the end of lines drawn with this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.CustomEndCap" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public CustomLineCap CustomEndCap
		{
			get;
			set;
		}

		/// <summary>Gets or sets a custom cap to use at the beginning of lines drawn with this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> that represents the cap used at the beginning of lines drawn with this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.CustomStartCap" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public CustomLineCap CustomStartCap
		{
			get;
			set;
		}

		/// <summary>Gets or sets the cap style used at the end of the dashes that make up dashed lines drawn with this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Drawing2D.DashCap" /> values that represents the cap style used at the beginning and end of the dashes that make up dashed lines drawn with this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not a member of <see cref="T:System.Drawing.Drawing2D.DashCap" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.DashCap" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public DashCap DashCap
		{
			get;
			set;
		}

		/// <summary>Gets or sets the distance from the start of a line to the beginning of a dash pattern.</summary>
		/// <returns>The distance from the start of a line to the beginning of a dash pattern.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.DashOffset" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public float DashOffset
		{
			get;
			set;
		}

		/// <summary>Gets or sets an array of custom dashes and spaces.</summary>
		/// <returns>An array of real numbers that specifies the lengths of alternating dashes and spaces in dashed lines.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.DashPattern" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public float[] DashPattern
		{
			get;
			set;
		}

		/// <summary>Gets or sets the style used for dashed lines drawn with this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.DashStyle" /> that represents the style used for dashed lines drawn with this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.DashStyle" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public DashStyle DashStyle
		{
			get;
			set;
		}

		/// <summary>Gets or sets the cap style used at the end of lines drawn with this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Drawing2D.LineCap" /> values that represents the cap style used at the end of lines drawn with this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not a member of <see cref="T:System.Drawing.Drawing2D.LineCap" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.EndCap" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public LineCap EndCap
		{
			get;
			set;
		}

		/// <summary>Gets or sets the join style for the ends of two consecutive lines drawn with this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.LineJoin" /> that represents the join style for the ends of two consecutive lines drawn with this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.LineJoin" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public LineJoin LineJoin
		{
			get;
			set;
		}

		/// <summary>Gets or sets the limit of the thickness of the join on a mitered corner.</summary>
		/// <returns>The limit of the thickness of the join on a mitered corner.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.MiterLimit" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public float MiterLimit
		{
			get;
			set;
		}

		/// <summary>Gets the style of lines drawn with this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.PenType" /> enumeration that specifies the style of lines drawn with this <see cref="T:System.Drawing.Pen" />.</returns>
		public PenType PenType
		{
			get
			{
				throw null;
			}
		}

		/// <summary>Gets or sets the cap style used at the beginning of lines drawn with this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Drawing2D.LineCap" /> values that represents the cap style used at the beginning of lines drawn with this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not a member of <see cref="T:System.Drawing.Drawing2D.LineCap" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.StartCap" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public LineCap StartCap
		{
			get;
			set;
		}

		/// <summary>Gets or sets a copy of the geometric transformation for this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>A copy of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> that represents the geometric transformation for this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.Transform" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public Matrix Transform
		{
			get;
			set;
		}

		/// <summary>Gets or sets the width of this <see cref="T:System.Drawing.Pen" />, in units of the <see cref="T:System.Drawing.Graphics" /> object used for drawing.</summary>
		/// <returns>The width of this <see cref="T:System.Drawing.Pen" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Pen.Width" /> property is set on an immutable <see cref="T:System.Drawing.Pen" />, such as those returned by the <see cref="T:System.Drawing.Pens" /> class.</exception>
		public float Width
		{
			get;
			set;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Pen" /> class with the specified <see cref="T:System.Drawing.Brush" />.</summary>
		/// <param name="brush">A <see cref="T:System.Drawing.Brush" /> that determines the fill properties of this <see cref="T:System.Drawing.Pen" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="brush" /> is <see langword="null" />.</exception>
		public Pen(Brush brush)
		{
			this.Brush = brush;
			if (brush is SolidBrush solid)
				this.Color = solid.Color;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Pen" /> class with the specified <see cref="T:System.Drawing.Brush" /> and <see cref="P:System.Drawing.Pen.Width" />.</summary>
		/// <param name="brush">A <see cref="T:System.Drawing.Brush" /> that determines the characteristics of this <see cref="T:System.Drawing.Pen" />.</param>
		/// <param name="width">The width of the new <see cref="T:System.Drawing.Pen" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="brush" /> is <see langword="null" />.</exception>
		public Pen(Brush brush, float width):this(brush)
		{
			this.Width = width;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Pen" /> class with the specified color.</summary>
		/// <param name="color">A <see cref="T:System.Drawing.Color" /> structure that indicates the color of this <see cref="T:System.Drawing.Pen" />.</param>
		public Pen(Color color)
		{
			this.Color = color;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Pen" /> class with the specified <see cref="T:System.Drawing.Color" /> and <see cref="P:System.Drawing.Pen.Width" /> properties.</summary>
		/// <param name="color">A <see cref="T:System.Drawing.Color" /> structure that indicates the color of this <see cref="T:System.Drawing.Pen" />.</param>
		/// <param name="width">A value indicating the width of this <see cref="T:System.Drawing.Pen" />.</param>
		public Pen(Color color, float width)
		{
			this.Color = color;
			this.Width = width;
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that can be cast to a <see cref="T:System.Drawing.Pen" />.</returns>
		public object Clone()
		{
            return null;
        }

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Pen" />.</summary>
		public void Dispose()
		{
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		~Pen()
		{
		}

		/// <summary>Multiplies the transformation matrix for this <see cref="T:System.Drawing.Pen" /> by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> object by which to multiply the transformation matrix.</param>
		public void MultiplyTransform(Matrix matrix)
		{
		}

		/// <summary>Multiplies the transformation matrix for this <see cref="T:System.Drawing.Pen" /> by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which to multiply the transformation matrix.</param>
		/// <param name="order">The order in which to perform the multiplication operation.</param>
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
		}

		/// <summary>Resets the geometric transformation matrix for this <see cref="T:System.Drawing.Pen" /> to identity.</summary>
		public void ResetTransform()
		{
		}

		/// <summary>Rotates the local geometric transformation by the specified angle. This method prepends the rotation to the transformation.</summary>
		/// <param name="angle">The angle of rotation.</param>
		public void RotateTransform(float angle)
		{
		}

		/// <summary>Rotates the local geometric transformation by the specified angle in the specified order.</summary>
		/// <param name="angle">The angle of rotation.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies whether to append or prepend the rotation matrix.</param>
		public void RotateTransform(float angle, MatrixOrder order)
		{
		}

		/// <summary>Scales the local geometric transformation by the specified factors. This method prepends the scaling matrix to the transformation.</summary>
		/// <param name="sx">The factor by which to scale the transformation in the x-axis direction.</param>
		/// <param name="sy">The factor by which to scale the transformation in the y-axis direction.</param>
		public void ScaleTransform(float sx, float sy)
		{
		}

		/// <summary>Scales the local geometric transformation by the specified factors in the specified order.</summary>
		/// <param name="sx">The factor by which to scale the transformation in the x-axis direction.</param>
		/// <param name="sy">The factor by which to scale the transformation in the y-axis direction.</param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies whether to append or prepend the scaling matrix.</param>
		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
		}

		/// <summary>Sets the values that determine the style of cap used to end lines drawn by this <see cref="T:System.Drawing.Pen" />.</summary>
		/// <param name="startCap">A <see cref="T:System.Drawing.Drawing2D.LineCap" /> that represents the cap style to use at the beginning of lines drawn with this <see cref="T:System.Drawing.Pen" />.</param>
		/// <param name="endCap">A <see cref="T:System.Drawing.Drawing2D.LineCap" /> that represents the cap style to use at the end of lines drawn with this <see cref="T:System.Drawing.Pen" />.</param>
		/// <param name="dashCap">A <see cref="T:System.Drawing.Drawing2D.LineCap" /> that represents the cap style to use at the beginning or end of dashed lines drawn with this <see cref="T:System.Drawing.Pen" />.</param>
		public void SetLineCap(LineCap startCap, LineCap endCap, DashCap dashCap)
		{
		}

		/// <summary>Translates the local geometric transformation by the specified dimensions. This method prepends the translation to the transformation.</summary>
		/// <param name="dx">The value of the translation in x.</param>
		/// <param name="dy">The value of the translation in y.</param>
		public void TranslateTransform(float dx, float dy)
		{
		}

		/// <summary>Translates the local geometric transformation by the specified dimensions in the specified order.</summary>
		/// <param name="dx">The value of the translation in x.</param>
		/// <param name="dy">The value of the translation in y.</param>
		/// <param name="order">The order (prepend or append) in which to apply the translation.</param>
		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
		}
	}
}
