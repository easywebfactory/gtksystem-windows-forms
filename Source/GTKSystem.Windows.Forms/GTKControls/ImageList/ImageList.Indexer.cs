// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Windows.Forms;

public sealed partial class ImageList
{
    /// <summary>
    ///  This class is for classes that want to support both an ImageIndex
    ///  and ImageKey. We want to toggle between using keys or indexes.
    ///  Default is to use the integer index.
    /// </summary>
    public class Indexer
    {
        // Used by TreeViewImageIndexConverter to show "(none)"
        internal const int noneIndex = -2;

        // Used by generally across the board to indicate unset image
        internal const string? defaultKey = "";
        internal const int defaultIndex = -1;

        private string? _key = defaultKey;
        private int _index = defaultIndex;
        private bool _useIntegerIndex = true;

        public virtual ImageList? ImageList { get; set; }


        public virtual string? Key
        {
            get => _key;
            set
            {
                _index = defaultIndex;
                _key = value ?? defaultKey;
                _useIntegerIndex = false;
            }
        }

        public virtual int Index
        {
            get => _index;
            set
            {
                _key = defaultKey;
                _index = value;
                _useIntegerIndex = true;
            }
        }

        public virtual int ActualIndex
        {
            get
            {
                if (_useIntegerIndex)
                {
                    return Index;
                }
                if (ImageList is null)
                {

                }
                else //  if (ImageList != null)
                {
                    return ImageList.Images.IndexOfKey(Key);
                }

                return defaultIndex;
            }
        }
    }
}