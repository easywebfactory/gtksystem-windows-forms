﻿namespace System.Drawing.Text;

/// <summary>
/// Provides a base class for installed and private font collections.
/// </summary>
public abstract class FontCollection : IDisposable
{
    /// <summary>
    /// Gets the array of System.Drawing.FontFamily objects associated with this System.Drawing.Text.FontCollection.
    /// </summary>
    /// <returns>An array of System.Drawing.FontFamily objects.</returns>
    public FontFamily[]? Families { get; } = default;

    /// <summary>
    /// Releases all resources used by this System.Drawing.Text.FontCollection.
    /// </summary>
    public void Dispose() { }
    /// <summary>
    /// Releases the unmanaged resources used by the System.Drawing.Text.FontCollection
    /// and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    /// true to release both managed and unmanaged resources; false to release only unmanaged
    /// resources.
    /// </param>
    protected virtual void Dispose(bool disposing) { }
}