// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

public partial class ListBox
{
    public partial class SelectedIndexCollection : IList
    {
        private readonly ListBox _owner;

        public SelectedIndexCollection(ListBox owner)
        {
            _owner = owner;
        }

        /// <summary>
        ///  Number of current selected items.
        /// </summary>
        [Browsable(false)]
        public int Count => _owner.SelectedItems.Count;

        object ICollection.SyncRoot => this;

        bool ICollection.IsSynchronized => true;

        bool IList.IsFixedSize => true;

        public bool IsReadOnly => true;

        public bool Contains(int selectedIndex)
        {
            return IndexOf(selectedIndex) != -1;
        }

        bool IList.Contains(object? selectedIndex)
        {
            if (selectedIndex is int selectedIndexAsInt)
            {
                return Contains(selectedIndexAsInt);
            }

            return false;
        }

        public int IndexOf(int selectedIndex)
        {
            // Just what does this do?  The selectedIndex parameter above is the index into the
            // main object collection.  We look at the state of that item, and if the state indicates
            // that it is selected, we get back the virtualized index into this collection.  Indexes on
            // this collection match those on the SelectedObjectCollection.
            if (selectedIndex >= 0 &&
                selectedIndex < InnerArray.Count &&
                InnerArray.GetState(selectedIndex, SelectedObjectCollection.selectedObjectMask))
            {
                return InnerArray.IndexOf(InnerArray.GetItem(selectedIndex), SelectedObjectCollection.selectedObjectMask);
            }

            return -1;
        }

        int IList.IndexOf(object? selectedIndex)
        {
            if (selectedIndex is int selectedIndexAsInt)
            {
                return IndexOf(selectedIndexAsInt);
            }

            return -1;
        }

        int IList.Add(object? value)
        {
            throw new NotSupportedException("SR.ListBoxSelectedIndexCollectionIsReadOnly");
        }

        void IList.Clear()
        {
            throw new NotSupportedException("SR.ListBoxSelectedIndexCollectionIsReadOnly");
        }

        void IList.Insert(int index, object? value)
        {
            throw new NotSupportedException("SR.ListBoxSelectedIndexCollectionIsReadOnly");
        }

        void IList.Remove(object? value)
        {
            throw new NotSupportedException("SR.ListBoxSelectedIndexCollectionIsReadOnly");
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException("SR.ListBoxSelectedIndexCollectionIsReadOnly");
        }

        /// <summary>
        ///  Retrieves the specified selected item.
        /// </summary>
        public int this[int index]
        {
            get
            {
                object identifier = InnerArray.GetEntryObject(index, SelectedObjectCollection.selectedObjectMask);
                return InnerArray.IndexOf(identifier);
            }
        }

        object? IList.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException("SR.ListBoxSelectedIndexCollectionIsReadOnly");
        }

        /// <summary>
        ///  This is the item array that stores our data.  We share this backing store
        ///  with the main object collection.
        /// </summary>
        private ItemArray InnerArray
        {
            get
            {
                _owner.SelectedItems.EnsureUpToDate();
                return ((ObjectCollection)_owner.Items).InnerArray;
            }
        }

        public void CopyTo(Array destination, int index)
        {
            var cnt = Count;
            for (var i = 0; i < cnt; i++)
            {
                destination.SetValue(this[i], i + index);
            }
        }

        public void Clear()
        {
            _owner?.ClearSelected();
        }

        public void Add(int index)
        {
            var items = _owner?.Items;
            if (items != null)
            {
                if (index != -1 && !Contains(index))
                {
                    _owner.SetSelected(index, true);
                }
            }
        }

        public void Remove(int index)
        {
            var items = _owner?.Items;
            if (items != null)
            {
                if (index != -1 && Contains(index))
                {
                    _owner.SetSelected(index, false);
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new SelectedIndexEnumerator(this);
        }
    }
}