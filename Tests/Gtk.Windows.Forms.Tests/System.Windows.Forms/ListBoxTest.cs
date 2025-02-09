//
// ComboBoxTest.cs: Test cases for ComboBox.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2005 Novell, Inc. (http://www.novell.com)
//
// Authors:
//   	Ritvik Mayank <mritvik@novell.com>
//	Jordi Mas i Hernandez <jordi@ximian.com>
//

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class ListBoxTest : TestHelper
{
    ListBox listBox;
    Form form;

    [TearDown]
    public void TestTearDown()
    {
        listBox.Dispose();
    }

    [SetUp]
    protected override void SetUp ()
    {
        listBox = new ListBox();
        form = new Form();
        form.ShowInTaskbar = false;
        base.SetUp ();
    }

    [TearDown]
    protected override void TearDown ()
    {
        form.Dispose ();
        base.TearDown ();
    }

    [Test] // bug #465422
    public void RemoveLast ()
    {
        listBox.Items.Clear ();

        for (var i = 0; i < 3; i++)
            listBox.Items.Add (i.ToString ());

        // need to create control to actually test the invalidation
        listBox.CreateControl ();

        // select last - then remove an item that is *not* the last,
        // so basically the selection is invalidated implicitly
        listBox.SelectedIndex = 2;
        listBox.Items.RemoveAt (1);
        Assert.AreEqual (1, listBox.SelectedIndex, "#A1");

        listBox.SelectedIndex = 0;
        Assert.AreEqual (0, listBox.SelectedIndex, "#B1");

        // 
        // MultiSelection
        //
        listBox.ClearSelected ();
        listBox.Items.Clear ();
        for (var i = 0; i < 3; i++)
            listBox.Items.Add (i.ToString ());

        listBox.SelectionMode = SelectionMode.MultiSimple;

        listBox.SetSelected (0, true);
        listBox.SetSelected (2, true);

        Assert.AreEqual (true, listBox.GetSelected (0), "#C1");
        Assert.AreEqual (false, listBox.GetSelected (1), "#C2");
        Assert.AreEqual (true, listBox.GetSelected (2), "#C3");

        listBox.Items.RemoveAt (2);
        Assert.AreEqual (true, listBox.GetSelected (0), "#D1");
        Assert.AreEqual (false, listBox.GetSelected (1), "#D2");
        Assert.AreEqual (1, listBox.SelectedIndices.Count, "#D3");
    }

    [Test]
    public void ListBoxPropertyTest ()
    {
        Assert.AreEqual (0, listBox.ColumnWidth, "#1");
        Assert.AreEqual (DrawMode.Normal, listBox.DrawMode, "#2");
        Assert.AreEqual (0, listBox.HorizontalExtent, "#3");
        Assert.AreEqual (false, listBox.HorizontalScrollbar, "#4");
        Assert.AreEqual (true, listBox.IntegralHeight, "#5");
        //Assert.AreEqual (13, listBox.ItemHeight, "#6"); // Note: Item height depends on the current font.
        listBox.Items.Add ("a");
        listBox.Items.Add ("b");
        listBox.Items.Add ("c");
        Assert.AreEqual (3,  listBox.Items.Count, "#7");
        Assert.AreEqual (false, listBox.MultiColumn, "#8");
        //Assert.AreEqual (46, listBox.PreferredHeight, "#9"); // Note: Item height depends on the current font.
        //Assert.AreEqual (RightToLeft.No , listBox.RightToLeft, "#10"); // Depends on Windows version
        Assert.AreEqual (false, listBox.ScrollAlwaysVisible, "#11");
        Assert.AreEqual (-1, listBox.SelectedIndex, "#12");
        listBox.SetSelected (2,true);
        Assert.AreEqual (2, listBox.SelectedIndices[0], "#13");
        Assert.AreEqual ("c", listBox.SelectedItem, "#14");
        Assert.AreEqual ("c", listBox.SelectedItems[0], "#15");
        Assert.AreEqual (SelectionMode.One, listBox.SelectionMode, "#16");
        listBox.SetSelected (2,false);
        Assert.AreEqual (false, listBox.Sorted, "#17");
        Assert.AreEqual ("", listBox.Text, "#18");
        Assert.AreEqual (0, listBox.TopIndex, "#19");
        Assert.AreEqual (true, listBox.UseTabStops, "#20");
    }

    [Test]
    public void BeginEndUpdateTest ()
    {
        form.Visible = true;
        listBox.Items.Add ("A");
        listBox.Visible = true;
        form.Controls.Add (listBox);
        listBox.BeginUpdate ();
        for (var x = 1; x < 5000; x++)
        {
            listBox.Items.Add ("Item " + x.ToString ());
        }
        listBox.EndUpdate ();
        listBox.SetSelected (1, true);
        listBox.SetSelected (3, true);
        Assert.AreEqual (true, listBox.SelectedItems.Contains ("Item 3"), "#21");
    }

    [Test]
    public void ClearSelectedTest ()
    {
        form.Visible = true;
        listBox.Items.Add ("A");
        listBox.Visible = true;
        form.Controls.Add (listBox);
        listBox.SetSelected (0, true);
        Assert.AreEqual ("A", listBox.SelectedItems [0].ToString (),"#22");
        listBox.ClearSelected ();
        Assert.AreEqual (0, listBox.SelectedItems.Count,"#23");
    }

    [Test] // bug #80620
    [NUnit.Framework.Category ("NotWorking")]
    public void ClientRectangle_Borders ()
    {
        // This test is invalid because createcontrol forces .net to resize
        // the listbox using integralheight, which defaults to true.  This
        // will only hold for most font sizes.
        listBox.CreateControl ();
        Assert.AreEqual (listBox.ClientRectangle, new ListBox ().ClientRectangle);
    }

    [Ignore ("It depends on user system settings")]
    public void GetItemHeightTest ()
    {
        listBox.Visible = true;
        form.Controls.Add (listBox);
        listBox.Items.Add ("A");
        Assert.AreEqual (13, listBox.GetItemHeight (0) , "#28");
    }

    [Ignore ("It depends on user system settings")]
    public void GetItemRectangleTest ()
    {
        form.Visible = true;
        listBox.Visible = true;
        form.Controls.Add (listBox);
        listBox.Items.Add ("A");
        Assert.AreEqual (new Rectangle(0,0,116,13), listBox.GetItemRectangle (0), "#29");
    }

    [Test]
    public void GetSelectedTest ()
    {
        listBox.Items.Add ("A");
        listBox.Items.Add ("B");
        listBox.Items.Add ("C");
        listBox.Items.Add ("D");
        listBox.SelectionMode = SelectionMode.MultiSimple;
        listBox.Sorted = true;
        listBox.SetSelected (0,true);
        listBox.SetSelected (2,true);
        listBox.TopIndex=0;
        Assert.AreEqual (true, listBox.GetSelected (0), "#30");
        listBox.SetSelected (2,false);
        Assert.AreEqual (false, listBox.GetSelected (2), "#31");
    }

    [Test]
    public void IndexFromPointTest ()
    {
        listBox.Items.Add ("A");
        var pt = new Point (100,100);
        listBox.IndexFromPoint (pt);
        Assert.AreEqual (-1, listBox.IndexFromPoint (100,100), "#32");
    }

    [Test]
    public void FindStringTest ()
    {
        listBox.FindString ("Hola", -5); // No exception, it's empty
        var x = listBox.FindString ("Hello");
        Assert.AreEqual (-1, x, "#19");
        listBox.Items.AddRange(new object[] {"ACBD", "ABDC", "ACBD", "ABCD"});
        var myString = "ABC";
        x = listBox.FindString (myString);
        Assert.AreEqual (3, x, "#191");
        x = listBox.FindString (string.Empty);
        Assert.AreEqual (0, x, "#192");
        x = listBox.FindString ("NonExistant");
        Assert.AreEqual (-1, x, "#193");

        x = listBox.FindString ("A", -1);
        Assert.AreEqual (0, x, "#194");
        x = listBox.FindString ("A", 0);
        Assert.AreEqual (1, x, "#195");
        x = listBox.FindString ("A", listBox.Items.Count - 1);
        Assert.AreEqual (0, x, "#196");
        x = listBox.FindString ("a", listBox.Items.Count - 1);
        Assert.AreEqual (0, x, "#197");
    }

    [Test]
    public void FindStringExactTest ()
    {
        listBox.FindStringExact ("Hola", -5); // No exception, it's empty
        var x = listBox.FindStringExact ("Hello");
        Assert.AreEqual (-1, x, "#20");
        listBox.Items.AddRange (new object[] {"ABCD","ABC","ABDC"});
        var myString = "ABC";
        x = listBox.FindStringExact (myString);
        Assert.AreEqual (1, x, "#201");
        x = listBox.FindStringExact (string.Empty);
        Assert.AreEqual (-1, x, "#202");
        x = listBox.FindStringExact ("NonExistant");
        Assert.AreEqual (-1, x, "#203");

        x = listBox.FindStringExact ("ABCD", -1);
        Assert.AreEqual (0, x, "#204");
        x = listBox.FindStringExact ("ABC", 0);
        Assert.AreEqual (1, x, "#205");
        x = listBox.FindStringExact ("ABC", listBox.Items.Count - 1);
        Assert.AreEqual (1, x, "#206");
        x = listBox.FindStringExact ("abcd", listBox.Items.Count - 1);
        Assert.AreEqual (0, x, "#207");
    }

    //
    // Exceptions
    //

    [Test]
    public void BorderStyleException ()
    {
        Assert.Throws<InvalidEnumArgumentException>(() =>
        {
            listBox.BorderStyle = (BorderStyle)10;
        });
    }

    [Test]
    public void ColumnWidthException ()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            listBox.ColumnWidth = -1;
        });
    }

    [Test]
    public void DrawModeException ()
    {
        Assert.Throws<InvalidEnumArgumentException>(() =>
        {
            listBox.DrawMode = (DrawMode)10;
        });
    }

    [Test]
    public void DrawModeAndMultiColumnException ()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            listBox.MultiColumn = true;
            listBox.DrawMode = DrawMode.OwnerDrawVariable;
        });
    }

    [Test]
    public void ItemHeightException ()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            listBox.ItemHeight = 256;
        });
    }

    [Test] // bug #80696
    public void SelectedIndex_Created ()
    {
        var form = new Form ();
        var listBox = new ListBox ();
        listBox.Items.Add ("A");
        listBox.Items.Add ("B");
        form.Controls.Add (listBox);
        form.Show ();

        Assert.AreEqual (-1, listBox.SelectedIndex, "#1");
        listBox.SelectedIndex = 0;
        Assert.AreEqual (0, listBox.SelectedIndex, "#2");
        listBox.SelectedIndex = -1;
        Assert.AreEqual (-1, listBox.SelectedIndex, "#3");
        listBox.SelectedIndex = 1;
        Assert.AreEqual (1, listBox.SelectedIndex, "#4");
			
        form.Close ();
    }

    [Test] // bug #80753
    public void SelectedIndex_NotCreated ()
    {
        var listBox = new ListBox ();
        listBox.Items.Add ("A");
        listBox.Items.Add ("B");
        Assert.AreEqual (-1, listBox.SelectedIndex, "#1");
        listBox.SelectedIndex = 0;
        Assert.AreEqual (0, listBox.SelectedIndex, "#2");
        listBox.SelectedIndex = -1;
        Assert.AreEqual (-1, listBox.SelectedIndex, "#3");
        listBox.SelectedIndex = 1;
        Assert.AreEqual (1, listBox.SelectedIndex, "#4");
    }

    [Test]
    public void SelectedIndex_Removed ()
    {
        var listBox = new ListBox ();
        listBox.Items.Add ("A");
        listBox.Items.Add ("B");
        listBox.Items.Add ("C");

        Assert.AreEqual (-1, listBox.SelectedIndex, "#1");
        listBox.SelectedIndex = 2;
        Assert.AreEqual (2, listBox.SelectedIndex, "#2");
        listBox.Items.RemoveAt (2);
        Assert.AreEqual (-1, listBox.SelectedIndex, "#3");

        listBox.SelectedIndex = 0;
        Assert.AreEqual (0, listBox.SelectedIndex, "#4");
        listBox.Items.RemoveAt (0);
        Assert.AreEqual (-1, listBox.SelectedIndex, "#5");
    }

    // This should also apply to MultiSimple selection mode
    [Test]
    public void Selection_MultiExtended ()
    {
        listBox.Items.Add ("A");
        listBox.Items.Add ("B");
        listBox.Items.Add ("C");
        listBox.Items.Add ("D");
        listBox.SelectionMode = SelectionMode.MultiExtended;

        //
        // First part: test the order of SelectedItems as well
        // as SelectedIndex when more than one item is selected
        //
        listBox.SelectedItems.Add ("D");
        listBox.SelectedItems.Add ("B");
        Assert.AreEqual (1, listBox.SelectedIndex, "#A1");
        Assert.AreEqual (2, listBox.SelectedItems.Count, "#A2");
        Assert.AreEqual ("B", listBox.SelectedItems [0], "#A3");
        Assert.AreEqual ("D", listBox.SelectedItems [1], "#A4");

        listBox.SelectedItems.Add ("C");
        Assert.AreEqual (1, listBox.SelectedIndex, "#B1");
        Assert.AreEqual (3, listBox.SelectedItems.Count, "#B2");
        Assert.AreEqual ("B", listBox.SelectedItems [0], "#B3");
        Assert.AreEqual ("C", listBox.SelectedItems [1], "#B4");
        Assert.AreEqual ("D", listBox.SelectedItems [2], "#B5");

        listBox.SelectedItems.Add ("A");
        Assert.AreEqual (0, listBox.SelectedIndex, "#C1");
        Assert.AreEqual (4, listBox.SelectedItems.Count, "#C2");
        Assert.AreEqual ("A", listBox.SelectedItems [0], "#C3");
        Assert.AreEqual ("B", listBox.SelectedItems [1], "#C4");
        Assert.AreEqual ("C", listBox.SelectedItems [2], "#C5");
        Assert.AreEqual ("D", listBox.SelectedItems [3], "#C6");

        // 
        // Second part: how does SelectedIndex setter work related
        // to SelectedItems
        //
        listBox.SelectedIndex = -1;
        Assert.AreEqual (-1, listBox.SelectedIndex, "#D1");
        Assert.AreEqual (0, listBox.SelectedItems.Count, "#D2");

        listBox.SelectedIndex = 3; // "D"
        Assert.AreEqual (3, listBox.SelectedIndex, "#E1");
        Assert.AreEqual (1, listBox.SelectedItems.Count, "#E2");
        Assert.AreEqual ("D", listBox.SelectedItems [0], "#E3");

        listBox.SelectedItems.Add ("B"); // index = 1
        Assert.AreEqual (1, listBox.SelectedIndex, "#F1");
        Assert.AreEqual (2, listBox.SelectedItems.Count, "#E3");
        Assert.AreEqual ("B", listBox.SelectedItems [0], "#E4");
        Assert.AreEqual ("D", listBox.SelectedItems [1], "#E5");

        listBox.SelectedIndex = 2;
        Assert.AreEqual (1, listBox.SelectedIndex, "#G1");
        Assert.AreEqual (3, listBox.SelectedItems.Count, "#G2");
        Assert.AreEqual ("B", listBox.SelectedItems [0], "#G3");
        Assert.AreEqual ("C", listBox.SelectedItems [1], "#G4");
        Assert.AreEqual ("D", listBox.SelectedItems [2], "#G5");

        listBox.SelectedIndex = 1; // already selected
        Assert.AreEqual (1, listBox.SelectedIndex, "#H1");
        Assert.AreEqual (3, listBox.SelectedItems.Count, "#H2");

        // NOTE: It seems that passing -1 does not affect the collection
        // in anyway (other wrong values generate an exception, however)
        listBox.SelectedIndices.Add (-1);
        Assert.AreEqual (3, listBox.SelectedItems.Count, "#J1");
    }

    [Test]
    public void Selection_One ()
    {
        listBox.Items.Add ("A");
        listBox.Items.Add ("B");
        listBox.Items.Add ("C");
        listBox.SelectionMode = SelectionMode.One;

        listBox.SelectedItems.Add ("B");
        Assert.AreEqual (1, listBox.SelectedIndex, "#A1");
        Assert.AreEqual (1, listBox.SelectedItems.Count, "#A2");
        Assert.AreEqual ("B", listBox.SelectedItems [0], "#A3");

        listBox.SelectedIndex = 2;
        Assert.AreEqual (2, listBox.SelectedIndex, "#B1");
        Assert.AreEqual (1, listBox.SelectedItems.Count, "#B2");
        Assert.AreEqual ("C", listBox.SelectedItems [0], "#B3");

        listBox.SelectedItems.Add ("A");
        Assert.AreEqual (0, listBox.SelectedIndex, "#C1");
        Assert.AreEqual (1, listBox.SelectedItems.Count, "#C2");
        Assert.AreEqual ("A", listBox.SelectedItems [0], "#C3");
    }

    [Test]
    public void Selection_None ()
    {
        listBox.Items.Add ("A");
        listBox.Items.Add ("B");
        listBox.SelectionMode = SelectionMode.None;

        try {
            listBox.SelectedIndex = 0;
            Assert.Fail ("#A");
        } catch (ArgumentException) {
        }

        try {
            listBox.SelectedIndices.Add (0);
            Assert.Fail ("#B");
        } catch (InvalidOperationException e) {
        }

        try {
            listBox.SelectedItems.Add ("A");
            Assert.Fail ("#C");
        } catch (ArgumentException) {
        }
    }

    [Test]
    public void SelectedIndexException ()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            listBox.SelectedIndex = -2;
        });
    }

    [Test]
    public void SelectedIndexException2 ()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            listBox.SelectedIndex = listBox.Items.Count;
        });
    }

    [Test]
    public void SelectedIndexModeNoneException ()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            listBox.SelectionMode = SelectionMode.None;
            listBox.SelectedIndex = -1;
        });
    }

    [Test]
    public void SelectionModeException ()
    {
        Assert.Throws<InvalidEnumArgumentException>(() =>
        {
            listBox.SelectionMode = (SelectionMode)10;
        });
    }

    [Test]
    public void SelectedValueNull()
    {
        listBox.Items.Clear ();

        listBox.Items.Add ("A");
        listBox.Items.Add ("B");
        listBox.Items.Add ("C");
        listBox.Items.Add ("D");

        listBox.SelectedIndex = 2;
        listBox.SelectedValue = null;
        Assert.AreEqual (listBox.SelectedIndex, 2);
    }

    [Test]
    public void SelectedValueEmptyString()
    {
        listBox.Items.Clear ();

        listBox.Items.Add ("A");
        listBox.Items.Add ("B");
        listBox.Items.Add ("C");
        listBox.Items.Add ("D");

        listBox.SelectedIndex = 2;
        listBox.SelectedValue = null;
        Assert.AreEqual (listBox.SelectedIndex, 2);
    }

    [Test]	// Bug #80466
    public void ListBoxHeight ()
    {
        var l = new ListBox ();
			
        for (var h = 0; h < 100; h++) {
            l.Height = h;
				
            if (l.Height != h)
                Assert.Fail ("Set ListBox height of {0}, got back {1}.  Should be the same.", h, l.Height);
        }
    }

    [Test]
    public void HeightAndIntegralHeight ()
    {
        var a = new ListBox();
        var defaultSize = new Size(120, 96);
        Assert.AreEqual (defaultSize, a.Size, "A1");
        a.CreateControl();
        Assert.AreEqual (0, (a.ClientSize.Height % a.ItemHeight), "A2");
        a.IntegralHeight = false;
        Assert.AreEqual (a.Size, defaultSize, "A3");
        a.IntegralHeight = true;
        Assert.AreEqual (0, (a.ClientSize.Height % a.ItemHeight), "A4");

        var clientSizeI = new Size(200, a.ItemHeight * 5);
        var clientSize = clientSizeI + new Size(0, a.ItemHeight / 2);
        var borderSize = new Size(a.Width - a.ClientSize.Width, a.Height - a.ClientSize.Height);
        var totalSizeI = clientSizeI + borderSize;
        var totalSize = clientSize + borderSize;

        a = new ListBox();
        a.ClientSize = clientSize;
        Assert.AreEqual (clientSize, a.ClientSize, "A5");
        Assert.AreEqual (totalSize, a.Size, "A6");
        a.IntegralHeight = false;
        a.IntegralHeight = true;
        Assert.AreEqual (clientSize, a.ClientSize, "A7");
        a.CreateControl();
        Assert.AreEqual (clientSizeI, a.ClientSize, "A8");
        Assert.AreEqual (totalSizeI, a.Size, "A9");
        a.IntegralHeight = false;
        Assert.AreEqual (clientSize, a.ClientSize, "A10");
        a.IntegralHeight = true;
        Assert.AreEqual (totalSizeI, a.Size, "A11");

        a = new ListBox();
        a.CreateControl();
        a.Size = totalSize;
        Assert.AreEqual (totalSizeI, a.Size, "A12");
        Assert.AreEqual (clientSizeI, a.ClientSize, "A13");
        a.IntegralHeight = false;
        Assert.AreEqual (totalSize, a.Size, "A14");
        Assert.AreEqual (clientSize, a.ClientSize, "A15");

        a = new ListBox();
        a.IntegralHeight = false;
        Assert.AreEqual (defaultSize, a.Size, "A16");
        a.CreateControl();
        Assert.AreEqual (defaultSize, a.Size, "A17");

        a = new ListBox();
        a.ClientSize = clientSize;
        a.IntegralHeight = false;
        Assert.AreEqual (clientSize, a.ClientSize, "A18");
        a.CreateControl();
        Assert.AreEqual (clientSize, a.ClientSize, "A19");
    }

    [Test]
    public void PropertyTopIndex ()
    {
        var f = new Form ();
        f.ShowInTaskbar = false;
        f.Show ();
			
        var l = new ListBox ();
        l.Height = 100;
        f.Controls.Add (l);
			
        l.Items.AddRange (new string[] { "A", "B", "C"});
			
        Assert.AreEqual (0, l.TopIndex, "A1");
			
        l.TopIndex = 2;
        Assert.AreEqual (0, l.TopIndex, "A2");

        l.Items.AddRange (new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M" });
        Assert.AreEqual (0, l.TopIndex, "A3");

        l.TopIndex = 2;
        Assert.AreEqual (2, l.TopIndex, "A4");

        // There aren't items enough for 12 to be the top index, but
        // the actual value is font height dependent.
        l.TopIndex = 12;
        Assert.IsTrue (l.TopIndex < 12, "A5");
			
        f.Close ();
        f.Dispose ();
    }
		
    //
    // Events
    //
    //private bool eventFired;

    //private void GenericHandler (object sender,  EventArgs e)
    //{
    //        eventFired = true;
    //}

    [Test]
    public void SelectedIndexUpdated () // Xamarin bug 4921
    {
        using var f = new Form ();
        f.ShowInTaskbar = false;

        var l = new ListBox ();
        l.Sorted = true;
        f.Controls.Add (l);

        l.Items.Add ("B");
        l.SelectedIndex = 0;

        Assert.AreEqual (0, l.SelectedIndex);

        l.Items.Add ("A");
        Assert.AreEqual (1, l.SelectedIndex);
    }

    [Test]
    public void SelectedIndexUpdated_MultiSelect () // Xamarin bug 4921
    {
        using var f = new Form ();
        f.ShowInTaskbar = false;

        var l = new ListBox ();
        l.Sorted = true;
        l.SelectionMode = SelectionMode.MultiSimple;
        f.Controls.Add (l);

        l.Items.Add ("B");
        l.Items.Add ("C");
        l.SelectedIndex = 0;
        l.SelectedIndex = 1;

        Assert.AreEqual (2, l.SelectedIndices.Count);
        Assert.AreEqual (0, l.SelectedIndices [0]);
        Assert.AreEqual (1, l.SelectedIndices [1]);
        Assert.AreEqual (2, l.SelectedItems.Count);
        Assert.AreEqual ("B", l.SelectedItems [0]);
        Assert.AreEqual ("C", l.SelectedItems [1]);

        l.Items.Add ("A");
        Assert.AreEqual (2, l.SelectedIndices.Count);
        Assert.AreEqual (1, l.SelectedIndices[0]);
        Assert.AreEqual (2, l.SelectedIndices[1]);
        Assert.AreEqual (2, l.SelectedItems.Count);
        Assert.AreEqual ("B", l.SelectedItems [0]);
        Assert.AreEqual ("C", l.SelectedItems [1]);
    }
}

