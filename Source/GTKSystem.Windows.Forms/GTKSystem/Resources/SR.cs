using System;
//using System.Resources;
using System.Runtime.CompilerServices;
using GTKSystem.Resources;

namespace GTKSystem
{
    internal static class SR
    {
        private static ResourceManager s_resourceManager;

        internal static ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager =
            new GTKSystem.Resources.ResourceManager(typeof(FxResources.System.Resources.Extensions.SR)));

        internal static string ArgumentOutOfRange_StreamLength => GetResourceString("ArgumentOutOfRange_StreamLength");

        internal static string Argument_StreamNotReadable => GetResourceString("Argument_StreamNotReadable");

        internal static string Argument_StreamNotWritable => GetResourceString("Argument_StreamNotWritable");

        internal static string Arg_ResourceFileUnsupportedVersion =>
            GetResourceString("Arg_ResourceFileUnsupportedVersion");

        internal static string BadImageFormat_InvalidType => GetResourceString("BadImageFormat_InvalidType");

        internal static string BadImageFormat_NegativeStringLength =>
            GetResourceString("BadImageFormat_NegativeStringLength");

        internal static string BadImageFormat_ResourceDataLengthInvalid =>
            GetResourceString("BadImageFormat_ResourceDataLengthInvalid");

        internal static string BadImageFormat_ResourceNameCorrupted =>
            GetResourceString("BadImageFormat_ResourceNameCorrupted");

        internal static string BadImageFormat_ResourceNameCorrupted_NameIndex =>
            GetResourceString("BadImageFormat_ResourceNameCorrupted_NameIndex");

        internal static string BadImageFormat_ResourcesDataInvalidOffset =>
            GetResourceString("BadImageFormat_ResourcesDataInvalidOffset");

        internal static string BadImageFormat_ResourcesHeaderCorrupted =>
            GetResourceString("BadImageFormat_ResourcesHeaderCorrupted");

        internal static string BadImageFormat_ResourcesIndexTooLong =>
            GetResourceString("BadImageFormat_ResourcesIndexTooLong");

        internal static string BadImageFormat_ResourcesNameInvalidOffset =>
            GetResourceString("BadImageFormat_ResourcesNameInvalidOffset");

        internal static string BadImageFormat_ResourcesNameTooLong =>
            GetResourceString("BadImageFormat_ResourcesNameTooLong");

        internal static string BadImageFormat_ResType_SerBlobMismatch =>
            GetResourceString("BadImageFormat_ResType_SerBlobMismatch");

        internal static string BadImageFormat_TypeMismatch => GetResourceString("BadImageFormat_TypeMismatch");

        internal static string Format_Bad7BitInt32 => GetResourceString("Format_Bad7BitInt32");

        internal static string InvalidOperation_EnumEnded => GetResourceString("InvalidOperation_EnumEnded");

        internal static string InvalidOperation_EnumNotStarted => GetResourceString("InvalidOperation_EnumNotStarted");

        internal static string InvalidOperation_ResourceNotString_Type =>
            GetResourceString("InvalidOperation_ResourceNotString_Type");

        internal static string InvalidOperation_ResourceWriterSaved =>
            GetResourceString("InvalidOperation_ResourceWriterSaved");

        internal static string NotSupported_BinarySerializedResources =>
            GetResourceString("NotSupported_BinarySerializedResources");

        internal static string NotSupported_ResourceObjectSerialization =>
            GetResourceString("NotSupported_ResourceObjectSerialization");

        internal static string NotSupported_UnseekableStream => GetResourceString("NotSupported_UnseekableStream");

        internal static string NotSupported_WrongResourceReader_Type =>
            GetResourceString("NotSupported_WrongResourceReader_Type");

        internal static string ObjectDisposed_ResourceSet => GetResourceString("ObjectDisposed_ResourceSet");

        internal static string ResourceReaderIsClosed => GetResourceString("ResourceReaderIsClosed");

        internal static string Resources_StreamNotValid => GetResourceString("Resources_StreamNotValid");

        internal static string TypeLoadException_CannotLoadConverter =>
            GetResourceString("TypeLoadException_CannotLoadConverter");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool UsingResourceKeys()
        {
            return false;
        }

        internal static string GetResourceString(string resourceKey, string defaultString = null)
        {
            if (UsingResourceKeys())
            {
                return defaultString ?? resourceKey;
            }

            string text = null;
            try
            {
                text = ResourceManager.GetString(resourceKey);
            }
            catch
            {
            }

            if (defaultString != null && resourceKey.Equals(text))
            {
                return defaultString;
            }

            return text;
        }

        internal static string Format(string resourceFormat, object p1)
        {
            if (UsingResourceKeys())
            {
                return string.Join(", ", resourceFormat, p1);
            }

            return string.Format(resourceFormat, p1);
        }

        internal static string Format(string resourceFormat, object p1, object p2)
        {
            if (UsingResourceKeys())
            {
                return string.Join(", ", resourceFormat, p1, p2);
            }

            return string.Format(resourceFormat, p1, p2);
        }

        internal static string Format(string resourceFormat, object p1, object p2, object p3)
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

        internal static string Format(IFormatProvider provider, string resourceFormat, object p1)
        {
            if (UsingResourceKeys())
            {
                return string.Join(", ", resourceFormat, p1);
            }

            return string.Format(provider, resourceFormat, p1);
        }

        internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2)
        {
            if (UsingResourceKeys())
            {
                return string.Join(", ", resourceFormat, p1, p2);
            }

            return string.Format(provider, resourceFormat, p1, p2);
        }

        internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2, object p3)
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
}