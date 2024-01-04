using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Controls;

namespace System.Windows.Forms
{
	[Docking(DockingBehavior.Ask)]
	[Designer("System.Windows.Forms.Design.ListViewDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Items")]
	[DefaultEvent("SelectedIndexChanged")]
	[SRDescription("DescriptionListView")]
	public class ListView : Control
	{
		private class DisposingContext : IDisposable
		{
			public DisposingContext(ListView owner)
			{
				throw null;
			}

			public void Dispose()
			{
				throw null;
			}
		}

		[ListBindable(false)]
		public class CheckedIndexCollection : IList, ICollection, IEnumerable
		{
			[Browsable(false)]
			public int Count
			{
				get
				{
					throw null;
				}
			}

			public int this[int index]
			{
				get
				{
					throw null;
				}
			}

			object? IList.this[int index]
			{
				get
				{
					throw null;
				}
				set
				{
					throw null;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					throw null;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					throw null;
				}
			}

			bool IList.IsFixedSize
			{
				get
				{
					throw null;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					throw null;
				}
			}

			public CheckedIndexCollection(ListView owner)
			{
				throw null;
			}

			public bool Contains(int checkedIndex)
			{
				throw null;
			}

			bool IList.Contains(object? checkedIndex)
			{
				throw null;
			}

			public int IndexOf(int checkedIndex)
			{
				throw null;
			}

			int IList.IndexOf(object? checkedIndex)
			{
				throw null;
			}

			int IList.Add(object? value)
			{
				throw null;
			}

			void IList.Clear()
			{
				throw null;
			}

			void IList.Insert(int index, object? value)
			{
				throw null;
			}

			void IList.Remove(object? value)
			{
				throw null;
			}

			void IList.RemoveAt(int index)
			{
				throw null;
			}

			void ICollection.CopyTo(Array dest, int index)
			{
				throw null;
			}

			public IEnumerator GetEnumerator()
			{
				throw null;
			}
		}

		[ListBindable(false)]
		public class CheckedListViewItemCollection : IList, ICollection, IEnumerable
		{
			[Browsable(false)]
			public int Count
			{
				get
				{
					throw null;
				}
			}

			public ListViewItem this[int index]
			{
				get
				{
					throw null;
				}
			}

			object? IList.this[int index]
			{
				get
				{
					throw null;
				}
				set
				{
					throw null;
				}
			}

			public virtual ListViewItem? this[string? key]
			{
				get
				{
					throw null;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					throw null;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					throw null;
				}
			}

			bool IList.IsFixedSize
			{
				get
				{
					throw null;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					throw null;
				}
			}

			public CheckedListViewItemCollection(ListView owner)
			{
				throw null;
			}

			public bool Contains(ListViewItem? item)
			{
				throw null;
			}

			bool IList.Contains(object? item)
			{
				throw null;
			}

			public virtual bool ContainsKey(string? key)
			{
				throw null;
			}

			public int IndexOf(ListViewItem item)
			{
				throw null;
			}

			public virtual int IndexOfKey(string? key)
			{
				throw null;
			}

			int IList.IndexOf(object? item)
			{
				throw null;
			}

			int IList.Add(object? value)
			{
				throw null;
			}

			void IList.Clear()
			{
				throw null;
			}

			void IList.Insert(int index, object? value)
			{
				throw null;
			}

			void IList.Remove(object? value)
			{
				throw null;
			}

			void IList.RemoveAt(int index)
			{
				throw null;
			}

			public void CopyTo(Array dest, int index)
			{
				throw null;
			}

			public IEnumerator GetEnumerator()
			{
				throw null;
			}
		}

		[ListBindable(false)]
		public class ColumnHeaderCollection : IList, ICollection, IEnumerable
		{
			public virtual ColumnHeader this[int index]
			{
				get
				{
					throw null;
				}
			}

			object? IList.this[int index]
			{
				get
				{
					throw null;
				}
				set
				{
					throw null;
				}
			}

			public virtual ColumnHeader? this[string? key]
			{
				get
				{
					throw null;
				}
			}

			[Browsable(false)]
			public int Count
			{
				get
				{
					throw null;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					throw null;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					throw null;
				}
			}

			bool IList.IsFixedSize
			{
				get
				{
					throw null;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					throw null;
				}
			}

			public ColumnHeaderCollection(ListView owner)
			{
				throw null;
			}