[TestFixture]
public class ListBoxObjectCollectionTest : TestHelper
{
    ListBox.ObjectCollection col;

    [SetUp]
    protected override void SetUp ()
    {
        col = new ListBox.ObjectCollection (new ListBox ());
    }

    [Test]
    public void DefaultProperties ()
    {
        Assert.AreEqual (false, col.IsReadOnly, "#B1");
        Assert.AreEqual (false, ((ICollection)col).IsSynchronized, "#B2");
        Assert.AreEqual (col, ((ICollection)col).SyncRoot, "#B3");
        Assert.AreEqual (false, ((IList)col).IsFixedSize, "#B4");
        Assert.AreEqual (0, col.Count);
    }

    [Test]
    public void Add ()
    {
        col.Add ("Item1");
        col.Add ("Item2");
        Assert.AreEqual (2, col.Count, "#1");
        Assert.AreEqual ("Item1", col [0], "#2");
        Assert.AreEqual ("Item2", col [1], "#3");
    }

    [Test]
    public void Add_Item_Null ()
    {
        try {
            col.Add (null);
            Assert.Fail ("#1");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("item", ex.ParamName, "#5");
        }
    }

    [Test] // AddRange (Object [])
    public void AddRange1_Items_Null ()
    {
        try {
            col.AddRange ((object []) null);
            Assert.Fail ("#1");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("items", ex.ParamName, "#5");
        }
    }

