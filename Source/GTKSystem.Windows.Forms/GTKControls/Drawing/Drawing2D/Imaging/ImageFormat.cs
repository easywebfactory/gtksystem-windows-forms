using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System.Drawing.Imaging
{
	/// <summary>Specifies the file format of the image. Not inheritable.</summary>
	[TypeConverter(typeof(ImageFormatConverter))]
	public sealed class ImageFormat
	{
		private static readonly ImageFormat s_memoryBMP = new ImageFormat(new Guid("{b96b3caa-0728-11d3-9d7b-0000f81ef32e}"));

		private static readonly ImageFormat s_bmp = new ImageFormat(new Guid("{b96b3cab-0728-11d3-9d7b-0000f81ef32e}"));

		private static readonly ImageFormat s_emf = new ImageFormat(new Guid("{b96b3cac-0728-11d3-9d7b-0000f81ef32e}"));

		private static readonly ImageFormat s_wmf = new ImageFormat(new Guid("{b96b3cad-0728-11d3-9d7b-0000f81ef32e}"));

		private static readonly ImageFormat s_jpeg = new ImageFormat(new Guid("{b96b3cae-0728-11d3-9d7b-0000f81ef32e}"));

		private static readonly ImageFormat s_png = new ImageFormat(new Guid("{b96b3caf-0728-11d3-9d7b-0000f81ef32e}"));

		private static readonly ImageFormat s_gif = new ImageFormat(new Guid("{b96b3cb0-0728-11d3-9d7b-0000f81ef32e}"));

		private static readonly ImageFormat s_tiff = new ImageFormat(new Guid("{b96b3cb1-0728-11d3-9d7b-0000f81ef32e}"));

		private static readonly ImageFormat s_exif = new ImageFormat(new Guid("{b96b3cb2-0728-11d3-9d7b-0000f81ef32e}"));

		private static readonly ImageFormat s_icon = new ImageFormat(new Guid("{b96b3cb5-0728-11d3-9d7b-0000f81ef32e}"));

		private static readonly ImageFormat s_heif = new ImageFormat(new Guid("{b96b3cb6-0728-11d3-9d7b-0000f81ef32e}"));

		private static readonly ImageFormat s_webp = new ImageFormat(new Guid("{b96b3cb7-0728-11d3-9d7b-0000f81ef32e}"));

		private Guid _guid;

		/// <summary>Gets a <see cref="T:System.Guid" /> structure that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</summary>
		/// <returns>A <see cref="T:System.Guid" /> structure that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</returns>
		public Guid Guid => _guid;

		/// <summary>Gets the format of a bitmap in memory.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the format of a bitmap in memory.</returns>
		public static ImageFormat MemoryBmp => s_memoryBMP;

		/// <summary>Gets the bitmap (BMP) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the bitmap image format.</returns>
		public static ImageFormat Bmp => s_bmp;

		/// <summary>Gets the enhanced metafile (EMF) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the enhanced metafile image format.</returns>
		public static ImageFormat Emf => s_emf;

		/// <summary>Gets the Windows metafile (WMF) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the Windows metafile image format.</returns>
		public static ImageFormat Wmf => s_wmf;

		/// <summary>Gets the Graphics Interchange Format (GIF) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the GIF image format.</returns>
		public static ImageFormat Gif => s_gif;

		/// <summary>Gets the Joint Photographic Experts Group (JPEG) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the JPEG image format.</returns>
		public static ImageFormat Jpeg => s_jpeg;

		/// <summary>Gets the W3C Portable Network Graphics (PNG) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the PNG image format.</returns>
		public static ImageFormat Png => s_png;

		/// <summary>Gets the Tagged Image File Format (TIFF) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the TIFF image format.</returns>
		public static ImageFormat Tiff => s_tiff;

		/// <summary>Gets the Exchangeable Image File (Exif) format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the Exif format.</returns>
		public static ImageFormat Exif => s_exif;

		/// <summary>Gets the Windows icon image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the Windows icon image format.</returns>
		public static ImageFormat Icon => s_icon;

		//[SupportedOSPlatform("windows10.0.17763.0")]
		public static ImageFormat Heif => s_heif;

		//[SupportedOSPlatform("windows10.0.17763.0")]
		public static ImageFormat Webp => s_webp;

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
		public override bool Equals([NotNullWhen(true)] object o)
		{
			ImageFormat imageFormat = o as ImageFormat;
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
            return new ImageCodecInfo() { FormatID = this._guid, MimeType = GetMimeType() };
        }
		private string GetMimeType()
		{

            if (Guid == s_memoryBMP.Guid)
            {
                return ".bmp";
            }
            if (Guid == s_bmp.Guid)
            {
                return ".bmp";
            }
            if (Guid == s_emf.Guid)
            {
                return ".emf";
            }
            if (Guid == s_wmf.Guid)
            {
                return ".wmf";
            }
            if (Guid == s_gif.Guid)
            {
                return ".gif";
            }
            if (Guid == s_jpeg.Guid)
            {
                return ".jpeg";
            }
            if (Guid == s_png.Guid)
            {
                return ".png";
            }
            if (Guid == s_tiff.Guid)
            {
                return ".tiff";
            }
            if (Guid == s_exif.Guid)
            {
                return ".exif";
            }
            if (Guid == s_icon.Guid)
            {
                return ".icon";
            }
            if (Guid == s_heif.Guid)
            {
                return ".heif";
            }
            if (Guid == s_webp.Guid)
            {
                return ".webp";
            }
			return ".bmp";
        }
		/// <summary>Converts this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object to a human-readable string.</summary>
		/// <returns>A string that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</returns>
		public override string ToString()
		{
			if (Guid == s_memoryBMP.Guid)
			{
				return "MemoryBMP";
			}
			if (Guid == s_bmp.Guid)
			{
				return "Bmp";
			}
			if (Guid == s_emf.Guid)
			{
				return "Emf";
			}
			if (Guid == s_wmf.Guid)
			{
				return "Wmf";
			}
			if (Guid == s_gif.Guid)
			{
				return "Gif";
			}
			if (Guid == s_jpeg.Guid)
			{
				return "Jpeg";
			}
			if (Guid == s_png.Guid)
			{
				return "Png";
			}
			if (Guid == s_tiff.Guid)
			{
				return "Tiff";
			}
			if (Guid == s_exif.Guid)
			{
				return "Exif";
			}
			if (Guid == s_icon.Guid)
			{
				return "Icon";
			}
			if (Guid == s_heif.Guid)
			{
				return "Heif";
			}
			if (Guid == s_webp.Guid)
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
}
