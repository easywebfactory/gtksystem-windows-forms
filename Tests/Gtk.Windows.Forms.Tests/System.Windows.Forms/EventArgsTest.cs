using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using GtkTests.System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class EventArgsTest : TestHelper
{
    [Test]
    public void TestBindingCompleteEventArgs ()
    {
        var b = new Binding ("TestBind", null, "TestMember");
        var c = new BindingCompleteContext ();
        var errorText = "This is an error!";
        Exception ex = new ArgumentNullException ();

        var e = new BindingCompleteEventArgs (b, BindingCompleteState.Success, c);

        Assert.AreEqual (b, e.Binding, "A1");
        Assert.AreEqual (BindingCompleteState.Success, e.BindingCompleteState, "A2");
        Assert.AreEqual (c, e.BindingCompleteContext, "A3");
        Assert.AreEqual (false, e.Cancel, "A4");
        Assert.AreEqual (String.Empty, e.ErrorText, "A5");
        Assert.AreEqual (null, e.Exception, "A6");

        var e2 = new BindingCompleteEventArgs (b, BindingCompleteState.Success, c, errorText);

        Assert.AreEqual (b, e2.Binding, "B1");
        Assert.AreEqual (BindingCompleteState.Success, e2.BindingCompleteState, "B2");
        Assert.AreEqual (c, e2.BindingCompleteContext, "B3");
        Assert.AreEqual (true, e2.Cancel, "B4");
        Assert.AreEqual (errorText, e2.ErrorText, "B5");
        Assert.AreEqual (null, e2.Exception, "B6");

        var e3 = new BindingCompleteEventArgs (b, BindingCompleteState.Success, c, errorText, ex);

        Assert.AreEqual (b, e3.Binding, "C1");
        Assert.AreEqual (BindingCompleteState.Success, e3.BindingCompleteState, "C2");
        Assert.AreEqual (c, e3.BindingCompleteContext, "C3");
        Assert.AreEqual (true, e3.Cancel, "C4");
        Assert.AreEqual (errorText, e3.ErrorText, "C5");
        Assert.AreEqual (ex, e3.Exception, "C6");

        var e4 = new BindingCompleteEventArgs (b, BindingCompleteState.Success, c, errorText, ex, true);

        Assert.AreEqual (b, e4.Binding, "D1");
        Assert.AreEqual (BindingCompleteState.Success, e4.BindingCompleteState, "D2");
        Assert.AreEqual (c, e4.BindingCompleteContext, "D3");
        Assert.AreEqual (true, e4.Cancel, "D4");
        Assert.AreEqual (errorText, e4.ErrorText, "D5");
        Assert.AreEqual (ex, e4.Exception, "D6");

    }

    [Test]
    public void TestBindingManagerDataErrorEventArgs ()
    {
        Exception ex = new ArgumentNullException ();

        var e = new BindingManagerDataErrorEventArgs (ex);

        Assert.AreEqual (ex, e.Exception, "A1");
    }

    [Test]
    public void TestCacheVirtualItemsEventArgs ()
    {
        var start = 7;
        var end = 26;

        var e = new CacheVirtualItemsEventArgs (start, end);

        Assert.AreEqual (start, e.StartIndex, "A1");
        Assert.AreEqual (end, e.EndIndex, "A2");
    }

    [Test]
    public void TestColumnReorderedEventArgs ()
    {
        var oldindex = 7;
        var newindex = 26;
        var ch = new ColumnHeader ();
        ch.Text = "TestHeader";

        var e = new ColumnReorderedEventArgs (oldindex, newindex, ch);

        Assert.AreEqual (oldindex, e.OldDisplayIndex, "A1");
        Assert.AreEqual (newindex, e.NewDisplayIndex, "A2");
        Assert.AreEqual (ch, e.Header, "A3");
        Assert.AreEqual (false, e.Cancel, "A4");
    }

    [Test]
    public void TestColumnWidthChangedEventArgs ()
    {
        var col = 42;

        var e = new ColumnWidthChangedEventArgs (col);

        Assert.AreEqual (col, e.ColumnIndex, "A1");
    }

    [Test]
    public void TestColumnWidthChangingEventArgs ()
    {
        var col = 27;
        var width = 543;

        var e = new ColumnWidthChangingEventArgs (col, width);

        Assert.AreEqual (col, e.ColumnIndex, "A1");
        Assert.AreEqual (width, e.NewWidth, "A2");
        Assert.AreEqual (false, e.Cancel, "A3");

        var e2 = new ColumnWidthChangingEventArgs (col, width, true);

        Assert.AreEqual (col, e2.ColumnIndex, "B1");
        Assert.AreEqual (width, e2.NewWidth, "B2");
        Assert.AreEqual (true, e2.Cancel, "B3");
    }

    [Test]
    public void TestFormClosedEventArgs ()
    {
        var cr = CloseReason.WindowsShutDown;

        var e = new FormClosedEventArgs (cr);

        Assert.AreEqual (cr, e.CloseReason, "A1");
    }

    [Test]
    public void TestFormClosingEventArgs ()
    {
        var cr = CloseReason.WindowsShutDown;

        var e = new FormClosingEventArgs (cr, true);

        Assert.AreEqual (cr, e.CloseReason, "A1");
        Assert.AreEqual (true, e.Cancel, "A2");
    }

    [Test]
    public void TestItemCheckedEventArgs ()
    {
        var item = new ListViewItem ("TestItem");

        var e = new ItemCheckedEventArgs (item);

        Assert.AreEqual (item, e.Item, "A1");
    }

    [Test]
    public void TestListControlConvertEventArgs ()
    {
        var item = new ListViewItem ("TestItem");
        var value = (object)"TestObject";
        var t = typeof (string);

        var e = new ListControlConvertEventArgs (value, t, item);

        Assert.AreEqual (item, e.ListItem, "A1");
        Assert.AreEqual (value, e.Value, "A2");
        Assert.AreEqual (t, e.DesiredType, "A3");
    }

    [Test]
    public void TestListViewItemMouseHoverEventArgs ()
    {
        var item = new ListViewItem ("TestItem");

        var e = new ListViewItemMouseHoverEventArgs (item);

        Assert.AreEqual (item, e.Item, "A1");
    }

    [Test]
    public void TestListViewItemSelectionChangedEventArgs ()
    {
        var item = new ListViewItem ("TestItem");
        var selected = false;
        var index = 35;

        var e = new ListViewItemSelectionChangedEventArgs (item, index, selected);

        Assert.AreEqual (item, e.Item, "A1");
        Assert.AreEqual (selected, e.IsSelected, "A2");
        Assert.AreEqual (index, e.ItemIndex, "A3");
    }

    [Test]
    public void TestListViewVirtualItemsSelectionRangeChangedEventArgs ()
    {
        var selected = false;
        var start = 3;
        var end = 76;

        var e = new ListViewVirtualItemsSelectionRangeChangedEventArgs (start, end, selected);

        Assert.AreEqual (selected, e.IsSelected, "A1");
        Assert.AreEqual (start, e.StartIndex, "A2");
        Assert.AreEqual (end, e.EndIndex, "A3");
    }

    [Test]
    public void TestMaskInputRejectedEventArgs ()
    {
        var pos = 2;
        var hint = MaskedTextResultHint.InvalidInput;

        var e = new MaskInputRejectedEventArgs (pos, hint);

        Assert.AreEqual (pos, e.Position, "A1");
        Assert.AreEqual (hint, e.RejectionHint, "A2");
    }

    [Test]
    public void TestPopupEventArgs ()
    {
        Control c = new ListBox ();
        IWin32Window w = null;
        var balloon = true;
        var s = new Size (123, 54);

        var e = new PopupEventArgs (w, c, balloon, s);

        Assert.AreEqual (c, e.AssociatedControl, "A1");
        Assert.AreEqual (w, e.AssociatedWindow, "A2");
        Assert.AreEqual (balloon, e.IsBalloon, "A3");
        Assert.AreEqual (s, e.ToolTipSize, "A4");
    }

    [Test]
    public void TestPreviewKeyDownEventArgs ()
    {
        var k = (Keys)196674;  // Control-Shift-B

        var e = new PreviewKeyDownEventArgs (k);

        Assert.AreEqual (false, e.Alt, "A1");
        Assert.AreEqual (true, e.Control, "A2");
        Assert.AreEqual (false, e.IsInputKey, "A3");
        Assert.AreEqual ((Keys)66, e.KeyCode, "A4");  // B
        Assert.AreEqual (k, e.KeyData, "A5");
        Assert.AreEqual (66, e.KeyValue, "A6");
        Assert.AreEqual ((Keys)196608, e.Modifiers, "A7");  // Control + Shift
        Assert.AreEqual (true, e.Shift, "A8");

        e.IsInputKey = true;

        Assert.AreEqual (true, e.IsInputKey, "A9");
    }

    [Test]
    public void TestRetrieveVirtualItemEventArgs()
    {
        var item = new ListViewItem("TestItem");
        var index = 75;
			
        var e = new RetrieveVirtualItemEventArgs(index);
			
        Assert.AreEqual(index, e.ItemIndex, "A1");
        Assert.AreEqual(null, e.Item, "A2");
			
        e.Item = item;
			
        Assert.AreEqual(item, e.Item, "A3");
    }
		
    [Test]
    public void TestSplitterCancelEventArgs()
    {
        var mx = 23;
        var my = 33;
        var sx = 43;
        var sy = 53;
			
        var e = new SplitterCancelEventArgs(mx, my, sx, sy);
			
        Assert.AreEqual(mx, e.MouseCursorX, "A1");
        Assert.AreEqual(my, e.MouseCursorY, "A2");
        Assert.AreEqual(sx, e.SplitX, "A3");
        Assert.AreEqual(sy, e.SplitY, "A4");
			
        e.SplitX = 11;
        e.SplitY = 12;
			
        Assert.AreEqual(11, e.SplitX, "A5");
        Assert.AreEqual(12, e.SplitY, "A6");
    }
		
    [Test]
    public void TestTabControlCancelEventArgs()
    {
        var tca = TabControlAction.Deselecting;
        var tp = new TabPage("HI!");
        var index = 477;
			
        var e = new TabControlCancelEventArgs(tp, index, true, tca);
			
        Assert.AreEqual(tca, e.Action, "A1");
        Assert.AreEqual(tp, e.TabPage, "A2");
        Assert.AreEqual(index, e.TabPageIndex, "A3");
        Assert.AreEqual(true, e.Cancel, "A4");
    }

    [Test]
    public void TestTabControlEventArgs ()
    {
        var tca = TabControlAction.Selected;
        var tp = new TabPage ("HI!");
        var index = 477;

        var e = new TabControlEventArgs (tp, index, tca);

        Assert.AreEqual (tca, e.Action, "A1");
        Assert.AreEqual (tp, e.TabPage, "A2");
        Assert.AreEqual (index, e.TabPageIndex, "A3");
    }
		
    [Test]
    public void TestTableLayoutCellPaintEventArgs()
    {
        var bounds = new Rectangle(0, 0, 100, 200);
        var clip = new Rectangle(50, 50, 50, 50);
        var col = 54;
        var row = 77;
        var b = new Bitmap(100, 100);
        var g = Graphics.FromImage(b);
			
        var e = new TableLayoutCellPaintEventArgs(g, clip, bounds, col, row);
			
        Assert.AreEqual(bounds, e.CellBounds, "A1");
        Assert.AreEqual(col, e.Column, "A2");
        Assert.AreEqual(row, e.Row, "A3");
        Assert.AreEqual(g, e.Graphics, "A4");
        Assert.AreEqual(clip, e.ClipRectangle, "A5");
    }
    
    [Test]
    public void TestTreeNodeMouseClickEventArgs()
    {
        var tn = new TreeNode("HI");
        var clicks = 4;
        var x = 75;
        var y = 34;
        var mb = MouseButtons.Right;
			
        var e = new TreeNodeMouseClickEventArgs(tn, mb, clicks, x, y);
			
        Assert.AreEqual(tn, e.Node, "A1");
        Assert.AreEqual(clicks, e.Clicks, "A2");
        Assert.AreEqual(x, e.X, "A3");
        Assert.AreEqual(y, e.Y, "A4");
        Assert.AreEqual(mb, e.Button, "A5");
    }

    [Test]
    public void TestTreeNodeMouseHoverEventArgs ()
    {
        var tn = new TreeNode ("HI");

        var e = new TreeNodeMouseHoverEventArgs (tn);

        Assert.AreEqual (tn, e.Node, "A1");
    }

    [Test]
    public void TestTypeValidationEventArgs()
    {
        var valid = true;
        var message = "This is a test.";
        var rv = (object) "MyObject";
        var vt = typeof(int);
			
        var e = new TypeValidationEventArgs (vt, valid, rv, message);
			
        Assert.AreEqual(valid, e.IsValidInput, "A1");
        Assert.AreEqual(message, e.Message, "A2");
        Assert.AreEqual(rv, e.ReturnValue, "A3");
        Assert.AreEqual(vt, e.ValidatingType, "A4");
        Assert.AreEqual(false, e.Cancel, "A5");
			
        e.Cancel = true;
			
        Assert.AreEqual(true, e.Cancel, "A6");
    }
		
    [Test]
    public void TestWebBrowserDocumentCompletedEventArgs()
    {
        var url = new Uri("http://www.example.com/");
			
        var e = new WebBrowserDocumentCompletedEventArgs(url);
			
        Assert.AreEqual(url, e.Url, "A1");
    }

    [Test]
    public void TestWebBrowserNavigatedEventArgs ()
    {
        var url = new Uri ("http://www.example.com/");

        var e = new WebBrowserNavigatedEventArgs (url);

        Assert.AreEqual (url, e.Url, "A1");
    }

    [Test]
    public void TestWebBrowserNavigatingEventArgs ()
    {
        var url = new Uri ("http://www.example.com/");
        var frame = "TOP";

        var e = new WebBrowserNavigatingEventArgs (url, frame);

        Assert.AreEqual (url, e.Url, "A1");
        Assert.AreEqual(frame, e.TargetFrameName, "A2");
    }

    [Test]
    public void TestWebBrowserProgressChangedEventArgs ()
    {
        long current = 3000;
        long max = 5000;

        var e = new WebBrowserProgressChangedEventArgs (current, max);

        Assert.AreEqual (current, e.CurrentProgress, "A1");
        Assert.AreEqual (max, e.MaximumProgress, "A2");
    }

}