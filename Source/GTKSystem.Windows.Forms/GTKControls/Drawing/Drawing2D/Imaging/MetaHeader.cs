using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
    /// <summary>Contains information about a windows-format (WMF) metafile.</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public sealed class MetaHeader
    {
        private WmfMetaHeader _data;

        /// <summary>Gets or sets the type of the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</summary>
        /// <returns>The type of the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</returns>
        public short Type
        {
            get { return _data._type; }
            set { _data._type = value; }
        }

        /// <summary>Gets or sets the size, in bytes, of the header file.</summary>
        /// <returns>The size, in bytes, of the header file.</returns>
        public short HeaderSize
        {
            get { return _data._headerSize; }
            set { _data._headerSize = value; }
        }

        /// <summary>Gets or sets the version number of the header format.</summary>
        /// <returns>The version number of the header format.</returns>
        public short Version
        {
            get { return _data._version; }
            set { _data._version = value; }
        }

        /// <summary>Gets or sets the size, in bytes, of the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</summary>
        /// <returns>The size, in bytes, of the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</returns>
        public int Size
        {
            get { return _data._size; }
            set { _data._size = value; }
        }

        /// <summary>Gets or sets the maximum number of objects that exist in the <see cref="T:System.Drawing.Imaging.Metafile" /> object at the same time.</summary>
        /// <returns>The maximum number of objects that exist in the <see cref="T:System.Drawing.Imaging.Metafile" /> object at the same time.</returns>
        public short NoObjects
        {
            get { return _data._noObjects; }
            set { _data._noObjects = value; }
        }

        /// <summary>Gets or sets the size, in bytes, of the largest record in the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</summary>
        /// <returns>The size, in bytes, of the largest record in the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</returns>
        public int MaxRecord
        {
            get { return _data._maxRecord; }
            set { _data._maxRecord = value; }
        }

        /// <summary>Not used. Always returns 0.</summary>
        /// <returns>Always 0.</returns>
        public short NoParameters
        {
            get { return _data._noParameters; }
            set { _data._noParameters = value; }
        }

        /// <summary>Initializes a new instance of the <see langword="MetaHeader" /> class.</summary>
        public MetaHeader()
        {
        }

        internal MetaHeader(WmfMetaHeader header)
        {
            _data._type = header._type;
            _data._headerSize = header._headerSize;
            _data._version = header._version;
            _data._size = header._size;
            _data._noObjects = header._noObjects;
            _data._maxRecord = header._maxRecord;
            _data._noParameters = header._noParameters;
        }

        internal WmfMetaHeader GetNativeValue()
        {
            return _data;
        }
    }
}