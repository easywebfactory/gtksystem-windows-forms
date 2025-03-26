namespace System.Drawing;

/// <summary>Specifies how much an image is rotated and the axis used to flip the image.</summary>
public enum RotateFlipType
{
    /// <summary>Specifies no clockwise rotation and no flipping.</summary>
    RotateNoneFlipNone = 0,
    /// <summary>Specifies a 90-degree clockwise rotation without flipping.</summary>
    Rotate90FlipNone = 1,
    /// <summary>Specifies a 180-degree clockwise rotation without flipping.</summary>
    Rotate180FlipNone = 2,
    /// <summary>Specifies a 270-degree clockwise rotation without flipping.</summary>
    Rotate270FlipNone = 3,
    /// <summary>Specifies no clockwise rotation followed by a horizontal flip.</summary>
    RotateNoneFlipX = 4,
    /// <summary>Specifies a 90-degree clockwise rotation followed by a horizontal flip.</summary>
    Rotate90FlipX = 5,
    /// <summary>Specifies a 180-degree clockwise rotation followed by a horizontal flip.</summary>
    Rotate180FlipX = 6,
    /// <summary>Specifies a 270-degree clockwise rotation followed by a horizontal flip.</summary>
    Rotate270FlipX = 7,
    /// <summary>Specifies no clockwise rotation followed by a vertical flip.</summary>
    RotateNoneFlipY = 6,
    /// <summary>Specifies a 90-degree clockwise rotation followed by a vertical flip.</summary>
    Rotate90FlipY = 7,
    /// <summary>Specifies a 180-degree clockwise rotation followed by a vertical flip.</summary>
    Rotate180FlipY = 4,
    /// <summary>Specifies a 270-degree clockwise rotation followed by a vertical flip.</summary>
    Rotate270FlipY = 5,
    /// <summary>Specifies no clockwise rotation followed by a horizontal and vertical flip.</summary>
    RotateNoneFlipXy = 2,
    /// <summary>Specifies a 90-degree clockwise rotation followed by a horizontal and vertical flip.</summary>
    Rotate90FlipXy = 3,
    /// <summary>Specifies a 180-degree clockwise rotation followed by a horizontal and vertical flip.</summary>
    Rotate180FlipXy = 0,
    /// <summary>Specifies a 270-degree clockwise rotation followed by a horizontal and vertical flip.</summary>
    Rotate270FlipXy = 1
}