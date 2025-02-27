// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace System.Windows.Forms.Design;

public class ImageListImage
{
    public ImageListImage(Image? image)
    {
        Image = image;
    }

    public ImageListImage(Image? image, string name)
    {
        Image = image;
        Name = name;
    }

    public string Name { get; set; } = string.Empty;

    [Browsable(false)]
    public Image? Image { get; set; }

    // Add properties to make this object "look" like Image in the Collection editor
    public float HorizontalResolution => Image?.HorizontalResolution??0;

    public float VerticalResolution => Image?.VerticalResolution ?? 0;

    public PixelFormat PixelFormat => Image?.PixelFormat ?? 0;

    public ImageFormat? RawFormat => Image?.RawFormat;

    public Size Size => Image?.Size??default;

    public SizeF PhysicalDimension => Image?.Size??default;

    public static ImageListImage? ImageListImageFromStream(Stream? stream, bool imageIsIcon)
    {
        if (imageIsIcon)
        {
            return new ImageListImage(new Icon(stream).ToBitmap());
        }

        var fromStream = (Bitmap?)Image.FromStream(stream);
        if (fromStream != null)
        {
            return new ImageListImage(fromStream);
        }

        return null;
    }
}