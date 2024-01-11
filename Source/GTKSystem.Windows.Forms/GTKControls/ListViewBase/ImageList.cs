using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


namespace System.Windows.Forms
{


	public sealed class ImageList : Component
    {
        private static readonly Color s_fakeTransparencyColor = Color.FromArgb(0x0d, 0x0b, 0x0c);
        private static readonly Size s_defaultImageSize = new Size(16, 16);

        private const int MaxDimension = 256;
        private static int s_maxImageWidth = MaxDimension;
        private static int s_maxImageHeight = MaxDimension;
        private static bool s_isScalingInitialized;
        private ColorDepth _colorDepth = ColorDepth.Depth8Bit;
        private Size _imageSize = s_defaultImageSize;
        private readonly List<Original> _originals = new List<Original>();
        private ImageCollection _images;
        public ImageList()
        {
            _images = new ImageCollection(this);
        }

        public ImageList(IContainer container)
        {
            _images = new ImageCollection(this);
            ArgumentNullException.ThrowIfNull(container);
            container.Add(this);
        }

		private enum OriginalOptions
		{
			Default = 0x0,
			ImageStrip = 0x1,
			CustomTransparentColor = 0x2,
			OwnsImage = 0x4
		}


		public ImageCollection Images
		{
			get
			{
				return _images;
			}
		}

		public Size ImageSize
		{
            get;
            set;
        }

		public object Tag
		{
            get;
            set;
        }



		public Color TransparentColor
		{
            get;
            set;
		}
		public System.Windows.Forms.ColorDepth ColorDepth
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
        //public ImageListStreamer ImageStream
        //{
        //    get;
        //    set;
        //}
        public ImageListStreamer ImageStream
        {
            get
            {
                if (Images.Empty)
                {
                    return null;
                }
                return new ImageListStreamer(this);
            }
            set
            {
                if (value is null)
                {
                    Images.Clear();
                    return;
                }
                if (value.ResourceInfo != null)
                {
                    //这里加载图像数据
                    string direc = System.IO.Directory.GetCurrentDirectory();
                    string path1 = $"{direc}/Resources/{value.ResourceInfo.ResourceName}";
                    string path2 = $"{direc}/Resources/{value.ResourceInfo.ResourceName.Split(".")[0]}";
                    string path3 = $"{direc}/Resources";

                    LoadOriginalImage(path1);
                    LoadOriginalImage(path2);
                    LoadOriginalImage(path3);

                    if (_originals.Count == 0)
                    {
                        MessageBox.Show("ImageList控件的图片请放到程序目录Resources/[imagelist名]下");
                    }
                }
            }
        }
        private void LoadOriginalImage(string directory)
        {
            if (System.IO.Directory.Exists(directory))
            {
                foreach (string f in System.IO.Directory.GetFiles(directory, "*.*").Where(w=>Regex.IsMatch(w,"[\\w\\W]+\\.(jpg|png|jpeg|bmp|ico)",RegexOptions.IgnoreCase)))
                {
                    Image img = Bitmap.FromFile(f);
                    Images.Add(img);
                }
            }
        }
        private Bitmap GetBitmap(int index) {
            Bitmap bitmp= _originals[index]._image as Bitmap;
            Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(bitmp.PixbufData);
            int w = Math.Max(16, Math.Min(ImageSize.Width,200));
            int h = Math.Max(16, Math.Min(ImageSize.Height, 200));
            Gdk.Pixbuf newpixbuf = pixbuf.ScaleSimple(w, h, Gdk.InterpType.Bilinear);
            return new Bitmap(w, h) { Pixbuf= newpixbuf };
        }
        public bool UseTransparentColor { get; set; }
        protected override void Dispose(bool disposing)
		{
			 
		}

        internal class Indexer
        {
            internal const int NoneIndex = -2;

            internal const string DefaultKey = "";

            internal const int DefaultIndex = -1;

            public virtual ImageList ImageList
            {
                [CompilerGenerated]
                get
                {
                    throw null;
                }
                [CompilerGenerated]
                set
                {
                    throw null;
                }
            }

            public virtual string Key
            {
                get
                {
                    throw null;
                }
                [param: AllowNull]
                set
                {
                    throw null;
                }
            }

            public virtual int Index
            {
                get
                {
                    throw null;
                }
                set
                {
                    throw null;
                }
            }

