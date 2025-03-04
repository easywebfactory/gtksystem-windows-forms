// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace System.Windows.Forms.Resources;

public partial class ResXFileRef
{
    public class Converter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object? ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
            object? created = null;
            if (destinationType == typeof(string))
            {
                created = ((ResXFileRef)value).ToString();
            }

            return created;
        }

        // "value" is the parameter name of ConvertFrom, which calls this method.
        internal static string?[]? ParseResxFileRefString(string? stringValue)
        {
            string?[]? result = null;
            if (stringValue != null)
            {
                stringValue = stringValue.Trim();
                string? fileName;
                string remainingString;
                if (stringValue.StartsWith("\""))
                {
                    var lastIndexOfQuote = stringValue.LastIndexOf('\"');
                    if (lastIndexOfQuote - 1 < 0)
                    {
                        throw new ArgumentException(nameof(stringValue));
                    }

                    fileName = stringValue.Substring(1, lastIndexOfQuote - 1); // remove the quotes in" ..... "
                    if (lastIndexOfQuote + 2 > stringValue.Length)
                    {
                        throw new ArgumentException(nameof(stringValue));
                    }

                    remainingString = stringValue.Substring(lastIndexOfQuote + 2);
                }
                else
                {
                    var nextSemiColumn = stringValue.IndexOf(';');
                    if (nextSemiColumn == -1)
                    {
                        throw new ArgumentException(nameof(stringValue));
                    }

                    fileName = stringValue.Substring(0, nextSemiColumn);
                    if (nextSemiColumn + 1 > stringValue.Length)
                    {
                        throw new ArgumentException(nameof(stringValue));
                    }

                    remainingString = stringValue.Substring(nextSemiColumn + 1);
                }

                var parts = remainingString.Split(';');
                if (parts.Length > 1)
                {
                    result = [fileName, parts[0], parts[1]];
                }
                else if (parts.Length > 0)
                {
                    result = [fileName, parts[0]];
                }
                else
                {
                    result = [fileName];
                }
            }

            return result;
        }

        public override object? ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value)
        {
            if (value is string stringValue)
            {
                var parts = ParseResxFileRefString(stringValue);
                var fileName = parts?[0];
                if (parts != null)
                {
                    var typeName = parts[1];
                    if (typeName != null)
                    {
                        var toCreate = Type.GetType(typeName, true);

                        // special case string and byte[]
                        if (toCreate == typeof(string))
                        {
                            // we have a string, now we need to check the encoding
                            var secondPart = SecondPart(parts);
                            if (secondPart != null)
                            {
                                var textFileEncoding =
                                    parts.Length > 2
                                        ? Encoding.GetEncoding(secondPart)
                                        : Encoding.Default;
                                if (fileName != null)
                                {
                                    using var sr = new StreamReader(fileName, textFileEncoding);
                                    return sr.ReadToEnd();
                                }
                            }
                        }

                        // this is a regular file, we call it's constructor with a stream as a parameter
                        // or if it's a byte array we just return that
                        byte[]? temp;

                        if (fileName != null)
                        {
                            using (var fileStream =
                                   new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                Debug.Assert(fileStream != null, "Couldn't open " + fileName);
                                temp = new byte[fileStream?.Length ?? 0];
                                _ = fileStream?.Read(temp, 0, (int)fileStream.Length);
                            }

                            if (toCreate == typeof(byte[]))
                            {
                                return temp;
                            }

                            var memStream = new MemoryStream(temp);
                            if (toCreate == typeof(MemoryStream))
                            {
                                return memStream;
                            }

                            if (toCreate == typeof(Bitmap) && fileName.EndsWith(".ico"))
                            {
                                // we special case the .ico bitmaps because GDI+ destroy the alpha channel component and
                                // we don't want that to happen
                                var ico = new Icon(memStream);
                                return ico.ToBitmap();
                            }

                            if (toCreate != null)
                            {
                                return Activator.CreateInstance(toCreate,
                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null,
                                    [memStream], null);
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static string? SecondPart(string?[] parts)
        {
            if (parts?.Length < 2)
            {
                return null;
            }
            return parts![2];
        }
    }
}