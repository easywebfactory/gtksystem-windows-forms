using System.Collections;
using System.Reflection;

namespace System.Drawing.Drawing2D
{
	/// <summary>Encapsulates a custom user-defined line cap.</summary>
	public class CustomLineCap : MarshalByRefObject, ICloneable, IDisposable
	{
		/// <summary>Gets or sets the <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration on which this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> is based.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration on which this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> is based.</returns>
		public LineCap BaseCap
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets the distance between the cap and the line.</summary>
		/// <returns>The distance between the beginning of the cap and the end of the line.</returns>
		public float BaseInset
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Drawing.Drawing2D.LineJoin" /> enumeration that determines how lines that compose this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> object are joined.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.LineJoin" /> enumeration this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> object uses to join lines.</returns>
		public LineJoin StrokeJoin
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets the amount by which to scale this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> Class object with respect to the width of the <see cref="T:System.Drawing.Pen" /> object.</summary>
		/// <returns>The amount by which to scale the cap.</returns>
		public float WidthScale
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> class with the specified outline and fill.</summary>
		/// <param name="fillPath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the fill for the custom cap.</param>
		/// <param name="strokePath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the outline of the custom cap.</param>
		public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> class from the specified existing <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration with the specified outline and fill.</summary>
		/// <param name="fillPath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the fill for the custom cap.</param>
		/// <param name="strokePath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the outline of the custom cap.</param>
		/// <param name="baseCap">The line cap from which to create the custom cap.</param>
		public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath, LineCap baseCap)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> class from the specified existing <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration with the specified outline, fill, and inset.</summary>
		/// <param name="fillPath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the fill for the custom cap.</param>
		/// <param name="strokePath">A <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object that defines the outline of the custom cap.</param>
		/// <param name="baseCap">The line cap from which to create the custom cap.</param>
		/// <param name="baseInset">The distance between the cap and the line.</param>
		public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath, LineCap baseCap, float baseInset)
		{
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> this method creates, cast as an object.</returns>
		public object Clone()
		{
            return null;
        }

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> object.</summary>
		public void Dispose()
		{
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
		}

		/// <summary>Allows an <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> to attempt to free resources and perform other cleanup operations before the <see cref="T:System.Drawing.Drawing2D.CustomLineCap" /> is reclaimed by garbage collection.</summary>
		~CustomLineCap()
		{
		}

		/// <summary>Gets the caps used to start and end lines that make up this custom cap.</summary>
		/// <param name="startCap">The <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration used at the beginning of a line within this cap.</param>
		/// <param name="endCap">The <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration used at the end of a line within this cap.</param>
		public void GetStrokeCaps(out LineCap startCap, out LineCap endCap)
		{
			throw null;
		}

		/// <summary>Sets the caps used to start and end lines that make up this custom cap.</summary>
		/// <param name="startCap">The <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration used at the beginning of a line within this cap.</param>
		/// <param name="endCap">The <see cref="T:System.Drawing.Drawing2D.LineCap" /> enumeration used at the end of a line within this cap.</param>
		public void SetStrokeCaps(LineCap startCap, LineCap endCap)
		{
		}
	}
}
