//
// WriterTest.cs: Unit Tests for ResXResourceWriter.
//
// Authors:
//     Robert Jordan <robertj@gmx.net>
//     Gary Barnett <gary.barnett.mono@gmail.com>

using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Text;

namespace GtkTests.System.Resources;

[TestFixture]
public class WriterTest : Windows.Forms.TestHelper
{
    string fileName;

    [SetUp]
    protected override void SetUp ()
    {
        fileName = Path.GetTempFileName ();
        base.SetUp ();
    }

    [TearDown]
    protected override void TearDown ()
    {
        File.Delete (fileName);
        base.TearDown ();
    }

    [Test] // ctor (Stream)
    [NUnit.Framework.Category ("NotDotNet")]
    public void Constructor1_Stream_Null ()
    {
        try {
            new ResXResourceWriter ((Stream) null);
            Assert.Fail ("#1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
        }
    }

    [Test] // ctor (Stream)
    [NUnit.Framework.Category ("NotDotNet")]
    public void Constructor1_Stream_NotWritable ()
    {
        var ms = new MemoryStream (new byte [0], false);

        try {
            new ResXResourceWriter (ms);
            Assert.Fail ("#1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
        }
    }

    [Test] // ctor (TextWriter)
    [NUnit.Framework.Category ("NotDotNet")]
    public void Constructor2_TextWriter_Null ()
    {
        try {
            new ResXResourceWriter ((TextWriter) null);
            Assert.Fail ("#1");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("textWriter", ex.ParamName, "#5");
        }
    }

    [Test] // ctor (String)
    [NUnit.Framework.Category ("NotDotNet")]
    public void Constructor3_FileName_Null ()
    {
        try {
            new ResXResourceWriter ((string) null);
            Assert.Fail ("#1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
        }
    }

    [Test]
    public void AddResource_WithComment ()
    {
        var w = new ResXResourceWriter (fileName);
        var node = new ResXDataNode ("key", "value");
        node.Comment = "comment is preserved";
        w.AddResource (node);
        w.Generate ();
        w.Close ();

        var r = new ResXResourceReader (fileName);
        ITypeResolutionService typeres = null;
        r.UseResXDataNodes = true;
			
        var count = 0;
        foreach (DictionaryEntry o in r)
        {
            var key = o.Key.ToString();
            node = (ResXDataNode)o.Value;
            var value = node.GetValue (typeres).ToString ();
            var comment = node.Comment;

            Assert.AreEqual ("key", key, "key");
            Assert.AreEqual ("value", value, "value");
            Assert.AreEqual ("comment is preserved", comment, "comment");
            Assert.AreEqual (0, count, "too many nodes");
            count++;
        }
        r.Close ();

        File.Delete (fileName);
    }

    [Test]
    public void TestWriter ()
    {
        var w = new ResXResourceWriter (fileName);
        w.AddResource ("String", "hola");
        w.AddResource ("String2", (object) "hello");
        w.AddResource ("Int", 42);
        w.AddResource ("Enum", PlatformID.Win32NT);
        w.AddResource ("Convertible", new Point (43, 45));
        w.AddResource ("ByteArray", new byte [] { 12, 13, 14 });
        w.AddResource ("ByteArray2", (object) new byte [] { 15, 16, 17 });
        w.AddResource ("StrType", new MyStrType ("hello"));
        w.AddResource ("BinType", new MyBinType ("world"));

        try {
            w.AddResource ("NonSerType", new MyNonSerializableType ());
            Assert.Fail ("#0");
        } catch (InvalidOperationException) {
        }

        w.Generate ();
        w.Close ();

        var r = new ResXResourceReader (fileName);
        var h = new Hashtable ();
        foreach (DictionaryEntry e in r) {
            h.Add (e.Key, e.Value);
        }
        r.Close ();

        Assert.AreEqual ("hola", (string) h ["String"], "#1");
        Assert.AreEqual ("hello", (string) h ["String2"], "#2");
        Assert.AreEqual (42, (int) h ["Int"], "#3");
        Assert.AreEqual (PlatformID.Win32NT, (PlatformID) h ["Enum"], "#4");
        Assert.AreEqual (43, ((Point) h ["Convertible"]).X, "#5");
        Assert.AreEqual (13, ((byte []) h ["ByteArray"]) [1], "#7");
        Assert.AreEqual (16, ((byte []) h ["ByteArray2"]) [1], "#8");
        Assert.AreEqual ("hello", ((MyStrType) h ["StrType"]).Value, "#12");
        Assert.AreEqual ("world", ((MyBinType) h ["BinType"]).Value, "#13");

        File.Delete (fileName);
    }

    ResXDataNode GetNodeFromResXWithBasePath (ResXDataNode node, string basePath)
    {
        var sw = new StringWriter ();
        using (var writer = new ResXResourceWriter (sw)) {
            writer.BasePath = basePath;
            writer.AddResource (node);
        }
			
        var sr = new StringReader (sw.ToString ());
			
        using (var reader = new ResXResourceReader (sr)) {
            reader.UseResXDataNodes = true;
            var enumerator = reader.GetEnumerator ();
            enumerator.MoveNext ();
            return ((DictionaryEntry) enumerator.Current).Value as ResXDataNode;
        }
    }
		
    [Test]
    public void BasePath_ChangesAbsoluteFileRef_Node ()
    {
        var node = new ResXDataNode ("name", new ResXFileRef (@"/dir/dir/filename.ext", "System.String"));
        var returnedNode = GetNodeFromResXWithBasePath (node, @"/dir");
        Assert.IsNotNull (returnedNode, "#A1");
        Assert.AreEqual (@$"dir{Path.DirectorySeparatorChar}filename.ext", returnedNode.FileRef.FileName, "#A2");
    }

    [Test]
    public void BasePath_ChangesAbsoluteFileRef_Object ()
    {
        var fileref = new ResXFileRef (@"/dir/dir/filename.ext", "System.String");
			
        var basePath = @"/dir";
			
        var sw = new StringWriter ();
        using (var writer = new ResXResourceWriter (sw)) {
            writer.BasePath = basePath;
            writer.AddResource ("name", fileref);
        }
			
        var sr = new StringReader (sw.ToString ());
			
        using (var reader = new ResXResourceReader (sr)) {
            reader.UseResXDataNodes = true;
            var enumerator = reader.GetEnumerator ();
            enumerator.MoveNext ();
            var returnedNode = ((DictionaryEntry) enumerator.Current).Value as ResXDataNode;
            Assert.AreEqual ($@"{Path.DirectorySeparatorChar}dir{Path.DirectorySeparatorChar}dir{Path.DirectorySeparatorChar}filename.ext", returnedNode.FileRef.FileName);
        }
    }

    [Test]
    public void BasePath_ChangesAbsoluteFileRef_Deeper ()
    {
        var node = new ResXDataNode ("name", new ResXFileRef (@"/adir/filename.ext", "System.String"));
        var returnedNode = GetNodeFromResXWithBasePath (node, @"/dir1/dir2");
        Assert.IsNotNull (returnedNode, "#A1");
        Assert.AreEqual (@$"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}adir{Path.DirectorySeparatorChar}filename.ext", returnedNode.FileRef.FileName, "#A2");
    }
		
    [Test]
    public void BasePath_ComplexPath ()
    {
        var node = new ResXDataNode ("name", new ResXFileRef (@"/dir/dir/../filename.ext", "System.String"));
        var returnedNode = GetNodeFromResXWithBasePath (node, @"/dir");
        Assert.IsNotNull (returnedNode, "#A1");
        Assert.AreEqual (@$"dir{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}filename.ext", returnedNode.FileRef.FileName, "#A2");
    }
		
    [Test]
    public void BasePath_IgnoresTrailingSeparator ()
    {
        var node = new ResXDataNode ("name", new ResXFileRef (@"/dir/filename.ext", "System.String"));
        var nonTrailNode = GetNodeFromResXWithBasePath (node, @"/dir");
        Assert.IsNotNull (nonTrailNode, "#A1");
        Assert.AreEqual (@"filename.ext", nonTrailNode.FileRef.FileName, "#A2");
			
        var trailingNode = GetNodeFromResXWithBasePath (node, @"/dir/");
        Assert.IsNotNull (trailingNode, "#A3");
        Assert.AreEqual (@"filename.ext", trailingNode.FileRef.FileName, "#A4");
    }
		
    [Test]
    public void BasePath_IgnoredIfNotPartOfPath () // not really a valid test on linux systems
    {
        var node = new ResXDataNode ("name", new ResXFileRef (@"/dir/filename.ext", "System.String"));
        var returnedNode = GetNodeFromResXWithBasePath (node, @"D/");
        Assert.IsNotNull (returnedNode, "#A1");
        Assert.AreEqual ($@"{Path.DirectorySeparatorChar}dir{Path.DirectorySeparatorChar}filename.ext", returnedNode.FileRef.FileName, "#A2");
    }

    [Test] // FIXME: this fails on mono ("/dir/filename.ext" is returned) but passes on .net
    [NUnit.Framework.Category ("NotWorking")]
    public void BasePath_Root ()
    {
        var node = new ResXDataNode ("name", new ResXFileRef (@"/dir/filename.ext", "System.String"));
        var returnedNode = GetNodeFromResXWithBasePath (node, @"/");
        Assert.IsNotNull (returnedNode, "#A1");
        Assert.AreEqual ($@"dir{Path.DirectorySeparatorChar}filename.ext", returnedNode.FileRef.FileName, "#A2");
    }
		
    [Test]
    public void BasePath_RelativeFileRef ()
    {
        var node = new ResXDataNode ("name", new ResXFileRef (@"../../filename.ext", "System.String"));
        var returnedNode = GetNodeFromResXWithBasePath (node, @"../");
        Assert.IsNotNull (returnedNode, "#A1");
        Assert.AreEqual ($@"..{Path.DirectorySeparatorChar}filename.ext", returnedNode.FileRef.FileName, "#A2");	
    }
		
    [Test]
    public void BasePath_DoesntAffectOriginalNode ()
    {
        var node = new ResXDataNode ("name", new ResXFileRef (@"/dir/dir/filename.ext", "System.String"));
        var returnedNode = GetNodeFromResXWithBasePath (node, @"/dir");
        Assert.IsNotNull (returnedNode, "#A1");
        Assert.AreEqual ($@"dir{Path.DirectorySeparatorChar}filename.ext", returnedNode.FileRef.FileName, "#A2");
        Assert.AreEqual ($@"{Path.DirectorySeparatorChar}dir{Path.DirectorySeparatorChar}dir{Path.DirectorySeparatorChar}filename.ext", node.FileRef.FileName, "#A3");
    }
}

[Serializable]
[TypeConverter (typeof (MyStrTypeConverter))]
public class MyStrType
{
    public string Value;

    public MyStrType (string s)
    {
        Value = s;
    }
}

public class MyStrTypeConverter : TypeConverter
{
    public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType)
    {
        if (destinationType == typeof (string))
            return true;
        return base.CanConvertTo (context, destinationType);
    }

    public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof (string))
            return true;
        return base.CanConvertFrom (context, sourceType);
    }

