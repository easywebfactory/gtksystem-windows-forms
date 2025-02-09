//
// MenuStripTest.cs
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

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class MenuStripTest : TestHelper
{
    [Test]
    public void Constructor ()
    {
        MenuStrip ms = new MenuStrip ();

        Assert.AreEqual (false, ms.CanSelect, "A0");
        Assert.AreEqual (ToolStripLayoutStyle.HorizontalStackWithOverflow, ms.LayoutStyle, "A6");
			
        Assert.AreEqual ("System.Windows.Forms.MenuStrip+MenuStripAccessibleObject", ms.AccessibilityObject.GetType ().ToString (), "A7");
    }

    [Test]
    public void ProtectedProperties ()
    {
        ExposeProtectedProperties epp = new ExposeProtectedProperties ();

        Assert.AreEqual (new Padding (6, 2, 0, 2), epp.DefaultPadding, "C2");
        Assert.AreEqual (new Size (200, 24), epp.DefaultSize, "C4");
    }

    [Test]
    public void PropertyShowItemToolTips ()
    {
        StatusStrip ts = new StatusStrip ();

        ts.ShowItemToolTips = true;
        Assert.AreEqual (true, ts.ShowItemToolTips, "B1");
    }
		
    [Test]
    public void PropertyStretch ()
    {
        StatusStrip ts = new StatusStrip ();

        ts.Stretch = false;
        Assert.AreEqual (false, ts.Stretch, "B1");
    }

    private class ExposeProtectedProperties : MenuStrip
    {
        public new Padding DefaultPadding { get { return base.DefaultPadding; } }
        public new Size DefaultSize { get { return base.DefaultSize; } }

    }
}