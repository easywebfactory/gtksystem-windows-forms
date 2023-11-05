namespace System.Drawing.Imaging
{
	/// <summary>Defines a map for converting colors. Several methods of the <see cref="T:System.Drawing.Imaging.ImageAttributes" /> class adjust image colors by using a color-remap table, which is an array of <see cref="T:System.Drawing.Imaging.ColorMap" /> structures. Not inheritable.</summary>
	public sealed class ColorMap
	{
		private Color _oldColor;

		private Color _newColor;

		/// <summary>Gets or sets the existing <see cref="T:System.Drawing.Color" /> structure to be converted.</summary>
		/// <returns>The existing <see cref="T:System.Drawing.Color" /> structure to be converted.</returns>
		public Color OldColor
		{
			get
			{
				return _oldColor;
			}
			set
			{
				_oldColor = value;
			}
		}

		/// <summary>Gets or sets the new <see cref="T:System.Drawing.Color" /> structure to which to convert.</summary>
		/// <returns>The new <see cref="T:System.Drawing.Color" /> structure to which to convert.</returns>
		public Color NewColor
		{
			get
			{
				return _newColor;
			}
			set
			{
				_newColor = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.ColorMap" /> class.</summary>
		public ColorMap()
		{
		}
	}
}
