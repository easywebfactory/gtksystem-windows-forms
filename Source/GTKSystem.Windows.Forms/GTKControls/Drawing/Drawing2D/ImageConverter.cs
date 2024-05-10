using System.Buffers.Binary;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace System.Drawing
{
	public class ImageConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			//if (!(sourceType == typeof(byte[])))
			//{
			//	return sourceType == typeof(Icon);
			//}
			return true;
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, [NotNullWhen(true)] Type destinationType)
		{
			if (!(destinationType == typeof(byte[])))
			{
				return destinationType == typeof(string);
			}
			return true;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			//Icon icon = value as Icon;
			//if (icon != null)
			//{
			//	return icon.ToBitmap();
			//}
			byte[] array = value as byte[];
			if (array != null)
			{
				Stream stream = GetBitmapStream(array) ?? new MemoryStream(array);
				return Image.FromStream(stream);
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				if (value == null)
				{
					return "none";
				}
				if (value is Image)
				{
					return value!.ToString();
				}
			}
			else if (destinationType == typeof(byte[]))
			{
				if (value == null)
				{
					return Array.Empty<byte>();
				}
				Image image = value as Image;
				if (image != null)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						ImageFormat imageFormat = image.RawFormat;
						if (imageFormat == ImageFormat.Jpeg)
						{
							imageFormat = ImageFormat.Png;
						}
						ImageCodecInfo encoder = FindEncoder(imageFormat) ?? FindEncoder(ImageFormat.Png);
						image.Save(memoryStream, encoder, null);
						return memoryStream.ToArray();
					}
				}
			}
			throw GetConvertFromException(value);
		}

		private static ImageCodecInfo FindEncoder(ImageFormat imageformat)
		{
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo[] array = imageEncoders;
			foreach (ImageCodecInfo imageCodecInfo in array)
			{
				if (imageCodecInfo.FormatID.Equals(imageformat.Guid))
				{
					return imageCodecInfo;
				}
			}
			return null;
		}

		//[RequiresUnreferencedCode("The Type of value cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(typeof(Image), attributes);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		private static Stream GetBitmapStream(ReadOnlySpan<byte> rawData)
		{
			try
			{
				short num = BinaryPrimitives.ReadInt16LittleEndian(rawData);
				if (num != 7189)
				{
					return null;
				}
				short num2 = BinaryPrimitives.ReadInt16LittleEndian(rawData.Slice(2, 2));
				if (rawData.Length <= num2 + 18 || !rawData.Slice(num2 + 12, 6).SequenceEqual(new byte[6] { 80, 66, 114, 117, 115, 104 }))
				{
					return null;
				}
				return new MemoryStream(rawData.Slice(78).ToArray());
			}
			catch (OutOfMemoryException)
			{
			}
			catch (ArgumentOutOfRangeException)
			{
			}
			return null;
		}
	}
}
