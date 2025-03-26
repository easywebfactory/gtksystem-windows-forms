using System.ComponentModel;
using System.Resources;
using System.Resources.Extensions;
using System.Text;

namespace System.Resources;

public sealed class PreserializedResourceWriter : IResourceWriter
{
    private class PrecannedResource
    {
        internal readonly string typeName;

        internal readonly object data;

        internal PrecannedResource(string typeName, object data)
        {
            this.typeName = typeName;
            this.data = data;
        }
    }

    private class StreamWrapper
    {
        internal readonly Stream stream;

        internal readonly bool closeAfterWrite;

        internal StreamWrapper(Stream s, bool closeAfterWrite)
        {
            stream = s;
            this.closeAfterWrite = closeAfterWrite;
        }
    }

    private class ResourceDataRecord
    {
        internal readonly SerializationFormat format;

        internal readonly object data;

        internal readonly bool closeAfterWrite;

        internal ResourceDataRecord(SerializationFormat format, object data, bool closeAfterWrite = false)
        {
            this.format = format;
            this.data = data;
            this.closeAfterWrite = closeAfterWrite;
        }
    }

    private const int averageNameSize = 40;

    private const int averageValueSize = 40;

    internal const string resourceReaderFullyQualifiedName = "System.Resources.ResourceReader, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

    private const string resSetTypeName = "System.Resources.RuntimeResourceSet";

    private const int resSetVersion = 2;

    private readonly SortedDictionary<string?, object?> _resourceList;

    private Stream? _output;

    private readonly Dictionary<string, object?> _caseInsensitiveDups;

    private Dictionary<string, PrecannedResource>? _preserializedData;

    private bool _requiresDeserializingResourceReader;

    internal const string deserializingResourceReaderFullyQualifiedName = "System.Resources.Extensions.DeserializingResourceReader, System.Resources.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51";

    internal const string runtimeResourceSetFullyQualifiedName = "System.Resources.Extensions.RuntimeResourceSet, System.Resources.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51";

    private static readonly string unknownObjectTypeName = typeof(UnknownType).FullName!;

    private static readonly IReadOnlyDictionary<string, Type> primitiveTypes = new Dictionary<string, Type>(16, TypeNameComparer.Instance)
    {
        {
            typeof(string).FullName,
            typeof(string)
        },
        {
            typeof(int).FullName,
            typeof(int)
        },
        {
            typeof(bool).FullName,
            typeof(bool)
        },
        {
            typeof(char).FullName,
            typeof(char)
        },
        {
            typeof(byte).FullName,
            typeof(byte)
        },
        {
            typeof(sbyte).FullName,
            typeof(sbyte)
        },
        {
            typeof(short).FullName,
            typeof(short)
        },
        {
            typeof(long).FullName,
            typeof(long)
        },
        {
            typeof(ushort).FullName,
            typeof(ushort)
        },
        {
            typeof(uint).FullName,
            typeof(uint)
        },
        {
            typeof(ulong).FullName,
            typeof(ulong)
        },
        {
            typeof(float).FullName,
            typeof(float)
        },
        {
            typeof(double).FullName,
            typeof(double)
        },
        {
            typeof(decimal).FullName,
            typeof(decimal)
        },
        {
            typeof(DateTime).FullName,
            typeof(DateTime)
        },
        {
            typeof(TimeSpan).FullName,
            typeof(TimeSpan)
        }
    };

    private string ResourceReaderTypeName
    {
        get
        {
            if (!_requiresDeserializingResourceReader)
            {
                return "System.Resources.ResourceReader, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            }
            return "System.Resources.Extensions.DeserializingResourceReader, System.Resources.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51";
        }
    }

    private string ResourceSetTypeName
    {
        get
        {
            if (!_requiresDeserializingResourceReader)
            {
                return "System.Resources.RuntimeResourceSet";
            }
            return "System.Resources.Extensions.RuntimeResourceSet, System.Resources.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51";
        }
    }

    public PreserializedResourceWriter(string fileName)
    {
        if (fileName == null)
        {
            throw new ArgumentNullException("fileName");
        }
        _output = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
        _resourceList = new SortedDictionary<string?, object?>(FastResourceComparer.@default!);
        _caseInsensitiveDups = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
    }

    public PreserializedResourceWriter(Stream? stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }
        if (!stream.CanWrite)
        {
            throw new ArgumentException(Messages.ArgumentStreamNotWritable);
        }
        _output = stream;
        _resourceList = new SortedDictionary<string?, object?>(FastResourceComparer.@default!);
        _caseInsensitiveDups = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
    }

