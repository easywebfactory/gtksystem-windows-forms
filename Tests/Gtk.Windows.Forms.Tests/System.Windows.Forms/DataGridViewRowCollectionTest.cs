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
// Copyright (c) 2007 Novell, Inc. (http://www.novell.com)
//
// Author:
//	Rolf Bjarne Kvinge  (RKvinge@novell.com)
//

using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class DataGridViewRowCollectionTest : TestHelper
{
    private DataGridView CreateAndFill ()
    {
        var dgv = DataGridViewCommon.CreateAndFill ();
        var row = new DataGridViewRow ();
        row.Cells.Add (new DataGridViewComboBoxCell ());
        row.Cells.Add (new DataGridViewComboBoxCell ());
        dgv.Rows.Add (row);
        return dgv;
    }

    [Test]
    public void ToStringTest ()
    {
        Assert.AreEqual ("System.Windows.Forms.DataGridViewRowCollection", new DataGridViewRowCollection (null).ToString (), "A");

        using var dgv = CreateAndFill ();
        Assert.AreEqual ("System.Windows.Forms.DataGridViewRowCollection", dgv.Rows.ToString (), "B");
    }
		
    [Test]
    public void CtorTest ()
    {
        DataGridViewRowCollection rc;
			
        rc = new DataGridViewRowCollection (null);
        Assert.AreEqual (0, rc.Count, "#01");

        using var dgv = new DataGridView ();
        rc = new DataGridViewRowCollection (dgv);
        Assert.AreEqual (0, rc.Count, "#02");
        Assert.IsTrue (rc != dgv.Rows, "#03");
    }
		
    [Test]
    [Category ("NotWorking")]	// Don't currently support shared rows
    public void AddTest ()
    {
        DataGridViewRow row;
        DataGridViewCell cell;
			
        using (var dgv = new DataGridView ()) {
            dgv.Columns.Add ("a", "A");
            row = new DataGridViewRow ();
            dgv.Rows.Add (row);
            Assert.AreEqual (-1, row.Index, "#01");
        }

        using (var dgv = new DataGridView ()) {
            dgv.Columns.Add ("a", "A");
            row = new DataGridViewRow ();
            cell = new DataGridViewTextBoxCell ();
            cell.Value = "abc";
            row.Cells.Add (cell);
            dgv.Rows.Add (row);
            Assert.AreEqual (0, row.Index, "#02");
        }
    }

    [Test]
    public void RemoveAtNewRowException ()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var dgv = new DataGridView();
            dgv.Columns.Add("A", "A");
            dgv.Rows.RemoveAt(0);
        });
    }

    [Test]
    public void RemoveNewRowException ()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var dgv = new DataGridView();
            dgv.Columns.Add("A", "A");
            dgv.Rows.Remove(dgv.Rows[0]);
        });
    }

    [Test]  // bug #448005
    public void ClearRows ()
    {
        var dgv = new DataGridView ();
        dgv.Columns.Add ("A", "A");
        dgv.Columns.Add ("A2", "A2");

        dgv.Rows.Add (1, 2);
        dgv.Rows.Add (1, 2);
        dgv.Rows.Add (1, 2);
        dgv.Rows.Add (1, 2);
        dgv.Rows.Add (1, 2);

        dgv.Rows.Clear ();

        Assert.AreEqual (1, dgv.Rows.Count, "A1");
    }
}