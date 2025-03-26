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
// Copyright (c) 2007 Novell, Inc.
//
// Authors:
//	Chris Toshok	toshok@ximian.com

using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms.DataBinding;

[TestFixture]
public class ControlBindingsCollectionTest  : TestHelper {

    [Test]
    // MS: "This would cause two bindings in the collection to bind to the same property."
    public void DuplicateBindingAdd ()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var c1 = new Control();
            var c2 = new Control();

            c2.DataBindings.Add("Text", c1, "Text");
            c2.DataBindings.Add("Text", c1, "Text");
        });
    }

    [Test]
    public void CtorTest ()
    {
        var item = new BindableToolStripItem ();
        var data_bindings = new ControlBindingsCollection (item);

        Assert.AreEqual (item, data_bindings.BindableComponent, "#A1");
        Assert.AreEqual (null, data_bindings.Control, "#A2");
    }

    [Test]
    public void DefaultDataSourceUpdateModeTest ()
    {
        var c = new Control ();
        var item = new MockItem ("A", -1);

        Assert.AreEqual (DataSourceUpdateMode.OnValidation, c.DataBindings.DefaultDataSourceUpdateMode, "#A1");

        c.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;

        c.DataBindings.Add (new Binding ("Name", item, "Text"));
        var b2 = c.DataBindings.Add ("Text", item, "Text");
        var b3 = c.DataBindings.Add ("Width", item, "Value");
        var b4 = c.DataBindings.Add ("Height", item, "Value", true, DataSourceUpdateMode.Never);
        var b1 = c.DataBindings [0];

        Assert.AreEqual (b1.DataSourceUpdateMode, DataSourceUpdateMode.OnValidation, "#B1");
        Assert.AreEqual (b2.DataSourceUpdateMode, DataSourceUpdateMode.Never, "#B2");
        Assert.AreEqual (b3.DataSourceUpdateMode, DataSourceUpdateMode.Never, "#B3");
        Assert.AreEqual (b4.DataSourceUpdateMode, DataSourceUpdateMode.Never, "#B4");
    }
}