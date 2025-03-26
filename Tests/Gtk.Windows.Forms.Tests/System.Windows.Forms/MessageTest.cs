//
// MessageTest.cs: Test cases for Message
//
// Authors:
//   Rolf Bjarne Kvinge (RKvinge@novell.com)
//
// (C) 2006 Novell, Inc. (http://www.novell.com)
//

using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class MessageTest : TestHelper
{
    [Test]
    public void ToStringTest ()
    {
        var msg = Message.Create (new IntPtr (123), 2, new IntPtr (234), new IntPtr (345));
        Assert.AreEqual ("msg=0x2 (WM_DESTROY) hwnd=0x7b wparam=0xea lparam=0x159 result=0x0", msg.ToString ());
        msg.Result = new IntPtr (2);
        Assert.AreEqual ("msg=0x2 (WM_DESTROY) hwnd=0x7b wparam=0xea lparam=0x159 result=0x2", msg.ToString ());
    }

    [Test]
    public void Equality ()
    {
        var msg1 = Message.Create (new IntPtr (1), 2, new IntPtr (3), new IntPtr (4));
        msg1.Result = new IntPtr (1);
        var msg2 = Message.Create (new IntPtr (1), 3, new IntPtr (4), new IntPtr (5));
        msg2.Result = new IntPtr (2);
        var msg3 = Message.Create (new IntPtr (1), 2, new IntPtr (4), new IntPtr (5));
        msg3.Result = new IntPtr (3);
        var msg4 = Message.Create (new IntPtr (1), 2, new IntPtr (3), new IntPtr (4));
        msg4.Result = new IntPtr (4);
        var msg5 = Message.Create (new IntPtr (1), 2, new IntPtr (3), new IntPtr (4));
        msg5.Result = new IntPtr (1);

        Assert.IsFalse (msg1 == msg2, "A1");
        Assert.IsFalse (msg1 == msg3, "A2");
        Assert.IsFalse (msg1 == msg4, "A3");
        Assert.IsTrue (msg1 == msg5, "A4");
    }

    [Test]
    public void Inequality ()
    {
        var msg1 = Message.Create (new IntPtr (1), 2, new IntPtr (3), new IntPtr (4));
        msg1.Result = new IntPtr (1);
        var msg2 = Message.Create (new IntPtr (1), 3, new IntPtr (4), new IntPtr (5));
        msg2.Result = new IntPtr (2);
        var msg3 = Message.Create (new IntPtr (1), 2, new IntPtr (4), new IntPtr (5));
        msg3.Result = new IntPtr (3);
        var msg4 = Message.Create (new IntPtr (1), 2, new IntPtr (3), new IntPtr (4));
        msg4.Result = new IntPtr (4);
        var msg5 = Message.Create (new IntPtr (1), 2, new IntPtr (3), new IntPtr (4));
        msg5.Result = new IntPtr (1);

        Assert.IsTrue (msg1 != msg2, "A1");
        Assert.IsTrue (msg1 != msg3, "A2");
        Assert.IsTrue (msg1 != msg4, "A3");
        Assert.IsFalse (msg1 != msg5, "A4");
    }
}