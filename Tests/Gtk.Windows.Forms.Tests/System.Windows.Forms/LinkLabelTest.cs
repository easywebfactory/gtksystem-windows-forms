//
// LinkLabelTest.cs: MWF LinkLabel unit tests.
//
// Author:
//   Everaldo Canuto (ecanuto@novell.com)
//
// (C) 2007 Novell, Inc. (http://www.novell.com)
//

using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class LinkLabelTest : TestHelper
{
    [Test]
    public void LinkLabelAccessibility ()
    {
        LinkLabel l = new LinkLabel ();
        Assert.IsNotNull (l.AccessibilityObject, "#1");
    }

    [Test]
    public void TestTabStop ()
    {
        LinkLabel l = new LinkLabel();

        Assert.IsFalse (l.TabStop, "#1");
        l.Text = "Hello";
        Assert.IsTrue (l.TabStop, "#2");
        l.Text = "";
        Assert.IsFalse (l.TabStop, "#3");
    }
		
    [Test] // bug #344012
    public void InvalidateManualLinks ()
    {
        Form form = new Form ();
        form.ShowInTaskbar = false;

        LinkLabel l = new LinkLabel ();
        l.Text = "linkLabel1";
        form.Controls.Add (l);

        LinkLabel.Link link = new LinkLabel.Link (2, 5);
        l.Links.Add (link);

        form.Show ();
        form.Dispose ();
    }
}


[TestFixture]
public class LinkTest : TestHelper
{
    [Test]
    public void Constructor ()
    {
        LinkLabel.Link l = new LinkLabel.Link ();
			
        Assert.AreEqual (null, l.Description, "A1");
        Assert.AreEqual (null, l.LinkData, "A4");

        l = new LinkLabel.Link (5, 20);

        Assert.AreEqual (null, l.Description, "A9");
        Assert.AreEqual (null, l.LinkData, "A12");

        l = new LinkLabel.Link (3, 7, "test");

        Assert.AreEqual (null, l.Description, "A17");
        Assert.AreEqual ("test", l.LinkData, "A20");
    }
}

[TestFixture]
public class LinkCollectionTest : TestHelper
{
    [Test] // ctor (LinkLabel)
    public void Constructor1 ()
    {
        LinkLabel l = new LinkLabel ();
        l.Text = "Managed Windows Forms";

        LinkLabel.LinkCollection links1 = new LinkLabel.LinkCollection (
            l);
        LinkLabel.LinkCollection links2 = new LinkLabel.LinkCollection (
            l);

        Assert.AreEqual (1, links1.Count, "#A1");
        Assert.IsFalse (links1.IsReadOnly, "#A2");
    }

    [Test] // ctor (LinkLabel)
    public void Constructor1_Owner_Null ()
    {
        try {
            new LinkLabel.LinkCollection ((LinkLabel) null);
            Assert.Fail ("#1");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.IsNotNull (ex.ParamName, "#5");
            Assert.AreEqual ("owner", ex.ParamName, "#6");
        }
    }

    [Test] // Add (LinkLabel.Link)
    public void Add1 ()
    {
        LinkLabel l = new LinkLabel ();
        l.Text = "Managed Windows Forms";

        LinkLabel.LinkCollection links1 = new LinkLabel.LinkCollection (
            l);
        LinkLabel.LinkCollection links2 = new LinkLabel.LinkCollection (
            l);

        LinkLabel.Link linkA = new LinkLabel.Link (0, 7);
        Assert.AreEqual (0, links1.Add (linkA), "#A1");
        Assert.AreEqual (1, links1.Count, "#A2");
        Assert.AreEqual (1, links2.Count, "#A3");
        Assert.AreSame (linkA, links1 [0], "#A6");
        Assert.AreSame (linkA, links2 [0], "#A7");

        LinkLabel.Link linkB = new LinkLabel.Link (8, 7);
        Assert.AreEqual (1, links1.Add (linkB), "#B1");
        Assert.AreEqual (2, links1.Count, "#B2");
        Assert.AreEqual (2, links2.Count, "#B3");
        Assert.AreSame (linkA, links1 [0], "#B6");
        Assert.AreSame (linkA, links2 [0], "#B7");
        Assert.AreSame (linkB, links1 [1], "#B8");
        Assert.AreSame (linkB, links2 [1], "#B9");

        LinkLabel.LinkCollection links3 = new LinkLabel.LinkCollection (
            l);
        Assert.AreEqual (2, links3.Count, "#C1");
        Assert.AreSame (linkA, links3 [0], "#C3");
        Assert.AreSame (linkB, links3 [1], "#C4");
    }

    [Test] // Add (LinkLabel.Link)
    public void Add1_Overlap ()
    {
        LinkLabel l = new LinkLabel ();
        l.Text = "Managed Windows Forms";

        LinkLabel.LinkCollection links = new LinkLabel.LinkCollection (
            l);

        LinkLabel.Link linkA = new LinkLabel.Link (0, 7);
        links.Add (linkA);
        Assert.AreEqual (1, links.Count, "#A1");
        Assert.AreSame (linkA, links [0], "#A3");

        LinkLabel.Link linkB = new LinkLabel.Link (5, 4);
        try {
            links.Add (linkB);
            Assert.Fail ("#B1");
        } catch (InvalidOperationException ex) {
            // Overlapping link regions
            Assert.AreEqual (typeof (InvalidOperationException), ex.GetType (), "#B2");
            Assert.IsNull (ex.InnerException, "#B3");
            Assert.IsNotNull (ex.Message, "#B4");
        }

        Assert.AreEqual (2, links.Count, "#B5");
        Assert.AreSame (linkA, links [0], "#B7");
        Assert.AreSame (linkB, links [1], "#B8");

        LinkLabel.Link linkC = new LinkLabel.Link (14, 3);
        try {
            links.Add (linkC);
            Assert.Fail ("#C1");
        } catch (InvalidOperationException ex) {
            // Overlapping link regions
            Assert.AreEqual (typeof (InvalidOperationException), ex.GetType (), "#C2");
            Assert.IsNull (ex.InnerException, "#C3");
            Assert.IsNotNull (ex.Message, "#C4");
        }

        Assert.AreEqual (3, links.Count, "#C5");
        Assert.AreSame (linkA, links [0], "#C7");
        Assert.AreSame (linkB, links [1], "#C8");
        Assert.AreSame (linkC, links [2], "#C9");
    }

    [Test] // Add (LinkLabel.Link)
    public void Add1_Value_Null ()
    {
        LinkLabel l = new LinkLabel ();
        l.Text = "Managed Windows Forms";

        LinkLabel.LinkCollection links = new LinkLabel.LinkCollection (
            l);
        try {
            links.Add ((LinkLabel.Link) null);
            Assert.Fail ("#1");
        } catch (NullReferenceException) {
        }
    }

}