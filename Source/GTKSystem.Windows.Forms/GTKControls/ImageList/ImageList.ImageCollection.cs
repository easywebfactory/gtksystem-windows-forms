// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;

namespace System.Windows.Forms
{
    public sealed partial class ImageList
    {

        public sealed partial class ImageCollection : IList
        {
            private readonly ImageList _owner;
            private readonly List<ImageInfo> _imageInfoCollection = new List<ImageInfo>();

            ///  A caching mechanism for key accessor
            ///  We use an index here rather than control so that we don't have lifetime
            ///  issues by holding on to extra references.
            private int _lastAccessedIndex = -1;

            // Indicates whether images are added in a batch.
            private bool _isBatchAdd;

            /// <summary>
            ///  Returns the keys in the image list - images without keys return String.Empty.
            /// </summary>
            public StringCollection Keys
            {
                get
                {
                    // pass back a copy of the current state.
                    StringCollection keysCollection = new StringCollection();

                    for (int i = 0; i < _imageInfoCollection.Count; i++)
                    {
                        if ((_imageInfoCollection[i] is ImageInfo image) && (image.Name != null) && (image.Name.Length != 0))
                        {
                            keysCollection.Add(image.Name);
                        }
                        else
                        {
                            keysCollection.Add(string.Empty);
                        }
                    }

                    return keysCollection;
                }
            }

            internal ImageCollection(ImageList owner)
            {
                _owner = owner;
            }

            internal void ResetKeys()
            {
                _imageInfoCollection.Clear();

                for (int i = 0; i < Count; i++)
                {
                    _imageInfoCollection.Add(new ImageInfo());
                }
            }

            [Browsable(false)]
            public int Count
            {
                get
                {
                    return _imageInfoCollection.Count;
                }
            }

            object ICollection.SyncRoot => this;

            bool ICollection.IsSynchronized => false;

            bool IList.IsFixedSize => false;

            public bool IsReadOnly => false;

            /// <summary>
            ///  Determines if the ImageList has any images, without forcing a handle creation.
            /// </summary>
            public bool Empty => Count == 0;

            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public Image this[int index]
            {
                get
                {
                    return _owner._originals[index]._image as Image;
                }
                set
                {
                    if ((_imageInfoCollection.Count > index))
                    {
                        Bitmap bitmap = value as Bitmap;
                        _owner._originals[index] = new Original(bitmap.Clone(), OriginalOptions.OwnsImage);
                    }
                }
            }

            object? IList.this[int index]
            {
                get => this[index];
                set
                {
                     
                }
            }

            /// <summary>
            ///  Retrieves the child control with the specified key.
            /// </summary>
            public Image? this[string key]
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

            /// <summary>
            ///  Adds an image to the end of the image list with a key accessor.
            /// </summary>
            public void Add(string key, Image image)
            {
                // Store off the name.
                ImageInfo imageInfo = new ImageInfo()
                {
                    Name = key
                };
                // Add the image to the IList
                Original original = new Original(image, OriginalOptions.Default);
                Add(original, imageInfo);
            }

            /// <summary>
            ///  Adds an icon to the end of the image list with a key accessor.
            /// </summary>
            public void Add(string key, Icon icon)
            {
                // Store off the name.
                ImageInfo imageInfo = new ImageInfo()
                {
                    Name = key
                };

                // Add the image to the IList
                Original original = new Original(icon, OriginalOptions.Default);
                Add(original, imageInfo);
            }

            int IList.Add(object? value)
            {
                Add((Image)value);
                return Count - 1;
            }

            public void Add(Icon value)
            {
                // Don't clone it now is a breaking change, so we have to keep track of this specific icon and dispose that
                Add(new Original(value.Clone(), OriginalOptions.OwnsImage), null);
            }

            /// <summary>
            ///  Add the given image to the ImageList.
            /// </summary>
            public void Add(Image value)
            {
                //ArgumentNullException.ThrowIfNull(value);

                Original original = new Original(value, OriginalOptions.Default);
                Add(original, null);
            }

            /// <summary>
            ///  Add the given image to the ImageList, using the given color
            ///  to generate the mask. The number of images to add is inferred from
            ///  the width of the given image.
            /// </summary>
            public int Add(Image value, Color transparentColor)
            {
                //ArgumentNullException.ThrowIfNull(value);

                Original original = new Original(value, OriginalOptions.CustomTransparentColor, transparentColor);
                return Add(original, null);
            }