    [Test] // AddRange (ListBox.ObjectCollection)
    public void AddRange2_Value_Null ()
    {
        try {
            col.AddRange ((ListBox.ObjectCollection) null);
            Assert.Fail ("#1");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("items", ex.ParamName, "#5");
        }
    }

    [Test]
    public void Clear ()
    {
        col.Add ("Item1");
        col.Add ("Item2");
        col.Clear ();
        Assert.AreEqual (0, col.Count, "#D1");
    }

    [Test]
    public void Contains ()
    {
        object obj = "Item1";
        col.Add (obj);
        Assert.IsTrue (col.Contains ("Item1"), "#1");
        Assert.IsFalse (col.Contains ("Item2"), "#2");
    }

    [Test]
    public void Contains_Value_Null ()
    {
        try {
            col.Contains (null);
            Assert.Fail ("#1");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("value", ex.ParamName, "#5");
        }
    }

    [Test]
    public void Indexer_Value_Null ()
    {
        col.Add ("Item1");
        try {
            col [0] = null;
            Assert.Fail ("#1");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("value", ex.ParamName, "#5");
        }
    }

    [Test]
    public void IndexOf ()
    {
        col.Add ("Item1");
        col.Add ("Item2");
        Assert.AreEqual (1, col.IndexOf ("Item2"), "#1");
        Assert.AreEqual (0, col.IndexOf ("Item1"), "#2");
        Assert.AreEqual (-1, col.IndexOf ("Item3"), "#3");
    }

