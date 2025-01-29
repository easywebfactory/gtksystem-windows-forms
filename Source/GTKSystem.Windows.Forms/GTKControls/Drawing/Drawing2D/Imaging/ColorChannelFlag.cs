namespace System.Drawing.Imaging
{
    /// <summary>Specifies individual channels in the CMYK (cyan, magenta, yellow, black) color space. This enumeration is used by the <see cref="Overload:System.Drawing.Imaging.ImageAttributes.SetOutputChannel" /> methods.</summary>
    public enum ColorChannelFlag
    {
        /// <summary>The cyan color channel.</summary>
        ColorChannelC,

        /// <summary>The magenta color channel.</summary>
        ColorChannelM,

        /// <summary>The yellow color channel.</summary>
        ColorChannelY,

        /// <summary>The black color channel.</summary>
        ColorChannelK,

        /// <summary>The last selected channel should be used.</summary>
        ColorChannelLast
    }
}