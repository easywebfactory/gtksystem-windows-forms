using System.Buffers.Binary;
using System.Collections;
using System.ComponentModel;
using System.Resources;
using System.Resources.Extensions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Resources.IO;
using System.Windows.Forms;
using GtkSystem.Resources.Extensions;

namespace System.Resources;

public sealed class GtkDeserializingResourceReader : IResourceReader
{
    internal sealed class ResourceEnumerator : IDictionaryEnumerator
    {
        private const int ENUM_DONE = int.MinValue;

        private const int ENUM_NOT_STARTED = -1;

        private readonly GtkDeserializingResourceReader _reader;

        private bool _currentIsValid;

        private int _currentName;

        private int _dataPosition;

        public object Key
        {
            get
            {
                if (_currentName == int.MinValue)
                {
                    throw new InvalidOperationException(SystemResources.InvalidOperation_EnumEnded);
                }
                if (!_currentIsValid)
                {
                    throw new InvalidOperationException(SystemResources.InvalidOperation_EnumNotStarted);
                }
                if (_reader._resCache == null)
                {
                    throw new InvalidOperationException(SystemResources.ResourceReaderIsClosed);
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
                    throw new InvalidOperationException(SystemResources.InvalidOperation_EnumEnded);
                }
                if (!_currentIsValid)
                {
                    throw new InvalidOperationException(SystemResources.InvalidOperation_EnumNotStarted);
                }
                if (_reader._resCache == null)
                {
                    throw new InvalidOperationException(SystemResources.ResourceReaderIsClosed);
                }
                var key = _reader.AllocateStringForNameIndex(_currentName, out _dataPosition);
                object? obj = null;
                lock (_reader._resCache)
                {
                    if (_reader._resCache.TryGetValue(key, out var value))
                    {
                        obj = value.Value;
                    }
                }
                if (obj == null)
                {
                    obj = ((_dataPosition != -1) ? _reader.LoadObject(_dataPosition) : _reader.GetValueForNameIndex(_currentName));
                }
                return new DictionaryEntry(key, obj);
            }
        }

        public object Value
        {
            get
            {
                if (_currentName == int.MinValue)
                {
                    throw new InvalidOperationException(SystemResources.InvalidOperation_EnumEnded);
                }
                if (!_currentIsValid)
                {
                    throw new InvalidOperationException(SystemResources.InvalidOperation_EnumNotStarted);
                }
                if (_reader._resCache == null)
                {
                    throw new InvalidOperationException(SystemResources.ResourceReaderIsClosed);
                }
                return _reader.GetValueForNameIndex(_currentName);
            }
        }