    [Test]
    public void IndexOf_Value_Null ()
    {
        try {
            col.IndexOf (null);
            Assert.Fail ("#1");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("value", ex.ParamName, "#5");
        }
    }

    [Test]
    [NUnit.Framework.Category ("NotDotNet")] // https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=363285
    public void Insert_Item_Null ()
    {
        col.Add ("Item1");
        try {
            col.Insert (0, null);
            Assert.Fail ("#1");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("item", ex.ParamName, "#5");
        }
    }

    [Test]
    public void Remove ()
    {
        col.Add ("Item1");
        col.Add ("Item2");
        col.Remove ("Item1");
        Assert.AreEqual (1, col.Count, "#A1");
        Assert.AreEqual (-1, col.IndexOf ("Item1"), "#A2");
        Assert.AreEqual (0, col.IndexOf ("Item2"), "#A3");
        col.Remove (null);
        Assert.AreEqual (1, col.Count, "#B1");
        Assert.AreEqual (-1, col.IndexOf ("Item1"), "#B2");
        Assert.AreEqual (0, col.IndexOf ("Item2"), "#B3");
        col.Remove ("Item3");
        Assert.AreEqual (1, col.Count, "#C1");
        Assert.AreEqual (-1, col.IndexOf ("Item1"), "#C2");
        Assert.AreEqual (0, col.IndexOf ("Item2"), "#C3");
        col.Remove ("Item2");
        Assert.AreEqual (0, col.Count, "#D1");
        Assert.AreEqual (-1, col.IndexOf ("Item1"), "#D2");
        Assert.AreEqual (-1, col.IndexOf ("Item2"), "#D3");
    }

