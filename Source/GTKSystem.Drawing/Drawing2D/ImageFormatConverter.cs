using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Imaging;
using System.Globalization;

namespace System.Drawing;

public class ImageFormatConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        if (!(sourceType == typeof(string)))
        {
            return base.CanConvertFrom(context, sourceType);
        }
        return true;
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type destinationType)
    {
        if (destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor))
        {
            return true;
        }
        return base.CanConvertTo(context, destinationType);
    }

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object? value)
		{
			var text = value as string;
			if (text == null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			if (text[0] == '[' && text.Length >= 50 && Guid.TryParse(text.Substring(14, 36), out var result))
			{
				return new ImageFormat(result);
			}
			if (text.Equals("Bmp", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Bmp;
			}
			if (text.Equals("Emf", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Emf;
			}
			if (text.Equals("Exif", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Exif;
			}
			if (text.Equals("Gif", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Gif;
			}
			if (text.Equals("Icon", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Icon;
			}
			if (text.Equals("Jpeg", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Jpeg;
			}
			if (text.Equals("MemoryBmp", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.MemoryBmp;
			}
			if (text.Equals("Png", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Png;
			}
			if (text.Equals("Tiff", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Tiff;
			}
			if (text.Equals("Wmf", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Wmf;
			}
			if (text.Equals("Heif", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Heif;
			}
			if (text.Equals("Webp", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Webp;
			}
			throw new FormatException("ImageFormat err");
		}

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        var imageFormat = value as ImageFormat;
        if (imageFormat != null)
        {
            if (destinationType == typeof(string))
            {
                return imageFormat.ToString();
            }
            if (destinationType == typeof(InstanceDescriptor))
            {
                string? text = null;
                if (ImageFormat.Bmp != null && imageFormat.Guid.Equals(ImageFormat.Bmp.Guid))
                {
                    text = "Bmp";
                }
                else if (ImageFormat.Emf != null && imageFormat.Guid.Equals(ImageFormat.Emf.Guid))
                {
                    text = "Emf";
                }
                else if (ImageFormat.Exif != null && imageFormat.Guid.Equals(ImageFormat.Exif.Guid))
                {
                    text = "Exif";
                }
                else if (ImageFormat.Gif != null && imageFormat.Guid.Equals(ImageFormat.Gif.Guid))
                {
                    text = "Gif";
                }
                else if (ImageFormat.Icon != null && imageFormat.Guid.Equals(ImageFormat.Icon.Guid))
                {
                    text = "Icon";
                }
                else if (ImageFormat.Jpeg != null && imageFormat.Guid.Equals(ImageFormat.Jpeg.Guid))
                {
                    text = "Jpeg";
                }
                else if (ImageFormat.MemoryBmp != null && imageFormat.Guid.Equals(ImageFormat.MemoryBmp.Guid))
                {
                    text = "MemoryBmp";
                }
                else if (ImageFormat.Png != null && imageFormat.Guid.Equals(ImageFormat.Png.Guid))
                {
                    text = "Png";
                }
                else if (ImageFormat.Tiff != null && imageFormat.Guid.Equals(ImageFormat.Tiff.Guid))
                {
                    text = "Tiff";
                }
                else if (ImageFormat.Wmf != null && imageFormat.Guid.Equals(ImageFormat.Wmf.Guid))
                {
                    text = "Wmf";
                }
                if (text != null)
                {
                    return new InstanceDescriptor(typeof(ImageFormat).GetProperty(text), null);
                }
                var constructor = typeof(ImageFormat).GetConstructor([typeof(Guid)]);
                return new InstanceDescriptor(constructor, new object[] { imageFormat.Guid });
            }
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        return new StandardValuesCollection(new[]
        {
            ImageFormat.MemoryBmp,
            ImageFormat.Bmp,
            ImageFormat.Emf,
            ImageFormat.Wmf,
            ImageFormat.Gif,
            ImageFormat.Jpeg,
            ImageFormat.Png,
            ImageFormat.Tiff,
            ImageFormat.Exif,
            ImageFormat.Icon,
            ImageFormat.Heif,
            ImageFormat.Webp
        });
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
        return true;
    }
}