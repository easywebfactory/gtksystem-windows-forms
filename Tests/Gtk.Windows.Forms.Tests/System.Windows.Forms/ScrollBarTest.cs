//
// Copyright (c) 2005 Novell, Inc.
//
// Authors:
//      Hisham Mardam Bey (hisham.mardambey@gmail.com)
//      Ritvik Mayank (mritvik@novell.com)
//
//

using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using GtkTests.Helpers;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class ScrollBarTest : TestHelper
{
    [Test]
    public void PubPropTest()
    {
        var myscrlbar = new MyScrollBar();

        // B
        myscrlbar.BackColor = Color.Red;
        Assert.AreEqual(255, myscrlbar.BackColor.R, "B2");
        myscrlbar.BackgroundImage = Image.FromFile(TestResourceHelper.GetFullPathOfResource("Test/System.Windows.Forms/bitmaps/a.png"));
        Assert.AreEqual(16, myscrlbar.BackgroundImage.Height, "B3");

        // F
        Assert.AreEqual("ControlText", myscrlbar.ForeColor.Name, "F1");

        // I
        //Assert.AreEqual (ImeMode.Disable, myscrlbar.ImeMode, "I1");

        // L
        Assert.AreEqual(10, myscrlbar.LargeChange, "L1");

        // M
        Assert.AreEqual(100, myscrlbar.Maximum, "M1");
        Assert.AreEqual(0, myscrlbar.Minimum, "M2");
        myscrlbar.Maximum = 300;
        myscrlbar.Minimum = 100;
        Assert.AreEqual(300, myscrlbar.Maximum, "M3");
        Assert.AreEqual(100, myscrlbar.Minimum, "M4");

        // S
        Assert.AreEqual(null, myscrlbar.Site, "S1");
        Assert.AreEqual(1, myscrlbar.SmallChange, "S2");
        myscrlbar.SmallChange = 10;
        Assert.AreEqual(10, myscrlbar.SmallChange, "S3");

        // T
        Assert.AreEqual(false, myscrlbar.TabStop, "T1");
        myscrlbar.TabStop = true;
        Assert.AreEqual(true, myscrlbar.TabStop, "T2");
        Assert.AreEqual("", myscrlbar.Text, "T3");
        myscrlbar.Text = "MONO SCROLLBAR";
        Assert.AreEqual("MONO SCROLLBAR", myscrlbar.Text, "T4");

        // V
        Assert.AreEqual(100, myscrlbar.Value, "V1");
        myscrlbar.Value = 150;
        Assert.AreEqual(150, myscrlbar.Value, "V2");
    }

    [Test]
    public void ExceptionValueTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var myscrlbar = new MyScrollBar();
            myscrlbar.Minimum = 10;
            myscrlbar.Maximum = 20;
            myscrlbar.Value = 9;
            myscrlbar.Value = 21;
        });
    }

    [Test]
    public void ExceptionSmallChangeTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var myscrlbar = new MyScrollBar();
            myscrlbar.SmallChange = -1;
        });
    }

    [Test]
    public void ExceptionLargeChangeTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var myscrlbar = new MyScrollBar();
            myscrlbar.LargeChange = -1;
        });
    }

    [Test]
    public void PubMethodTest()
    {
        var myscrlbar = new MyScrollBar();
        myscrlbar.Text = "New HScrollBar";
        Assert.AreEqual("MonoTests.System.Windows.Forms.MyScrollBar, Minimum: 0, Maximum: 100, Value: 0",
            myscrlbar.ToString(), "T5");
    }

    [Test]
    public void DefaultMarginTest()
    {
        var s = new MyScrollBar();
        Assert.AreEqual(new Padding(0), s.PublicDefaultMargin, "A1");
    }

    [Test]
    public void MaximumValueTest()
    {
        ScrollBar s = new VScrollBar();

        s.LargeChange = 0;
        s.Maximum = 100;
        s.Value = 20;
        s.Maximum = 0;

        Assert.AreEqual(0, s.LargeChange, "A1");
        Assert.AreEqual(0, s.Maximum, "A2");
        Assert.AreEqual(0, s.Value, "A3");
    }

    [Test]
    public void LargeSmallerThanSmallChange()
    {
        ScrollBar s = new VScrollBar();

        s.LargeChange = 0;

        Assert.AreEqual(0, s.LargeChange, "A1");
        Assert.AreEqual(0, s.SmallChange, "A2");

        s.SmallChange = 10;

        Assert.AreEqual(0, s.LargeChange, "A3");
        Assert.AreEqual(0, s.SmallChange, "A4");

        s.LargeChange = 15;

        Assert.AreEqual(15, s.LargeChange, "A5");
        Assert.AreEqual(10, s.SmallChange, "A6");

        s.LargeChange = 5;

        Assert.AreEqual(5, s.LargeChange, "A7");
        Assert.AreEqual(5, s.SmallChange, "A8");
    }

    [Test]
    public void CalculateLargeChange()
    {
        ScrollBar s = new HScrollBar();

        s.Minimum = -50;
        s.Maximum = 50;
        s.LargeChange = 1000;

        Assert.AreEqual(101, s.LargeChange, "A1");

        s.Maximum = 200;
        s.Minimum = 199;
        s.LargeChange = 1000;

        Assert.AreEqual(2, s.LargeChange, "A2");

        s.Minimum = 200;
        s.LargeChange = 1000;

        Assert.AreEqual(1, s.LargeChange, "A3");
    }
}

