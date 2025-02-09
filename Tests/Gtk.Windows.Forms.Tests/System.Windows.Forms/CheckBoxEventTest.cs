//
// Copyright (c) 2005 Novell, Inc.
//
// Authors:
//      Ritvik Mayank (mritvik@novell.com)
//

using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class CheckBoxEventTest : TestHelper
{
    static bool eventhandled = false;
    public void CheckBox_EventHandler (object sender,EventArgs e)
    {
        eventhandled = true;
    }		

    [Test]
    public void CheckedChangedEventTest ()
    {
        var myform = new Form ();
        myform.ShowInTaskbar = false;
        eventhandled = false;
        myform.Visible = true;
        var chkbox = new CheckBox ();
        chkbox.Visible = true;
        myform.Controls.Add (chkbox);
        chkbox.CheckedChanged += new EventHandler (CheckBox_EventHandler);
        chkbox.CheckState = CheckState.Indeterminate;
        Assert.AreEqual (true, eventhandled, "#A2");
        myform.Dispose ();
    }

    [Test]
    public void CheckStateChangedEventTest ()
    {
        var myform = new Form ();
        myform.ShowInTaskbar = false;
        eventhandled = false;
        myform.Visible = true;
        var chkbox = new CheckBox ();
        chkbox.Visible = true;
        myform.Controls.Add (chkbox);
        chkbox.CheckStateChanged += new EventHandler (CheckBox_EventHandler);
        chkbox.CheckState = CheckState.Checked;
        Assert.AreEqual (true, eventhandled, "#A3");
        myform.Dispose ();
    }
}