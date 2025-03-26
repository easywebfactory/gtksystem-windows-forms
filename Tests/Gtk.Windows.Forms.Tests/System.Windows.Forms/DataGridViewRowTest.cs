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
// Copyright (c) 2005 Novell, Inc. (http://www.novell.com)
//
// Author:
//	Pedro Martínez Juliá <pedromj@gmail.com>
//

using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class DataGridViewRowTest : TestHelper
{

    [Test]
    public void TestDefaultValues()
    {
    }

    [Test]
    public void TestVisibleInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var grid = new DataGridView();
            var row = new DataGridViewRow();
            grid.Rows.Add(row);
            row.Visible = false;
        });
    }

    [Test]
    public void Height()
    {
        var row = new DataGridViewRow();
        Assert.IsTrue(row.Height > 5, "#1");
        row.Height = 70;
        Assert.AreEqual(70, row.Height, "#2");
        row.Height = 40;
        Assert.AreEqual(40, row.Height, "#3");
    }

    [Test]
    public void Height_SetHeightLessThanMinHeightSilentlySetsToMinHeight()
    {
        var row = new DataGridViewRow();
        // Setup
        row.MinimumHeight = 5;

        // Execute
        row.Height = 2;

        // Verify
        Assert.AreEqual(5, row.Height, "Height didn't get set to MinimumHeight");
    }

    [Test]
    public void MinimumHeight_DefaultValues()
    {
        var row = new DataGridViewRow();
        Assert.IsTrue(row.MinimumHeight > 0, "#A1");
        Assert.IsTrue(row.Height >= row.MinimumHeight, "#A2");
    }

    [Test]
    public void MinimumHeight_SetValues()
    {
        var row = new DataGridViewRow();
        row.MinimumHeight = 40;
        row.Height = 50;
        Assert.AreEqual(40, row.MinimumHeight, "#B1");
        Assert.AreEqual(50, row.Height, "#B2");
    }

    [Test]
    public void MinimumHeight_IncreaseMinHeightChangesHeight()
    {
        var row = new DataGridViewRow();
        row.MinimumHeight = 20;
        row.Height = 20;
        Assert.AreEqual(20, row.MinimumHeight, "#C1");
        Assert.AreEqual(20, row.Height, "#C2");
        row.MinimumHeight = 40;
        Assert.AreEqual(40, row.MinimumHeight, "#D1");
        Assert.AreEqual(40, row.Height, "#D2");
    }

    [Test]
    public void MinimumHeight_SettingToLessThan2ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var row = new DataGridViewRow();
            // We expect the next line to throw an ArgumentOutOfRangeException
            row.MinimumHeight = 1;
        });
    }

    [Test]
    [Category("NotWorking")]	// DGVComboBox not implemented
    public void AddRow_Changes()
    {
        using var dgv = new DataGridView();
        DataGridViewColumn col = new DataGridViewComboBoxColumn();
        var row = new DataGridViewRow();
        DataGridViewCell cell = new DataGridViewComboBoxCell();

        Assert.IsNotNull(row.AccessibilityObject, "#A row.AccessibilityObject");
        Assert.IsNotNull(row.Cells, "#A row.Cells");
        Assert.IsNull(row.ContextMenuStrip, "#A row.ContextMenuStrip");
        Assert.IsNull(row.DataBoundItem, "#A row.DataBoundItem");
        Assert.IsNull(row.DataGridView, "#A row.DataGridView");
        Assert.IsNotNull(row.DefaultCellStyle, "#A row.DefaultCellStyle");
        Assert.AreEqual(false, row.Displayed, "#A row.Displayed");
        Assert.AreEqual(0, row.DividerHeight, "#A row.DividerHeight");
        Assert.AreEqual(@"", row.ErrorText, "#A row.ErrorText");
        Assert.AreEqual(false, row.Frozen, "#A row.Frozen");
        Assert.IsNotNull(row.HeaderCell, "#A row.HeaderCell");
        // DPI Dependent? // Assert.AreEqual (22, row.Height, "#A row.Height");
        Assert.AreEqual(-1, row.Index, "#A row.Index");
        try
        {
            object zxf = row.InheritedStyle;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#A row.InheritedStyle");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the InheritedStyle property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#A row.InheritedStyle");
        }
        Assert.AreEqual(false, row.IsNewRow, "#A row.IsNewRow");
        Assert.AreEqual(3, row.MinimumHeight, "#A row.MinimumHeight");
        Assert.AreEqual(false, row.ReadOnly, "#A row.ReadOnly");
        Assert.AreEqual(DataGridViewTriState.NotSet, row.Resizable, "#A row.Resizable");
        Assert.AreEqual(false, row.Selected, "#A row.Selected");
        Assert.AreEqual(DataGridViewElementStates.Visible, row.State, "#A row.State");
        Assert.AreEqual(true, row.Visible, "#A row.Visible");

        row.Cells.Add(cell);

        Assert.IsNotNull(row.AccessibilityObject, "#B row.AccessibilityObject");
        Assert.IsNotNull(row.Cells, "#B row.Cells");
        Assert.IsNull(row.ContextMenuStrip, "#B row.ContextMenuStrip");
        Assert.IsNull(row.DataBoundItem, "#B row.DataBoundItem");
        Assert.IsNull(row.DataGridView, "#B row.DataGridView");
        Assert.IsNotNull(row.DefaultCellStyle, "#B row.DefaultCellStyle");
        Assert.AreEqual(false, row.Displayed, "#B row.Displayed");
        Assert.AreEqual(0, row.DividerHeight, "#B row.DividerHeight");
        Assert.AreEqual(@"", row.ErrorText, "#B row.ErrorText");
        Assert.AreEqual(false, row.Frozen, "#B row.Frozen");
        Assert.IsNotNull(row.HeaderCell, "#B row.HeaderCell");
        // DPI Dependent? // Assert.AreEqual (22, row.Height, "#B row.Height");
        Assert.AreEqual(-1, row.Index, "#B row.Index");
        try
        {
            object zxf = row.InheritedStyle;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#B row.InheritedStyle");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the InheritedStyle property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#B row.InheritedStyle");
        }
        Assert.AreEqual(false, row.IsNewRow, "#B row.IsNewRow");
        Assert.AreEqual(3, row.MinimumHeight, "#B row.MinimumHeight");
        Assert.AreEqual(false, row.ReadOnly, "#B row.ReadOnly");
        Assert.AreEqual(DataGridViewTriState.NotSet, row.Resizable, "#B row.Resizable");
        Assert.AreEqual(false, row.Selected, "#B row.Selected");
        Assert.AreEqual(DataGridViewElementStates.Visible, row.State, "#B row.State");
        Assert.AreEqual(true, row.Visible, "#B row.Visible");

        dgv.Columns.Add(col);

        Assert.IsNotNull(row.AccessibilityObject, "#C row.AccessibilityObject");
        Assert.IsNotNull(row.Cells, "#C row.Cells");
        Assert.IsNull(row.ContextMenuStrip, "#C row.ContextMenuStrip");
        Assert.IsNull(row.DataBoundItem, "#C row.DataBoundItem");
        Assert.IsNull(row.DataGridView, "#C row.DataGridView");
        Assert.IsNotNull(row.DefaultCellStyle, "#C row.DefaultCellStyle");
        Assert.AreEqual(false, row.Displayed, "#C row.Displayed");
        Assert.AreEqual(0, row.DividerHeight, "#C row.DividerHeight");
        Assert.AreEqual(@"", row.ErrorText, "#C row.ErrorText");
        Assert.AreEqual(false, row.Frozen, "#C row.Frozen");
        Assert.IsNotNull(row.HeaderCell, "#C row.HeaderCell");
        // DPI Dependent? // Assert.AreEqual (22, row.Height, "#C row.Height");
        Assert.AreEqual(-1, row.Index, "#C row.Index");
        try
        {
            object zxf = row.InheritedStyle;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#C row.InheritedStyle");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the InheritedStyle property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#C row.InheritedStyle");
        }
        Assert.AreEqual(false, row.IsNewRow, "#C row.IsNewRow");
        Assert.AreEqual(3, row.MinimumHeight, "#C row.MinimumHeight");
        Assert.AreEqual(false, row.ReadOnly, "#C row.ReadOnly");
        Assert.AreEqual(DataGridViewTriState.NotSet, row.Resizable, "#C row.Resizable");
        Assert.AreEqual(false, row.Selected, "#C row.Selected");
        Assert.AreEqual(DataGridViewElementStates.Visible, row.State, "#C row.State");
        Assert.AreEqual(true, row.Visible, "#C row.Visible");

        dgv.Rows.Add(row);

        Assert.IsNotNull(row.AccessibilityObject, "#D row.AccessibilityObject");
        Assert.IsNotNull(row.Cells, "#D row.Cells");
        try
        {
            object zxf = row.ContextMenuStrip;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#D row.ContextMenuStrip");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Operation cannot be performed on a shared row.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#D row.ContextMenuStrip");
        }
        Assert.IsNull(row.DataBoundItem, "#D row.DataBoundItem");
        Assert.IsNotNull(row.DataGridView, "#D row.DataGridView");
        Assert.IsNotNull(row.DefaultCellStyle, "#D row.DefaultCellStyle");
        try
        {
            object zxf = row.Displayed;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#D row.Displayed");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the Displayed property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#D row.Displayed");
        }
        Assert.AreEqual(0, row.DividerHeight, "#D row.DividerHeight");
        try
        {
            object zxf = row.ErrorText;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#D row.ErrorText");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Operation cannot be performed on a shared row.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#D row.ErrorText");
        }
        try
        {
            object zxf = row.Frozen;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#D row.Frozen");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the Frozen property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#D row.Frozen");
        }
        Assert.IsNotNull(row.HeaderCell, "#D row.HeaderCell");
        // DPI Dependent? // Assert.AreEqual (22, row.Height, "#D row.Height");
        Assert.AreEqual(-1, row.Index, "#D row.Index");
        try
        {
            object zxf = row.InheritedStyle;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#D row.InheritedStyle");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the InheritedStyle property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#D row.InheritedStyle");
        }
        Assert.AreEqual(false, row.IsNewRow, "#D row.IsNewRow");
        Assert.AreEqual(3, row.MinimumHeight, "#D row.MinimumHeight");
        try
        {
            object zxf = row.ReadOnly;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#D row.ReadOnly");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the ReadOnly property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#D row.ReadOnly");
        }
        try
        {
            object zxf = row.Resizable;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#D row.Resizable");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the Resizable property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#D row.Resizable");
        }
        try
        {
            object zxf = row.Selected;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#D row.Selected");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the Selected property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#D row.Selected");
        }
        try
        {
            object zxf = row.State;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#D row.State");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the State property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#D row.State");
        }
        try
        {
            object zxf = row.Visible;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#D row.Visible");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the Visible property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#D row.Visible");
        }
    }

    [Test]
    public void InitialValues()
    {
        var row = new DataGridViewRow();

        Assert.IsNotNull(row.AccessibilityObject, "#A row.AccessibilityObject");
        Assert.IsNotNull(row.Cells, "#A row.Cells");
        Assert.IsNull(row.ContextMenuStrip, "#A row.ContextMenuStrip");
        Assert.IsNull(row.DataBoundItem, "#A row.DataBoundItem");
        Assert.IsNull(row.DataGridView, "#A row.DataGridView");
        Assert.IsNotNull(row.DefaultCellStyle, "#A row.DefaultCellStyle");
        Assert.AreEqual(false, row.Displayed, "#A row.Displayed");
        Assert.AreEqual(0, row.DividerHeight, "#A row.DividerHeight");
        Assert.AreEqual(@"", row.ErrorText, "#A row.ErrorText");
        Assert.AreEqual(false, row.Frozen, "#A row.Frozen");
        Assert.IsNotNull(row.HeaderCell, "#A row.HeaderCell");
        // DPI Dependent? // Assert.AreEqual (22, row.Height, "#A row.Height");
        Assert.AreEqual(-1, row.Index, "#A row.Index");
        try
        {
            object zxf = row.InheritedStyle;
            RemoveWarning(zxf);
            Assert.Fail("Expected 'System.InvalidOperationException', but no exception was thrown.", "#A row.InheritedStyle");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual(@"Getting the InheritedStyle property of a shared row is not a valid operation.", ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected 'System.InvalidOperationException', got '" + ex.GetType().FullName + "'.", "#A row.InheritedStyle");
        }
        Assert.AreEqual(false, row.IsNewRow, "#A row.IsNewRow");
        Assert.AreEqual(3, row.MinimumHeight, "#A row.MinimumHeight");
        Assert.AreEqual(false, row.ReadOnly, "#A row.ReadOnly");
        Assert.AreEqual(DataGridViewTriState.NotSet, row.Resizable, "#A row.Resizable");
        Assert.AreEqual(false, row.Selected, "#A row.Selected");
        Assert.AreEqual(DataGridViewElementStates.Visible, row.State, "#A row.State");
        Assert.AreEqual(true, row.Visible, "#A row.Visible");
    }


    private class MockDataGridViewCellCollection : DataGridViewCellCollection
    {
        public MockDataGridViewCellCollection(DataGridViewRow dataGridViewRow) : base(dataGridViewRow)
        {
        }
    }
}