            private int Add(Original original, ImageInfo? imageInfo)
            {
                _imageInfoCollection.Add(imageInfo ?? new ImageInfo());
                _owner._originals.Add(original);
                _owner.OnChangeHandle(EventArgs.Empty);
                return _imageInfoCollection.Count - 1;
            }

            public void AddRange(params Image[] images)
            {
                foreach (Image image in images)
                {
                    Add(image);
                }
            }

            /// <summary>
            ///  Add an image strip the given image to the ImageList. A strip is a single Image
            ///  which is treated as multiple images arranged side-by-side.
            /// </summary>
            public int AddStrip(Image value)
            {
                int nImages = value.Width / _owner.ImageSize.Width;

                Original original = new Original(value, OriginalOptions.ImageStrip, nImages);

                return Add(original, null);
            }

            /// <summary>
            ///  Remove all images and masks from the ImageList.
            /// </summary>
            public void Clear()
            {
                _owner._originals?.Clear();
                _imageInfoCollection.Clear();
                _owner.OnChangeHandle(EventArgs.Empty);
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public bool Contains(Image image) => throw new NotSupportedException();

            bool IList.Contains(object? value)
            {
                if (value is Image image)
                {
                    return Contains(image);
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            ///  Returns true if the collection contains an item with the specified key, false otherwise.
            /// </summary>
            public bool ContainsKey(string key) => IsValidIndex(IndexOfKey(key));

            [EditorBrowsable(EditorBrowsableState.Never)]
            public int IndexOf(Image image) => throw new NotSupportedException();

            int IList.IndexOf(object? value)
            {
                if (value is Image image)
                {
                    return IndexOf(image);
                    
                }
                return -1;
            }

            /// <summary>
            ///  The zero-based index of the first occurrence of value within the entire CollectionBase,
            ///  if found; otherwise, -1.
            /// </summary>
            public int IndexOfKey(string key)
            {
                if (string.IsNullOrEmpty(key))
                {
                    // We don't support empty or null keys.
                    return -1;
                }
                return _imageInfoCollection.FindIndex(o => o.Name == key);
            }

            void IList.Insert(int index, object? value) => throw new NotSupportedException();

            /// <summary>
            ///  Determines if the index is valid for the collection.
            /// </summary>
            private bool IsValidIndex(int index) => index >= 0 && index < Count;

            void ICollection.CopyTo(Array dest, int index)
            {
                for (int i = 0; i < Count; ++i)
                {
                    dest.SetValue(_owner.GetBitmap(i), index++);
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

            [EditorBrowsable(EditorBrowsableState.Never)]
            public void Remove(Image image) => throw new NotSupportedException();

            void IList.Remove(object? value)
            {
                if (value is Image image)
                {
                    Remove(image);

                    _owner.OnChangeHandle(EventArgs.Empty);
                }
            }

            public void RemoveAt(int index)
            {
                if (_imageInfoCollection is null)
                    return;
                if (index >= 0 && index < _imageInfoCollection.Count)
                {
                    _imageInfoCollection.RemoveAt(index);
                    _owner._originals.RemoveAt(index);
                    _owner.OnChangeHandle(EventArgs.Empty);
                }
            }

            /// <summary>
            ///  Removes the child control with the specified key.
            /// </summary>
            public void RemoveByKey(string key)
            {
                int index = IndexOfKey(key);
                if (IsValidIndex(index))
                {
                    RemoveAt(index);
                }
            }

            /// <summary>
            ///  请必须把相关图片复制到目录Resources下，保持文件名与name一致
            /// </summary>
            public void SetKeyName(int index, string name)
            {
                int len = _imageInfoCollection.Count;
                if (len > index)
                {
                    _imageInfoCollection[index].Name = name;
                    _owner._originals[index] = new Original(_owner.GetOriginalImage(name), OriginalOptions.OwnsImage);
                }
                else
                {
                    for (int i = len; i < index + 1; i++)
                    {
                        this.Add(name, _owner.GetOriginalImage(name));
                    }
                }
            }
        }
    }
}