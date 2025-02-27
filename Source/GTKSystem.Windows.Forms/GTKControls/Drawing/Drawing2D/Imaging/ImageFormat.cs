using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Drawing.Imaging;

/// <summary>Specifies the file format of the image. Not inheritable.</summary>
[TypeConverter(typeof(ImageFormatConverter))]
public sealed class ImageFormat
{
    private static readonly ImageFormat? memoryBmp = new(new Guid("{b96b3caa-0728-11d3-9d7b-0000f81ef32e}"));

    private static readonly ImageFormat? bmp = new(new Guid("{b96b3cab-0728-11d3-9d7b-0000f81ef32e}"));

    private static readonly ImageFormat? emf = new(new Guid("{b96b3cac-0728-11d3-9d7b-0000f81ef32e}"));

    private static readonly ImageFormat? wmf = new(new Guid("{b96b3cad-0728-11d3-9d7b-0000f81ef32e}"));

    private static readonly ImageFormat? jpeg = new(new Guid("{b96b3cae-0728-11d3-9d7b-0000f81ef32e}"));

    private static readonly ImageFormat? png = new(new Guid("{b96b3caf-0728-11d3-9d7b-0000f81ef32e}"));

    private static readonly ImageFormat? gif = new(new Guid("{b96b3cb0-0728-11d3-9d7b-0000f81ef32e}"));

    private static readonly ImageFormat? tiff = new(new Guid("{b96b3cb1-0728-11d3-9d7b-0000f81ef32e}"));

    private static readonly ImageFormat? exif = new(new Guid("{b96b3cb2-0728-11d3-9d7b-0000f81ef32e}"));

    private static readonly ImageFormat? icon = new(new Guid("{b96b3cb5-0728-11d3-9d7b-0000f81ef32e}"));

    private static readonly ImageFormat? heif = new(new Guid("{b96b3cb6-0728-11d3-9d7b-0000f81ef32e}"));

    private static readonly ImageFormat? webp = new(new Guid("{b96b3cb7-0728-11d3-9d7b-0000f81ef32e}"));

    private Guid _guid;

    /// <summary>Gets a <see cref="T:System.Guid" /> structure that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</summary>
    /// <returns>A <see cref="T:System.Guid" /> structure that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</returns>
    public Guid Guid => _guid;

    /// <summary>Gets the format of a bitmap in memory.</summary>
    /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the format of a bitmap in memory.</returns>
    public static ImageFormat? MemoryBmp => memoryBmp;

    /// <summary>Gets the bitmap (BMP) image format.</summary>
    /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the bitmap image format.</returns>
    public static ImageFormat? Bmp => bmp;

    /// <summary>Gets the enhanced metafile (EMF) image format.</summary>
    /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the enhanced metafile image format.</returns>
    public static ImageFormat? Emf => emf;

    /// <summary>Gets the Windows metafile (WMF) image format.</summary>
    /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the Windows metafile image format.</returns>
    public static ImageFormat? Wmf => wmf;

    /// <summary>Gets the Graphics Interchange Format (GIF) image format.</summary>
    /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the GIF image format.</returns>
    public static ImageFormat? Gif => gif;

    /// <summary>Gets the Joint Photographic Experts Group (JPEG) image format.</summary>
    /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the JPEG image format.</returns>
    public static ImageFormat? Jpeg => jpeg;

    /// <summary>Gets the W3C Portable Network Graphics (PNG) image format.</summary>
    /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the PNG image format.</returns>
    public static ImageFormat? Png => png;

    /// <summary>Gets the Tagged Image File Format (TIFF) image format.</summary>
    /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the TIFF image format.</returns>
    public static ImageFormat? Tiff => tiff;

    /// <summary>Gets the Exchangeable Image File (Exif) format.</summary>
    /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the Exif format.</returns>
    public static ImageFormat? Exif => exif;

    /// <summary>Gets the Windows icon image format.</summary>
    /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the Windows icon image format.</returns>
    public static ImageFormat? Icon => icon;

    //[SupportedOSPlatform("windows10.0.17763.0")]
    public static ImageFormat? Heif => heif;

    //[SupportedOSPlatform("windows10.0.17763.0")]
    public static ImageFormat? Webp => webp;

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.ImageFormat" /> class by using the specified <see cref="T:System.Guid" /> structure.</summary>
    /// <param name="guid">The <see cref="T:System.Guid" /> structure that specifies a particular image format.</param>
    public ImageFormat(Guid guid)
    {
        _guid = guid;
    }

    /// <summary>Returns a value that indicates whether the specified object is an <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that is equivalent to this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</summary>
    /// <param name="o">The object to test.</param>
    /// <returns>
    ///   <see langword="true" /> if <paramref name="o" /> is an <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that is equivalent to this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? o)
    {
        var imageFormat = o as ImageFormat;
        if (imageFormat == null)
        {
            return false;
        }
        return _guid == imageFormat._guid;
    }

    /// <summary>Returns a hash code value that represents this object.</summary>
    /// <returns>A hash code that represents this object.</returns>
    public override int GetHashCode()
    {
        return _guid.GetHashCode();
    }

    internal ImageCodecInfo FindEncoder()
    {
        return new ImageCodecInfo { FormatId = _guid, MimeType = GetMimeType() };
    }
    private string GetMimeType()
    {

        if (Guid == memoryBmp.Guid)
        {
            return ".bmp";
        }
        if (Guid == bmp.Guid)
        {
            return ".bmp";
        }
        if (Guid == emf.Guid)
        {
            return ".emf";
        }
        if (Guid == wmf.Guid)
        {
            return ".wmf";
        }
        if (Guid == gif.Guid)
        {
            return ".gif";
        }
        if (Guid == jpeg.Guid)
        {
            return ".jpeg";
        }
        if (Guid == png.Guid)
        {
            return ".png";
        }
        if (Guid == tiff.Guid)
        {
            return ".tiff";
        }
        if (Guid == exif.Guid)
        {
            return ".exif";
        }
        if (Guid == icon.Guid)
        {
            return ".icon";
        }
        if (Guid == heif.Guid)
        {
            return ".heif";
        }
        if (Guid == webp.Guid)
        {
            return ".webp";
        }
        return ".bmp";
    }
    /// <summary>Converts this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object to a human-readable string.</summary>
    /// <returns>A string that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</returns>
    public override string ToString()
    {
        if (Guid == memoryBmp.Guid)
        {
            return "MemoryBMP";
        }
        if (Guid == bmp.Guid)
        {
            return "Bmp";
        }
        if (Guid == emf.Guid)
        {
            return "Emf";
        }
        if (Guid == wmf.Guid)
        {
            return "Wmf";
        }
        if (Guid == gif.Guid)
        {
            return "Gif";
        }
        if (Guid == jpeg.Guid)
        {
            return "Jpeg";
        }
        if (Guid == png.Guid)
        {
            return "Png";
        }
        if (Guid == tiff.Guid)
        {
            return "Tiff";
        }
        if (Guid == exif.Guid)
        {
            return "Exif";
        }
        if (Guid == icon.Guid)
        {
            return "Icon";
        }
        if (Guid == heif.Guid)
        {
            return "Heif";
        }
        if (Guid == webp.Guid)
        {
            return "Webp";
        }
        //DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
        //defaultInterpolatedStringHandler.AppendLiteral("[ImageFormat: ");
        //defaultInterpolatedStringHandler.AppendFormatted(_guid);
        //defaultInterpolatedStringHandler.AppendLiteral("]");
        //return defaultInterpolatedStringHandler.ToStringAndClear();
        return $"[ImageFormat: {_guid}]";

    }
}