			public virtual void RemoveByKey(string? key)
			{
				throw null;
			}

			public virtual int IndexOfKey(string? key)
			{
				throw null;
			}

			public virtual ColumnHeader Add(string? text, int width, HorizontalAlignment textAlign)
			{
				throw null;
			}

			public virtual int Add(ColumnHeader value)
			{
				throw null;
			}

			public virtual ColumnHeader Add(string? text)
			{
				throw null;
			}

			public virtual ColumnHeader Add(string? text, int width)
			{
				throw null;
			}

			public virtual ColumnHeader Add(string? key, string? text)
			{
				throw null;
			}

			public virtual ColumnHeader Add(string? key, string? text, int width)
			{
				throw null;
			}

			public virtual ColumnHeader Add(string? key, string? text, int width, HorizontalAlignment textAlign, string imageKey)
			{
				throw null;
			}

			public virtual ColumnHeader Add(string? key, string? text, int width, HorizontalAlignment textAlign, int imageIndex)
			{
				throw null;
			}

			public virtual void AddRange(ColumnHeader[] values)
			{
				throw null;
			}

			int IList.Add(object? value)
			{
				throw null;
			}

			public virtual void Clear()
			{
				throw null;
			}

			public bool Contains(ColumnHeader? value)
			{
				throw null;
			}

			bool IList.Contains(object? value)
			{
				throw null;
			}

			public virtual bool ContainsKey(string? key)
			{
				throw null;
			}

			void ICollection.CopyTo(Array dest, int index)
			{
				throw null;
			}

			public int IndexOf(ColumnHeader? value)
			{
				throw null;
			}

			int IList.IndexOf(object? value)
			{
				throw null;
			}

			public void Insert(int index, ColumnHeader value)
			{
				throw null;
			}

			void IList.Insert(int index, object? value)
			{
				throw null;
			}

			public void Insert(int index, string? text, int width, HorizontalAlignment textAlign)
			{
				throw null;
			}

			public void Insert(int index, string? text)
			{
				throw null;
			}

			public void Insert(int index, string? text, int width)
			{
				throw null;
			}

			public void Insert(int index, string? key, string? text)
			{
				throw null;
			}

			public void Insert(int index, string? key, string? text, int width)
			{
				throw null;
			}

			public void Insert(int index, string? key, string? text, int width, HorizontalAlignment textAlign, string imageKey)
			{
				throw null;
			}

			public void Insert(int index, string? key, string? text, int width, HorizontalAlignment textAlign, int imageIndex)
			{
				throw null;
			}

			public virtual void RemoveAt(int index)
			{
				throw null;
			}

			public virtual void Remove(ColumnHeader column)
			{
				throw null;
			}

			void IList.Remove(object? value)
			{
				throw null;
			}

			public IEnumerator GetEnumerator()
			{
				throw null;
			}
		}

		internal class IconComparer : IComparer
		{
			public SortOrder SortOrder
			{
				set
				{
					throw null;
				}
			}

			public IconComparer(SortOrder currentSortOrder)
			{
				throw null;
			}

			public int Compare(object? obj1, object? obj2)
			{
				throw null;
			}
		}

		internal class ListViewAccessibleObject : ControlAccessibleObject
		{
			internal override Rectangle BoundingRectangle
			{
				get
				{
					throw null;
				}
			}

			internal override bool CanSelectMultiple
			{
				get
				{
					throw null;
				}
			}

			internal override int ColumnCount
			{
				get
				{
					throw null;
				}
			}

			internal bool OwnerHasDefaultGroup
			{
				get
				{
					throw null;
				}
			}

			internal override int RowCount
			{
				get
				{
					throw null;
				}
			}

			internal override Interop.UiaCore.RowOrColumnMajor RowOrColumnMajor
			{
				get
				{
					throw null;
				}
			}

			internal ListViewAccessibleObject(ListView owningListView)
			{
				throw null;
			}

			internal override Interop.UiaCore.IRawElementProviderFragment? ElementProviderFromPoint(double x, double y)
			{
				throw null;
			}

			internal override Interop.UiaCore.IRawElementProviderFragment? FragmentNavigate(Interop.UiaCore.NavigateDirection direction)
			{
				throw null;
			}

			public override AccessibleObject? GetChild(int index)
			{
				throw null;
			}

			public override int GetChildCount()
			{
				throw null;
			}

			internal override int GetChildIndex(AccessibleObject? child)
			{
				throw null;
			}

