using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Drawing.Imaging
{
	/// <summary>Provides properties that get the frame dimensions of an image. Not inheritable.</summary>
	public sealed class FrameDimension
	{
		private static readonly FrameDimension s_time = new FrameDimension(new Guid("{6aedbd6d-3fb5-418a-83a6-7f45229dc872}"));

		private static readonly FrameDimension s_resolution = new FrameDimension(new Guid("{84236f7b-3bd3-428f-8dab-4ea1439ca315}"));

		private static readonly FrameDimension s_page = new FrameDimension(new Guid("{7462dc86-6180-4c7e-8e3f-ee7333a7a483}"));

		private Guid _guid;

		/// <summary>Gets a globally unique identifier (GUID) that represents this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</summary>
		/// <returns>A <see langword="Guid" /> structure that contains a GUID that represents this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</returns>
		public Guid Guid => _guid;

		/// <summary>Gets the time dimension.</summary>
		/// <returns>The time dimension.</returns>
		public static FrameDimension Time => s_time;

		/// <summary>Gets the resolution dimension.</summary>
		/// <returns>The resolution dimension.</returns>
		public static FrameDimension Resolution => s_resolution;

		/// <summary>Gets the page dimension.</summary>
		/// <returns>The page dimension.</returns>
		public static FrameDimension Page => s_page;

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.FrameDimension" /> class using the specified <see langword="Guid" /> structure.</summary>
		/// <param name="guid">A <see langword="Guid" /> structure that contains a GUID for this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</param>
		public FrameDimension(Guid guid)
		{
			_guid = guid;
		}

		/// <summary>Returns a value that indicates whether the specified object is a <see cref="T:System.Drawing.Imaging.FrameDimension" /> equivalent to this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</summary>
		/// <param name="o">The object to test.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="o" /> is a <see cref="T:System.Drawing.Imaging.FrameDimension" /> equivalent to this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object; otherwise, <see langword="false" />.</returns>
		public override bool Equals([NotNullWhen(true)] object o)
		{
			FrameDimension frameDimension = o as FrameDimension;
			if (frameDimension == null)
			{
				return false;
			}
			return _guid == frameDimension._guid;
		}

		/// <summary>Returns a hash code for this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</summary>
		/// <returns>The hash code of this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</returns>
		public override int GetHashCode()
		{
			return _guid.GetHashCode();
		}

		/// <summary>Converts this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object to a human-readable string.</summary>
		/// <returns>A string that represents this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</returns>
		public override string ToString()
		{
			if (this == s_time)
			{
				return "Time";
			}
			if (this == s_resolution)
			{
				return "Resolution";
			}
			if (this == s_page)
			{
				return "Page";
			}
			//DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			//defaultInterpolatedStringHandler.AppendLiteral("[FrameDimension: ");
			//defaultInterpolatedStringHandler.AppendFormatted(_guid);
			//defaultInterpolatedStringHandler.AppendLiteral("]");
			//return defaultInterpolatedStringHandler.ToStringAndClear();
			return $"[FrameDimension: {_guid}]";
		}
	}
}
