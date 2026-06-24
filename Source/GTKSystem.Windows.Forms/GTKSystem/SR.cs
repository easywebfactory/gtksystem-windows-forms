// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//using GTKSystem.Resources;
using System.Resources;

namespace System;

internal static partial class SR
{
    private static readonly bool s_usingResourceKeys = AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out bool usingResourceKeys) ? usingResourceKeys : true;
    private static ResourceManager s_resourceManager;
    internal static Resources.ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new System.Resources.ResourceManager(typeof(System.SR)));
    internal static bool UsingResourceKeys() => s_usingResourceKeys;

    internal static string GetResourceString(string resourceKey)
    {
        if (UsingResourceKeys())
        {
            return resourceKey;
        }

        string? resourceString = null;
        try
        {
            resourceString =
#if SYSTEM_PRIVATE_CORELIB || NATIVEAOT
                InternalGetResourceString(resourceKey);
#else
                ResourceManager.GetString(resourceKey);
#endif
        }
        catch (MissingManifestResourceException) { }

        return resourceString!; // only null if missing resources
    }

    internal static string GetResourceString(string resourceKey, string defaultString)
    {
        string resourceString = GetResourceString(resourceKey);

        return resourceKey == resourceString || resourceString is null ? defaultString : resourceString;
    }

    internal static string Format(string resourceFormat, object? p1)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1);
        }

        return string.Format(resourceFormat, p1);
    }

    internal static string Format(string resourceFormat, object? p1, object? p2)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1, p2);
        }

        return string.Format(resourceFormat, p1, p2);
    }

    internal static string Format(string resourceFormat, object? p1, object? p2, object? p3)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1, p2, p3);
        }

        return string.Format(resourceFormat, p1, p2, p3);
    }

    internal static string Format(string resourceFormat, params object?[]? args)
    {
        if (args is not null)
        {
            if (UsingResourceKeys())
            {
                return resourceFormat + ", " + string.Join(", ", args);
            }

            return string.Format(resourceFormat, args);
        }

        return resourceFormat;
    }

    internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1);
        }

        return string.Format(provider, resourceFormat, p1);
    }

    internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1, p2);
        }

        return string.Format(provider, resourceFormat, p1, p2);
    }

    internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2, object? p3)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1, p2, p3);
        }

        return string.Format(provider, resourceFormat, p1, p2, p3);
    }

    internal static string Format(IFormatProvider? provider, string resourceFormat, params object?[]? args)
    {
        if (args is not null)
        {
            if (UsingResourceKeys())
            {
                return resourceFormat + ", " + string.Join(", ", args);
            }

            return string.Format(provider, resourceFormat, args);
        }

        return resourceFormat;
    }
}
