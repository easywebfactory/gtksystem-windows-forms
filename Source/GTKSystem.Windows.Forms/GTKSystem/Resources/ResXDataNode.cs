// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Resources;
using System.Xml;

namespace System.Resources;

public sealed class ResXDataNode : ISerializable
{
    private static readonly char[] specialChars = [' ', '\r', '\n'];

    private DataNodeInfo? _nodeInfo;

    private string? _name;
    private string? _comment;

    private string? _typeName; // is only used when we create a resxdatanode manually with an object and contains the FQN

    private string? _fileRefFullPath;
    private string? _fileRefType;
    private string? _fileRefTextEncoding;

    private object? _value;
    private ResXFileRef? _fileRef;

    //private IFormatter _binaryFormatter;
    private DataContractSerializer? _binaryFormatter;
    // this is going to be used to check if a ResXDataNode is of type ResXFileRef
    private static readonly ITypeResolutionService internalTypeResolver = new AssemblyNamesTypeResolutionService([new("System.Windows.Forms")
    ]);

    // callback function to get type name for multitargeting.
    // No public property to force using constructors for the following reasons:
    // 1. one of the constructors needs this field (if used) to initialize the object, make it consistent with the other constructors to avoid errors.
    // 2. once the object is constructed the delegate should not be changed to avoid getting inconsistent results.
    private Func<Type, string>? _typeNameConverter;

    private ResXDataNode()
    {
    }

    internal ResXDataNode DeepClone()
    {
        return new ResXDataNode
        {
            // nodeinfo is just made up of immutable objects, we don't need to clone it
            _nodeInfo = _nodeInfo?.Clone(),
            _name = _name,
            _comment = _comment,
            _typeName = _typeName,
            _fileRefFullPath = _fileRefFullPath,
            _fileRefType = _fileRefType,
            _fileRefTextEncoding = _fileRefTextEncoding,
            // we don't clone the value, because we don't know how
            _value = _value,
            _fileRef = _fileRef?.Clone(),
            _typeNameConverter = _typeNameConverter
        };
    }

    public ResXDataNode(string? name, object value) : this(name, value, null)
    {
    }

    public ResXDataNode(string? name, object value, Func<Type, string>? typeNameConverter)
    {
        //ArgumentNullException.ThrowIfNull(name);
        if ((name?.Length ?? 0) == 0)
        {
            throw new ArgumentException(nameof(name));
        }

        _typeNameConverter = typeNameConverter;

        var valueType = value is null ? typeof(object) : value.GetType();

        if (value != null && !valueType.IsSerializable)
        {
            throw new InvalidOperationException(string.Format("SR.NotSerializableType,{0},{1}", name, valueType.FullName));
        }

        if (value != null)
        {
            _typeName = MultitargetUtil.GetAssemblyQualifiedName(valueType, _typeNameConverter);
        }

        _name = name;
        _value = value;
    }

    public ResXDataNode(string? name, ResXFileRef fileRef) : this(name, fileRef, null)
    {
    }

    public ResXDataNode(string? name, ResXFileRef fileRef, Func<Type, string>? typeNameConverter)
    {
        _name = name.OrThrowIfNullOrEmpty();
        _fileRef = fileRef.OrThrowIfNull();
        _typeNameConverter = typeNameConverter;
    }

    internal ResXDataNode(DataNodeInfo? nodeInfo, string? basePath)
    {
        _nodeInfo = nodeInfo;
        InitializeDataNode(basePath);
    }