[TestFixture]
public class ScrollBarEventTest : TestHelper
{
    static bool eventhandled = false;
    public void ScrollBar_EventHandler(object sender, EventArgs e)
    {
        eventhandled = true;
    }

    public void ScrollBarMouse_EventHandler(object sender, MouseEventArgs e)
    {
        eventhandled = true;
    }

    public void ScrollBarScroll_EventHandler(object sender, ScrollEventArgs e)
    {
        eventhandled = true;
    }

    public void ScrollBarPaint_EventHandler(object sender, PaintEventArgs e)
    {
        eventhandled = true;
    }

    [Test]
    public void BackColorChangedTest()
    {
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        ScrollBar myHscrlbar = new HScrollBar();
        myform.Controls.Add(myHscrlbar);
        myHscrlbar.BackColorChanged += ScrollBar_EventHandler;
        myHscrlbar.BackColor = Color.Red;
        Assert.AreEqual(true, eventhandled, "B4");
        eventhandled = false;
        myform.Dispose();
    }

    [Test]
    public void BackgroundImageChangedTest()
    {
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        ScrollBar myHscrlbar = new HScrollBar();
        myform.Controls.Add(myHscrlbar);
        myHscrlbar.BackgroundImageChanged += ScrollBar_EventHandler;
        myHscrlbar.BackgroundImage = Image.FromFile(TestResourceHelper.GetFullPathOfResource("Test/System.Windows.Forms/bitmaps/a.png"));
        Assert.AreEqual(true, eventhandled, "B5");
        eventhandled = false;
        myform.Dispose();
    }

    [Test]
    public void FontChangedTest()
    {
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        ScrollBar myHscrlbar = new HScrollBar();
        myform.Controls.Add(myHscrlbar);
        myHscrlbar.Font = new Font(FontFamily.GenericMonospace, 10);
        myHscrlbar.FontChanged += ScrollBar_EventHandler;
        var myFontDialog = new FontDialog();
        myHscrlbar.Font = myFontDialog.Font;
        Assert.AreEqual(true, eventhandled, "F2");
        eventhandled = false;
        myform.Dispose();
    }

    [Test]
    public void ForeColorChangedTest()
    {
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        ScrollBar myHscrlbar = new HScrollBar();
        myform.Controls.Add(myHscrlbar);
        myHscrlbar.ForeColorChanged += ScrollBar_EventHandler;
        myHscrlbar.ForeColor = Color.Azure;
        Assert.AreEqual(true, eventhandled, "F3");
        eventhandled = false;
        myform.Dispose();
    }

    [Test]
    [Category("NotWorking")]
    public void ScrollTest()
    {
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var myHscrlbar = new MyScrollBar();
        myform.Controls.Add(myHscrlbar);
        myHscrlbar.Scroll += ScrollBarScroll_EventHandler;
        myHscrlbar.ScrollNow();

        Assert.AreEqual(true, eventhandled, "S4");
        eventhandled = false;
        myform.Dispose();
    }

    [Test]
    public void TextChangedTest()
    {
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var myHscrlbar = new MyScrollBar();
        myform.Controls.Add(myHscrlbar);
        myHscrlbar.TextChanged += ScrollBar_EventHandler;
        myHscrlbar.Text = "foo";

        Assert.AreEqual(true, eventhandled, "T6");
        eventhandled = false;
        myform.Dispose();
    }

