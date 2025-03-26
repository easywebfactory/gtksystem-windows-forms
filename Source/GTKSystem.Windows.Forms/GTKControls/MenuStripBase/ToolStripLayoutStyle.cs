namespace System.Windows.Forms;

/// <summary>
///     Specifies the possible alignments with which the items of a System.Windows.Forms.ToolStrip
///     can be displayed.
/// </summary>
public enum ToolStripLayoutStyle
{
    /// <summary>
    ///     Specifies that items are laid out automatically.
    /// </summary>
    StackWithOverflow = 0,

    /// <summary>
    ///     Specifies that items are laid out horizontally and overflow as necessary.
    /// </summary>
    HorizontalStackWithOverflow = 1,

    /// <summary>
    ///     Specifies that items are laid out vertically, are centered within the control,
    ///     and overflow as necessary.
    /// </summary>
    VerticalStackWithOverflow = 2,

    /// <summary>
    ///     Specifies that items flow horizontally or vertically as necessary.
    /// </summary>
    Flow = 3,

    /// <summary>
    ///     Specifies that items are laid out flush left.
    /// </summary>
    Table = 4
}