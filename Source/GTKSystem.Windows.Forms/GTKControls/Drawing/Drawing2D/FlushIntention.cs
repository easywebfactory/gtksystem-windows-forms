namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies whether commands in the graphics stack are terminated (flushed) immediately or executed as soon as possible.</summary>
	public enum FlushIntention
	{
		/// <summary>Specifies that the stack of all graphics operations is flushed immediately.</summary>
		Flush,
		/// <summary>Specifies that all graphics operations on the stack are executed as soon as possible. This synchronizes the graphics state.</summary>
		Sync
	}
}
