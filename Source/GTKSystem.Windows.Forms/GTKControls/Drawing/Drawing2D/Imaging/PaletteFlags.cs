namespace System.Drawing.Imaging
{
	/// <summary>Specifies the type of color data in the system palette. The data can be color data with alpha, grayscale data only, or halftone data.</summary>
	[Flags]
	public enum PaletteFlags
	{
		/// <summary>Alpha data.</summary>
		HasAlpha = 0x1,
		/// <summary>Grayscale data.</summary>
		GrayScale = 0x2,
		/// <summary>Halftone data.</summary>
		Halftone = 0x4
	}
}