			internal override Interop.UiaCore.IRawElementProviderSimple[]? GetColumnHeaders()
			{
				throw null;
			}

			internal override Interop.UiaCore.IRawElementProviderFragment? GetFocus()
			{
				throw null;
			}

			internal override int GetMultiViewProviderCurrentView()
			{
				throw null;
			}

			internal override int[] GetMultiViewProviderSupportedViews()
			{
				throw null;
			}

			internal override string GetMultiViewProviderViewName(int viewId)
			{
				throw null;
			}

			internal override object? GetPropertyValue(Interop.UiaCore.UIA propertyID)
			{
				throw null;
			}

			internal override Interop.UiaCore.IRawElementProviderSimple[]? GetRowHeaders()
			{
				throw null;
			}

			internal override Interop.UiaCore.IRawElementProviderSimple[] GetSelection()
			{
				throw null;
			}

			internal IReadOnlyList<ListViewGroup> GetVisibleGroups()
			{
				throw null;
			}

			public override AccessibleObject? HitTest(int x, int y)
			{
				throw null;
			}

			internal override bool IsPatternSupported(Interop.UiaCore.UIA patternId)
			{
				throw null;
			}

			internal override void SetMultiViewProviderCurrentView(int viewId)
			{
				throw null;
			}
		}

		[ListBindable(false)]
		public class ListViewItemCollection : IList, ICollection, IEnumerable
		{
			internal interface IInnerList
			{
				int Count { get; }

				bool OwnerIsVirtualListView { get; }

				bool OwnerIsDesignMode { get; }

				ListViewItem this[int index] { get; set; }

				ListViewItem Add(ListViewItem item);

				void AddRange(ListViewItem[] items);

				void Clear();

				bool Contains(ListViewItem item);

				void CopyTo(Array dest, int index);

				IEnumerator GetEnumerator();

				int IndexOf(ListViewItem item);

				ListViewItem Insert(int index, ListViewItem item);

				void Remove(ListViewItem item);

				void RemoveAt(int index);
			}

			[Browsable(false)]
			public int Count
			{
				get
				{
					throw null;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					throw null;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					throw null;
				}
			}

			bool IList.IsFixedSize
			{
				get
				{
					throw null;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					throw null;
				}
			}

			public virtual ListViewItem this[int index]
			{
				get
				{
					throw null;
				}
				set
				{
					throw null;
				}
			}

			object? IList.this[int index]
			{
				get
				{
					throw null;
				}
				set
				{
					throw null;
				}
			}

			public virtual ListViewItem? this[string key]
			{
				get
				{
					throw null;
				}
			}

			public ListViewItemCollection(ListView owner)
			{
				throw null;
			}

			internal ListViewItemCollection(IInnerList innerList)
			{
				throw null;
			}

			public virtual ListViewItem Add(string? text)
			{
				throw null;
			}

			int IList.Add(object? item)
			{
				throw null;
			}

			public virtual ListViewItem Add(string? text, int imageIndex)
			{
				throw null;
			}

			public virtual ListViewItem Add(ListViewItem value)
			{
				throw null;
			}

			public virtual ListViewItem Add(string? text, string? imageKey)
			{
				throw null;
			}

			public virtual ListViewItem Add(string? key, string? text, string? imageKey)
			{
				throw null;
			}

			public virtual ListViewItem Add(string? key, string? text, int imageIndex)
			{
				throw null;
			}

			public void AddRange(ListViewItem[] items)
			{
				throw null;
			}

			public void AddRange(ListViewItemCollection items)
			{
				throw null;
			}

			public virtual void Clear()
			{
				throw null;
			}

			public bool Contains(ListViewItem item)
			{
				throw null;
			}

			bool IList.Contains(object? item)
			{
				throw null;
			}

			public virtual bool ContainsKey(string? key)
			{
				throw null;
			}

			public void CopyTo(Array dest, int index)
			{
				throw null;
			}

			public ListViewItem[] Find(string key, bool searchAllSubItems)
			{
				throw null;
			}

			public IEnumerator GetEnumerator()
			{
				throw null;
			}

			public int IndexOf(ListViewItem item)
			{
				throw null;
			}

			int IList.IndexOf(object? item)
			{
				throw null;
			}

			public virtual int IndexOfKey(string? key)
			{
				throw null;
			}

			public ListViewItem Insert(int index, ListViewItem item)
			{
				throw null;
			}