    [Test]
    public void RemoveAt ()
    {
        col.Add ("Item1");
        col.Add ("Item2");
        col.RemoveAt (0);
        Assert.AreEqual (1, col.Count, "#1");
        Assert.AreEqual (-1, col.IndexOf ("Item1"), "#2");
        Assert.AreEqual (0, col.IndexOf ("Item2"), "#3");
    }
}

[TestFixture]
public class ListBoxIntegerCollectionTest : TestHelper
{
    ListBox.IntegerCollection col;
    ListBox listBox;

    [TearDown]
    public void TearDown()
    {
        listBox.Dispose();
    }

    [SetUp]
    protected override void SetUp ()
    {
        listBox = new ListBox ();
        col = new ListBox.IntegerCollection (listBox);
    }

    [Test]
    public void Add ()
    {
        col.Add (5);
        Assert.AreEqual (1, col.Count, "#1");
        col.Add (7);
        Assert.AreEqual (2, col.Count, "#2");
        col.Add (5);
        Assert.AreEqual (2, col.Count, "#3");
        col.Add (3);
        Assert.AreEqual (3, col.Count, "#4");
    }

    [Test] // AddRange (Int32 [])
    public void AddRange1 ()
    {
        col.Add (5);
        col.Add (3);
        col.AddRange (new int [] { 3, 7, 9, 5, 4 });
        Assert.AreEqual (5, col.Count, "#1");
        Assert.AreEqual (3, col [0], "#2");
        Assert.AreEqual (4, col [1], "#3");
        Assert.AreEqual (5, col [2], "#4");
        Assert.AreEqual (7, col [3], "#5");
        Assert.AreEqual (9, col [4], "#6");
    }

