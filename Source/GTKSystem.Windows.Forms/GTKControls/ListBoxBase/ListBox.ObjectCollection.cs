// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Globalization;
using static System.Windows.Forms.ItemArray;

namespace System.Windows.Forms;

public partial class ListBox
{
    /// <summary>
    ///  A collection that stores objects.
    /// </summary>
    [ListBindable(false)]
    public class ObjectCollection : IList
    {
        private readonly ListBox _owner;
        private ItemArray _items = null!;

        public ObjectCollection(ListBox owner)
        {
            _owner = owner;
        }

        /// <summary>
        ///  Initializes a new instance of ListBox.ObjectCollection based on another ListBox.ObjectCollection.
        /// </summary>
        public ObjectCollection(ListBox owner, ObjectCollection value)
            : this(owner)
        {
            AddRange(value);
        }

        /// <summary>
        ///  Initializes a new instance of ListBox.ObjectCollection containing any array of objects.
        /// </summary>
        public ObjectCollection(ListBox owner, object[] value)
            : this(owner)
        {
            AddRange(value);
        }

        /// <summary>
        ///  Retrieves the number of items.
        /// </summary>
        public int Count => InnerArray.Count;

        /// <summary>
        ///  Internal access to the actual data store.
        /// </summary>
        internal ItemArray InnerArray
        {
            get
            {
                _items ??= new ItemArray(_owner);

                return _items;
            }
        }

        object ICollection.SyncRoot => this;

        bool ICollection.IsSynchronized => false;

        bool IList.IsFixedSize => false;

        public bool IsReadOnly => false;

        /// <summary>
        ///  Adds an item to the List box. For an unsorted List box, the item is
        ///  added to the end of the existing list of items. For a sorted List box,
        ///  the item is inserted into the list according to its sorted position.
        ///  The item's toString() method is called to obtain the string that is
        ///  displayed in the List box.
        ///  A SystemException occurs if there is insufficient space available to
        ///  store the new item.
        /// </summary>
        public int Add(object? item)
        {
            _owner.CheckNoDataSource();
            var index = AddInternal(item);
            return index;
        }

        private int AddInternal(object? item)
        {
            item ??= "";
            var index = -1;
            if (!_owner._sorted)
            {
                InnerArray.Add(item);
            }
            else
            {
                var entry = GetEntry(item);
                if (Count > 0)
                {
                    index = InnerArray.BinarySearch(entry);
                    if (index < 0)
                    {
                        // getting the index of the first element that is larger than the search value
                        // this index will be used for insert
                        index = ~index;
                    }
                }
                else
                {
                    index = 0;
                }

                //Debug.Assert(index >= 0 && index <= Count, "Wrong index for insert");
                InnerArray.InsertEntry(index, entry);
            }

            var successful = false;

            try
            {
                if (_owner._sorted)
                {
                    if (_owner.IsHandleCreated)
                    {
                        _owner.NativeInsert(index, item);
                    }
                }
                else
                {
                    index = Count - 1;
                    if (_owner.IsHandleCreated)
                    {
                        _owner.NativeAdd(item);
                    }
                }

                successful = true;
            }
            finally
            {
                if (!successful)
                {
                    InnerArray.Remove(item);
                }
            }

            return index;
        }

        int IList.Add(object? item) => Add(item!);

        public void AddRange(ObjectCollection value)
        {
            _owner.CheckNoDataSource();
            AddRangeInternal(value);
        }

        public void AddRange(params object[] items)
        {
            _owner.CheckNoDataSource();
            AddRangeInternal(items);
        }

        internal void AddRangeInternal(ICollection items)
        {
            _owner.BeginUpdate();
            try
            {
                foreach (var item in items)
                {
                    AddInternal(item);
                }
            }
            finally
            {
                _owner.EndUpdate();
            }
        }

        /// <summary>
        ///  Retrieves the item with the specified index.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object? this[int index]
        {
            get => InnerArray.GetItem(index);
            set
            {
                _owner.CheckNoDataSource();
                SetItemInternal(index, value);
            }
        }

        object? IList.this[int index]
        {
            get => this[index];
            set => this[index] = value!;
        }

        /// <summary>
        ///  Removes all items from the ListBox.
        /// </summary>
        public virtual void Clear()
        {
            _owner.CheckNoDataSource();
            ClearInternal();
        }

