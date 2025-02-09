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
// Copyright (c) 2007 Novell, Inc. (http://www.novell.com)
//
// Author:
//	Rolf Bjarne Kvinge  (RKvinge@novell.com)
//

using System.Windows.Forms;
using GtkTests.System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class DataGridViewTextBoxColumnTest : TestHelper
{
    [Test]
    public void InitialValues ()
    {
        var col = new DataGridViewTextBoxColumn ();

        Assert.AreEqual ("DataGridViewTextBoxColumn { Name=, Index=-1 }", col.ToString (), "T3");
        Assert.AreEqual ("DataGridViewTextBoxColumn", col.GetType ().Name, "G2");

        Assert.AreEqual (DataGridViewAutoSizeColumnMode.NotSet, col.AutoSizeMode, "#A col.AutoSizeMode");
        Assert.IsNotNull (col.CellTemplate, "#A col.CellTemplate");
        Assert.IsNotNull (col.CellType, "#A col.CellType");
        Assert.IsNull (col.ContextMenuStrip, "#A col.ContextMenuStrip");
        Assert.IsNull (col.DataGridView, "#A col.DataGridView");
        Assert.AreEqual (@"", col.DataPropertyName, "#A col.DataPropertyName");
        Assert.IsNotNull (col.DefaultCellStyle, "#A col.DefaultCellStyle");
        Assert.AreEqual (-1, col.DisplayIndex, "#A col.DisplayIndex");
        Assert.AreEqual (0, col.DividerWidth, "#A col.DividerWidth");
        Assert.AreEqual (100, col.FillWeight, "#A col.FillWeight");
        Assert.AreEqual (false, col.Frozen, "#A col.Frozen");
        Assert.IsNotNull (col.HeaderCell, "#A col.HeaderCell");
        Assert.AreEqual (@"", col.HeaderText, "#A col.HeaderText");
        Assert.AreEqual (-1, col.Index, "#A col.Index");
        Assert.AreEqual (DataGridViewAutoSizeColumnMode.NotSet, col.InheritedAutoSizeMode, "#A col.InheritedAutoSizeMode");
        Assert.IsNotNull (col.InheritedStyle, "#A col.InheritedStyle");
        Assert.AreEqual (false, col.IsDataBound, "#A col.IsDataBound");
        Assert.AreEqual (5, col.MinimumWidth, "#A col.MinimumWidth");
        Assert.AreEqual (@"", col.Name, "#A col.Name");
        Assert.AreEqual (false, col.ReadOnly, "#A col.ReadOnly");
        Assert.AreEqual (DataGridViewTriState.NotSet, col.Resizable, "#A col.Resizable");
        Assert.IsNull (col.Site, "#A col.Site");
        Assert.AreEqual (DataGridViewColumnSortMode.Automatic, col.SortMode, "#A col.SortMode");
        Assert.AreEqual (DataGridViewElementStates.Visible, col.State, "#A col.State");
        Assert.AreEqual (@"", col.ToolTipText, "#A col.ToolTipText");
        Assert.IsNull (col.ValueType, "#A col.ValueType");
        Assert.AreEqual (true, col.Visible, "#A col.Visible");
        Assert.AreEqual (100, col.Width, "#A col.Width");
    }
}