    [Test] // AddRange (Int32 [])
    public void AddRange1_Items_Null ()
    {
        try {
            col.AddRange ((int []) null);
            Assert.Fail ("#1");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("items", ex.ParamName, "#5");
        }
    }

    [Test] // AddRange (ListBox.IntegerCollection)
    public void AddRange2 ()
    {
        var ints = new ListBox.IntegerCollection (
            listBox);
        ints.Add (3);
        ints.Add (1);
        ints.Add (-5);
        ints.Add (4);
        ints.Add (2);

        col.Add (5);
        col.Add (3);
        col.Add (12);
        col.AddRange (ints);

        Assert.AreEqual (7, col.Count, "#1");
        Assert.AreEqual (-5, col [0], "#2");
        Assert.AreEqual (1, col [1], "#3");
        Assert.AreEqual (2, col [2], "#4");
        Assert.AreEqual (3, col [3], "#5");
        Assert.AreEqual (4, col [4], "#6");
        Assert.AreEqual (5, col [5], "#7");
    }

    [Test] // AddRange (ListBox.IntegerCollection)
    public void AddRange2_Items_Null ()
    {
        try {
            col.AddRange ((ListBox.IntegerCollection) null);
            Assert.Fail ("#1");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("items", ex.ParamName, "#5");
        }
    }

    [Test]
    [NUnit.Framework.Category ("NotDotNet")] // https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=363278
    public void Clear ()
    {
        col.Add (5);
        col.Add (3);
        col.Clear ();

        Assert.AreEqual (0, col.Count, "#1");
        Assert.AreEqual (-1, col.IndexOf (5), "#2");
        Assert.AreEqual (-1, col.IndexOf (3), "#3");
    }

    [Test]
    public void Contains ()
    {
        col.Add (5);
        col.Add (7);
        Assert.IsTrue (col.Contains (5), "#1");
        Assert.IsFalse (col.Contains (3), "#2");
        Assert.IsTrue (col.Contains (7), "#3");
        Assert.IsFalse (col.Contains (-5), "#4");
    }

    [Test]
    public void CopyTo ()
    {
        var copy = new int [5] { 9, 4, 6, 2, 8 };
			
        col.Add (3);
        col.Add (7);
        col.Add (5);
        col.CopyTo (copy, 1);

        Assert.AreEqual (9, copy [0], "#1");
        Assert.AreEqual (3, copy [1], "#2");
        Assert.AreEqual (5, copy [2], "#3");
        Assert.AreEqual (7, copy [3], "#4");
        Assert.AreEqual (8, copy [4], "#5");
    }

    [Test]
    public void CopyTo_Destination_Invalid ()
    {
        string [] copy = new string [3] { "A", "B", "C" };

        col.CopyTo (copy, 1);
        col.Add (3);

        try {
            col.CopyTo (copy, 1);
            Assert.Fail ("#1");
        } catch (InvalidCastException) {
        }
    }

