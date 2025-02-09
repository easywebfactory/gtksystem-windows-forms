//
// ToolStripButtonTests.cs
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
// Copyright (c) 2006 Jonathan Pobst
//
// Authors:
//	Jonathan Pobst (monkey@jpobst.com)
//

using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class ToolStripButtonTests : TestHelper
{
    [Test]
    public void Constructor ()
    {
        ToolStripButton tsi = new ToolStripButton ();

        Assert.AreEqual (true, tsi.AutoToolTip, "A1");
        Assert.AreEqual (false, tsi.Checked, "A3");
        Assert.AreEqual (false, tsi.CheckOnClick, "A4");
        Assert.AreEqual (CheckState.Unchecked, tsi.CheckState, "A5");

        int count = 0;
        EventHandler oc = new EventHandler (delegate (object sender, EventArgs e) { count++; });
        Image i = new Bitmap (1,1);
			
    }

    [Test]
    public void ProtectedProperties ()
    {
        ExposeProtectedProperties epp = new ExposeProtectedProperties ();

        Assert.AreEqual (true, epp.DefaultAutoToolTip, "C1");
    }

    [Test]
    public void PropertyAutoToolTip ()
    {
        ToolStripButton tsi = new ToolStripButton ();
        EventWatcher ew = new EventWatcher (tsi);

        tsi.AutoToolTip = true;
        Assert.AreEqual (true, tsi.AutoToolTip, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.AutoToolTip = true;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyChecked ()
    {
        ToolStripButton tsi = new ToolStripButton ();
        EventWatcher ew = new EventWatcher (tsi);

        tsi.Checked = true;
        Assert.AreEqual (true, tsi.Checked, "B1");
        Assert.AreEqual ("CheckedChanged;CheckStateChanged", ew.ToString (), "B2");

        ew.Clear ();
        tsi.Checked = true;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyCheckOnClick ()
    {
        ToolStripButton tsi = new ToolStripButton ();
        EventWatcher ew = new EventWatcher (tsi);

        tsi.CheckOnClick = true;
        Assert.AreEqual (true, tsi.CheckOnClick, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.CheckOnClick = true;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyCheckState ()
    {
        ToolStripButton tsi = new ToolStripButton ();
        EventWatcher ew = new EventWatcher (tsi);

        tsi.CheckState = CheckState.Checked;
        Assert.AreEqual (CheckState.Checked, tsi.CheckState, "B1");
        Assert.AreEqual ("CheckedChanged;CheckStateChanged", ew.ToString (), "B2");

        ew.Clear ();
        tsi.CheckState = CheckState.Checked;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyCheckStateIEAE ()
    {
        Assert.Throws<InvalidEnumArgumentException>(() =>
        {
            ToolStripButton tsi = new ToolStripButton();
            tsi.CheckState = (CheckState)42;
        });
    }

    private class EventWatcher
    {
        private string events = string.Empty;
			
        public EventWatcher (ToolStripButton tsi)
        {
            tsi.CheckedChanged += new EventHandler (delegate (Object obj, EventArgs e) { events += ("CheckedChanged;"); });
            tsi.CheckStateChanged += new EventHandler (delegate (Object obj, EventArgs e) { events += ("CheckStateChanged;"); });
        }

        public override string ToString ()
        {
            return events.TrimEnd (';');
        }
			
        public void Clear ()
        {
            events = string.Empty;
        }
    }
		
    private class ExposeProtectedProperties : ToolStripButton
    {
        public new bool DefaultAutoToolTip { get { return base.DefaultAutoToolTip; } }
    }
}