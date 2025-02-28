using System.Resources;

namespace System.Windows.Forms.Resources;

internal static class ResXExtensions
{
    public static string? OrThrowIfNullOrEmpty(this string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("string Is Null Or Empty");
        }

        return value;
    }
    public static string OrThrowIfNull(this string? value)
    {
        if (value == null)
        {
            throw new ArgumentException("string Is Null Or Empty");
        }

        return value;
    }
    public static ResXFileRef OrThrowIfNull(this ResXFileRef? value)
    {
        if (value == null)
        {
            throw new ArgumentException("string is null");
        }

        return value;
    }

}