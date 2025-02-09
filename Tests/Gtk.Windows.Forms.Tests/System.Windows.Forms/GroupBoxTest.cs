//
// GroupBoxTest.cs: Test cases for GroupBox.
//
// Author:
//   Ritvik Mayank (mritvik@novell.com)
//
// (C) 2005 Novell, Inc. (http://www.novell.com)
//

using System.Windows.Forms;
using System.Drawing;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class GroupBoxTest : TestHelper
{
    [Test]
    public void Constructor ()
    {
        GroupBox gb = new GroupBox ();

        Assert.AreEqual (false, gb.AllowDrop, "A1");
        // Top/Height are dependent on font height
        // Assert.AreEqual (new Rectangle (3, 16, 194, 81), gb.DisplayRectangle, "A2");
        Assert.AreEqual (false, gb.TabStop, "A4");
        Assert.AreEqual (string.Empty, gb.Text, "A5");
			
        Assert.AreEqual (false, gb.AutoSize, "A6");
        Assert.AreEqual ("System.Windows.Forms.GroupBox+GroupBoxAccessibleObject", gb.AccessibilityObject.GetType ().ToString (), "A9");
    }
		
    [Test]
    public void AutoSize ()
    {
        if (TestHelper.RunningOnUnix)
            Assert.Ignore ("Dependent on font height and theme, values are for windows.");
				
        Form f = new Form ();
        f.ShowInTaskbar = false;

        GroupBox p = new GroupBox ();
        p.AutoSize = true;
        f.Controls.Add (p);

        Button b = new Button ();
        b.Size = new Size (200, 200);
        b.Location = new Point (200, 200);
        p.Controls.Add (b);

        f.Show ();

        Assert.AreEqual (new Size (406, 419), p.ClientSize, "A1");

        p.Controls.Remove (b);
        Assert.AreEqual (new Size (200, 100), p.ClientSize, "A2");

        f.Dispose ();
    }

    [Test]
    public void PropertyDisplayRectangle ()
    {
        GroupBox gb = new GroupBox ();
        gb.Size = new Size (200, 200);
			
        Assert.AreEqual (new Padding (3), gb.Padding, "A0");
        gb.Padding = new Padding (25, 25, 25, 25);

        Assert.AreEqual (new Rectangle (0, 0, 200, 200), gb.ClientRectangle, "A1");

        // Basically, we are testing that the DisplayRectangle includes
        // Padding.  Top/Height are affected by font height, so we aren't
        // using exact numbers.
        Assert.AreEqual (25, gb.DisplayRectangle.Left, "A2");
        Assert.AreEqual (150, gb.DisplayRectangle.Width, "A3");
        Assert.IsTrue (gb.DisplayRectangle.Top > gb.Padding.Top, "A4");
        Assert.IsTrue (gb.DisplayRectangle.Height < (gb.Height - gb.Padding.Vertical), "A5");
    }
}