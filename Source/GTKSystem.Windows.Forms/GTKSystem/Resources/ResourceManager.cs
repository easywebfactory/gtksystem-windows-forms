#if NETSTANDARD
extern alias sdc;
#else
extern alias sd;
#endif
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Resources.Extensions;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.Xml;

namespace System.Resources;

#if NETSTANDARD
using SdcBitmap = sdc::System.Drawing.Image;
using SdcIcon = sdc::System.Drawing.Icon;
using SdcImageFormat = sdc::System.Drawing.Imaging.ImageFormat;
#else
using SdcBitmap = sd::System.Drawing.Image;
using SdcIcon = sd::System.Drawing.Icon;
using SdcImageFormat = sd::System.Drawing.Imaging.ImageFormat;
#endif

[EditorBrowsable(EditorBrowsableState.Never)]
public class GtkResourceManager : ResourceManager
{
    internal const string resFileExtension = ".resources";
    private readonly string? _baseName;
    public ResourceInfo? getResourceInfo = new();
    private readonly Assembly? _assemblyWithResources = null!;

    private Type? _resourceSource;

    private readonly Dictionary<string, (string Culture, Assembly Assembly)[]> _assemblies = new();

    public (string Culture, Assembly Assembly)[] GetAssembliesWithResources(string? cultureName)
    {
        if (cultureName != null && _assemblies.ContainsKey(cultureName))
        {
            return _assemblies[cultureName];
        }

        if (cultureName == null)
        {
            return [(string.Empty, _assemblyWithResources!)];
        }

        _assemblies[cultureName] = _assemblyWithResources!.GetAssembliesWithResources(cultureName);
        return _assemblies[cultureName];
    }

