using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class ControlPropertyEventsTest : TestHelper
{
    [Test]
    public void PropertyAllowDrop ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.AllowDrop = true;
        Assert.AreEqual (true, c.AllowDrop, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        c.AllowDrop = true;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyAnchor ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Anchor = AnchorStyles.Bottom;
        Assert.AreEqual (AnchorStyles.Bottom, c.Anchor, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        c.Anchor = AnchorStyles.Bottom;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyAutoSize ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.AutoSize = true;
        Assert.AreEqual (true, c.AutoSize, "B1");
        Assert.AreEqual ("AutoSizeChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.AutoSize = true;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyBackColor ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.BackColor = Color.Aquamarine;
        Assert.AreEqual (Color.Aquamarine, c.BackColor, "B1");
        Assert.AreEqual ("BackColorChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.BackColor = Color.Aquamarine;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyBackgroundImage ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);
        Image i = new Bitmap (5, 5);

        c.BackgroundImage = i;
        Assert.AreSame (i, c.BackgroundImage, "B1");
        Assert.AreEqual ("BackgroundImageChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.BackgroundImage = i;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyBackgroundImageLayout ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.BackgroundImageLayout = ImageLayout.Zoom;
        Assert.AreEqual (ImageLayout.Zoom, c.BackgroundImageLayout, "B1");
        Assert.AreEqual ("BackgroundImageLayoutChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.BackgroundImageLayout = ImageLayout.Zoom;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyBindingContext ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);
        var b = new BindingContext ();

        c.BindingContext = b;
        Assert.AreSame (b, c.BindingContext, "B1");
        Assert.AreEqual ("BindingContextChanged", ew.ToString (), "B2");
			
        c.BindingContext = b;
        Assert.AreEqual ("BindingContextChanged", ew.ToString (), "B3");
    }

    [Test]
    public void PropertyBounds ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Bounds = new Rectangle (0, 0, 5, 5);
        Assert.AreEqual (new Rectangle (0, 0, 5, 5), c.Bounds, "B1");
        Assert.AreEqual ("Layout;Resize;SizeChanged;ClientSizeChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Bounds = new Rectangle (0, 0, 5, 5);
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    [Ignore ("Setting Capture to true does not hold, getter returns false.")]
    public void PropertyCapture ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Capture = true;
        Assert.AreEqual (true, c.Capture, "B1");
        Assert.AreEqual ("HandleCreated", ew.ToString (), "B2");

        ew.Clear ();
        c.Capture = true;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyClientSize ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.ClientSize = new Size (5, 5);
        Assert.AreEqual (new Size (5, 5), c.ClientSize, "B1");
        Assert.AreEqual ("Layout;Resize;SizeChanged;ClientSizeChanged;ClientSizeChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.ClientSize = new Size (5, 5);
        Assert.AreEqual ("ClientSizeChanged", ew.ToString (), "B3");
    }

    [Test]
    public void PropertyContextMenuStrip ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);
        var cm = new ContextMenuStrip ();

        c.ContextMenuStrip = cm;
        Assert.AreEqual (cm, c.ContextMenuStrip, "B1");
        Assert.AreEqual ("ContextMenuStripChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.ContextMenuStrip = cm;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyCursor ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Cursor = Cursors.HSplit;
        Assert.AreEqual (Cursors.HSplit, c.Cursor, "B1");
        Assert.AreEqual ("CursorChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Cursor = Cursors.HSplit;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyDock ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Dock = DockStyle.Fill;
        Assert.AreEqual (DockStyle.Fill, c.Dock, "B1");
        Assert.AreEqual ("DockChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Dock = DockStyle.Fill;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyEnabled ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Enabled = false;
        Assert.AreEqual (false, c.Enabled, "B1");
        Assert.AreEqual ("EnabledChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Enabled = false;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyFont ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);
        var f = new Font ("Arial", 14);
			
        c.Font = f;
        Assert.AreEqual (f, c.Font, "B1");
        Assert.AreEqual ("FontChanged;Layout", ew.ToString (), "B2");

        ew.Clear ();
        c.Font = f;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyForeColor ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.ForeColor = Color.Peru;
        Assert.AreEqual (Color.Peru, c.ForeColor, "B1");
        Assert.AreEqual ("ForeColorChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.ForeColor = Color.Peru;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyHeight ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Height = 27;
        Assert.AreEqual (27, c.Height, "B1");
        Assert.AreEqual ("Layout;Resize;SizeChanged;ClientSizeChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Height = 27;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyImeMode ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.ImeMode = ImeMode.Hiragana;
        Assert.AreEqual (ImeMode.Hiragana, c.ImeMode, "B1");
        Assert.AreEqual ("ImeModeChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.ImeMode = ImeMode.Hiragana;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyLeft ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Left = 27;
        Assert.AreEqual (27, c.Left, "B1");
        Assert.AreEqual ("Move;LocationChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Left = 27;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyLocation ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Location = new Point (5, 5);
        Assert.AreEqual (new Point (5, 5), c.Location, "B1");
        Assert.AreEqual ("Move;LocationChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Location = new Point (5, 5);
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyMargin ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Margin = new Padding (5);
        Assert.AreEqual (new Padding (5), c.Margin, "B1");
        Assert.AreEqual ("MarginChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Margin = new Padding (5);
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyMaximumSize ()
    {
        var c = new Control ();
        c.Size = new Size(10, 10);

        // Chaning MaximumSize below Size forces a size change
        var ew = new EventWatcher (c);
        c.MaximumSize = new Size (5, 5);
        Assert.AreEqual (new Size (5, 5), c.MaximumSize, "B1");
        Assert.AreEqual ("Layout;Resize;SizeChanged;ClientSizeChanged", ew.ToString (), "B2");

        // Changing MaximumSize when Size is already smaller or equal doesn't raise any events
        ew.Clear ();
        c.MaximumSize = new Size (5, 5);
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyMinimumSize ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.MinimumSize = new Size (5, 5);
        Assert.AreEqual (new Size (5, 5), c.MinimumSize, "B1");
        Assert.AreEqual ("Layout;Resize;SizeChanged;ClientSizeChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.MinimumSize = new Size (5, 5);
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyName ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Name = "Bob";
        Assert.AreEqual ("Bob", c.Name, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        c.Name = "Bob";
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyPadding ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Padding = new Padding (5);
        Assert.AreEqual (new Padding (5), c.Padding, "B1");
        Assert.AreEqual ("PaddingChanged;Layout", ew.ToString (), "B2");

        ew.Clear ();
        c.Padding = new Padding (5);
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyRegion ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);
        var r = new Region ();
			
        c.Region = r;
        Assert.AreSame (r, c.Region, "B1");
        Assert.AreEqual ("RegionChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Region = r;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyRightToLeft ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.RightToLeft = RightToLeft.Yes;
        Assert.AreEqual (RightToLeft.Yes, c.RightToLeft, "B1");
        Assert.AreEqual ("RightToLeftChanged;Layout", ew.ToString (), "B2");

        ew.Clear ();
        c.RightToLeft = RightToLeft.Yes;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertySize ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Size = new Size (5, 5);
        Assert.AreEqual (new Size (5, 5), c.Size, "B1");
        Assert.AreEqual ("Layout;Resize;SizeChanged;ClientSizeChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Size = new Size (5, 5);
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyTabIndex ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.TabIndex = 4;
        Assert.AreEqual (4, c.TabIndex, "B1");
        Assert.AreEqual ("TabIndexChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.TabIndex = 4;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyTabStop ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.TabStop = false;
        Assert.AreEqual (false, c.TabStop, "B1");
        Assert.AreEqual ("TabStopChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.TabStop = false;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyTag ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);
        Object o = "Hello";

        c.Tag = o;
        Assert.AreSame (o, c.Tag, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        c.Tag = o;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyText ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Text = "Enchilada";
        Assert.AreEqual ("Enchilada", c.Text, "B1");
        Assert.AreEqual ("TextChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Text = "Enchilada";
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyTop ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Top = 27;
        Assert.AreEqual (27, c.Top, "B1");
        Assert.AreEqual ("Move;LocationChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Top = 27;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyVisible ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Visible = false;
        Assert.AreEqual (false, c.Visible, "B1");
        Assert.AreEqual ("VisibleChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Visible = false;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyWidth ()
    {
        var c = new Control ();
        var ew = new EventWatcher (c);

        c.Width = 27;
        Assert.AreEqual (27, c.Width, "B1");
        Assert.AreEqual ("Layout;Resize;SizeChanged;ClientSizeChanged", ew.ToString (), "B2");

        ew.Clear ();
        c.Width = 27;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    private class EventWatcher
    {
        private string events = string.Empty;

        public EventWatcher (Control c)
        {
            c.AutoSizeChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("AutoSizeChanged;"); });
            c.BackColorChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("BackColorChanged;"); });
            c.BackgroundImageChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("BackgroundImageChanged;"); });
            c.BackgroundImageLayoutChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("BackgroundImageLayoutChanged;"); });
            c.BindingContextChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("BindingContextChanged;"); });
            c.CausesValidationChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("CausesValidationChanged;"); });
            c.ChangeUICues += new UICuesEventHandler (delegate (Object _, UICuesEventArgs _) { events += ("ChangeUICues;"); });
            c.Click += new EventHandler (delegate (Object _, EventArgs _) { events += ("Click;"); });
            c.ClientSizeChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("ClientSizeChanged;"); });
            c.ContextMenuStripChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("ContextMenuStripChanged;"); });
            c.ControlAdded += new ControlEventHandler (delegate (Object _, ControlEventArgs _) { events += ("ControlAdded;"); });
            c.ControlRemoved += new ControlEventHandler (delegate (Object _, ControlEventArgs _) { events += ("ControlRemoved;"); });
            c.CursorChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("CursorChanged;"); });
            c.DockChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("DockChanged;"); });
            c.DoubleClick += new EventHandler (delegate (Object _, EventArgs _) { events += ("DoubleClick;"); });
            c.DragDrop += new DragEventHandler (delegate (Object _, DragEventArgs _) { events += ("DragDrop;"); });
            c.DragEnter += new DragEventHandler (delegate (Object _, DragEventArgs _) { events += ("DragEnter;"); });
            c.DragLeave += new EventHandler (delegate (Object _, EventArgs _) { events += ("DragLeave;"); });
            c.DragOver += new DragEventHandler (delegate (Object _, DragEventArgs _) { events += ("DragOver;"); });
            c.EnabledChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("EnabledChanged;"); });
            c.Enter += new EventHandler (delegate (Object _, EventArgs _) { events += ("Enter;"); });
            c.FontChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("FontChanged;"); });
            c.ForeColorChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("ForeColorChanged;"); });
            c.GiveFeedback += new GiveFeedbackEventHandler (delegate (Object _, GiveFeedbackEventArgs _) { events += ("GiveFeedback;"); });
            c.GotFocus += new EventHandler (delegate (Object _, EventArgs _) { events += ("GotFocus;"); });
            c.HandleCreated += new EventHandler (delegate (Object _, EventArgs _) { events += ("HandleCreated;"); });
            c.HandleDestroyed += new EventHandler (delegate (Object _, EventArgs _) { events += ("HandleDestroyed;"); });
            c.ImeModeChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("ImeModeChanged;"); });
            c.Invalidated += new InvalidateEventHandler (delegate (Object _, InvalidateEventArgs _) { events += ("Invalidated;"); });
            c.KeyDown += new KeyEventHandler (delegate (Object _, KeyEventArgs _) { events += ("KeyDown;"); });
            c.KeyPress += new KeyPressEventHandler (delegate (Object _, KeyPressEventArgs _) { events += ("KeyPress;"); });
            c.KeyUp += new KeyEventHandler (delegate (Object _, KeyEventArgs _) { events += ("KeyUp;"); });
            c.Layout += new LayoutEventHandler (delegate (Object _, LayoutEventArgs _) { events += ("Layout;"); });
            c.Leave += new EventHandler (delegate (Object _, EventArgs _) { events += ("Leave;"); });
            c.LocationChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("LocationChanged;"); });
            c.LostFocus += new EventHandler (delegate (Object _, EventArgs _) { events += ("LostFocus;"); });
            c.MarginChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("MarginChanged;"); });
            c.MouseCaptureChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("MouseCaptureChanged;"); });
            c.MouseClick += new MouseEventHandler (delegate (Object _, MouseEventArgs _) { events += ("MouseClick;"); });
            c.MouseDoubleClick += new MouseEventHandler (delegate (Object _, MouseEventArgs _) { events += ("MouseDoubleClick;"); });
            c.MouseDown += new MouseEventHandler (delegate (Object _, MouseEventArgs _) { events += ("MouseDown;"); });
            c.MouseEnter += new EventHandler (delegate (Object _, EventArgs _) { events += ("MouseEnter;"); });
            c.MouseLeave += new EventHandler (delegate (Object _, EventArgs _) { events += ("MouseLeave;"); });
            c.MouseMove += new MouseEventHandler (delegate (Object _, MouseEventArgs _) { events += ("MouseMove;"); });
            c.MouseUp += new MouseEventHandler (delegate (Object _, MouseEventArgs _) { events += ("MouseUp;"); });
            c.MouseWheel += new MouseEventHandler (delegate (Object _, MouseEventArgs _) { events += ("MouseWheel;"); });
            c.Move += new EventHandler (delegate (Object _, EventArgs _) { events += ("Move;"); });
            c.PaddingChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("PaddingChanged;"); });
            c.Paint += new PaintEventHandler (delegate (Object _, PaintEventArgs _) { events += ("Paint;"); });
            c.ParentChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("ParentChanged;"); });
            c.PreviewKeyDown += new PreviewKeyDownEventHandler (delegate (Object _, PreviewKeyDownEventArgs _) { events += ("PreviewKeyDown;"); });
            c.QueryAccessibilityHelp += new QueryAccessibilityHelpEventHandler (delegate (Object _, QueryAccessibilityHelpEventArgs _) { events += ("QueryAccessibilityHelp;"); });
            c.QueryContinueDrag += new QueryContinueDragEventHandler (delegate (Object _, QueryContinueDragEventArgs _) { events += ("QueryContinueDrag;"); });
            c.RegionChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("RegionChanged;"); });
            c.Resize += new EventHandler (delegate (Object _, EventArgs _) { events += ("Resize;"); });
            c.RightToLeftChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("RightToLeftChanged;"); });
            c.SizeChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("SizeChanged;"); });
            c.StyleChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("StyleChanged;"); });
            c.SystemColorsChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("SystemColorsChanged;"); });
            c.TabIndexChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("TabIndexChanged;"); });
            c.TabStopChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("TabStopChanged;"); });
            c.TextChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("TextChanged;"); });
            c.Validated += new EventHandler (delegate (Object _, EventArgs _) { events += ("Validated;"); });
            c.Validating += new CancelEventHandler (delegate (Object _, CancelEventArgs _) { events += ("Validating;"); });
            c.VisibleChanged += new EventHandler (delegate (Object _, EventArgs _) { events += ("VisibleChanged;"); });
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

    private class ExposeProtectedProperties : Control
    {
        //public new bool CanRaiseEvents { get { return base.CanRaiseEvents; } }
        public new Cursor DefaultCursor { get { return base.DefaultCursor; } }
        public new Size DefaultMaximumSize { get { return base.DefaultMaximumSize; } }
        public new Size DefaultMinimumSize { get { return base.DefaultMinimumSize; } }
        public new Padding DefaultPadding { get { return base.DefaultPadding; } }
    }
}