    public void AddResource(string? name, string value)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        if (_resourceList == null)
        {
            throw new InvalidOperationException(Messages.InvalidOperationResourceWriterSaved);
        }
        _caseInsensitiveDups.Add(name, null);
        _resourceList.Add(name, value);
    }

    public void AddResource(string? name, object? value)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        if (_resourceList == null)
        {
            throw new InvalidOperationException(Messages.InvalidOperationResourceWriterSaved);
        }
        if (value is Stream)
        {
            AddResourceInternal(name, (Stream)value, closeAfterWrite: false);
            return;
        }
        _caseInsensitiveDups.Add(name, null);
        _resourceList.Add(name, value);
    }

    public void AddResource(string? name, Stream value, bool closeAfterWrite = false)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        if (_resourceList == null)
        {
            throw new InvalidOperationException(Messages.InvalidOperationResourceWriterSaved);
        }
        AddResourceInternal(name, value, closeAfterWrite);
    }

    private void AddResourceInternal(string name, Stream value, bool closeAfterWrite)
    {
        if (value == null)
        {
            _caseInsensitiveDups.Add(name, null);
            _resourceList.Add(name, value);
            return;
        }
        if (!value.CanSeek)
        {
            throw new ArgumentException(Messages.NotSupportedUnseekableStream);
        }
        _caseInsensitiveDups.Add(name, null);
        _resourceList.Add(name, new StreamWrapper(value, closeAfterWrite));
    }

    public void AddResource(string? name, byte[] value)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        if (_resourceList == null)
        {
            throw new InvalidOperationException(Messages.InvalidOperationResourceWriterSaved);
        }
        _caseInsensitiveDups.Add(name, null);
        _resourceList.Add(name, value);
    }

    private void AddResourceData(string name, string typeName, object data)
    {
        if (_resourceList == null)
        {
            throw new InvalidOperationException(Messages.InvalidOperationResourceWriterSaved);
        }
        _caseInsensitiveDups.Add(name, null);
        if (_preserializedData == null)
        {
            _preserializedData = new Dictionary<string, PrecannedResource>(FastResourceComparer.@default);
        }
        _preserializedData.Add(name, new PrecannedResource(typeName, data));
    }

    public void Close()
    {
        Dispose(disposing: true);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_resourceList != null)
            {
                Generate();
            }

            _output?.Dispose();
        }
        _output = null;
        _caseInsensitiveDups?.Clear();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }

    public void Generate()
    {
        if (_resourceList == null)
        {
            throw new InvalidOperationException(Messages.InvalidOperationResourceWriterSaved);
        }

        if (_output != null)
        {
            var binaryWriter = new BinaryWriter(_output, Encoding.UTF8);
            List<string> list = [];
            binaryWriter.Write(ResourceManager.MagicNumber);
            binaryWriter.Write(ResourceManager.HeaderVersionNumber);
            var memoryStream = new MemoryStream(240);
            var binaryWriter2 = new BinaryWriter(memoryStream);
            binaryWriter2.Write(ResourceReaderTypeName);
            binaryWriter2.Write(ResourceSetTypeName);
            binaryWriter2.Flush();
            binaryWriter.Write((int)memoryStream.Length);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            memoryStream.CopyTo(binaryWriter.BaseStream, (int)memoryStream.Length);
            binaryWriter.Write(2);
            var num = _resourceList.Count;
            if (_preserializedData != null)
            {
                num += _preserializedData.Count;
            }
            binaryWriter.Write(num);
            var array = new int[num];
            var array2 = new int[num];
            var num2 = 0;
            var memoryStream2 = new MemoryStream(num * 40);
            var binaryWriter3 = new BinaryWriter(memoryStream2, Encoding.Unicode);
            Stream stream = new MemoryStream();
            using (stream)
            {
                var binaryWriter4 = new BinaryWriter(stream, Encoding.UTF8);
                if (_preserializedData != null)
                {
                    foreach (var pair in _preserializedData)
                    {
                        _resourceList.Add(pair.Key, pair.Value);
                    }
                }
                foreach (var resource in _resourceList)
                {
                    array[num2] = FastResourceComparer.HashFunction(resource.Key);
                    array2[num2++] = (int)binaryWriter3.Seek(0, SeekOrigin.Current);
                    if (resource.Key != null)
                    {
                        binaryWriter3.Write(resource.Key);
                    }

                    binaryWriter3.Write((int)binaryWriter4.Seek(0, SeekOrigin.Current));
                    var value = resource.Value;
                    var resourceTypeCode = FindTypeCode(value, list);
                    Write7BitEncodedInt(binaryWriter4, (int)resourceTypeCode);
                    if (value is PrecannedResource precannedResource)
                    {
                        WriteData(binaryWriter4, precannedResource.data);
                    }
                    else
                    {
                        if (value != null)
                        {
                            WriteValue(resourceTypeCode, value, binaryWriter4);
                        }
                    }
                }
                binaryWriter.Write(list.Count);
                foreach (var item in list)
                {
                    binaryWriter.Write(item);
                }
                Array.Sort(array, array2);
                binaryWriter.Flush();
                var num3 = (int)binaryWriter.BaseStream.Position & 7;
                if (num3 > 0)
                {
                    for (var i = 0; i < 8 - num3; i++)
                    {
                        binaryWriter.Write("PAD"[i % 3]);
                    }
                }
                var array3 = array;
                foreach (var value2 in array3)
                {
                    binaryWriter.Write(value2);
                }
                var array4 = array2;
                foreach (var value3 in array4)
                {
                    binaryWriter.Write(value3);
                }
                binaryWriter.Flush();
                binaryWriter3.Flush();
                binaryWriter4.Flush();
                var num4 = (int)(binaryWriter.Seek(0, SeekOrigin.Current) + memoryStream2.Length);
                num4 += 4;
                binaryWriter.Write(num4);
                if (memoryStream2.Length > 0)
                {
                    memoryStream2.Seek(0L, SeekOrigin.Begin);
                    memoryStream2.CopyTo(binaryWriter.BaseStream, (int)memoryStream2.Length);
                }
                binaryWriter3.Dispose();
                stream.Position = 0L;
                stream.CopyTo(binaryWriter.BaseStream);
                binaryWriter4.Dispose();
            }
            binaryWriter.Flush();
        }

        _resourceList?.Clear();
    }

    private static void Write7BitEncodedInt(BinaryWriter store, int value)
    {
        uint num;
        for (num = (uint)value; num >= 128; num >>= 7)
        {
            store.Write((byte)(num | 0x80u));
        }
        store.Write((byte)num);
    }

    private ResourceTypeCode FindTypeCode(object? value, List<string> types)
    {
        if (value == null)
        {
            return ResourceTypeCode.Null;
        }
        var type = value.GetType();
        if (type == typeof(string))
        {
            return ResourceTypeCode.String;
        }
        if (type == typeof(int))
        {
            return ResourceTypeCode.Int32;
        }
        if (type == typeof(bool))
        {
            return ResourceTypeCode.Boolean;
        }
        if (type == typeof(char))
        {
            return ResourceTypeCode.Char;
        }
        if (type == typeof(byte))
        {
            return ResourceTypeCode.Byte;
        }
        if (type == typeof(sbyte))
        {
            return ResourceTypeCode.SByte;
        }
        if (type == typeof(short))
        {
            return ResourceTypeCode.Int16;
        }
        if (type == typeof(long))
        {
            return ResourceTypeCode.Int64;
        }
        if (type == typeof(ushort))
        {
            return ResourceTypeCode.UInt16;
        }
        if (type == typeof(uint))
        {
            return ResourceTypeCode.UInt32;
        }
        if (type == typeof(ulong))
        {
            return ResourceTypeCode.UInt64;
        }
        if (type == typeof(float))
        {
            return ResourceTypeCode.Single;
        }
        if (type == typeof(double))
        {
            return ResourceTypeCode.Double;
        }
        if (type == typeof(decimal))
        {
            return ResourceTypeCode.Decimal;
        }
        if (type == typeof(DateTime))
        {
            return ResourceTypeCode.DateTime;
        }
        if (type == typeof(TimeSpan))
        {
            return ResourceTypeCode.TimeSpan;
        }
        if (type == typeof(byte[]))
        {
            return ResourceTypeCode.ByteArray;
        }
        if (type == typeof(StreamWrapper))
        {
            return ResourceTypeCode.Stream;
        }
        if (type == typeof(PrecannedResource))
        {
            var typeName = ((PrecannedResource)value).typeName;
            if (typeName.StartsWith("ResourceTypeCode.", StringComparison.Ordinal))
            {
                typeName = typeName.Substring(17);
                return (ResourceTypeCode)Enum.Parse(typeof(ResourceTypeCode), typeName);
            }
            var num = types.IndexOf(typeName);
            if (num == -1)
            {
                num = types.Count;
                types.Add(typeName);
            }
            return (ResourceTypeCode)(num + 64);
        }
        throw new PlatformNotSupportedException(Messages.NotSupportedBinarySerializedResources);
    }

    private void WriteValue(ResourceTypeCode typeCode, object value, BinaryWriter writer)
    {
        switch (typeCode)
        {
            case ResourceTypeCode.String:
                writer.Write((string)value);
                break;
            case ResourceTypeCode.Boolean:
                writer.Write((bool)value);
                break;
            case ResourceTypeCode.Char:
                writer.Write((ushort)(char)value);
                break;
            case ResourceTypeCode.Byte:
                writer.Write((byte)value);
                break;
            case ResourceTypeCode.SByte:
                writer.Write((sbyte)value);
                break;
            case ResourceTypeCode.Int16:
                writer.Write((short)value);
                break;
            case ResourceTypeCode.UInt16:
                writer.Write((ushort)value);
                break;
            case ResourceTypeCode.Int32:
                writer.Write((int)value);
                break;
            case ResourceTypeCode.UInt32:
                writer.Write((uint)value);
                break;
            case ResourceTypeCode.Int64:
                writer.Write((long)value);
                break;
            case ResourceTypeCode.UInt64:
                writer.Write((ulong)value);
                break;
            case ResourceTypeCode.Single:
                writer.Write((float)value);
                break;
            case ResourceTypeCode.Double:
                writer.Write((double)value);
                break;
            case ResourceTypeCode.Decimal:
                writer.Write((decimal)value);
                break;
            case ResourceTypeCode.DateTime:
                {
                    var value2 = ((DateTime)value).ToBinary();
                    writer.Write(value2);
                    break;
                }
            case ResourceTypeCode.TimeSpan:
                writer.Write(((TimeSpan)value).Ticks);
                break;
            case ResourceTypeCode.ByteArray:
                {
                    var array3 = (byte[])value;
                    writer.Write(array3.Length);
                    writer.Write(array3, 0, array3.Length);
                    break;
                }
            case ResourceTypeCode.Stream:
                {
                    var streamWrapper = (StreamWrapper)value;
                    if (streamWrapper.stream.GetType() == typeof(MemoryStream))
                    {
                        var memoryStream = (MemoryStream)streamWrapper.stream;
                        if (memoryStream.Length > int.MaxValue)
                        {
                            throw new ArgumentException(Messages.ArgumentOutOfRangeStreamLength);
                        }
                        var array = memoryStream.ToArray();
                        writer.Write(array.Length);
                        writer.Write(array, 0, array.Length);
                        break;
                    }
                    var stream = streamWrapper.stream;
                    if (stream.Length > int.MaxValue)
                    {
                        throw new ArgumentException(Messages.ArgumentOutOfRangeStreamLength);
                    }
                    stream.Position = 0L;
                    writer.Write((int)stream.Length);
                    var array2 = new byte[4096];
                    int num;
                    while ((num = stream.Read(array2, 0, array2.Length)) != 0)
                    {
                        writer.Write(array2, 0, num);
                    }
                    if (streamWrapper.closeAfterWrite)
                    {
                        stream.Close();
                    }
                    break;
                }
            default:
                throw new PlatformNotSupportedException(Messages.NotSupportedBinarySerializedResources);
            case ResourceTypeCode.Null:
                break;
        }
    }

    public void AddResource(string? name, string value, string typeName)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (typeName == null)
        {
            throw new ArgumentNullException("typeName");
        }
        if (primitiveTypes.TryGetValue(typeName, out var value2))
        {
            if (value2 == typeof(string))
            {
                AddResource(name, value);
                return;
            }
            var converter = TypeDescriptor.GetConverter(value2);
            if (converter == null)
            {
                throw new TypeLoadException(Messages.Format(Messages.TypeLoadExceptionCannotLoadConverter, value2));
            }
            var value3 = converter.ConvertFromInvariantString(value);
            AddResource(name, value3);
        }
        else
        {
            AddResourceData(name, typeName, new ResourceDataRecord(SerializationFormat.TypeConverterString, value));
            _requiresDeserializingResourceReader = true;
        }
    }

    public void AddTypeConverterResource(string? name, byte[] value, string typeName)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (typeName == null)
        {
            throw new ArgumentNullException("typeName");
        }
        AddResourceData(name, typeName, new ResourceDataRecord(SerializationFormat.TypeConverterByteArray, value));
        _requiresDeserializingResourceReader = true;
    }

    public void AddBinaryFormattedResource(string? name, byte[] value, string? typeName = null)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (typeName == null)
        {
            typeName = unknownObjectTypeName;
            _requiresDeserializingResourceReader = true;
        }
        AddResourceData(name, typeName, new ResourceDataRecord(SerializationFormat.BinaryFormatter, value));
    }

    public void AddActivatorResource(string? name, Stream value, string typeName, bool closeAfterWrite = false)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (typeName == null)
        {
            throw new ArgumentNullException("typeName");
        }
        if (!value.CanSeek)
        {
            throw new ArgumentException(Messages.NotSupportedUnseekableStream);
        }
        AddResourceData(name, typeName, new ResourceDataRecord(SerializationFormat.ActivatorStream, value, closeAfterWrite));
        _requiresDeserializingResourceReader = true;
    }

    private void WriteData(BinaryWriter writer, object dataContext)
    {
        var resourceDataRecord = dataContext as ResourceDataRecord;
        if (_requiresDeserializingResourceReader)
        {
            Write7BitEncodedInt(writer, (int)(resourceDataRecord?.format??0));
        }
        try
        {
            if (resourceDataRecord != null)
            {
                switch (resourceDataRecord.format)
                {
                    case SerializationFormat.BinaryFormatter:
                    {
                        var array2 = (byte[])resourceDataRecord.data;
                        if (_requiresDeserializingResourceReader)
                        {
                            Write7BitEncodedInt(writer, array2.Length);
                        }

                        writer.Write(array2);
                        break;
                    }
                    case SerializationFormat.ActivatorStream:
                    {
                        var stream = (Stream)resourceDataRecord.data;
                        if (stream.Length > int.MaxValue)
                        {
                            throw new ArgumentException(Messages.ArgumentOutOfRangeStreamLength);
                        }

                        stream.Position = 0L;
                        Write7BitEncodedInt(writer, (int)stream.Length);
                        stream.CopyTo(writer.BaseStream);
                        break;
                    }
                    case SerializationFormat.TypeConverterByteArray:
                    {
                        var array = (byte[])resourceDataRecord.data;
                        Write7BitEncodedInt(writer, array.Length);
                        writer.Write(array);
                        break;
                    }
                    case SerializationFormat.TypeConverterString:
                    {
                        var value = (string)resourceDataRecord.data;
                        writer.Write(value);
                        break;
                    }
                    default:
                        throw new ArgumentException("Format");
                }
            }
        }
        finally
        {
            var disposable = resourceDataRecord?.data as IDisposable;
            if (disposable != null && resourceDataRecord!.closeAfterWrite)
            {
                disposable.Dispose();
            }
        }
    }
}