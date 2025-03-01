using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms
{
    [DefaultProperty("DataSource")]
	[DefaultEvent("CurrentChanged")]
	[ComplexBindingProperties("DataSource", "DataMember")]
    public class BindingSource : Component, IBindingListView, ICollection, IEnumerable, IList, IBindingList, ITypedList, ICancelAddNew, ISupportInitializeNotification, ISupportInitialize, ICurrencyManagerProvider
	{
		[Browsable(false)]
		public virtual CurrencyManager CurrencyManager
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public object Current
		{
			get
			{
				throw null;
			}
		}


		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public string DataMember
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

		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		public object DataSource
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
		public bool IsBindingSuspended
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public IList List
		{
			get
			{
				throw null;
			}
		}

		[DefaultValue(-1)]
		[Browsable(false)]
		public int Position
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

		[DefaultValue(true)]
		[Browsable(false)]
		public bool RaiseListChangedEvents
		{
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}


		[DefaultValue(null)]
		public string Sort
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

		bool ISupportInitializeNotification.IsInitialized
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public virtual int Count
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public virtual bool IsSynchronized
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public virtual object SyncRoot
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public virtual object this[int index]
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
		public virtual bool IsFixedSize
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public virtual bool IsReadOnly
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public virtual bool AllowEdit
		{
			get
			{
				throw null;
			}
		}

		public virtual bool AllowNew
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
		public virtual bool AllowRemove
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public virtual bool SupportsChangeNotification
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public virtual bool SupportsSearching
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public virtual bool SupportsSorting
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public virtual bool IsSorted
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual PropertyDescriptor SortProperty
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual ListSortDirection SortDirection
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual ListSortDescriptionCollection SortDescriptions
		{
			get
			{
				throw null;
			}
		}

		[DefaultValue(null)]
		public virtual string Filter
		{
			get
			{
				return null;
			}
			//[RequiresUnreferencedCode("Members of types used in the filter expression might be trimmed.")]
			set
			{
				//throw null;
			}
		}

		[Browsable(false)]
		public virtual bool SupportsAdvancedSorting
		{
			get
			{
				throw null;
			}
		}

		[Browsable(false)]
		public virtual bool SupportsFiltering
		{
			get
			{
				throw null;
			}
		}

		public event AddingNewEventHandler AddingNew
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

		public event BindingCompleteEventHandler BindingComplete
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

		public event BindingManagerDataErrorEventHandler DataError
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

		public event EventHandler DataSourceChanged
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

		public event EventHandler DataMemberChanged
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

		public event EventHandler CurrentChanged
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


		public event EventHandler CurrentItemChanged
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

		public event ListChangedEventHandler ListChanged
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

		public event EventHandler PositionChanged
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

		event EventHandler ISupportInitializeNotification.Initialized
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

		public BindingSource()
		{
			throw null;
		}

		public BindingSource(object dataSource, string dataMember)
		{
			throw null;
		}

		public BindingSource(IContainer container)
		{
			throw null;
		}

		public virtual CurrencyManager GetRelatedCurrencyManager(string dataMember)
		{
			throw null;
		}

		public void CancelEdit()
		{
			throw null;
		}

		protected override void Dispose(bool disposing)
		{
			throw null;
		}

		public void EndEdit()
		{
			throw null;
		}

		public int Find(string propertyName, object key)
		{
			throw null;
		}

		public void MoveFirst()
		{
			throw null;
		}

		public void MoveLast()
		{
			throw null;
		}

		public void MoveNext()
		{
			throw null;
		}

		public void MovePrevious()
		{
			throw null;
		}

		protected virtual void OnAddingNew(AddingNewEventArgs e)
		{
			throw null;
		}

		protected virtual void OnBindingComplete(BindingCompleteEventArgs e)
		{
			throw null;
		}

		protected virtual void OnCurrentChanged(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnCurrentItemChanged(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnDataError(BindingManagerDataErrorEventArgs e)
		{
			throw null;
		}

		protected virtual void OnDataMemberChanged(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnDataSourceChanged(EventArgs e)
		{
			throw null;
		}

		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			throw null;
		}

		protected virtual void OnPositionChanged(EventArgs e)
		{
			throw null;
		}

		public void RemoveCurrent()
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void ResetAllowNew()
		{
			throw null;
		}

		public void ResetBindings(bool metadataChanged)
		{
			throw null;
		}

		public void ResetCurrentItem()
		{
			throw null;
		}

		public void ResetItem(int itemIndex)
		{
			throw null;
		}

		public void ResumeBinding()
		{
			throw null;
		}

		public void SuspendBinding()
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeAllowNew()
		{
			throw null;
		}

		void ISupportInitialize.BeginInit()
		{
			throw null;
		}

		void ISupportInitialize.EndInit()
		{
			throw null;
		}

		public virtual IEnumerator GetEnumerator()
		{
			throw null;
		}

		public virtual void CopyTo(Array arr, int index)
		{
			throw null;
		}

		public virtual int Add(object value)
		{
			throw null;
		}

		public virtual void Clear()
		{
			throw null;
		}

		public virtual bool Contains(object value)
		{
			throw null;
		}

		public virtual int IndexOf(object value)
		{
			throw null;
		}

		public virtual void Insert(int index, object value)
		{
			throw null;
		}

		public virtual void Remove(object value)
		{
			throw null;
		}

		public virtual void RemoveAt(int index)
		{
			throw null;
		}

		public virtual string GetListName(PropertyDescriptor[] listAccessors)
		{
			throw null;
		}

		public virtual PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			throw null;
		}

		public virtual object AddNew()
		{
			throw null;
		}

		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ApplySort(PropertyDescriptor property, ListSortDirection sort)
		{
			throw null;
		}

		public virtual int Find(PropertyDescriptor prop, object key)
		{
			throw null;
		}

		void IBindingList.RemoveIndex(PropertyDescriptor prop)
		{
			throw null;
		}

		public virtual void RemoveSort()
		{
			throw null;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ApplySort(ListSortDescriptionCollection sorts)
		{
			throw null;
		}

		public virtual void RemoveFilter()
		{
			throw null;
		}

		void ICancelAddNew.CancelNew(int position)
		{
			throw null;
		}

		void ICancelAddNew.EndNew(int position)
		{
			throw null;
		}
	}
}