    private void InitializeDataNode(string? basePath)
    {
        // we can only use our internal type resolver here
        // because we only want to check if this is a ResXFileRef node
        // and we can't be sure that we have a typeResolutionService that can
        // recognize this. It's not very clean but this should work.
        Type? nodeType = null;
        if (!string.IsNullOrEmpty(_nodeInfo?.typeName)) // can be null if we have a string (default for string is TypeName == null)
        {
            nodeType = internalTypeResolver.GetType(_nodeInfo!.typeName!, false, true);
        }

        if (nodeType != null && nodeType.Equals(typeof(ResXFileRef)))
        {
            // we have a fileref, split the value data and populate the fields
            var fileRefDetails = ResXFileRef.Converter.ParseResxFileRefString(_nodeInfo!.valueData);
            if (fileRefDetails is { Length: > 1 })
            {
                if (!Path.IsPathRooted(fileRefDetails[0]) && basePath != null)
                {
                    _fileRefFullPath = Path.Combine(basePath, fileRefDetails[0]!);
                }
                else
                {
                    _fileRefFullPath = fileRefDetails[0];
                }

                _fileRefType = fileRefDetails[1];
                if (fileRefDetails.Length > 2)
                {
                    _fileRefTextEncoding = fileRefDetails[2];
                }
            }
        }
    }

    public string? Comment
    {
        get
        {
            var result = _comment;
            if (result is null && _nodeInfo != null)
            {
                result = _nodeInfo.comment;
            }

            return result ?? string.Empty;
        }
        set => _comment = value;
    }

    public string Name
    {
        get
        {
            var result = _name;
            if (result is null && _nodeInfo != null)
            {
                result = _nodeInfo.name;
            }

            return result ?? string.Empty;
        }
        set
        {
            //ArgumentNullException.ThrowIfNull(value, nameof(Name));
            if ((value?.Length ?? 0) == 0)
            {
                throw new ArgumentException(nameof(Name));
            }

            _name = value;
        }
    }

    public ResXFileRef? FileRef
    {
        get
        {
            if (FileRefFullPath is null)
            {
                return null;
            }

            if (_fileRef is null)
            {
                _fileRef =
                    string.IsNullOrEmpty(_fileRefTextEncoding)
                        ? new ResXFileRef(FileRefFullPath, FileRefType)
                        : new ResXFileRef(FileRefFullPath, FileRefType, TextFileEncoding());
            }

            return _fileRef;
        }
    }

    private Encoding TextFileEncoding()
    {
        if (FileRefTextEncoding != null)
        {
            return Encoding.GetEncoding(FileRefTextEncoding);
        }
        return Encoding.UTF8;
    }

    private string? FileRefFullPath => _fileRef?.FileName ?? _fileRefFullPath;

    private string? FileRefType => _fileRef?.TypeName ?? _fileRefType;

    private string? FileRefTextEncoding => _fileRef?.TextFileEncoding?.BodyName ?? _fileRefTextEncoding;

    private static string ToBase64WrappedString(byte[]? data)
    {
        const int lineWrap = 80;
        const string crlf = "\r\n";
        const string prefix = "        ";
        var raw= string.Empty;
        if (data != null)
        {
            raw = Convert.ToBase64String(data);
        }

        if (raw.Length > lineWrap)
        {
            var output = new StringBuilder(raw.Length + raw.Length / lineWrap * 3); // word wrap on lineWrap chars, \r\n
            var current = 0;
            for (; current < raw.Length - lineWrap; current += lineWrap)
            {
                output.Append(crlf);
                output.Append(prefix);
                output.Append(raw, current, lineWrap);
            }

            output.Append(crlf);
            output.Append(prefix);
            output.Append(raw, current, raw.Length - current);
            output.Append(crlf);
            return output.ToString();
        }

        return raw;
    }