    [Test]
    public void CopyTo_Destination_Null ()
    {
        col.CopyTo ((Array) null, 1);
        col.Add (3);

        try {
            col.CopyTo ((Array) null, 1);
            Assert.Fail ("#1");
        } catch (NullReferenceException) {
        }
    }

    [Test]
    public void CopyTo_Index_Negative ()
    {
        var copy = new int [5] { 9, 4, 6, 2, 8 };

        col.CopyTo (copy, -5);
        col.Add (3);

        try {
            col.CopyTo (copy, -5);
            Assert.Fail ("#1");
        } catch (IndexOutOfRangeException ex) {
            // Index was outside the bounds of the array
            Assert.AreEqual (typeof (IndexOutOfRangeException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
        }
    }

    [Test]
    public void Count ()
    {
        Assert.AreEqual (0, col.Count, "#1");
        col.Add (5);
        Assert.AreEqual (1, col.Count, "#2");
        col.Add (7);
        Assert.AreEqual (2, col.Count, "#3");
        col.Remove (7);
        Assert.AreEqual (1, col.Count, "#4");
    }

    [Test]
    public void Indexer ()
    {
        col.Add (5);
        col.Add (7);
        Assert.AreEqual (7, col [1], "#1");
        Assert.AreEqual (5, col [0], "#2");
        col [0] = 3;
        Assert.AreEqual (7, col [1], "#3");
        Assert.AreEqual (3, col [0], "#4");
    }

    [Test]
    public void IndexOf ()
    {
        col.Add (5);
        col.Add (7);
        Assert.AreEqual (0, col.IndexOf (5), "#1");
        Assert.AreEqual (-1, col.IndexOf (3), "#2");
        Assert.AreEqual (1, col.IndexOf (7), "#3");
        Assert.AreEqual (-1, col.IndexOf (-5), "#4");
    }

    [Test]
    public void Remove ()
    {
        col.Add (5);
        col.Add (3);
        col.Remove (5);
        col.Remove (7);

        Assert.AreEqual (1, col.Count, "#1");
        Assert.AreEqual (3, col [0], "#2");
        col.Remove (3);
        Assert.AreEqual (0, col.Count, "#3");
        col.Remove (3);
        // https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=363280
        //Assert.AreEqual (0, col.Count, "#4");
    }

    [Test]
    public void RemoveAt ()
    {
        col.Add (5);
        col.Add (3);
        col.Add (7);
        col.RemoveAt (1);
        Assert.AreEqual (2, col.Count, "#A1");
        Assert.AreEqual (3, col [0], "#A2");
        Assert.AreEqual (7, col [1], "#A3");
        col.RemoveAt (0);
        Assert.AreEqual (1, col.Count, "#B1");
        Assert.AreEqual (7, col [0], "#B2");
        col.RemoveAt (0);
        Assert.AreEqual (0, col.Count, "#C1");
        Assert.AreEqual (-1, col.IndexOf (5), "#C2");
        Assert.AreEqual (-1, col.IndexOf (3), "#C3");
        // https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=363280
        //Assert.AreEqual (-1, col.IndexOf (7), "#C4");
    }

    [Test]
    [NUnit.Framework.Category ("NotDotNet")] // https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=363276
    public void RemoveAt_Index_Negative ()
    {
        col.Add (5);

        try {
            col.RemoveAt (-1);
            Assert.Fail ("#1");
        } catch (IndexOutOfRangeException ex) {
            // Index was outside the bounds of the array
            Assert.AreEqual (typeof (IndexOutOfRangeException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
        }

        Assert.AreEqual (1, col.Count, "#5");
    }

    [Test]
    [NUnit.Framework.Category ("NotDotNet")] // https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=363276
    public void RemoveAt_Index_Overflow ()
    {
        col.Add (5);

        try {
            col.RemoveAt (1);
            Assert.Fail ("#1");
        } catch (ArgumentOutOfRangeException ex) {
            // Index was outside the bounds of the array
            Assert.AreEqual (typeof (ArgumentOutOfRangeException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("index", ex.ParamName, "#5");
        }

        Assert.AreEqual (1, col.Count, "#6");
    }

    [Test]
    public void ICollection_IsSynchronized ()
    {
        var collection = (ICollection) col;
        Assert.IsTrue (collection.IsSynchronized);
    }

    [Test]
    public void ICollection_SyncRoot ()
    {
        var collection = (ICollection) col;
        Assert.AreSame (collection, collection.SyncRoot);
    }

    [Test]
    public void IList_Add ()
    {
        var list = (IList) col;
        list.Add (5);
        Assert.AreEqual (1, list.Count, "#1");
        list.Add (7);
        Assert.AreEqual (2, list.Count, "#2");
        list.Add (5);
        Assert.AreEqual (2, list.Count, "#3");
        list.Add (3);
        Assert.AreEqual (3, list.Count, "#4");
    }

    [Test]
    public void IList_Add_Item_Invalid ()
    {
        var list = (IList) col;

        try {
            list.Add (null);
            Assert.Fail ("#A1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#A2");
            Assert.IsNull (ex.InnerException, "#A3");
            Assert.AreEqual ("item", ex.Message, "#A4");
            Assert.IsNull (ex.ParamName, "#A5");
        }

        try {
            list.Add ("x");
            Assert.Fail ("#B1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#B2");
            Assert.IsNull (ex.InnerException, "#B3");
            Assert.AreEqual ("item", ex.Message, "#B4");
            Assert.IsNull (ex.ParamName, "#B5");
        }
    }

    [Test]
    public void IList_Clear ()
    {
        var list = (IList) col;
        list.Add (5);
        list.Add (7);
        list.Clear ();
        Assert.AreEqual (0, list.Count);
    }

    [Test]
    public void IList_Contains ()
    {
        var list = (IList) col;
        list.Add (5);
        list.Add (7);
        Assert.IsTrue (list.Contains (5), "#1");
        Assert.IsFalse (list.Contains (3), "#2");
        Assert.IsTrue (list.Contains (7), "#3");
        Assert.IsFalse (list.Contains (null), "#4");
        Assert.IsFalse (list.Contains ("x"), "#5");
    }

    [Test]
    public void IList_Indexer ()
    {
        var list = (IList) col;
        list.Add (5);
        list.Add (7);
        Assert.AreEqual (7, list [1], "#1");
        Assert.AreEqual (5, list [0], "#2");
        list [0] = 3;
        Assert.AreEqual (7, list [1], "#3");
        Assert.AreEqual (3, list [0], "#4");
    }

    [Test]
    public void IList_IndexOf ()
    {
        var list = (IList) col;
        list.Add (5);
        list.Add (7);
        Assert.AreEqual (0, list.IndexOf (5), "#1");
        Assert.AreEqual (-1, list.IndexOf (3), "#2");
        Assert.AreEqual (1, list.IndexOf (7), "#3");
        Assert.AreEqual (-1, list.IndexOf (null), "#4");
        Assert.AreEqual (-1, list.IndexOf ("x"), "#5");
    }

    [Test]
    public void IList_Insert ()
    {
        var list = (IList) col;
        list.Add (5);

        try {
            list.Insert (0, 7);
            Assert.Fail ("#A1");
        } catch (NotSupportedException ex) {
            // ListBox.IntegerCollection is sorted, and
            // items cannot be inserted into it
            Assert.AreEqual (typeof (NotSupportedException), ex.GetType (), "#A2");
            Assert.IsNull (ex.InnerException, "#A3");
            Assert.IsNotNull (ex.Message, "#A4");
        }

        try {
            list.Insert (-5, null);
            Assert.Fail ("#B1");
        } catch (NotSupportedException ex) {
            // ListBox.IntegerCollection is sorted, and
            // items cannot be inserted into it
            Assert.AreEqual (typeof (NotSupportedException), ex.GetType (), "#B2");
            Assert.IsNull (ex.InnerException, "#B3");
            Assert.IsNotNull (ex.Message, "#B4");
        }
    }

    [Test]
    public void IList_IsFixedSize ()
    {
        var list = (IList) col;
        Assert.IsFalse (list.IsFixedSize);
    }

    [Test]
    public void IList_IsReadOnly ()
    {
        var list = (IList) col;
        Assert.IsFalse (list.IsReadOnly);
    }

    [Test]
    public void IList_Remove ()
    {
        var list = (IList) col;
        list.Add (5);
        list.Add (3);
        list.Remove (5);
        list.Remove (7);
        list.Remove (int.MinValue);
        list.Remove (int.MaxValue);

        Assert.AreEqual (1, list.Count, "#1");
        Assert.AreEqual (3, list [0], "#2");
        list.Remove (3);
        Assert.AreEqual (0, list.Count, "#3");
    }

    [Test]
    public void IList_Remove_Value_Invalid ()
    {
        var list = (IList) col;
        list.Add (5);

        try {
            list.Remove ("x");
            Assert.Fail ("#A1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#A2");
            Assert.IsNull (ex.InnerException, "#A3");
            Assert.AreEqual ("value", ex.Message, "#A4");
            Assert.IsNull (ex.ParamName, "#A5");
        }

        try {
            list.Remove (null);
            Assert.Fail ("#B1");
        } catch (ArgumentException ex) {
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#B2");
            Assert.IsNull (ex.InnerException, "#B3");
            Assert.AreEqual ("value", ex.Message, "#B4");
            Assert.IsNull (ex.ParamName, "#B5");
        }
    }

    [Test]
    public void IList_RemoveAt ()
    {
        var list = (IList) col;
        list.Add (5);
        list.Add (3);
        list.Add (7);
        list.RemoveAt (1);
        Assert.AreEqual (2, list.Count, "#A1");
        Assert.AreEqual (3, list [0], "#A2");
        Assert.AreEqual (7, list [1], "#A3");
        list.RemoveAt (0);
        Assert.AreEqual (1, list.Count, "#B1");
        Assert.AreEqual (7, list [0], "#B2");
        list.RemoveAt (0);
        Assert.AreEqual (0, list.Count, "#C");
    }

    [Test]
    public void IList_RemoveAt_Index_Negative ()
    {
        var list = (IList) col;
        list.Add (5);

        try {
            list.RemoveAt (-1);
            Assert.Fail ("#1");
        } catch (IndexOutOfRangeException ex) {
            // Index was outside the bounds of the array
            Assert.AreEqual (typeof (IndexOutOfRangeException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
        }

        // // https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=363276
        //Assert.AreEqual (1, list.Count, "#5");
    }

    [Test]
    [NUnit.Framework.Category ("NotDotNet")] // https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=363276
    public void IList_RemoveAt_Index_Overflow ()
    {
        var list = (IList) col;
        list.Add (5);

        try {
            list.RemoveAt (1);
            Assert.Fail ("#1");
        } catch (ArgumentOutOfRangeException ex) {
            // Index was outside the bounds of the array
            Assert.AreEqual (typeof (ArgumentOutOfRangeException), ex.GetType (), "#2");
            Assert.IsNull (ex.InnerException, "#3");
            Assert.IsNotNull (ex.Message, "#4");
            Assert.AreEqual ("index", ex.ParamName, "#5");
        }

        Assert.AreEqual (1, list.Count, "#6");
    }
}