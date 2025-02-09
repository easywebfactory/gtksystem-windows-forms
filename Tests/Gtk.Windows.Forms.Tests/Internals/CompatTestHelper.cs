using System.Collections;
using System.Drawing;
using System.Resources;

namespace GtkTests;

internal class CompatTestHelper
{
    public static void TestReader(string fileName)
    {
        var r = new ResXResourceReader(fileName);
        var h = new Hashtable();
        foreach (DictionaryEntry e in r)
        {
            h.Add(e.Key, e.Value);
        }
        r.Close();

        Assert.AreEqual("hola", (string)h["String"]!, fileName + "#1");
        Assert.AreEqual("hello", (string)h["String2"]!, fileName + "#2");
        Assert.AreEqual(42, (int?)h["Int"], fileName + "#3");
        Assert.AreEqual(PlatformID.Win32NT, (PlatformID?)h["Enum"], fileName + "#4");
        Assert.AreEqual(43, ((Point)h["Convertible"]!).X, fileName + "#5");
        Assert.AreEqual(13, ((byte[])h["ByteArray"]!)[1], fileName + "#7");
        Assert.AreEqual(16, ((byte[])h["ByteArray2"]!)[1], fileName + "#8");
        Assert.IsNull(h["InvalidMimeType"], "#11");
        Assert.IsNotNull(h["Image"], fileName + "#12");
        Assert.AreEqual(typeof(Bitmap).FullName, h["Image"].GetType().FullName, fileName + "#13");
    }
}