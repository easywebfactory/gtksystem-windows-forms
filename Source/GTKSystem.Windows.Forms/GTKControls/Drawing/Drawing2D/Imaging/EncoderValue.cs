namespace System.Drawing.Imaging;

/// <summary>Used to specify the parameter value passed to a JPEG or TIFF image encoder when using the <see cref="M:System.Drawing.Image.Save(System.String,System.Drawing.Imaging.ImageCodecInfo,System.Drawing.Imaging.EncoderParameters)" /> or <see cref="M:System.Drawing.Image.SaveAdd(System.Drawing.Imaging.EncoderParameters)" /> methods.</summary>
public enum EncoderValue
{
    /// <summary>Not used in GDI+ version 1.0.</summary>
    ColorTypeCmyk,
    /// <summary>Not used in GDI+ version 1.0.</summary>
    ColorTypeYcck,
    /// <summary>Specifies the LZW compression scheme. Can be passed to the TIFF encoder as a parameter that belongs to the Compression category.</summary>
    CompressionLzw,
    /// <summary>Specifies the CCITT3 compression scheme. Can be passed to the TIFF encoder as a parameter that belongs to the compression category.</summary>
    CompressionCcitt3,
    /// <summary>Specifies the CCITT4 compression scheme. Can be passed to the TIFF encoder as a parameter that belongs to the compression category.</summary>
    CompressionCcitt4,
    /// <summary>Specifies the RLE compression scheme. Can be passed to the TIFF encoder as a parameter that belongs to the compression category.</summary>
    CompressionRle,
    /// <summary>Specifies no compression. Can be passed to the TIFF encoder as a parameter that belongs to the compression category.</summary>
    CompressionNone,
    /// <summary>Not used in GDI+ version 1.0.</summary>
    ScanMethodInterlaced,
    /// <summary>Not used in GDI+ version 1.0.</summary>
    ScanMethodNonInterlaced,
    /// <summary>Not used in GDI+ version 1.0.</summary>
    VersionGif87,
    /// <summary>Not used in GDI+ version 1.0.</summary>
    VersionGif89,
    /// <summary>Not used in GDI+ version 1.0.</summary>
    RenderProgressive,
    /// <summary>Not used in GDI+ version 1.0.</summary>
    RenderNonProgressive,
    /// <summary>Specifies that the image is to be rotated clockwise 90 degrees about its center. Can be passed to the JPEG encoder as a parameter that belongs to the transformation category.</summary>
    TransformRotate90,
    /// <summary>Specifies that the image is to be rotated 180 degrees about its center. Can be passed to the JPEG encoder as a parameter that belongs to the transformation category.</summary>
    TransformRotate180,
    /// <summary>Specifies that the image is to be rotated clockwise 270 degrees about its center. Can be passed to the JPEG encoder as a parameter that belongs to the transformation category.</summary>
    TransformRotate270,
    /// <summary>Specifies that the image is to be flipped horizontally (about the vertical axis). Can be passed to the JPEG encoder as a parameter that belongs to the transformation category.</summary>
    TransformFlipHorizontal,
    /// <summary>Specifies that the image is to be flipped vertically (about the horizontal axis). Can be passed to the JPEG encoder as a parameter that belongs to the transformation category.</summary>
    TransformFlipVertical,
    /// <summary>Specifies that the image has more than one frame (page). Can be passed to the TIFF encoder as a parameter that belongs to the save flag category.</summary>
    MultiFrame,
    /// <summary>Specifies the last frame in a multiple-frame image. Can be passed to the TIFF encoder as a parameter that belongs to the save flag category.</summary>
    LastFrame,
    /// <summary>Specifies that a multiple-frame file or stream should be closed. Can be passed to the TIFF encoder as a parameter that belongs to the save flag category.</summary>
    Flush,
    /// <summary>Not used in GDI+ version 1.0.</summary>
    FrameDimensionTime,
    /// <summary>Not used in GDI+ version 1.0.</summary>
    FrameDimensionResolution,
    /// <summary>Specifies that a frame is to be added to the page dimension of an image. Can be passed to the TIFF encoder as a parameter that belongs to the save flag category.</summary>
    FrameDimensionPage
}