			public ListViewItem Insert(int index, string? text)
			{
				throw null;
			}

			public ListViewItem Insert(int index, string? text, int imageIndex)
			{
				throw null;
			}

			void IList.Insert(int index, object? item)
			{
				throw null;
			}

			public ListViewItem Insert(int index, string? text, string? imageKey)
			{
				throw null;
			}

			public virtual ListViewItem Insert(int index, string? key, string? text, string? imageKey)
			{
				throw null;
			}

			public virtual ListViewItem Insert(int index, string? key, string? text, int imageIndex)
			{
				throw null;
			}

			public virtual void Remove(ListViewItem item)
			{
				throw null;
			}

			public virtual void RemoveAt(int index)
			{
				throw null;
			}

			public virtual void RemoveByKey(string key)
			{
				throw null;
			}

			void IList.Remove(object? item)
			{
				throw null;
			}
		}

		internal class ListViewNativeItemCollection : ListViewItemCollection.IInnerList
		{
			public int Count
			{
				get
				{
					throw null;
				}
			}

			public bool OwnerIsVirtualListView
			{
				get
				{
					throw null;
				}
			}

			public bool OwnerIsDesignMode
			{
				get
				{
					throw null;
				}
			}

			public ListViewItem this[int displayIndex]
			{
				get
				{
					throw null;
				}
				set
				{
					throw null;
				}
			}

			public ListViewNativeItemCollection(ListView owner)
			{
				throw null;
			}

			public ListViewItem Add(ListViewItem value)
			{
				throw null;
			}

			public void AddRange(ListViewItem[] values)
			{
				throw null;
			}

			public void Clear()
			{
				throw null;
			}

			public bool Contains(ListViewItem item)
			{
				throw null;
			}

			public ListViewItem Insert(int index, ListViewItem item)
			{
				throw null;
			}

			public int IndexOf(ListViewItem item)
			{
				throw null;
			}

			public void Remove(ListViewItem item)
			{
				throw null;
			}

			public void RemoveAt(int index)
			{
				throw null;
			}

			public void CopyTo(Array dest, int index)
			{
				throw null;
			}

			public IEnumerator GetEnumerator()
			{
				throw null;
			}
		}

		[ListBindable(false)]
		public class SelectedIndexCollection : IList, ICollection, IEnumerable
		{
			[Browsable(false)]
			public int Count
			{
				get
				{
					throw null;
				}
			}

			public int this[int index]
			{
				get
				{
					throw null;
				}
			}

			object? IList.this[int index]
			{
				get
				{
					throw null;
				}
				set
				{
					throw null;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					throw null;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					throw null;
				}
			}

			bool IList.IsFixedSize
			{
				get
				{
					throw null;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					throw null;
				}
			}

			public SelectedIndexCollection(ListView owner)
			{
				throw null;
			}

			public bool Contains(int selectedIndex)
			{
				throw null;
			}

			bool IList.Contains(object? selectedIndex)
			{
				throw null;
			}

			public int IndexOf(int selectedIndex)
			{
				throw null;
			}

			int IList.IndexOf(object? selectedIndex)
			{
				throw null;
			}

			int IList.Add(object? value)
			{
				throw null;
			}

			void IList.Clear()
			{
				throw null;
			}

			void IList.Insert(int index, object? value)
			{
				throw null;
			}

			void IList.Remove(object? value)
			{
				throw null;
			}

			void IList.RemoveAt(int index)
			{
				throw null;
			}

			public int Add(int itemIndex)
			{
				throw null;
			}

			public void Clear()
			{
				throw null;
			}

			public void CopyTo(Array dest, int index)
			{
				throw null;
			}

			public IEnumerator GetEnumerator()
			{
				throw null;
			}

			public void Remove(int itemIndex)
			{
				throw null;
			}
		}

		[ListBindable(false)]
		public class SelectedListViewItemCollection : IList, ICollection, IEnumerable
		{
			[Browsable(false)]
			public int Count
			{
				get
				{
					throw null;
				}
			}

			public ListViewItem this[int index]
			{
				get
				{
					throw null;
				}
			}

			public virtual ListViewItem? this[string? key]
			{
				get
				{
					throw null;
				}
			}

			object? IList.this[int index]
			{
				get
				{
					throw null;
				}
				set
				{
					throw null;
				}
			}

			bool IList.IsFixedSize
			{
				get
				{
					throw null;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					throw null;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					throw null;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					throw null;
				}
			}

