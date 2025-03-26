//
// PropertyGridTest.cs: Test cases for PropertyGrid.
//
// Author:
//   Gert Driesen (drieseng@users.sourceforge.net)
//
// (C) 2006 Novell, Inc. (http://www.novell.com)
//

using System.Windows.Forms;
using CategoryAttribute = NUnit.Framework.CategoryAttribute;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class PropertyGridTest : TestHelper
{
    [Test]
    public void SelectedObject ()
    {
        var pg = new PropertyGrid ();
        var button1 = new Button ();
        Assert.IsNull (pg.SelectedObject, "#A1");
        Assert.IsNotNull (pg.SelectedObjects, "#A2");
        Assert.AreEqual (0, pg.SelectedObjects.Length, "#A3");
        pg.SelectedObject = button1;
        Assert.IsNotNull (pg.SelectedObject, "#B1");
        Assert.AreSame (button1, pg.SelectedObject, "#B2");
        Assert.IsNotNull (pg.SelectedObjects, "#B3");
        Assert.AreEqual (1, pg.SelectedObjects.Length, "#B4");
        Assert.AreSame (button1, pg.SelectedObjects [0], "#B5");
        Assert.IsNotNull (pg.SelectedGridItem, "#B6");
    }

    [Test] // bug #81796
    public void SelectedObject_NoProperties ()
    {
        var propertyGrid = new PropertyGrid ();
        propertyGrid.SelectedObject = new Button ();
        propertyGrid.SelectedObject = new object ();
        propertyGrid.SelectedObject = new Button ();
    }

    [Test]
    public void SelectedObject_Null ()
    {
        var pg = new PropertyGrid ();
        Assert.IsNull (pg.SelectedObject, "#A1");
        Assert.IsNotNull (pg.SelectedObjects, "#A2");
        Assert.AreEqual (0, pg.SelectedObjects.Length, "#A3");
        pg.SelectedObject = null;
        Assert.IsNull (pg.SelectedObject, "#B1");
        Assert.IsNotNull (pg.SelectedObjects, "#B2");
        Assert.AreEqual (0, pg.SelectedObjects.Length, "#B3");
    }

    [Test]
    public void SelectedGridItem_Null ()
    {
        var pg = new PropertyGrid ();
        pg.SelectedObject = new TextBox ();
        Assert.IsNotNull (pg.SelectedGridItem, "#1");
        try {
            pg.SelectedGridItem = null;
            Assert.Fail ("#2");
        } catch (ArgumentException ex) {
            // GridItem specified to PropertyGrid.SelectedGridItem must be
            // a valid GridItem
            Assert.AreEqual (typeof (ArgumentException), ex.GetType (), "#3");
            Assert.IsNull (ex.InnerException, "#4");
            Assert.IsNotNull (ex.Message, "#5");
            Assert.IsNull (ex.ParamName, "#6");
        }
    }

    [Test] // bug #79615
    public void SelectedObjects_Multiple ()
    {
        var button1 = new Button ();
        var button2 = new Button ();

        var pg = new PropertyGrid ();
        pg.SelectedObjects = new object [] { button1, button2 };
        Assert.IsNotNull (pg.SelectedObjects, "#1");
        Assert.AreEqual (2, pg.SelectedObjects.Length, "#2");
        Assert.AreSame (button1, pg.SelectedObjects [0], "#3");
        Assert.AreSame (button2, pg.SelectedObjects [1], "#4");
        Assert.IsNotNull (pg.SelectedObject, "#5");
        Assert.AreSame (button1, pg.SelectedObject, "#6");
    }

    [Test]
    public void SelectedObjects_Null ()
    {
        var pg = new PropertyGrid ();
        var button1 = new Button ();
        pg.SelectedObjects = new object [] { button1 };
        Assert.IsNotNull (pg.SelectedObjects, "#A1");
        Assert.AreEqual (1, pg.SelectedObjects.Length, "#A2");
        Assert.AreSame (button1, pg.SelectedObjects [0], "#A3");
        Assert.AreSame (button1, pg.SelectedObject, "#A4");
        pg.SelectedObjects = null;
        Assert.IsNotNull (pg.SelectedObjects, "#B1");
        Assert.AreEqual (0, pg.SelectedObjects.Length, "#B2");
        Assert.IsNull (pg.SelectedObject, "#B3");
    }

    [Test]
    public void SelectedObjects_Null_Item ()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var pg = new PropertyGrid();
            var button1 = new Button();
            pg.SelectedObjects = new object[] { button1, null };
        });
    }

    [Test]
    [Category ("NotWorking")]
    public void PropertyGrid_MergedTest ()
    {
        var pg = new PropertyGrid ();
        pg.SelectedObjects = new object[] { new Button (), new Label () };

        Assert.IsNotNull (pg.SelectedGridItem, "1");
        Assert.AreEqual ("Accessibility", pg.SelectedGridItem.Label, "2");
        Assert.AreEqual (GridItemType.Category, pg.SelectedGridItem.GridItemType, "3");
    }

    [Test]
    [Category ("NotWorking")]
    public void PropertyGrid_MergedRootTest ()
    {
        object[] selected_objects = new object[] { new Button (), new Label () };
        var pg = new PropertyGrid ();

        pg.SelectedObjects = selected_objects;

        Assert.IsNotNull (pg.SelectedGridItem.Parent, "1");
        Assert.AreEqual ("System.Object[]", pg.SelectedGridItem.Parent.Label, "2");
        Assert.AreEqual (GridItemType.Root, pg.SelectedGridItem.Parent.GridItemType, "3");

        Assert.AreEqual (selected_objects, pg.SelectedGridItem.Parent.Value, "4");

        Assert.IsNull (pg.SelectedGridItem.Parent.Parent, "5");
    }

    class ArrayTest_object
    {
        readonly int[] array;
        public ArrayTest_object ()
        {
            array = new int[10];
            for (var i = 0; i < array.Length; i ++)
                array[i] = array.Length - i;
        }
        public int[] Array {
            get { return array; }
        }
    }

    [Test]
    public void PropertyGrid_ArrayTest ()
    {
        var pg = new PropertyGrid ();

        pg.SelectedObject = new ArrayTest_object ();

        // selected object
        Assert.AreEqual ("Array", pg.SelectedGridItem.Label, "1");
        Assert.IsTrue (pg.SelectedGridItem.Value is Array, "2");
        Assert.AreEqual (10, pg.SelectedGridItem.GridItems.Count, "3");
        Assert.AreEqual (GridItemType.Property, pg.SelectedGridItem.GridItemType, "4");
    }

    [Test]
    public void PropertyGrid_ArrayParentTest ()
    {
        var pg = new PropertyGrid ();

        pg.SelectedObject = new ArrayTest_object ();

        // parent
        Assert.IsNotNull (pg.SelectedGridItem.Parent, "1");
        Assert.AreEqual ("Misc", pg.SelectedGridItem.Parent.Label, "2");
        Assert.AreEqual (GridItemType.Category, pg.SelectedGridItem.Parent.GridItemType, "3");
        Assert.AreEqual (1, pg.SelectedGridItem.Parent.GridItems.Count, "4");
    }

    [Test]
    public void PropertyGrid_ArrayRootTest ()
    {
        var obj = new ArrayTest_object ();
        var pg = new PropertyGrid ();

        pg.SelectedObject = obj;

        // grandparent
        Assert.IsNotNull (pg.SelectedGridItem.Parent.Parent, "1");
        Assert.AreEqual (typeof(ArrayTest_object).ToString(), pg.SelectedGridItem.Parent.Parent.Label, "2");
        Assert.AreEqual (GridItemType.Root, pg.SelectedGridItem.Parent.Parent.GridItemType, "3");
        Assert.AreEqual (1, pg.SelectedGridItem.Parent.Parent.GridItems.Count, "4");
        Assert.AreEqual (obj, pg.SelectedGridItem.Parent.Parent.Value, "5");

        Assert.IsNull (pg.SelectedGridItem.Parent.Parent.Parent, "6");
    }

    [Test]
    public void PropertyGrid_ArrayChildrenTest ()
    {
        var pg = new PropertyGrid ();

        pg.SelectedObject = new ArrayTest_object ();

        // children
        Assert.AreEqual ("[0]", pg.SelectedGridItem.GridItems[0].Label, "1");
        Assert.AreEqual (GridItemType.Property, pg.SelectedGridItem.GridItems[0].GridItemType, "2");
        Assert.AreEqual (10, pg.SelectedGridItem.GridItems[0].Value, "3");
        Assert.AreEqual (0, pg.SelectedGridItem.GridItems[0].GridItems.Count, "4");
    }

    [Test]
    public void PropertyGrid_ItemSelectTest ()
    {
        var pg = new PropertyGrid ();

        pg.SelectedObject = new ArrayTest_object ();

        // the selected grid item is the "Array" property item.
        var array_item = pg.SelectedGridItem;
        var misc_item = array_item.Parent;
        var root_item = misc_item.Parent;

        Assert.AreEqual (array_item, pg.SelectedGridItem, "1");

        Assert.IsTrue (misc_item.Select (), "2");
        Assert.AreEqual (misc_item, pg.SelectedGridItem, "3");

        Assert.IsTrue (array_item.Select (), "4");
        Assert.AreEqual (array_item, pg.SelectedGridItem, "5");

        Assert.IsFalse (root_item.Select (), "6");
        Assert.AreEqual (array_item, pg.SelectedGridItem, "7");
    }
}