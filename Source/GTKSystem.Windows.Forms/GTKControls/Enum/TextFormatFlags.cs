// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Windows.Forms;

/// <summary>
///  Specifies the display and layout information for text strings.
/// </summary>
[Flags]
public enum TextFormatFlags
{
    Bottom                              = 0x0010_0000,
    EndEllipsis                         = 0x0020_0000,
    ExpandTabs                          = 0x0030_0000,
    ExternalLeading                     = 0x0040_0000,
    Default                             = default,
    HidePrefix                          = 0x0050_0000,
    HorizontalCenter                    = 0x0060_0000,
    Internal                            = 0x0070_0000,

    /// <remarks>
    ///  This is the default.
    /// </remarks>
    Left                                = 0x0080_0000,

    [Obsolete("ModifyString mutates strings and should be avoided. It will be blocked in a future release.")]
    ModifyString                        = 0x0090_0000,
    NoClipping                          = 0x0011_0000,
    NoPrefix                            = 0x0012_0000,
    NoFullWidthCharacterBreak           = 0x0013_0000,
    PathEllipsis                        = 0x0014_0000,
    PrefixOnly                          = 0x0015_0000,
    Right                               = 0x0016_0000,
    RightToLeft                         = 0x0017_0000,
    SingleLine                          = 0x0018_0000,
    TextBoxControl                      = 0x0019_0000,

    /// <remarks>
    ///  This is the default.
    /// </remarks>
    Top                                 = 0x0021_0000,

    VerticalCenter                      = 0x0022_0000,
    WordBreak                           = 0x0023_0000,
    WordEllipsis                        = 0x0024_0000,


    PreserveGraphicsClipping            = 0x0100_0000,
    PreserveGraphicsTranslateTransform  = 0x0200_0000,


    GlyphOverhangPadding                = 0x0000_0000,
    NoPadding                           = 0x1000_0000,
    LeftAndRightPadding                 = 0x2000_0000
}