    public GtkResourceManager([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                                       DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type? usingResourceSet) :
        this(null, null, usingResourceSet)
    {

    }

    public GtkResourceManager(string? baseName, Assembly? assemblyWithResources) : this(baseName, assemblyWithResources, null)
    {

    }

    public GtkResourceManager(string? baseName, Assembly? assemblyWithResources,
                           [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                                       DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type? usingResourceSet) :
        base(baseName ?? string.Empty, assemblyWithResources ?? Assembly.GetExecutingAssembly(), usingResourceSet)
    {
        _baseName = baseName;
        _assemblyWithResources = assemblyWithResources!;
        _resourceSource = usingResourceSet;
        if (getResourceInfo != null)
        {
            getResourceInfo.Assembly = assemblyWithResources;
            getResourceInfo.BaseName = baseName;
            getResourceInfo.SourceType = usingResourceSet;
        }
    }

    protected GtkResourceManager()
    {

    }

    private byte[] ReadResourceFile(string name)
    {
        byte[]? result = null;
        try
        {
            //string resourceDirctory = System.AppContext.BaseDirectory.Replace("\\", "/") + $"Resources";//linux路径必须用/
            //string resourceDirctory = Environment.CurrentDirectory.Replace("\\", "/") + $"Resources";//linux路径必须用/
            var filepath = $"./{Path.GetExtension(_baseName).TrimStart('.')}.resx"; //linux路径必须用/
            if (File.Exists(filepath))
            {
                try
                {
                    var doc = new XmlDocument();
                    var xmlReaderSettings = new XmlReaderSettings { CheckCharacters = false };
                    doc.Load(filepath);
                    var docElem = doc.DocumentElement;
                    var nodes = docElem.SelectNodes("data");
                    //<data name="pictureBox1.Image" type="System.Drawing.Bitmap, System.Drawing.Common" mimetype="application/x-microsoft.net.object.bytearray.base64">
                    //<value> </value>
                    //</data>

                    if (nodes != null)
                    {
                        foreach (XmlNode xn in nodes)
                        {
                            if (xn.Attributes != null && xn.Attributes["name"].Value == name)
                            {
                                var data = xn.SelectSingleNode("value")?.InnerText;
                                if (data != null)
                                {
                                    result = Convert.FromBase64String(data);
                                }

                                break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.Write(e);
                }
            }
        }
        catch (Exception ex)
        {
            Trace.Write(ex);
        }
        return result;
    }

    private object? ReadResourceData(string name, CultureInfo? culture)
    {
        var assembliesWithResources = GetAssembliesWithResources(culture?.Name);
        if (assembliesWithResources == null)
            throw new FileNotFoundException();
        try
        {
            foreach (var assemblyWithResources in assembliesWithResources)
            {
                var culturePart = string.IsNullOrEmpty(assemblyWithResources.Culture) ? string.Empty : $".{assemblyWithResources.Culture}";
                var stream = assemblyWithResources.Assembly.GetManifestResourceStream($"{_baseName}{culturePart}{resFileExtension}");
                if (stream != null)
                {
                    var reader = new GtkDeserializingResourceReader(stream);
                    var enumerator = reader.GetEnumerator();
                    using var disposable = enumerator as IDisposable;
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Key?.ToString() == name)
                        {
                            try
                            {
                                if (enumerator.Value is ImageListStreamer streamer)
                                {
                                    streamer.ResourceInfo = getResourceInfo;
                                    return streamer;
                                }

                                return enumerator.Value;
                            }
                            catch
                            {
                                // Image format content cannot be extracted
                                return null;
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            return null;
        }
        return null;
    }

    private string? ReadResourceText(string name, CultureInfo? culture)
    {
        var assemblies = GetAssembliesWithResources(culture?.Name);
        if (assemblies == null || assemblies.All(a => a.Assembly == null))
            throw new FileNotFoundException();
        foreach (var tuple in assemblies)
        {
            var culturePart = string.IsNullOrEmpty(tuple.Culture) ? string.Empty : $".{tuple.Culture}";
            var stream = tuple.Assembly.GetManifestResourceStream($"{_baseName}{culturePart}.resources");
            if (stream != null)
            {
                var reader = new GtkDeserializingResourceReader(stream);
                var dict = reader.GetEnumerator();
                using var disposable = dict as IDisposable;
                while (dict.MoveNext())
                {
                    if (dict.Key?.ToString() == name)
                    {
                        try
                        {
                            return dict.Value.ToString();
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }

                break;
            }
        }
        return null;
    }

    public override object? GetObject(string name)
    {
        return GetObject(name, Thread.CurrentThread.CurrentUICulture);
    }

    public override object? GetObject(string name, CultureInfo culture)
    {
        if (getResourceInfo != null)
        {
            getResourceInfo.ResourceName = name;
            var obj = ReadResourceData(name, culture);
            if (obj is ImageListStreamer)
            {
                return obj;
            }

            var fileName = name;
            var filebytes = ReadResourceFile(name);
            var _formName = Path.GetExtension(BaseName).TrimStart('.');
            var path = $"./Resources/{_formName}";
            var searchPattern = $"{fileName}.*";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (filebytes == null)
            {
                if (obj is SdcBitmap b)
                {
                    var filename = Path.Combine(path, Path.ChangeExtension(searchPattern, ".png"));
                    if (!File.Exists(filename))
                    {
                        b.Save(filename, SdcImageFormat.Png);
                    }
                }
                if (obj is SdcIcon i)
                {
                    var combine = Path.Combine(path, Path.ChangeExtension(searchPattern, ".ico"));
                    if (!File.Exists(combine))
                    {
                        using var outputStream = File.OpenWrite(combine);
                        i.Save(outputStream);
                    }
                }
            }

            if (filebytes == null)
            {
                string[] files = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
                if (files is { Length: > 0 })
                {
                    fileName = files[0];
                    filebytes = File.ReadAllBytes(files[0]);
                }

                if (name.EndsWith(".BackgroundImage"))
                {
                    return new Drawing.Bitmap(filebytes ?? []) { FileName = fileName };
                }

                if (name.EndsWith(".Image"))
                {
                    return new Drawing.Bitmap(filebytes ?? []) { FileName = fileName };
                }

                if (name.EndsWith(".Icon"))
                {
                    return new Drawing.Icon(filebytes ?? []) { FileName = fileName };
                }

                if (filebytes == null)
                {
                    return new Drawing.Bitmap(0, 0);
                }

                return new Drawing.Bitmap(filebytes ?? []) { FileName = fileName };
            }

            return obj;
            return obj;
        }

        return null;
    }

    //public  UnmanagedMemoryStream GetStream(string name)
    //{
    //    return GetStream(name);
    //}
    //public UnmanagedMemoryStream GetStream(string name, CultureInfo culture)
    //{
    //    byte[] data = ReadResourceFile(name);
    //    if (data == null)
    //        return null;
    //    else
    //    {
    //        using (MemoryStream ms = new MemoryStream(data))
    //        {
    //            BinaryReader br = new BinaryReader(ms);
    //            return br.BaseStream as UnmanagedMemoryStream;
    //        }
    //    }
    //}
    public override string? GetString(string name)
    {
        return GetString(name, null);
    }
    public override string? GetString(string name, CultureInfo? culture)
    {
        return ReadResourceText(name, culture);
    }
    public class ResourceInfo
    {
        public string? ResourceName { get; set; }
        public byte[]? ImageBytes { get; set; }
        public string? BaseName { get; set; }
        public Assembly? Assembly { get; set; }
        public Type? SourceType { get; set; }
    }
}