﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Xml;

namespace System.Resources;

/// <summary>
///  ResX resource writer. See the text in "ResourceSchema" for more
///  information.
/// </summary>
public class ResXResourceWriter : IResourceWriter
{
    internal const string typeStr = "type";
    internal const string nameStr = "name";
    internal const string dataStr = "data";
    internal const string metadataStr = "metadata";
    internal const string mimeTypeStr = "mimetype";
    internal const string valueStr = "value";
    internal const string resHeaderStr = "resheader";
    internal const string versionStr = "version";
    internal const string resMimeTypeStr = "resmimetype";
    internal const string readerStr = "reader";
    internal const string writerStr = "writer";
    internal const string commentStr = "comment";
    internal const string assemblyStr = "assembly";
    internal const string aliasStr = "alias";

    private Hashtable? _cachedAliases;

    private static readonly TraceSwitch resValueProviderSwitch = new("ResX", "Debug the resource value provider");

#pragma warning disable IDE1006 // Naming Styles (Shipped public API)
    public static readonly string binSerializedObjectMimeType = "application/x-microsoft.net.object.binary.base64";
    public static readonly string soapSerializedObjectMimeType = "application/x-microsoft.net.object.soap.base64";
    public static readonly string defaultSerializedObjectMimeType = binSerializedObjectMimeType;
    public static readonly string byteArraySerializedObjectMimeType = "application/x-microsoft.net.object.bytearray.base64";
    public static readonly string ResMimeType = "text/microsoft-resx";
    public static readonly string version = "2.0";

    public static readonly string resourceSchema = @"
    <xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
        <xsd:import namespace=""http://www.w3.org/XML/1998/namespace""/>
        <xsd:element name=""root"" msdata:IsDataSet=""true"">
            <xsd:complexType>
                <xsd:choice maxOccurs=""unbounded"">
                    <xsd:element name=""metadata"">
                        <xsd:complexType>
                            <xsd:sequence>
                            <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0""/>
                            </xsd:sequence>
                            <xsd:attribute name=""name"" use=""required"" type=""xsd:string""/>
                            <xsd:attribute name=""type"" type=""xsd:string""/>
                            <xsd:attribute name=""mimetype"" type=""xsd:string""/>
                            <xsd:attribute ref=""xml:space""/>
                        </xsd:complexType>
                    </xsd:element>
                    <xsd:element name=""assembly"">
                      <xsd:complexType>
                        <xsd:attribute name=""alias"" type=""xsd:string""/>
                        <xsd:attribute name=""name"" type=""xsd:string""/>
                      </xsd:complexType>
                    </xsd:element>
                    <xsd:element name=""data"">
                        <xsd:complexType>
                            <xsd:sequence>
                                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                                <xsd:element name=""comment"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""2"" />
                            </xsd:sequence>
                            <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" msdata:Ordinal=""1"" />
                            <xsd:attribute name=""type"" type=""xsd:string"" msdata:Ordinal=""3"" />
                            <xsd:attribute name=""mimetype"" type=""xsd:string"" msdata:Ordinal=""4"" />
                            <xsd:attribute ref=""xml:space""/>
                        </xsd:complexType>
                    </xsd:element>
                    <xsd:element name=""resheader"">
                        <xsd:complexType>
                            <xsd:sequence>
                                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                            </xsd:sequence>
                            <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" />
                        </xsd:complexType>
                    </xsd:element>
                </xsd:choice>
            </xsd:complexType>
        </xsd:element>
        </xsd:schema>
        ";
#pragma warning restore IDE1006 // Naming Styles

    private readonly string? _fileName;
    private Stream? _stream;
    private TextWriter? _textWriter;
    private XmlTextWriter? _xmlTextWriter;

    private bool _hasBeenSaved;
    private bool _initialized;

    private readonly Func<Type, string>? _typeNameConverter; // no public property to be consistent with ResXDataNode class.
    private string? basePath;

