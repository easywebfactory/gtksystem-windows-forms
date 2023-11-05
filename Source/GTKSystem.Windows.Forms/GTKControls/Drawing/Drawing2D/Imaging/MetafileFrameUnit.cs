namespace System.Drawing.Imaging
{
	/// <summary>Specifies the unit of measurement for the rectangle used to size and position a metafile. This is specified during the creation of the <see cref="T:System.Drawing.Imaging.Metafile" /> object.</summary>
	public enum MetafileFrameUnit
	{
		/// <summary>The unit of measurement is 1 pixel.</summary>
		Pixel = 2,
		/// <summary>The unit of measurement is 1 printer's point.</summary>
		Point,
		/// <summary>The unit of measurement is 1 inch.</summary>
		Inch,
		/// <summary>The unit of measurement is 1/300 of an inch.</summary>
		Document,
		/// <summary>The unit of measurement is 1 millimeter.</summary>
		Millimeter,
		/// <summary>The unit of measurement is 0.01 millimeter. Provided for compatibility with GDI.</summary>
		GdiCompatible
	}
}
