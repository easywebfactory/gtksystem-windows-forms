namespace System.Drawing.Imaging
{
	/// <summary>Encapsulates an array of <see cref="T:System.Drawing.Imaging.EncoderParameter" /> objects.</summary>
	public sealed class EncoderParameters : IDisposable
	{
		private EncoderParameter[] _param;

		/// <summary>Gets or sets an array of <see cref="T:System.Drawing.Imaging.EncoderParameter" /> objects.</summary>
		/// <returns>The array of <see cref="T:System.Drawing.Imaging.EncoderParameter" /> objects.</returns>
		public EncoderParameter[] Param
		{
			get
			{
				return _param;
			}
			set
			{
				_param = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameters" /> class that can contain the specified number of <see cref="T:System.Drawing.Imaging.EncoderParameter" /> objects.</summary>
		/// <param name="count">An integer that specifies the number of <see cref="T:System.Drawing.Imaging.EncoderParameter" /> objects that the <see cref="T:System.Drawing.Imaging.EncoderParameters" /> object can contain.</param>
		public EncoderParameters(int count)
		{
			_param = new EncoderParameter[count];
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameters" /> class that can contain one <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
		public EncoderParameters()
		{
			_param = new EncoderParameter[1];
		}

		internal IntPtr ConvertToMemory()
		{
			
			return IntPtr.Zero;
		}

		internal  static EncoderParameters ConvertFromMemory(IntPtr memory)
		{

			EncoderParameters encoderParameters = new EncoderParameters(1);

			return encoderParameters;
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Imaging.EncoderParameters" /> object.</summary>
		public void Dispose()
		{
			EncoderParameter[] param = _param;
			for (int i = 0; i < param.Length; i++)
			{
				param[i]?.Dispose();
			}
			_param = null;
		}
	}
}
