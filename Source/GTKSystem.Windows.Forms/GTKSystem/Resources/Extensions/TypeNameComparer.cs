using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms.Numerics.Hashing;

namespace System.Windows.Forms.Resources;

internal sealed class TypeNameComparer : IEqualityComparer<string>
{
    private static readonly char[] whiteSpaceChars = [' ', '\n', '\r', '\t'];

    public static TypeNameComparer Instance { get; } = new();


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ReadOnlySpan<char> ReadTypeName(ReadOnlySpan<char> assemblyQualifiedTypeName)
    {
        var num = assemblyQualifiedTypeName.IndexOf(',');
        if (num != -1)
        {
            return assemblyQualifiedTypeName.Slice(0, num);
        }
        return assemblyQualifiedTypeName;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ReadOnlySpan<char> ReadAssemblySimpleName(ReadOnlySpan<char> assemblyName)
    {
        var num = assemblyName.IndexOf(',');
        if (num != -1)
        {
            return assemblyName.Slice(0, num).TrimEnd(whiteSpaceChars);
        }
        return assemblyName;
    }

    private static bool IsMscorlib(ReadOnlySpan<char> assemblyName)
    {
        return assemblyName.Equals("mscorlib".AsSpan(), StringComparison.OrdinalIgnoreCase);
    }

    public bool Equals(string? assemblyQualifiedTypeName1, string assemblyQualifiedTypeName2)
    {
        if (assemblyQualifiedTypeName1 == null)
        {
            throw new ArgumentNullException("assemblyQualifiedTypeName1");
        }
        if (assemblyQualifiedTypeName2 == null)
        {
            throw new ArgumentNullException("assemblyQualifiedTypeName2");
        }
        if ((object)assemblyQualifiedTypeName1 == assemblyQualifiedTypeName2)
        {
            return true;
        }
        var readOnlySpan = assemblyQualifiedTypeName1.AsSpan().TrimStart(whiteSpaceChars);
        var readOnlySpan2 = assemblyQualifiedTypeName2.AsSpan().TrimStart(whiteSpaceChars);
        var span = ReadTypeName(readOnlySpan);
        var other = ReadTypeName(readOnlySpan2);
        if (!span.Equals(other, StringComparison.Ordinal))
        {
            return false;
        }
        readOnlySpan = readOnlySpan.Length > span.Length ? readOnlySpan.Slice(span.Length + 1).TrimStart(whiteSpaceChars) : ReadOnlySpan<char>.Empty;
        readOnlySpan2 = readOnlySpan2.Length > other.Length ? readOnlySpan2.Slice(other.Length + 1).TrimStart(whiteSpaceChars) : ReadOnlySpan<char>.Empty;
        var readOnlySpan3 = ReadAssemblySimpleName(readOnlySpan);
        var readOnlySpan4 = ReadAssemblySimpleName(readOnlySpan2);
        if ((readOnlySpan3.IsEmpty && !readOnlySpan.IsEmpty) || (readOnlySpan4.IsEmpty && !readOnlySpan2.IsEmpty))
        {
            return false;
        }
        if (readOnlySpan3.IsEmpty)
        {
            if (!readOnlySpan4.IsEmpty)
            {
                return IsMscorlib(readOnlySpan4);
            }
            return true;
        }
        if (readOnlySpan4.IsEmpty)
        {
            return IsMscorlib(readOnlySpan3);
        }
        if (!readOnlySpan3.Equals(readOnlySpan4, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        if (IsMscorlib(readOnlySpan3))
        {
            return true;
        }
        var assemblyName = new AssemblyName(readOnlySpan.ToString());
        var assemblyName2 = new AssemblyName(readOnlySpan2.ToString());
        if (assemblyName.CultureInfo?.LCID != assemblyName2.CultureInfo?.LCID)
        {
            return false;
        }
        var publicKeyToken = assemblyName.GetPublicKeyToken();
        var publicKeyToken2 = assemblyName2.GetPublicKeyToken();
        return publicKeyToken.AsSpan().SequenceEqual(publicKeyToken2);
    }

    public int GetHashCode(string assemblyQualifiedTypeName)
    {
        var assemblyQualifiedTypeName2 = assemblyQualifiedTypeName.AsSpan().TrimStart(whiteSpaceChars);
        var readOnlySpan = ReadTypeName(assemblyQualifiedTypeName2);
        var num = 0;
        for (var i = 0; i < readOnlySpan.Length; i++)
        {
            var h = num;
            var c = readOnlySpan[i];
            num = HashHelpers.Combine(h, c.GetHashCode());
        }
        return num;
    }
}