//
// ResXFileRefTest.cs: Unit Tests for ResXFileRef.
//
// Authors:
//     Gert Driesen <drieseng@users.sourceforge.net>
//

using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Text;

namespace GtkTests.System.Resources;

[TestFixture]
public class ResXFileRefTest : Windows.Forms.TestHelper
{
    [Test]
    public void Constructor1 ()
    {
        var r = new ResXFileRef ("mono.bmp", "Bitmap");
        RemoveWarning (r);
        Assert.AreEqual ("mono.bmp", r.FileName, "#1");
        Assert.AreEqual ("Bitmap", r.TypeName, "#2");
    }

    [Test]
    public void Constructor1_FileName_Null ()
    {
        try {
            new ResXFileRef ((string?) null, "Bitmap");
            Assert.Fail ("#1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#2");
            Assert.IsNotNull (ex.Message, "#3");
            Assert.IsNull (ex.ParamName, "#4");
            Assert.IsNull (ex.InnerException, "#6");
        }
    }

    [Test]
    public void Constructor1_TypeName_Null ()
    {
        try {
            new ResXFileRef ("mono.bmp", (string) null);
            Assert.Fail ("#1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#2");
            Assert.IsNotNull (ex.Message, "#3");
            Assert.IsNull (ex.ParamName, "#4");
            Assert.IsNull (ex.InnerException, "#6");
        }
    }

    [Test]
    public void Constructor2 ()
    {
        var utf8 = Encoding.UTF8;

        var r = new ResXFileRef ("mono.bmp", "Bitmap", utf8);
        Assert.AreEqual ("mono.bmp", r.FileName, "#A1");
        Assert.AreSame (utf8, r.TextFileEncoding, "#A2");
        Assert.AreEqual ("Bitmap", r.TypeName, "#A3");

        r = new ResXFileRef ("mono.bmp", "Bitmap", (Encoding) null);
        Assert.AreEqual ("mono.bmp", r.FileName, "#B1");
        Assert.IsNull (r.TextFileEncoding, "#B2");
        Assert.AreEqual ("Bitmap", r.TypeName, "#B3");
    }

    [Test]
    public void Constructor2_FileName_Null ()
    {
        try {
            new ResXFileRef ((string) null, "Bitmap", Encoding.UTF8);
            Assert.Fail ("#1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#2");
            Assert.IsNotNull (ex.Message, "#3");
            Assert.IsNull (ex.ParamName, "#4");
            Assert.IsNull (ex.InnerException, "#6");
        }
    }

    [Test]
    public void Constructor2_TypeName_Null ()
    {
        try {
            new ResXFileRef ("mono.bmp", (string) null, Encoding.UTF8);
            Assert.Fail ("#1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#2");
            Assert.IsNotNull (ex.Message, "#3");
            Assert.IsNull (ex.ParamName, "#4");
            Assert.IsNull (ex.InnerException, "#6");
        }
    }

    [Test]
    public void ToStringTest ()
    {
        var r = new ResXFileRef ("mono.bmp", "Bitmap");
        Assert.AreEqual ("mono.bmp;Bitmap", r.ToString (), "#1");

        r = new ResXFileRef ("mono.bmp", "Bitmap", Encoding.UTF8);
        Assert.AreEqual ("mono.bmp;Bitmap;utf-8", r.ToString (), "#2");

        r = new ResXFileRef ("mono.bmp", "Bitmap", (Encoding) null);
        Assert.AreEqual ("mono.bmp;Bitmap", r.ToString (), "#3");
    }
}

[TestFixture]
public class ResXFileRefConverterTest : Windows.Forms.TestHelper
{
    [SetUp]
    protected override void SetUp () {
        _converter = new ResXFileRef.Converter ();
        _tempDirectory = Path.Combine (Path.GetTempPath (), "ResXResourceReaderTest");
        if (!Directory.Exists (_tempDirectory)) {
            Directory.CreateDirectory (_tempDirectory);
        }
        _tempFileUTF7 = Path.Combine (_tempDirectory, "string_utf7.txt");
        using (var sw = new StreamWriter (_tempFileUTF7, false, Encoding.UTF7)) {
            sw.Write ("\u0021\u0026\u002A\u003B");
        }
        base.SetUp ();
    }

    [TearDown]
    protected override void TearDown ()
    {
        if (Directory.Exists (_tempDirectory))
            Directory.Delete (_tempDirectory, true);
        base.TearDown ();
    }

    [Test]
    public void CanConvertFrom ()
    {
        Assert.IsTrue (_converter.CanConvertFrom (typeof (string)), "#1");
        Assert.IsFalse (_converter.CanConvertFrom (typeof (byte [])), "#2");
    }

    [Test]
    public void CanConvertTo ()
    {
        Assert.IsTrue (_converter.CanConvertTo (typeof (string)), "#1");
        Assert.IsFalse (_converter.CanConvertTo (typeof (MemoryStream)), "#2");
        Assert.IsFalse (_converter.CanConvertTo (typeof (Bitmap)), "#3");
    }

    [Test]
    public void ConvertFrom_File_DoesNotExist ()
    {
        // file does not exist
        var fileRef = "doesnotexist.txt;" + typeof (string).AssemblyQualifiedName;
        try {
            _converter.ConvertFrom (fileRef);
            Assert.Fail ("#A1");
        } catch (FileNotFoundException ex) {
            Assert.AreEqual (typeof (FileNotFoundException), ex.GetType (), "#A2");
            Assert.IsNull (ex.InnerException, "#A3");
            Assert.IsNotNull (ex.FileName, "#A4");
            Assert.AreEqual (Path.Combine (Directory.GetCurrentDirectory (), "doesnotexist.txt"), ex.FileName, "#A5");
            Assert.IsNotNull (ex.Message, "#A6");
        }
    }

