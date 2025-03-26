namespace System.Windows.Forms;

public enum ImageLayout
{
    /// <summary>
    ///     The image is left-aligned at the top across the control's client rectangle.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The image is tiled across the control's client rectangle.
    /// </summary>
    Tile = 1,

    /// <summary>
    ///     The image is centered within the control's client rectangle.
    /// </summary>
    Center = 2,

    /// <summary>
    ///     The image is stretched across the control's client rectangle.
    /// </summary>
    Stretch = 3,

    /// <summary>
    ///     The image is enlarged within the control's client rectangle.
    /// </summary>
    Zoom = 4
}