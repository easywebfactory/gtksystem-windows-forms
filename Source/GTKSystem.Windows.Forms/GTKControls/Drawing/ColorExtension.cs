namespace System.Drawing;

internal static class ColorExtension
{
    public static Color FromName(string name)
    {
        // try to get a known color first
        if (ColorTable.TryGetNamedColor(name, out var color))
            return color;

        // otherwise treat it as a named color
        return FromName(name);
    }
}