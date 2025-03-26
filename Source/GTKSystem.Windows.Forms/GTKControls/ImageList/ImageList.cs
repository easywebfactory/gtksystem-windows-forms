// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace System.Windows.Forms;

/// <summary>
///  The ImageList is an object that stores a collection of Images, most
///  commonly used by other controls, such as the ListView, TreeView, or
///  Toolbar. You can add either bitmaps or Icons to the ImageList, and the
///  other controls will be able to use the Images as they desire.
/// </summary>
[ToolboxItemFilter("System")]
[DefaultProperty(nameof(Images))]
[TypeConverter(typeof(ImageListConverter))]
public sealed partial class ImageList : Component//, IHandle<HIMAGELIST>
{
    private static readonly Color fakeTransparencyColor = Color.FromArgb(0x0d, 0x0b, 0x0c);
    private static readonly Size defaultImageSize = new(16, 16);

#pragma warning disable CS0169 // Field is never used
    private static int maxImageWidth;
    private static int maxImageHeight;
    private static bool isScalingInitialized;
#pragma warning restore CS0169 // Field is never used

    private ImageList.NativeImageList? _nativeImageList;

    private ColorDepth _colorDepth = ColorDepth.Depth32Bit;
    private Size _imageSize = defaultImageSize;

    private ImageList.ImageCollection? _imageCollection;

    // The usual handle virtualization problem, with a new twist: image
    // lists are lossy. At runtime, we delay handle creation as long as possible, and store
    // away the original images until handle creation (and hope no one disposes of the images!). At design time, we keep the originals around indefinitely.
    // This variable will become null when the original images are lost.
    private List<Original>? _originals = [];
    private EventHandler? _recreateHandler;
    private EventHandler? _changeHandler;

    /// <summary>
    ///  Creates a new ImageList Control with a default image size of 16x16 pixels
    /// </summary>
    public ImageList()
    {

    }

    /// <summary>
    ///  Creates a new ImageList Control with a default image size of 16x16
    ///  pixels and adds the ImageList to the passed in container.
    /// </summary>
    public ImageList(IContainer container) : this()
    {
        //ArgumentNullException.ThrowIfNull(container);

        container.Add(this);
    }

    public ColorDepth ColorDepth
    {
        get => _colorDepth;
        set
        {
            if (_colorDepth == value)
            {
                return;
            }

            _colorDepth = value;
        }
    }

    private bool ShouldSerializeColorDepth() => Images.Count == 0;

    private void ResetColorDepth() => ColorDepth = ColorDepth.Depth32Bit;

    public IntPtr Handle => IntPtr.Zero;

    public bool HandleCreated => !(_nativeImageList is null);

    public System.Windows.Forms.ImageList.ImageCollection Images => _imageCollection ??= new ImageList.ImageCollection(this);

    public Size ImageSize
    {
        get => _imageSize;
        set
        {

            if (_imageSize.Width != value.Width || _imageSize.Height != value.Height)
            {
                _imageSize = new Size(value.Width, value.Height);
            }
        }
    }

    private bool ShouldSerializeImageSize() => Images.Count == 0;

    private ImageListStreamer? imageStream;
    public ImageListStreamer? ImageStream
    {
        get => imageStream;
        set
        {
            imageStream = value;
            if (value is null)
            {
                Images.Clear();
            }
            else
            {
                _originals?.Clear();
                if (imageStream is { Images: not null })
                {
                    foreach (var image in imageStream.Images)
                    {
                        _originals?.Add(new Original(image, OriginalOptions.Default));
                    }
                }
            }
        }
    }

