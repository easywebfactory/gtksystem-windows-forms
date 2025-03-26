//
// Copyright (c) 2007 Novell, Inc.
//
// Authors:
//	Rolf Bjarne Kvinge  (RKvinge@novell.com)
//

using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using Sys_Threading=System.Threading;
using FormsApplication=System.Windows.Forms.Application;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class TimerTest : TestHelper
{
    bool Ticked;
		
    [Test ()]
    public void IntervalException1 ()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var timer = new Timer();
            timer.Interval = 0;
        });
    }

    [Test ()]
    public void IntervalException2 ()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var timer = new Timer();
            timer.Interval = -1;
        });
    }

    [Test ()]
    public void IntervalException3 ()
    {
        var timer = new Timer ();
        timer.Interval = int.MaxValue;
    }

    [Test ()]
    public void IntervalException4 ()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var timer = new Timer();
            timer.Interval = int.MinValue;
        });
    }
		
    [Test]
    [Category ("NotWorking")]
    public void StartTest ()
    {
        // This test fails about 50% of the time on the buildbots.
        Ticked = false;
        using var timer = new Timer ();
        timer.Tick += TickHandler;
        timer.Start ();
        Thread.Sleep (500);
        Application.DoEvents ();
        Assert.AreEqual (true, timer.Enabled, "1");
        Assert.AreEqual (true, Ticked, "2");
    }

    [Test]
    public void StopTest ()
    {
        Ticked = false;
        using var timer = new Timer ();
        timer.Tick += TickHandler;
        timer.Interval = 200;
        timer.Start ();
        Assert.AreEqual (true, timer.Enabled, "1");
        Assert.AreEqual (false, Ticked, "2");
        timer.Stop ();
        Assert.AreEqual (false, Ticked, "3"); // This may fail if we are running on a very slow machine...
        Assert.AreEqual (false, timer.Enabled, "4");
        Thread.Sleep (500);
        Assert.AreEqual (false, Ticked, "5");
    }
		
    [Test]
    public void TagTest ()
    {
        var timer = new Timer ();
        timer.Tag = "a";
        Assert.AreEqual ("a", timer.Tag, "1");
    }

    /* Application.DoEvents and Sleep are not guarenteed on Linux
    [Test]
    public void EnabledTest ()
    {
        Ticked = false;
        using (Timer timer = new Timer ()) {
            timer.Tick += new EventHandler (TickHandler);
            timer.Enabled = true;
            Sys_Threading.Thread.Sleep (150);
            Application.DoEvents ();
            Assert.AreEqual (true, timer.Enabled, "1");
            Assert.AreEqual (true, Ticked, "2");
        }

        Ticked = false;
        using (Timer timer = new Timer ()) {
            timer.Tick += new EventHandler (TickHandler);
            timer.Interval = 1000;
            timer.Enabled = true;
            Assert.AreEqual (true, timer.Enabled, "3");
            Assert.AreEqual (false, Ticked, "4");
            timer.Enabled = false;
            Assert.AreEqual (false, Ticked, "5"); // This may fail if we are running on a very slow machine...
            Assert.AreEqual (false, timer.Enabled, "6");
        }
    }
    */

    void TickHandler (object? sender, EventArgs e)
    {
        Ticked = true;
    }
		
    [Test]
    public void DefaultProperties ()
    {
        var timer = new Timer ();
        Assert.AreEqual (null, timer.Container, "C1");
        Assert.AreEqual (false, timer.Enabled, "E1");
        Assert.AreEqual (100, timer.Interval, "I1");
        Assert.AreEqual (null, timer.Site, "S1");
        Assert.AreEqual (null, timer.Tag, "T1");
    }

    [Test] // bug #325033
    public void RunningThread ()
    {
        var f1 = new Bug325033Form ();
        Application.Run (f1);
        var f2 = new Bug325033Form2 ();
        Application.Run (f2);

        f1.Dispose ();
        f2.Dispose ();
    }

    class Bug325033Form : Form
    {
        public Bug325033Form ()
        {
            Load += Form_Load;
        }

        void Form_Load (object? sender, EventArgs e)
        {
            var t = new Thread (Run);
            t.IsBackground = true;
            t.Start ();
            t.Join ();
            Close ();
        }

        void Run ()
        {
            FormsApplication.Run (new Bug325033Form2());
        }
    }

    class Bug325033Form2 : Form
    {
        public Bug325033Form2 ()
        {
            _label = new Label ();
            _label.AutoSize = true;
            _label.Dock = DockStyle.Fill;
            _label.Text = "It should close automatically.";
            Controls.Add (_label);
            _timer = new Timer ();
            _timer.Tick += Timer_Tick;
            _timer.Interval = 500;
            _timer.Start ();
        }

        void Timer_Tick (object? sender, EventArgs e)
        {
            _timer.Stop ();
            Close ();
        }

        private readonly Label _label;
        private readonly Timer _timer;
    }
}