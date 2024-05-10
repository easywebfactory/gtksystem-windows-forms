namespace System.Drawing.Drawing2D
{
	/// <summary>Defines a blend pattern for a <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> object. This class cannot be inherited.</summary>
	public sealed class Blend
	{
		/// <summary>Gets or sets an array of blend factors for the gradient.</summary>
		/// <returns>An array of blend factors that specify the percentages of the starting color and the ending color to be used at the corresponding position.</returns>
		public float[] Factors
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets an array of blend positions for the gradient.</summary>
		/// <returns>An array of blend positions that specify the percentages of distance along the gradient line.</returns>
		public float[] Positions
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Blend" /> class.</summary>
		public Blend()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Blend" /> class with the specified number of factors and positions.</summary>
		/// <param name="count">The number of elements in the <see cref="P:System.Drawing.Drawing2D.Blend.Factors" /> and <see cref="P:System.Drawing.Drawing2D.Blend.Positions" /> arrays.</param>
		public Blend(int count)
		{
		}
	}
}
