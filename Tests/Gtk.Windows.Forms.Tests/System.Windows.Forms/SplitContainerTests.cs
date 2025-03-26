using System.Windows.Forms;
using System.Drawing;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class SplitContainerTests : TestHelper
{
    [Test]
    public void TestSplitContainerConstruction ()
    {
        var sc = new SplitContainer ();

        Assert.AreEqual (new Size (150, 100), sc.Size, "A1");
        Assert.AreEqual (FixedPanel.None, sc.FixedPanel, "A2");
        Assert.AreEqual (Orientation.Vertical, sc.Orientation, "A4");
        Assert.AreEqual (50, sc.SplitterDistance, "A11");
        Assert.AreEqual (1, sc.SplitterIncrement, "A12");
        Assert.AreEqual (4, sc.SplitterWidth, "A14");
        Assert.AreEqual (BorderStyle.None, sc.BorderStyle, "A14");
        Assert.AreEqual (DockStyle.None, sc.Dock, "A15");
    }
		
    [Test]
    public void TestProperties ()
    {
        var sc = new SplitContainer ();
			
        sc.BorderStyle = BorderStyle.FixedSingle;
        Assert.AreEqual (BorderStyle.FixedSingle, sc.BorderStyle, "C1");

        sc.Dock =  DockStyle.Fill;
        Assert.AreEqual (DockStyle.Fill, sc.Dock, "C2");

        sc.FixedPanel = FixedPanel.Panel1;
        Assert.AreEqual (FixedPanel.Panel1, sc.FixedPanel, "C3");

        sc.Orientation = Orientation.Horizontal;
        Assert.AreEqual (Orientation.Horizontal, sc.Orientation, "C5");

        sc.SplitterDistance = 77;
        Assert.AreEqual (77, sc.SplitterDistance, "C10");
			
        sc.SplitterIncrement = 5;
        Assert.AreEqual (5, sc.SplitterIncrement, "C11");
			
        sc.SplitterWidth = 10;
        Assert.AreEqual (10, sc.SplitterWidth, "C12");
    }
		
    [Test]
    public void TestPanelProperties ()
    {
        var sc = new SplitContainer ();
        var p = sc.Panel1;

        Assert.AreEqual (AnchorStyles.Top | AnchorStyles.Left, p.Anchor, "D1");
        p.Anchor = AnchorStyles.None;
        Assert.AreEqual (AnchorStyles.None, p.Anchor, "D1-2");

        Assert.AreEqual (false, p.AutoSize, "D2");
        p.AutoSize = true;
        Assert.AreEqual (true, p.AutoSize, "D2-2");

        Assert.AreEqual (BorderStyle.None, p.BorderStyle, "D4");
        p.BorderStyle = BorderStyle.FixedSingle;
        Assert.AreEqual (BorderStyle.FixedSingle, p.BorderStyle, "D4-2");

        Assert.AreEqual (DockStyle.None, p.Dock, "D5");
        p.Dock = DockStyle.Left;
        Assert.AreEqual (DockStyle.Left, p.Dock, "D5-2");

        Assert.AreEqual (new Point (0, 0), p.Location, "D7");
        p.Location = new Point (10, 10);
        Assert.AreEqual (new Point (0, 0), p.Location, "D7-2");

        Assert.AreEqual (new Size (0, 0), p.MaximumSize, "D8");
        p.MaximumSize = new Size (10, 10);
        Assert.AreEqual (new Size (10, 10), p.MaximumSize, "D8-2");

        Assert.AreEqual (new Size (0, 0), p.MinimumSize, "D9");
        p.MinimumSize = new Size (10, 10);
        Assert.AreEqual (new Size (10, 10), p.MinimumSize, "D9-2");

        Assert.AreEqual (String.Empty, p.Name, "D10");
        p.Name = "MyPanel";
        Assert.AreEqual ("MyPanel", p.Name, "D10-2");

        // We set a new max/min size above, so let's start over with new controls
        sc = new SplitContainer();
        p = sc.Panel1;

        Assert.AreEqual (new Size (50, 100), p.Size, "D12");
        p.Size = new Size (10, 10);
        Assert.AreEqual (new Size (50, 100), p.Size, "D12-2");

        //Assert.AreEqual (0, p.TabIndex, "D13");
        p.TabIndex = 4;
        Assert.AreEqual (4, p.TabIndex, "D13-2");

        Assert.AreEqual (false, p.TabStop, "D14");
        p.TabStop = true;
        Assert.AreEqual (true, p.TabStop, "D14-2");

        Assert.AreEqual (true, p.Visible, "D15");
        p.Visible = false;
        Assert.AreEqual (false, p.Visible, "D15-2");
    }
		
    [Test]
    public void TestPanelHeightProperty ()
    {
        Assert.Throws<NotSupportedException>(() =>
        {
            var sc = new SplitContainer();
            var p = sc.Panel1;

            Assert.AreEqual(100, p.Height, "E1");

            p.Height = 200;
        });
    }

    [Test]
    public void TestPanelWidthProperty ()
    {
        Assert.Throws<NotSupportedException>(() =>
        {
            var sc = new SplitContainer();
            var p = sc.Panel1;

            Assert.AreEqual(50, p.Width, "F1");

            p.Width = 200;
        });
    }

    [Test]
    public void TestPanelParentProperty ()
    {
        Assert.Throws<NotSupportedException>(() =>
        {
            var sc = new SplitContainer();
            var sc2 = new SplitContainer();
            var p = sc.Panel1;

            Assert.AreEqual(sc, p.Parent, "G1");

            p.Parent = sc2;
        });
    }

    [Test]
    public void TestFixedPanelNone ()
    {
        var sc = new SplitContainer ();

        Assert.AreEqual (50, sc.SplitterDistance, "I1");

        sc.Width = 300;
        Assert.AreEqual (100, sc.SplitterDistance, "I2");
    }
		
    [Test]
    public void TestFixedPanel1 ()
    {
        var sc = new SplitContainer ();
        sc.FixedPanel = FixedPanel.Panel1;
			
        Assert.AreEqual (50, sc.SplitterDistance, "J1");

        sc.Width = 300;
        Assert.AreEqual (50, sc.SplitterDistance, "J2");
    }
		
    [Test]
    public void TestFixedPanel2 ()
    {
        var sc = new SplitContainer ();
        sc.FixedPanel = FixedPanel.Panel2;

        Assert.AreEqual (50, sc.SplitterDistance, "K1");

        sc.Width = 300;
        Assert.AreEqual (200, sc.SplitterDistance, "K2");
    }
}