        internal ResourceEnumerator(GtkDeserializingResourceReader reader)
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
                throw new InvalidOperationException(SystemResources.ResourceReaderIsClosed);
            }
            _currentIsValid = false;
            _currentName = -1;
        }
    }

    internal sealed class UndoTruncatedTypeNameSerializationBinder : SerializationBinder
    {
        public override Type? BindToType(string assemblyName, string typeName)
        {
            var patch = false;
            if (assemblyName == "System.Windows.Forms, Culture=neutral, PublicKeyToken=b77a5c561934e089")
            {
                if (typeName == "System.Windows.Forms.ImageListStreamer")
                {
                    assemblyName = typeof(ImageListStreamer).Assembly.FullName;
                }
                else
                {
                    assemblyName = typeof(UndoTruncatedTypeNameSerializationBinder).Assembly.FullName;
                }
                patch = true;
            }
            Type? result = null;
            if (typeName != null && assemblyName != null && (!AreBracketsBalanced(typeName)||patch))
            {
                typeName = typeName + ", " + assemblyName;
                result = Type.GetType(typeName, throwOnError: false, ignoreCase: false);
            }
            return result;
        }

        private static bool AreBracketsBalanced(string typeName)
        {
            var num = typeName.IndexOf('[');
            if (num == -1)
            {
                return true;
            }
            var num2 = 1;
            for (var i = num + 1; i < typeName.Length; i++)
            {
                if (typeName[i] == '[')
                {
                    num2++;
                }
                else if (typeName[i] == ']')
                {
                    num2--;
                    if (num2 < 0)
                    {
                        break;
                    }
                }
            }
            return num2 == 0;
        }
    }

    private const int DefaultFileStreamBufferSize = 4096;

    private BinaryReader _store;

    internal Dictionary<string, ResourceLocator> _resCache;

    private long _nameSectionOffset;

    private long _dataSectionOffset;

    private int[]? _nameHashes;

    private unsafe int* _nameHashesPtr;

    private int[]? _namePositions;

    private unsafe int* _namePositionsPtr;

    private Type[]? _typeTable;

    private int[]? _typeNamePositions;

    private int _numResources;

    private UnmanagedMemoryStream? _ums;

    private int _version;

    private bool _assumeBinaryFormatter;

    private BinaryFormatter? _formatter;

    internal static bool AllowCustomResourceTypes { get; } = !AppContext.TryGetSwitch("System.Resources.ResourceManager.AllowCustomResourceTypes", out var isEnabled) || isEnabled;


    public GtkDeserializingResourceReader(string fileName)
    {
        _resCache = new Dictionary<string, ResourceLocator>(FastResourceComparer.@default);
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

    public GtkDeserializingResourceReader(Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }
        if (!stream.CanRead)
        {
            throw new ArgumentException(SystemResources.Argument_StreamNotReadable);
        }
        _resCache = new Dictionary<string, ResourceLocator>(FastResourceComparer.@default);
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
                _store = null;
                store?.Close();
            }
            _store = null;
            _namePositions = null;
            _nameHashes = null;
            _ums = null;
            _namePositionsPtr = null;
            _nameHashesPtr = null;
        }
    }

    private unsafe static int ReadUnalignedI4(int* p)
    {
        return BinaryPrimitives.ReadInt32LittleEndian(new ReadOnlySpan<byte>(p, 4));
    }

    private void SkipString()
    {
        var num = _store.Read7BitEncodedInt();
        if (num < 0)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_NegativeStringLength);
        }
        _store.BaseStream.Seek(num, SeekOrigin.Current);
    }

    private unsafe int GetNameHash(int index)
    {
        if (_ums == null)
        {
            return _nameHashes[index];
        }
        return ReadUnalignedI4(_nameHashesPtr + index);
    }

    private unsafe int GetNamePosition(int index)
    {
        var num = ((_ums != null) ? ReadUnalignedI4(_namePositionsPtr + index) : _namePositions[index]);
        if (num < 0 || num > _dataSectionOffset - _nameSectionOffset)
        {
            throw new FormatException(Messages.Format(SystemResources.BadImageFormat_ResourcesNameInvalidOffset, num));
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
            throw new InvalidOperationException(SystemResources.ResourceReaderIsClosed);
        }
        return new ResourceEnumerator(this);
    }

    internal ResourceEnumerator GetEnumeratorInternal()
    {
        return new ResourceEnumerator(this);
    }

    internal int FindPosForResource(string name)
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
            var num4 = ((nameHash != num) ? ((nameHash >= num) ? 1 : (-1)) : 0);
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
                _store.BaseStream.Seek(_nameSectionOffset + GetNamePosition(j), SeekOrigin.Begin);
                if (CompareStringEqualsName(name))
                {
                    var num5 = _store.ReadInt32();
                    if (num5 < 0 || num5 >= _store.BaseStream.Length - _dataSectionOffset)
                    {
                        throw new FormatException(Messages.Format(SystemResources.BadImageFormat_ResourcesDataInvalidOffset, num5));
                    }
                    return num5;
                }
            }
        }
        return -1;
    }

    private unsafe bool CompareStringEqualsName(string name)
    {
        var num = _store.Read7BitEncodedInt();
        if (num < 0)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_NegativeStringLength);
        }
        if (_ums != null)
        {
            var positionPointer = _ums.PositionPointer;
            _ums.Seek(num, SeekOrigin.Current);
            if (_ums.Position > _ums.Length)
            {
                throw new BadImageFormatException(SystemResources.BadImageFormat_ResourcesNameTooLong);
            }
            return FastResourceComparer.CompareOrdinal(positionPointer, num, name) == 0;
        }
        var array = new byte[num];
        var num2 = num;
        while (num2 > 0)
        {
            var num3 = _store.Read(array, num - num2, num2);
            if (num3 == 0)
            {
                throw new BadImageFormatException(SystemResources.BadImageFormat_ResourceNameCorrupted);
            }
            num2 -= num3;
        }
        return FastResourceComparer.CompareOrdinal(array, num / 2, name) == 0;
    }

    private unsafe string AllocateStringForNameIndex(int index, out int dataOffset)
    {
        long num = GetNamePosition(index);
        int num2;
        byte[] array2;
        lock (this)
        {
            _store.BaseStream.Seek(num + _nameSectionOffset, SeekOrigin.Begin);
            num2 = _store.Read7BitEncodedInt();
            if (num2 < 0)
            {
                throw new BadImageFormatException(SystemResources.BadImageFormat_NegativeStringLength);
            }
            if (_ums != null)
            {
                if (_ums.Position > _ums.Length - num2)
                {
                    throw new BadImageFormatException(Messages.Format(SystemResources.BadImageFormat_ResourcesIndexTooLong, index));
                }
                string? text = null;
                var positionPointer = (char*)_ums.PositionPointer;
                if (BitConverter.IsLittleEndian)
                {
                    text = new string(positionPointer, 0, num2 / 2);
                }
                else
                {
                    var array = new char[num2 / 2];
                    for (var i = 0; i < array.Length; i++)
                    {
                        array[i] = (char)BinaryPrimitives.ReverseEndianness((short)positionPointer[i]);
                    }
                    text = new string(array);
                }
                _ums.Position += num2;
                dataOffset = _store.ReadInt32();
                if (dataOffset < 0 || dataOffset >= _store.BaseStream.Length - _dataSectionOffset)
                {
                    throw new FormatException(Messages.Format(SystemResources.BadImageFormat_ResourcesDataInvalidOffset, dataOffset));
                }
                return text;
            }
            array2 = new byte[num2];
            var num3 = num2;
            while (num3 > 0)
            {
                var num4 = _store.Read(array2, num2 - num3, num3);
                if (num4 == 0)
                {
                    throw new EndOfStreamException(Messages.Format(SystemResources.BadImageFormat_ResourceNameCorrupted_NameIndex, index));
                }
                num3 -= num4;
            }
            dataOffset = _store.ReadInt32();
            if (dataOffset < 0 || dataOffset >= _store.BaseStream.Length - _dataSectionOffset)
            {
                throw new FormatException(Messages.Format(SystemResources.BadImageFormat_ResourcesDataInvalidOffset, dataOffset));
            }
        }
        return Encoding.Unicode.GetString(array2, 0, num2);
    }

    private object GetValueForNameIndex(int index)
    {
        long num = GetNamePosition(index);
        lock (this)
        {
            _store.BaseStream.Seek(num + _nameSectionOffset, SeekOrigin.Begin);
            SkipString();
            var num2 = _store.ReadInt32();
            if (num2 < 0 || num2 >= _store.BaseStream.Length - _dataSectionOffset)
            {
                throw new FormatException(Messages.Format(SystemResources.BadImageFormat_ResourcesDataInvalidOffset, num2));
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
                    throw new InvalidOperationException(Messages.Format(SystemResources.InvalidOperation_ResourceNotString_Type, FindType(num).FullName));
                }
                result = _store.ReadString();
            }
            else
            {
                var resourceTypeCode = (ResourceTypeCode)num;
                if (resourceTypeCode != ResourceTypeCode.String && resourceTypeCode != 0)
                {
                    throw new InvalidOperationException(Messages.Format(p1: (resourceTypeCode >= ResourceTypeCode.StartOfUserTypes) ? FindType((int)(resourceTypeCode - 64)).FullName : resourceTypeCode.ToString(), resourceFormat: SystemResources.InvalidOperation_ResourceNotString_Type));
                }
                if (resourceTypeCode == ResourceTypeCode.String)
                {
                    result = _store.ReadString();
                }
            }
            return result;
        }
    }

    internal object LoadObject(int pos)
    {
        lock (this)
        {
            ResourceTypeCode typeCode;
            return (_version == 1) ? LoadObjectV1(pos) : LoadObjectV2(pos, out typeCode);
        }
    }

    internal object LoadObject(int pos, out ResourceTypeCode typeCode)
    {
        lock (this)
        {
            if (_version == 1)
            {
                var obj = LoadObjectV1(pos);
                typeCode = ((obj is string) ? ResourceTypeCode.String : ResourceTypeCode.StartOfUserTypes);
                return obj;
            }
            return LoadObjectV2(pos, out typeCode);
        }
    }

    private object LoadObjectV1(int pos)
    {
        try
        {
            return _LoadObjectV1(pos);
        }
        catch (EndOfStreamException inner)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_TypeMismatch, inner);
        }
        catch (ArgumentOutOfRangeException inner2)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_TypeMismatch, inner2);
        }
    }

    private object _LoadObjectV1(int pos)
    {
        _store.BaseStream.Seek(_dataSectionOffset + pos, SeekOrigin.Begin);
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

    private object LoadObjectV2(int pos, out ResourceTypeCode typeCode)
    {
        try
        {
            return _LoadObjectV2(pos, out typeCode);
        }
        catch (EndOfStreamException inner)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_TypeMismatch, inner);
        }
        catch (ArgumentOutOfRangeException inner2)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_TypeMismatch, inner2);
        }
    }

    private unsafe object _LoadObjectV2(int pos, out ResourceTypeCode typeCode)
    {
        _store.BaseStream.Seek(_dataSectionOffset + pos, SeekOrigin.Begin);
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
                        throw new BadImageFormatException(Messages.Format(SystemResources.BadImageFormat_ResourceDataLengthInvalid, num2));
                    }
                    if (_ums == null)
                    {
                        if (num2 > _store.BaseStream.Length)
                        {
                            throw new BadImageFormatException(Messages.Format(SystemResources.BadImageFormat_ResourceDataLengthInvalid, num2));
                        }
                        return _store.ReadBytes(num2);
                    }
                    if (num2 > _ums.Length - _ums.Position)
                    {
                        throw new BadImageFormatException(Messages.Format(SystemResources.BadImageFormat_ResourceDataLengthInvalid, num2));
                    }
                    var array2 = new byte[num2];
                    var num3 = _ums.Read(array2, 0, num2);
                    return array2;
                }
            case ResourceTypeCode.Stream:
                {
                    var num = _store.ReadInt32();
                    if (num < 0)
                    {
                        throw new BadImageFormatException(Messages.Format(SystemResources.BadImageFormat_ResourceDataLengthInvalid, num));
                    }
                    if (_ums == null)
                    {
                        var array = _store.ReadBytes(num);
                        return new PinnedBufferMemoryStream(array);
                    }
                    if (num > _ums.Length - _ums.Position)
                    {
                        throw new BadImageFormatException(Messages.Format(SystemResources.BadImageFormat_ResourceDataLengthInvalid, num));
                    }
                    return new UnmanagedMemoryStream(_ums.PositionPointer, num, num, FileAccess.Read);
                }
            default:
                {
                    if (typeCode < ResourceTypeCode.StartOfUserTypes)
                    {
                        throw new BadImageFormatException(SystemResources.BadImageFormat_TypeMismatch);
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
            throw new BadImageFormatException(SystemResources.BadImageFormat_ResourcesHeaderCorrupted, inner);
        }
        catch (IndexOutOfRangeException inner2)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_ResourcesHeaderCorrupted, inner2);
        }
    }

    private unsafe void _ReadResources()
    {
        var num = _store.ReadInt32();
        if (num != ResourceManager.MagicNumber)
        {
            throw new ArgumentException(SystemResources.Resources_StreamNotValid);
        }
        var num2 = _store.ReadInt32();
        var num3 = _store.ReadInt32();
        if (num3 < 0 || num2 < 0)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_ResourcesHeaderCorrupted);
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
                throw new NotSupportedException(Messages.Format(SystemResources.NotSupported_WrongResourceReader_Type, text));
            }
            SkipString();
        }
        var num4 = _store.ReadInt32();
        if (num4 != 2 && num4 != 1)
        {
            throw new ArgumentException(Messages.Format(SystemResources.Arg_ResourceFileUnsupportedVersion, 2, num4));
        }
        _version = num4;
        _numResources = _store.ReadInt32();
        if (_numResources < 0)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_ResourcesHeaderCorrupted);
        }
        var num5 = _store.ReadInt32();
        if (num5 < 0)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_ResourcesHeaderCorrupted);
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
                throw new BadImageFormatException(SystemResources.BadImageFormat_ResourcesHeaderCorrupted);
            }
            _nameHashesPtr = (int*)_ums.PositionPointer;
            _ums.Seek(num7, SeekOrigin.Current);
            _ = _ums.PositionPointer;
        }
        if (_ums == null)
        {
            _namePositions = new int[_numResources];
            for (var l = 0; l < _numResources; l++)
            {
                var num8 = _store.ReadInt32();
                if (num8 < 0)
                {
                    throw new BadImageFormatException(SystemResources.BadImageFormat_ResourcesHeaderCorrupted);
                }
                _namePositions[l] = num8;
            }
        }
        else
        {
            var num9 = 4 * _numResources;
            if (num9 < 0)
            {
                throw new BadImageFormatException(SystemResources.BadImageFormat_ResourcesHeaderCorrupted);
            }
            _namePositionsPtr = (int*)_ums.PositionPointer;
            _ums.Seek(num9, SeekOrigin.Current);
            _ = _ums.PositionPointer;
        }
        _dataSectionOffset = _store.ReadInt32();
        if (_dataSectionOffset < 0)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_ResourcesHeaderCorrupted);
        }
        _nameSectionOffset = _store.BaseStream.Position;
        if (_dataSectionOffset < _nameSectionOffset)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_ResourcesHeaderCorrupted);
        }
    }

    private Type FindType(int typeIndex)
    {
        if (!AllowCustomResourceTypes)
        {
            throw new NotSupportedException("ReflectionNotAllowed");
        }
        if (typeIndex < 0 || typeIndex >= _typeTable.Length)
        {
            throw new BadImageFormatException(SystemResources.BadImageFormat_InvalidType);
        }
        return _typeTable[typeIndex] ?? UseReflectionToGetType(typeIndex);
    }

    private Type UseReflectionToGetType(int typeIndex)
    {
        var position = _store.BaseStream.Position;
        try
        {
            _store.BaseStream.Position = _typeNamePositions[typeIndex];
            var typeName = _store.ReadString();
            _typeTable[typeIndex] = Type.GetType(typeName, throwOnError: true)!;
            return _typeTable[typeIndex];
        }
        finally
        {
            _store.BaseStream.Position = position;
        }
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
            _store.BaseStream.Position = _typeNamePositions[num];
            return _store.ReadString();
        }
        finally
        {
            _store.BaseStream.Position = position;
        }
    }

    private bool ValidateReaderType(string readerType)
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

    private object ReadBinaryFormattedObject()
    {
        //IL_0009: Unknown result type (might be due to invalid IL or missing references)
        //IL_000e: Unknown result type (might be due to invalid IL or missing references)
        //IL_001e: Expected O, but got Unknown
        if (_formatter == null)
        {
            _formatter = new BinaryFormatter
            {
                Binder = (SerializationBinder)(object)new UndoTruncatedTypeNameSerializationBinder()
            };
        }
        return _formatter.Deserialize(_store.BaseStream);
    }

    private unsafe object DeserializeObject(int typeIndex)
    {
        var type = FindType(typeIndex);
        if (_assumeBinaryFormatter)
        {
            return ReadBinaryFormattedObject();
        }
        object? obj;
        switch ((SerializationFormat)_store.Read7BitEncodedInt())
        {
            case SerializationFormat.BinaryFormatter:
                {
                    var num3 = _store.Read7BitEncodedInt();
                    if (num3 < 0)
                    {
                        throw new BadImageFormatException(Messages.Format(SystemResources.BadImageFormat_ResourceDataLengthInvalid, num3));
                    }
                    var position = _store.BaseStream.Position;
                    obj = ReadBinaryFormattedObject();
                    if (type == typeof(UnknownType))
                    {
                        type = obj.GetType();
                    }
                    var num4 = _store.BaseStream.Position - position;
                    if (num4 != num3)
                    {
                        throw new BadImageFormatException(Messages.Format(SystemResources.BadImageFormat_ResourceDataLengthInvalid, num3));
                    }
                    break;
                }
            case SerializationFormat.TypeConverterByteArray:
                {
                    var num2 = _store.Read7BitEncodedInt();
                    if (num2 < 0)
                    {
                        throw new BadImageFormatException(Messages.Format(SystemResources.BadImageFormat_ResourceDataLengthInvalid, num2));
                    }
                    var value = _store.ReadBytes(num2);
                    var converter = TypeDescriptor.GetConverter(type);
                    if (converter == null)
                    {
                        throw new TypeLoadException(Messages.Format(SystemResources.TypeLoadException_CannotLoadConverter, type));
                    }
                    obj = converter.ConvertFrom(value);
                    break;
                }
            case SerializationFormat.TypeConverterString:
                {
                    var text = _store.ReadString();
                    var converter2 = TypeDescriptor.GetConverter(type);
                    if (converter2 == null)
                    {
                        throw new TypeLoadException(Messages.Format(SystemResources.TypeLoadException_CannotLoadConverter, type));
                    }
                    obj = converter2.ConvertFromInvariantString(text);
                    break;
                }
            case SerializationFormat.ActivatorStream:
                {
                    var num = _store.Read7BitEncodedInt();
                    if (num < 0)
                    {
                        throw new BadImageFormatException(Messages.Format(SystemResources.BadImageFormat_ResourceDataLengthInvalid, num));
                    }
                    Stream stream;
                    if (_store.BaseStream is UnmanagedMemoryStream unmanagedMemoryStream)
                    {
                        stream = new UnmanagedMemoryStream(unmanagedMemoryStream.PositionPointer, num, num, FileAccess.Read);
                    }
                    else
                    {
                        var buffer = _store.ReadBytes(num);
                        stream = new MemoryStream(buffer, writable: false);
                    }
                    obj = Activator.CreateInstance(type, stream);
                    break;
                }
            default:
                throw new BadImageFormatException(SystemResources.BadImageFormat_TypeMismatch);
        }
        if (obj.GetType() != type)
        {
            throw new BadImageFormatException(Messages.Format(SystemResources.BadImageFormat_ResType_SerBlobMismatch, type.FullName, obj.GetType().FullName));
        }
        return obj;
    }
}