            public virtual int ActualIndex
            {
                get
                {
                    throw null;
                }
            }

            public Indexer()
            {
                throw null;
            }
        }
        public sealed class ImageCollection : IList
        {
            private readonly ImageList _owner;
            private readonly List<ImageInfo> _imageInfoCollection = new List<ImageInfo>();
            
            internal class ImageInfo
            {
                public ImageInfo()
                {
                }
                public string Name
                {
                    get;
                    set;
                }
            }
            public StringCollection Keys
            {
                get
                {
                    StringCollection strs = new StringCollection();
                    foreach (ImageInfo item in this)
                    {
                        if (string.IsNullOrWhiteSpace(item.Name))
                            strs.Add(item.Name);
                        else
                            strs.Add(string.Empty);

                    }
                    return strs;
                }
            }
            public Image this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index), index, string.Format("InvalidArgument", nameof(index), index));
                    }

                    return _owner.GetBitmap(index);
                }
                set
                {
                    if (index < 0 || index >= Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index), index, string.Format("InvalidArgument", nameof(index), index));
                    }

                    ArgumentNullException.ThrowIfNull(value);

                    if (value is not Bitmap bitmap)
                    {
                        throw new ArgumentException("ImageListBitmap");
                    }

                    try
                    {
                        _owner._originals[index]._image = value;
                    }
                    finally
                    {

                    }
                }
            }

            public Image this[string key]
            {
                get
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        return null;
                    }
                    int index = IndexOfKey(key);
                    if (!IsValidIndex(index))
                    {
                        return null;
                    }

                    return this[index];
                }
            }
            internal ImageCollection(ImageList owner)
            {
                _owner = owner;
            }
            public bool Empty { get => Count == 0; }

            bool IList.IsFixedSize => throw new NotImplementedException();

            bool IList.IsReadOnly => throw new NotImplementedException();

            int ICollection.Count => throw new NotImplementedException();

            bool ICollection.IsSynchronized => throw new NotImplementedException();

            object ICollection.SyncRoot => throw new NotImplementedException();

            object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public void Add(string key, Drawing.Image image)
            {
                var imageInfo = new ImageInfo
                {
                    Name = key
                };
                var original = new Original(image, OriginalOptions.Default);
                Add(original, imageInfo);
            }

            public void Add(string key, Drawing.Icon icon)
            {
                var imageInfo = new ImageInfo
                {
                    Name = key
                };
                var original = new Original(icon, OriginalOptions.Default);
                Add(original, imageInfo);
            }

            public void Add(Drawing.Icon value)
            {
                ArgumentNullException.ThrowIfNull(value);

                Add(new Original(value.Clone(), OriginalOptions.OwnsImage), null);
            }
            public void Add(Drawing.Image value)
            {
                ArgumentNullException.ThrowIfNull(value);

                var original = new Original(value, OriginalOptions.Default);
                Add(original, null);
            }
            public int Add(Drawing.Image value, Drawing.Color transparentColor)
            {
                ArgumentNullException.ThrowIfNull(value);

                var original = new Original(value, OriginalOptions.CustomTransparentColor, transparentColor);
                return Add(original, null);
            }

            private int Add(Original original, ImageInfo imageInfo)
            {
                ArgumentNullException.ThrowIfNull(original);
                ArgumentNullException.ThrowIfNull(original._image, nameof(original));

                int index = -1;
                if (original._image is Bitmap)
                {
                    if (_owner._originals is not null)
                    {
                        _owner._originals.Add(original);
                        index = _owner._originals.Count - 1;
                    }
                }
                else if (original._image is Icon originalIcon)
                {
                    if (_owner._originals is not null)
                    {
                        _owner._originals.Add(original);
                        index = _owner._originals.Count - 1;
                    }

                }
                else
                {
                    throw new ArgumentException("ImageListBitmap");
                }
                if ((original._options & OriginalOptions.ImageStrip) != 0)
                {
                    for (int i = 0; i < original._nImages; i++)
                    {
                        _imageInfoCollection.Add(new ImageInfo());
                    }
                }
                else
                {
                    imageInfo ??= new ImageInfo();
                    _imageInfoCollection.Add(imageInfo);
                }
                //_owner.OnChangeHandle(EventArgs.Empty);

                return index;
            }
            public void AddRange(Image[] images)
            {
                foreach (Image image in images)
                {
                    Add(image);
                }
               // _owner.OnChangeHandle(EventArgs.Empty);
            }

            public int AddStrip(Image value)
            {
                if (value.Width == 0 || (value.Width % _owner.ImageSize.Width) != 0)
                {
                    throw new ArgumentException("ImageListStripBadWidth", nameof(value));
                }

                if (value.Height != _owner.ImageSize.Height)
                {
                    throw new ArgumentException("ImageListImageTooShort", nameof(value));
                }

                int nImages = value.Width / _owner.ImageSize.Width;

                var original = new Original(value, OriginalOptions.ImageStrip, nImages);

                return Add(original, null);
            }

            public bool ContainsKey(string key)
            {
                return IsValidIndex(IndexOfKey(key));
            }

            public int IndexOfKey(string key)
            {
                for (int i = 0; i < Count; i++)
                {
                    if ((_imageInfoCollection[i] is not null) && _imageInfoCollection[i].Name == key)
                    {
                        return i;
                    }
                }
                return -1;
            }
            public void RemoveAt(int index)
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, string.Format("InvalidArgument", nameof(index), index));
                }
                _owner._originals.RemoveAt(index);

                if ((_imageInfoCollection is not null) && (index >= 0 && index < _imageInfoCollection.Count))
                {
                    _imageInfoCollection.RemoveAt(index);

                   // _owner.OnChangeHandle(EventArgs.Empty);
                }
            }
            public void RemoveByKey(string key)
            {
                int index = IndexOfKey(key);
                if (IsValidIndex(index))
                {
                    RemoveAt(index);
                }
            }

            //private readonly string _callStack = new StackTrace().ToString();
            
            public void SetKeyName(int index, string name)
            {
                if (!IsValidIndex(index))
                {
                    throw new IndexOutOfRangeException();
                }

                if (_imageInfoCollection[index] is null)
                {
                    _imageInfoCollection[index] = new ImageInfo();
                }

                _imageInfoCollection[index].Name = name;
            }
            public int Count
            {
                get
                {
                    int count = 0;
                    foreach (Original original in _owner._originals)
                    {
                        if (original is not null)
                        {
                            count += original._nImages;
                        }
                    }
                    return count;
                }
            }
            private bool IsValidIndex(int index) => index >= 0 && index < Count;

            int IList.Add(object value)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                if (_owner._originals is not null)
                {
                    _owner._originals.Clear();
                }

                _imageInfoCollection.Clear();
                //_owner.OnChangeHandle(EventArgs.Empty);
            }

            bool IList.Contains(object value)
            {
                throw new NotImplementedException();
            }

            int IList.IndexOf(object value)
            {
                throw new NotImplementedException();
            }

            void IList.Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            void IList.Remove(object value)
            {
                throw new NotImplementedException();
            }

            void ICollection.CopyTo(Array array, int index)
            {
                for (int i = 0; i < Count; ++i)
                {
                    array.SetValue(_owner.GetBitmap(i), index++);
                }
            }

            public IEnumerator GetEnumerator()
            {
                Image[] images = new Image[Count];
                for (int i = 0; i < images.Length; ++i)
                {
                    images[i] = _owner.GetBitmap(i);
                }
                return images.GetEnumerator();
            }
        }

        /// <summary>
        ///  An image before we add it to the image list, along with a few details about how to add it.
        /// </summary>
        private class Original
        {
            internal object _image;
            internal OriginalOptions _options;
            internal Color _customTransparentColor = Color.Transparent;

            internal int _nImages = 1;

            internal Original(object image, OriginalOptions options) : this(image, options, Color.Transparent)
            {
            }

            internal Original(object image, OriginalOptions options, int nImages) : this(image, options, Color.Transparent)
            {
                _nImages = nImages;
            }

            internal Original(object image, OriginalOptions options, Color customTransparentColor)
            {
                if (image is not Icon && image is not Image)
                {
                    throw new InvalidOperationException("ImageListEntryType");
                }

                _image = image;
                _options = options;
                _customTransparentColor = customTransparentColor;
                if ((options & OriginalOptions.CustomTransparentColor) == 0)
                {
                    Debug.Assert(customTransparentColor.Equals(Color.Transparent), "Specified a custom transparent color then told us to ignore it");
                }
            }
        }

    }
}
