namespace System.Drawing.Drawing2D;

/// <summary>Specifies whether smoothing (antialiasing) is applied to lines and curves and the edges of filled areas.</summary>
public enum SmoothingMode
{
    /// <summary>Specifies an invalid mode.</summary>
    Invalid = -1,
    /// <summary>Specifies no antialiasing.</summary>
    Default,
    /// <summary>Specifies no antialiasing.</summary>
    HighSpeed,
    /// <summary>Specifies antialiased rendering.</summary>
    HighQuality,
    /// <summary>Specifies no antialiasing.</summary>
    None,
    /// <summary>Specifies antialiased rendering.</summary>
    AntiAlias
}