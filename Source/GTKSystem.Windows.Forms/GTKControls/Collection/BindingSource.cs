using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace System.Windows.Forms;

/// <summary>Encapsulates the data source for a form.</summary>
[ComplexBindingProperties("DataSource", "DataMember")]
[DefaultEvent("CurrentChanged")]
[DefaultProperty("DataSource")]
[Designer("System.Windows.Forms.Design.BindingSourceDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[Description("DescriptionBindingSource")]
public class BindingSource : Component, IBindingListView, ITypedList, ICancelAddNew, ISupportInitializeNotification, ICurrencyManagerProvider
{
    private static readonly object eventAddingnew;

    private static readonly object eventBindingcomplete;

    private static readonly object eventCurrentchanged;

    private static readonly object eventCurrentitemchanged;

    private static readonly object eventDataerror;

    private static readonly object eventDatamemberchanged;

    private static readonly object eventDatasourcechanged;

    private static readonly object eventListchanged;

    private static readonly object eventPositionchanged;

    private static readonly object eventInitialized;

    private object? dataSource;

    private string? dataMember;

    private string? sort;

    private string? filter;

    private readonly CurrencyManager? currencyManager;

    private bool raiseListChangedEvents = true;

    private bool parentsCurrentItemChanging;

    private bool disposedOrFinalized;

    private IList _innerList;

    private bool isBindingList;

    private bool listRaisesItemChangedEvents;

    private bool listExtractedFromEnumerable;

    private Type? itemType;

    private ConstructorInfo? itemConstructor;

    private PropertyDescriptorCollection? itemShape;

    private Dictionary<string, BindingSource>? relatedBindingSources;

    private bool allowNewIsSet;

    private bool allowNewSetValue = true;

    private object? currentItemHookedForItemChange;

    private object? lastCurrentItem;

    private readonly EventHandler listItemPropertyChangedHandler;

    private int addNewPos = -1;

    private bool initializing;

    private bool needToSetList;

    private bool recursionDetectionFlag;

    private bool innerListChanging;

    private bool endingEdit;

    /// <summary>Gets a value indicating whether items in the underlying list can be edited.</summary>
    /// <returns>true to indicate list items can be edited; otherwise, false.</returns>
    [Browsable(false)]
    public virtual bool AllowEdit
    {
        get
        {
            if (isBindingList)
            {
                return ((IBindingList)List).AllowEdit;
            }
            return !List.IsReadOnly;
        }
    }

    /// <summary>Gets or sets a value indicating whether the <see cref="M:System.Windows.Forms.BindingSource.AddNew" /> method can be used to add items to the list.</summary>
    /// <returns>true if <see cref="M:System.Windows.Forms.BindingSource.AddNew" /> can be used to add items to the list; otherwise, false.</returns>
    /// <exception cref="T:System.InvalidOperationException">This property is set to true when the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property has a fixed size or is read-only.</exception>
    /// <exception cref="T:System.MissingMethodException">The property is set to true and the <see cref="E:System.Windows.Forms.BindingSource.AddingNew" /> event is not handled when the underlying list type does not have a default constructor.</exception>
    [Category("CatBehavior")]
    [Description("BindingSourceAllowNewDescr")]
    public virtual bool AllowNew
    {
        get => AllowNewInternal(true);
        set
        {
            if (allowNewIsSet && value == allowNewSetValue)
            {
                return;
            }
            if (value && !isBindingList && !IsListWriteable(false))
            {
                throw new InvalidOperationException("NoAllowNewOnReadOnlyList");
            }
            allowNewIsSet = true;
            allowNewSetValue = value;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
    }

    /// <summary>Gets a value indicating whether items can be removed from the underlying list.</summary>
    /// <returns>true to indicate list items can be removed from the list; otherwise, false.</returns>
    [Browsable(false)]
    public virtual bool AllowRemove
    {
        get
        {
            if (isBindingList)
            {
                return ((IBindingList)List).AllowRemove;
            }
            if (List.IsReadOnly)
            {
                return false;
            }
            return !List.IsFixedSize;
        }
    }

    /// <summary>Gets the total number of items in the underlying list, taking the current <see cref="P:System.Windows.Forms.BindingSource.Filter" /> value into consideration.</summary>
    /// <returns>The total number of filtered items in the underlying list.</returns>
    [Browsable(false)]
    public virtual int Count
    {
        get
        {
            int count;
            try
            {
                if (!disposedOrFinalized)
                {
                    if (recursionDetectionFlag)
                    {
                        throw new InvalidOperationException("BindingSourceRecursionDetected");
                    }
                    recursionDetectionFlag = true;
                    count = List.Count;
                }
                else
                {
                    count = 0;
                }
            }
            finally
            {
                recursionDetectionFlag = false;
            }
            return count;
        }
    }

    /// <summary>Gets the currency manager associated with this <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
    /// <returns>The <see cref="T:System.Windows.Forms.CurrencyManager" /> associated with this <see cref="T:System.Windows.Forms.BindingSource" />.</returns>
    [Browsable(false)]
    public virtual CurrencyManager? CurrencyManager => ((ICurrencyManagerProvider)this).GetRelatedCurrencyManager(null);

    /// <summary>Gets the current item in the list.</summary>
    /// <returns>An <see cref="T:System.Object" /> that represents the current item in the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property, or null if the list has no items.</returns>
    [Browsable(false)]
    public object? Current
    {
        get
        {
            if (currencyManager is { Count: <= 0 })
            {
                return null;
            }
            return currencyManager?.Current;
        }
    }

    /// <summary>Gets or sets the specific list in the data source to which the connector currently binds to.</summary>
    /// <returns>The name of a list (or row) in the <see cref="P:System.Windows.Forms.BindingSource.DataSource" />. The default is an empty string ("").</returns>
    [DefaultValue("")]
    [RefreshProperties(RefreshProperties.Repaint)]
    [Category("CatData")]
    [Description("BindingSourceDataMemberDescr")]
    public string? DataMember
    {
        get => dataMember;
        set
        {
            if (value == null)
            {
                value = string.Empty;
            }
            if (dataMember == null || !dataMember.Equals(value))
            {
                dataMember = value;
                ResetList();
                OnDataMemberChanged(EventArgs.Empty);
            }
        }
    }

    /// <summary>Gets or sets the data source that the connector binds to.</summary>
    /// <returns>An <see cref="T:System.Object" /> that acts as a data source. The default is null.</returns>
    [AttributeProvider(typeof(IListSource))]
    [DefaultValue(null)]
    [RefreshProperties(RefreshProperties.Repaint)]
    [Category("CatData")]
    [Description("BindingSourceDataSourceDescr")]
    public object? DataSource
    {
        get => dataSource;
        set
        {
            if (dataSource != value)
            {
                ThrowIfBindingSourceRecursionDetected(value);
                UnwireDataSource();
                dataSource = value;
                ClearInvalidDataMember();
                ResetList();
                WireDataSource();
                OnDataSourceChanged(EventArgs.Empty);
            }
        }
    }

    /// <summary>Gets or sets the expression used to filter which rows are viewed.</summary>
    /// <returns>A string that specifies how rows are to be filtered. The default is null.</returns>
    [DefaultValue(null)]
    [Category("CatData")]
    [Description("BindingSourceFilterDescr")]
    public virtual string? Filter
    {
        get => filter;
        set
        {
            filter = value;
            InnerListFilter = value;
        }
    }

    private string? InnerListFilter
    {
        get
        {
            var list = List as IBindingListView;
            if (list == null || !list.SupportsFiltering)
            {
                return string.Empty;
            }
            return list.Filter;
        }
        set
        {
            if (initializing || DesignMode)
            {
                return;
            }
            if (string.Equals(value, InnerListFilter, StringComparison.Ordinal))
            {
                return;
            }
            var list = List as IBindingListView;
            if (list is { SupportsFiltering: true })
            {
                list.Filter = value;
            }
        }
    }

    private string? InnerListSort
    {
        get
        {
            ListSortDescriptionCollection? sortDescriptions = null;
            var list = List as IBindingListView;
            var bindingLists = List as IBindingList;
            if (list is { SupportsAdvancedSorting: true })
            {
                sortDescriptions = list.SortDescriptions;
            }
            else if (bindingLists is { SupportsSorting: true, IsSorted: true })
            {
                ListSortDescription[] listSortDescription = [new(bindingLists.SortProperty, bindingLists.SortDirection)
                ];
                sortDescriptions = new ListSortDescriptionCollection(listSortDescription);
            }
            return BuildSortString(sortDescriptions);
        }
        set
        {
            if (initializing || DesignMode)
            {
                return;
            }
            if (string.Compare(value, InnerListSort, false, CultureInfo.InvariantCulture) == 0)
            {
                return;
            }
            var listSortDescriptionCollections = ParseSortString(value);
            var list = List as IBindingListView;
            var bindingLists = List as IBindingList;
            if (list is { SupportsAdvancedSorting: true })
            {
                if (listSortDescriptionCollections.Count == 0)
                {
                    list.RemoveSort();
                    return;
                }
                list.ApplySort(listSortDescriptionCollections);
                return;
            }
            if (bindingLists is { SupportsSorting: true })
            {
                if (listSortDescriptionCollections.Count == 0)
                {
                    bindingLists.RemoveSort();
                    return;
                }
                if (listSortDescriptionCollections.Count == 1)
                {
                    bindingLists.ApplySort(listSortDescriptionCollections[0].PropertyDescriptor, listSortDescriptionCollections[0].SortDirection);
                }
            }
        }
    }

    /// <summary>Gets a value indicating whether the list binding is suspended.</summary>
    /// <returns>true to indicate the binding is suspended; otherwise, false. </returns>
    [Browsable(false)]
    public bool IsBindingSuspended => currencyManager is { IsBindingSuspended: true };

    /// <summary>Gets a value indicating whether the underlying list has a fixed size.</summary>
    /// <returns>true if the underlying list has a fixed size; otherwise, false.</returns>
    [Browsable(false)]
    public virtual bool IsFixedSize => List.IsFixedSize;

    /// <summary>Gets a value indicating whether the underlying list is read-only.</summary>
    /// <returns>true if the list is read-only; otherwise, false.</returns>
    [Browsable(false)]
    public virtual bool IsReadOnly => List.IsReadOnly;

    /// <summary>Gets a value indicating whether the items in the underlying list are sorted. </summary>
    /// <returns>true if the list is an <see cref="T:System.ComponentModel.IBindingList" /> and is sorted; otherwise, false. </returns>
    [Browsable(false)]
    public virtual bool IsSorted
    {
        get
        {
            if (!isBindingList)
            {
                return false;
            }
            return ((IBindingList)List).IsSorted;
        }
    }

    /// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
    /// <returns>true to indicate the list is synchronized; otherwise, false.</returns>
    [Browsable(false)]
    public virtual bool IsSynchronized => List.IsSynchronized;

    /// <summary>Gets or sets the list element at the specified index.</summary>
    /// <returns>The element at the specified index.</returns>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="index" /> is less than zero or is equal to or greater than <see cref="P:System.Windows.Forms.BindingSource.Count" />.</exception>
    [Browsable(false)]
    public virtual object this[int index]
    {
        get => List[index];
        set
        {
            List[index] = value;
            if (!isBindingList)
            {
                OnSimpleListChanged(ListChangedType.ItemChanged, index);
            }
        }
    }

    /// <summary>Gets the list that the connector is bound to.</summary>
    /// <returns>An <see cref="T:System.Collections.IList" /> that represents the list, or null if there is no underlying list associated with this <see cref="T:System.Windows.Forms.BindingSource" />.</returns>
    [Browsable(false)]
    public IList List
    {
        get
        {
            EnsureInnerList();
            return _innerList;
        }
    }

    /// <summary>Gets or sets the index of the current item in the underlying list.</summary>
    /// <returns>A zero-based index that specifies the position of the current item in the underlying list.</returns>
    [Browsable(false)]
    [DefaultValue(-1)]
    public int Position
    {
        get => currencyManager?.Position??0;
        set
        {
            if (currencyManager != null && currencyManager.Position != value)
            {
                currencyManager.Position = value;
            }
        }
    }

    /// <summary>Gets or sets a value indicating whether <see cref="E:System.Windows.Forms.BindingSource.ListChanged" /> events should be raised.</summary>
    /// <returns>true if <see cref="E:System.Windows.Forms.BindingSource.ListChanged" /> events should be raised; otherwise, false. The default is true.</returns>
    [Browsable(false)]
    [DefaultValue(true)]
    public bool RaiseListChangedEvents
    {
        get => raiseListChangedEvents;
        set => raiseListChangedEvents = value;
    }

    /// <summary>Gets or sets the column names used for sorting, and the sort order for viewing the rows in the data source.</summary>
    /// <returns>A case-sensitive string containing the column name followed by "ASC" (for ascending) or "DESC" (for descending). The default is null.</returns>
    [DefaultValue(null)]
    [Category("CatData")]
    [Description("BindingSourceSortDescr")]
    public string? Sort
    {
        get => sort;
        set
        {
            sort = value;
            InnerListSort = value;
        }
    }

    /// <summary>Gets the collection of sort descriptions applied to the data source.</summary>
    /// <returns>If the data source is an <see cref="T:System.ComponentModel.IBindingListView" />, a <see cref="T:System.ComponentModel.ListSortDescriptionCollection" /> that contains the sort descriptions applied to the list; otherwise, null.</returns>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual ListSortDescriptionCollection? SortDescriptions
    {
        get
        {
            var list = List as IBindingListView;
            if (list == null)
            {
                return null;
            }
            return list.SortDescriptions;
        }
    }

    /// <summary>Gets the direction the items in the list are sorted.</summary>
    /// <returns>One of the <see cref="T:System.ComponentModel.ListSortDirection" /> values indicating the direction the list is sorted.</returns>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual ListSortDirection SortDirection
    {
        get
        {
            if (!isBindingList)
            {
                return ListSortDirection.Ascending;
            }
            return ((IBindingList)List).SortDirection;
        }
    }

    /// <summary>Gets the <see cref="T:System.ComponentModel.PropertyDescriptor" /> that is being used for sorting the list.</summary>
    /// <returns>If the list is an <see cref="T:System.ComponentModel.IBindingList" />, the <see cref="T:System.ComponentModel.PropertyDescriptor" /> that is being used for sorting; otherwise, null.</returns>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual PropertyDescriptor? SortProperty
    {
        get
        {
            if (!isBindingList)
            {
                return null;
            }
            return ((IBindingList)List).SortProperty;
        }
    }

    /// <summary>Gets a value indicating whether the data source supports multi-column sorting.</summary>
    /// <returns>true if the list is an <see cref="T:System.ComponentModel.IBindingListView" /> and supports multi-column sorting; otherwise, false. </returns>
    [Browsable(false)]
    public virtual bool SupportsAdvancedSorting
    {
        get
        {
            var list = List as IBindingListView;
            if (list == null)
            {
                return false;
            }
            return list.SupportsAdvancedSorting;
        }
    }

    /// <summary>Gets a value indicating whether the data source supports change notification.</summary>
    /// <returns>true in all cases.</returns>
    [Browsable(false)]
    public virtual bool SupportsChangeNotification => true;

    /// <summary>Gets a value indicating whether the data source supports filtering.</summary>
    /// <returns>true if the list is an <see cref="T:System.ComponentModel.IBindingListView" /> and supports filtering; otherwise, false.</returns>
    [Browsable(false)]
    public virtual bool SupportsFiltering
    {
        get
        {
            var list = List as IBindingListView;
            if (list == null)
            {
                return false;
            }
            return list.SupportsFiltering;
        }
    }

    /// <summary>Gets a value indicating whether the data source supports searching with the <see cref="M:System.Windows.Forms.BindingSource.Find(System.ComponentModel.PropertyDescriptor,System.Object)" /> method.</summary>
    /// <returns>true if the list is a <see cref="T:System.ComponentModel.IBindingList" /> and supports the searching with the <see cref="Overload:System.Windows.Forms.BindingSource.Find" /> method; otherwise, false.</returns>
    [Browsable(false)]
    public virtual bool SupportsSearching
    {
        get
        {
            if (!isBindingList)
            {
                return false;
            }
            return ((IBindingList)List).SupportsSearching;
        }
    }

    /// <summary>Gets a value indicating whether the data source supports sorting.</summary>
    /// <returns>true if the data source is an <see cref="T:System.ComponentModel.IBindingList" /> and supports sorting; otherwise, false.</returns>
    [Browsable(false)]
    public virtual bool SupportsSorting
    {
        get
        {
            if (!isBindingList)
            {
                return false;
            }
            return ((IBindingList)List).SupportsSorting;
        }
    }

    /// <summary>Gets an object that can be used to synchronize access to the underlying list.</summary>
    /// <returns>An object that can be used to synchronize access to the underlying list.</returns>
    [Browsable(false)]
    public virtual object SyncRoot => List.SyncRoot;

    /// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.BindingSource" /> is initialized.</summary>
    /// <returns>true to indicate the <see cref="T:System.Windows.Forms.BindingSource" /> is initialized; otherwise, false.</returns>
    bool ISupportInitializeNotification.IsInitialized => !initializing;

    static BindingSource()
    {
        eventAddingnew = new object();
        eventBindingcomplete = new object();
        eventCurrentchanged = new object();
        eventCurrentitemchanged = new object();
        eventDataerror = new object();
        eventDatamemberchanged = new object();
        eventDatasourcechanged = new object();
        eventListchanged = new object();
        eventPositionchanged = new object();
        eventInitialized = new object();
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingSource" /> class to the default property values.</summary>
    public BindingSource() : this(null, string.Empty)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingSource" /> class with the specified data source and data member.</summary>
    /// <param name="dataSource">The data source for the <see cref="T:System.Windows.Forms.BindingSource" />.</param>
    /// <param name="dataMember">The specific column or list name within the data source to bind to.</param>
    public BindingSource(object? dataSource, string? dataMember)
    {
        this.dataSource = dataSource;
        this.dataMember = dataMember;
        _innerList = new ArrayList();
        currencyManager = new CurrencyManager(this);
        WireCurrencyManager(currencyManager);
        listItemPropertyChangedHandler = ListItem_PropertyChanged;
        ResetList();
        WireDataSource();
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingSource" /> class and adds the <see cref="T:System.Windows.Forms.BindingSource" /> to the specified container.</summary>
    /// <param name="container">The <see cref="T:System.ComponentModel.IContainer" /> to add the current <see cref="T:System.Windows.Forms.BindingSource" /> to.</param>
    public BindingSource(IContainer container) : this()
    {
        if (container == null)
        {
            throw new ArgumentNullException("container");
        }
        container.Add(this);
    }

    /// <summary>Adds an existing item to the internal list.</summary>
    /// <returns>The zero-based index at which <paramref name="value" /> was added to the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property. </returns>
    /// <param name="value">An <see cref="T:System.Object" /> to be added to the internal list.</param>
    /// <exception cref="T:System.InvalidOperationException">
    ///   <paramref name="value" /> differs in type from the existing items in the underlying list.</exception>
    public virtual int Add(object? value)
    {
        if (dataSource == null && List.Count == 0)
        {
            SetList(CreateBindingList(value == null ? typeof(object) : value.GetType()), true, true);
        }
        if (value != null && itemType != null && !itemType.IsInstanceOfType(value))
        {
            throw new InvalidOperationException("BindingSourceItemTypeMismatchOnAdd");
        }
        if (value == null && itemType is { IsValueType: true })
        {
            throw new InvalidOperationException("BindingSourceItemTypeIsValueType");
        }
        var num = List.Add(value);
        OnSimpleListChanged(ListChangedType.ItemAdded, num);
        return num;
    }

    /// <summary>Adds a new item to the underlying list.</summary>
    /// <returns>The <see cref="T:System.Object" /> that was created and added to the list.</returns>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.BindingSource.AllowNew" /> property is set to false. -or-A public default constructor could not be found for the current item type.</exception>
    public virtual object? AddNew()
    {
        if (!AllowNewInternal(false))
        {
            throw new InvalidOperationException("BindingSourceBindingListWrapperAddToReadOnlyList");
        }
        if (!AllowNewInternal(true))
        {
            throw new InvalidOperationException(string.Format("BindingSourceBindingListWrapperNeedToSetAllowNew {0}",  itemType == null ? "(null)" : itemType.FullName ));
        }
        var num = addNewPos;
        EndEdit();
        if (num != -1)
        {
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, num));
        }
        var addingNewEventArg = new AddingNewEventArgs();
        var count = List.Count;
        OnAddingNew(addingNewEventArg);
        var newObject = addingNewEventArg.NewObject;
        if (newObject == null)
        {
            if (isBindingList)
            {
                newObject = (List as IBindingList)?.AddNew();
                Position = Count - 1;
                return newObject;
            }
            if (itemConstructor == null)
            {
                throw new InvalidOperationException(string.Format("BindingSourceBindingListWrapperNeedAParameterlessConstructor {0}", itemType == null ? "(null)" : itemType.FullName ));
            }
            newObject = itemConstructor.Invoke(null);
        }
        if (List.Count <= count)
        {
            addNewPos = Add(newObject);
            Position = addNewPos;
        }
        else
        {
            addNewPos = Position;
        }
        return newObject;
    }

    private bool AllowNewInternal(bool checkconstructor)
    {
        if (disposedOrFinalized)
        {
            return false;
        }
        if (allowNewIsSet)
        {
            return allowNewSetValue;
        }
        if (listExtractedFromEnumerable)
        {
            return false;
        }
        if (!isBindingList)
        {
            return IsListWriteable(checkconstructor);
        }
        return ((IBindingList)List).AllowNew;
    }

    /// <summary>Sorts the data source using the specified property descriptor and sort direction.</summary>
    /// <param name="property">A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that describes the property by which to sort the data source.</param>
    /// <param name="sortDirection">A <see cref="T:System.ComponentModel.ListSortDirection" /> indicating how the list should be sorted.</param>
    /// <exception cref="T:System.NotSupportedException">The data source is not an <see cref="T:System.ComponentModel.IBindingList" />.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual void ApplySort(PropertyDescriptor property, ListSortDirection sortDirection)
    {
        if (!isBindingList)
        {
            throw new NotSupportedException("OperationRequiresIBindingList");
        }
        ((IBindingList)List).ApplySort(property, sortDirection);
    }

    /// <summary>Sorts the data source with the specified sort descriptions.</summary>
    /// <param name="sorts">A <see cref="T:System.ComponentModel.ListSortDescriptionCollection" /> containing the sort descriptions to apply to the data source.</param>
    /// <exception cref="T:System.NotSupportedException">The data source is not an <see cref="T:System.ComponentModel.IBindingListView" />.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual void ApplySort(ListSortDescriptionCollection sorts)
    {
        var list = List as IBindingListView;
        if (list == null)
        {
            throw new NotSupportedException("OperationRequiresIBindingListView");
        }
        list.ApplySort(sorts);
    }

    private static string BuildSortString(ListSortDescriptionCollection? sortsColln)
    {
        if (sortsColln == null)
        {
            return string.Empty;
        }
        var stringBuilder = new StringBuilder(sortsColln.Count);
        for (var i = 0; i < sortsColln.Count; i++)
        {
            stringBuilder.Append(string.Concat(sortsColln[i].PropertyDescriptor.Name, sortsColln[i].SortDirection == ListSortDirection.Ascending ? " ASC" : " DESC", i < sortsColln.Count - 1 ? "," : string.Empty));
        }
        return stringBuilder.ToString();
    }

    /// <summary>Cancels the current edit operation.</summary>
    public void CancelEdit()
    {
        currencyManager?.CancelCurrentEdit();
    }

    /// <summary>Removes all elements from the list.</summary>
    public virtual void Clear()
    {
        UnhookItemChangedEventsForOldCurrent();
        List.Clear();
        OnSimpleListChanged(ListChangedType.Reset, -1);
    }

    private void ClearInvalidDataMember()
    {
        if (!IsDataMemberValid())
        {
            dataMember = "";
            OnDataMemberChanged(EventArgs.Empty);
        }
    }

    /// <summary>Determines whether an object is an item in the list.</summary>
    /// <returns>true if the <paramref name="value" /> parameter is found in the <see cref="P:System.Windows.Forms.BindingSource.List" />; otherwise, false.</returns>
    /// <param name="value">The <see cref="T:System.Object" /> to locate in the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property. The value can be null. </param>
    public virtual bool Contains(object? value)
    {
        return List.Contains(value);
    }

    /// <summary>Copies the contents of the <see cref="P:System.Windows.Forms.BindingSource.List" /> to the specified array, starting at the specified index value.</summary>
    /// <param name="arr">The destination array.</param>
    /// <param name="index">The index in the destination array at which to start the copy operation.</param>
    public virtual void CopyTo(Array arr, int index)
    {
        List.CopyTo(arr, index);
    }

    private static IList? CreateBindingList(Type? type)
    {
        var bindingListType = typeof(BindingList<>);
        return (IList?)SecurityUtils.SecureCreateInstance(bindingListType.MakeGenericType(type));
    }

    private static object? CreateInstanceOfType(Type? type)
    {
        object? obj = null;
        Exception? exception = null;
        try
        {
            obj = SecurityUtils.SecureCreateInstance(type);
        }
        catch (TargetInvocationException? targetInvocationException)
        {
            exception = targetInvocationException;
        }
        catch (MethodAccessException? methodAccessException)
        {
            exception = methodAccessException;
        }
        catch (MissingMethodException? missingMethodException)
        {
            exception = missingMethodException;
        }
        if (exception != null)
        {
            throw new NotSupportedException("BindingSourceInstanceError", exception);
        }
        return obj;
    }

    private void CurrencyManager_BindingComplete(object sender, BindingCompleteEventArgs e)
    {
        OnBindingComplete(e);
    }

    private void CurrencyManager_CurrentChanged(object sender, EventArgs e)
    {
        OnCurrentChanged(EventArgs.Empty);
    }

    private void CurrencyManager_CurrentItemChanged(object sender, EventArgs e)
    {
        OnCurrentItemChanged(EventArgs.Empty);
    }

    private void CurrencyManager_DataError(object sender, BindingManagerDataErrorEventArgs e)
    {
        OnDataError(e);
    }

    private void CurrencyManager_PositionChanged(object sender, EventArgs e)
    {
        OnPositionChanged(e);
    }

    private void DataSource_Initialized(object sender, EventArgs e)
    {
        if (DataSource is ISupportInitializeNotification source)
        {
            source.Initialized -= DataSource_Initialized;
        }
        EndInitCore();
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.BindingSource" /> and optionally releases the managed resources. </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            UnwireDataSource();
            UnwireInnerList();
            UnhookItemChangedEventsForOldCurrent();
            UnwireCurrencyManager(currencyManager);
            dataSource = null;
            sort = null;
            dataMember = null;
            _innerList.Clear();
            isBindingList = false;
            needToSetList = true;
            raiseListChangedEvents = false;
        }
        disposedOrFinalized = true;
        base.Dispose(disposing);
    }

    /// <summary>Applies pending changes to the underlying data source.</summary>
    public void EndEdit()
    {
        if (endingEdit)
        {
            return;
        }
        try
        {
            endingEdit = true;
            currencyManager?.EndCurrentEdit();
        }
        finally
        {
            endingEdit = false;
        }
    }

    private void EndInitCore()
    {
        initializing = false;
        EnsureInnerList();
        OnInitialized();
    }

    private void EnsureInnerList()
    {
        if (!initializing && needToSetList)
        {
            needToSetList = false;
            ResetList();
        }
    }

    /// <summary>Returns the index of the item in the list with the specified property name and value.</summary>
    /// <returns>The zero-based index of the item with the specified property name and value. </returns>
    /// <param name="propertyName">The name of the property to search for.</param>
    /// <param name="key">The value of the item with the specified <paramref name="propertyName" /> to find.</param>
    /// <exception cref="T:System.InvalidOperationException">The underlying list is not a <see cref="T:System.ComponentModel.IBindingList" /> with searching functionality implemented.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="propertyName" /> does not match a property in the list.</exception>
    public int Find(string propertyName, object key)
    {
        PropertyDescriptor? propertyDescriptor;
        if (itemShape == null)
        {
            propertyDescriptor = null;
        }
        else
        {
            propertyDescriptor = itemShape.Find(propertyName, true);
        }
        var propertyDescriptor1 = propertyDescriptor;
        if (propertyDescriptor1 == null)
        {
            throw new ArgumentException(string.Format("DataSourceDataMemberPropNotFound {0}", propertyName));
        }
        return ((IBindingList)this).Find(propertyDescriptor1, key);
    }

    /// <summary>Searches for the index of the item that has the given property descriptor.</summary>
    /// <returns>The zero-based index of the item that has the given value for <see cref="T:System.ComponentModel.PropertyDescriptor" />.</returns>
    /// <param name="prop">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to search for. </param>
    /// <param name="key">The value of <paramref name="prop" /> to match. </param>
    /// <exception cref="T:System.NotSupportedException">The underlying list is not of type <see cref="T:System.ComponentModel.IBindingList" />.</exception>
    public virtual int Find(PropertyDescriptor prop, object key)
    {
        if (!isBindingList)
        {
            throw new NotSupportedException("OperationRequiresIBindingList");
        }
        return ((IBindingList)List).Find(prop, key);
    }

    /// <summary>Retrieves an enumerator for the <see cref="P:System.Windows.Forms.BindingSource.List" />.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="P:System.Windows.Forms.BindingSource.List" />. </returns>
    public virtual IEnumerator GetEnumerator()
    {
        return List.GetEnumerator();
    }

    /// <summary>Retrieves an array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects representing the bindable properties of the data source list type.</summary>
    /// <returns>An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects that represents the properties on this list type used to bind data.</returns>
    /// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects to find in the list as bindable.</param>
    public virtual PropertyDescriptorCollection? GetItemProperties(PropertyDescriptor[]? listAccessors)
    {
        var list = ListBindingHelper.GetList(dataSource);
        if (!(list is ITypedList) || string.IsNullOrEmpty(dataMember))
        {
            return ListBindingHelper.GetListItemProperties(List, listAccessors);
        }
        return ListBindingHelper.GetListItemProperties(list, dataMember, listAccessors);
    }

    private static IList? GetListFromEnumerable(IEnumerable enumerable)
    {
        IList? lists = null;
        foreach (var obj in enumerable)
        {
            if (lists == null)
            {
                lists = CreateBindingList(obj.GetType());
            }
            lists!.Add(obj);
        }
        return lists;
    }

    private static IList? GetListFromType(Type? type)
    {
        IList? lists;
        if (!typeof(ITypedList).IsAssignableFrom(type) || !typeof(IList).IsAssignableFrom(type))
        {
            lists = !typeof(IListSource).IsAssignableFrom(type) ? CreateBindingList(ListBindingHelper.GetListItemType(type)) : (CreateInstanceOfType(type) as IListSource)?.GetList();
        }
        else
        {
            lists = CreateInstanceOfType(type) as IList;
        }
        return lists;
    }

    /// <summary>Gets the name of the list supplying data for the binding.</summary>
    /// <returns>The name of the list supplying the data for binding.</returns>
    /// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects to find in the list as bindable.</param>
    public virtual string? GetListName(PropertyDescriptor[] listAccessors)
    {
        return ListBindingHelper.GetListName(List, listAccessors);
    }

    private BindingSource GetRelatedBindingSource(string? member)
    {
        BindingSource item;
        if (relatedBindingSources == null)
        {
            relatedBindingSources = new Dictionary<string, BindingSource>();
        }
        var enumerator = relatedBindingSources.Keys.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (!string.Equals(current, member, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                item = relatedBindingSources[current];
                return item;
            }
            var bindingSources = new BindingSource(this, member);
            if (member != null)
            {
                relatedBindingSources[member] = bindingSources;
            }

            return bindingSources;
        }
        finally
        {
            enumerator.Dispose();
        }
    }

    /// <summary>Gets the related currency manager for the specified data member.</summary>
    /// <returns>The related <see cref="T:System.Windows.Forms.CurrencyManager" /> for the specified data member.</returns>
    /// <param name="member">The name of column or list, within the data source to retrieve the currency manager for.</param>
    public virtual CurrencyManager? GetRelatedCurrencyManager(string? member)
    {
        EnsureInnerList();
        if (string.IsNullOrEmpty(member))
        {
            return currencyManager;
        }
        if ((member?.IndexOf(".", StringComparison.Ordinal)??-1) != -1)
        {
            return null;
        }
        return ((ICurrencyManagerProvider)GetRelatedBindingSource(member)).CurrencyManager;
    }

    private void HookItemChangedEventsForNewCurrent()
    {
        if (!listRaisesItemChangedEvents)
        {
            if (Position >= 0 && Position <= Count - 1)
            {
                currentItemHookedForItemChange = Current;
                WirePropertyChangedEvents(currentItemHookedForItemChange);
                return;
            }
            currentItemHookedForItemChange = null;
        }
    }

    /// <summary>Searches for the specified object and returns the index of the first occurrence within the entire list.</summary>
    /// <returns>The zero-based index of the first occurrence of the <paramref name="value" /> parameter; otherwise, -1 if <paramref name="value" /> is not in the list.</returns>
    /// <param name="value">The <see cref="T:System.Object" /> to locate in the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property. The value can be null. </param>
    public virtual int IndexOf(object value)
    {
        return List.IndexOf(value);
    }

    private void InnerList_ListChanged(object sender, ListChangedEventArgs e)
    {
        if (!innerListChanging)
        {
            try
            {
                innerListChanging = true;
                OnListChanged(e);
            }
            finally
            {
                innerListChanging = false;
            }
        }
    }

    /// <summary>Inserts an item into the list at the specified index.</summary>
    /// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted. </param>
    /// <param name="value">The <see cref="T:System.Object" /> to insert. The value can be null. </param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="index" /> is less than zero or greater than <see cref="P:System.Windows.Forms.BindingSource.Count" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The list is read-only or has a fixed size.</exception>
    public virtual void Insert(int index, object value)
    {
        List.Insert(index, value);
        OnSimpleListChanged(ListChangedType.ItemAdded, index);
    }

    private bool IsDataMemberValid()
    {
        if (initializing)
        {
            return true;
        }
        if (string.IsNullOrEmpty(dataMember))
        {
            return true;
        }
        if (dataMember != null && ListBindingHelper.GetListItemProperties(dataSource)?[dataMember] != null)
        {
            return true;
        }
        return false;
    }

    private bool IsListWriteable(bool checkconstructor)
    {
        if (List.IsReadOnly || List.IsFixedSize)
        {
            return false;
        }
        if (!checkconstructor)
        {
            return true;
        }
        return itemConstructor != null;
    }

    private void ListItem_PropertyChanged(object sender, EventArgs e)
    {
        int num;
        num = sender != currentItemHookedForItemChange ? ((IList)this).IndexOf(sender) : Position;
        OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, num));
    }

    /// <summary>Moves to the first item in the list.</summary>
    public void MoveFirst()
    {
        Position = 0;
    }

    /// <summary>Moves to the last item in the list.</summary>
    public void MoveLast()
    {
        Position = Count - 1;
    }

    /// <summary>Moves to the next item in the list.</summary>
    public void MoveNext()
    {
        Position = Position + 1;
    }

    /// <summary>Moves to the previous item in the list.</summary>
    public void MovePrevious()
    {
        Position = Position - 1;
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.AddingNew" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected virtual void OnAddingNew(AddingNewEventArgs e)
    {
        var item = (AddingNewEventHandler)Events[eventAddingnew];
        if (item != null)
        {
            item(this, e);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.BindingComplete" /> event. </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.BindingCompleteEventArgs" />  that contains the event data. </param>
    protected virtual void OnBindingComplete(BindingCompleteEventArgs e)
    {
        var item = (BindingCompleteEventHandler)Events[eventBindingcomplete];
        if (item != null)
        {
            item(this, e);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.CurrentChanged" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected virtual void OnCurrentChanged(EventArgs e)
    {
        UnhookItemChangedEventsForOldCurrent();
        HookItemChangedEventsForNewCurrent();
        var item = (EventHandler)Events[eventCurrentchanged];
        if (item != null)
        {
            item(this, e);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.CurrentItemChanged" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected virtual void OnCurrentItemChanged(EventArgs e)
    {
        var item = (EventHandler)Events[eventCurrentitemchanged];
        if (item != null)
        {
            item(this, e);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.DataError" /> event.</summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.BindingManagerDataErrorEventArgs" /> that contains the event data. </param>
    protected virtual void OnDataError(BindingManagerDataErrorEventArgs e)
    {
        var item = Events[eventDataerror] as BindingManagerDataErrorEventHandler;
        if (item != null)
        {
            item(this, e);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.DataMemberChanged" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected virtual void OnDataMemberChanged(EventArgs e)
    {
        var item = Events[eventDatamemberchanged] as EventHandler;
        if (item != null)
        {
            item(this, e);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.DataSourceChanged" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected virtual void OnDataSourceChanged(EventArgs e)
    {
        var item = Events[eventDatasourcechanged] as EventHandler;
        if (item != null)
        {
            item(this, e);
        }
    }

    private void OnInitialized()
    {
        var item = (EventHandler)Events[eventInitialized];
        if (item != null)
        {
            item(this, EventArgs.Empty);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.ListChanged" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected virtual void OnListChanged(ListChangedEventArgs e)
    {
        if (!raiseListChangedEvents || initializing)
        {
            return;
        }
        var item = (ListChangedEventHandler)Events[eventListchanged];
        if (item != null)
        {
            item(this, e);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.PositionChanged" /> event.</summary>
    /// <param name="e">A <see cref="T:System.ComponentModel.ListChangedEventArgs" /> that contains the event data.</param>
    protected virtual void OnPositionChanged(EventArgs e)
    {
        var item = (EventHandler)Events[eventPositionchanged];
        if (item != null)
        {
            item(this, e);
        }
    }

    private void OnSimpleListChanged(ListChangedType listChangedType, int newIndex)
    {
        if (!isBindingList)
        {
            OnListChanged(new ListChangedEventArgs(listChangedType, newIndex));
        }
    }

    private void ParentCurrencyManager_CurrentItemChanged(object sender, EventArgs e)
    {
        if (initializing)
        {
            return;
        }
        if (parentsCurrentItemChanging)
        {
            return;
        }
        try
        {
            parentsCurrentItemChanging = true;
            this.currencyManager?.PullData(out _);
        }
        finally
        {
            parentsCurrentItemChanging = false;
        }
        var manager = (CurrencyManager)sender;
        if (!string.IsNullOrEmpty(dataMember))
        {
            object? list = null;
            IList? lists = null;
            if (manager.Count > 0)
            {
                if (dataMember != null)
                {
                    var item = manager.GetItemProperties()?[dataMember];
                    if (item != null)
                    {
                        var managerCurrent = manager.Current;
                        if (managerCurrent != null)
                        {
                            list = ListBindingHelper.GetList(item.GetValue(managerCurrent));
                        }

                        lists = list as IList;
                    }
                }
            }
            if (lists != null)
            {
                SetList(lists, false, true);
            }
            else if (list == null)
            {
                SetList(CreateBindingList(itemType), false, false);
            }
            else
            {
                SetList(WrapObjectInBindingList(list), false, false);
            }
            var flag1 = lastCurrentItem == null || manager.Count == 0 || lastCurrentItem != manager.Current ? true : Position >= Count;
            lastCurrentItem = manager.Count > 0 ? manager.Current : null;
            if (flag1)
            {
                Position = Count > 0 ? 0 : -1;
            }
        }
        OnCurrentItemChanged(EventArgs.Empty);
    }

    private void ParentCurrencyManager_MetaDataChanged(object sender, EventArgs e)
    {
        ClearInvalidDataMember();
        ResetList();
    }

    private ListSortDescriptionCollection ParseSortString(string? sortString)
    {
        if (string.IsNullOrEmpty(sortString))
        {
            return new ListSortDescriptionCollection();
        }
        var arrayLists = new ArrayList();
        var itemProperties = currencyManager?.GetItemProperties();
        var strArrays = sortString?.Split(',')??[];
        for (var i = 0; i < strArrays.Length; i++)
        {
            var str = strArrays[i].Trim();
            var length = str.Length;
            var flag = true;
            if (length >= 5 && string.Compare(str, length - 4, " ASC", 0, 4, true, CultureInfo.InvariantCulture) == 0)
            {
                str = str.Substring(0, length - 4).Trim();
            }
            else if (length >= 6 && string.Compare(str, length - 5, " DESC", 0, 5, true, CultureInfo.InvariantCulture) == 0)
            {
                flag = false;
                str = str.Substring(0, length - 5).Trim();
            }
            if (str.StartsWith("["))
            {
                if (!str.EndsWith("]"))
                {
                    throw new ArgumentException("BindingSourceBadSortString");
                }
                str = str.Substring(1, str.Length - 2);
            }
            var propertyDescriptor = itemProperties?.Find(str, true);
            if (propertyDescriptor == null)
            {
                throw new ArgumentException("BindingSourceSortStringPropertyNotInIBindingList");
            }
            arrayLists.Add(new ListSortDescription(propertyDescriptor, flag ? ListSortDirection.Ascending : ListSortDirection.Descending));
        }
        var listSortDescriptionArray = new ListSortDescription[arrayLists.Count];
        arrayLists.CopyTo(listSortDescriptionArray);
        return new ListSortDescriptionCollection(listSortDescriptionArray);
    }

    /// <summary>Removes the specified item from the list.</summary>
    /// <param name="value">The item to remove from the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property.</param>
    /// <exception cref="T:System.NotSupportedException">The underlying list has a fixed size or is read-only. </exception>
    public virtual void Remove(object value)
    {
        var num = ((IList)this).IndexOf(value);
        List.Remove(value);
        if (num != -1)
        {
            OnSimpleListChanged(ListChangedType.ItemDeleted, num);
        }
    }

    /// <summary>Removes the item at the specified index in the list.</summary>
    /// <param name="index">The zero-based index of the item to remove. </param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="index" /> is less than zero or greater than the value of the <see cref="P:System.Windows.Forms.BindingSource.Count" /> property.</exception>
    /// <exception cref="T:System.NotSupportedException">The underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property is read-only or has a fixed size.</exception>
    public virtual void RemoveAt(int index)
    {
        List.RemoveAt(index);
        OnSimpleListChanged(ListChangedType.ItemDeleted, index);
    }

    /// <summary>Removes the current item from the list.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.BindingSource.AllowRemove" /> property is false.-or-<see cref="P:System.Windows.Forms.BindingSource.Position" /> is less than zero or greater than <see cref="P:System.Windows.Forms.BindingSource.Count" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property is read-only or has a fixed size.</exception>
    public void RemoveCurrent()
    {
        if (!((IBindingList)this).AllowRemove)
        {
            throw new InvalidOperationException("BindingSourceRemoveCurrentNotAllowed");
        }
        if (Position < 0 || Position >= Count)
        {
            throw new InvalidOperationException("BindingSourceRemoveCurrentNoCurrentItem");
        }
        RemoveAt(Position);
    }

    /// <summary>Removes the filter associated with the <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
    /// <exception cref="T:System.NotSupportedException">The underlying list does not support filtering.</exception>
    public virtual void RemoveFilter()
    {
        filter = null;
        var list = List as IBindingListView;
        if (list != null)
        {
            list.RemoveFilter();
        }
    }

    /// <summary>Removes the sort associated with the <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
    /// <exception cref="T:System.NotSupportedException">The underlying list does not support sorting.</exception>
    public virtual void RemoveSort()
    {
        sort = null;
        if (isBindingList)
        {
            ((IBindingList)List).RemoveSort();
        }
    }

    /// <summary>Reinitializes the <see cref="P:System.Windows.Forms.BindingSource.AllowNew" /> property.</summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public virtual void ResetAllowNew()
    {
        allowNewIsSet = false;
        allowNewSetValue = true;
    }

    /// <summary>Causes a control bound to the <see cref="T:System.Windows.Forms.BindingSource" /> to reread all the items in the list and refresh their displayed values. </summary>
    /// <param name="metadataChanged">true if the data schema has changed; false if only values have changed.</param>
    public void ResetBindings(bool metadataChanged)
    {
        if (metadataChanged)
        {
            OnListChanged(new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, null));
        }
        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    /// <summary>Causes a control bound to the <see cref="T:System.Windows.Forms.BindingSource" /> to reread the currently selected item and refresh its displayed value.</summary>
    public void ResetCurrentItem()
    {
        OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, Position));
    }

    /// <summary>Causes a control bound to the <see cref="T:System.Windows.Forms.BindingSource" /> to reread the item at the specified index, and refresh its displayed value. </summary>
    /// <param name="itemIndex">The zero-based index of the item that has changed.</param>
    public void ResetItem(int itemIndex)
    {
        OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, itemIndex));
    }

    private void ResetList()
    {
        object? listFromType;
        if (initializing)
        {
            needToSetList = true;
            return;
        }
        needToSetList = false;
        if (dataSource is Type)
        {
            listFromType = GetListFromType(dataSource as Type);
        }
        else
        {
            listFromType = dataSource;
        }
        var list = ListBindingHelper.GetList(listFromType, dataMember);
        listExtractedFromEnumerable = false;
        IList? listFromEnumerable = null;
        if (!(list is IList))
        {
            if (list is IListSource)
            {
                listFromEnumerable = (list as IListSource)?.GetList();
            }
            else if (list is IEnumerable enumerable)
            {
                listFromEnumerable = GetListFromEnumerable(enumerable);
                if (listFromEnumerable != null)
                {
                    listExtractedFromEnumerable = true;
                }
            }
            if (listFromEnumerable == null)
            {
                if (list == null)
                {
                    var listItemType = ListBindingHelper.GetListItemType(dataSource, dataMember);
                    listFromEnumerable = GetListFromType(listItemType) ?? CreateBindingList(listItemType);
                }
                else
                {
                    listFromEnumerable = WrapObjectInBindingList(list);
                }
            }
        }
        else
        {
            listFromEnumerable = list as IList;
        }
        SetList(listFromEnumerable, true, true);
    }

    /// <summary>Resumes data binding.</summary>
    public void ResumeBinding()
    {
        currencyManager?.ResumeBinding();
    }

    private void SetList(IList? list, bool metaDataChanged, bool applySortAndFilter)
    {
        list ??= CreateBindingList(itemType);
        UnwireInnerList();
        UnhookItemChangedEventsForOldCurrent();
        var lists = ListBindingHelper.GetList(list) as IList ?? list;
        _innerList = lists!;
        isBindingList = lists is IBindingList;
        listRaisesItemChangedEvents = !(lists is IRaiseItemChangedEvents events) ? isBindingList : events.RaisesItemChangedEvents;
        if (metaDataChanged)
        {
            itemType = ListBindingHelper.GetListItemType(List);
            itemShape = ListBindingHelper.GetListItemProperties(List);
            itemConstructor = itemType?.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, [], null);
        }
        WireInnerList();
        HookItemChangedEventsForNewCurrent();
        ResetBindings(metaDataChanged);
        if (applySortAndFilter)
        {
            if (Sort != null)
            {
                InnerListSort = Sort;
            }
            if (Filter != null)
            {
                InnerListFilter = Filter;
            }
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal virtual bool ShouldSerializeAllowNew()
    {
        return allowNewIsSet;
    }

    /// <summary>Suspends data binding to prevent changes from updating the bound data source.</summary>
    public void SuspendBinding()
    {
        currencyManager?.SuspendBinding();
    }

    /// <summary>Adds the <see cref="T:System.ComponentModel.PropertyDescriptor" /> to the indexes used for searching.</summary>
    /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to add to the indexes used for searching. </param>
    /// <exception cref="T:System.NotSupportedException">The underlying list is not an <see cref="T:System.ComponentModel.IBindingList" />.</exception>
    void IBindingList.AddIndex(PropertyDescriptor property)
    {
        if (!isBindingList)
        {
            throw new NotSupportedException("OperationRequiresIBindingList");
        }
        ((IBindingList)List).AddIndex(property);
    }

    /// <summary>Removes the <see cref="T:System.ComponentModel.PropertyDescriptor" /> from the indexes used for searching.</summary>
    /// <param name="prop">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to remove from the indexes used for searching.  </param>
    void IBindingList.RemoveIndex(PropertyDescriptor prop)
    {
        if (!isBindingList)
        {
            throw new NotSupportedException("OperationRequiresIBindingList");
        }
        ((IBindingList)List).RemoveIndex(prop);
    }

    /// <summary>Discards a pending new item from the collection.</summary>
    /// <param name="position">The index of the item that was added to the collection. </param>
    void ICancelAddNew.CancelNew(int position)
    {
        if (addNewPos >= 0 && addNewPos == position)
        {
            RemoveAt(addNewPos);
            addNewPos = -1;
            return;
        }
        var list = List as ICancelAddNew;
        if (list != null)
        {
            list.CancelNew(position);
        }
    }

    /// <summary>Commits a pending new item to the collection.</summary>
    /// <param name="position">The index of the item that was added to the collection. </param>
    void ICancelAddNew.EndNew(int position)
    {
        if (addNewPos >= 0 && addNewPos == position)
        {
            addNewPos = -1;
            return;
        }
        var list = List as ICancelAddNew;
        if (list != null)
        {
            list.EndNew(position);
        }
    }

    /// <summary>Signals the <see cref="T:System.Windows.Forms.BindingSource" /> that initialization is starting.</summary>
    void ISupportInitialize.BeginInit()
    {
        initializing = true;
    }

    /// <summary>Signals the <see cref="T:System.Windows.Forms.BindingSource" /> that initialization is complete. </summary>
    void ISupportInitialize.EndInit()
    {
        var source = DataSource as ISupportInitializeNotification;
        if (source == null || source.IsInitialized)
        {
            EndInitCore();
            return;
        }
        source.Initialized += DataSource_Initialized;
    }

    private void ThrowIfBindingSourceRecursionDetected(object? newDataSource)
    {
        for (var i = newDataSource as BindingSource; i != null; i = i.DataSource as BindingSource)
        {
            if (i == this)
            {
                throw new InvalidOperationException("BindingSourceRecursionDetected");
            }
        }
    }

    private void UnhookItemChangedEventsForOldCurrent()
    {
        if (!listRaisesItemChangedEvents)
        {
            UnwirePropertyChangedEvents(currentItemHookedForItemChange);
            currentItemHookedForItemChange = null;
        }
    }

    private void UnwireCurrencyManager(CurrencyManager? cm)
    {
        if (cm != null)
        {
            cm.PositionChanged -= CurrencyManager_PositionChanged;
            cm.CurrentChanged -= CurrencyManager_CurrentChanged;
            cm.CurrentItemChanged -= CurrencyManager_CurrentItemChanged;
            cm.BindingComplete -= CurrencyManager_BindingComplete;
            cm.DataError -= CurrencyManager_DataError;
        }
    }

    private void UnwireDataSource()
    {
        if (dataSource is ICurrencyManagerProvider)
        {
            var manager = (dataSource as ICurrencyManagerProvider)?.CurrencyManager;
            if (manager != null)
            {
                manager.CurrentItemChanged -= ParentCurrencyManager_CurrentItemChanged;
                manager.MetaDataChanged -= ParentCurrencyManager_MetaDataChanged;
            }
        }
    }

    private void UnwireInnerList()
    {
        if (_innerList is IBindingList list)
        {
            list.ListChanged -= InnerList_ListChanged;
        }
    }

    private void UnwirePropertyChangedEvents(object? item)
    {
        if (item != null && itemShape != null)
        {
            for (var i = 0; i < itemShape.Count; i++)
            {
                itemShape[i].RemoveValueChanged(item, listItemPropertyChangedHandler);
            }
        }
    }

    private void WireCurrencyManager(CurrencyManager? cm)
    {
        if (cm != null)
        {
            cm.PositionChanged += CurrencyManager_PositionChanged;
            cm.CurrentChanged += CurrencyManager_CurrentChanged;
            cm.CurrentItemChanged += CurrencyManager_CurrentItemChanged;
            cm.BindingComplete += CurrencyManager_BindingComplete;
            cm.DataError += CurrencyManager_DataError;
        }
    }

    private void WireDataSource()
    {
        if (dataSource is ICurrencyManagerProvider)
        {
            var manager = (dataSource as ICurrencyManagerProvider)?.CurrencyManager;
            if (manager != null)
            {
                manager.CurrentItemChanged += ParentCurrencyManager_CurrentItemChanged;
                manager.MetaDataChanged += ParentCurrencyManager_MetaDataChanged;
            }
        }
    }

    private void WireInnerList()
    {
        if (_innerList is IBindingList list)
        {
            list.ListChanged += InnerList_ListChanged;
        }
    }

    private void WirePropertyChangedEvents(object? item)
    {
        if (item != null && itemShape != null)
        {
            for (var i = 0; i < itemShape.Count; i++)
            {
                itemShape[i].AddValueChanged(item, listItemPropertyChangedHandler);
            }
        }
    }

    private static IList? WrapObjectInBindingList(object? obj)
    {
        var lists = CreateBindingList(obj?.GetType());
        lists?.Add(obj);
        return lists;
    }

    /// <summary>Occurs before an item is added to the underlying list.</summary>
    /// <exception cref="T:System.InvalidOperationException">
    ///   <see cref="P:System.ComponentModel.AddingNewEventArgs.NewObject" /> is not the same type as the type contained in the list.</exception>
    [Category("CatData")]
    [Description("BindingSourceAddingNewEventHandlerDescr")]
    public event AddingNewEventHandler AddingNew
    {
        add => Events.AddHandler(eventAddingnew, value);
        remove => Events.RemoveHandler(eventAddingnew, value);
    }

    /// <summary>Occurs when all the clients have been bound to this <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
    [Category("CatData")]
    [Description("BindingSourceBindingCompleteEventHandlerDescr")]
    public event BindingCompleteEventHandler BindingComplete
    {
        add => Events.AddHandler(eventBindingcomplete, value);
        remove => Events.RemoveHandler(eventBindingcomplete, value);
    }

    /// <summary>Occurs when the currently bound item changes.</summary>
    [Category("CatData")]
    [Description("BindingSourceCurrentChangedEventHandlerDescr")]
    public event EventHandler CurrentChanged
    {
        add => Events.AddHandler(eventCurrentchanged, value);
        remove => Events.RemoveHandler(eventCurrentchanged, value);
    }

    /// <summary>Occurs when a property value of the <see cref="P:System.Windows.Forms.BindingSource.Current" /> property has changed.</summary>
    [Category("CatData")]
    [Description("BindingSourceCurrentItemChangedEventHandlerDescr")]
    public event EventHandler CurrentItemChanged
    {
        add => Events.AddHandler(eventCurrentitemchanged, value);
        remove => Events.RemoveHandler(eventCurrentitemchanged, value);
    }

    /// <summary>Occurs when a currency-related exception is silently handled by the <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
    [Category("CatData")]
    [Description("BindingSourceDataErrorEventHandlerDescr")]
    public event BindingManagerDataErrorEventHandler DataError
    {
        add => Events.AddHandler(eventDataerror, value);
        remove => Events.RemoveHandler(eventDataerror, value);
    }

    /// <summary>Occurs when the <see cref="P:System.Windows.Forms.BindingSource.DataMember" /> property value has changed.</summary>
    [Category("CatData")]
    [Description("BindingSourceDataMemberChangedEventHandlerDescr")]
    public event EventHandler DataMemberChanged
    {
        add => Events.AddHandler(eventDatamemberchanged, value);
        remove => Events.RemoveHandler(eventDatamemberchanged, value);
    }

    /// <summary>Occurs when the <see cref="P:System.Windows.Forms.BindingSource.DataSource" /> property value has changed.</summary>
    [Category("CatData")]
    [Description("BindingSourceDataSourceChangedEventHandlerDescr")]
    public event EventHandler DataSourceChanged
    {
        add => Events.AddHandler(eventDatasourcechanged, value);
        remove => Events.RemoveHandler(eventDatasourcechanged, value);
    }

    /// <summary>Occurs when the underlying list changes or an item in the list changes.</summary>
    [Category("CatData")]
    [Description("BindingSourceListChangedEventHandlerDescr")]
    public event ListChangedEventHandler ListChanged
    {
        add => Events.AddHandler(eventListchanged, value);
        remove => Events.RemoveHandler(eventListchanged, value);
    }

    /// <summary>Occurs after the value of the <see cref="P:System.Windows.Forms.BindingSource.Position" /> property has changed.</summary>
    [Category("CatData")]
    [Description("BindingSourcePositionChangedEventHandlerDescr")]
    public event EventHandler PositionChanged
    {
        add => Events.AddHandler(eventPositionchanged, value);
        remove => Events.RemoveHandler(eventPositionchanged, value);
    }

    /// <summary>Occurs when the <see cref="T:System.Windows.Forms.BindingSource" /> is initialized.</summary>
    event EventHandler ISupportInitializeNotification.Initialized
    {
        add => Events.AddHandler(eventInitialized, value);
        remove => Events.RemoveHandler(eventInitialized, value);
    }
}