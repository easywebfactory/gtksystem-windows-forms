//
// ToolStripItemTests.cs
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

using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Image = System.Drawing.Image;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class ToolStripItemTests : TestHelper
{
    [Test]
    public void Constructor ()
    {
        ToolStripItem tsi = new NullToolStripItem ();

        Assert.AreEqual (false, tsi.AutoToolTip, "A5");
        Assert.AreEqual (GtkSystemColors.Control, tsi.BackColor, "A7");
        Assert.AreEqual (null, tsi.BackgroundImage, "A8");
        Assert.AreEqual (ImageLayout.Tile, tsi.BackgroundImageLayout, "A9");
        Assert.AreEqual (new Rectangle (0,0,23,23), tsi.Bounds, "A10");
        Assert.AreEqual (ToolStripItemDisplayStyle.ImageAndText, tsi.DisplayStyle, "A13");
        Assert.AreEqual (true, tsi.Enabled, "A16");
        //Assert.AreEqual (new Font ("Tahoma", 8.25f), tsi.Font, "A17");
        Assert.AreEqual (GtkSystemColors.ControlText, tsi.ForeColor, "A18");
        Assert.AreEqual (23, tsi.Height, "A19");
        Assert.AreEqual (null, tsi.Image, "A20");
        Assert.AreEqual (ContentAlignment.MiddleCenter, tsi.ImageAlign, "A21");
        Assert.AreEqual (-1, tsi.ImageIndex, "A22");
        Assert.AreEqual (string.Empty, tsi.ImageKey, "A22-1");
        Assert.AreEqual (ToolStripItemImageScaling.SizeToFit, tsi.ImageScaling, "A23");
        Assert.AreEqual (Color.Empty, tsi.ImageTransparentColor, "A24");
        Assert.AreEqual (MergeAction.Append, tsi.MergeAction, "A29");
        Assert.AreEqual (-1, tsi.MergeIndex, "A30");
        Assert.AreEqual (string.Empty, tsi.Name, "A31");
        Assert.AreEqual (ToolStripItemOverflow.AsNeeded, tsi.Overflow, "A32");
        Assert.AreEqual (null, tsi.Owner, "A33");
        Assert.AreEqual (null, tsi.OwnerItem, "A34");
        Assert.AreEqual (new Padding(0), tsi.Padding, "A35");
        Assert.AreEqual (ToolStripItemPlacement.None, tsi.Placement, "A36");
        Assert.AreEqual (false, tsi.Pressed, "A37");
        Assert.AreEqual (RightToLeft.Inherit, tsi.RightToLeft, "A38");
        Assert.AreEqual (false, tsi.RightToLeftAutoMirrorImage, "A39");
        Assert.AreEqual (new Size (23,23), tsi.Size, "A41");
        Assert.AreEqual (null, tsi.Tag, "A42");
        Assert.AreEqual (string.Empty, tsi.Text, "A43");
        Assert.AreEqual (ContentAlignment.MiddleCenter, tsi.TextAlign, "A44");
        Assert.AreEqual (ToolStripTextDirection.Horizontal, tsi.TextDirection, "A45");
        Assert.AreEqual (TextImageRelation.ImageBeforeText, tsi.TextImageRelation, "A46");
        Assert.AreEqual (null, tsi.ToolTipText, "A47");
        Assert.AreEqual (23, tsi.Width, "A49");

    }
		
    [Test]
    public void PropertyAutoToolTip ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.AutoToolTip = true;
        Assert.AreEqual (true, tsi.AutoToolTip, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.AutoToolTip = true;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyBackColor ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.BackColor = Color.BurlyWood;
        Assert.AreEqual (Color.BurlyWood, tsi.BackColor, "B1");
        Assert.AreEqual ("BackColorChanged", ew.ToString (), "B2");

        ew.Clear ();
        tsi.BackColor = Color.BurlyWood;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyBackgroundImage ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        Image i = new Bitmap (1, 1);
        tsi.BackgroundImage = i;
        Assert.AreSame (i, tsi.BackgroundImage, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.BackgroundImage = i;
        Assert.AreSame (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyBackgroundImageLayout ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.BackgroundImageLayout = ImageLayout.Zoom;
        Assert.AreEqual (ImageLayout.Zoom, tsi.BackgroundImageLayout, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.BackgroundImageLayout = ImageLayout.Zoom;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyDisplayStyle ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.DisplayStyle = ToolStripItemDisplayStyle.Image;
        Assert.AreEqual (ToolStripItemDisplayStyle.Image, tsi.DisplayStyle, "B1");
        Assert.AreEqual ("DisplayStyleChanged", ew.ToString (), "B2");

        ew.Clear ();
        tsi.DisplayStyle = ToolStripItemDisplayStyle.Image;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyEnabled ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.Enabled = false;
        Assert.AreEqual (false, tsi.Enabled, "B1");
        Assert.AreEqual ("EnabledChanged", ew.ToString (), "B2");

        ew.Clear ();
        tsi.Enabled = false;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyFont ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        var f = new Font ("Arial", 12);

        tsi.Font = f;
        Assert.AreSame (f, tsi.Font, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Font = f;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyForeColor ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.ForeColor = Color.BurlyWood;
        Assert.AreEqual (Color.BurlyWood, tsi.ForeColor, "B1");
        Assert.AreEqual ("ForeColorChanged", ew.ToString (), "B2");

        ew.Clear ();
        tsi.ForeColor = Color.BurlyWood;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyHeight ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.Height = 42;
        Assert.AreEqual (42, tsi.Height, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Height = 42;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyImage ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        Image i = new Bitmap (1, 1);
        tsi.Image = i;
        Assert.AreSame (i, tsi.Image, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Image = i;
        Assert.AreSame (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyImageAlign ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.ImageAlign = ContentAlignment.TopRight;
        Assert.AreEqual (ContentAlignment.TopRight, tsi.ImageAlign, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.ImageAlign = ContentAlignment.TopRight;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyImageAlignIEAE ()
    {
        Assert.Throws<InvalidEnumArgumentException>(() =>
        {
            ToolStripItem tsi = new NullToolStripItem();
            tsi.ImageAlign = (ContentAlignment)42;
        });
    }

    [Test]
    public void PropertyImageIndex ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.ImageIndex = 42;
        Assert.AreEqual (42, tsi.ImageIndex, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.ImageIndex = 42;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyImageIndexAE ()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            ToolStripItem tsi = new NullToolStripItem();
            tsi.ImageIndex = -2;
        });
    }

    [Test]
    public void PropertyImageKey ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.ImageKey = "open";
        Assert.AreEqual ("open", tsi.ImageKey, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.ImageKey = "open";
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyImageScaling ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.ImageScaling = ToolStripItemImageScaling.None;
        Assert.AreEqual (ToolStripItemImageScaling.None, tsi.ImageScaling, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.ImageScaling = ToolStripItemImageScaling.None;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyImageTransparentColor ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.ImageTransparentColor = Color.BurlyWood;
        Assert.AreEqual (Color.BurlyWood, tsi.ImageTransparentColor, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.ImageTransparentColor = Color.BurlyWood;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyMergeAction ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.MergeAction = MergeAction.Replace;
        Assert.AreEqual (MergeAction.Replace, tsi.MergeAction, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.MergeAction = MergeAction.Replace;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyMergeActionIEAE ()
    {
        Assert.Throws<InvalidEnumArgumentException>(() =>
        {
            ToolStripItem tsi = new NullToolStripItem();
            tsi.MergeAction = (MergeAction)42;
        });
    }

    [Test]
    public void PropertyMergeIndex ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.MergeIndex = 42;
        Assert.AreEqual (42, tsi.MergeIndex, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.MergeIndex = 42;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyName ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.Name = "MyName";
        Assert.AreEqual ("MyName", tsi.Name, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Name = "MyName";
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyOverflow ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.Overflow = ToolStripItemOverflow.Never;
        Assert.AreEqual (ToolStripItemOverflow.Never, tsi.Overflow, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Overflow = ToolStripItemOverflow.Never;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyOverflowIEAE ()
    {
        Assert.Throws<InvalidEnumArgumentException>(() =>
        {
            ToolStripItem tsi = new NullToolStripItem();
            tsi.Overflow = (ToolStripItemOverflow)42;
        });
    }

    [Test]
    public void PropertyOwner ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        var ts = new ToolStrip ();
        tsi.Owner = ts;
        Assert.AreSame (ts, tsi.Owner, "B1");
        Assert.AreEqual ("OwnerChanged", ew.ToString (), "B2");

        ew.Clear ();
        tsi.Owner = ts;
        Assert.AreSame (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyPadding ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.Padding = new Padding (6);
        Assert.AreEqual (new Padding (6), tsi.Padding, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Padding = new Padding (6);
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyRightToLeft ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.RightToLeft = RightToLeft.No;
        Assert.AreEqual (RightToLeft.No, tsi.RightToLeft, "B1");
        Assert.AreEqual ("RightToLeftChanged", ew.ToString (), "B2");

        ew.Clear ();
        tsi.RightToLeft = RightToLeft.No;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyRightToLeftAutoMirrorImage ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.RightToLeftAutoMirrorImage = true;
        Assert.AreEqual (true, tsi.RightToLeftAutoMirrorImage, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.RightToLeftAutoMirrorImage = true;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertySize ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.Size = new Size (42, 42);
        Assert.AreEqual (new Size (42, 42), tsi.Size, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Size = new Size (42, 42);
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyTag ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.Tag = "tag";
        Assert.AreSame ("tag", tsi.Tag, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Tag = "tag";
        Assert.AreSame (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyText ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.Text = "Text";
        Assert.AreEqual ("Text", tsi.Text, "B1");
        Assert.AreEqual ("TextChanged", ew.ToString (), "B2");

        ew.Clear ();
        tsi.Text = "Text";
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyTextAlign ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.TextAlign = ContentAlignment.TopRight;
        Assert.AreEqual (ContentAlignment.TopRight, tsi.TextAlign, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.TextAlign = ContentAlignment.TopRight;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyTextAlignIEAE ()
    {
        Assert.Throws<InvalidEnumArgumentException>(() =>
        {
            ToolStripItem tsi = new NullToolStripItem();
            tsi.TextAlign = (ContentAlignment)42;
        });
    }

    [Test]
    public void PropertyTextImageRelation ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.TextImageRelation = TextImageRelation.Overlay;
        Assert.AreEqual (TextImageRelation.Overlay, tsi.TextImageRelation, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.TextImageRelation = TextImageRelation.Overlay;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyToolTipText ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.ToolTipText = "Text";
        Assert.AreEqual ("Text", tsi.ToolTipText, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.ToolTipText = "Text";
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void PropertyWidth ()
    {
        ToolStripItem tsi = new NullToolStripItem ();
        var ew = new EventWatcher (tsi);

        tsi.Width = 42;
        Assert.AreEqual (42, tsi.Width, "B1");
        Assert.AreEqual (string.Empty, ew.ToString (), "B2");

        ew.Clear ();
        tsi.Width = 42;
        Assert.AreEqual (string.Empty, ew.ToString (), "B3");
    }

    [Test]
    public void MethodDispose ()
    {
        var ts = new ToolStrip ();
        var tsi = new NullToolStripItem ();
			
        ts.Items.Add (tsi);
			
        Assert.AreEqual (1, ts.Items.Count, "A2");
        Assert.AreEqual (ts, tsi.Owner, "A3");

        tsi.Dispose ();
        Assert.AreEqual (0, ts.Items.Count, "A5");
        Assert.AreEqual (null, tsi.Owner, "A6");
    }

    [Test]
    public void BehaviorBackColor ()
    {
        var ts = new ToolStrip ();
        ToolStripItem tsi = new NullToolStripItem ();

        ts.Items.Add (tsi);

        Assert.AreEqual (GtkSystemColors.Control, ts.BackColor, "C1");
        Assert.AreEqual (GtkSystemColors.Control, tsi.BackColor, "C2");

        ts.BackColor = Color.BlueViolet;

        Assert.AreEqual (Color.BlueViolet, ts.BackColor, "C3");
        Assert.AreEqual (GtkSystemColors.Control, tsi.BackColor, "C4");

        tsi.BackColor = Color.Snow;

        Assert.AreEqual (Color.BlueViolet, ts.BackColor, "C5");
        Assert.AreEqual (Color.Snow, tsi.BackColor, "C6");

    }

    [Test]
    public void BehaviorEnabled ()
    {
        var ts = new ToolStrip ();
        ToolStripItem tsi = new NullToolStripItem ();

        ts.Items.Add (tsi);
			
        Assert.AreEqual (true, ts.Enabled, "A1");
        Assert.AreEqual (true, tsi.Enabled, "A2");
			
        tsi.Enabled = false;

        Assert.AreEqual (true, ts.Enabled, "A3");
        Assert.AreEqual (false, tsi.Enabled, "A4");

        ts.Enabled = false;

        Assert.AreEqual (false, ts.Enabled, "A5");
        Assert.AreEqual (false, tsi.Enabled, "A6");
			
        tsi.Enabled = true;

        Assert.AreEqual (false, ts.Enabled, "A7");
        Assert.AreEqual (false, tsi.Enabled, "A8");
    }
		
    [Test]
    public void BehaviorImageList ()
    {
        // Basically, this shows that whichever of [Image|ImageIndex|ImageKey]
        // is set last resets the others to their default state
        ToolStripItem tsi = new NullToolStripItem ();
			
        var i1 = new Bitmap (16, 16);
        i1.SetPixel (0, 0, Color.Blue);
        var i2 = new Bitmap (16, 16);
        i2.SetPixel (0, 0, Color.Red);
        var i3 = new Bitmap (16, 16);
        i3.SetPixel (0, 0, Color.Green);
			
        Assert.AreEqual (null, tsi.Image, "D1");
        Assert.AreEqual (-1, tsi.ImageIndex, "D2");
        Assert.AreEqual (string.Empty, tsi.ImageKey, "D3");
			
        var il = new ImageList ();
        il.Images.Add ("i2", i2);
        il.Images.Add ("i3", i3);
			
        var ts = new ToolStrip ();
			
        ts.Items.Add (tsi);
	
        tsi.ImageKey = "i3";
        Assert.AreEqual (-1, tsi.ImageIndex, "D4");
        Assert.AreEqual ("i3", tsi.ImageKey, "D5");
        Assert.AreEqual (i3.GetPixel (0, 0), (tsi.Image as Bitmap).GetPixel (0, 0), "D6");

        tsi.ImageIndex = 0;
        Assert.AreEqual (0, tsi.ImageIndex, "D7");
        Assert.AreEqual (string.Empty, tsi.ImageKey, "D8");
        Assert.AreEqual (i2.GetPixel (0, 0), (tsi.Image as Bitmap).GetPixel (0, 0), "D9");

        tsi.Image = i1;
        Assert.AreEqual (-1, tsi.ImageIndex, "D10");
        Assert.AreEqual (string.Empty, tsi.ImageKey, "D11");
        Assert.AreEqual (i1.GetPixel (0, 0), (tsi.Image as Bitmap).GetPixel (0, 0), "D12");
			
        tsi.Image = null;
        Assert.AreEqual (null, tsi.Image, "D13");
        Assert.AreEqual (-1, tsi.ImageIndex, "D14");
        Assert.AreEqual (string.Empty, tsi.ImageKey, "D15");
			
        // Also, Image is not cached, changing the underlying ImageList image is reflected
        tsi.ImageIndex = 0;
        il.Images[0] = i1;
        Assert.AreEqual (i1.GetPixel (0, 0), (tsi.Image as Bitmap).GetPixel (0, 0), "D16");
    }
		
    [Test]	// This should not crash
    public void BehaviorImageListBadIndex ()
    {
        var f = new Form ();
        var ts = new ToolStrip ();
        ts.Items.Add ("Hey").ImageIndex = 3;

        var i = ts.Items[0].Image;
			
        f.Controls.Add (ts);

        f.Show ();
        f.Dispose ();
    }
		
    private class EventWatcher
    {
        private string events = string.Empty;
			
        public EventWatcher (ToolStripItem tsi)
        {
            tsi.Click += delegate (Object _, EventArgs _) { events += ("Click;"); };
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
		
    private class NullToolStripItem : ToolStripItem
    {
        public NullToolStripItem () : base () {}
        public NullToolStripItem (string text, Image image, EventHandler onClick) : base (text, image, onClick) { }
        public NullToolStripItem (string text, Image image, EventHandler onClick, string name) : base (text, image, onClick, name) { }
    }
		
}