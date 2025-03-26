//
// ToolStripProgressBarTests.cs
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

using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class ToolStripProgressBarTests : TestHelper
{
    [Test]
    public void Constructor ()
    {
        var tsi = new ToolStripProgressBar ();

        Assert.AreEqual (100, tsi.MarqueeAnimationSpeed, "A1");
        Assert.AreEqual (100, tsi.Maximum, "A2");
        Assert.AreEqual (0, tsi.Minimum, "A3");
        Assert.AreEqual ("System.Windows.Forms.ProgressBar", tsi.ProgressBar.GetType ().ToString (), "A4");
        Assert.AreEqual (10, tsi.Step, "A6");
        Assert.AreEqual (ProgressBarStyle.Blocks, tsi.Style, "A7");
        Assert.AreEqual (string.Empty, tsi.Text, "A8");
        Assert.AreEqual (0, tsi.Value, "A9");

        tsi = new ToolStripProgressBar ("Bob");
        Assert.AreEqual ("Bob", tsi.Name, "A10");
    }
	
    [Test]
    public void PropertyMarqueeAnimationSpeed ()
    {
        var tsi = new ToolStripProgressBar ();
        var ew = new EventWatcher (tsi);

        tsi.MarqueeAnimationSpeed = 200;
        Assert.AreEqual (200, tsi.MarqueeAnimationSpeed, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.MarqueeAnimationSpeed = 200;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyMaximum ()
    {
        var tsi = new ToolStripProgressBar ();
        var ew = new EventWatcher (tsi);

        tsi.Maximum = 200;
        Assert.AreEqual (200, tsi.Maximum, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Maximum = 200;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyMinimum ()
    {
        var tsi = new ToolStripProgressBar ();
        var ew = new EventWatcher (tsi);

        tsi.Minimum = 200;
        Assert.AreEqual (200, tsi.Minimum, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Minimum = 200;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyStep ()
    {
        var tsi = new ToolStripProgressBar ();
        var ew = new EventWatcher (tsi);

        tsi.Step = 200;
        Assert.AreEqual (200, tsi.Step, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Step = 200;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyStyle ()
    {
        var tsi = new ToolStripProgressBar ();
        var ew = new EventWatcher (tsi);

        tsi.Style = ProgressBarStyle.Continuous;
        Assert.AreEqual (ProgressBarStyle.Continuous, tsi.Style, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Style = ProgressBarStyle.Continuous;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyText ()
    {
        var tsi = new ToolStripProgressBar ();
        var ew = new EventWatcher (tsi);

        tsi.Text = "Hi";
        Assert.AreEqual ("Hi", tsi.Text, "B1");
        Assert.AreEqual ("Hi", tsi.ProgressBar.Text, "B2");
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");

        ew.Clear ();
        tsi.Text = "Hi";
        Assert.AreEqual (string.Empty, ew.ToString (), "B4");
    }

    [Test]
    public void PropertyValue ()
    {
        var tsi = new ToolStripProgressBar ();
        var ew = new EventWatcher (tsi);

        tsi.Value = 30;
        Assert.AreEqual (30, tsi.Value, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Value = 30;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyValueAOORE ()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var tsi = new ToolStripProgressBar();

            tsi.Value = 200;
        });
    }

    [Test]
    public void BehaviorIncrement ()
    {
        var tsi = new ToolStripProgressBar ();
			
        tsi.Increment (14);
        Assert.AreEqual (14, tsi.Value, "B1");

        tsi.Increment (104);
        Assert.AreEqual (100, tsi.Value, "B2");

        tsi.Increment (-245);
        Assert.AreEqual (0, tsi.Value, "B3");
    }

    [Test]
    public void BehaviorPerformStep ()
    {
        var tsi = new ToolStripProgressBar ();

        tsi.PerformStep ();
        Assert.AreEqual (10, tsi.Value, "B1");
    }

    private class EventWatcher
    {
        private string events = string.Empty;
			
        public EventWatcher (ToolStripProgressBar tsi)
        {
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
}