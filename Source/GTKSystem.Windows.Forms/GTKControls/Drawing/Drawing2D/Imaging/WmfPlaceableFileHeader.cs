using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	/// <summary>Defines a placeable metafile. Not inheritable.</summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class WmfPlaceableFileHeader
	{
		private int _key = -1698247209;

		private short _hmf;

		private short _bboxLeft;

		private short _bboxTop;

		private short _bboxRight;

		private short _bboxBottom;

		private short _inch;

		private int _reserved;

		private short _checksum;

		/// <summary>Gets or sets a value indicating the presence of a placeable metafile header.</summary>
		/// <returns>A value indicating presence of a placeable metafile header.</returns>
		public int Key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
			}
		}

		/// <summary>Gets or sets the handle of the metafile in memory.</summary>
		/// <returns>The handle of the metafile in memory.</returns>
		public short Hmf
		{
			get
			{
				return _hmf;
			}
			set
			{
				_hmf = value;
			}
		}

		/// <summary>Gets or sets the x-coordinate of the upper-left corner of the bounding rectangle of the metafile image on the output device.</summary>
		/// <returns>The x-coordinate of the upper-left corner of the bounding rectangle of the metafile image on the output device.</returns>
		public short BboxLeft
		{
			get
			{
				return _bboxLeft;
			}
			set
			{
				_bboxLeft = value;
			}
		}

		/// <summary>Gets or sets the y-coordinate of the upper-left corner of the bounding rectangle of the metafile image on the output device.</summary>
		/// <returns>The y-coordinate of the upper-left corner of the bounding rectangle of the metafile image on the output device.</returns>
		public short BboxTop
		{
			get
			{
				return _bboxTop;
			}
			set
			{
				_bboxTop = value;
			}
		}

		/// <summary>Gets or sets the x-coordinate of the lower-right corner of the bounding rectangle of the metafile image on the output device.</summary>
		/// <returns>The x-coordinate of the lower-right corner of the bounding rectangle of the metafile image on the output device.</returns>
		public short BboxRight
		{
			get
			{
				return _bboxRight;
			}
			set
			{
				_bboxRight = value;
			}
		}

		/// <summary>Gets or sets the y-coordinate of the lower-right corner of the bounding rectangle of the metafile image on the output device.</summary>
		/// <returns>The y-coordinate of the lower-right corner of the bounding rectangle of the metafile image on the output device.</returns>
		public short BboxBottom
		{
			get
			{
				return _bboxBottom;
			}
			set
			{
				_bboxBottom = value;
			}
		}

		/// <summary>Gets or sets the number of twips per inch.</summary>
		/// <returns>The number of twips per inch.</returns>
		public short Inch
		{
			get
			{
				return _inch;
			}
			set
			{
				_inch = value;
			}
		}

		/// <summary>Reserved. Do not use.</summary>
		/// <returns>Reserved. Do not use.</returns>
		public int Reserved
		{
			get
			{
				return _reserved;
			}
			set
			{
				_reserved = value;
			}
		}

		/// <summary>Gets or sets the checksum value for the previous ten <see langword="WORD" /> s in the header.</summary>
		/// <returns>The checksum value for the previous ten <see langword="WORD" /> s in the header.</returns>
		public short Checksum
		{
			get
			{
				return _checksum;
			}
			set
			{
				_checksum = value;
			}
		}

		internal ref int GetPinnableReference()
		{
			return ref _key;
		}

		/// <summary>Initializes a new instance of the <see langword="WmfPlaceableFileHeader" /> class.</summary>
		public WmfPlaceableFileHeader()
		{
		}
	}
}
