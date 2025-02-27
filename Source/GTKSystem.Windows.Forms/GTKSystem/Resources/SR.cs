using System.Diagnostics;
using System.Runtime.CompilerServices;
//using System.Resources;

namespace System.Windows.Forms.Resources;

internal static class SR
{
    private static ResourceManager? resourceManager;

    internal static ResourceManager ResourceManager => resourceManager ?? (resourceManager = new ResourceManager(typeof(System.Windows.Forms.Resources.Sr)));

    internal static string ArgumentOutOfRangeStreamLength => GetResourceString("ArgumentOutOfRange_StreamLength");

    internal static string ArgumentStreamNotReadable => GetResourceString("Argument_StreamNotReadable");

    internal static string ArgumentStreamNotWritable => GetResourceString("Argument_StreamNotWritable");

    internal static string ArgResourceFileUnsupportedVersion => GetResourceString("Arg_ResourceFileUnsupportedVersion");

    internal static string BadImageFormatInvalidType => GetResourceString("BadImageFormat_InvalidType");

    internal static string BadImageFormatNegativeStringLength => GetResourceString("BadImageFormat_NegativeStringLength");

    internal static string BadImageFormatResourceDataLengthInvalid => GetResourceString("BadImageFormat_ResourceDataLengthInvalid");

    internal static string BadImageFormatResourceNameCorrupted => GetResourceString("BadImageFormat_ResourceNameCorrupted");

    internal static string BadImageFormatResourceNameCorruptedNameIndex => GetResourceString("BadImageFormat_ResourceNameCorrupted_NameIndex");

    internal static string BadImageFormatResourcesDataInvalidOffset => GetResourceString("BadImageFormat_ResourcesDataInvalidOffset");

    internal static string BadImageFormatResourcesHeaderCorrupted => GetResourceString("BadImageFormat_ResourcesHeaderCorrupted");

    internal static string BadImageFormatResourcesIndexTooLong => GetResourceString("BadImageFormat_ResourcesIndexTooLong");

    internal static string BadImageFormatResourcesNameInvalidOffset => GetResourceString("BadImageFormat_ResourcesNameInvalidOffset");

    internal static string BadImageFormatResourcesNameTooLong => GetResourceString("BadImageFormat_ResourcesNameTooLong");

    internal static string BadImageFormatResTypeSerBlobMismatch => GetResourceString("BadImageFormat_ResType_SerBlobMismatch");

    internal static string BadImageFormatTypeMismatch => GetResourceString("BadImageFormat_TypeMismatch");

    internal static string FormatBad7BitInt32 => GetResourceString("Format_Bad7BitInt32");

    internal static string InvalidOperationEnumEnded => GetResourceString("InvalidOperation_EnumEnded");

    internal static string InvalidOperationEnumNotStarted => GetResourceString("InvalidOperation_EnumNotStarted");

    internal static string InvalidOperationResourceNotStringType => GetResourceString("InvalidOperation_ResourceNotString_Type");

    internal static string InvalidOperationResourceWriterSaved => GetResourceString("InvalidOperation_ResourceWriterSaved");

    internal static string NotSupportedBinarySerializedResources => GetResourceString("NotSupported_BinarySerializedResources");

    internal static string NotSupportedResourceObjectSerialization => GetResourceString("NotSupported_ResourceObjectSerialization");

    internal static string NotSupportedUnseekableStream => GetResourceString("NotSupported_UnseekableStream");

    internal static string NotSupportedWrongResourceReaderType => GetResourceString("NotSupported_WrongResourceReader_Type");

    internal static string ObjectDisposedResourceSet => GetResourceString("ObjectDisposed_ResourceSet");

    internal static string ResourceReaderIsClosed => GetResourceString("ResourceReaderIsClosed");

    internal static string ResourcesStreamNotValid => GetResourceString("Resources_StreamNotValid");

    internal static string TypeLoadExceptionCannotLoadConverter => GetResourceString("TypeLoadException_CannotLoadConverter");

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool UsingResourceKeys()
    {
        return false;
    }

    internal static string GetResourceString(string resourceKey, string? defaultString = null)
    {
        if (UsingResourceKeys())
        {
            return defaultString ?? resourceKey;
        }
        string? text = null;
        try
        {
            text = ResourceManager.GetString(resourceKey);
        }
        catch (Exception ex)
        {
            Trace.Write(ex);
        }
        if (defaultString != null && resourceKey.Equals(text))
        {
            return defaultString;
        }
        return text ?? string.Empty;
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

    internal static string Format(string resourceFormat, params object[] args)
    {
        if (args != null)
        {
            if (UsingResourceKeys())
            {
                return resourceFormat + ", " + string.Join(", ", args);
            }
            return string.Format(resourceFormat, args);
        }
        return resourceFormat;
    }

    internal static string Format(IFormatProvider provider, string resourceFormat, object? p1)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1);
        }
        return string.Format(provider, resourceFormat, p1);
    }

    internal static string Format(IFormatProvider provider, string resourceFormat, object? p1, object? p2)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1, p2);
        }
        return string.Format(provider, resourceFormat, p1, p2);
    }

    internal static string Format(IFormatProvider provider, string resourceFormat, object? p1, object? p2, object? p3)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1, p2, p3);
        }
        return string.Format(provider, resourceFormat, p1, p2, p3);
    }

    internal static string Format(IFormatProvider provider, string resourceFormat, params object[] args)
    {
        if (args != null)
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