        /// <summary>
        ///  Removes all items from the ListBox.  Bypasses the data source check.
        /// </summary>
        internal void ClearInternal()
        {
            // update the width.. to reset Scrollbars..
            // Clear the selection state.
            if (_owner.IsHandleCreated)
            {
                _owner.NativeClear();
            }
            InnerArray.Clear();
        }

        public bool Contains(object? value)
        {
            return IndexOf(value) != -1;
        }

        bool IList.Contains(object? value) => Contains(value!);

        /// <summary>
        ///  Copies the ListBox Items collection to a destination array.
        /// </summary>
        public void CopyTo(object?[] destination, int arrayIndex)
        {
            var count = InnerArray.Count;
            for (var i = 0; i < count; i++)
            {
                destination[i + arrayIndex] = InnerArray.GetItem(i);
            }
        }

        void ICollection.CopyTo(Array destination, int index)
        {
            var count = InnerArray.Count;
            for (var i = 0; i < count; i++)
            {
                destination.SetValue(InnerArray.GetItem(i), i + index);
            }
        }

        /// <summary>
        ///  Returns an enumerator for the ListBox Items collection.
        /// </summary>
        public IEnumerator GetEnumerator() => InnerArray.GetEnumerator();

        public int IndexOf(object? value)
        {
            return InnerArray.IndexOf(value);
        }

        int IList.IndexOf(object? value) => IndexOf(value!);

        internal int IndexOfIdentifier(object? value)
        {
            return InnerArray.IndexOf(value);
        }

        /// <summary>
        ///  Adds an item to the List box. For an unsorted List box, the item is
        ///  added to the end of the existing list of items. For a sorted List box,
        ///  the item is inserted into the list according to its sorted position.
        ///  The item's toString() method is called to obtain the string that is
        ///  displayed in the List box.
        ///  A SystemException occurs if there is insufficient space available to
        ///  store the new item.
        /// </summary>
        public void Insert(int index, object? item)
        {
            _owner.CheckNoDataSource();
            // If the List box is sorted, then nust treat this like an add
            // because we are going to twiddle the index anyway.
            if (_owner._sorted)
            {
                Add(item);
            }
            else
            {
                InnerArray.Insert(index, item);
                if (_owner.IsHandleCreated)
                {
                    var successful = false;

                    try
                    {
                        _owner.NativeInsert(index, item);
                        successful = true;
                    }
                    finally
                    {
                        if (!successful)
                        {
                            InnerArray.RemoveAt(index);
                        }
                    }
                }
            }
        }

        void IList.Insert(int index, object? item) => Insert(index, item!);

        /// <summary>
        ///  Removes the given item from the ListBox, provided that it is
        ///  actually in the list.
        /// </summary>
        public void Remove(object? value)
        {
            _owner.CheckNoDataSource();

            var index = InnerArray.IndexOf(value);
            if (index != -1)
            {
                RemoveAt(index);
            }
        }

        void IList.Remove(object? value) => Remove(value!);

        /// <summary>
        ///  Removes an item from the ListBox at the given index.
        /// </summary>
        public void RemoveAt(int index)
        {
            _owner.CheckNoDataSource();
            // Update InnerArray before calling NativeRemoveAt to ensure that when
            // SelectedIndexChanged is raised (by NativeRemoveAt), InnerArray's state matches wrapped LB state.
            InnerArray.RemoveAt(index);

            if (_owner.IsHandleCreated)
            {
                _owner.NativeRemoveAt(index);
            }
        }

        internal void SetItemInternal(int index, object? value)
        {
            InnerArray.SetItem(index, value);

            // If the native control has been created, and the display text of the new list item object
            // is different to the current text in the native list item, recreate the native list item...
            if (_owner.IsHandleCreated)
            {
                var selected = _owner.SelectedIndex == index;
                if (string.Compare(_owner.GetItemText(value), _owner.NativeGetItemText(index), true, CultureInfo.CurrentCulture) != 0)
                {
                    _owner.NativeRemoveAt(index);
                    _owner.SelectedItems.SetSelected(index, false);
                    _owner.NativeInsert(index, value);
                    if (selected)
                    {
                        _owner.SelectedIndex = index;
                    }
                }
                else
                {
                    // FOR COMPATIBILITY REASONS
                    if (selected)
                    {
                        _owner.OnSelectedIndexChanged(EventArgs.Empty); // will fire selectedvaluechanged

                    }
                }
            }
        }
    }
}