    public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == typeof (string))
            return ((MyStrType) value).Value;
        return base.ConvertTo (context, culture, value, destinationType);
    }

    public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value.GetType () == typeof (string))
            return new MyStrType ((string) value);
        return base.ConvertFrom (context, culture, value);
    }
}

[Serializable]
[TypeConverter (typeof (MyBinTypeConverter))]
public class MyBinType
{
    public string Value;

    public MyBinType (string s)
    {
        Value = s;
    }
}

public class MyBinTypeConverter : TypeConverter
{
    public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType)
    {
        if (destinationType == typeof (byte []))
            return true;
        return base.CanConvertTo (context, destinationType);
    }

    public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof (byte []))
            return true;
        return base.CanConvertFrom (context, sourceType);
    }

    public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == typeof (byte []))
            return Encoding.Default.GetBytes (((MyBinType) value).Value);
        return base.ConvertTo (context, culture, value, destinationType);
    }

    public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value.GetType () == typeof (byte []))
            return new MyBinType (Encoding.Default.GetString ((byte []) value));
        return base.ConvertFrom (context, culture, value);
    }
}

[TypeConverter (typeof (MyNonSerializableTypeConverter))]
public class MyNonSerializableType
{
    public MyNonSerializableType ()
    {
    }
}

public class MyNonSerializableTypeConverter : TypeConverter
{
    public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType)
    {
        if (destinationType == typeof (byte []))
            return true;
        return base.CanConvertTo (context, destinationType);
    }

    public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof (byte []))
            return true;
        return base.CanConvertFrom (context, sourceType);
    }

    public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == typeof (byte []))
            return new byte [] { 0, 1, 2, 3 };
        return base.ConvertTo (context, culture, value, destinationType);
    }

    public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value.GetType () == typeof (byte []))
            return new MyNonSerializableType ();
        return base.ConvertFrom (context, culture, value);
    }
}