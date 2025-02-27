using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms.Resources.IO;

namespace System.Windows.Forms.Resources;

public sealed class DeserializingResourceReader : System.Resources.IResourceReader, IEnumerable, IDisposable
{
    internal sealed class ResourceEnumerator : IDictionaryEnumerator, IEnumerator
    {
        private const int enumDone = int.MinValue;

        private const int enumNotStarted = -1;

        private readonly DeserializingResourceReader _reader;

        private bool _currentIsValid;

        private int _currentName;

        private int _dataPosition;

        public object Key
        {
            get
            {
                if (_currentName == int.MinValue)
                {
                    throw new InvalidOperationException(SR.InvalidOperationEnumEnded);
                }
                if (!_currentIsValid)
                {
                    throw new InvalidOperationException(SR.InvalidOperationEnumNotStarted);
                }
                if (_reader._resCache == null)
                {
                    throw new InvalidOperationException(SR.ResourceReaderIsClosed);
                }
                return _reader.AllocateStringForNameIndex(_currentName, out _dataPosition);
            }
        }

        public object Current => Entry;

        internal int DataPosition => _dataPosition;

        public DictionaryEntry Entry
        {
            get
            {
                if (_currentName == int.MinValue)
                {
                    throw new InvalidOperationException(SR.InvalidOperationEnumEnded);
                }
                if (!_currentIsValid)
                {
                    throw new InvalidOperationException(SR.InvalidOperationEnumNotStarted);
                }
                if (_reader._resCache == null)
                {
                    throw new InvalidOperationException(SR.ResourceReaderIsClosed);
                }
                object? obj = null;
                string? key;
                lock (_reader)
                {
                    lock (_reader._resCache)
                    {
                        key = _reader.AllocateStringForNameIndex(_currentName, out _dataPosition);
                        if (_reader._resCache.TryGetValue(key ?? string.Empty, out var value))
                        {
                            obj = value.Value;
                        }
                        if (obj == null)
                        {
                            obj = _dataPosition != -1 ? _reader.LoadObject(_dataPosition) : _reader.GetValueForNameIndex(_currentName);
                        }
                    }
                }
                return new DictionaryEntry(key ?? string.Empty, obj);
            }
        }

        public object? Value
        {
            get
            {
                if (_currentName == int.MinValue)
                {
                    throw new InvalidOperationException(SR.InvalidOperationEnumEnded);
                }
                if (!_currentIsValid)
                {
                    throw new InvalidOperationException(SR.InvalidOperationEnumNotStarted);
                }
                if (_reader._resCache == null)
                {
                    throw new InvalidOperationException(SR.ResourceReaderIsClosed);
                }
                return _reader.GetValueForNameIndex(_currentName);
            }
        }

        internal ResourceEnumerator(DeserializingResourceReader reader)
        {
            _currentName = -1;
            _reader = reader;
            _dataPosition = -2;
        }

        public bool MoveNext()
        {
            if (_currentName == _reader._numResources - 1 || _currentName == int.MinValue)
            {
                _currentIsValid = false;
                _currentName = int.MinValue;
                return false;
            }
            _currentIsValid = true;
            _currentName++;
            return true;
        }

        public void Reset()
        {
            if (_reader._resCache == null)
            {
                throw new InvalidOperationException(SR.ResourceReaderIsClosed);
            }
            _currentIsValid = false;
            _currentName = -1;
        }
    }

    private const int defaultFileStreamBufferSize = 4096;

    private BinaryReader _store;

    internal Dictionary<string?, ResourceLocator>? _resCache;

    private long _nameSectionOffset;

    private long _dataSectionOffset;

    private int[]? _nameHashes;

    private unsafe int* _nameHashesPtr;

    private int[]? _namePositions;

    private unsafe int* _namePositionsPtr;

    private Type?[]? _typeTable;

    private int[]? _typeNamePositions;

    private int _numResources;

    private UnmanagedMemoryStream? _ums;

    private int _version;

    private bool _assumeBinaryFormatter;

