//
// CultureTest.cs: Test cases for culture-invariant string convertions
//
// Authors:
//     Robert Jordan <robertj@gmx.net>
//

using System.Collections;
using System.Globalization;
using System.Drawing;
using System.Resources;

namespace GtkTests.System.Resources;

[TestFixture]
public class CultureTest : Windows.Forms.TestHelper
{
    string? fileName;

    [SetUp]
    protected override void SetUp ()
    {
        fileName = Path.GetTempFileName ();
        base.SetUp ();
    }

    [TearDown]
    protected override void TearDown ()
    {
        if (fileName != null)
        {
            File.Delete(fileName);
        }

        base.TearDown ();
    }

    [Test]
    public void Test ()
    {
        Thread.CurrentThread.CurrentCulture =
            Thread.CurrentThread.CurrentUICulture = new CultureInfo ("de-DE");

        var w = new ResXResourceWriter (fileName);
        w.AddResource ("point", new Point (42, 43));
        w.Generate ();
        w.Close ();

        var count = 0;
        var r = new ResXResourceReader (fileName);
        var e = r.GetEnumerator ();
        using var disposable = e as IDisposable;
        while (e.MoveNext ()) {
            if ((string) e.Key == "point") {
                Assert.AreEqual (typeof (Point).FullName, e.Value!.GetType().FullName, "#1");
                var p = (Point) e.Value;
                Assert.AreEqual (42, p.X, "#2");
                Assert.AreEqual (43, p.Y, "#3");
                count++;
            }
        }
        r.Close ();
        Assert.AreEqual (1, count, "#100");
    }
}