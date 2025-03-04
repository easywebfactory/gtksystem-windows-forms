// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

public partial class ListBox
{
    // Should be "ObjectCollection", except we already have one of those.
    public class SelectedObjectCollection : IList
    {
        // This is the bitmask used within ItemArray to identify selected objects.
        internal static int selectedObjectMask = ItemArray.CreateMask();

        private readonly ListBox _owner;
        private bool _stateDirty;
        private int _lastVersion;
        private int _count;

        public SelectedObjectCollection(ListBox owner)
        {
            _owner = owner;
            _stateDirty = true;
            _lastVersion = -1;
        }

        /// <summary>
        ///  Number of current selected items.
        /// </summary>
        public int Count
        {
            get
            {
                if (_owner.IsHandleCreated)
                {
                    var current = _owner.SelectionMode;
                    switch (current)
                    {
                        case SelectionMode.None:
                            return 0;

                        case SelectionMode.One:
                            var index = _owner.SelectedIndex;
                            if (index >= 0)
                            {
                                return 1;
                            }

                            return 0;

                        case SelectionMode.MultiSimple:
                        case SelectionMode.MultiExtended:
                            return InnerArray.GetCount(selectedObjectMask);
                    }

                    return 0;
                }

                // If the handle hasn't been created, we must do this the hard way.
                // Getting the count when using a mask is expensive, so cache it.
                if (_lastVersion != InnerArray.Version)
                {
                    _lastVersion = InnerArray.Version;
                    _count = InnerArray.GetCount(selectedObjectMask);
                }

                return _count;
            }
        }

        object ICollection.SyncRoot => this;

        bool ICollection.IsSynchronized => false;

        bool IList.IsFixedSize => true;

        /// <summary>
        ///  Called by the list box to dirty the selected item state.
        /// </summary>
        internal void Dirty()
        {
            _stateDirty = true;
        }

        /// <summary>
        ///  This is the item array that stores our data.  We share this backing store
        ///  with the main object collection.
        /// </summary>
        private ItemArray InnerArray
        {
            get
            {
                EnsureUpToDate();
                return _owner.Items.InnerArray;
            }
        }

        /// <summary>
        ///  This is the function that Ensures that the selections are uptodate with
        ///  current listbox handle selections.
        /// </summary>
        internal void EnsureUpToDate()
        {
            //if (_stateDirty)
            //{
            //    _stateDirty = false;
            //    if (_owner.IsHandleCreated)
            //    {
            //        _owner.NativeUpdateSelection();
            //    }
            //}
        }

        public bool IsReadOnly => true;

        public bool Contains(object? selectedObject)
        {
            return IndexOf(selectedObject) != -1;
        }

        public int IndexOf(object? selectedObject)
        {
            return selectedObject is null ? -1 : InnerArray.IndexOf(selectedObject, selectedObjectMask);
        }

        int IList.Add(object? value)
        {
            throw new NotSupportedException("SR.ListBoxSelectedObjectCollectionIsReadOnly");
        }

        void IList.Clear()
        {
            throw new NotSupportedException("SR.ListBoxSelectedObjectCollectionIsReadOnly");
        }

        void IList.Insert(int index, object? value)
        {
            throw new NotSupportedException("SR.ListBoxSelectedObjectCollectionIsReadOnly");
        }

        void IList.Remove(object? value)
        {
            throw new NotSupportedException("SR.ListBoxSelectedObjectCollectionIsReadOnly");
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException("SR.ListBoxSelectedObjectCollectionIsReadOnly");
        }

        // A new internal method used in SelectedIndex getter...
        // For a Multi select ListBox there can be two items with the same name ...
        // and hence a object comparison is required...
        // This method returns the "object" at the passed index rather than the "item" ...
        // this "object" is then compared in the IndexOf( ) method of the itemsCollection.
        internal object GetObjectAt(int index)
        {
            return InnerArray.GetEntryObject(index, selectedObjectMask);
        }

        /// <summary>
        ///  Retrieves the specified selected item.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object? this[int index]
        {
            get => InnerArray.GetItem(index, selectedObjectMask);
            set => throw new NotSupportedException("SR.ListBoxSelectedObjectCollectionIsReadOnly");
        }

        public void CopyTo(Array destination, int index)
        {
            var cnt = InnerArray.GetCount(selectedObjectMask);
            for (var i = 0; i < cnt; i++)
            {
                destination.SetValue(InnerArray.GetItem(i, selectedObjectMask), i + index);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return InnerArray.GetEnumerator(selectedObjectMask);
        }

        /// <summary>
        ///  This method returns if the actual item index is selected.  The index is the index to the MAIN
        ///  collection, not this one.
        /// </summary>
        internal bool GetSelected(int index)
        {
            return InnerArray.GetState(index, selectedObjectMask);
        }

        // when SelectedObjectsCollection::ItemArray is accessed we push the selection from Native ListBox into our .Net ListBox - see EnsureUpToDate()
        // when we create the handle we need to be able to do the opposite : push the selection from .Net ListBox into Native ListBox
        internal void PushSelectionIntoNativeListBox(int index)
        {
            //// we can't use ItemArray accessor because this will wipe out our Selection collection
            //bool selected = _owner.Items.InnerArray.GetState(index, SelectedObjectMask);
            //// push selection only if the item is actually selected
            //// this also takes care of the case where owner.SelectionMode == SelectionMode.One
            //if (selected)
            //{
            //    _owner.NativeSetSelected(index, true /*we signal selection to the native listBox only if the item is actually selected*/);
            //}
        }

        /// <summary>
        ///  Same thing for GetSelected.
        /// </summary>
        internal void SetSelected(int index, bool value)
        {
            if (index > -1 && index < InnerArray.Count)
            {
                InnerArray.SetState(index, selectedObjectMask, value);
                _owner.SetSelected(index, value);
            }
        }

        public void Clear()
        {
            _owner?.ClearSelected();
        }

        public void Add(object? value)
        {
            var items = _owner?.Items;
            if (items != null && value != null)
            {
                var index = items.IndexOf(value);
                if (index != -1 && !GetSelected(index))
                {
                    _owner.SelectedIndex = index;
                }
            }
        }

        public void Remove(object? value)
        {
            var items = _owner?.Items;
            if (items != null && value != null)
            {
                var index = items.IndexOf(value);
                if (index != -1 && GetSelected(index))
                {
                    _owner.SetSelected(index, false);
                }
            }
        }
    }
}