    internal Image? GetOriginalImage(string name)
    {
        // string direc = Path.GetDirectoryName(Application.ExecutablePath);
        var path1 = $"./Resources";
        var value = ImageStream;
        if (value.ResourceInfo is { BaseName: not null })
        {
            // Load image data here
            var dir = $"{path1}/{Path.GetExtension(value.ResourceInfo.BaseName).TrimStart('.')}";
            var path2 = $"{dir}/{name}";
            if (File.Exists(path2))
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return ScaleSimpleBitmap(Image.FromFile(path2));
            }
        }
        if (File.Exists($"{path1}/{name}"))
        {
            return ScaleSimpleBitmap(Image.FromFile($"{path1}/{name}"));
        }
        return null;
    }
    private Bitmap ScaleSimpleBitmap(Image bitmp)
    {
        var pixbuf = new Gdk.Pixbuf(bitmp.PixbufData);
        var w = Math.Max(16, Math.Min(ImageSize.Width, 200));
        var h = Math.Max(16, Math.Min(ImageSize.Height, 200));
        var newpixbuf = pixbuf.ScaleSimple(w, h, Gdk.InterpType.Bilinear);
        return new Bitmap(w, h) { Pixbuf = newpixbuf };
    }
#if DEBUG

    internal bool IsDisposed { get; private set; }
#endif

    public object? Tag { get; set; }

    public Color TransparentColor { get; set; } = Color.Transparent;

    private bool UseTransparentColor => TransparentColor.A > 0;

    public event EventHandler? RecreateHandle
    {
        add => _recreateHandler += value;
        remove => _recreateHandler -= value;
    }

    internal event EventHandler? ChangeHandle
    {
        add => _changeHandler += value;
        remove => _changeHandler -= value;
    }

    private Bitmap CreateBitmap(Original original, out bool ownsBitmap)
    {
        ownsBitmap = false;
        var bitmp = original._image as Bitmap;
        var pixbuf = new Gdk.Pixbuf(bitmp?.PixbufData);
        var w = Math.Max(16, Math.Min(ImageSize.Width, 200));
        var h = Math.Max(16, Math.Min(ImageSize.Height, 200));
        var newpixbuf = pixbuf.ScaleSimple(w, h, Gdk.InterpType.Bilinear);
        ownsBitmap = true;
        return new Bitmap(w, h) { Pixbuf = newpixbuf };
    }

    private int AddIconToHandle(Original original, Icon icon)
    {
        return 0;
    }

    private int AddToHandle(Bitmap bitmap)
    {
        return 0;
    }

    private void CreateHandle()
    {

    }

    // Don't merge this function into Dispose() -- that base.Dispose() will damage the design time experience
    private void DestroyHandle()
    {
        if (HandleCreated)
        {
            _nativeImageList?.Dispose();
            _nativeImageList = null;
            _originals = [];
        }
    }

    /// <summary>
    ///  Releases the unmanaged resources used by the <see cref="ImageList" />
    ///  and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///  <see langword="true" /> to release both managed and unmanaged resources;
    ///  <see langword="false" /> to release only unmanaged resources.
    /// </param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (!(_originals is null))
            {
                // we might own some of the stuff that's not been created yet
                foreach (var original in _originals)
                {
                    if ((original._options & OriginalOptions.OwnsImage) != 0)
                    {
                        (original._image as IDisposable)?.Dispose();
                    }
                }
                _originals.Clear();
                _imageCollection?.Clear();
            }

            DestroyHandle();
        }

#if DEBUG
        // At this stage we've released all resources, and the component is essentially disposed
        IsDisposed = true;
