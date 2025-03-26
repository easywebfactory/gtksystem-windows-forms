// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#if NET462_OR_GREATER
namespace System.Drawing
#else
namespace System.Drawing.Gtk
#endif
{
    [DebuggerDisplay("{NameAndARGBValue}")]
    [Editor("System.Drawing.Design.ColorEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [Serializable]
    [TypeConverter("System.Drawing.ColorConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [TypeForwardedFrom("System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public readonly struct Color : IEquatable<Color>
    {
        public static readonly Color Empty;

        // -------------------------------------------------------------------
        //  static list of "web" colors...
        //
        public static Color Transparent => new(KnownColor.Transparent);

        public static Color AliceBlue => new(KnownColor.AliceBlue);

        public static Color AntiqueWhite => new(KnownColor.AntiqueWhite);

        public static Color Aqua => new(KnownColor.Aqua);

        public static Color Aquamarine => new(KnownColor.Aquamarine);

        public static Color Azure => new(KnownColor.Azure);

        public static Color Beige => new(KnownColor.Beige);

        public static Color Bisque => new(KnownColor.Bisque);

        public static Color Black => new(KnownColor.Black);

        public static Color BlanchedAlmond => new(KnownColor.BlanchedAlmond);

        public static Color Blue => new(KnownColor.Blue);

        public static Color BlueViolet => new(KnownColor.BlueViolet);

        public static Color Brown => new(KnownColor.Brown);

        public static Color BurlyWood => new(KnownColor.BurlyWood);

        public static Color CadetBlue => new(KnownColor.CadetBlue);

        public static Color Chartreuse => new(KnownColor.Chartreuse);

        public static Color Chocolate => new(KnownColor.Chocolate);

        public static Color Coral => new(KnownColor.Coral);

        public static Color CornflowerBlue => new(KnownColor.CornflowerBlue);

        public static Color Cornsilk => new(KnownColor.Cornsilk);

        public static Color Crimson => new(KnownColor.Crimson);

        public static Color Cyan => new(KnownColor.Cyan);

        public static Color DarkBlue => new(KnownColor.DarkBlue);

        public static Color DarkCyan => new(KnownColor.DarkCyan);

        public static Color DarkGoldenrod => new(KnownColor.DarkGoldenrod);

        public static Color DarkGray => new(KnownColor.DarkGray);

        public static Color DarkGreen => new(KnownColor.DarkGreen);

        public static Color DarkKhaki => new(KnownColor.DarkKhaki);

        public static Color DarkMagenta => new(KnownColor.DarkMagenta);

        public static Color DarkOliveGreen => new(KnownColor.DarkOliveGreen);

        public static Color DarkOrange => new(KnownColor.DarkOrange);

        public static Color DarkOrchid => new(KnownColor.DarkOrchid);

        public static Color DarkRed => new(KnownColor.DarkRed);

        public static Color DarkSalmon => new(KnownColor.DarkSalmon);

        public static Color DarkSeaGreen => new(KnownColor.DarkSeaGreen);

        public static Color DarkSlateBlue => new(KnownColor.DarkSlateBlue);

        public static Color DarkSlateGray => new(KnownColor.DarkSlateGray);

        public static Color DarkTurquoise => new(KnownColor.DarkTurquoise);

        public static Color DarkViolet => new(KnownColor.DarkViolet);

        public static Color DeepPink => new(KnownColor.DeepPink);

        public static Color DeepSkyBlue => new(KnownColor.DeepSkyBlue);

        public static Color DimGray => new(KnownColor.DimGray);

        public static Color DodgerBlue => new(KnownColor.DodgerBlue);

        public static Color Firebrick => new(KnownColor.Firebrick);

        public static Color FloralWhite => new(KnownColor.FloralWhite);

        public static Color ForestGreen => new(KnownColor.ForestGreen);

        public static Color Fuchsia => new(KnownColor.Fuchsia);

        public static Color Gainsboro => new(KnownColor.Gainsboro);

        public static Color GhostWhite => new(KnownColor.GhostWhite);

        public static Color Gold => new(KnownColor.Gold);

        public static Color Goldenrod => new(KnownColor.Goldenrod);

        public static Color Gray => new(KnownColor.Gray);

        public static Color Green => new(KnownColor.Green);

        public static Color GreenYellow => new(KnownColor.GreenYellow);

        public static Color Honeydew => new(KnownColor.Honeydew);

        public static Color HotPink => new(KnownColor.HotPink);

        public static Color IndianRed => new(KnownColor.IndianRed);

        public static Color Indigo => new(KnownColor.Indigo);

        public static Color Ivory => new(KnownColor.Ivory);

        public static Color Khaki => new(KnownColor.Khaki);

        public static Color Lavender => new(KnownColor.Lavender);

        public static Color LavenderBlush => new(KnownColor.LavenderBlush);

        public static Color LawnGreen => new(KnownColor.LawnGreen);

        public static Color LemonChiffon => new(KnownColor.LemonChiffon);

        public static Color LightBlue => new(KnownColor.LightBlue);

        public static Color LightCoral => new(KnownColor.LightCoral);

        public static Color LightCyan => new(KnownColor.LightCyan);

        public static Color LightGoldenrodYellow => new(KnownColor.LightGoldenrodYellow);

        public static Color LightGreen => new(KnownColor.LightGreen);

        public static Color LightGray => new(KnownColor.LightGray);

        public static Color LightPink => new(KnownColor.LightPink);

        public static Color LightSalmon => new(KnownColor.LightSalmon);

        public static Color LightSeaGreen => new(KnownColor.LightSeaGreen);

        public static Color LightSkyBlue => new(KnownColor.LightSkyBlue);

        public static Color LightSlateGray => new(KnownColor.LightSlateGray);

        public static Color LightSteelBlue => new(KnownColor.LightSteelBlue);

        public static Color LightYellow => new(KnownColor.LightYellow);

        public static Color Lime => new(KnownColor.Lime);

        public static Color LimeGreen => new(KnownColor.LimeGreen);

        public static Color Linen => new(KnownColor.Linen);

        public static Color Magenta => new(KnownColor.Magenta);

        public static Color Maroon => new(KnownColor.Maroon);

        public static Color MediumAquamarine => new(KnownColor.MediumAquamarine);

        public static Color MediumBlue => new(KnownColor.MediumBlue);

        public static Color MediumOrchid => new(KnownColor.MediumOrchid);

        public static Color MediumPurple => new(KnownColor.MediumPurple);

        public static Color MediumSeaGreen => new(KnownColor.MediumSeaGreen);

        public static Color MediumSlateBlue => new(KnownColor.MediumSlateBlue);

        public static Color MediumSpringGreen => new(KnownColor.MediumSpringGreen);

        public static Color MediumTurquoise => new(KnownColor.MediumTurquoise);

        public static Color MediumVioletRed => new(KnownColor.MediumVioletRed);

        public static Color MidnightBlue => new(KnownColor.MidnightBlue);

        public static Color MintCream => new(KnownColor.MintCream);

        public static Color MistyRose => new(KnownColor.MistyRose);

        public static Color Moccasin => new(KnownColor.Moccasin);

        public static Color NavajoWhite => new(KnownColor.NavajoWhite);

        public static Color Navy => new(KnownColor.Navy);

        public static Color OldLace => new(KnownColor.OldLace);

        public static Color Olive => new(KnownColor.Olive);

        public static Color OliveDrab => new(KnownColor.OliveDrab);

        public static Color Orange => new(KnownColor.Orange);

        public static Color OrangeRed => new(KnownColor.OrangeRed);

        public static Color Orchid => new(KnownColor.Orchid);

        public static Color PaleGoldenrod => new(KnownColor.PaleGoldenrod);

        public static Color PaleGreen => new(KnownColor.PaleGreen);

        public static Color PaleTurquoise => new(KnownColor.PaleTurquoise);

        public static Color PaleVioletRed => new(KnownColor.PaleVioletRed);

        public static Color PapayaWhip => new(KnownColor.PapayaWhip);

        public static Color PeachPuff => new(KnownColor.PeachPuff);

        public static Color Peru => new(KnownColor.Peru);

        public static Color Pink => new(KnownColor.Pink);

        public static Color Plum => new(KnownColor.Plum);

        public static Color PowderBlue => new(KnownColor.PowderBlue);

        public static Color Purple => new(KnownColor.Purple);

        /// <summary>
        /// Gets a system-defined color that has an ARGB value of <c>#663399</c>.
        /// </summary>
        /// <value>A system-defined color.</value>
        public static Color RebeccaPurple => new(KnownColor.RebeccaPurple);

        public static Color Red => new(KnownColor.Red);

        public static Color RosyBrown => new(KnownColor.RosyBrown);

        public static Color RoyalBlue => new(KnownColor.RoyalBlue);

        public static Color SaddleBrown => new(KnownColor.SaddleBrown);

        public static Color Salmon => new(KnownColor.Salmon);

        public static Color SandyBrown => new(KnownColor.SandyBrown);

        public static Color SeaGreen => new(KnownColor.SeaGreen);

        public static Color SeaShell => new(KnownColor.SeaShell);

        public static Color Sienna => new(KnownColor.Sienna);

        public static Color Silver => new(KnownColor.Silver);

        public static Color SkyBlue => new(KnownColor.SkyBlue);

        public static Color SlateBlue => new(KnownColor.SlateBlue);

        public static Color SlateGray => new(KnownColor.SlateGray);

        public static Color Snow => new(KnownColor.Snow);

        public static Color SpringGreen => new(KnownColor.SpringGreen);

        public static Color SteelBlue => new(KnownColor.SteelBlue);

        public static Color Tan => new(KnownColor.Tan);

        public static Color Teal => new(KnownColor.Teal);

        public static Color Thistle => new(KnownColor.Thistle);

        public static Color Tomato => new(KnownColor.Tomato);

        public static Color Turquoise => new(KnownColor.Turquoise);

        public static Color Violet => new(KnownColor.Violet);

        public static Color Wheat => new(KnownColor.Wheat);

        public static Color White => new(KnownColor.White);

        public static Color WhiteSmoke => new(KnownColor.WhiteSmoke);

        public static Color Yellow => new(KnownColor.Yellow);

        public static Color YellowGreen => new(KnownColor.YellowGreen);
        //
        //  end "web" colors
        // -------------------------------------------------------------------

        // NOTE : The "zero" pattern (all members being 0) must represent
        //      : "not set". This allows "Color c;" to be correct.

        private const short StateKnownColorValid = 0x0001;
        private const short StateARGBValueValid = 0x0002;
        private const short StateValueMask = StateARGBValueValid;
        private const short StateNameValid = 0x0008;
        private const long NotDefinedValue = 0;

        // Shift counts and bit masks for A, R, G, B components in ARGB mode

        internal const int ARGBAlphaShift = 24;
        internal const int ARGBRedShift = 16;
        internal const int ARGBGreenShift = 8;
        internal const int ARGBBlueShift = 0;
        internal const uint ARGBAlphaMask = 0xFFu << ARGBAlphaShift;
        internal const uint ARGBRedMask = 0xFFu << ARGBRedShift;
        internal const uint ARGBGreenMask = 0xFFu << ARGBGreenShift;
        internal const uint ARGBBlueMask = 0xFFu << ARGBBlueShift;

        // User supplied name of color. Will not be filled in if
        // we map to a "knowncolor"
        private readonly string? name; // Do not rename (binary serialization)

        // Standard 32bit sRGB (ARGB)
        private readonly long value; // Do not rename (binary serialization)

        // Ignored, unless "state" says it is valid
        private readonly short knownColor; // Do not rename (binary serialization)

        // State flags.
        private readonly short state; // Do not rename (binary serialization)

        internal Color(KnownColor knownColor)
        {
            value = 0;
            state = StateKnownColorValid;
            name = null;
            this.knownColor = unchecked((short)knownColor);
        }

        private Color(long value, short state, string? name, KnownColor knownColor)
        {
            this.value = value;
            this.state = state;
            this.name = name;
            this.knownColor = unchecked((short)knownColor);
        }

        public byte R => unchecked((byte)(Value >> ARGBRedShift));

        public byte G => unchecked((byte)(Value >> ARGBGreenShift));

        public byte B => unchecked((byte)(Value >> ARGBBlueShift));

        public byte A => unchecked((byte)(Value >> ARGBAlphaShift));

        public bool IsKnownColor => (state & StateKnownColorValid) != 0;

        public bool IsEmpty => state == 0;

        public bool IsNamedColor => ((state & StateNameValid) != 0) || IsKnownColor;

        public bool IsSystemColor => IsKnownColor && IsKnownColorSystem((KnownColor)knownColor);

        internal static bool IsKnownColorSystem(KnownColor knownColor)
            => KnownColorTable.ColorKindTable[(int)knownColor] == KnownColorTable.KnownColorKindSystem;

        // Used for the [DebuggerDisplay]. Inlining in the attribute is possible, but
        // against best practices as the current project language parses the string with
        // language specific heuristics.

        private string NameAndARGBValue => $"{{Name = {Name}, ARGB = ({A}, {R}, {G}, {B})}}";

        public string Name
        {
            get
            {
                if ((state & StateNameValid) != 0)
                {
                    Debug.Assert(name != null);
                    return name;
                }

                if (IsKnownColor)
                {
                    var tablename = KnownColorNames.KnownColorToName((KnownColor)knownColor);
                    Debug.Assert(tablename != null, $"Could not find known color '{(KnownColor)knownColor}' in the KnownColorTable");

                    return tablename;
                }

                // if we reached here, just encode the value
                //
                return value.ToString("x");
            }
        }

        private long Value
        {
            get
            {
                if ((state & StateValueMask) != 0)
                {
                    return value;
                }

                // This is the only place we have system colors value exposed
                if (IsKnownColor)
                {
                    return KnownColorTable.KnownColorToArgb((KnownColor)knownColor);
                }

                return NotDefinedValue;
            }
        }

        private static void CheckByte(int value, string name)
        {
            static void ThrowOutOfByteRange(int v, string n) =>
                throw new ArgumentException(string.Format("SR.InvalidEx2BoundArgument {0} {1} {2} {3}", n, v, byte.MinValue, byte.MaxValue));

            if (unchecked((uint)value) > byte.MaxValue)
                ThrowOutOfByteRange(value, name);
        }

        private static Color FromArgb(uint argb) => new(argb, StateARGBValueValid, null, (KnownColor)0);

        public static Color FromArgb(int argb) => FromArgb(unchecked((uint)argb));

        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            CheckByte(alpha, nameof(alpha));
            CheckByte(red, nameof(red));
            CheckByte(green, nameof(green));
            CheckByte(blue, nameof(blue));

            return FromArgb(
                (uint)alpha << ARGBAlphaShift |
                (uint)red << ARGBRedShift |
                (uint)green << ARGBGreenShift |
                (uint)blue << ARGBBlueShift
            );
        }

        public static Color FromArgb(int alpha, Color baseColor)
        {
            CheckByte(alpha, nameof(alpha));

            return FromArgb(
                (uint)alpha << ARGBAlphaShift |
                (uint)baseColor.Value & ~ARGBAlphaMask
            );
        }

        public static Color FromArgb(int red, int green, int blue) => FromArgb(byte.MaxValue, red, green, blue);

        public static Color FromKnownColor(KnownColor color) =>
            color <= 0 || color > KnownColor.RebeccaPurple ? FromName(color.ToString()) : new Color(color);

#if NET462_OR_GREATER
        public static Color FromName(string name)
#else
        public static Color FromName(string name)
#endif
        {
            // try to get a known color first
            if (ColorTable.TryGetNamedColor(name, out var color))
#if NET462_OR_GREATER
                return color;
#else
                return FromArgb(color.ToArgb());
#endif

            // otherwise treat it as a named color
            return new Color(NotDefinedValue, StateNameValid, name, (KnownColor)0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetRgbValues(out int r, out int g, out int b)
        {
            var value = (uint)Value;
            r = (int)(value & ARGBRedMask) >> ARGBRedShift;
            g = (int)(value & ARGBGreenMask) >> ARGBGreenShift;
            b = (int)(value & ARGBBlueMask) >> ARGBBlueShift;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MinMaxRgb(out int min, out int max, int r, int g, int b)
        {
            if (r > g)
            {
                max = r;
                min = g;
            }
            else
            {
                max = g;
                min = r;
            }
            if (b > max)
            {
                max = b;
            }
            else if (b < min)
            {
                min = b;
            }
        }

        public float GetBrightness()
        {
            GetRgbValues(out var r, out var g, out var b);

            MinMaxRgb(out var min, out var max, r, g, b);

            return (max + min) / (byte.MaxValue * 2f);
        }

        public float GetHue()
        {
            GetRgbValues(out var r, out var g, out var b);

            if (r == g && g == b)
                return 0f;

            MinMaxRgb(out var min, out var max, r, g, b);

            float delta = max - min;
            float hue;

            if (r == max)
                hue = (g - b) / delta;
            else if (g == max)
                hue = (b - r) / delta + 2f;
            else
                hue = (r - g) / delta + 4f;

            hue *= 60f;
            if (hue < 0f)
                hue += 360f;

            return hue;
        }

        public float GetSaturation()
        {
            GetRgbValues(out var r, out var g, out var b);

            if (r == g && g == b)
                return 0f;

            MinMaxRgb(out var min, out var max, r, g, b);

            var div = max + min;
            if (div > byte.MaxValue)
                div = byte.MaxValue * 2 - max - min;

            return (max - min) / (float)div;
        }

        public int ToArgb() => unchecked((int)Value);

        public KnownColor ToKnownColor() => (KnownColor)knownColor;

        public override string ToString() =>
            IsNamedColor ? $"{nameof(Color)} [{Name}]" :
            (state & StateValueMask) != 0 ? $"{nameof(Color)} [A={A}, R={R}, G={G}, B={B}]" :
            $"{nameof(Color)} [Empty]";

        public static bool operator ==(Color left, Color right) =>
            left.value == right.value
                && left.state == right.state
                && left.knownColor == right.knownColor
                && left.name == right.name;

        public static bool operator !=(Color left, Color right) => !(left == right);

        public override bool Equals(object? obj) => obj is Color other && Equals(other);

        public bool Equals(Color other) => this == other;

        public override int GetHashCode()
        {
            // Three cases:
            // 1. We don't have a name. All relevant data, including this fact, is in the remaining fields.
            // 2. We have a known name. The name will be the same instance of any other with the same
            // knownColor value, so we can ignore it for hashing. Note this also hashes different to
            // an unnamed color with the same ARGB value.
            // 3. Have an unknown name. Will differ from other unknown-named colors only by name, so we
            // can usefully use the names hash code alone.
            if (name != null && !IsKnownColor)
                return name.GetHashCode();

            return HashCode.Combine(value.GetHashCode(), state.GetHashCode(), knownColor.GetHashCode());
        }
    }
}