    /// <summary>
    ///  Base Path for ResXFileRefs.
    /// </summary>
    public string? BasePath
    {
        get => basePath;
        set
        {
            var path = value;
            path = path?.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("\\", Path.DirectorySeparatorChar.ToString());
            basePath = path;
        }
    }

    /// <summary>
    ///  Creates a new ResXResourceWriter that will write to the specified file.
    /// </summary>
    public ResXResourceWriter(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException(nameof(fileName));
        }
        _fileName = fileName;
    }

    public ResXResourceWriter(string? fileName, Func<Type, string>? typeNameConverter): this(fileName)
    {
        _typeNameConverter = typeNameConverter;
    }

    /// <summary>
    ///  Creates a new ResXResourceWriter that will write to the specified stream.
    /// </summary>
    public ResXResourceWriter(Stream stream)
    {
        if ((stream?.Length??0) == 0)
        {
            throw new ArgumentException(nameof(stream));
        }
        _stream = stream;
    }

    public ResXResourceWriter(Stream stream, Func<Type, string>? typeNameConverter):this(stream)
    {
        _typeNameConverter = typeNameConverter;
    }

    /// <summary>
    ///  Creates a new ResXResourceWriter that will write to the specified TextWriter.
    /// </summary>
    public ResXResourceWriter(TextWriter? textWriter)
    {
        if (textWriter == null)
        {
            throw new ArgumentNullException(nameof(textWriter));
        }
        _textWriter = textWriter;
    }

    public ResXResourceWriter(TextWriter? textWriter, Func<Type, string>? typeNameConverter):  this(textWriter)
    {
        _typeNameConverter = typeNameConverter;
    }

    ~ResXResourceWriter()
    {
        Dispose(false);
    }

    private void InitializeWriter()
    {
        if (_xmlTextWriter is null)
        {
            var writeHeaderRequired = false;

            if (_textWriter != null)
            {
                _textWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writeHeaderRequired = true;

                _xmlTextWriter = new XmlTextWriter(_textWriter);
            }
            else if (_stream != null)
            {
                _xmlTextWriter = new XmlTextWriter(_stream, Encoding.UTF8);
            }
            else
            {
                if(_fileName != null) throw new InvalidOperationException("Nothing to output to");
                if (_fileName != null)
                {
                    _xmlTextWriter = new XmlTextWriter(_fileName, Encoding.UTF8);
                }
            }

            if (_xmlTextWriter != null)
            {
                _xmlTextWriter.Formatting = Formatting.Indented;
                _xmlTextWriter.Indentation = 2;

                if (!writeHeaderRequired)
                {
                    _xmlTextWriter.WriteStartDocument(); // writes <?xml version="1.0" encoding="utf-8"?>
                }
            }
        }
        else
        {
            _xmlTextWriter.WriteStartDocument();
        }

        if (_xmlTextWriter != null)
        {
            _xmlTextWriter.WriteStartElement("root");
            var reader = new XmlTextReader(new StringReader(resourceSchema))
            {
                WhitespaceHandling = WhitespaceHandling.None
            };
            _xmlTextWriter.WriteNode(reader, true);

            _xmlTextWriter.WriteStartElement(resHeaderStr);
            {
                _xmlTextWriter.WriteAttributeString(nameStr, resMimeTypeStr);
                _xmlTextWriter.WriteStartElement(valueStr);
                {
                    _xmlTextWriter.WriteString(ResMimeType);
                }

                _xmlTextWriter.WriteEndElement();
            }

            _xmlTextWriter.WriteEndElement();

            _xmlTextWriter.WriteStartElement(resHeaderStr);
            {
                _xmlTextWriter.WriteAttributeString(nameStr, versionStr);
                _xmlTextWriter.WriteStartElement(valueStr);
                {
                    _xmlTextWriter.WriteString(version);
                }

                _xmlTextWriter.WriteEndElement();
            }

            _xmlTextWriter.WriteEndElement();

            _xmlTextWriter.WriteStartElement(resHeaderStr);
            {
                _xmlTextWriter.WriteAttributeString(nameStr, readerStr);
                _xmlTextWriter.WriteStartElement(valueStr);
                {
                    _xmlTextWriter.WriteString(
                        MultiTargetUtil.GetAssemblyQualifiedName(typeof(ResXResourceReader), _typeNameConverter) ??
                        string.Empty);
                }

                _xmlTextWriter.WriteEndElement();
            }

            _xmlTextWriter.WriteEndElement();

            _xmlTextWriter.WriteStartElement(resHeaderStr);
            {
                _xmlTextWriter.WriteAttributeString(nameStr, writerStr);
                _xmlTextWriter.WriteStartElement(valueStr);
                {
                    _xmlTextWriter.WriteString(
                        MultiTargetUtil.GetAssemblyQualifiedName(typeof(ResXResourceWriter), _typeNameConverter) ??
                        string.Empty);
                }

                _xmlTextWriter.WriteEndElement();
            }

            _xmlTextWriter.WriteEndElement();
        }

        _initialized = true;
    }

    private XmlWriter? Writer
    {
        get
        {
            if (!_initialized)
            {
                InitializeWriter();
            }

            return _xmlTextWriter;
        }
    }

    /// <summary>
    ///  Adds aliases to the resource file...
    /// </summary>
    public virtual void AddAlias(string aliasName, AssemblyName assemblyName)
    {
        //ArgumentNullException.ThrowIfNull(assemblyName);

        if (_cachedAliases is null)
        {
            _cachedAliases = new Hashtable();
        }

        _cachedAliases[assemblyName.FullName] = aliasName;
    }

    /// <summary>
    ///  Adds the given value to the collection of metadata.  These name/value pairs
    ///  will be emitted to the &lt;metadata&gt; elements in the .resx file.
    /// </summary>
    public void AddMetadata(string? name, byte[] value) => AddDataRow(metadataStr, name, value);

    /// <summary>
    ///  Adds the given value to the collection of metadata.  These name/value pairs
    ///  will be emitted to the &lt;metadata&gt; elements in the .resx file.
    /// </summary>
    public void AddMetadata(string? name, string? value) => AddDataRow(metadataStr, name, value);

    /// <summary>
    ///  Adds the given value to the collection of metadata.  These name/value pairs
    ///  will be emitted to the &lt;metadata&gt; elements in the .resx file.
    /// </summary>
    public void AddMetadata(string? name, object value) => AddDataRow(metadataStr, name, value);

    /// <summary>
    ///  Adds a blob resource to the resources.
    /// </summary>
    public void AddResource(string? name, byte[] value) => AddDataRow(dataStr, name, value);

    /// <summary>
    ///  Adds a resource to the resources. If the resource is a string,
    ///  it will be saved that way, otherwise it will be serialized
    ///  and stored as in binary.
    /// </summary>
    public void AddResource(string? name, object value)
    {
        if (value is ResXDataNode node)
        {
            AddResource(node);
        }
        else
        {
            AddDataRow(dataStr, name, value);
        }
    }

    /// <summary>
    ///  Adds a string resource to the resources.
    /// </summary>
    public void AddResource(string? name, string? value) => AddDataRow(dataStr, name, value);

    /// <summary>
    ///  Adds a string resource to the resources.
    /// </summary>
    public void AddResource(ResXDataNode node)
    {
        // we're modifying the node as we're adding it to the resxwriter
        // this is BAD, so we clone it. adding it to a writer doesnt change it
        // we're messing with a copy
        var nodeClone = node.DeepClone();

        var fileRef = nodeClone.FileRef;
        var modifiedBasePath = BasePath;

        if (!string.IsNullOrEmpty(modifiedBasePath))
        {
            if (!modifiedBasePath!.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                modifiedBasePath += Path.DirectorySeparatorChar;
            }

            fileRef?.MakeFilePathRelative(modifiedBasePath);
        }

        var info = nodeClone.GetDataNodeInfo();
        if (info != null)
        {
            AddDataRow(dataStr, info.name, info.valueData, info.typeName, info.mimeType, info.comment);
        }
    }

    /// <summary>
    ///  Adds a blob resource to the resources.
    /// </summary>
    private void AddDataRow(string elementName, string? name, byte[] value)
    {
        AddDataRow(elementName, name, ToBase64WrappedString(value), TypeNameWithAssembly(typeof(byte[])), null, null);
    }

    /// <summary>
    ///  Adds a resource to the resources. If the resource is a string,
    ///  it will be saved that way, otherwise it will be serialized
    ///  and stored as in binary.
    /// </summary>
    private void AddDataRow(string elementName, string? name, object value)
    {
        Debug.WriteLineIf(resValueProviderSwitch.TraceVerbose, "  resx: adding resource " + name);
        switch (value)
        {
            case string str:
                AddDataRow(elementName, name, str);
                break;
            case byte[] bytes:
                AddDataRow(elementName, name, bytes);
                break;
            case ResXFileRef fileRef:
            {
                var node = new ResXDataNode(name, fileRef, _typeNameConverter);
                var info = node.GetDataNodeInfo();
                if (info != null)
                {
                    AddDataRow(elementName, info.name, info.valueData, info.typeName, info.mimeType, info.comment);
                }

                break;
            }

            default:
            {
                var node = new ResXDataNode(name, value, _typeNameConverter);
                var info = node.GetDataNodeInfo();
                if (info != null)
                {
                    AddDataRow(elementName, info.name, info.valueData, info.typeName, info.mimeType, info.comment);
                }

                break;
            }
        }
    }

    /// <summary>
    ///  Adds a string resource to the resources.
    /// </summary>
    private void AddDataRow(string elementName, string? name, string? value)
    {
        // if it's a null string, set it here as a resxnullref
        var typeName =
            value is null
                ? MultiTargetUtil.GetAssemblyQualifiedName(typeof(ResXNullRef), _typeNameConverter)
                : null;
        AddDataRow(elementName, name, value, typeName, null, null);
    }

    /// <summary>
    ///  Adds a new row to the Resources table. This helper is used because
    ///  we want to always late bind to the columns for greater flexibility.
    /// </summary>
    private void AddDataRow(string elementName, string? name, string? value, string? type, string? mimeType, string? comment)
    {
        if (_hasBeenSaved)
        {
            throw new InvalidOperationException("SR.ResXResourceWriterSaved");
        }

        string? alias = null;
        if (!string.IsNullOrEmpty(type) && elementName == dataStr)
        {
            var assemblyName = GetFullName(type);
            if (string.IsNullOrEmpty(assemblyName))
            {
                try
                {
                    var typeObject = Type.GetType(type);
                    if (typeObject == typeof(string))
                    {
                        type = null;
                    }
                    else if (typeObject != null)
                    {
                        assemblyName = GetFullName(MultiTargetUtil.GetAssemblyQualifiedName(typeObject, _typeNameConverter));
                        alias = GetAliasFromName(new AssemblyName(assemblyName ?? string.Empty));
                    }
                }
                catch (Exception ex)
                {
                    Trace.Write(ex);
                }
            }
            else
            {
                alias = GetAliasFromName(new AssemblyName(GetFullName(type) ?? string.Empty));
            }
        }

        if (Writer != null)
        {
            Writer.WriteStartElement(elementName);
            {
                Writer.WriteAttributeString(nameStr, name ?? string.Empty);

                if (!string.IsNullOrEmpty(alias) && !string.IsNullOrEmpty(type) && elementName == dataStr)
                {
                    // CHANGE: we still output version information. This might have
                    // to change in 3.2
                    var typeName = GetTypeName(type);
                    var typeValue = typeName + ", " + alias;
                    Writer.WriteAttributeString(typeStr, typeValue);
                }
                else
                {
                    if (type != null)
                    {
                        Writer.WriteAttributeString(typeStr, type);
                    }
                }

                if (mimeType != null)
                {
                    Writer.WriteAttributeString(mimeTypeStr, mimeType);
                }

                if ((type is null && mimeType is null) ||
                    (type != null && type.StartsWith("System.Char", StringComparison.Ordinal)))
                {
                    Writer.WriteAttributeString("xml", "space", null, "preserve");
                }

                Writer.WriteStartElement(valueStr);
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        Writer.WriteString(value);
                    }
                }

            Writer.WriteEndElement();

                if (!string.IsNullOrEmpty(comment))
                {
                    Writer.WriteStartElement(commentStr);
                    {
                        Writer.WriteString(comment);
                    }

                Writer.WriteEndElement();
            }
        }

            Writer.WriteEndElement();
        }
    }

    private void AddAssemblyRow(string elementName, string alias, string name)
    {
        if (Writer != null)
        {
            Writer.WriteStartElement(elementName);
            {
                if (!string.IsNullOrEmpty(alias))
                {
                    Writer.WriteAttributeString(aliasStr, alias);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    Writer.WriteAttributeString(nameStr, name);
                }
            }

            Writer.WriteEndElement();
        }
    }

    private string GetAliasFromName(AssemblyName assemblyName)
    {
        if (_cachedAliases is null)
        {
            _cachedAliases = new Hashtable();
        }

        var alias = (string)_cachedAliases[assemblyName.FullName];

        if (string.IsNullOrEmpty(alias))
        {
            alias = assemblyName.Name;
            AddAlias(alias, assemblyName);
            AddAssemblyRow(assemblyStr, alias, assemblyName.FullName);
        }

        return alias;
    }

    /// <summary>
    ///  Closes any files or streams locked by the writer.
    /// </summary>
    public void Close()
    {
        Dispose();
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (!_hasBeenSaved)
            {
                Generate();
            }

            if (_xmlTextWriter != null)
            {
                _xmlTextWriter.Close();
                _xmlTextWriter = null;
            }

            if (_stream != null)
            {
                _stream.Close();
                _stream = null;
            }

            if (_textWriter != null)
            {
                _textWriter.Close();
                _textWriter = null;
            }
        }
    }

    private static string? GetTypeName(string? typeName)
    {
        var indexStart = typeName?.IndexOf(',')??-1;
        return indexStart == -1 ? typeName : typeName!.Substring(0, indexStart);
    }

    private static string? GetFullName(string? typeName)
    {
        var indexStart = typeName?.IndexOf(',')??-1;
        if (indexStart == -1)
        {
            return null;
        }

        return typeName!.Substring(indexStart + 2);
    }

    static string ToBase64WrappedString(byte[] data)
    {
        const int lineWrap = 80;
        const string crlf = "\r\n";
        const string prefix = "        ";
        var raw = Convert.ToBase64String(data);
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

    private string? TypeNameWithAssembly(Type type)
    {
        var result = MultiTargetUtil.GetAssemblyQualifiedName(type, _typeNameConverter);
        return result;
    }

    /// <summary>
    ///  Writes the resources out to the file or stream.
    /// </summary>
    public void Generate()
    {
        if (_hasBeenSaved)
        {
            throw new InvalidOperationException("SR.ResXResourceWriterSaved");
        }

        _hasBeenSaved = true;
        Debug.WriteLineIf(resValueProviderSwitch.TraceVerbose, "writing XML");

        if (Writer != null)
        {
            Writer.WriteEndElement();
            Writer.Flush();
        }

        Debug.WriteLineIf(resValueProviderSwitch.TraceVerbose, "done");
    }
}