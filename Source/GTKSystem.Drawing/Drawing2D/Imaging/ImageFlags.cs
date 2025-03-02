namespace System.Drawing.Imaging;

/// <summary>Specifies the attributes of the pixel data contained in an <see cref="T:System.Drawing.Image" /> object. The <see cref="P:System.Drawing.Image.Flags" /> property returns a member of this enumeration.</summary>
[Flags]
public enum ImageFlags
{
    /// <summary>There is no format information.</summary>
    None = 0x0,
    /// <summary>The pixel data is scalable.</summary>
    Scalable = 0x1,
    /// <summary>The pixel data contains alpha information.</summary>
    HasAlpha = 0x2,
    /// <summary>Specifies that the pixel data has alpha values other than 0 (transparent) and 255 (opaque).</summary>
    HasTranslucent = 0x4,
    /// <summary>The pixel data is partially scalable, but there are some limitations.</summary>
    PartiallyScalable = 0x8,
    /// <summary>The pixel data uses an RGB color space.</summary>
    ColorSpaceRgb = 0x10,
    /// <summary>The pixel data uses a CMYK color space.</summary>
    ColorSpaceCmyk = 0x20,
    /// <summary>The pixel data is grayscale.</summary>
    ColorSpaceGray = 0x40,
    /// <summary>Specifies that the image is stored using a YCBCR color space.</summary>
    ColorSpaceYcbcr = 0x80,
    /// <summary>Specifies that the image is stored using a YCCK color space.</summary>
    ColorSpaceYcck = 0x100,
    /// <summary>Specifies that dots per inch information is stored in the image.</summary>
    HasRealDpi = 0x1000,
    /// <summary>Specifies that the pixel size is stored in the image.</summary>
    HasRealPixelSize = 0x2000,
    /// <summary>The pixel data is read-only.</summary>
    ReadOnly = 0x10000,
    /// <summary>The pixel data can be cached for faster access.</summary>
    Caching = 0x20000
}