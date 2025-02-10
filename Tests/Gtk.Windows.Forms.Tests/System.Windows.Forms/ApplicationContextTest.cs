//
// ApplicationContextTest.cs
//
// Author:
//   Chris Toshok (toshok@ximian.com)
//
// (C) 2006 Novell, Inc. (http://www.novell.com)
//

using System.Windows.Forms;
using GtkTests.System.Windows.Forms;
using CategoryAttribute=NUnit.Framework.CategoryAttribute;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class ApplicationContextTest : TestHelper
{
    private ApplicationContext ctx;
    int thread_exit_count;
    bool reached_form_handle_destroyed;

    [TearDown]
    public void TearDown()
    {
        ctx?.Dispose();
    }

    void thread_exit (object sender, EventArgs e)
    {
        thread_exit_count++;
    }

    void form_handle_destroyed (object sender, EventArgs e)
    {
        Assert.AreEqual (0, thread_exit_count, "1");
        Assert.AreEqual (sender, ctx.MainForm, "2");
        reached_form_handle_destroyed = true;
    }

    void form_handle_destroyed2 (object sender, EventArgs e)
    {
        Assert.AreEqual (1, thread_exit_count, "1");
        Assert.AreEqual (sender, ctx.MainForm, "2");
        reached_form_handle_destroyed = true;
    }

    [Test]
    public void TestEventOrdering ()
    {
        thread_exit_count = 0;
        reached_form_handle_destroyed = false;

        var f1 = new Form ();
        f1.ShowInTaskbar = false;
        f1.HandleDestroyed += form_handle_destroyed;

        ctx = new ApplicationContext (f1);
        ctx.ThreadExit += thread_exit;

        f1.Show ();
        f1.Dispose();

        Assert.AreEqual (true, reached_form_handle_destroyed, "3");
        Assert.AreEqual (1, thread_exit_count, "4");

        f1.Dispose ();
    }

    [Test]
    public void TestEventOrdering2 ()
    {
        thread_exit_count = 0;
        reached_form_handle_destroyed = false;

        var f1 = new Form ();
        f1.ShowInTaskbar = false;

        ctx = new ApplicationContext (f1);
        ctx.ThreadExit += thread_exit;

        f1.HandleDestroyed += form_handle_destroyed2;

        f1.Show ();
        f1.Dispose();
        Assert.AreEqual (true, reached_form_handle_destroyed, "3");
        Assert.AreEqual (1, thread_exit_count, "4");
			
        f1.Dispose ();
    }

    [Test]
    public void ThreadExitTest ()
    {
        thread_exit_count = 0;

        var f1 = new Form ();
        f1.ShowInTaskbar = false;
        ctx = new ApplicationContext (f1);
        ctx.ThreadExit += thread_exit;

        Assert.AreEqual (f1, ctx.MainForm, "1");
        f1.ShowInTaskbar = false;
        f1.Show ();
        f1.Dispose ();
        Assert.AreEqual (f1, ctx.MainForm, "2");
        Assert.AreEqual (1, thread_exit_count, "3");

        f1 = new Form ();
        ctx = new ApplicationContext (f1);
        ctx.ThreadExit += thread_exit;
        f1.ShowInTaskbar = false;
        f1.Show ();
        f1.Dispose();
        Assert.AreEqual (f1, ctx.MainForm, "4");
        Assert.AreEqual (2, thread_exit_count, "5");
        f1.Dispose ();
    }
}