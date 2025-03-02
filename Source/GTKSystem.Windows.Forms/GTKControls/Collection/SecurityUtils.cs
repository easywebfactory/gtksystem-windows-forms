using System.Reflection;
using System.Security;

namespace System.Windows.Forms;

internal static class SecurityUtils
{
    private static bool HasReflectionPermission(Type? _)
    {
        bool flag;
        try
        {
            flag = true;
        }
        catch (SecurityException)
        {
            return false;
        }
        return flag;
    }

    internal static object? SecureCreateInstance(Type? type)
    {
        return SecureCreateInstance(type, null, false);
    }

    internal static object? SecureCreateInstance(Type? type, object[]? args, bool allowNonPublic)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }
        var bindingFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
        if (!type.IsVisible)
        {
        }
        else if (allowNonPublic && !HasReflectionPermission(type))
        {
            allowNonPublic = false;
        }
        if (allowNonPublic)
        {
            bindingFlag |= BindingFlags.NonPublic;
        }
        return Activator.CreateInstance(type, bindingFlag, null, args, null);
    }
}