#endif

        base.Dispose(disposing);
    }

    /// <summary>
    ///  Draw the image indicated by the given index on the given Graphics
    ///  at the given location.
    /// </summary>
    public void Draw(Graphics g, Point pt, int index) => Draw(g, pt.X, pt.Y, index);

    /// <summary>
    ///  Draw the image indicated by the given index on the given Graphics
    ///  at the given location.
    /// </summary>
    public void Draw(Graphics g, int x, int y, int index) => Draw(g, x, y, _imageSize.Width, _imageSize.Height, index);

    /// <summary>
    ///  Draw the image indicated by the given index using the location, size
    ///  and raster op code specified. The image is stretched or compressed as
    ///  necessary to fit the bounds provided.
    /// </summary>
    public void Draw(Graphics g, int x, int y, int width, int height, int index)
    {
        //ArgumentOutOfRangeException.ThrowIfNegative(index);
        //ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Images.Count);

        //HDC dc = (HDC)g.GetHdc();
        //try
        //{
        //    PInvoke.ImageList.DrawEx(
        //        this,
        //        index,
        //        new HandleRef<HDC>(g, dc),
        //        x,
        //        y,
        //        width,
        //        height,
        //        (COLORREF)PInvoke.CLR_NONE,
        //        (COLORREF)PInvoke.CLR_NONE,
        //        IMAGE_LIST_DRAW_STYLE.ILD_TRANSPARENT);
        //}
        //finally
        //{
        //    g.ReleaseHdcInternal(dc);
        //}
    }

    private static unsafe void CopyBitmapData(BitmapData sourceData, BitmapData targetData)
    {
        Debug.Assert(Image.GetPixelFormatSize(sourceData.PixelFormat) == 32);
        Debug.Assert(Image.GetPixelFormatSize(sourceData.PixelFormat) == Image.GetPixelFormatSize(targetData.PixelFormat));
        Debug.Assert(targetData.Width == sourceData.Width);
        Debug.Assert(targetData.Height == sourceData.Height);
        Debug.Assert(targetData.Stride == targetData.Width * 4);

        // do the actual copy
        var offsetSrc = 0;
        var offsetDest = 0;
        for (var i = 0; i < targetData.Height; i++)
        {
            var srcPtr = sourceData.Scan0 + offsetSrc;
            var destPtr = targetData.Scan0 + offsetDest;
            var length = Math.Abs(targetData.Stride);
            Buffer.MemoryCopy(srcPtr.ToPointer(), destPtr.ToPointer(), length, length);
            offsetSrc += sourceData.Stride;
            offsetDest += targetData.Stride;
        }
    }

    private static unsafe bool BitmapHasAlpha(BitmapData bmpData)
    {
        if (bmpData.PixelFormat != PixelFormat.Format32BppArgb && bmpData.PixelFormat != PixelFormat.Format32BppRgb)
        {
            return false;
        }

        for (var i = 0; i < bmpData.Height; i++)
        {
            var offsetRow = i * bmpData.Stride;
            for (var j = 3; j < bmpData.Width * 4; j += 4) // *4 is safe since we know PixelFormat is ARGB
            {
                var candidate = (byte*)bmpData.Scan0.ToPointer() + offsetRow + j;
                if (*candidate != 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    ///  Returns the image specified by the given index. The bitmap returned is a
    ///  copy of the original image.
    /// </summary>
    // NOTE: forces handle creation, so doesn't return things from the original list

    public Bitmap? GetBitmap(int index)
    {
        try
        {
            return _originals?[index]._image as Bitmap;
        }
        catch (IndexOutOfRangeException ex)
        {
            throw new IndexOutOfRangeException("The index is out of range. Please check whether the serial number is within the data range of ImageList and save the relevant images to the Resources directory.", ex);
        }
    }

    public Bitmap GetBitmap(string? name)
    {
        var index = _imageCollection.IndexOfKey(name);
        if (index == -1)
        {
            throw new FileNotFoundException($"\"{name}\" is not loaded. Please save the relevant pictures to the Resources directory.", name);
        }
        return GetBitmap(index);
    }
    /// <summary>
    ///  Called when the Handle property changes.
    /// </summary>
    private void OnRecreateHandle(EventArgs e) => _recreateHandler?.Invoke(this, e);

    private void OnChangeHandle(EventArgs e) => _changeHandler?.Invoke(this, e);

    /// <summary>
    ///  Returns a string representation for this control.
    /// </summary>
    public override string ToString()
        => Images is null
            ? base.ToString()
            : $"{base.ToString()} Images.Count: {Images.Count}, ImageSize: {ImageSize}";
}