using System.Globalization;
using System.Windows.Forms;
using Threading = System.Threading;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class NumericUpDownTest : TestHelper
{
    [Test]
    public void Minimum ()
    {
        Form f = new Form ();
        NumericUpDown nud = new NumericUpDown ();
        nud.Value = 0;
        nud.Minimum = 2;
        nud.Maximum = 4;
        f.Controls.Add (nud);
        f.Show ();

        Assert.AreEqual (2, nud.Value, "#A1");
        nud.Minimum = 3;
        Assert.AreEqual (3, nud.Value, "#A2");
        f.Dispose ();
    }

    [Test]
    public void Maximum ()
    {
        Form f = new Form ();
        NumericUpDown nud = new NumericUpDown ();
        nud.BeginInit ();
        nud.Value = 1000;
        nud.Minimum = 2;
        nud.Maximum = 4;
        nud.EndInit ();
        f.Controls.Add (nud);
        f.Show ();

        Assert.AreEqual (4, nud.Value, "#A1");
        nud.Maximum = 3;
        Assert.AreEqual (3, nud.Value, "#A2");
        f.Dispose ();
    }

    [Test]
    public void Hexadecimal ()
    {
        Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo ("en-US");
        Form f = new Form ();
        NumericUpDown nud = new NumericUpDown ();
        nud.Maximum = 100000;
        f.Controls.Add (nud);
        f.Show ();

        nud.Value = 0; // bug 661750
        Assert.AreEqual ("0", nud.Text, "#A3");
        Assert.AreEqual (0, nud.Value, "#A4");
        f.Dispose ();
    }

    [Test]
    public void SetValueThrowsException ()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            NumericUpDown nud = new NumericUpDown();
            nud.Maximum = 3;
            nud.Value = 4;
            nud.Dispose();
        });
    }

    [Test]
    public void InitTest ()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            NumericUpDown nud = new NumericUpDown();
            nud.BeginInit();
            nud.Maximum = 3;
            nud.BeginInit();
            nud.EndInit();
            nud.Value = 4;
            nud.Dispose();
        });
    }
}