    [Test]
    public void ValueChangeTest()
    {
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var myHscrlbar = new MyScrollBar();
        myform.Controls.Add(myHscrlbar);
        myHscrlbar.Value = 40;
        myHscrlbar.ValueChanged += ScrollBar_EventHandler;
        myHscrlbar.Value = 50;
        Assert.AreEqual(true, eventhandled, "V3");
        eventhandled = false;
        myform.Dispose();
    }
}

public class MyHScrollBar : HScrollBar
{
    public MyHScrollBar() : base()
    {
    }

    public Size MyDefaultSize
    {
        get { return DefaultSize; }
    }

    public CreateParams MyCreateParams
    {
        get { return CreateParams; }
    }
}

[TestFixture]
public class MyHScrollBarTest : TestHelper
{
    [Test]
    public void ProtectedTest()
    {
        var msbar = new MyHScrollBar();

        Assert.AreEqual(80, msbar.MyDefaultSize.Width, "D1");
        // this is environment dependent.
        //Assert.AreEqual (21, msbar.MyDefaultSize.Height, "D2");
    }
}

public class MyVScrollBar : VScrollBar
{
    public MyVScrollBar() : base()
    {
    }

    public Size MyDefaultSize
    {
        get { return DefaultSize; }
    }

    public CreateParams MyCreateParams
    {
        get { return CreateParams; }
    }
}

[TestFixture]
public class MyVScrollBarTest : TestHelper
{
    [Test]
    public void PubMethodTest()
    {
        var msbar = new MyVScrollBar();

        Assert.AreEqual(RightToLeft.No, msbar.RightToLeft, "R1");

    }

    [Test]
    public void ProtMethodTest()
    {
        var msbar = new MyVScrollBar();

        // This is environment dependent.
        //Assert.AreEqual (21, msbar.MyDefaultSize.Width, "D3");
        Assert.AreEqual(80, msbar.MyDefaultSize.Height, "D4");
    }
}

[TestFixture]
[Ignore("Tests too strict")]
public class HScrollBarTestEventsOrder : TestHelper
{
    public string[] ArrayListToString(ArrayList arrlist)
    {
        string[] retval = new string[arrlist.Count];
        for (var i = 0; i < arrlist.Count; i++)
            retval[i] = (string)arrlist[i];
        return retval;
    }