    //private BinaryFormatter _formatter;
    private DataContractSerializer? _formatter;
    public DeserializingResourceReader(string fileName)
    {
        _resCache = new Dictionary<string?, ResourceLocator>(FastResourceComparer.@default!);
        _store = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.RandomAccess), Encoding.UTF8);
        try
        {
            ReadResources();
        }
        catch
        {
            _store.Close();
            throw;
        }
    }

    public DeserializingResourceReader(Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }
        if (!stream.CanRead)
        {
            throw new ArgumentException(SR.ArgumentStreamNotReadable);
        }
        _resCache = new Dictionary<string?, ResourceLocator>(FastResourceComparer.@default!);
        _store = new BinaryReader(stream, Encoding.UTF8);
        _ums = stream as UnmanagedMemoryStream;
        ReadResources();
    }

    public void Close()
    {
        Dispose(disposing: true);
    }

    public void Dispose()
    {
        Close();
    }

    private unsafe void Dispose(bool disposing)
    {
        if (_store != null)
        {
            _resCache = null;
            if (disposing)
            {
                var store = _store;
                _store = null!;
                store?.Close();
            }
            _store = null!;
            _namePositions = null;
            _nameHashes = null;
            _ums = null;
            _namePositionsPtr = null;
            _nameHashesPtr = null;
        }
    }

    internal static unsafe int ReadUnalignedI4(int* p)
    {
        return *(byte*)p | (((byte*)p)[1] << 8) | (((byte*)p)[2] << 16) | (((byte*)p)[3] << 24);
    }

    private void SkipString()
    {
        var num = _store.Read7BitEncodedInt();
        if (num < 0)
        {
            throw new BadImageFormatException(SR.BadImageFormatNegativeStringLength);
        }
        _store?.BaseStream.Seek(num, SeekOrigin.Current);
    }

    private unsafe int GetNameHash(int index)
    {
        if (_ums == null)
        {
            return _nameHashes![index];
        }
        return ReadUnalignedI4(_nameHashesPtr + index);
    }

    private unsafe int GetNamePosition(int index)
    {
        var num = _ums != null ? ReadUnalignedI4(_namePositionsPtr + index) : _namePositions![index];
        lock (this)
        {
            if (num < 0 || num > _dataSectionOffset - _nameSectionOffset)
            {
                throw new FormatException(SR.Format(SR.BadImageFormatResourcesNameInvalidOffset, num));
            }
        }
        return num;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IDictionaryEnumerator GetEnumerator()
    {
        if (_resCache == null)
        {
            throw new InvalidOperationException(SR.ResourceReaderIsClosed);
        }
        return new ResourceEnumerator(this);
    }

    internal ResourceEnumerator GetEnumeratorInternal()
    {
        return new ResourceEnumerator(this);
    }

    internal int FindPosForResource(string? name)
    {
        var num = FastResourceComparer.HashFunction(name);
        var num2 = 0;
        var i = _numResources - 1;
        var num3 = -1;
        var flag = false;
        while (num2 <= i)
        {
            num3 = num2 + i >> 1;
            var nameHash = GetNameHash(num3);
            var num4 = nameHash != num ? nameHash >= num ? 1 : -1 : 0;
            if (num4 == 0)
            {
                flag = true;
                break;
            }
            if (num4 < 0)
            {
                num2 = num3 + 1;
            }
            else
            {
                i = num3 - 1;
            }
        }
        if (!flag)
        {
            return -1;
        }
        if (num2 != num3)
        {
            num2 = num3;
            while (num2 > 0 && GetNameHash(num2 - 1) == num)
            {
                num2--;
            }
        }
        if (i != num3)
        {
            for (i = num3; i < _numResources - 1 && GetNameHash(i + 1) == num; i++)
            {
            }
        }
        lock (this)
        {
            for (var j = num2; j <= i; j++)
            {
                _store?.BaseStream.Seek(_nameSectionOffset + GetNamePosition(j), SeekOrigin.Begin);
                if (CompareStringEqualsName(name))
                {
                    if (_store != null)
                    {
                        var num5 = _store.ReadInt32();
                        if (num5 < 0 || num5 >= _store.BaseStream.Length - _dataSectionOffset)
                        {
                            throw new FormatException(SR.Format(SR.BadImageFormatResourcesDataInvalidOffset, num5));
                        }
                        return num5;
                    }
                }
            }
        }
        return -1;
    }

    private unsafe bool CompareStringEqualsName(string? name)
    {
        var num = _store.Read7BitEncodedInt();
        if (num < 0)
        {
            throw new BadImageFormatException(SR.BadImageFormatNegativeStringLength);
        }
        if (_ums != null)
        {
            var positionPointer = _ums.PositionPointer;
            _ums.Seek(num, SeekOrigin.Current);
            if (_ums.Position > _ums.Length)
            {
                throw new BadImageFormatException(SR.BadImageFormatResourcesNameTooLong);
            }
            return FastResourceComparer.CompareOrdinal(positionPointer, num, name) == 0;
        }
        var array = new byte[num];
        var num2 = num;
        while (num2 > 0)
        {
            if (_store != null)
            {
                var num3 = _store.Read(array, num - num2, num2);
                if (num3 == 0)
                {
                    throw new BadImageFormatException(SR.BadImageFormatResourceNameCorrupted);
                }
                num2 -= num3;
            }
        }
        return FastResourceComparer.CompareOrdinal(array, num / 2, name) == 0;
    }

    private unsafe string AllocateStringForNameIndex(int index, out int dataOffset)
    {
        long num = GetNamePosition(index);
        int num2;
        byte[] array;
        lock (this)
        {
            _store.BaseStream.Seek(num + _nameSectionOffset, SeekOrigin.Begin);
            num2 = _store.Read7BitEncodedInt();
            if (num2 < 0)
            {
                throw new BadImageFormatException(SR.BadImageFormatNegativeStringLength);
            }
            if (_ums != null)
            {
                if (_ums.Position > _ums.Length - num2)
                {
                    throw new BadImageFormatException(SR.Format(SR.BadImageFormatResourcesIndexTooLong, index));
                }

                var positionPointer = (char*)_ums.PositionPointer;
                var text = new string(positionPointer, 0, num2 / 2);
                _ums.Position += num2;
                dataOffset = _store.ReadInt32();
                if (dataOffset < 0 || dataOffset >= _store.BaseStream.Length - _dataSectionOffset)
                {
                    throw new FormatException(SR.Format(SR.BadImageFormatResourcesDataInvalidOffset, dataOffset));
                }
                return text;
            }
            array = new byte[num2];
            var num3 = num2;
            while (num3 > 0)
            {
                var num4 = _store.Read(array, num2 - num3, num3);
                if (num4 == 0)
                {
                    throw new EndOfStreamException(SR.Format(SR.BadImageFormatResourceNameCorruptedNameIndex, index));
                }
                num3 -= num4;
            }
            dataOffset = _store.ReadInt32();
            if (dataOffset < 0 || dataOffset >= _store.BaseStream.Length - _dataSectionOffset)
            {
                throw new FormatException(SR.Format(SR.BadImageFormatResourcesDataInvalidOffset, dataOffset));
            }
        }
        return Encoding.Unicode.GetString(array, 0, num2);
    }

    private object? GetValueForNameIndex(int index)
    {
        long num = GetNamePosition(index);
        lock (this)
        {
            _store.BaseStream.Seek(num + _nameSectionOffset, SeekOrigin.Begin);
            SkipString();
            var num2 = _store.ReadInt32();
            if (num2 < 0 || num2 >= _store.BaseStream.Length - _dataSectionOffset)
            {
                throw new FormatException(SR.Format(SR.BadImageFormatResourcesDataInvalidOffset, num2));
            }
            if (_version == 1)
            {
                return LoadObjectV1(num2);
            }

            return LoadObjectV2(num2, out _);
        }
    }

    internal string? LoadString(int pos)
    {
        lock (this)
        {
            _store.BaseStream.Seek(_dataSectionOffset + pos, SeekOrigin.Begin);
        }
        string? result = null;
        var num = _store.Read7BitEncodedInt();
        if (_version == 1)
        {
            if (num == -1)
            {
                return null;
            }
            if (FindType(num) != typeof(string))
            {
                throw new InvalidOperationException(SR.Format(SR.InvalidOperationResourceNotStringType, FindType(num)?.FullName));
            }
            result = _store.ReadString();
        }
        else
        {
            var resourceTypeCode = (ResourceTypeCode)num;
            if (resourceTypeCode != ResourceTypeCode.String && resourceTypeCode != 0)
            {
                throw new InvalidOperationException(SR.Format(p1: resourceTypeCode >= ResourceTypeCode.StartOfUserTypes ? FindType((int)(resourceTypeCode - 64))?.FullName : resourceTypeCode.ToString(), resourceFormat: SR.InvalidOperationResourceNotStringType));
            }
            if (resourceTypeCode == ResourceTypeCode.String)
            {
                result = _store.ReadString();
            }
        }
        return result;
    }

    internal object? LoadObject(int pos)
    {
        if (_version == 1)
        {
            return LoadObjectV1(pos);
        }

        return LoadObjectV2(pos, out _);
    }

    internal object? LoadObject(int pos, out ResourceTypeCode typeCode)
    {
        if (_version == 1)
        {
            var obj = LoadObjectV1(pos);
            typeCode = obj is string ? ResourceTypeCode.String : ResourceTypeCode.StartOfUserTypes;
            return obj;
        }
        return LoadObjectV2(pos, out typeCode);
    }

    internal object? LoadObjectV1(int pos)
    {
        try
        {
            return _LoadObjectV1(pos);
        }
        catch (EndOfStreamException inner)
        {
            throw new BadImageFormatException(SR.BadImageFormatTypeMismatch, inner);
        }
        catch (ArgumentOutOfRangeException inner2)
        {
            throw new BadImageFormatException(SR.BadImageFormatTypeMismatch, inner2);
        }
    }

    private object? _LoadObjectV1(int pos)
    {
        lock (this)
        {
            _store.BaseStream.Seek(_dataSectionOffset + pos, SeekOrigin.Begin);
        }
        var num = _store.Read7BitEncodedInt();
        if (num == -1)
        {
            return null;
        }
        var type = FindType(num);
        if (type == typeof(string))
        {
            return _store.ReadString();
        }
        if (type == typeof(int))
        {
            return _store.ReadInt32();
        }
        if (type == typeof(byte))
        {
            return _store.ReadByte();
        }
        if (type == typeof(sbyte))
        {
            return _store.ReadSByte();
        }
        if (type == typeof(short))
        {
            return _store.ReadInt16();
        }
        if (type == typeof(long))
        {
            return _store.ReadInt64();
        }
        if (type == typeof(ushort))
        {
            return _store.ReadUInt16();
        }
        if (type == typeof(uint))
        {
            return _store.ReadUInt32();
        }
        if (type == typeof(ulong))
        {
            return _store.ReadUInt64();
        }
        if (type == typeof(float))
        {
            return _store.ReadSingle();
        }
        if (type == typeof(double))
        {
            return _store.ReadDouble();
        }
        if (type == typeof(DateTime))
        {
            return new DateTime(_store.ReadInt64());
        }
        if (type == typeof(TimeSpan))
        {
            return new TimeSpan(_store.ReadInt64());
        }
        if (type == typeof(decimal))
        {
            var array = new int[4];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = _store.ReadInt32();
            }
            return new decimal(array);
        }
        return DeserializeObject(num);
    }

    internal object? LoadObjectV2(int pos, out ResourceTypeCode typeCode)
    {
        try
        {
            return _LoadObjectV2(pos, out typeCode);
        }
        catch (EndOfStreamException inner)
        {
            throw new BadImageFormatException(SR.BadImageFormatTypeMismatch, inner);
        }
        catch (ArgumentOutOfRangeException inner2)
        {
            throw new BadImageFormatException(SR.BadImageFormatTypeMismatch, inner2);
        }
    }

    private unsafe object? _LoadObjectV2(int pos, out ResourceTypeCode typeCode)
    {
        lock (this)
        {
            _store.BaseStream.Seek(_dataSectionOffset + pos, SeekOrigin.Begin);
        }
        typeCode = (ResourceTypeCode)_store.Read7BitEncodedInt();
        switch (typeCode)
        {
            case ResourceTypeCode.Null:
                return null;
            case ResourceTypeCode.String:
                return _store.ReadString();
            case ResourceTypeCode.Boolean:
                return _store.ReadBoolean();
            case ResourceTypeCode.Char:
                return (char)_store.ReadUInt16();
            case ResourceTypeCode.Byte:
                return _store.ReadByte();
            case ResourceTypeCode.SByte:
                return _store.ReadSByte();
            case ResourceTypeCode.Int16:
                return _store.ReadInt16();
            case ResourceTypeCode.UInt16:
                return _store.ReadUInt16();
            case ResourceTypeCode.Int32:
                return _store.ReadInt32();
            case ResourceTypeCode.UInt32:
                return _store.ReadUInt32();
            case ResourceTypeCode.Int64:
                return _store.ReadInt64();
            case ResourceTypeCode.UInt64:
                return _store.ReadUInt64();
            case ResourceTypeCode.Single:
                return _store.ReadSingle();
            case ResourceTypeCode.Double:
                return _store.ReadDouble();
            case ResourceTypeCode.Decimal:
                return _store.ReadDecimal();
            case ResourceTypeCode.DateTime:
                {
                    var dateData = _store.ReadInt64();
                    return DateTime.FromBinary(dateData);
                }
            case ResourceTypeCode.TimeSpan:
                {
                    var ticks = _store.ReadInt64();
                    return new TimeSpan(ticks);
                }
            case ResourceTypeCode.ByteArray:
                {
                    var num2 = _store.ReadInt32();
                    if (num2 < 0)
                    {
                        throw new BadImageFormatException(SR.Format(SR.BadImageFormatResourceDataLengthInvalid, num2));
                    }
                    if (_ums == null)
                    {
                        if (num2 > _store.BaseStream.Length)
                        {
                            throw new BadImageFormatException(SR.Format(SR.BadImageFormatResourceDataLengthInvalid, num2));
                        }
                        return _store.ReadBytes(num2);
                    }
                    if (num2 > _ums.Length - _ums.Position)
                    {
                        throw new BadImageFormatException(SR.Format(SR.BadImageFormatResourceDataLengthInvalid, num2));
                    }
                    var array2 = new byte[num2];
                    _ = _ums.Read(array2, 0, num2);
                    return array2;
                }
            case ResourceTypeCode.Stream:
                {
                    var num = _store.ReadInt32();
                    if (num < 0)
                    {
                        throw new BadImageFormatException(SR.Format(SR.BadImageFormatResourceDataLengthInvalid, num));
                    }
                    if (_ums == null)
                    {
                        var array = _store.ReadBytes(num);
                        return new PinnedBufferMemoryStream(array);
                    }
                    if (num > _ums.Length - _ums.Position)
                    {
                        throw new BadImageFormatException(SR.Format(SR.BadImageFormatResourceDataLengthInvalid, num));
                    }
                    return new UnmanagedMemoryStream(_ums.PositionPointer, num, num, FileAccess.Read);
                }
            default:
                {
                    if (typeCode < ResourceTypeCode.StartOfUserTypes)
                    {
                        throw new BadImageFormatException(SR.BadImageFormatTypeMismatch);
                    }
                    var typeIndex = (int)(typeCode - 64);
                    return DeserializeObject(typeIndex);
                }
        }
    }

    private void ReadResources()
    {
        try
        {
            _ReadResources();
        }
        catch (EndOfStreamException inner)
        {
            throw new BadImageFormatException(SR.BadImageFormatResourcesHeaderCorrupted, inner);
        }
        catch (IndexOutOfRangeException inner2)
        {
            throw new BadImageFormatException(SR.BadImageFormatResourcesHeaderCorrupted, inner2);
        }
    }

    private unsafe void _ReadResources()
    {
        var num = _store.ReadInt32();
        if (num != System.Resources.ResourceManager.MagicNumber)
        {
            throw new ArgumentException(SR.ResourcesStreamNotValid);
        }
        var num2 = _store.ReadInt32();
        var num3 = _store.ReadInt32();
        if (num3 < 0 || num2 < 0)
        {
            throw new BadImageFormatException(SR.BadImageFormatResourcesHeaderCorrupted);
        }
        if (num2 > 1)
        {
            _store.BaseStream.Seek(num3, SeekOrigin.Current);
        }
        else
        {
            var text = _store.ReadString();
            if (!ValidateReaderType(text))
            {
                throw new NotSupportedException(SR.Format(SR.NotSupportedWrongResourceReaderType, text));
            }
            SkipString();
        }
        var num4 = _store.ReadInt32();
        if (num4 != 2 && num4 != 1)
        {
            throw new ArgumentException(SR.Format(SR.ArgResourceFileUnsupportedVersion, 2, num4));
        }
        _version = num4;
        _numResources = _store.ReadInt32();
        if (_numResources < 0)
        {
            throw new BadImageFormatException(SR.BadImageFormatResourcesHeaderCorrupted);
        }
        var num5 = _store.ReadInt32();
        if (num5 < 0)
        {
            throw new BadImageFormatException(SR.BadImageFormatResourcesHeaderCorrupted);
        }
        _typeTable = new Type[num5];
        _typeNamePositions = new int[num5];
        for (var i = 0; i < num5; i++)
        {
            _typeNamePositions[i] = (int)_store.BaseStream.Position;
            SkipString();
        }
        var position = _store.BaseStream.Position;
        var num6 = (int)position & 7;
        if (num6 != 0)
        {
            for (var j = 0; j < 8 - num6; j++)
            {
                _store.ReadByte();
            }
        }
        if (_ums == null)
        {
            _nameHashes = new int[_numResources];
            for (var k = 0; k < _numResources; k++)
            {
                _nameHashes[k] = _store.ReadInt32();
            }
        }
        else
        {
            var num7 = 4 * _numResources;
            if (num7 < 0)
            {
                throw new BadImageFormatException(SR.BadImageFormatResourcesHeaderCorrupted);
            }
            _nameHashesPtr = (int*)_ums.PositionPointer;
            _ums.Seek(num7, SeekOrigin.Current);
        }
        if (_ums == null)
        {
            _namePositions = new int[_numResources];
            for (var l = 0; l < _numResources; l++)
            {
                var num8 = _store.ReadInt32();
                if (num8 < 0)
                {
                    throw new BadImageFormatException(SR.BadImageFormatResourcesHeaderCorrupted);
                }
                _namePositions[l] = num8;
            }
        }
        else
        {
            var num9 = 4 * _numResources;
            if (num9 < 0)
            {
                throw new BadImageFormatException(SR.BadImageFormatResourcesHeaderCorrupted);
            }
            _namePositionsPtr = (int*)_ums.PositionPointer;
            _ums.Seek(num9, SeekOrigin.Current);
        }
        lock (this)
        {
            _dataSectionOffset = _store.ReadInt32();
            if (_dataSectionOffset < 0)
            {
                throw new BadImageFormatException(SR.BadImageFormatResourcesHeaderCorrupted);
            }
            _nameSectionOffset = _store.BaseStream.Position;
            if (_dataSectionOffset < _nameSectionOffset)
            {
                throw new BadImageFormatException(SR.BadImageFormatResourcesHeaderCorrupted);
            }
        }
    }

    private Type? FindType(int typeIndex)
    {
        if (typeIndex < 0 || typeIndex >= _typeTable?.Length)
        {
            throw new BadImageFormatException(SR.BadImageFormatInvalidType);
        }
        if (_typeTable?[typeIndex] == null)
        {
            var position = _store.BaseStream.Position;
            try
            {
                if (_typeNamePositions != null)
                {
                    _store.BaseStream.Position = _typeNamePositions[typeIndex];
                }

                var typeName = _store.ReadString();
                if (typeName == "System.Resources.Extensions.UnknownType")
                {
                    if (_typeTable != null)
                    {
                        _typeTable[typeIndex] = typeof(System.Windows.Forms.ImageListStreamer);
                    }
                }
                else if (_typeTable != null)
                    _typeTable[typeIndex] = Type.GetType(typeName.Split(',')[0], throwOnError: true);
            }
            catch (FileNotFoundException)
            {
                throw new NotSupportedException(SR.NotSupportedResourceObjectSerialization);
            }
            finally
            {
                _store.BaseStream.Position = position;
            }
        }
        return _typeTable?[typeIndex];
    }

    private string TypeNameFromTypeCode(ResourceTypeCode typeCode)
    {
        if (typeCode < ResourceTypeCode.StartOfUserTypes)
        {
            return "ResourceTypeCode." + typeCode;
        }
        var num = (int)(typeCode - 64);
        var position = _store.BaseStream.Position;
        try
        {
            if (_typeNamePositions != null)
            {
                _store.BaseStream.Position = _typeNamePositions[num];
            }

            return _store.ReadString();
        }
        finally
        {
            _store.BaseStream.Position = position;
        }
    }

    private bool ValidateReaderType(string? readerType)
    {
        if (TypeNameComparer.Instance.Equals(readerType, "System.Resources.Extensions.DeserializingResourceReader, System.Resources.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"))
        {
            return true;
        }
        if (TypeNameComparer.Instance.Equals(readerType, "System.Resources.ResourceReader, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"))
        {
            _assumeBinaryFormatter = true;
            return true;
        }
        return false;
    }

    private object? ReadBinaryFormattedObject()
    {
        if (_formatter == null)
        {
            _formatter = new DataContractSerializer(typeof(Bitmap));

            // _formatter.Binder = new ImageListSerializationBinder();
        }
        return _formatter.ReadObject(_store.BaseStream);
        //return null;
    }

    private unsafe object? DeserializeObject(int typeIndex)
    {
        var type = FindType(typeIndex);
        if (_assumeBinaryFormatter)
        {
            return ReadBinaryFormattedObject();
        }
        object? obj = null;
        switch (_store.Read7BitEncodedInt())
        {
            case 1:
                {
                    var num2 = _store.Read7BitEncodedInt();
                    if (num2 < 0)
                    {
                        throw new BadImageFormatException(SR.Format(SR.BadImageFormatResourceDataLengthInvalid, num2));
                    }
                    var position = _store.BaseStream.Position;
                    if (type?.Name == "System.Windows.Forms.ImageListStreamer")
                    {
                        var bytedata = _store.ReadBytes(num2);
                        using var mem = new MemoryStream(bytedata);

                        //BinaryFormatter formatter = new BinaryFormatter();
                        //formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                        //formatter.Binder = new ImageListSerializationBinder();
                        //obj = formatter.Deserialize(mem);
                        var formatter = new DataContractSerializer(typeof(Bitmap));
                        obj = formatter.ReadObject(mem);
                    }
                    else
                    {
                        obj = ReadBinaryFormattedObject();
                    }
                    if (type == typeof(UnknownType))
                    {
                        type = obj?.GetType();
                    }
                    var num3 = _store.BaseStream.Position - position;
                    if (num3 != num2)
                    {
                        throw new BadImageFormatException(SR.Format(SR.BadImageFormatResourceDataLengthInvalid, num2));
                    }
                    break;
                }
            case 2:
                {
                    var num4 = _store.Read7BitEncodedInt();
                    if (num4 < 0)
                    {
                        throw new BadImageFormatException(SR.Format(SR.BadImageFormatResourceDataLengthInvalid, num4));
                    }
                    var value = _store.ReadBytes(num4);
                    var converter2 = TypeDescriptor.GetConverter(type!);
                    if (converter2 == null)
                    {
                        throw new TypeLoadException(SR.Format(SR.TypeLoadExceptionCannotLoadConverter, type));
                    }

                    if (type == typeof(Bitmap))
                    {
                        obj = new Bitmap(value);
                    }
                    else
                    {
                        obj = converter2.ConvertFrom(value);
                    }
                    break;
                }
            case 3:
                {
                    var text = _store.ReadString();
                    var converter = TypeDescriptor.GetConverter(type!);
                    if (converter == null)
                    {
                        throw new TypeLoadException(SR.Format(SR.TypeLoadExceptionCannotLoadConverter, type));
                    }
                    obj = converter.ConvertFromInvariantString(text);
                    break;
                }
            case 4:
                {
                    var num = _store.Read7BitEncodedInt();
                    if (num < 0)
                    {
                        throw new BadImageFormatException(SR.Format(SR.BadImageFormatResourceDataLengthInvalid, num));
                    }
                    var unmanagedMemoryStream = _store.BaseStream as UnmanagedMemoryStream;
                    Stream stream;
                    if (unmanagedMemoryStream != null)
                    {
                        stream = new UnmanagedMemoryStream(unmanagedMemoryStream.PositionPointer, num, num, FileAccess.Read);
                    }
                    else
                    {
                        var buffer = _store.ReadBytes(num);
                        stream = new MemoryStream(buffer, writable: false);
                    }

                    if (type != null)
                    {
                        obj = Activator.CreateInstance(type, stream);
                    }

                    break;
                }
            default:
                throw new BadImageFormatException(SR.BadImageFormatTypeMismatch);
        }
        if (obj?.GetType() != type)
        {
            throw new BadImageFormatException(SR.Format(SR.BadImageFormatResTypeSerBlobMismatch, type?.FullName, obj.GetType().FullName));
        }
        return obj;
    }
    internal class ImageListSerializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            return Type.GetType(typeName);
        }
    }
}