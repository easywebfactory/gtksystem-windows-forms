using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Drawing
{
    [Serializable]
	public abstract class Image : Gtk.Widget, IDisposable, ICloneable, ISerializable//,MarshalByRefObject
    {
        #region 只取图像byte[]数据 
        internal Image(byte[] pixbuf)
		{
            PixbufData = pixbuf;
        }
        private byte[] _PixbufData;
        //“jpeg”, “tiff”, “png”, “ico” or “bmp”.
        public byte[] PixbufData
        {
            get { if (_PixbufData == null && _Pixbuf != null) { _PixbufData = _Pixbuf.SaveToBuffer("bmp"); } return _PixbufData; }
            set { _PixbufData = value; _Pixbuf = new Gdk.Pixbuf(value); }
        }
        private Gdk.Pixbuf _Pixbuf;
        public Gdk.Pixbuf Pixbuf
        {
            get {
                if (_Pixbuf == null && _PixbufData != null) { _Pixbuf = new Gdk.Pixbuf(_PixbufData); }
                return _Pixbuf;
            }
            set { _Pixbuf = value; _PixbufData = value.SaveToBuffer("bmp"); }
        }
		private string _fileName;
        public string FileName { get=> _fileName; set { _fileName = value; Pixbuf = new Gdk.Pixbuf(value); } }
        #endregion

        /// <summary>Provides a callback method for determining when the <see cref="M:System.Drawing.Image.GetThumbnailImage(System.Int32,System.Int32,System.Drawing.Image.GetThumbnailImageAbort,System.IntPtr)" /> method should prematurely cancel execution.</summary>
        /// <returns>This method returns <see langword="true" /> if it decides that the <see cref="M:System.Drawing.Image.GetThumbnailImage(System.Int32,System.Int32,System.Drawing.Image.GetThumbnailImageAbort,System.IntPtr)" /> method should prematurely stop execution; otherwise, it returns <see langword="false" />.</returns>
        public delegate bool GetThumbnailImageAbort();

		internal IntPtr nativeImage;

		private object _userData;

		private byte[] _rawData;

		/// <summary>Gets or sets an object that provides additional data about the image.</summary>
		/// <returns>The <see cref="T:System.Object" /> that provides additional data about the image.</returns>
		[Localizable(false)]
		[DefaultValue(null)]
		public object Tag
		{
			get
			{
				return _userData;
			}
			set
			{
				_userData = value;
			}
		}

		/// <summary>Gets the width and height of this image.</summary>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> structure that represents the width and height of this <see cref="T:System.Drawing.Image" />.</returns>
		public SizeF PhysicalDimension
		{
			get
			{
				float width=10;
				float height = 10;
                return new SizeF(width, height);
			}
		}

		/// <summary>Gets the width and height, in pixels, of this image.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> structure that represents the width and height, in pixels, of this image.</returns>
		public Size Size => new Size(Width, Height);
		private int _width;
		public int Width
		{
            get => Pixbuf == null ? _width : Pixbuf.Width;
            internal set { _width = value; }
        }
		private int _height;
		public int Height
        {
            get => Pixbuf == null ? _height : Pixbuf.Height;
            internal set { _height = value; }
        }

        /// <summary>Gets the horizontal resolution, in pixels per inch, of this <see cref="T:System.Drawing.Image" />.</summary>
        /// <returns>The horizontal resolution, in pixels per inch, of this <see cref="T:System.Drawing.Image" />.</returns>
        public float HorizontalResolution
		{
			get
			{
				float horzRes = 10;
                return horzRes;
			}
		}

		/// <summary>Gets the vertical resolution, in pixels per inch, of this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>The vertical resolution, in pixels per inch, of this <see cref="T:System.Drawing.Image" />.</returns>
		public float VerticalResolution
		{
			get
			{
				float vertRes = 10;
                return vertRes;
			}
		}

		/// <summary>Gets attribute flags for the pixel data of this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>The integer representing a bitwise combination of <see cref="T:System.Drawing.Imaging.ImageFlags" /> for this <see cref="T:System.Drawing.Image" />.</returns>
		[Browsable(false)]
		public int Flags
		{
			get
			{
				int flags = 10;
                return flags;
			}
		}
		private ImageFormat _rawFormat;
        /// <summary>Gets the file format of this <see cref="T:System.Drawing.Image" />.</summary>
        /// <returns>The <see cref="T:System.Drawing.Imaging.ImageFormat" /> that represents the file format of this <see cref="T:System.Drawing.Image" />.</returns>
        public ImageFormat RawFormat
        {
            get => _rawFormat ?? ImageFormat.Bmp;
            internal set => _rawFormat = value;
        }

        /// <summary>Gets the pixel format for this <see cref="T:System.Drawing.Image" />.</summary>
        /// <returns>A <see cref="T:System.Drawing.Imaging.PixelFormat" /> that represents the pixel format for this <see cref="T:System.Drawing.Image" />.</returns>
        public PixelFormat PixelFormat
		{
			get;
			internal set;
		}

		/// <summary>Gets IDs of the property items stored in this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>An array of the property IDs, one for each property item stored in this image.</returns>
		[Browsable(false)]
		public int[] PropertyIdList
		{
            get;
            internal set;
        }

		/// <summary>Gets all the property items (pieces of metadata) stored in this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>An array of <see cref="T:System.Drawing.Imaging.PropertyItem" /> objects, one for each property item stored in the image.</returns>
		[Browsable(false)]
		public PropertyItem[] PropertyItems
		{
            get;
            internal set;
        }

		/// <summary>Gets or sets the color palette used for this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Imaging.ColorPalette" /> that represents the color palette used for this <see cref="T:System.Drawing.Image" />.</returns>
		[Browsable(false)]
		public ColorPalette Palette
		{
            get;
            internal set;
        }

		/// <summary>Gets an array of GUIDs that represent the dimensions of frames within this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>An array of GUIDs that specify the dimensions of frames within this <see cref="T:System.Drawing.Image" /> from most significant to least significant.</returns>
		[Browsable(false)]
		public Guid[] FrameDimensionsList
		{
            get;
            internal set;
        }

        protected Image()
		{
		}

		protected Image(SerializationInfo info, StreamingContext context)
		{
			byte[] buffer = (byte[])info.GetValue("Data", typeof(byte[]));
			try
			{
				SetNativeImage(InitializeFromStream(new MemoryStream(buffer)));
			}
			catch (ExternalException)
			{
			}
			catch (ArgumentException)
			{
			}
			catch (OutOfMemoryException)
			{
			}
			catch (InvalidOperationException)
			{
			}
			catch (NotImplementedException)
			{
			}
			catch (FileNotFoundException)
			{
			}
		}

        /// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
        void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			using MemoryStream memoryStream = new MemoryStream();
			Save(memoryStream);
			si.AddValue("Data", memoryStream.ToArray(), typeof(byte[]));
		}

		/// <summary>Creates an <see cref="T:System.Drawing.Image" /> from the specified file.</summary>
		/// <param name="filename">A string that contains the name of the file from which to create the <see cref="T:System.Drawing.Image" />.</param>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates.</returns>
		/// <exception cref="T:System.OutOfMemoryException">The file does not have a valid image format.
		/// -or-
		/// GDI+ does not support the pixel format of the file.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified file does not exist.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="filename" /> is a <see cref="T:System.Uri" />.</exception>
		public static Image FromFile(string filename)
		{
			return FromFile(filename, useEmbeddedColorManagement: false);
		}

		/// <summary>Creates an <see cref="T:System.Drawing.Image" /> from the specified file using embedded color management information in that file.</summary>
		/// <param name="filename">A string that contains the name of the file from which to create the <see cref="T:System.Drawing.Image" />.</param>
		/// <param name="useEmbeddedColorManagement">Set to <see langword="true" /> to use color management information embedded in the image file; otherwise, <see langword="false" />.</param>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates.</returns>
		/// <exception cref="T:System.OutOfMemoryException">The file does not have a valid image format.
		/// -or-
		/// GDI+ does not support the pixel format of the file.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified file does not exist.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="filename" /> is a <see cref="T:System.Uri" />.</exception>
		public static Image FromFile(string filename, bool useEmbeddedColorManagement)
		{
			if (!System.IO.File.Exists(filename))
			{
				filename = System.IO.Path.GetFullPath(filename);
				throw new FileNotFoundException(filename);
			}
			filename = System.IO.Path.GetFullPath(filename);
            string extension = IO.Path.GetExtension(filename)?.ToLower();
            byte[] filebytes =File.ReadAllBytes(filename);
            Bitmap bitmap = new Bitmap(filebytes);
            bitmap.GetImageFormat(extension);
            return bitmap;
		}

		/// <summary>Creates an <see cref="T:System.Drawing.Image" /> from the specified data stream.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Image" />.</param>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">The stream does not have a valid image format
		/// -or-
		/// <paramref name="stream" /> is <see langword="null" />.</exception>
		public static Image FromStream(Stream stream)
		{
			return FromStream(stream, useEmbeddedColorManagement: false);
		}

		/// <summary>Creates an <see cref="T:System.Drawing.Image" /> from the specified data stream, optionally using embedded color management information in that stream.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Image" />.</param>
		/// <param name="useEmbeddedColorManagement">
		///   <see langword="true" /> to use color management information embedded in the data stream; otherwise, <see langword="false" />.</param>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">The stream does not have a valid image format
		/// -or-
		/// <paramref name="stream" /> is <see langword="null" />.</exception>
		public static Image FromStream(Stream stream, bool useEmbeddedColorManagement)
		{
			return FromStream(stream, useEmbeddedColorManagement, validateImageData: true);
		}

		/// <summary>Creates an <see cref="T:System.Drawing.Image" /> from the specified data stream, optionally using embedded color management information and validating the image data.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Image" />.</param>
		/// <param name="useEmbeddedColorManagement">
		///   <see langword="true" /> to use color management information embedded in the data stream; otherwise, <see langword="false" />.</param>
		/// <param name="validateImageData">
		///   <see langword="true" /> to validate the image data; otherwise, <see langword="false" />.</param>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">The stream does not have a valid image format.</exception>
		public static Image FromStream(Stream stream, bool useEmbeddedColorManagement, bool validateImageData)
		{
			if (stream != null)
			{ 
				return new Bitmap(stream);
			}
			return null;
		}

		private IntPtr InitializeFromStream(Stream stream)
		{
            Pixbuf = new Gdk.Pixbuf(stream);
            return Pixbuf.Handle;
        }

        internal Image(IntPtr nativeImage)
		{
			SetNativeImage(nativeImage);
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Image" />.</summary>
		//public void Dispose()
		//{
		//	Dispose(disposing: true);
		//	GC.SuppressFinalize(this);
		//}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		~Image()
		{
			Dispose(disposing: false);
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates, cast as an object.</returns>
		public object Clone()
		{
			return new Bitmap((byte[])this.PixbufData.Clone()) { Width = this.Width, Height = this.Height };
        }

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Drawing.Image" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		protected new virtual void Dispose(bool disposing)
		{
            if (_Pixbuf != null)
                _Pixbuf.Dispose();
            if (_PixbufData != null)
                _PixbufData = null;
			base.Dispose(disposing);
        }
		private ImageFormat GetImageFormat(string extension)
		{
            if (extension == ".memorybmp")
            {
                RawFormat = ImageFormat.MemoryBmp;
            }
            else if (extension == ".bmp")
            {
                RawFormat = ImageFormat.Bmp;
            }
            else if (extension == ".emf")
            {
                RawFormat = ImageFormat.Emf;
            }
            else if (extension == ".wmf")
            {
                RawFormat = ImageFormat.Wmf;
            }
            else if (extension == ".gif")
            {
                RawFormat = ImageFormat.Gif;
            }
            else if (extension == ".jpeg")
            {
                RawFormat = ImageFormat.Jpeg;
            }
            else if (extension == ".png")
            {
                RawFormat = ImageFormat.Png;
            }
            else if (extension == ".tiff")
            {
                RawFormat = ImageFormat.Tiff;
            }
            else if (extension == ".exif")
            {
                RawFormat = ImageFormat.Exif;
            }
            else if (extension == ".icon")
            {
                RawFormat = ImageFormat.Icon;
            }
            else if (extension == ".heif")
            {
                RawFormat = ImageFormat.Heif;
            }
            else if (extension == ".webp")
            {
                RawFormat = ImageFormat.Webp;
            }
			return RawFormat;
        }
		/// <summary>Saves this <see cref="T:System.Drawing.Image" /> to the specified file or stream.</summary>
		/// <param name="filename">A string that contains the name of the file to which to save this <see cref="T:System.Drawing.Image" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="filename" /> is <see langword="null." /></exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.
		/// -or-
		/// The image was saved to the same file it was created from.</exception>
		public void Save(string filename)
		{
			string extension = IO.Path.GetExtension(filename)?.ToLower();
			GetImageFormat(extension);
            Save(filename, RawFormat);
        }

		/// <summary>Saves this <see cref="T:System.Drawing.Image" /> to the specified file in the specified format.</summary>
		/// <param name="filename">A string that contains the name of the file to which to save this <see cref="T:System.Drawing.Image" />.</param>
		/// <param name="format">The <see cref="T:System.Drawing.Imaging.ImageFormat" /> for this <see cref="T:System.Drawing.Image" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="filename" /> or <paramref name="format" /> is <see langword="null." /></exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.
		/// -or-
		/// The image was saved to the same file it was created from.</exception>
		public void Save(string filename, ImageFormat format)
		{
			Save(filename, format.FindEncoder(), null);
		}

		/// <summary>Saves this <see cref="T:System.Drawing.Image" /> to the specified file, with the specified encoder and image-encoder parameters.</summary>
		/// <param name="filename">A string that contains the name of the file to which to save this <see cref="T:System.Drawing.Image" />.</param>
		/// <param name="encoder">The <see cref="T:System.Drawing.Imaging.ImageCodecInfo" /> for this <see cref="T:System.Drawing.Image" />.</param>
		/// <param name="encoderParams">An <see cref="T:System.Drawing.Imaging.EncoderParameters" /> to use for this <see cref="T:System.Drawing.Image" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="filename" /> or <paramref name="encoder" /> is <see langword="null." /></exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.
		/// -or-
		/// The image was saved to the same file it was created from.</exception>
		public void Save(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams)
		{
            if(Pixbuf != null)
				Pixbuf.Save(filename, encoder.MimeType.Trim('.').ToLower());
        }

		private void Save(MemoryStream stream)
		{
			Save(stream, null, null);
		}

		/// <summary>Saves this image to the specified stream in the specified format.</summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> where the image will be saved.</param>
		/// <param name="format">An <see cref="T:System.Drawing.Imaging.ImageFormat" /> that specifies the format of the saved image.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> or <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format</exception>
		public void Save(Stream stream, ImageFormat format)
		{
            Save(stream, null, null);
		}

		/// <summary>Saves this image to the specified stream, with the specified encoder and image encoder parameters.</summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> where the image will be saved.</param>
		/// <param name="encoder">The <see cref="T:System.Drawing.Imaging.ImageCodecInfo" /> for this <see cref="T:System.Drawing.Image" />.</param>
		/// <param name="encoderParams">An <see cref="T:System.Drawing.Imaging.EncoderParameters" /> that specifies parameters used by the image encoder.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.</exception>
		public  void Save(Stream stream, ImageCodecInfo encoder, EncoderParameters encoderParams)
		{
            if (PixbufData != null)
            {
                foreach (byte b in PixbufData)
                    stream.WriteByte(b);
            }
        }

		/// <summary>Adds a frame to the file or stream specified in a previous call to the <see cref="Overload:System.Drawing.Image.Save" /> method. Use this method to save selected frames from a multiple-frame image to another multiple-frame image.</summary>
		/// <param name="encoderParams">An <see cref="T:System.Drawing.Imaging.EncoderParameters" /> that holds parameters required by the image encoder that is used by the save-add operation.</param>
		public void SaveAdd(EncoderParameters encoderParams)
		{
			
		}

		/// <summary>Adds a frame to the file or stream specified in a previous call to the <see cref="Overload:System.Drawing.Image.Save" /> method.</summary>
		/// <param name="image">An <see cref="T:System.Drawing.Image" /> that contains the frame to add.</param>
		/// <param name="encoderParams">An <see cref="T:System.Drawing.Imaging.EncoderParameters" /> that holds parameters required by the image encoder that is used by the save-add operation.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="image" /> is <see langword="null" />.</exception>
		public void SaveAdd(Image image, EncoderParameters encoderParams)
		{
			
		}

		private static void ThrowIfDirectoryDoesntExist(string filename)
		{
			
		}

		/// <summary>Gets the bounds of the image in the specified unit.</summary>
		/// <param name="pageUnit">One of the <see cref="T:System.Drawing.GraphicsUnit" /> values indicating the unit of measure for the bounding rectangle.</param>
		/// <returns>The <see cref="T:System.Drawing.RectangleF" /> that represents the bounds of the image, in the specified unit.</returns>
		public RectangleF GetBounds(ref GraphicsUnit pageUnit)
		{
			return new RectangleF();
		}

		/// <summary>Returns a thumbnail for this <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="thumbWidth">The width, in pixels, of the requested thumbnail image.</param>
		/// <param name="thumbHeight">The height, in pixels, of the requested thumbnail image.</param>
		/// <param name="callback">A <see cref="T:System.Drawing.Image.GetThumbnailImageAbort" /> delegate.
		/// Note You must create a delegate and pass a reference to the delegate as the <paramref name="callback" /> parameter, but the delegate is not used.</param>
		/// <param name="callbackData">Must be <see cref="F:System.IntPtr.Zero" />.</param>
		/// <returns>An <see cref="T:System.Drawing.Image" /> that represents the thumbnail.</returns>
		public Image GetThumbnailImage(int thumbWidth, int thumbHeight, GetThumbnailImageAbort callback, IntPtr callbackData)
		{
			return null;
		}

		internal static void ValidateImage(IntPtr image)
		{

		}

		/// <summary>Returns the number of frames of the specified dimension.</summary>
		/// <param name="dimension">A <see cref="T:System.Drawing.Imaging.FrameDimension" /> that specifies the identity of the dimension type.</param>
		/// <returns>The number of frames in the specified dimension.</returns>
		public int GetFrameCount(FrameDimension dimension)
		{
			
			return 1;
		}

		/// <summary>Gets the specified property item from this <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="propid">The ID of the property item to get.</param>
		/// <returns>The <see cref="T:System.Drawing.Imaging.PropertyItem" /> this method gets.</returns>
		/// <exception cref="T:System.ArgumentException">The image format of this image does not support property items.</exception>
		public  PropertyItem GetPropertyItem(int propid)
		{

				return null;
			
		}

		/// <summary>Selects the frame specified by the dimension and index.</summary>
		/// <param name="dimension">A <see cref="T:System.Drawing.Imaging.FrameDimension" /> that specifies the identity of the dimension type.</param>
		/// <param name="frameIndex">The index of the active frame.</param>
		/// <returns>Always returns 0.</returns>
		public int SelectActiveFrame(FrameDimension dimension, int frameIndex)
		{

			return 0;
		}

		/// <summary>Stores a property item (piece of metadata) in this <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="propitem">The <see cref="T:System.Drawing.Imaging.PropertyItem" /> to be stored.</param>
		/// <exception cref="T:System.ArgumentException">The image format of this image does not support property items.</exception>
		public  void SetPropertyItem(PropertyItem propitem)
		{

		}

		/// <summary>Rotates, flips, or rotates and flips the <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="rotateFlipType">A <see cref="T:System.Drawing.RotateFlipType" /> member that specifies the type of rotation and flip to apply to the image.</param>
		public void RotateFlip(RotateFlipType rotateFlipType)
		{

		}

		/// <summary>Removes the specified property item from this <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="propid">The ID of the property item to remove.</param>
		/// <exception cref="T:System.ArgumentException">The image does not contain the requested property item.
		/// -or-
		/// The image format for this image does not support property items.</exception>
		public void RemovePropertyItem(int propid)
		{
		
		}

		/// <summary>Returns information about the parameters supported by the specified image encoder.</summary>
		/// <param name="encoder">A GUID that specifies the image encoder.</param>
		/// <returns>An <see cref="T:System.Drawing.Imaging.EncoderParameters" /> that contains an array of <see cref="T:System.Drawing.Imaging.EncoderParameter" /> objects. Each <see cref="T:System.Drawing.Imaging.EncoderParameter" /> contains information about one of the parameters supported by the specified image encoder.</returns>
		public EncoderParameters GetEncoderParameterList(Guid encoder)
		{

				return null;
	
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Bitmap" /> from a handle to a GDI bitmap.</summary>
		/// <param name="hbitmap">The GDI bitmap handle from which to create the <see cref="T:System.Drawing.Bitmap" />.</param>
		/// <returns>The <see cref="T:System.Drawing.Bitmap" /> this method creates.</returns>
		public static Bitmap FromHbitmap(IntPtr hbitmap)
		{
			return FromHbitmap(hbitmap, IntPtr.Zero);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Bitmap" /> from a handle to a GDI bitmap and a handle to a GDI palette.</summary>
		/// <param name="hbitmap">The GDI bitmap handle from which to create the <see cref="T:System.Drawing.Bitmap" />.</param>
		/// <param name="hpalette">A handle to a GDI palette used to define the bitmap colors if the bitmap specified in the <paramref name="hbitmap" /> parameter is not a device-independent bitmap (DIB).</param>
		/// <returns>The <see cref="T:System.Drawing.Bitmap" /> this method creates.</returns>
		public static Bitmap FromHbitmap(IntPtr hbitmap, IntPtr hpalette)
		{

			return new Bitmap(hbitmap);
		}

		/// <summary>Returns a value that indicates whether the pixel format is 64 bits per pixel.</summary>
		/// <param name="pixfmt">The <see cref="T:System.Drawing.Imaging.PixelFormat" /> enumeration to test.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="pixfmt" /> is extended; otherwise, <see langword="false" />.</returns>
		public static bool IsExtendedPixelFormat(PixelFormat pixfmt)
		{
			return (pixfmt & PixelFormat.Extended) != 0;
		}

		/// <summary>Returns a value that indicates whether the pixel format is 32 bits per pixel.</summary>
		/// <param name="pixfmt">The <see cref="T:System.Drawing.Imaging.PixelFormat" /> to test.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="pixfmt" /> is canonical; otherwise, <see langword="false" />.</returns>
		public static bool IsCanonicalPixelFormat(PixelFormat pixfmt)
		{
			return (pixfmt & PixelFormat.Canonical) != 0;
		}

        internal void SetNativeImage(IntPtr handle)
        {
            if (GLib.Object.TryGetObject(handle) is Gdk.Pixbuf pixbuf)
                _Pixbuf = pixbuf;
        }

        /// <summary>Returns the color depth, in number of bits per pixel, of the specified pixel format.</summary>
        /// <param name="pixfmt">The <see cref="T:System.Drawing.Imaging.PixelFormat" /> member that specifies the format for which to find the size.</param>
        /// <returns>The color depth of the specified pixel format.</returns>
        public static int GetPixelFormatSize(PixelFormat pixfmt)
		{
			return ((int)pixfmt >> 8) & 0xFF;
		}

		/// <summary>Returns a value that indicates whether the pixel format for this <see cref="T:System.Drawing.Image" /> contains alpha information.</summary>
		/// <param name="pixfmt">The <see cref="T:System.Drawing.Imaging.PixelFormat" /> to test.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="pixfmt" /> contains alpha information; otherwise, <see langword="false" />.</returns>
		public static bool IsAlphaPixelFormat(PixelFormat pixfmt)
		{
			return (pixfmt & PixelFormat.Alpha) != 0;
		}

		internal static Image CreateImageObject(IntPtr nativeImage)
		{
			return new Bitmap(nativeImage);
		}

		internal  static void EnsureSave(Image image, string filename, Stream dataStream)
		{
			 
		}

        public new void Dispose()
        {
			Dispose(true);
        }
    }


    public class GtkImageConverter : TypeConverter
	{
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
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
			
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
			return value;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(Image), attributes);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