    [Test]
    public void CreateEventsOrder()
    {
        string[] EventsWanted = {
            "OnHandleCreated",
            "OnBindingContextChanged",
            "OnBindingContextChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar();
        myform.Controls.Add(s);

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void BackColorChangedEventsOrder()
    {
        string[] EventsWanted = {
            "OnHandleCreated",
            "OnBindingContextChanged",
            "OnBindingContextChanged",
            "OnBackColorChanged",
            "OnInvalidated"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar();
        myform.Controls.Add(s);
        s.BackColor = Color.Aqua;

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void BackgroundImageChangedEventsOrder()
    {
        string[] EventsWanted = {
            "OnHandleCreated",
            "OnBindingContextChanged",
            "OnBindingContextChanged",
            "OnBackgroundImageChanged",
            "OnInvalidated"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar();
        myform.Controls.Add(s);
        s.BackgroundImage = Image.FromFile(TestResourceHelper.GetFullPathOfResource("Test/System.Windows.Forms/bitmaps/a.png"));

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    //[Test, Ignore ("Need to send proper Click / DoubleClick")]
    //public void ClickEventsOrder ()
    //   {
    //           string[] EventsWanted = {
    //                   "OnHandleCreated",
    //                     "OnBindingContextChanged",
    //                     "OnBindingContextChanged"
    //           };
    //           Form myform = new Form ();
    //           myform.ShowInTaskbar = false;
    //           myform.Visible = true;
    //           MyScrollBar s = new MyScrollBar ();
    //           myform.Controls.Add (s);
    //           s.MouseClick ();

    //           Assert.AreEqual (EventsWanted, ArrayListToString (s.Results));
    //           myform.Dispose ();
    //   }

    //[Test, Ignore ("Need to send proper Click / DoubleClick")]
    //public void DoubleClickEventsOrder ()
    //   {
    //           string[] EventsWanted = {
    //                   "OnHandleCreated",
    //                     "OnBindingContextChanged",
    //                     "OnBindingContextChanged"
    //           };
    //           Form myform = new Form ();
    //           myform.ShowInTaskbar = false;
    //           myform.Visible = true;
    //           MyScrollBar s = new MyScrollBar ();
    //           myform.Controls.Add (s);
    //           s.MouseDoubleClick ();

    //           Assert.AreEqual (EventsWanted, ArrayListToString (s.Results));
    //           myform.Dispose ();
    //   }

    [Test]
    public void FontChangedEventsOrder()
    {
        string[] EventsWanted = {
            "OnHandleCreated",
            "OnBindingContextChanged",
            "OnBindingContextChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar();
        myform.Controls.Add(s);
        var myFontDialog = new FontDialog();
        s.Font = myFontDialog.Font;

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void ForeColorChangedEventsOrder()
    {
        string[] EventsWanted = {
            "OnHandleCreated",
            "OnBindingContextChanged",
            "OnBindingContextChanged",
            "OnForeColorChanged",
            "OnInvalidated"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar();
        myform.Controls.Add(s);
        s.ForeColor = Color.Aqua;

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void ImeModeChangedChangedEventsOrder()
    {
        string[] EventsWanted = {
            "OnHandleCreated",
            "OnBindingContextChanged",
            "OnBindingContextChanged",
            "OnImeModeChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar();
        myform.Controls.Add(s);
        s.ImeMode = ImeMode.Katakana;

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void PaintEventsOrder()
    {
        string[] EventsWanted = {
            "OnHandleCreated",
            "OnBindingContextChanged",
            "OnBindingContextChanged",
            "OnInvalidated"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar();
        myform.Controls.Add(s);
        s.Visible = true;
        s.Refresh();
        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void ScrollEventsOrder()
    {
        string[] EventsWanted = {
            "OnHandleCreated",
            "OnBindingContextChanged",
            "OnBindingContextChanged",
            "OnScroll",
            "OnValueChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar();
        myform.Controls.Add(s);
        s.ScrollNow();

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void TextChangedEventsOrder()
    {
        string[] EventsWanted = {
            "OnHandleCreated",
            "OnBindingContextChanged",
            "OnBindingContextChanged",
            "OnTextChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar();
        myform.Controls.Add(s);
        s.Text = "foobar";

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void ValueChangedEventsOrder()
    {
        string[] EventsWanted = {
            "OnHandleCreated",
            "OnBindingContextChanged",
            "OnBindingContextChanged",
            "OnValueChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar();
        myform.Controls.Add(s);
        s.Value = 10;

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }
}

public class MyScrollBar2 : HScrollBar
{
    protected ArrayList results = new ArrayList();
    public MyScrollBar2() : base()
    {
        HandleCreated += HandleCreated_Handler;
        BackColorChanged += BackColorChanged_Handler;
        BackgroundImageChanged += BackgroundImageChanged_Handler;
        BindingContextChanged += BindingContextChanged_Handler;
        Click += Click_Handler;
        DoubleClick += DoubleClick_Handler;
        FontChanged += FontChanged_Handler;
        ForeColorChanged += ForeColorChanged_Handler;
        ImeModeChanged += ImeModeChanged_Handler;
        MouseDown += MouseDown_Handler;
        MouseMove += MouseMove_Handler;
        MouseUp += MouseUp_Handler;
        Invalidated += Invalidated_Handler;
        Resize += Resize_Handler;
        SizeChanged += SizeChanged_Handler;
        Layout += Layout_Handler;
        VisibleChanged += VisibleChanged_Handler;
        Paint += Paint_Handler;
        Scroll += Scroll_Handler;
        TextChanged += TextChanged_Handler;
        ValueChanged += ValueChanged_Handler;
    }

    protected void HandleCreated_Handler(object sender, EventArgs e)
    {
        results.Add("HandleCreated");
    }

    protected void BackColorChanged_Handler(object sender, EventArgs e)
    {
        results.Add("BackColorChanged");
    }

    protected void BackgroundImageChanged_Handler(object sender, EventArgs e)
    {
        results.Add("BackgroundImageChanged");
    }

    protected void Click_Handler(object sender, EventArgs e)
    {
        results.Add("Click");
    }

    protected void DoubleClick_Handler(object sender, EventArgs e)
    {
        results.Add("DoubleClick");
    }

    protected void FontChanged_Handler(object sender, EventArgs e)
    {
        results.Add("FontChanged");
    }

    protected void ForeColorChanged_Handler(object sender, EventArgs e)
    {
        results.Add("ForeColorChanged");
    }

    protected void ImeModeChanged_Handler(object sender, EventArgs e)
    {
        results.Add("ImeModeChanged");
    }

    protected void MouseDown_Handler(object sender, MouseEventArgs e)
    {
        results.Add("MouseDown");
    }

    protected void MouseMove_Handler(object sender, MouseEventArgs e)
    {
        results.Add("MouseMove");
    }

    protected void MouseUp_Handler(object sender, MouseEventArgs e)
    {
        results.Add("MouseUp");
    }

    protected void BindingContextChanged_Handler(object sender, EventArgs e)
    {
        results.Add("BindingContextChanged");
    }

    protected void Invalidated_Handler(object sender, InvalidateEventArgs e)
    {
        results.Add("Invalidated");
    }

    protected void Resize_Handler(object sender, EventArgs e)
    {
        results.Add("Resize");
    }

    protected void SizeChanged_Handler(object sender, EventArgs e)
    {
        results.Add("SizeChanged");
    }

    protected void Layout_Handler(object sender, LayoutEventArgs e)
    {
        results.Add("Layout");
    }

    protected void VisibleChanged_Handler(object sender, EventArgs e)
    {
        results.Add("VisibleChanged");
    }

    protected void Paint_Handler(object sender, PaintEventArgs e)
    {
        results.Add("Paint");
    }

    protected void Scroll_Handler(object sender, ScrollEventArgs e)
    {
        results.Add("Scroll");
    }

    protected void TextChanged_Handler(object sender, EventArgs e)
    {
        results.Add("TextChanged");
    }

    protected void ValueChanged_Handler(object sender, EventArgs e)
    {
        results.Add("ValueChanged");
    }

    public ArrayList Results
    {
        get { return results; }
    }

    //public void MoveMouse ()
    // {
    //         Message m;

    //         m = new Message ();

    //         m.Msg = (int)WndMsg.WM_NCHITTEST;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x0;
    //         m.LParam = (IntPtr)0x1c604ea;
    //         this.WndProc(ref m);

    //         m.Msg = (int)WndMsg.WM_SETCURSOR;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x100448;
    //         m.LParam = (IntPtr)0x2000001;
    //         this.WndProc(ref m);

    //         m.Msg = (int)WndMsg.WM_MOUSEFIRST;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x0;
    //         m.LParam = (IntPtr)0x14000b;
    //         this.WndProc(ref m);

    //         m.Msg = (int)WndMsg.WM_MOUSEHOVER;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x0;
    //         m.LParam = (IntPtr)0x14000b;
    //         this.WndProc(ref m);
    // }

    //public void MouseRightDown()
    // {
    //         Message m;

    //         m = new Message();

    //         m.Msg = (int)WndMsg.WM_RBUTTONDOWN;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x01;
    //         m.LParam = (IntPtr)0x9004f;
    //         this.WndProc(ref m);
    // }

    //public new void MouseClick()
    // {
    //         Message m;

    //         m = new Message();

    //         m.Msg = (int)WndMsg.WM_LBUTTONDOWN;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x01;
    //         m.LParam = (IntPtr)0x9004f;
    //         this.WndProc(ref m);

    //         m = new Message();

    //         m.Msg = (int)WndMsg.WM_LBUTTONUP;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x01;
    //         m.LParam = (IntPtr)0x9004f;
    //         this.WndProc(ref m);
    // }

    //public new void MouseDoubleClick ()
    // {
    //         MouseClick ();
    //         MouseClick ();
    // }

    //public void MouseRightUp()
    // {
    //         Message m;

    //         m = new Message();

    //         m.Msg = (int)WndMsg.WM_RBUTTONUP;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x01;
    //         m.LParam = (IntPtr)0x9004f;
    //         this.WndProc(ref m);
    // }

    public void ScrollNow()
    {
        Message m;

        m = new Message();

        m.Msg = 8468;
        m.HWnd = Handle;
        m.WParam = (IntPtr)0x1;
        m.LParam = (IntPtr)0x1a051a;
        WndProc(ref m);

        m.Msg = 233;
        m.HWnd = Handle;
        m.WParam = (IntPtr)0x1;
        m.LParam = (IntPtr)0x12eb34;
        WndProc(ref m);
    }
}

[TestFixture]
[Ignore("Tests too strict")]
public class HScrollBarTestEventsOrder2 : TestHelper
{
    public string[] ArrayListToString(ArrayList arrlist)
    {
        string[] retval = new string[arrlist.Count];
        for (var i = 0; i < arrlist.Count; i++)
            retval[i] = (string)arrlist[i];
        return retval;
    }

    [Test]
    public void CreateEventsOrder()
    {
        string[] EventsWanted = {
            "HandleCreated",
            "BindingContextChanged",
            "BindingContextChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar2();
        myform.Controls.Add(s);

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void BackColorChangedEventsOrder()
    {
        string[] EventsWanted = {
            "HandleCreated",
            "BindingContextChanged",
            "BindingContextChanged",
            "Invalidated",
            "BackColorChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar2();
        myform.Controls.Add(s);
        s.BackColor = Color.Aqua;

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void BackgroundImageChangedEventsOrder()
    {
        string[] EventsWanted = {
            "HandleCreated",
            "BindingContextChanged",
            "BindingContextChanged",
            "Invalidated",
            "BackgroundImageChanged"

        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar2();
        myform.Controls.Add(s);
        s.BackgroundImage = Image.FromFile(TestResourceHelper.GetFullPathOfResource("Test/System.Windows.Forms/bitmaps/a.png"));

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    //[Test, Ignore ("Need to send proper Click / DoubleClick")]
    //public void ClickEventsOrder ()
    //   {
    //           string[] EventsWanted = {
    //                   "HandleCreated",
    //                     "BindingContextChanged",
    //                     "BindingContextChanged"
    //           };
    //           Form myform = new Form ();
    //           myform.ShowInTaskbar = false;
    //           myform.Visible = true;
    //           MyScrollBar2 s = new MyScrollBar2 ();
    //           myform.Controls.Add (s);
    //           s.MouseClick ();

    //           Assert.AreEqual (EventsWanted, ArrayListToString (s.Results));
    //           myform.Dispose ();
    //   }

    //[Test, Ignore ("Need to send proper Click / DoubleClick")]
    //public void DoubleClickEventsOrder ()
    //   {
    //           string[] EventsWanted = {
    //                   "HandleCreated",
    //                     "BindingContextChanged",
    //                     "BindingContextChanged"
    //           };
    //           Form myform = new Form ();
    //           myform.ShowInTaskbar = false;
    //           myform.Visible = true;
    //           MyScrollBar2 s = new MyScrollBar2 ();
    //           myform.Controls.Add (s);
    //           s.MouseDoubleClick ();

    //           Assert.AreEqual (EventsWanted, ArrayListToString (s.Results));
    //           myform.Dispose ();
    //   }

    [Test]
    public void FontChangedEventsOrder()
    {
        string[] EventsWanted = {
            "HandleCreated",
            "BindingContextChanged",
            "BindingContextChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar2();
        myform.Controls.Add(s);
        var myFontDialog = new FontDialog();
        s.Font = myFontDialog.Font;

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void ForeColorChangedEventsOrder()
    {
        string[] EventsWanted = {
            "HandleCreated",
            "BindingContextChanged",
            "BindingContextChanged",
            "Invalidated",
            "ForeColorChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar2();
        myform.Controls.Add(s);
        s.ForeColor = Color.Aqua;

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void ImeModeChangedChangedEventsOrder()
    {
        string[] EventsWanted = {
            "HandleCreated",
            "BindingContextChanged",
            "BindingContextChanged",
            "ImeModeChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar2();
        myform.Controls.Add(s);
        s.ImeMode = ImeMode.Katakana;

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void PaintEventsOrder()
    {
        string[] EventsWanted = {
            "HandleCreated",
            "BindingContextChanged",
            "BindingContextChanged",
            "Invalidated"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar2();
        myform.Controls.Add(s);
        s.Visible = true;
        s.Refresh();

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void ScrollEventsOrder()
    {
        string[] EventsWanted = {
            "HandleCreated",
            "BindingContextChanged",
            "BindingContextChanged",
            "Scroll",
            "ValueChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar2();
        myform.Controls.Add(s);
        s.ScrollNow();

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void TextChangedEventsOrder()
    {
        string[] EventsWanted = {
            "HandleCreated",
            "BindingContextChanged",
            "BindingContextChanged",
            "TextChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar2();
        myform.Controls.Add(s);
        s.Text = "foobar";

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }

    [Test]
    public void ValueChangedEventsOrder()
    {
        string[] EventsWanted = {
            "HandleCreated",
            "BindingContextChanged",
            "BindingContextChanged",
            "ValueChanged"
        };
        var myform = new Form();
        myform.ShowInTaskbar = false;
        myform.Visible = true;
        var s = new MyScrollBar2();
        myform.Controls.Add(s);
        s.Value = 10;

        Assert.AreEqual(EventsWanted, ArrayListToString(s.Results));
        myform.Dispose();
    }
}

[TestFixture]
public class ScrollEventArgsTest : TestHelper
{
    [Test]
    public void Defaults()
    {
        var e = new ScrollEventArgs(ScrollEventType.EndScroll, 5);

        Assert.AreEqual(5, e.NewValue, "A1");
        Assert.AreEqual(-1, e.OldValue, "A2");
        Assert.AreEqual(ScrollOrientation.HorizontalScroll, e.ScrollOrientation, "A3");
        Assert.AreEqual(ScrollEventType.EndScroll, e.Type, "A4");

        e = new ScrollEventArgs(ScrollEventType.EndScroll, 5, 10);

        Assert.AreEqual(10, e.NewValue, "A5");
        Assert.AreEqual(5, e.OldValue, "A6");
        Assert.AreEqual(ScrollOrientation.HorizontalScroll, e.ScrollOrientation, "A7");
        Assert.AreEqual(ScrollEventType.EndScroll, e.Type, "A8");

        e = new ScrollEventArgs(ScrollEventType.EndScroll, 5, ScrollOrientation.VerticalScroll);

        Assert.AreEqual(5, e.NewValue, "A9");
        Assert.AreEqual(-1, e.OldValue, "A10");
        Assert.AreEqual(ScrollOrientation.VerticalScroll, e.ScrollOrientation, "A11");
        Assert.AreEqual(ScrollEventType.EndScroll, e.Type, "A12");

        e = new ScrollEventArgs(ScrollEventType.EndScroll, 5, 10, ScrollOrientation.VerticalScroll);

        Assert.AreEqual(10, e.NewValue, "A13");
        Assert.AreEqual(5, e.OldValue, "A14");
        Assert.AreEqual(ScrollOrientation.VerticalScroll, e.ScrollOrientation, "A15");
        Assert.AreEqual(ScrollEventType.EndScroll, e.Type, "A16");
    }
}

public class MyScrollBar : HScrollBar
{
    private readonly ArrayList results = new ArrayList();

    public MyScrollBar() : base()
    {
        BackColorChanged += OnBackColorChanged;
        BackgroundImageChanged += OnBackgroundImageChanged;
        Click += OnClick;
        DoubleClick += OnDoubleClick;
        FontChanged += OnFontChanged;
        ForeColorChanged += OnForeColorChanged;
        ImeModeChanged += OnImeModeChanged;
        MouseDown += OnMouseDown;
        MouseMove += OnMouseMove;
        MouseEnter += OnMouseEnter;
        MouseLeave += OnMouseLeave;
        MouseHover += OnMouseHover;
        MouseUp += OnMouseUp;
        HandleCreated += OnHandleCreated;
        BindingContextChanged += OnBindingContextChanged;
        Invalidated += OnInvalidated;
        Resize += OnResize;
        SizeChanged += OnSizeChanged;
        Layout += OnLayout;
        VisibleChanged += OnVisibleChanged;
        Scroll += OnScroll;
        TextChanged += OnTextChanged;
        ValueChanged += OnValueChanged;
        Paint += OnPaint;

    }

    public Padding PublicDefaultMargin { get { return base.DefaultMargin; } }

    protected void OnBackColorChanged(object sender, EventArgs e)
    {
        results.Add("OnBackColorChanged");
    }

    protected void OnBackgroundImageChanged(object sender, EventArgs e)
    {
        results.Add("OnBackgroundImageChanged");
    }

    protected void OnClick(object sender, EventArgs e)
    {
        results.Add("OnClick");
    }

    protected void OnDoubleClick(object sender, EventArgs e)
    {
        results.Add("OnDoubleClick");
    }

    protected void OnFontChanged(object sender, EventArgs e)
    {
        results.Add("OnFontChanged");
    }

    protected void OnForeColorChanged(object sender, EventArgs e)
    {
        results.Add("OnForeColorChanged");
    }

    protected void OnImeModeChanged(object sender, EventArgs e)
    {
        results.Add("OnImeModeChanged");
    }

    protected void OnMouseDown(object sender, MouseEventArgs e)
    {
        results.Add("OnMouseDown");
    }

    protected void OnMouseMove(object sender, MouseEventArgs e)
    {
        results.Add("OnMouseMove");
    }

    protected void OnMouseEnter(object sender, EventArgs e)
    {
        results.Add("OnMouseEnter");
    }

    protected void OnMouseLeave(object sender, EventArgs e)
    {
        results.Add("OnMouseLeave");
    }

    protected void OnMouseHover(object sender, EventArgs e)
    {
        results.Add("OnMouseHover");
    }

    protected void OnMouseUp(object sender, MouseEventArgs e)
    {
        results.Add("OnMouseUp");
    }

    protected void OnHandleCreated(object sender, EventArgs e)
    {
        results.Add("OnHandleCreated");
    }

    protected void OnBindingContextChanged(object sender, EventArgs e)
    {
        results.Add("OnBindingContextChanged");
    }

    protected void OnInvalidated(object sender, InvalidateEventArgs e)
    {
        results.Add("OnInvalidated");
    }

    protected void OnResize(object sender, EventArgs e)
    {
        results.Add("OnResize");
    }

    protected void OnSizeChanged(object sender, EventArgs e)
    {
        results.Add("OnSizeChanged");
    }

    protected void OnLayout(object sender, LayoutEventArgs e)
    {
        results.Add("OnLayout");
    }

    protected void OnVisibleChanged(object sender, EventArgs e)
    {
        results.Add("OnVisibleChanged");
    }

    protected void OnScroll(object sender, ScrollEventArgs e)
    {
        results.Add("OnScroll");
    }

    protected void OnTextChanged(object sender, EventArgs e)
    {
        results.Add("OnTextChanged");
    }

    protected void OnValueChanged(object sender, EventArgs e)
    {
        results.Add("OnValueChanged");
    }

    protected void OnPaint(object sender, PaintEventArgs e)
    {
        results.Add("OnPaint");
    }

    public ArrayList Results
    {
        get { return results; }
    }

    //public void MoveMouse ()
    // {
    //         Message m;

    //         m = new Message ();

    //         m.Msg = (int)WndMsg.WM_NCHITTEST;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x0;
    //         m.LParam = (IntPtr)0x1c604ea;
    //         this.WndProc(ref m);

    //         m.Msg = (int)WndMsg.WM_SETCURSOR;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x100448;
    //         m.LParam = (IntPtr)0x2000001;
    //         this.WndProc(ref m);

    //         m.Msg = (int)WndMsg.WM_MOUSEFIRST;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x0;
    //         m.LParam = (IntPtr)0x14000b;
    //         this.WndProc(ref m);

    //         m.Msg = (int)WndMsg.WM_MOUSEHOVER;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x0;
    //         m.LParam = (IntPtr)0x14000b;
    //         this.WndProc(ref m);
    // }

    //public new void MouseClick()
    // {

    //         Message m;

    //         m = new Message();

    //         m.Msg = (int)WndMsg.WM_LBUTTONDOWN;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x01;
    //         m.LParam = (IntPtr)0x9004f;
    //         this.WndProc(ref m);

    //         m = new Message();

    //         m.Msg = (int)WndMsg.WM_LBUTTONUP;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x01;
    //         m.LParam = (IntPtr)0x9004f;
    //         this.WndProc(ref m);
    // }

    //public new void MouseDoubleClick ()
    // {
    //         this.MouseClick ();
    //         this.MouseClick ();
    // }
    //public void MouseRightDown()
    // {
    //         Message m;

    //         m = new Message();

    //         m.Msg = (int)WndMsg.WM_RBUTTONDOWN;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x01;
    //         m.LParam = (IntPtr)0x9004f;
    //         this.WndProc(ref m);
    // }

    //public void MouseRightUp()
    // {
    //         Message m;

    //         m = new Message();

    //         m.Msg = (int)WndMsg.WM_RBUTTONUP;
    //         m.HWnd = this.Handle;
    //         m.WParam = (IntPtr)0x01;
    //         m.LParam = (IntPtr)0x9004f;
    //         this.WndProc(ref m);
    // }

    public void ScrollNow()
    {
        Message m;

        m = new Message();

        m.Msg = 8468;
        m.HWnd = Handle;
        m.WParam = (IntPtr)0x1;
        m.LParam = (IntPtr)0x1a051a;
        WndProc(ref m);

        m.Msg = 233;
        m.HWnd = Handle;
        m.WParam = (IntPtr)0x1;
        m.LParam = (IntPtr)0x12eb34;
        WndProc(ref m);
    }
}
