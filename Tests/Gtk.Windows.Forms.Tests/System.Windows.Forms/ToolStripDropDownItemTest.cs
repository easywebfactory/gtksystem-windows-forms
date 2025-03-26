//
// ToolStripDropDownItemTests.cs
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
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Carlos Alberto Cortez <calberto.cortez@gmail.com>
//

using System.Drawing;
using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class ToolStripDropDownItemTest
{
    [Test]
    public void FontTest ()
    {
        var dropdown_item = new ToolStripMenuItem ();
        var tool_strip = new ToolStrip ();
        tool_strip.Items.Add (dropdown_item);

        Assert.AreEqual (tool_strip.Font, dropdown_item.Font, "#A1");

        tool_strip.Font = new Font (tool_strip.Font, FontStyle.Bold);
        Assert.AreEqual (tool_strip.Font, dropdown_item.Font, "#B1");


        tool_strip.Font = new Font (tool_strip.Font, FontStyle.Italic);
        Assert.AreEqual (tool_strip.Font, dropdown_item.Font, "#D1");
    }
}