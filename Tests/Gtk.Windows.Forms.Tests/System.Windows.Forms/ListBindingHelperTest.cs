//
// ListBindingHelperTest.cs: Test cases for ListBindingHelper class.
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

// Author:
// 	Carlos Alberto Cortez <calberto.cortez@gmail.com>
//
// (C) 2008 Novell, Inc. (http://www.novell.com)
//

using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class ListBindingHelperTest : TestHelper
{
    [Test]
    public void GetListTest ()
    {
        var lsource = new ListSource (true);
        var stack = new Stack ();
        stack.Push (3);


        // Even if IListSource.ContainsListCollection is false, we return the result of GetList ()
        lsource = new ListSource (false);
        // List (IEnumerable)
        stack.Clear ();
        stack.Push (new SimpleItem (3));
        stack.Push (new SimpleItem (7));

        // Empty IEnumerable - valid property for list item type
        // Since it's empty, it needs to check whether the datamember is
        // a valid value, and thus we need the datasource to also be IList
        // Then we need a parameterized IEnumerable, which returns null.
        // *Observation: if it is empty and it doesn't implement IList,
        // it doesn't have a way to get the properties, and will throw an exc
        var str_coll = new StringCollection ();

        // IEnumerable that returns instances of ICustomTypeDescriptor
        // Use DataTable as source, which returns, when enumerating,
        // instances of DataRowView, which in turn implement ICustomTypeDescriptor
        var table = new DataTable ();
        table.Columns.Add ("Id", typeof (int));
        table.Rows.Add (666);
    }

    internal class ListSource : IListSource
    {
        readonly bool contains_collection;

        public ListSource (bool containsCollection)
        {
            contains_collection = containsCollection;
        }

        public bool ContainsListCollection {
            get {
                return contains_collection;
            }
        }

        public IList GetList ()
        {
            return new SimpleItem [] { new() };
        }
    }

    class SuperContainer
    {
        public ListContainer ListContainer
        {
            get
            {
                return new ListContainer ();
            }
        }
    }

    class ListContainer
    {
        public IList List {
            get {
                return new SimpleItem [0];
            }
        }

        public SimpleItem NonList {
            get {
                return new SimpleItem ();
            }
        }
    }

    class SimpleItem
    {
        int value;

        public SimpleItem ()
        {
        }

        public SimpleItem (int value)
        {
            this.value = value;
        }

        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public override int GetHashCode ()
        {
            return base.GetHashCode ();
        }

        public override bool Equals (object obj)
        {
            return value == ((SimpleItem)obj).value;
        }
    }

    // useless class that help us with a simple enumerator with a null Current property
    // and implementing IList to let the ListBindingHelper get info from the this [] property
    class NullEnumerable : IList, ICollection, IEnumerable
    {
        public IEnumerator GetEnumerator ()
        {
            return new NullEnumerator ();
        }

        class NullEnumerator : IEnumerator
        {
            int pos = -1;

            // the idea is that we just move one time - the first time
            public bool MoveNext ()
            {
                if (pos > -1)
                    return false;

                pos = 0;
                return true;
            }

            public void Reset ()
            {
                pos = -1;
            }

            public object Current {
                get {
                    return null;
                }
            }
        }

        // make this return a string, and hide the interface impl,
        // so we are sure ListBindingHelper is actually accessing this property
        public string this [int index] {
            get {
                if (index != 0)
                    throw new ArgumentOutOfRangeException ("index");

                return null;
            }
            set {
            }
        }

        object IList.this [int index] {
            get {
                return this [index];
            }
            set {
            }
        }

        public int Add (object o)
        {
            return 0;
        }

        public void Clear ()
        {
        }

        public bool Contains (object o)
        {
            return false;
        }

        public int IndexOf (object o)
        {
            return -1;
        }

        public void Insert (int index, object o)
        {
        }

        public void Remove (object o)
        {
        }

        public void RemoveAt (int index)
        {
        }

        public bool IsFixedSize {
            get {
                return true;
            }
        }

        public bool IsReadOnly {
            get {
                return true;
            }
        }

        public void CopyTo (Array array, int offset)
        {
        }

        public int Count {
            get {
                return 1;
            }
        }

        public bool IsSynchronized {
            get {
                return false;
            }
        }

        public object SyncRoot {
            get {
                return this;
            }
        }
    }
}