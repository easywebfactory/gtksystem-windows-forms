namespace System.Drawing.Drawing2D;

/// <summary>Specifies how to join consecutive line or curve segments in a figure (subpath) contained in a <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object.</summary>
public enum LineJoin
{
    /// <summary>Specifies a mitered join. This produces a sharp corner or a clipped corner, depending on whether the length of the miter exceeds the miter limit.</summary>
    Miter,
    /// <summary>Specifies a beveled join. This produces a diagonal corner.</summary>
    Bevel,
    /// <summary>Specifies a circular join. This produces a smooth, circular arc between the lines.</summary>
    Round,
    /// <summary>Specifies a mitered join. This produces a sharp corner or a beveled corner, depending on whether the length of the miter exceeds the miter limit.</summary>
    MiterClipped
}