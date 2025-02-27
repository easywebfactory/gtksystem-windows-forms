using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace System.Windows.Forms.Resources;

internal sealed class FastResourceComparer : IComparer, IEqualityComparer, IComparer<string>, IEqualityComparer<string>
{
    internal static readonly FastResourceComparer @default = new();

    public int GetHashCode(object? key)
    {
        var key2 = key as string;
        return HashFunction(key2);
    }

    public int GetHashCode([DisallowNull] string? key)
    {
        return HashFunction(key);
    }

    internal static int HashFunction(string? key)
    {
        var num = 5381u;
        for (var i = 0; i < key?.Length; i++)
        {
            num = ((num << 5) + num) ^ key[i];
        }
        return (int)num;
    }

    public int Compare(object? a, object? b)
    {
        if (a == b)
        {
            return 0;
        }
        var strA = a as string;
        var strB = b as string;
        return string.CompareOrdinal(strA, strB);
    }

    public int Compare(string a, string b)
    {
        return string.CompareOrdinal(a, b);
    }

    public bool Equals(string a, string b)
    {
        return string.Equals(a, b);
    }

    public new bool Equals(object? a, object? b)
    {
        if (a == b)
        {
            return true;
        }
        var a2 = a as string;
        var b2 = b as string;
        return string.Equals(a2, b2);
    }

    public static unsafe int CompareOrdinal(string? a, byte[] bytes, int bCharLength)
    {
        var num = 0;
        var num2 = 0;
        var num3 = a?.Length??0;
        if (num3 > bCharLength)
        {
            num3 = bCharLength;
        }
        if (bCharLength == 0)
        {
            if ((a?.Length??0) != 0)
            {
                return -1;
            }
            return 0;
        }
        fixed (byte* ptr = bytes)
        {
            var ptr2 = ptr;
            while (num < num3 && num2 == 0)
            {
                var num4 = *ptr2 | (ptr2[1] << 8);
                if (a != null)
                {
                    num2 = a[num++] - num4;
                }

                ptr2 += 2;
            }
        }
        if (num2 != 0)
        {
            return num2;
        }
        return a?.Length ?? 0 - bCharLength;
    }

    public static int CompareOrdinal(byte[] bytes, int aCharLength, string? b)
    {
        return -CompareOrdinal(b, bytes, aCharLength);
    }

    internal static unsafe int CompareOrdinal(byte* a, int byteLen, string? b)
    {
        var num = 0;
        var num2 = 0;
        var num3 = byteLen >> 1;
        if (num3 > (b?.Length??0))
        {
            num3 = b?.Length ?? 0;
        }
        while (num2 < num3 && num == 0)
        {
            var c = (char)(*a++ | (*a++ << 8));
            if (b != null)
            {
                num = c - b[num2++];
            }
        }
        if (num != 0)
        {
            return num;
        }
        return byteLen - (b?.Length ?? 0) * 2;
    }
}