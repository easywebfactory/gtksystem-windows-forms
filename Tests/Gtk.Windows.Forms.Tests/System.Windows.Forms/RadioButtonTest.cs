//
// RadioRadioButtonTest.cs: Test cases for RadioRadioButton.
//
// Author:
//   Ritvik Mayank (mritvik@novell.com)
//
// (C) 2005 Novell, Inc. (http://www.novell.com)
//

using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class RadioButtonTest : TestHelper
{
    [Test]
    public void RadioButtonPropertyTest ()
    {
        var rButton1 = new RadioButton ();
			
        // S
        Assert.AreEqual (null, rButton1.Site, "#S1");	

        // T
        rButton1.Text = "New RadioButton";
        Assert.AreEqual ("New RadioButton", rButton1.Text, "#T1");
        Assert.IsFalse (rButton1.TabStop, "#T3");
    }

    [Test]
    public void CheckedTest ()
    {
        var rb = new RadioButton ();

        Assert.AreEqual (false, rb.TabStop, "#A1");
        Assert.AreEqual (false, rb.Checked, "#A2");

        rb.Checked = true;

        Assert.AreEqual (true, rb.TabStop, "#B1");
        Assert.AreEqual (true, rb.Checked, "#B2");

        rb.Checked = false;

        Assert.AreEqual (false, rb.TabStop, "#C1");
        Assert.AreEqual (false, rb.Checked, "#C2");

        // RadioButton is NOT checked, but since it is the only
        // RadioButton instance in Form, when it gets selected (Form.Show)
        // it should acquire the focus
        var f = new Form ();
        f.Controls.Add (rb);
        rb.CheckedChanged += new EventHandler (rb_checked_changed);
        event_received = false;

        f.ActiveControl = rb;

        Assert.AreEqual (true, event_received, "#D1");
        Assert.AreEqual (true, rb.Checked, "#D2");
        Assert.AreEqual (true, rb.TabStop, "#D3");

        f.Dispose ();
    }

    bool event_received = false;
    void rb_tabstop_changed (object sender, EventArgs e)
    {
        event_received = true;
    }

    void rb_checked_changed (object sender, EventArgs e)
    {
        event_received = true;
    }

    [Test]
    public void TabStopEventTest ()
    {
        var rb = new RadioButton ();

        rb.TabStopChanged += new EventHandler (rb_tabstop_changed);
        event_received = false;

        rb.TabStop = true;

        Assert.IsTrue (event_received);
    }

    [Test]
    public void ToStringTest ()
    {
        var rButton1 = new RadioButton ();
        Assert.AreEqual ("System.Windows.Forms.RadioButton, Checked: False" , rButton1.ToString (), "#9");
    }

    [Test]
    public void AutoSizeText ()
    {
        var f = new Form ();
        f.ShowInTaskbar = false;
			
        var rb = new RadioButton ();
        rb.AutoSize = true;
        rb.Width = 14;
        f.Controls.Add (rb);
			
        var width = rb.Width;
			
        rb.Text = "Some text that is surely longer than 100 pixels.";

        if (rb.Width == width)
            Assert.Fail ("RadioButton did not autosize, actual: {0}", rb.Width);
    }
}
	
[TestFixture]
public class RadioButtonEventTestClass : TestHelper
{
    static bool eventhandled = false;
    public static void RadioButton_EventHandler (object sender, EventArgs e)
    {
        eventhandled = true;
    }

    [Test]
    public void ApperanceChangedTest ()
    {
        var myForm = new Form ();
        myForm.ShowInTaskbar = false;
        var rButton1 = new RadioButton ();
        rButton1.Select ();
        rButton1.Visible = true;
        myForm.Controls.Add (rButton1);
        eventhandled = false;
        Assert.AreEqual (true, eventhandled, "#2");
        myForm.Dispose ();
    }
	
    [Test]
    public void CheckedChangedTest ()
    {
        var myForm = new Form ();
        myForm.ShowInTaskbar = false;
        var rButton1 = new RadioButton ();
        rButton1.Select ();
        rButton1.Visible = true;
        myForm.Controls.Add (rButton1);
        rButton1.Checked = false;
        eventhandled = false;
        rButton1.CheckedChanged += new EventHandler (RadioButton_EventHandler);
        rButton1.Checked = true;
        Assert.AreEqual (true, eventhandled, "#3");
        myForm.Dispose ();
    }
}