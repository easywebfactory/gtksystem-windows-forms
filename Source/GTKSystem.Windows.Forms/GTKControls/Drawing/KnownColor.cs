#if NETSTANDARD2_0
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Drawing;

/// <summary>Specifies the known system colors.</summary>
[TypeForwardedFrom("System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
internal enum KnownColor
{
    /// <summary>The system-defined color of the active window's border.</summary>
    ActiveBorder = 1,
    /// <summary>The system-defined color of the background of the active window's title bar.</summary>
    ActiveCaption = 2,
    /// <summary>The system-defined color of the text in the active window's title bar.</summary>
    ActiveCaptionText = 3,
    /// <summary>The system-defined color of the application workspace. The application workspace is the area in a multiple-document view that is not being occupied by documents.</summary>
    AppWorkspace = 4,
    /// <summary>The system-defined face color of a 3-D element.</summary>
    Control = 5,
    /// <summary>The system-defined shadow color of a 3-D element. The shadow color is applied to parts of a 3-D element that face away from the light source.</summary>
    ControlDark = 6,
    /// <summary>The system-defined color that is the dark shadow color of a 3-D element. The dark shadow color is applied to the parts of a 3-D element that are the darkest color.</summary>
    ControlDarkDark = 7,
    /// <summary>The system-defined color that is the light color of a 3-D element. The light color is applied to parts of a 3-D element that face the light source.</summary>
    ControlLight = 8,
    /// <summary>The system-defined highlight color of a 3-D element. The highlight color is applied to the parts of a 3-D element that are the lightest color.</summary>
    ControlLightLight = 9,
    /// <summary>The system-defined color of text in a 3-D element.</summary>
    ControlText = 10, // 0x0000000A
    /// <summary>The system-defined color of the desktop.</summary>
    Desktop = 11, // 0x0000000B
    /// <summary>The system-defined color of dimmed text. Items in a list that are disabled are displayed in dimmed text.</summary>
    GrayText = 12, // 0x0000000C
    /// <summary>The system-defined color of the background of selected items. This includes selected menu items as well as selected text.</summary>
    Highlight = 13, // 0x0000000D
    /// <summary>The system-defined color of the text of selected items.</summary>
    HighlightText = 14, // 0x0000000E
    /// <summary>The system-defined color used to designate a hot-tracked item. Single-clicking a hot-tracked item executes the item.</summary>
    HotTrack = 15, // 0x0000000F
    /// <summary>The system-defined color of an inactive window's border.</summary>
    InactiveBorder = 16, // 0x00000010
    /// <summary>The system-defined color of the background of an inactive window's title bar.</summary>
    InactiveCaption = 17, // 0x00000011
    /// <summary>The system-defined color of the text in an inactive window's title bar.</summary>
    InactiveCaptionText = 18, // 0x00000012
    /// <summary>The system-defined color of the background of a ToolTip.</summary>
    Info = 19, // 0x00000013
    /// <summary>The system-defined color of the text of a ToolTip.</summary>
    InfoText = 20, // 0x00000014
    /// <summary>The system-defined color of a menu's background.</summary>
    Menu = 21, // 0x00000015
    /// <summary>The system-defined color of a menu's text.</summary>
    MenuText = 22, // 0x00000016
    /// <summary>The system-defined color of the background of a scroll bar.</summary>
    ScrollBar = 23, // 0x00000017
    /// <summary>The system-defined color of the background in the client area of a window.</summary>
    Window = 24, // 0x00000018
    /// <summary>The system-defined color of a window frame.</summary>
    WindowFrame = 25, // 0x00000019
    /// <summary>The system-defined color of the text in the client area of a window.</summary>
    WindowText = 26, // 0x0000001A
    /// <summary>A system-defined color.</summary>
    Transparent = 27, // 0x0000001B
    /// <summary>A system-defined color.</summary>
    AliceBlue = 28, // 0x0000001C
    /// <summary>A system-defined color.</summary>
    AntiqueWhite = 29, // 0x0000001D
    /// <summary>A system-defined color.</summary>
    Aqua = 30, // 0x0000001E
    /// <summary>A system-defined color.</summary>
    Aquamarine = 31, // 0x0000001F
    /// <summary>A system-defined color.</summary>
    Azure = 32, // 0x00000020
    /// <summary>A system-defined color.</summary>
    Beige = 33, // 0x00000021
    /// <summary>A system-defined color.</summary>
    Bisque = 34, // 0x00000022
    /// <summary>A system-defined color.</summary>
    Black = 35, // 0x00000023
    /// <summary>A system-defined color.</summary>
    BlanchedAlmond = 36, // 0x00000024
    /// <summary>A system-defined color.</summary>
    Blue = 37, // 0x00000025
    /// <summary>A system-defined color.</summary>
    BlueViolet = 38, // 0x00000026
    /// <summary>A system-defined color.</summary>
    Brown = 39, // 0x00000027
    /// <summary>A system-defined color.</summary>
    BurlyWood = 40, // 0x00000028
    /// <summary>A system-defined color.</summary>
    CadetBlue = 41, // 0x00000029
    /// <summary>A system-defined color.</summary>
    Chartreuse = 42, // 0x0000002A
    /// <summary>A system-defined color.</summary>
    Chocolate = 43, // 0x0000002B
    /// <summary>A system-defined color.</summary>
    Coral = 44, // 0x0000002C
    /// <summary>A system-defined color.</summary>
    CornflowerBlue = 45, // 0x0000002D
    /// <summary>A system-defined color.</summary>
    Cornsilk = 46, // 0x0000002E
    /// <summary>A system-defined color.</summary>
    Crimson = 47, // 0x0000002F
    /// <summary>A system-defined color.</summary>
    Cyan = 48, // 0x00000030
    /// <summary>A system-defined color.</summary>
    DarkBlue = 49, // 0x00000031
    /// <summary>A system-defined color.</summary>
    DarkCyan = 50, // 0x00000032
    /// <summary>A system-defined color.</summary>
    DarkGoldenrod = 51, // 0x00000033
    /// <summary>A system-defined color.</summary>
    DarkGray = 52, // 0x00000034
    /// <summary>A system-defined color.</summary>
    DarkGreen = 53, // 0x00000035
    /// <summary>A system-defined color.</summary>
    DarkKhaki = 54, // 0x00000036
    /// <summary>A system-defined color.</summary>
    DarkMagenta = 55, // 0x00000037
    /// <summary>A system-defined color.</summary>
    DarkOliveGreen = 56, // 0x00000038
    /// <summary>A system-defined color.</summary>
    DarkOrange = 57, // 0x00000039
    /// <summary>A system-defined color.</summary>
    DarkOrchid = 58, // 0x0000003A
    /// <summary>A system-defined color.</summary>
    DarkRed = 59, // 0x0000003B
    /// <summary>A system-defined color.</summary>
    DarkSalmon = 60, // 0x0000003C
    /// <summary>A system-defined color.</summary>
    DarkSeaGreen = 61, // 0x0000003D
    /// <summary>A system-defined color.</summary>
    DarkSlateBlue = 62, // 0x0000003E
    /// <summary>A system-defined color.</summary>
    DarkSlateGray = 63, // 0x0000003F
    /// <summary>A system-defined color.</summary>
    DarkTurquoise = 64, // 0x00000040
    /// <summary>A system-defined color.</summary>
    DarkViolet = 65, // 0x00000041
    /// <summary>A system-defined color.</summary>
    DeepPink = 66, // 0x00000042
    /// <summary>A system-defined color.</summary>
    DeepSkyBlue = 67, // 0x00000043
    /// <summary>A system-defined color.</summary>
    DimGray = 68, // 0x00000044
    /// <summary>A system-defined color.</summary>
    DodgerBlue = 69, // 0x00000045
    /// <summary>A system-defined color.</summary>
    Firebrick = 70, // 0x00000046
    /// <summary>A system-defined color.</summary>
    FloralWhite = 71, // 0x00000047
    /// <summary>A system-defined color.</summary>
    ForestGreen = 72, // 0x00000048
    /// <summary>A system-defined color.</summary>
    Fuchsia = 73, // 0x00000049
    /// <summary>A system-defined color.</summary>
    Gainsboro = 74, // 0x0000004A
    /// <summary>A system-defined color.</summary>
    GhostWhite = 75, // 0x0000004B
    /// <summary>A system-defined color.</summary>
    Gold = 76, // 0x0000004C
    /// <summary>A system-defined color.</summary>
    Goldenrod = 77, // 0x0000004D
    /// <summary>A system-defined color.</summary>
    Gray = 78, // 0x0000004E
    /// <summary>A system-defined color.</summary>
    Green = 79, // 0x0000004F
    /// <summary>A system-defined color.</summary>
    GreenYellow = 80, // 0x00000050
    /// <summary>A system-defined color.</summary>
    Honeydew = 81, // 0x00000051
    /// <summary>A system-defined color.</summary>
    HotPink = 82, // 0x00000052
    /// <summary>A system-defined color.</summary>
    IndianRed = 83, // 0x00000053
    /// <summary>A system-defined color.</summary>
    Indigo = 84, // 0x00000054
    /// <summary>A system-defined color.</summary>
    Ivory = 85, // 0x00000055
    /// <summary>A system-defined color.</summary>
    Khaki = 86, // 0x00000056
    /// <summary>A system-defined color.</summary>
    Lavender = 87, // 0x00000057
    /// <summary>A system-defined color.</summary>
    LavenderBlush = 88, // 0x00000058
    /// <summary>A system-defined color.</summary>
    LawnGreen = 89, // 0x00000059
    /// <summary>A system-defined color.</summary>
    LemonChiffon = 90, // 0x0000005A
    /// <summary>A system-defined color.</summary>
    LightBlue = 91, // 0x0000005B
    /// <summary>A system-defined color.</summary>
    LightCoral = 92, // 0x0000005C
    /// <summary>A system-defined color.</summary>
    LightCyan = 93, // 0x0000005D
    /// <summary>A system-defined color.</summary>
    LightGoldenrodYellow = 94, // 0x0000005E
    /// <summary>A system-defined color.</summary>
    LightGray = 95, // 0x0000005F
    /// <summary>A system-defined color.</summary>
    LightGreen = 96, // 0x00000060
    /// <summary>A system-defined color.</summary>
    LightPink = 97, // 0x00000061
    /// <summary>A system-defined color.</summary>
    LightSalmon = 98, // 0x00000062
    /// <summary>A system-defined color.</summary>
    LightSeaGreen = 99, // 0x00000063
    /// <summary>A system-defined color.</summary>
    LightSkyBlue = 100, // 0x00000064
    /// <summary>A system-defined color.</summary>
    LightSlateGray = 101, // 0x00000065
    /// <summary>A system-defined color.</summary>
    LightSteelBlue = 102, // 0x00000066
    /// <summary>A system-defined color.</summary>
    LightYellow = 103, // 0x00000067
    /// <summary>A system-defined color.</summary>
    Lime = 104, // 0x00000068
    /// <summary>A system-defined color.</summary>
    LimeGreen = 105, // 0x00000069
    /// <summary>A system-defined color.</summary>
    Linen = 106, // 0x0000006A
    /// <summary>A system-defined color.</summary>
    Magenta = 107, // 0x0000006B
    /// <summary>A system-defined color.</summary>
    Maroon = 108, // 0x0000006C
    /// <summary>A system-defined color.</summary>
    MediumAquamarine = 109, // 0x0000006D
    /// <summary>A system-defined color.</summary>
    MediumBlue = 110, // 0x0000006E
    /// <summary>A system-defined color.</summary>
    MediumOrchid = 111, // 0x0000006F
    /// <summary>A system-defined color.</summary>
    MediumPurple = 112, // 0x00000070
    /// <summary>A system-defined color.</summary>
    MediumSeaGreen = 113, // 0x00000071
    /// <summary>A system-defined color.</summary>
    MediumSlateBlue = 114, // 0x00000072
    /// <summary>A system-defined color.</summary>
    MediumSpringGreen = 115, // 0x00000073
    /// <summary>A system-defined color.</summary>
    MediumTurquoise = 116, // 0x00000074
    /// <summary>A system-defined color.</summary>
    MediumVioletRed = 117, // 0x00000075
    /// <summary>A system-defined color.</summary>
    MidnightBlue = 118, // 0x00000076
    /// <summary>A system-defined color.</summary>
    MintCream = 119, // 0x00000077
    /// <summary>A system-defined color.</summary>
    MistyRose = 120, // 0x00000078
    /// <summary>A system-defined color.</summary>
    Moccasin = 121, // 0x00000079
    /// <summary>A system-defined color.</summary>
    NavajoWhite = 122, // 0x0000007A
    /// <summary>A system-defined color.</summary>
    Navy = 123, // 0x0000007B
    /// <summary>A system-defined color.</summary>
    OldLace = 124, // 0x0000007C
    /// <summary>A system-defined color.</summary>
    Olive = 125, // 0x0000007D
    /// <summary>A system-defined color.</summary>
    OliveDrab = 126, // 0x0000007E
    /// <summary>A system-defined color.</summary>
    Orange = 127, // 0x0000007F
    /// <summary>A system-defined color.</summary>
    OrangeRed = 128, // 0x00000080
    /// <summary>A system-defined color.</summary>
    Orchid = 129, // 0x00000081
    /// <summary>A system-defined color.</summary>
    PaleGoldenrod = 130, // 0x00000082
    /// <summary>A system-defined color.</summary>
    PaleGreen = 131, // 0x00000083
    /// <summary>A system-defined color.</summary>
    PaleTurquoise = 132, // 0x00000084
    /// <summary>A system-defined color.</summary>
    PaleVioletRed = 133, // 0x00000085
    /// <summary>A system-defined color.</summary>
    PapayaWhip = 134, // 0x00000086
    /// <summary>A system-defined color.</summary>
    PeachPuff = 135, // 0x00000087
    /// <summary>A system-defined color.</summary>
    Peru = 136, // 0x00000088
    /// <summary>A system-defined color.</summary>
    Pink = 137, // 0x00000089
    /// <summary>A system-defined color.</summary>
    Plum = 138, // 0x0000008A
    /// <summary>A system-defined color.</summary>
    PowderBlue = 139, // 0x0000008B
    /// <summary>A system-defined color.</summary>
    Purple = 140, // 0x0000008C
    /// <summary>A system-defined color.</summary>
    Red = 141, // 0x0000008D
    /// <summary>A system-defined color.</summary>
    RosyBrown = 142, // 0x0000008E
    /// <summary>A system-defined color.</summary>
    RoyalBlue = 143, // 0x0000008F
    /// <summary>A system-defined color.</summary>
    SaddleBrown = 144, // 0x00000090
    /// <summary>A system-defined color.</summary>
    Salmon = 145, // 0x00000091
    /// <summary>A system-defined color.</summary>
    SandyBrown = 146, // 0x00000092
    /// <summary>A system-defined color.</summary>
    SeaGreen = 147, // 0x00000093
    /// <summary>A system-defined color.</summary>
    SeaShell = 148, // 0x00000094
    /// <summary>A system-defined color.</summary>
    Sienna = 149, // 0x00000095
    /// <summary>A system-defined color.</summary>
    Silver = 150, // 0x00000096
    /// <summary>A system-defined color.</summary>
    SkyBlue = 151, // 0x00000097
    /// <summary>A system-defined color.</summary>
    SlateBlue = 152, // 0x00000098
    /// <summary>A system-defined color.</summary>
    SlateGray = 153, // 0x00000099
    /// <summary>A system-defined color.</summary>
    Snow = 154, // 0x0000009A
    /// <summary>A system-defined color.</summary>
    SpringGreen = 155, // 0x0000009B
    /// <summary>A system-defined color.</summary>
    SteelBlue = 156, // 0x0000009C
    /// <summary>A system-defined color.</summary>
    Tan = 157, // 0x0000009D
    /// <summary>A system-defined color.</summary>
    Teal = 158, // 0x0000009E
    /// <summary>A system-defined color.</summary>
    Thistle = 159, // 0x0000009F
    /// <summary>A system-defined color.</summary>
    Tomato = 160, // 0x000000A0
    /// <summary>A system-defined color.</summary>
    Turquoise = 161, // 0x000000A1
    /// <summary>A system-defined color.</summary>
    Violet = 162, // 0x000000A2
    /// <summary>A system-defined color.</summary>
    Wheat = 163, // 0x000000A3
    /// <summary>A system-defined color.</summary>
    White = 164, // 0x000000A4
    /// <summary>A system-defined color.</summary>
    WhiteSmoke = 165, // 0x000000A5
    /// <summary>A system-defined color.</summary>
    Yellow = 166, // 0x000000A6
    /// <summary>A system-defined color.</summary>
    YellowGreen = 167, // 0x000000A7
    /// <summary>The system-defined face color of a 3-D element.</summary>
    ButtonFace = 168, // 0x000000A8
    /// <summary>The system-defined color that is the highlight color of a 3-D element. This color is applied to parts of a 3-D element that face the light source.</summary>
    ButtonHighlight = 169, // 0x000000A9
    /// <summary>The system-defined color that is the shadow color of a 3-D element. This color is applied to parts of a 3-D element that face away from the light source.</summary>
    ButtonShadow = 170, // 0x000000AA
    /// <summary>The system-defined color of the lightest color in the color gradient of an active window's title bar.</summary>
    GradientActiveCaption = 171, // 0x000000AB
    /// <summary>The system-defined color of the lightest color in the color gradient of an inactive window's title bar.</summary>
    GradientInactiveCaption = 172, // 0x000000AC
    /// <summary>The system-defined color of the background of a menu bar.</summary>
    MenuBar = 173, // 0x000000AD
    /// <summary>The system-defined color used to highlight menu items when the menu appears as a flat menu.</summary>
    MenuHighlight = 174, // 0x000000AE
    /// <summary>A system-defined color representing the ARGB value <c>#663399</c>.</summary>
    RebeccaPurple = 175, // 0x000000AF
}

#endif