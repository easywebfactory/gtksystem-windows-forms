namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies the type of fill a <see cref="T:System.Drawing.Pen" /> object uses to fill lines.</summary>
	public enum PenType
	{
		/// <summary>Specifies a solid fill.</summary>
		SolidColor,
		/// <summary>Specifies a hatch fill.</summary>
		HatchFill,
		/// <summary>Specifies a bitmap texture fill.</summary>
		TextureFill,
		/// <summary>Specifies a path gradient fill.</summary>
		PathGradient,
		/// <summary>Specifies a linear gradient fill.</summary>
		LinearGradient
	}
}