    [Test]
    public void ConvertFrom_Type_NotSet ()
    {
        var fileRef = "doesnotexist.txt";
        try {
            _converter.ConvertFrom (fileRef);
            Assert.Fail ("#B1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#B2");
            Assert.IsNull (ex.InnerException, "#B3");
            Assert.IsNotNull (ex.Message, "#B4");
            Assert.AreEqual ("value", ex.Message, "#B5");
            Assert.IsNull (ex.ParamName, "#B6");
        }
    }

    [Test]
    public void ConvertFrom_NotString ()
    {
        Assert.IsNull (_converter.ConvertFrom (null), "#G1");
        Assert.IsNull (_converter.ConvertFrom (1), "#G2");
        Assert.IsNull (_converter.ConvertFrom (true), "#G3");
    }

    [Test]
    public void ConvertFrom_Type_String ()
    {
        // read UTF-7 content without setting encoding
        var fileRef = _tempFileUTF7 + ";" + typeof (string).AssemblyQualifiedName;
        var result = _converter.ConvertFrom (fileRef) as string;
        Assert.IsNotNull (result, "#A1");
        Assert.IsFalse (result == "\u0021\u0026\u002A\u003B", "#A2");

        // read UTF-7 content using UTF-7 encoding
        fileRef = _tempFileUTF7 + ";" + typeof (string).AssemblyQualifiedName + ";utf-7";
        result = _converter.ConvertFrom (fileRef) as string;
        Assert.IsNotNull (result, "#B1");
        Assert.AreEqual ("\u0021\u0026\u002A\u003B", result, "#B2");

        // invalid encoding
        fileRef = _tempFileUTF7 + ";" + typeof (string).AssemblyQualifiedName + ";utf-99";
        try {
            _converter.ConvertFrom (fileRef);
            Assert.Fail ("#D1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#D2");
            Assert.IsNull (ex.InnerException, "#D3");
            Assert.IsNotNull (ex.Message, "#D4");
            Assert.IsTrue (ex.Message.IndexOf ("'utf-99'") != -1, "#D5");
            Assert.IsNotNull (ex.ParamName, "#D6");
            Assert.AreEqual ("name", ex.ParamName, "#D7");
        }
    }

    [Test]
    public void ConvertFrom_Type_String_FilePathWithBackslashes ()
    {
        if (Path.DirectorySeparatorChar == '\\')
            // non-windows test
            return;

        var fileContents = "foobar";
        var fileName = "foo.txt";
        var filePath = Path.Combine (_tempDirectory, fileName);
        File.WriteAllText (filePath, fileContents);

        filePath = _tempDirectory + "\\.\\" + fileName;

        var fileRef = filePath + ";" + typeof (string).AssemblyQualifiedName;
        var result = _converter.ConvertFrom (fileRef) as string;
        Assert.IsNotNull (result, "#A1");
        Assert.AreEqual (result, fileContents, "#A2");
    }

    [Test]
    public void ConvertFrom_Type_StreamReader ()
    {
        // read UTF-7 content without setting encoding
        var fileRef = _tempFileUTF7 + ";" + typeof (StreamReader).AssemblyQualifiedName;
        using (var sr = (StreamReader) _converter.ConvertFrom (fileRef)) {
            var result = sr.ReadToEnd ();
            Assert.IsTrue (result.Length > 0, "#D1");
            Assert.IsFalse (result == "\u0021\u0026\u002A\u003B", "#D2");
        }

        // UTF-7 encoding is set, but not used
        fileRef = _tempFileUTF7 + ";" + typeof (StreamReader).AssemblyQualifiedName + ";utf-7";
        using (var sr = (StreamReader) _converter.ConvertFrom (fileRef)) {
            var result = sr.ReadToEnd ();
            Assert.IsTrue (result.Length > 0, "#F1");
            Assert.IsFalse (result == "\u0021\u0026\u002A\u003B", "#F2");
        }

        // invalid encoding is set, no error
        fileRef = _tempFileUTF7 + ";" + typeof (StreamReader).AssemblyQualifiedName + ";utf-99";
        using (var sr = (StreamReader) _converter.ConvertFrom (fileRef)) {
            var result = sr.ReadToEnd ();
            Assert.IsTrue (result.Length > 0, "#A1");
            Assert.IsFalse (result == "\u0021\u0026\u002A\u003B", "#A2");
        }
    }

    [Test]
    public void ConvertFrom_Type_MemoryStream ()
    {
        var fileRef = _tempFileUTF7 + ";" + typeof (MemoryStream).AssemblyQualifiedName;
        using var ms = (MemoryStream) _converter.ConvertFrom (fileRef);
        Assert.IsTrue (ms.Length > 0);
    }

    [Test]
    public void ConvertTo ()
    {
        var r = new ResXFileRef ("mono.bmp", "Bitmap");
        Assert.AreEqual ("mono.bmp;Bitmap", (string) _converter.ConvertTo (
            r, typeof (string)), "#1");

        r = new ResXFileRef ("mono.bmp", "Bitmap", Encoding.UTF8);
        Assert.AreEqual ("mono.bmp;Bitmap;utf-8", (string) _converter.ConvertTo (
            r, typeof (string)), "#2");

        r = new ResXFileRef ("mono.bmp", "Bitmap", (Encoding) null);
        Assert.AreEqual ("mono.bmp;Bitmap", (string) _converter.ConvertTo (
            r, typeof (string)), "#3");
    }

    private TypeConverter _converter;
    private string _tempDirectory;
    private string _tempFileUTF7;
}