using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging;

/// <summary>Specifies the attributes of a bitmap image. The <see cref="T:System.Drawing.Imaging.BitmapData" /> class is used by the <see cref="Overload:System.Drawing.Bitmap.LockBits" /> and <see cref="M:System.Drawing.Bitmap.UnlockBits(System.Drawing.Imaging.BitmapData)" /> methods of the <see cref="T:System.Drawing.Bitmap" /> class. Not inheritable.</summary>
[StructLayout(LayoutKind.Sequential)]
public sealed class BitmapData
{
    private int _width;

    private int _height;

    private int _stride;

    private PixelFormat _pixelFormat;

    private IntPtr _scan0;

    private int _reserved;

    /// <summary>Gets or sets the pixel width of the <see cref="T:System.Drawing.Bitmap" /> object. This can also be thought of as the number of pixels in one scan line.</summary>
    /// <returns>The pixel width of the <see cref="T:System.Drawing.Bitmap" /> object.</returns>
    public int Width
    {
        get => _width;
        set => _width = value;
    }

    /// <summary>Gets or sets the pixel height of the <see cref="T:System.Drawing.Bitmap" /> object. Also sometimes referred to as the number of scan lines.</summary>
    /// <returns>The pixel height of the <see cref="T:System.Drawing.Bitmap" /> object.</returns>
    public int Height
    {
        get => _height;
        set => _height = value;
    }

    /// <summary>Gets or sets the stride width (also called scan width) of the <see cref="T:System.Drawing.Bitmap" /> object.</summary>
    /// <returns>The stride width, in bytes, of the <see cref="T:System.Drawing.Bitmap" /> object.</returns>
    public int Stride
    {
        get => _stride;
        set => _stride = value;
    }

    /// <summary>Gets or sets the format of the pixel information in the <see cref="T:System.Drawing.Bitmap" /> object that returned this <see cref="T:System.Drawing.Imaging.BitmapData" /> object.</summary>
    /// <returns>A <see cref="T:System.Drawing.Imaging.PixelFormat" /> that specifies the format of the pixel information in the associated <see cref="T:System.Drawing.Bitmap" /> object.</returns>
    public PixelFormat PixelFormat
    {
        get => _pixelFormat;
        set
        {
            switch (value)
            {
                default:
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(PixelFormat));
                case PixelFormat.Undefined:
                case PixelFormat.Max:
                case PixelFormat.Indexed:
                case PixelFormat.Gdi:
                case PixelFormat.Format16BppRgb555:
                case PixelFormat.Format16BppRgb565:
                case PixelFormat.Format24BppRgb:
                case PixelFormat.Format32BppRgb:
                case PixelFormat.Format1BppIndexed:
                case PixelFormat.Format4BppIndexed:
                case PixelFormat.Format8BppIndexed:
                case PixelFormat.Alpha:
                case PixelFormat.Format16BppArgb1555:
                case PixelFormat.PAlpha:
                case PixelFormat.Format32BppPArgb:
                case PixelFormat.Extended:
                case PixelFormat.Format16BppGrayScale:
                case PixelFormat.Format48BppRgb:
                case PixelFormat.Format64BppPArgb:
                case PixelFormat.Canonical:
                case PixelFormat.Format32BppArgb:
                case PixelFormat.Format64BppArgb:
                    _pixelFormat = value;
                    break;
            }
        }
    }

    /// <summary>Gets or sets the address of the first pixel data in the bitmap. This can also be thought of as the first scan line in the bitmap.</summary>
    /// <returns>The address of the first pixel data in the bitmap.</returns>
    public IntPtr Scan0
    {
        get => _scan0;
        set => _scan0 = value;
    }

    /// <summary>Reserved. Do not use.</summary>
    /// <returns>Reserved. Do not use.</returns>
    public int Reserved
    {
        get => _reserved;
        set => _reserved = value;
    }

    internal ref int GetPinnableReference()
    {
        return ref _width;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.BitmapData" /> class.</summary>
    public BitmapData()
    {
    }
}