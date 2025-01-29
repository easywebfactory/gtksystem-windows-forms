// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
    public partial class ComboBox
    {
        [ListBindable(false)]
        public partial class ObjectCollection : IList, IComparer<ComboBox.ObjectCollection.Entry>
        {
            private readonly ComboBox _owner;
            private List<Entry> _innerList = new List<Entry>();

            public ObjectCollection(ComboBox owner)
            {
                _owner = owner;
            }

            internal List<Entry> InnerList
            {
                get { return _innerList; }
            }

            /// <summary>
            ///  Retrieves the number of items.
            /// </summary>
            public int Count => InnerList.Count;

            object ICollection.SyncRoot
            {
                get { return this; }
            }

            bool ICollection.IsSynchronized
            {
                get { return false; }
            }

            bool IList.IsFixedSize
            {
                get { return false; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            /// <summary>
            ///  Adds an item to the combo box. For an unsorted combo box, the item is
            ///  added to the end of the existing list of items. For a sorted combo box,
            ///  the item is inserted into the list according to its sorted position.
            ///  The item's toString() method is called to obtain the string that is
            ///  displayed in the combo box.
            ///  A SystemException occurs if there is insufficient space available to
            ///  store the new item.
            /// </summary>
            public int Add(object item)
            {
                int index = AddInternal(item);

                return index;
            }

            private int AddInternal(object item)
            {
                item ??= "";
                int index = -1;
                if (_owner.IsHandleCreated)
                {
                    if (!_owner._sorted)
                    {
                        InnerList.Add(new Entry(item));
                        index = InnerList.Count - 1;
                    }
                    else
                    {
                        Entry entry = item is Entry entryItem ? entryItem : new Entry(item);
                        index = InnerList.BinarySearch(index: 0, Count, entry, this);
                        if (index < 0)
                        {
                            index = ~index; // getting the index of the first element that is larger than the search value
                        }

                        InnerList.Insert(index, entry);
                    }

                    bool successful = false;
                    try
                    {
                        if (_owner._sorted)
                        {
                            _owner.self.InsertText(index, item.ToString());
                        }
                        else
                        {
                            _owner.self.AppendText(item.ToString());
                        }

                        successful = true;
                    }
                    finally
                    {
                        if (!successful)
                        {
                            InnerList.RemoveAt(index);
                        }
                    }
                }

                return index;
            }

            int IList.Add(object? item)
            {
                return Add(item!);
            }

            public void AddRange(params object[] items)
            {
                try
                {
                    AddRangeInternal(items);
                }
                finally
                {
                }
            }

            internal void AddRangeInternal(IList items)
            {
                foreach (object item in items)
                {
                    // adding items one-by-one for performance (especially for sorted combobox)
                    // we can not rely on ArrayList.Sort since its worst case complexity is n*n
                    // AddInternal is based on BinarySearch and ensures n*log(n) complexity
                    AddInternal(item);
                }
            }

            /// <summary>
            ///  Retrieves the item with the specified index.
            /// </summary>
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public virtual object? this[int index]
            {
                get { return InnerList[index].Item; }
                set { SetItemInternal(index, value!); }
            }

            /// <summary>
            ///  Removes all items from the ComboBox.
            /// </summary>
            public void Clear()
            {
                ClearInternal();
            }

            internal void ClearInternal()
            {
                if (_owner.IsHandleCreated)
                {
                    ((Gtk.ListStore)_owner.self.Model).Clear();
                    InnerList.Clear();
                }

                _owner.SelectedIndex = -1;
            }

            public bool Contains(object? value)
            {
                return IndexOf(value) != -1;
            }

            /// <summary>
            ///  Copies the ComboBox Items collection to a destination array.
            /// </summary>
            public void CopyTo(object[] destination, int arrayIndex)
            {
                int count = InnerList.Count;

                for (int i = 0; i < count; i++)
                {
                    destination[i + arrayIndex] = InnerList[i].Item;
                }
            }

            void ICollection.CopyTo(Array destination, int index)
            {
                int count = InnerList.Count;

                for (int i = 0; i < count; i++)
                {
                    destination.SetValue(InnerList[i], i + index);
                }
            }

            /// <summary>
            ///  Returns an enumerator for the ComboBox Items collection.
            /// </summary>
            public IEnumerator GetEnumerator() => new EntryEnumerator(InnerList);

            /// <summary>
            ///  Adds an item to the combo box. For an unsorted combo box, the item is
            ///  added to the end of the existing list of items. For a sorted combo box,
            ///  the item is inserted into the list according to its sorted position.
            ///  The item's toString() method is called to obtain the string that is
            ///  displayed in the combo box.
            ///  A SystemException occurs if there is insufficient space available to
            ///  store the new item.
            /// </summary>
            public void Insert(int index, object? item)
            {
                item ??= "";
                if (_owner._sorted)
                {
                    Add(item);
                }
                else
                {
                    InnerList.Insert(index, new Entry(item));
                    if (_owner.IsHandleCreated)
                    {
                        bool successful = false;

                        try
                        {
                            _owner.self.InsertText(index, item.ToString());
                            successful = true;
                        }
                        finally
                        {
                            if (successful)
                            {
                            }
                            else
                            {
                                InnerList.RemoveAt(index);
                            }
                        }
                    }
                }
            }

            /// <summary>
            ///  Removes an item from the ComboBox at the given index.
            /// </summary>
            public void RemoveAt(int index)
            {
                if (_owner.IsHandleCreated)
                {
                    _owner.self.Remove(index);
                    InnerList.RemoveAt(index);
                }

                if (!_owner.IsHandleCreated)
                {
                    if (index < _owner._selectedIndex)
                    {
                        _owner._selectedIndex--;
                    }
                    else if (index == _owner._selectedIndex)
                    {
                        _owner._selectedIndex = -1;
                        _owner.Text = string.Empty;
                    }
                }
            }

            /// <summary>
            ///  Removes the given item from the ComboBox, provided that it is
            ///  actually in the list.
            /// </summary>
            public void Remove(object? value)
            {
                int index = IndexOf(value);

                if (index != -1)
                {
                    RemoveAt(index);
                }
            }

            internal void SetItemInternal(int index, object value)
            {
                // If the native control has been created, and the display text of the new list item object
                // is different to the current text in the native list item, recreate the native list item...
                if (!_owner.IsHandleCreated)
                {
                    return;
                }

                bool selected = (index == _owner.SelectedIndex);

                if (string.Compare(_owner.GetItemText(value), _owner.NativeGetItemText(index), true,
                        CultureInfo.CurrentCulture) != 0)
                {
                    _owner.self.Remove(index);
                    _owner.self.InsertText(index, value?.ToString());
                    InnerList.RemoveAt(index);
                    InnerList.Insert(index, new Entry(value));
                    if (selected)
                    {
                        _owner.SelectedIndex = index;
                        _owner.Text = value?.ToString();
                    }
                }
                else
                {
                    // NEW - FOR COMPATIBILITY REASONS
                    // Minimum compatibility fix
                    if (selected)
                    {
                        _owner.self.SetStateFlags(Gtk.StateFlags.Selected, true);
                    }
                }
            }

            public int IndexOf(object? value)
            {
                int virtualIndex = -1;

                foreach (Entry entry in InnerList)
                {
                    virtualIndex++;
                    if ((value is Entry itemEntry && entry == itemEntry) || entry.Item.Equals(value))
                    {
                        return virtualIndex;
                    }
                }

                return -1;
            }

            int IComparer<Entry>.Compare(Entry? entry1, Entry? entry2)
            {
                string? itemName1 = _owner.GetItemText(entry1.Item);
                string? itemName2 = _owner.GetItemText(entry2.Item);

                CompareInfo compInfo = Application.CurrentCulture.CompareInfo;
                return compInfo.Compare(itemName1, itemName2, CompareOptions.StringSort);
            }
        }
    }
}