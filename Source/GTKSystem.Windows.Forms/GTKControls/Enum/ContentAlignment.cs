﻿namespace System.Drawing;

/// <summary>
///     Specifies alignment of content on the drawing surface.
/// </summary>
public enum ContentAlignment
{
    /// <summary>
    ///     Content is vertically aligned at the top, and horizontally aligned on the left.
    /// </summary>
    TopLeft = 1,

    /// <summary>
    ///     Content is vertically aligned at the top, and horizontally aligned at the center.
    /// </summary>
    TopCenter = 2,

    /// <summary>
    ///     Content is vertically aligned at the top, and horizontally aligned on the right.
    /// </summary>
    TopRight = 4,

    /// <summary>
    ///     Content is vertically aligned in the middle, and horizontally aligned on the
    ///     left.
    /// </summary>
    MiddleLeft = 16,

    /// <summary>
    ///     Content is vertically aligned in the middle, and horizontally aligned at the
    ///     center.
    /// </summary>
    MiddleCenter = 32,

    /// <summary>
    ///     Content is vertically aligned in the middle, and horizontally aligned on the
    ///     right.
    /// </summary>
    MiddleRight = 64,

    /// <summary>
    ///     Content is vertically aligned at the bottom, and horizontally aligned on the
    ///     left.
    /// </summary>
    BottomLeft = 256,

    /// <summary>
    ///     Content is vertically aligned at the bottom, and horizontally aligned at the
    ///     center.
    /// </summary>
    BottomCenter = 512,

    /// <summary>
    ///     Content is vertically aligned at the bottom, and horizontally aligned on the
    ///     right.
    /// </summary>
    BottomRight = 1024
}