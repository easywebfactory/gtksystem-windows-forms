using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace System.Windows.Forms.Resources;

[EditorBrowsable(EditorBrowsableState.Never)]
public class ResourceManager : System.Resources.ResourceManager
{
    internal const string resFileExtension = ".resources";
    private Type? _resourceSource;
    private readonly Assembly? _assembly;
    private readonly string? _baseName;
    public ResourceInfo? getResourceInfo = new();
    public ResourceManager([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type? usingResourceSet) : this(null, null, usingResourceSet)
    {

    }
    public ResourceManager(string? baseName, Assembly? assembly) : this(baseName, assembly, null)
    {

    }
    public ResourceManager(string? baseName, Assembly? assembly, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type? usingResourceSet) : base(baseName ?? string.Empty, assembly??Assembly.GetExecutingAssembly(), usingResourceSet)
    {
        _baseName = baseName;
        _assembly = assembly;
        _resourceSource = usingResourceSet;
        if (getResourceInfo != null)
        {
            getResourceInfo.Assembly = assembly;
            getResourceInfo.BaseName = baseName;
            getResourceInfo.SourceType = usingResourceSet;
        }
    }
    protected ResourceManager()
    {

    }

    private byte[]? ReadResourceFile(string name)
    {
        byte[]? result = null;
        try
        {
            var resourceDirctory = AppContext.BaseDirectory.Replace("\\", "/") + $"Resources";//linux路径必须用/
            //string resourceDirctory = Environment.CurrentDirectory.Replace("\\", "/") + $"Resources";//linux路径必须用/
            var filepath = resourceDirctory + $"/{Path.GetExtension(_baseName)?.TrimStart('.')}.resx"; //linux路径必须用/
            if (File.Exists(filepath))
            {
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(filepath);
                    var docElem = doc.DocumentElement;
                    var nodes = docElem?.SelectNodes("data");
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
    private object? ReadResourceData(string name)
    {
        if (_assembly == null)
            throw new FileNotFoundException();
        try
        {
            var stream = _assembly.GetManifestResourceStream(_baseName + resFileExtension);
            if (stream != null)
            {
                var reader = new DeserializingResourceReader(stream);
                var dict = reader.GetEnumerator();
                using var disposable = dict as IDisposable;
                while (dict.MoveNext())
                {
                    if (dict.Key?.ToString() == name)
                    {
                        try
                        {
                            if(dict.Value is ImageListStreamer streamer)
                            {
                                streamer.ResourceInfo = getResourceInfo;
                                return streamer;
                            }

                            return dict.Value;
                        }
                        catch
                        {
                            //图像格式内容不能提取
                            return null;
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
    private string? ReadResourceText(string name)
    {
        if (_assembly == null)
            throw new FileNotFoundException();
        var stream = _assembly.GetManifestResourceStream(_baseName + ".resources");
        if (stream != null)
        {
            var reader = new DeserializingResourceReader(stream);
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
        }

        return null;
    }
    public override object? GetObject(string name, CultureInfo culture)
    {
        var result = GetObject(name);
        var stack = new StackTrace(true);
        var method = (MethodInfo)stack.GetFrame(1).GetMethod();
        if (method.ReturnType.Name == "Object" || method.ReturnType.Name.Equals(result?.GetType().Name))
        {
            return result;
        }

        return null;

    }
    public override object? GetObject(string name)
    {
        if (getResourceInfo != null)
        {
            getResourceInfo.ResourceName = name;
            var obj = ReadResourceData(name);
            if (obj == null)
            {
                if (name.EndsWith(".ImageStream"))
                {
                    return new ImageListStreamer(new ImageList()) { ResourceInfo = getResourceInfo };
                }

                var fileName = name;
                var filebytes = ReadResourceFile(name);
                if (filebytes == null)
                {
                    var fName = Path.GetExtension(BaseName).TrimStart('.') + "$" + name.TrimStart('$');
                    var files = Directory.GetFiles("Resources", $"{fName}.*", SearchOption.AllDirectories);
                    if (files is { Length: > 0 })
                    {
                        fileName = files[0];
                        filebytes = File.ReadAllBytes(files[0]);
                    }
                }

                if (name.EndsWith(".BackgroundImage"))
                {
                    return new Drawing.Bitmap(filebytes) { FileName = fileName };
                }

                if (name.EndsWith(".Image"))
                {
                    return new Drawing.Bitmap(filebytes) { FileName = fileName };
                }

                if (name.EndsWith(".Icon"))
                {
                    return new Drawing.Icon(filebytes) { FileName = fileName };
                }

                if (filebytes == null)
                {
                    return new Drawing.Bitmap(0, 0);
                }

                return new Drawing.Bitmap(filebytes) { FileName = fileName };
            }

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
        return ReadResourceText(name);
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