			public SelectedListViewItemCollection(ListView owner)
			{
				throw null;
			}

			int IList.Add(object? value)
			{
				throw null;
			}

			void IList.Insert(int index, object? value)
			{
				throw null;
			}

			void IList.Remove(object? value)
			{
				throw null;
			}

			void IList.RemoveAt(int index)
			{
				throw null;
			}

			public void Clear()
			{
				throw null;
			}

			public virtual bool ContainsKey(string? key)
			{
				throw null;
			}

			public bool Contains(ListViewItem? item)
			{
				throw null;
			}

			bool IList.Contains(object? item)
			{
				throw null;
			}

			public void CopyTo(Array dest, int index)
			{
				throw null;
			}

			public IEnumerator GetEnumerator()
			{
				throw null;
			}

			public int IndexOf(ListViewItem? item)
			{
				throw null;
			}

			int IList.IndexOf(object? item)
			{
				throw null;
			}

			public virtual int IndexOfKey(string? key)
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(ItemActivation.Standard)]
		[SRDescription("ListViewActivationDescr")]
		public ItemActivation Activation
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(ListViewAlignment.Top)]
		[Localizable(true)]
		[SRDescription("ListViewAlignmentDescr")]
		public ListViewAlignment Alignment
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewAllowColumnReorderDescr")]
		public bool AllowColumnReorder
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ListViewAutoArrangeDescr")]
		public bool AutoArrange
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		public override Color BackColor
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("ListViewBackgroundImageTiledDescr")]
		public bool BackgroundImageTiled
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		[SRDescription("borderStyleDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("ListViewCheckBoxesDescr")]
		public bool CheckBoxes
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CheckedIndexCollection CheckedIndices
		{
			get
			{
				throw null;
			}
		}

		internal ToolTip KeyboardToolTip
		{
			[CompilerGenerated]
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CheckedListViewItemCollection CheckedItems
		{
			get
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.ColumnHeaderCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ListViewColumnsDescr")]
		[Localizable(true)]
		[MergableProperty(false)]
		public ColumnHeaderCollection Columns
		{
			get
			{
				throw null;
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				throw null;
			}
		}

		internal ListViewGroup DefaultGroup
		{
			get
			{
				throw null;
			}
		}

		protected override Size DefaultSize
		{
			get
			{
				throw null;
			}
		}

		protected override bool DoubleBuffered
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		internal bool ExpectingMouseUp
		{
			get
			{
				throw null;
			}
		}

		internal ListViewGroup? FocusedGroup
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewFocusedItemDescr")]
		public ListViewItem? FocusedItem
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		public override Color ForeColor
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("ListViewFullRowSelectDescr")]
		public bool FullRowSelect
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("ListViewGridLinesDescr")]
		public bool GridLines
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ListViewGroupImageListDescr")]
		public ImageList? GroupImageList
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.ListViewGroupCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ListViewGroupsDescr")]
		[MergableProperty(false)]
		public ListViewGroupCollection Groups
		{
			get
			{
				throw null;
			}
		}

		internal bool GroupsDisplayed
		{
			get
			{
				throw null;
			}
		}

		internal bool GroupsEnabled
		{
			get
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(ColumnHeaderStyle.Clickable)]
		[SRDescription("ListViewHeaderStyleDescr")]
		public ColumnHeaderStyle HeaderStyle
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewHideSelectionDescr")]
		public bool HideSelection
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewHotTrackingDescr")]
		public bool HotTracking
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewHoverSelectDescr")]
		public bool HoverSelection
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		internal bool InsertingItemsNatively
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewInsertionMarkDescr")]
		public ListViewInsertionMark InsertionMark
		{
			get
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.ListViewItemCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ListViewItemsDescr")]
		[MergableProperty(false)]
		public ListViewItemCollection Items
		{
			get
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewLabelEditDescr")]
		public bool LabelEdit
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("ListViewLabelWrapDescr")]
		public bool LabelWrap
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ListViewLargeImageListDescr")]
		public ImageList? LargeImageList
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		internal bool ListViewHandleDestroyed
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewItemSorterDescr")]
		public IComparer? ListViewItemSorter
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ListViewMultiSelectDescr")]
		public bool MultiSelect
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewOwnerDrawDescr")]
		public bool OwnerDraw
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("ControlRightToLeftLayoutDescr")]
		public virtual bool RightToLeftLayout
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ListViewScrollableDescr")]
		public bool Scrollable
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SelectedIndexCollection SelectedIndices
		{
			get
			{
				throw null;
			}
		}

		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewSelectedItemsDescr")]
		public SelectedListViewItemCollection SelectedItems
		{
			get
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ListViewShowGroupsDescr")]
		public bool ShowGroups
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ListViewSmallImageListDescr")]
		public ImageList? SmallImageList
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewShowItemToolTipsDescr")]
		public bool ShowItemToolTips
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(SortOrder.None)]
		[SRDescription("ListViewSortingDescr")]
		public SortOrder Sorting
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ListViewStateImageListDescr")]
		public ImageList? StateImageList
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		internal bool SupportsListViewSubItems
		{
			get
			{
				throw null;
			}
		}

		internal override bool SupportsUiaProviders
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		public override string Text
		{
			get
			{
				throw null;
			}
			[param: AllowNull]
			set
			{
				throw null;
			}
		}

		[SRCategory("CatAppearance")]
		[Browsable(true)]
		[SRDescription("ListViewTileSizeDescr")]
		public Size TileSize
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewTopItemDescr")]
		public ListViewItem? TopItem
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DefaultValue(true)]
		public bool UseCompatibleStateImageBehavior
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatAppearance")]
		[DefaultValue(View.LargeIcon)]
		[SRDescription("ListViewViewDescr")]
		public View View
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ListViewVirtualListSizeDescr")]
		public int VirtualListSize
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ListViewVirtualModeDescr")]
		public bool VirtualMode
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding
		{
			get
			{
				throw null;
			}
			set
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler? BackgroundImageLayoutChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		public event EventHandler? RightToLeftLayoutChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler? TextChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[SRDescription("ListViewAfterLabelEditDescr")]
		public event LabelEditEventHandler? AfterLabelEdit
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[SRDescription("ListViewBeforeLabelEditDescr")]
		public event LabelEditEventHandler? BeforeLabelEdit
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatAction")]
		[SRDescription("ListViewCacheVirtualItemsEventDescr")]
		public event CacheVirtualItemsEventHandler? CacheVirtualItems
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatAction")]
		[SRDescription("ListViewColumnClickDescr")]
		public event ColumnClickEventHandler? ColumnClick
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatAction")]
		[SRDescription("ListViewGroupTaskLinkClickDescr")]
		public event EventHandler<ListViewGroupEventArgs>? GroupTaskLinkClick
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListViewColumnReorderedDscr")]
		public event ColumnReorderedEventHandler? ColumnReordered
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListViewColumnWidthChangedDscr")]
		public event ColumnWidthChangedEventHandler? ColumnWidthChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListViewColumnWidthChangingDscr")]
		public event ColumnWidthChangingEventHandler? ColumnWidthChanging
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[SRDescription("ListViewDrawColumnHeaderEventDescr")]
		public event DrawListViewColumnHeaderEventHandler? DrawColumnHeader
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[SRDescription("ListViewDrawItemEventDescr")]
		public event DrawListViewItemEventHandler? DrawItem
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[SRDescription("ListViewDrawSubItemEventDescr")]
		public event DrawListViewSubItemEventHandler? DrawSubItem
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatAction")]
		[SRDescription("ListViewItemClickDescr")]
		public event EventHandler? ItemActivate
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[SRDescription("CheckedListBoxItemCheckDescr")]
		public event ItemCheckEventHandler? ItemCheck
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[SRDescription("ListViewItemCheckedDescr")]
		public event ItemCheckedEventHandler? ItemChecked
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatAction")]
		[SRDescription("ListViewItemDragDescr")]
		public event ItemDragEventHandler? ItemDrag
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatAction")]
		[SRDescription("ListViewItemMouseHoverDescr")]
		public event ListViewItemMouseHoverEventHandler? ItemMouseHover
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[SRDescription("ListViewItemSelectionChangedDescr")]
		public event ListViewItemSelectionChangedEventHandler? ItemSelectionChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[SRDescription("ListViewGroupCollapsedStateChangedDescr")]
		public event EventHandler<ListViewGroupEventArgs>? GroupCollapsedStateChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler? PaddingChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler? Paint
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatAction")]
		[SRDescription("ListViewRetrieveVirtualItemEventDescr")]
		public event RetrieveVirtualItemEventHandler? RetrieveVirtualItem
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatAction")]
		[SRDescription("ListViewSearchForVirtualItemDescr")]
		public event SearchForVirtualItemEventHandler? SearchForVirtualItem
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[SRDescription("ListViewSelectedIndexChangedDescr")]
		public event EventHandler? SelectedIndexChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		[SRCategory("CatBehavior")]
		[SRDescription("ListViewVirtualItemsSelectionRangeChangedDescr")]
		public event ListViewVirtualItemsSelectionRangeChangedEventHandler? VirtualItemsSelectionRangeChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		public ListView()
		{
			throw null;
		}

		internal void AnnounceColumnHeader(Point point)
		{
			throw null;
		}

		public void ArrangeIcons(ListViewAlignment value)
		{
			throw null;
		}

		public void ArrangeIcons()
		{
			throw null;
		}

		public void AutoResizeColumns(ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			throw null;
		}

		public void AutoResizeColumn(int columnIndex, ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			throw null;
		}

		public void BeginUpdate()
		{
			throw null;
		}

		internal void CacheSelectedStateForItem(ListViewItem lvi, bool selected)
		{
			throw null;
		}

		public void Clear()
		{
			throw null;
		}

		protected override void CreateHandle()
		{
			throw null;
		}

		protected override void Dispose(bool disposing)
		{
			throw null;
		}

		public void EndUpdate()
		{
			throw null;
		}

		public void EnsureVisible(int index)
		{
			throw null;
		}

		public ListViewItem? FindItemWithText(string text)
		{
			throw null;
		}

		public ListViewItem? FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex)
		{
			throw null;
		}

		public ListViewItem? FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex, bool isPrefixSearch)
		{
			throw null;
		}

		public ListViewItem? FindNearestItem(SearchDirectionHint dir, Point point)
		{
			throw null;
		}

		public ListViewItem? FindNearestItem(SearchDirectionHint searchDirection, int x, int y)
		{
			throw null;
		}

		internal int GetDisplayIndex(ListViewItem item, int lastIndex)
		{
			throw null;
		}

		internal int GetColumnIndex(ColumnHeader ch)
		{
			throw null;
		}

		public ListViewItem? GetItemAt(int x, int y)
		{
			throw null;
		}

		internal int GetNativeGroupId(ListViewItem item)
		{
			throw null;
		}

		internal override Interop.ComCtl32.ToolInfoWrapper<Control> GetToolInfoWrapper(TOOLTIP_FLAGS flags, string? caption, ToolTip tooltip)
		{
			throw null;
		}

		internal void GetSubItemAt(int x, int y, out int iItem, out int iSubItem)
		{
			throw null;
		}

		internal Point GetItemPosition(int index)
		{
			throw null;
		}

		internal LIST_VIEW_ITEM_STATE_FLAGS GetItemState(int index)
		{
			throw null;
		}

		internal LIST_VIEW_ITEM_STATE_FLAGS GetItemState(int index, LIST_VIEW_ITEM_STATE_FLAGS mask)
		{
			throw null;
		}

		public Rectangle GetItemRect(int index)
		{
			throw null;
		}

		public Rectangle GetItemRect(int index, ItemBoundsPortion portion)
		{
			throw null;
		}

		internal Rectangle GetSubItemRect(int itemIndex, int subItemIndex)
		{
			throw null;
		}

		internal Rectangle GetSubItemRect(int itemIndex, int subItemIndex, ItemBoundsPortion portion)
		{
			throw null;
		}

		public ListViewHitTestInfo HitTest(Point point)
		{
			throw null;
		}

		public ListViewHitTestInfo HitTest(int x, int y)
		{
			throw null;
		}

		internal ColumnHeader InsertColumn(int index, ColumnHeader ch, bool refreshSubItems = true)
		{
			throw null;
		}

		internal void InsertGroupInListView(int index, ListViewGroup group)
		{
			throw null;
		}

		protected override bool IsInputKey(Keys keyData)
		{
			throw null;
		}

		internal void ListViewItemToolTipChanged(ListViewItem item)
		{
			throw null;
		}

		protected virtual void OnAfterLabelEdit(LabelEditEventArgs e)
		{
			throw null;
		}

		protected override void OnBackgroundImageChanged(EventArgs e)
		{
			throw null;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			throw null;
		}

		protected override void OnMouseHover(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnBeforeLabelEdit(LabelEditEventArgs e)
		{
			throw null;
		}

		protected virtual void OnCacheVirtualItems(CacheVirtualItemsEventArgs e)
		{
			throw null;
		}

		protected virtual void OnGroupCollapsedStateChanged(ListViewGroupEventArgs e)
		{
			throw null;
		}

		protected virtual void OnColumnClick(ColumnClickEventArgs e)
		{
			throw null;
		}

		protected virtual void OnGroupTaskLinkClick(ListViewGroupEventArgs e)
		{
			throw null;
		}

		protected virtual void OnColumnReordered(ColumnReorderedEventArgs e)
		{
			throw null;
		}

		protected virtual void OnColumnWidthChanged(ColumnWidthChangedEventArgs e)
		{
			throw null;
		}

		protected virtual void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
		{
			throw null;
		}

		protected virtual void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
			throw null;
		}

		protected virtual void OnDrawItem(DrawListViewItemEventArgs e)
		{
			throw null;
		}

		protected virtual void OnDrawSubItem(DrawListViewSubItemEventArgs e)
		{
			throw null;
		}

		protected override void OnFontChanged(EventArgs e)
		{
			throw null;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			throw null;
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			throw null;
		}

		protected override void OnGotFocus(EventArgs e)
		{
			throw null;
		}

		protected override void OnLostFocus(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnItemActivate(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnItemCheck(ItemCheckEventArgs ice)
		{
			throw null;
		}

		protected virtual void OnItemChecked(ItemCheckedEventArgs e)
		{
			throw null;
		}

		protected virtual void OnItemDrag(ItemDragEventArgs e)
		{
			throw null;
		}

		protected virtual void OnItemMouseHover(ListViewItemMouseHoverEventArgs e)
		{
			throw null;
		}

		protected virtual void OnItemSelectionChanged(ListViewItemSelectionChangedEventArgs e)
		{
			throw null;
		}

		protected override void OnParentChanged(EventArgs e)
		{
			throw null;
		}

		protected override void OnResize(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnRetrieveVirtualItem(RetrieveVirtualItemEventArgs e)
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnSearchForVirtualItem(SearchForVirtualItemEventArgs e)
		{
			throw null;
		}

		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			throw null;
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnVirtualItemsSelectionRangeChanged(ListViewVirtualItemsSelectionRangeChangedEventArgs e)
		{
			throw null;
		}

		protected void RealizeProperties()
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void RedrawItems(int startIndex, int endIndex, bool invalidateOnly)
		{
			throw null;
		}

		internal override void ReleaseUiaProvider(HWND handle)
		{
			throw null;
		}

		internal void RemoveGroupFromListView(ListViewGroup group)
		{
			throw null;
		}

		internal void SetColumnInfo(LVCOLUMNW_MASK mask, ColumnHeader ch)
		{
			throw null;
		}

		internal void SetColumnWidth(int columnIndex, ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			throw null;
		}

		internal void UpdateSavedCheckedItems(ListViewItem item, bool addItem)
		{
			throw null;
		}

		internal override void SetToolTip(ToolTip toolTip)
		{
			throw null;
		}

		internal void SetItemImage(int itemIndex, int imageIndex)
		{
			throw null;
		}

		internal void SetItemIndentCount(int index, int indentCount)
		{
			throw null;
		}

		internal void SetItemPosition(int index, int x, int y)
		{
			throw null;
		}

		internal void SetItemState(int index, LIST_VIEW_ITEM_STATE_FLAGS state, LIST_VIEW_ITEM_STATE_FLAGS mask)
		{
			throw null;
		}

		internal void SetItemText(int itemIndex, int subItemIndex, string text)
		{
			throw null;
		}

		internal void SetSelectionMark(int itemIndex)
		{
			throw null;
		}

		public void Sort()
		{
			throw null;
		}

		public override string ToString()
		{
			throw null;
		}

		internal void UpdateListViewItemsLocations()
		{
			throw null;
		}

		protected void UpdateExtendedStyles()
		{
			throw null;
		}

		internal void UpdateGroupNative(ListViewGroup group)
		{
			throw null;
		}

		internal void UpdateGroupView()
		{
			throw null;
		}

		internal void RecreateHandleInternal()
		{
			throw null;
		}

		protected override void WndProc(ref Message m)
		{
			throw null;
		}

		protected override AccessibleObject CreateAccessibilityInstance()
		{
			throw null;
		}
	}
}