    private void FillDataNodeInfoFromObject(DataNodeInfo? nodeInfo, object? value)
    {
        if (value is CultureInfo ci)
        {
            // special-case CultureInfo, cannot use CultureInfoConverter for serialization
            if (nodeInfo != null)
            {
                nodeInfo.valueData = ci.Name;
                nodeInfo.typeName = MultitargetUtil.GetAssemblyQualifiedName(typeof(CultureInfo), _typeNameConverter);
            }
        }
        else if (value is string str)
        {
            if (nodeInfo != null)
            {
                nodeInfo.valueData = str;
            }
        }
        else if (value is byte[] bytes)
        {
            if (nodeInfo != null)
            {
                nodeInfo.valueData = ToBase64WrappedString(bytes);
                nodeInfo.typeName = MultitargetUtil.GetAssemblyQualifiedName(typeof(byte[]), _typeNameConverter);
            }
        }
        else
        {
            var valueType = value is null ? typeof(object) : value.GetType();
            if (value != null && !valueType.IsSerializable)
            {
                throw new InvalidOperationException(string.Format("SR.NotSerializableType,{0},{1}", _name, valueType.FullName));
            }

            var tc = TypeDescriptor.GetConverter(valueType);
            var toString = tc.CanConvertTo(typeof(string));
            var fromString = tc.CanConvertFrom(typeof(string));
            try
            {
                if (toString && fromString)
                {
                    if (nodeInfo != null)
                    {
                        if (value != null)
                        {
                            nodeInfo.valueData = tc.ConvertToInvariantString(value);
                        }

                        nodeInfo.typeName = MultitargetUtil.GetAssemblyQualifiedName(valueType, _typeNameConverter);
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                // Some custom type converters will throw in ConvertTo(string)
                // to indicate that this object should be serialized through ISerializable
                // instead of as a string. This is semi-wrong, but something we will have to
                // live with to allow user created Cursors to be serializable.
                if (ClientUtils.IsCriticalException(ex))
                {
                    throw;
                }
            }

            var toByteArray = tc.CanConvertTo(typeof(byte[]));
            var fromByteArray = tc.CanConvertFrom(typeof(byte[]));
            if (toByteArray && fromByteArray)
            {
                if (value != null)
                {
                    var convertTo = tc.ConvertTo(value, typeof(byte[]));
                    var data = (byte[]?)convertTo;
                    if (nodeInfo != null)
                    {
                        nodeInfo.valueData = ToBase64WrappedString(data);
                    }
                }

                if (nodeInfo != null)
                {
                    nodeInfo.mimeType = ResXResourceWriter.byteArraySerializedObjectMimeType;
                    nodeInfo.typeName = MultitargetUtil.GetAssemblyQualifiedName(valueType, _typeNameConverter);
                }

                return;
            }

            if (value is null)
            {
                if (nodeInfo != null)
                {
                    nodeInfo.valueData = string.Empty;
                    nodeInfo.typeName =
                        MultitargetUtil.GetAssemblyQualifiedName(typeof(ResXNullRef), _typeNameConverter);
                }
            }
            else
            {
                if (_binaryFormatter is null)
                {
                    //_binaryFormatter = new BinaryFormatter
                    //{
                    //    Binder = new ResXSerializationBinder(_typeNameConverter)
                    //};
                    _binaryFormatter = new DataContractSerializer(typeof(ResXNullRef));
                }

                using (var ms = new MemoryStream())
                {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                    //_binaryFormatter.Serialize(ms, value);
                    _binaryFormatter.WriteObject(ms, value);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                    if (nodeInfo != null)
                    {
                        nodeInfo.valueData = ToBase64WrappedString(ms.ToArray());
                    }
                }

                if (nodeInfo != null)
                {
                    nodeInfo.mimeType = ResXResourceWriter.defaultSerializedObjectMimeType;
                }
            }
        }
    }

    private object? GenerateObjectFromDataNodeInfo(DataNodeInfo? dataNodeInfo, ITypeResolutionService? typeResolver)
    {
        object? result = null;
        var mimeTypeName = dataNodeInfo?.mimeType;
        // default behavior: if we don't have a type name, it's a string
        var typeName =
            string.IsNullOrEmpty(dataNodeInfo?.typeName)
                ? MultitargetUtil.GetAssemblyQualifiedName(typeof(string), _typeNameConverter)
                : dataNodeInfo?.typeName;

        if (!string.IsNullOrEmpty(mimeTypeName))
        {
            if (string.Equals(mimeTypeName, ResXResourceWriter.binSerializedObjectMimeType))
            {
                var text = dataNodeInfo?.valueData;
                var serializedData = FromBase64WrappedString(text);

                if (_binaryFormatter is null)
                {
                    //_binaryFormatter = new BinaryFormatter
                    //{
                    //    Binder = new ResXSerializationBinder(typeResolver)
                    //};
                    _binaryFormatter = new DataContractSerializer(typeof(ResXNullRef));
                }

                //IFormatter formatter = _binaryFormatter;
                if (serializedData is { Length: > 0 })
                {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                    // result = formatter.Deserialize(new MemoryStream(serializedData));
                    result = _binaryFormatter.ReadObject(new MemoryStream(serializedData));
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                    if (result is ResXNullRef)
                    {
                        result = null;
                    }
                }
            }
            else if (string.Equals(mimeTypeName, ResXResourceWriter.byteArraySerializedObjectMimeType))
            {
                if (!string.IsNullOrEmpty(typeName))
                {
                    var type = ResolveType(typeName, typeResolver);
                    if (type != null)
                    {
                        var tc = TypeDescriptor.GetConverter(type);
                        if (tc.CanConvertFrom(typeof(byte[])))
                        {
                            var text = dataNodeInfo?.valueData;
                            var serializedData = FromBase64WrappedString(text);

                            if (serializedData != null)
                            {
                                result = tc.ConvertFrom(serializedData);
                            }
                        }
                    }
                    else
                    {
                        var newMessage = string.Format("SR.TypeLoadException,{0},{1},{2}", typeName, dataNodeInfo?.readerPosition.Y, dataNodeInfo?.readerPosition.X);
                        var xml = new XmlException(newMessage, null, dataNodeInfo?.readerPosition.Y??0, dataNodeInfo?.readerPosition.X??0);
                        var newTle = new TypeLoadException(newMessage, xml);

                        throw newTle;
                    }
                }
            }
        }
        else if (!string.IsNullOrEmpty(typeName))
        {
            var type = ResolveType(typeName, typeResolver);
            if (type != null)
            {
                if (type == typeof(ResXNullRef))
                {
                    result = null;
                }
                else if (type == typeof(byte[]) ||
                         ((typeName??string.Empty).Contains("System.Byte[]") && ((typeName ?? string.Empty).Contains("mscorlib") || (typeName ?? string.Empty).Contains("System.Private.CoreLib"))))
                {
                    // Handle byte[]'s, which are stored as base-64 encoded strings.
                    // We can't hard-code byte[] type name due to version number
                    // updates & potential whitespace issues with ResX files.
                    result = FromBase64WrappedString(dataNodeInfo?.valueData);
                }
                else
                {
                    var tc = TypeDescriptor.GetConverter(type);
                    if (tc.CanConvertFrom(typeof(string)))
                    {
                        var text = dataNodeInfo?.valueData;
                        try
                        {
                            result = tc.ConvertFromInvariantString(text);
                        }
                        catch (NotSupportedException nse)
                        {
                            var newMessage = string.Format("SR.NotSupported,{0},{1},{2},{3}", typeName, dataNodeInfo?.readerPosition.Y, dataNodeInfo?.readerPosition.X, nse.Message);
                            var xml = new XmlException(newMessage, nse, dataNodeInfo?.readerPosition.Y ?? 0, dataNodeInfo?.readerPosition.X??0);
                            var newNse = new NotSupportedException(newMessage, xml);
                            throw newNse;
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Converter for " + type.FullName + " doesn't support string conversion");
                    }
                }
            }
            else
            {
                var newMessage = string.Format("SR.TypeLoadException,{0},{1},{2}", typeName, dataNodeInfo?.readerPosition.Y, dataNodeInfo?.readerPosition.X);
                var xml = new XmlException(newMessage, null, dataNodeInfo?.readerPosition.Y??0, dataNodeInfo?.readerPosition.X??0);
                var newTle = new TypeLoadException(newMessage, xml);

                throw newTle;
            }
        }
        else
        {
            // if mimeTypeName and typeName are not filled in, the value must be a string
            Debug.Assert(_value is string, "Resource entries with no Type or MimeType must be encoded as strings");
        }

        return result;
    }

    internal DataNodeInfo? GetDataNodeInfo()
    {
        var shouldSerialize = true;
        if (_nodeInfo != null)
        {
            shouldSerialize = false;
        }
        else
        {
            _nodeInfo = new DataNodeInfo();
        }

        _nodeInfo.name = Name;
        _nodeInfo.comment = Comment;

        // We always serialize if this node represents a FileRef. This is because FileRef is a public property,
        // so someone could have modified it.
        if (shouldSerialize || FileRefFullPath != null)
        {
            // if we don't have a datanodeinfo it could be either
            // a direct object OR a fileref
            if (FileRefFullPath != null)
            {
                _nodeInfo.valueData = FileRef?.ToString();
                _nodeInfo.mimeType = null;
                _nodeInfo.typeName = MultitargetUtil.GetAssemblyQualifiedName(typeof(ResXFileRef), _typeNameConverter);
            }
            else
            {
                // serialize to string inside the nodeInfo
                FillDataNodeInfoFromObject(_nodeInfo, _value);
            }
        }

        return _nodeInfo;
    }

    /// <summary>
    ///  Might return the position in the resx file of the current node, if known
    ///  otherwise, will return Point(0,0) since point is a struct
    /// </summary>
    public Point GetNodePosition()
    {
        return _nodeInfo?.readerPosition ?? new Point();
    }

    /// <summary>
    ///  Get the FQ type name for this datanode.
    ///  We return typeof(object) for ResXNullRef
    /// </summary>
    public string? GetValueTypeName(ITypeResolutionService? typeResolver)
    {
        // the type name here is always a FQN
        if (!string.IsNullOrEmpty(_typeName))
        {
            return
                _typeName == MultitargetUtil.GetAssemblyQualifiedName(typeof(ResXNullRef), _typeNameConverter)
                    ? MultitargetUtil.GetAssemblyQualifiedName(typeof(object), _typeNameConverter)
                    : _typeName;
        }

        var result = FileRefType;
        Type? objectType = null;
        // do we have a fileref?
        if (result != null)
        {
            // try to resolve this type
            objectType = ResolveType(FileRefType, typeResolver);
        }
        else if (_nodeInfo != null)
        {
            // we don't have a fileref, try to resolve the type of the datanode
            result = _nodeInfo.typeName;
            // if typename is null, the default is just a string
            if (string.IsNullOrEmpty(result))
            {
                // we still don't know... do we have a mimetype? if yes, our only option is to
                // deserialize to know what we're dealing with... very inefficient...
                if (!string.IsNullOrEmpty(_nodeInfo.mimeType))
                {
                    object? insideObject = null;

                    try
                    {
                        insideObject = GenerateObjectFromDataNodeInfo(_nodeInfo, typeResolver);
                    }
                    catch (Exception ex)
                    {
                        // it'd be better to catch SerializationException but the underlying type resolver
                        // can throw things like FileNotFoundException which is kinda confusing, so I am catching all here..
                        if (ClientUtils.IsCriticalException(ex))
                        {
                            throw;
                        }

                        // something went wrong, type is not specified at all or stream is corrupted
                        // return system.object
                        result = MultitargetUtil.GetAssemblyQualifiedName(typeof(object), _typeNameConverter);
                    }

                    if (insideObject != null)
                    {
                        result = MultitargetUtil.GetAssemblyQualifiedName(insideObject.GetType(), _typeNameConverter);
                    }
                }
                else
                {
                    // no typename, no mimetype, we have a string...
                    result = MultitargetUtil.GetAssemblyQualifiedName(typeof(string), _typeNameConverter);
                }
            }
            else
            {
                objectType = ResolveType(_nodeInfo.typeName, typeResolver);
            }
        }

        if (objectType != null)
        {
            if (objectType == typeof(ResXNullRef))
            {
                result = MultitargetUtil.GetAssemblyQualifiedName(typeof(object), _typeNameConverter);
            }
            else
            {
                result = MultitargetUtil.GetAssemblyQualifiedName(objectType, _typeNameConverter);
            }
        }

        return result;
    }

    /// <summary>
    ///  Get the FQ type name for this datanode
    /// </summary>
    public string? GetValueTypeName(AssemblyName[]? names)
    {
        return GetValueTypeName(new AssemblyNamesTypeResolutionService(names));
    }

    /// <summary>
    ///  Get the value contained in this datanode
    /// </summary>
    public object? GetValue(ITypeResolutionService? typeResolver)
    {
        if (_value != null)
        {
            return _value;
        }

        object? result;
        if (FileRefFullPath != null)
        {
            var objectType = ResolveType(FileRefType, typeResolver);
            if (objectType != null)
            {
                // we have the FQN for this type
                _fileRef =
                    FileRefTextEncoding != null
                        ? new ResXFileRef(FileRefFullPath, FileRefType, Encoding.GetEncoding(FileRefTextEncoding))
                        : new ResXFileRef(FileRefFullPath, FileRefType);
                var tc = TypeDescriptor.GetConverter(typeof(ResXFileRef));
                result = tc.ConvertFrom(_fileRef.ToString());
            }
            else
            {
                var newMessage = string.Format("R.TypeLoadExceptionShort,{0}", FileRefType);
                var newTle = new TypeLoadException(newMessage);
                throw newTle;
            }
        }
        else if (_nodeInfo?.valueData != null)
        {
            // it's embedded, we deserialize it
            result = GenerateObjectFromDataNodeInfo(_nodeInfo, typeResolver);
        }
        else
        {
            // schema is wrong and say minOccur for Value is 0,
            // but it's too late to change it...
            // we need to return null here
            return null;
        }

        return result;
    }

    /// <summary>
    ///  Get the value contained in this datanode
    /// </summary>
    public object? GetValue(AssemblyName[]? names)
    {
        return GetValue(new AssemblyNamesTypeResolutionService(names));
    }

    private static byte[] FromBase64WrappedString(string? text)
    {
        if ((text?.IndexOfAny(specialChars)??-1) != -1)
        {
            var sb = new StringBuilder(text!.Length);
            foreach (var ch in text)
            {
                switch (ch)
                {
                    case ' ':
                    case '\r':
                    case '\n':
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }

            return Convert.FromBase64String(sb.ToString());
        }

        return Convert.FromBase64String(text??string.Empty);
    }

    private static Type? ResolveType(string? typeName, ITypeResolutionService? typeResolver)
    {
        Type? resolvedType = null;
        if (typeResolver != null)
        {
            // If we cannot find the strong-named type, then try to see
            // if the TypeResolver can bind to partial names. For this,
            // we will strip out the partial names and keep the rest of the
            // strong-name information to try again.

            resolvedType = typeResolver.GetType(typeName??string.Empty, false);
            if (resolvedType is null)
            {
                var typeParts = (typeName??string.Empty).Split(',');

                // Break up the type name from the rest of the assembly strong name.
                if (typeParts is { Length: >= 2 })
                {
                    var partialName = typeParts[0].Trim();
                    var assemblyName = typeParts[1].Trim();
                    partialName = partialName + ", " + assemblyName;
                    resolvedType = typeResolver.GetType(partialName, false);
                }
            }
        }

        if (resolvedType is null && typeName != null)
        {
            resolvedType = Type.GetType(typeName, false);
        }

        return resolvedType;
    }

    /// <summary>
    ///  Get the value contained in this datanode
    /// </summary>
    void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
    {
        throw new PlatformNotSupportedException();
    }
}