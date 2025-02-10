// This test is designed to find exactly what conditions cause the control's
// Handle to be created.

using System.Drawing;
using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class ControlHandleTest : TestHelper
{
    [Test]
    public void TestPublicProperties ()
    {
        // This long, carpal-tunnel syndrome inducing test shows us that
        // the following properties cause the Handle to be created:
        // - AccessibilityObject	[get]
        // - Capture			[set]
        // - Handle			[get]
        var c = new Control ();
        // A
        object o = c.AccessibilityObject;
        Assert.IsTrue (c.IsHandleCreated, "A0");
        c = new Control ();

        o = c.AccessibleDefaultActionDescription;
        c.AccessibleDefaultActionDescription = "playdoh";
        Assert.IsFalse (c.IsHandleCreated, "A1");
        o = c.AccessibleDescription;
        c.AccessibleDescription = "more playdoh!";
        Assert.IsFalse (c.IsHandleCreated, "A2");
        o = c.AccessibleName;
        c.AccessibleName = "playdoh fun factory";
        Assert.IsFalse (c.IsHandleCreated, "A3");
        o = c.AllowDrop;
        c.AllowDrop = true;
        Assert.IsFalse (c.IsHandleCreated, "A5");
        // If we don't reset the control, handle creation will fail
        // because AllowDrop requires STAThread, which Nunit doesn't do
        c = new Control();
        o = c.Anchor;
        c.Anchor = AnchorStyles.Right;
        Assert.IsFalse (c.IsHandleCreated, "A6");
#if !MONO
        o = c.AutoScrollOffset;
        c.AutoScrollOffset = new Point (40, 40);
        Assert.IsFalse (c.IsHandleCreated, "A7");
#endif
        o = c.AutoSize;
        c.AutoSize = true;
        Assert.IsFalse (c.IsHandleCreated, "A8");
		
        // B
        o = c.BackColor;
        c.BackColor = Color.Green;
        Assert.IsFalse (c.IsHandleCreated, "A9");
        o = c.BackgroundImage;
        c.BackgroundImage = new Bitmap (1, 1);
        Assert.IsFalse (c.IsHandleCreated, "A10");
        o = c.BackgroundImageLayout;
        c.BackgroundImageLayout = ImageLayout.Stretch;
        Assert.IsFalse (c.IsHandleCreated, "A11");
        o = c.BindingContext;
        c.BindingContext = new BindingContext ();
        Assert.IsFalse (c.IsHandleCreated, "A12");
        o = c.Bottom;
        Assert.IsFalse (c.IsHandleCreated, "A13");
        o = c.Bounds;
        c.Bounds = new Rectangle (0, 0, 12, 12);
        Assert.IsFalse (c.IsHandleCreated, "A14");
			
        // C
        o = c.CanFocus;
        Assert.IsFalse (c.IsHandleCreated, "A15");
        o = c.CanSelect;
        Assert.IsFalse (c.IsHandleCreated, "A16");
        o = c.Capture;
        Assert.IsFalse (c.IsHandleCreated, "A17a");
        c.Capture = true;
        Assert.IsTrue (c.IsHandleCreated, "A17b");
        c = new Control ();
        o = c.CausesValidation;
        c.CausesValidation = false;
        Assert.IsFalse (c.IsHandleCreated, "A18");
        o = c.ClientRectangle;
        Assert.IsFalse (c.IsHandleCreated, "A19");
        o = c.ClientSize;
        c.ClientSize = new Size (30, 30);
        Assert.IsFalse (c.IsHandleCreated, "A20");
        o = c.CompanyName;
        Assert.IsFalse (c.IsHandleCreated, "A21");
        o = c.Container;
        Assert.IsFalse (c.IsHandleCreated, "A22");
        o = c.ContainsFocus;
        Assert.IsFalse (c.IsHandleCreated, "A23");
        o = c.ContextMenuStrip;
        c.ContextMenuStrip = new ContextMenuStrip ();
        Assert.IsFalse (c.IsHandleCreated, "A25");
        o = c.Controls;
        Assert.IsFalse (c.IsHandleCreated, "A26");
        o = c.Created;
        Assert.IsFalse (c.IsHandleCreated, "A27");
        o = c.Cursor;
        c.Cursor = Cursors.Arrow;
        Assert.IsFalse (c.IsHandleCreated, "A28");
			
        // D
        o = c.DataBindings;
        Assert.IsFalse (c.IsHandleCreated, "A29");
        o = c.DisplayRectangle;
        Assert.IsFalse (c.IsHandleCreated, "A30");
        o = c.Disposing;
        Assert.IsFalse (c.IsHandleCreated, "A31");
        o = c.Dock;
        c.Dock = DockStyle.Fill;
        Assert.IsFalse (c.IsHandleCreated, "A32");

        // E-H
        o = c.Enabled;
        c.Enabled = false;
        Assert.IsFalse (c.IsHandleCreated, "A33");
        c = new Control ();  //Reset just in case enable = false affects things
        o = c.Focused;
        Assert.IsFalse (c.IsHandleCreated, "A34");
        o = c.Font;
        c.Font = new Font (c.Font, FontStyle.Bold);
        Assert.IsFalse (c.IsHandleCreated, "A35");
        o = c.ForeColor;
        c.ForeColor = Color.Green;
        Assert.IsFalse (c.IsHandleCreated, "A36");
        o = c.Handle;
        Assert.IsTrue (c.IsHandleCreated, "A37");
        c = new Control ();
        o = c.HasChildren;
        Assert.IsFalse (c.IsHandleCreated, "A38");
        o = c.Height;
        c.Height = 12;
        Assert.IsFalse (c.IsHandleCreated, "A39");
			
        // I - L
        o = c.ImeMode;
        c.ImeMode = ImeMode.On;
        Assert.IsFalse (c.IsHandleCreated, "A40");
        o = c.InvokeRequired;
        Assert.IsFalse (c.IsHandleCreated, "A41");
        o = c.IsAccessible;
        Assert.IsFalse (c.IsHandleCreated, "A42");
        o = c.IsDisposed;
        Assert.IsFalse (c.IsHandleCreated, "A43");
#if !MONO
        o = c.IsMirrored;
        Assert.IsFalse (c.IsHandleCreated, "A44");
#endif
        o = c.LayoutEngine;
        Assert.IsFalse (c.IsHandleCreated, "A45");
        o = c.Left;
        c.Left = 15;
        Assert.IsFalse (c.IsHandleCreated, "A46");
        o = c.Location;
        c.Location = new Point (20, 20);
        Assert.IsFalse (c.IsHandleCreated, "A47");

        // M - N
        o = c.Margin;
        c.Margin = new Padding (6);
        Assert.IsFalse (c.IsHandleCreated, "A48");
        o = c.MaximumSize;
        c.MaximumSize = new Size (500, 500);
        Assert.IsFalse (c.IsHandleCreated, "A49");
        o = c.MinimumSize;
        c.MinimumSize = new Size (100, 100);
        Assert.IsFalse (c.IsHandleCreated, "A50");
        o = c.Name;
        c.Name = "web";
        Assert.IsFalse (c.IsHandleCreated, "A51");
			
        // P - R
        o = c.Padding;
        c.Padding = new Padding (4);
        Assert.IsFalse (c.IsHandleCreated, "A52");
        o = c.Parent;
        c.Parent = new Control ();
        Assert.IsFalse (c.IsHandleCreated, "A53");
        o = c.PreferredSize;
        Assert.IsFalse (c.IsHandleCreated, "A54");
        o = c.ProductName;
        Assert.IsFalse (c.IsHandleCreated, "A55");
        o = c.ProductVersion;
        Assert.IsFalse (c.IsHandleCreated, "A56");
        o = c.RecreatingHandle;
        Assert.IsFalse (c.IsHandleCreated, "A57");
        o = c.Region;
        c.Region = new Region (new Rectangle (0, 0, 177, 177));
        Assert.IsFalse (c.IsHandleCreated, "A58");
        o = c.Right;
        Assert.IsFalse (c.IsHandleCreated, "A59");
        o = c.RightToLeft;
        c.RightToLeft = RightToLeft.Yes;
        Assert.IsFalse (c.IsHandleCreated, "A60");
			
        // S - W
        o = c.Site;
        Assert.IsFalse (c.IsHandleCreated, "A61");
        o = c.Size;
        c.Size = new Size (188, 188);
        Assert.IsFalse (c.IsHandleCreated, "A62");
        o = c.TabIndex;
        c.TabIndex = 5;
        Assert.IsFalse (c.IsHandleCreated, "A63");
        o = c.Tag;
        c.Tag = "moooooooo";
        Assert.IsFalse (c.IsHandleCreated, "A64");
        o = c.Text;
        c.Text = "meoooowww";
        Assert.IsFalse (c.IsHandleCreated, "A65");
        o = c.Top;
        c.Top = 16;
        Assert.IsFalse (c.IsHandleCreated, "A66");
        o = c.TopLevelControl;
        Assert.IsFalse (c.IsHandleCreated, "A67");
#if !MONO
        o = c.UseWaitCursor;
        c.UseWaitCursor = true;
        Assert.IsFalse (c.IsHandleCreated, "A68");
#endif
        o = c.Visible;
        c.Visible = true;
        Assert.IsFalse (c.IsHandleCreated, "A69");
        o = c.Width;
        c.Width = 190;
        Assert.IsFalse (c.IsHandleCreated, "A70");
        o = c.WindowTarget;
        Assert.IsFalse (c.IsHandleCreated, "A71");
			
        RemoveWarning (o);
    }
		
    [Test]
    public void TestProtectedProperties ()
    {
        // Not a surprise, but none of these cause handle creation.
        // Included just to absolutely certain.
        var c = new ProtectedPropertyControl ();

        object o;
#if !MONO
        o = c.PublicCanRaiseEvents;
        Assert.IsFalse (c.IsHandleCreated, "A1");
#endif
        o = c.PublicCreateParams;
        Assert.IsFalse (c.IsHandleCreated, "A2");
        o = c.PublicDefaultCursor;
        Assert.IsFalse (c.IsHandleCreated, "A3");
        o = c.PublicDefaultImeMode;
        Assert.IsFalse (c.IsHandleCreated, "A4");
        o = c.PublicDefaultMargin;
        Assert.IsFalse (c.IsHandleCreated, "A5");
        o = c.PublicDefaultMaximumSize;
        Assert.IsFalse (c.IsHandleCreated, "A6");
        o = c.PublicDefaultMinimumSize;
        Assert.IsFalse (c.IsHandleCreated, "A7");
        o = c.PublicDefaultPadding;
        Assert.IsFalse (c.IsHandleCreated, "A8");
        o = c.PublicDefaultSize;
        Assert.IsFalse (c.IsHandleCreated, "A9");
        o = c.PublicDoubleBuffered;
        c.PublicDoubleBuffered = !c.PublicDoubleBuffered;
        Assert.IsFalse (c.IsHandleCreated, "A10");
        o = c.PublicFontHeight;
        c.PublicFontHeight = c.PublicFontHeight + 1;
        Assert.IsFalse (c.IsHandleCreated, "A11");
        o = c.PublicResizeRedraw;
        c.PublicResizeRedraw = !c.PublicResizeRedraw;
        Assert.IsFalse (c.IsHandleCreated, "A13");
#if !MONO
        o = c.PublicScaleChildren;
        Assert.IsFalse (c.IsHandleCreated, "A14");
#endif
        o = c.PublicShowFocusCues;
        Assert.IsFalse (c.IsHandleCreated, "A15");
        o = c.PublicShowKeyboardCues;
        Assert.IsFalse (c.IsHandleCreated, "A16");
			
        RemoveWarning (o);
    }

    readonly Control invokecontrol = new Control ();

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        invokecontrol.Dispose();
    }
		
    [Test]
    public void TestPublicMethods ()
    {
        // Public Methods that force Handle creation:
        // - CreateControl ()
        // - CreateGraphics ()
        // - GetChildAtPoint ()
        // - Invoke, BeginInvoke throws InvalidOperationException if Handle has not been created
        // - PointToClient ()
        // - PointToScreen ()
        // - RectangleToClient ()
        // - RectangleToScreen ()
        var c = new Control ();
			
        c.BringToFront ();
        Assert.IsFalse (c.IsHandleCreated, "A1");
        c.Contains (new Control ());
        Assert.IsFalse (c.IsHandleCreated, "A2");
        c.CreateControl ();
        Assert.IsTrue (c.IsHandleCreated, "A3");
        c = new Control ();
        var g = c.CreateGraphics ();
        g.Dispose ();
        Assert.IsTrue (c.IsHandleCreated, "A4");
        c = new Control ();
        c.Dispose ();
        Assert.IsFalse (c.IsHandleCreated, "A5");
        c = new Control ();
        //DragDropEffects d = c.DoDragDrop ("yo", DragDropEffects.None);
        //Assert.IsFalse (c.IsHandleCreated, "A6");
        //Assert.AreEqual (DragDropEffects.None, d, "A6b");
        //Bitmap b = new Bitmap (100, 100);
        //c.DrawToBitmap (b, new Rectangle (0, 0, 100, 100));
        //Assert.IsFalse (c.IsHandleCreated, "A7");
        //b.Dispose ();
        c.FindForm ();
        Assert.IsFalse (c.IsHandleCreated, "A8");
        c.Focus ();
        Assert.IsFalse (c.IsHandleCreated, "A9");

        c.GetChildAtPoint (new Point (10, 10));
        Assert.IsTrue (c.IsHandleCreated, "A10");
        c.GetContainerControl ();
        c = new Control ();
        Assert.IsFalse (c.IsHandleCreated, "A11");
        c.GetNextControl (new Control (), true);
        Assert.IsFalse (c.IsHandleCreated, "A12");
        c.GetPreferredSize (Size.Empty);
        Assert.IsFalse (c.IsHandleCreated, "A13");
        c.Hide ();
        Assert.IsFalse (c.IsHandleCreated, "A14");
        c.Invalidate ();
        Assert.IsFalse (c.IsHandleCreated, "A15");
        //c.Invoke (new InvokeDelegate (InvokeMethod));
        //Assert.IsFalse (c.IsHandleCreated, "A16");
        c.PerformLayout ();
        Assert.IsFalse (c.IsHandleCreated, "A17");
        c.PointToClient (new Point (100, 100));
        Assert.IsTrue (c.IsHandleCreated, "A18");
        c = new Control ();
        c.PointToScreen (new Point (100, 100));
        Assert.IsTrue (c.IsHandleCreated, "A19");
        c = new Control ();
        //c.PreProcessControlMessage   ???
        //c.PreProcessMessage          ???
        c.RectangleToClient (new Rectangle (0, 0, 100, 100));
        Assert.IsTrue (c.IsHandleCreated, "A20");
        c = new Control ();
        c.RectangleToScreen (new Rectangle (0, 0, 100, 100));
        Assert.IsTrue (c.IsHandleCreated, "A21");
        c = new Control ();
        c.Refresh ();
        Assert.IsFalse (c.IsHandleCreated, "A22");
        c.ResetBackColor ();
        Assert.IsFalse (c.IsHandleCreated, "A23");
        c.ResetBindings ();
        Assert.IsFalse (c.IsHandleCreated, "A24");
        c.ResetCursor ();
        Assert.IsFalse (c.IsHandleCreated, "A25");
        c.ResetFont ();
        Assert.IsFalse (c.IsHandleCreated, "A26");
        c.ResetForeColor ();
        Assert.IsFalse (c.IsHandleCreated, "A27");
        c.ResetImeMode ();
        Assert.IsFalse (c.IsHandleCreated, "A28");
        c.ResetRightToLeft ();
        Assert.IsFalse (c.IsHandleCreated, "A29");
        c.ResetText ();
        Assert.IsFalse (c.IsHandleCreated, "A30");
        c.SuspendLayout ();
        Assert.IsFalse (c.IsHandleCreated, "A31");
        c.ResumeLayout ();
        Assert.IsFalse (c.IsHandleCreated, "A32");
        c.Scale (new SizeF (1.5f, 1.5f));
        Assert.IsFalse (c.IsHandleCreated, "A33");
        c.Select ();
        Assert.IsFalse (c.IsHandleCreated, "A34");
        c.SelectNextControl (new Control (), true, true, true, true);
        Assert.IsFalse (c.IsHandleCreated, "A35");
        c.SetBounds (0, 0, 100, 100);
        Assert.IsFalse (c.IsHandleCreated, "A36");
        c.Update ();
        Assert.IsFalse (c.IsHandleCreated, "A37");
    }

    [Test]
    public void Show ()
    {
        var c = new Control ();
        Assert.IsFalse (c.IsHandleCreated, "A1");
        c.HandleCreated += HandleCreated_WriteStackTrace;
        c.Show ();
        Assert.IsFalse (c.IsHandleCreated, "A2");
    }

    void HandleCreated_WriteStackTrace (object sender, EventArgs e)
    {
        Console.WriteLine (Environment.StackTrace);
    }

    public delegate void InvokeDelegate ();
    public void InvokeMethod () { invokecontrol.Text = "methodinvoked"; }
		
    [Test]
    public void InvokeIOE ()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var c = new Control();
            c.Invoke(new InvokeDelegate(InvokeMethod));
        });
    }
		
    private class ProtectedPropertyControl : Control
    {
#if !MONO
        public bool PublicCanRaiseEvents { get { return base.CanRaiseEvents; } }
#endif
        public CreateParams PublicCreateParams { get { return base.CreateParams; } }
        public Cursor PublicDefaultCursor { get { return base.DefaultCursor; } }
        public ImeMode PublicDefaultImeMode { get { return base.DefaultImeMode; } }
        public Padding PublicDefaultMargin { get { return base.DefaultMargin; } }
        public Size PublicDefaultMaximumSize { get { return base.DefaultMaximumSize; } }
        public Size PublicDefaultMinimumSize { get { return base.DefaultMinimumSize; } }
        public Padding PublicDefaultPadding { get { return base.DefaultPadding; } }
        public Size PublicDefaultSize { get { return base.DefaultSize; } }
        public bool PublicDoubleBuffered { get { return base.DoubleBuffered; } set { base.DoubleBuffered = value; } }
        public int PublicFontHeight { get { return FontHeight; } set { FontHeight = value; } }
        public bool PublicResizeRedraw { get { return ResizeRedraw; } set { ResizeRedraw = value; } }
#if !MONO
        public bool PublicScaleChildren { get { return base.ScaleChildren; } }
#endif
        public bool PublicShowFocusCues { get { return base.ShowFocusCues; } }
        public bool PublicShowKeyboardCues { get { return base.ShowKeyboardCues; } }
    }
		
}