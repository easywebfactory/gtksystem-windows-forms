namespace System.Drawing.Drawing2D
{
	/// <summary>Encapsulates the data that makes up a <see cref="T:System.Drawing.Region" /> object. This class cannot be inherited.</summary>
	public sealed class RegionData
	{
		/// <summary>Gets or sets an array of bytes that specify the <see cref="T:System.Drawing.Region" /> object.</summary>
		/// <returns>An array of bytes that specify the <see cref="T:System.Drawing.Region" /> object.</returns>
		public byte[] Data { get; set; }

		internal RegionData()
		{
		}
	}
}
