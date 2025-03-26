﻿namespace System.Windows.Forms;

/// <summary>
///     Specifies what to render (image or text) for this System.Windows.Forms.ToolStripItem.
/// </summary>
public enum ToolStripItemDisplayStyle
{
    /// <summary>
    ///     Specifies that neither image nor text is to be rendered for this System.Windows.Forms.ToolStripItem.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Specifies that only text is to be rendered for this System.Windows.Forms.ToolStripItem.
    /// </summary>
    Text = 1,

    /// <summary>
    ///     Specifies that only an image is to be rendered for this System.Windows.Forms.ToolStripItem.
    /// </summary>
    Image = 2,

    /// <summary>
    ///     Specifies that both an image and text are to be rendered for this System.Windows.Forms.ToolStripItem.
    /// </summary>
    ImageAndText = 3
}