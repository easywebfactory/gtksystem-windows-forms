using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace GTKSystem.Resources
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ResourceManager
    {
        public static readonly int HeaderVersionNumber = 1;
        public static readonly int MagicNumber = -1091581234;

        //internal const string ResFileExtension = ".resources";
        //internal const int ResFileExtensionLength = 10;
        //internal static readonly int DEBUG = 0;

        protected Assembly MainAssembly;

        private System.Type _resourceSource;
        private Assembly _assembly;
        private string _baseName;
        public ResourceManager(System.Type resourceSource) : this(null, null, resourceSource)
        {

        }
        public ResourceManager(string baseName, Assembly assembly) : this(baseName, assembly, null)
        {

        }
        public ResourceManager(string baseName, Assembly assembly, System.Type resourceSource)
        {
            this._baseName = baseName;
            this._assembly = assembly;
            this._resourceSource = resourceSource;
        }
        protected ResourceManager()
        {

        }

        public virtual string BaseName { get; }
        public virtual bool IgnoreCase { get; set; }
        public virtual System.Type ResourceSetType { get; }
        protected UltimateResourceFallbackLocation FallbackLocation { get; set; }

        public static ResourceManager CreateFileBasedResourceManager(string baseName, string resourceDir, System.Type usingResourceSet)
        {
            return new ResourceManager();
        }
        protected static CultureInfo GetNeutralResourcesLanguage(Assembly a)
        {
            return CultureInfo.CurrentCulture;
        }
        protected static System.Version GetSatelliteContractVersion(Assembly a)
        {
            return a.GetName().Version;
        }
        private byte[] ReadResourceFile(string name)
        {
            byte[] result = null;
            try
            {
                string resourceDirctory = System.AppContext.BaseDirectory.Replace("\\", "/") + $"Resources";//linux路径必须用/
                //string[] files = Directory.GetFiles(resourceDirctory, $"{name}.*");
                //if (files != null && files.Length > 0)
                //{
                //    byte[] imagebytes = File.ReadAllBytes(files[0]);
                //    result = imagebytes;
                //}
                //else
                //{
                    string filepath = resourceDirctory + $"/{Path.GetExtension(_baseName).TrimStart('.')}.resx"; //linux路径必须用/
                    if (System.IO.File.Exists(filepath))
                    {
                        try
                        {
                            XmlDocument doc = new XmlDocument();
                            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { CheckCharacters = false };
                            doc.Load(filepath);
                            var docElem = doc.DocumentElement;
                            XmlNodeList nodes = docElem.SelectNodes("data");
                            //<data name="pictureBox1.Image" type="System.Drawing.Bitmap, System.Drawing.Common" mimetype="application/x-microsoft.net.object.bytearray.base64">
                            //<value> </value>
                            //</data>

                            foreach (XmlNode xn in nodes)
                            {
                                if (xn.Attributes["name"].Value == name)
                                {
                                    string data = xn.SelectSingleNode("value").InnerText;
                                    result = System.Convert.FromBase64String(data);
                                    break;
                                }
                            }
                        }
                        catch { }
                    }
                //}
            }
            catch (System.Exception ex)
            {

            }
            return result;
        }
        private object ReadResourceData(string name)
        {
            if (_assembly == null)
                throw new FileNotFoundException();
            else
            {
                Stream stream = _assembly.GetManifestResourceStream(_baseName + ".resources");
                GTKSystem.Resources.Extensions.DeserializingResourceReader reader = new GTKSystem.Resources.Extensions.DeserializingResourceReader(stream);
                IDictionaryEnumerator dict = reader.GetEnumerator();
                while (dict.MoveNext())
                {
                    if (dict.Key.ToString() == name)
                    {
                        try
                        {
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
            return null;
        }
        private string ReadResourceText(string name)
        {
            if (_assembly == null)
                throw new FileNotFoundException();
            else
            {
                Stream stream = _assembly.GetManifestResourceStream(_baseName + ".resources");
                GTKSystem.Resources.Extensions.DeserializingResourceReader reader = new GTKSystem.Resources.Extensions.DeserializingResourceReader(stream);
                IDictionaryEnumerator dict = reader.GetEnumerator();
                while (dict.MoveNext())
                {
                    if (dict.Key.ToString() == name)
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
        public virtual object GetObject(string name, CultureInfo culture)
        {
            return GetObject(name);
        }
        public virtual object GetObject(string name)
        {
            object obj = ReadResourceData(name);
            if (obj == null)
            {
                System.Drawing.Bitmap img = new System.Drawing.Bitmap(1, 1);
                img.FileName = name;
                img.PixbufData = ReadResourceFile(name);
                return img;
            }
            else
            {
                return obj;
            }
        }
        public virtual ResourceSet GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
        {

            return null;
        }
        public UnmanagedMemoryStream GetStream(string name)
        {
            return GetStream(name);
        }
        public UnmanagedMemoryStream GetStream(string name, CultureInfo culture)
        {
            byte[] data = ReadResourceFile(name);
            if (data == null)
                return null;
            else
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    BinaryReader br = new BinaryReader(ms);
                    return br.BaseStream as UnmanagedMemoryStream;
                }
            }
        }
        public virtual string GetString(string name)
        {
            return GetString(name, null);
        }
        public virtual string GetString(string name, CultureInfo culture)
        {
            return ReadResourceText(name);
        }

        public virtual void ReleaseAllResources()
        {
        }
        protected virtual string GetResourceFileName(CultureInfo culture)
        {

            return null;
        }
        protected virtual ResourceSet InternalGetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
        {

            return null;
        }
    }
}