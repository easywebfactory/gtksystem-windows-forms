using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

/// <summary>Manages a list of <see cref="T:System.Windows.Forms.Binding" /> objects.</summary>
/// <filterpriority>2</filterpriority>
public class CurrencyManager : BindingManagerBase
{
    private object? dataSource;

    private IList? list;

    private bool bound;

    private bool shouldBind = true;

    /// <summary>Specifies the current position of the <see cref="T:System.Windows.Forms.CurrencyManager" /> in the list.</summary>
    protected int listposition = -1;

    private int lastGoodKnownRow = -1;

    private bool pullingData;

    private bool inChangeRecordState;

    private bool suspendPushDataInCurrentChanged;

    private ItemChangedEventHandler? onItemChanged;

    private ListChangedEventHandler? onListChanged;

    private readonly ItemChangedEventArgs resetEvent = new(-1);

    private EventHandler? onMetaDataChangedHandler;

    /// <summary>Specifies the data type of the list.</summary>
    protected Type? finalType;

    internal bool AllowAdd
    {
        get
        {
            if (list is IBindingList)
            {
                return ((IBindingList)list).AllowNew;
            }
            if (list == null)
            {
                return false;
            }
            if (list.IsReadOnly)
            {
                return false;
            }
            return !list.IsFixedSize;
        }
    }

    internal bool AllowEdit
    {
        get
        {
            if (list is IBindingList)
            {
                return ((IBindingList)list).AllowEdit;
            }
            if (list == null)
            {
                return false;
            }
            return !list.IsReadOnly;
        }
    }

    internal bool AllowRemove
    {
        get
        {
            if (list is IBindingList)
            {
                return ((IBindingList)list).AllowRemove;
            }
            if (list == null)
            {
                return false;
            }
            if (list.IsReadOnly)
            {
                return false;
            }
            return !list.IsFixedSize;
        }
    }

    internal override Type? BindType => ListBindingHelper.GetListItemType(List);

    /// <summary>Gets the number of items in the list.</summary>
    /// <returns>The number of items in the list.</returns>
    /// <filterpriority>1</filterpriority>
    public override int Count
    {
        get
        {
            if (list == null)
            {
                return 0;
            }
            return list.Count;
        }
    }

    /// <summary>Gets the current item in the list.</summary>
    /// <returns>A list item of type <see cref="T:System.Object" />.</returns>
    /// <filterpriority>1</filterpriority>
    public override object? Current => this[Position];

    internal override object? DataSource => dataSource;

    internal override bool IsBinding => bound;

    internal object? this[int index]
    {
        get
        {
            if (index < 0 || index >= list?.Count)
            {
                throw new IndexOutOfRangeException("ListManagerNoValue");
            }
            return list?[index];
        }
        set
        {
            if (index < 0 || index >= list?.Count)
            {
                throw new IndexOutOfRangeException("ListManagerNoValue");
            }

            if (list != null)
            {
                list[index] = value;
            }
        }
    }

    /// <summary>Gets the list for this <see cref="T:System.Windows.Forms.CurrencyManager" />.</summary>
    /// <returns>An <see cref="T:System.Collections.IList" /> that contains the list.</returns>
    /// <filterpriority>1</filterpriority>
    public IList? List => list;

    /// <summary>Gets or sets the position you are at within the list.</summary>
    /// <returns>A number between 0 and <see cref="P:System.Windows.Forms.CurrencyManager.Count" /> minus 1.</returns>
    /// <filterpriority>1</filterpriority>
    public override int Position
    {
        get => listposition;
        set
        {
            if (listposition == -1)
            {
                return;
            }
            if (value < 0)
            {
                value = 0;
            }
            var count = list?.Count??0;
            if (value >= count)
            {
                value = count - 1;
            }
            ChangeRecordState(value, listposition != value, true, true, false);
        }
    }

    internal bool ShouldBind => shouldBind;

    internal CurrencyManager(object? dataSource)
    {
        Init(dataSource);
    }

    private void Init(object? source)
    {
        SetDataSource(source);
    }

    /// <summary>Adds a new item to the underlying list.</summary>
    /// <exception cref="T:System.NotSupportedException">The underlying data source does not implement <see cref="T:System.ComponentModel.IBindingList" />, or the data source has thrown an exception because the user has attempted to add a row to a read-only or fixed-size <see cref="T:System.Data.DataView" />. </exception>
    /// <filterpriority>1</filterpriority>
    public override void AddNew()
    {
        var bindingLists = list as IBindingList;
        if (bindingLists == null)
        {
            throw new NotSupportedException("CurrencyManagerCantAddNew");
        }
        bindingLists.AddNew();
        if (list != null)
        {
            ChangeRecordState(list.Count - 1, Position != list.Count - 1, Position != list.Count - 1, true, true);
        }
    }

    /// <summary>Cancels the current edit operation.</summary>
    /// <filterpriority>1</filterpriority>
    public override void CancelCurrentEdit()
    {
        if (Count > 0)
        {
            var editableObject = (list != null && (Position < 0 || Position >= list.Count) ? null : list?[Position]) as IEditableObject;
            editableObject?.CancelEdit();
            var cancelAddNew = list as ICancelAddNew;
            cancelAddNew?.CancelNew(Position);
            OnItemChanged(new ItemChangedEventArgs(Position));
            if (Position != -1)
            {
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, Position));
            }
        }
    }

    private void ChangeRecordState(int newPosition, bool validating, bool endCurrentEdit, bool firePositionChange, bool pullData)
    {
        if (newPosition == -1 && (list?.Count??0) == 0)
        {
            if (listposition != -1)
            {
                listposition = -1;
                OnPositionChanged(EventArgs.Empty);
            }
            return;
        }
        if ((newPosition < 0 || newPosition >= Count) && IsBinding)
        {
            throw new IndexOutOfRangeException("ListManagerBadPosition");
        }
        var num = listposition;
        if (endCurrentEdit)
        {
            inChangeRecordState = true;
            try
            {
                EndCurrentEdit();
            }
            finally
            {
                inChangeRecordState = false;
            }
        }
        if (validating & pullData)
        {
            CurrencyManager_PullData();
        }
        listposition = Math.Min(newPosition, Count - 1);
        if (validating)
        {
            OnCurrentChanged(EventArgs.Empty);
        }
        if (num != listposition & firePositionChange)
        {
            OnPositionChanged(EventArgs.Empty);
        }
    }

    /// <summary>Throws an exception if there is no list, or the list is empty.</summary>
    /// <exception cref="T:System.Exception">There is no list, or the list is empty. </exception>
    protected void CheckEmpty()
    {
        if (dataSource == null || list == null || list.Count == 0)
        {
            throw new InvalidOperationException("ListManagerEmptyList");
        }
    }

    private bool CurrencyManager_PullData()
    {
        bool flag;
        pullingData = true;
        try
        {
            PullData(out flag);
        }
        finally
        {
            pullingData = false;
        }
        return flag;
    }

    private bool CurrencyManager_PushData()
    {
        if (pullingData)
        {
            return false;
        }
        var num = listposition;
        if (lastGoodKnownRow != -1)
        {
            try
            {
                PushData();
            }
            catch (Exception exception)
            {
                OnDataError(exception);
                listposition = lastGoodKnownRow;
                PushData();
            }
            lastGoodKnownRow = listposition;
        }
        else
        {
            try
            {
                PushData();
            }
            catch (Exception exception1)
            {
                OnDataError(exception1);
                FindGoodRow();
            }
            lastGoodKnownRow = listposition;
        }
        return num != listposition;
    }

    /// <summary>Ends the current edit operation.</summary>
    /// <filterpriority>1</filterpriority>
    public override void EndCurrentEdit()
    {
        if (Count > 0 && CurrencyManager_PullData())
        {
            var editableObject = (list != null && (Position < 0 || Position >= list.Count) ? null : list?[Position]) as IEditableObject;
            editableObject?.EndEdit();
            var cancelAddNew = list as ICancelAddNew;
            cancelAddNew?.EndNew(Position);
        }
    }

    internal int Find(PropertyDescriptor property, object key, bool keepIndex)
    {
        if (key == null)
        {
            throw new ArgumentNullException("key");
        }
        if (property != null && list is IBindingList && ((IBindingList)list).SupportsSearching)
        {
            return ((IBindingList)list).Find(property, key);
        }
        if (property != null)
        {
            for (var i = 0; i < list?.Count; i++)
            {
                if (key.Equals(property.GetValue(list[i])))
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private void FindGoodRow()
    {
        var count = list?.Count??0;
        for (var i = 0; i < count; i++)
        {
            listposition = i;
            try
            {
                PushData();
                listposition = i;
                return;
            }
            catch (Exception exception)
            {
                OnDataError(exception);
            }
        }
        SuspendBinding();
        throw new InvalidOperationException("DataBindingPushDataException");
    }

    internal override PropertyDescriptorCollection? GetItemProperties(PropertyDescriptor?[]? listAccessors)
    {
        return ListBindingHelper.GetListItemProperties(list, listAccessors);
    }

    /// <summary>Gets the property descriptor collection for the underlying list.</summary>
    /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> for the list.</returns>
    /// <filterpriority>1</filterpriority>
    public override PropertyDescriptorCollection? GetItemProperties()
    {
        return GetItemProperties(null);
    }

    internal override string? GetListName()
    {
        if (!(list is ITypedList))
        {
            return finalType?.Name;
        }
        return ((ITypedList)list).GetListName(null);
    }

    /// <summary>Gets the name of the list supplying the data for the binding using the specified set of bound properties.</summary>
    /// <returns>If successful, a <see cref="T:System.String" /> containing name of the list supplying the data for the binding; otherwise, an <see cref="F:System.String.Empty" /> string.</returns>
    /// <param name="listAccessors">An <see cref="T:System.Collections.ArrayList" /> of properties to be found in the data source.</param>
    protected internal override string? GetListName(ArrayList? listAccessors)
    {
        if (!(list is ITypedList))
        {
            return "";
        }
        var propertyDescriptorArray = new PropertyDescriptor[listAccessors?.Count??0];
        if (listAccessors != null)
        {
            listAccessors.CopyTo(propertyDescriptorArray, 0);
        }

        return ((ITypedList)list).GetListName(propertyDescriptorArray);
    }

    internal ListSortDirection GetSortDirection()
    {
        if (!(list is IBindingList) || !((IBindingList)list).SupportsSorting)
        {
            return ListSortDirection.Ascending;
        }
        return ((IBindingList)list).SortDirection;
    }

    internal PropertyDescriptor? GetSortProperty()
    {
        if (!(list is IBindingList) || !((IBindingList)list).SupportsSorting)
        {
            return null;
        }
        return ((IBindingList)list).SortProperty;
    }

    private void List_ListChanged(object? sender, ListChangedEventArgs e)
    {
        ListChangedEventArgs listChangedEventArg;
        if (e.ListChangedType != ListChangedType.ItemMoved || e.OldIndex >= 0)
        {
            listChangedEventArg = e.ListChangedType != ListChangedType.ItemMoved || e.NewIndex >= 0 ? e : new ListChangedEventArgs(ListChangedType.ItemDeleted, e.OldIndex, e.NewIndex);
        }
        else
        {
            listChangedEventArg = new ListChangedEventArgs(ListChangedType.ItemAdded, e.NewIndex, e.OldIndex);
        }
        var num = listposition;
        UpdateLastGoodKnownRow(listChangedEventArg);
        UpdateIsBinding();
        if ((list?.Count??0) == 0)
        {
            listposition = -1;
            if (num != -1)
            {
                OnPositionChanged(EventArgs.Empty);
                OnCurrentChanged(EventArgs.Empty);
            }
            if (listChangedEventArg.ListChangedType == ListChangedType.Reset && e.NewIndex == -1)
            {
                OnItemChanged(resetEvent);
            }
            if (listChangedEventArg.ListChangedType == ListChangedType.ItemDeleted)
            {
                OnItemChanged(resetEvent);
            }
            if (e.ListChangedType == ListChangedType.PropertyDescriptorAdded || e.ListChangedType == ListChangedType.PropertyDescriptorDeleted || e.ListChangedType == ListChangedType.PropertyDescriptorChanged)
            {
                OnMetaDataChanged(EventArgs.Empty);
            }
            OnListChanged(listChangedEventArg);
            return;
        }
        suspendPushDataInCurrentChanged = true;
        try
        {
            switch (listChangedEventArg.ListChangedType)
            {
                case ListChangedType.Reset:
                {
                    if (listposition != -1 || (list?.Count??0) <= 0)
                    {
                        ChangeRecordState(Math.Min(listposition, list?.Count??0 - 1), true, false, true, false);
                    }
                    else
                    {
                        ChangeRecordState(0, true, false, true, false);
                    }
                    UpdateIsBinding(false);
                    OnItemChanged(resetEvent);
                    break;
                }
                case ListChangedType.ItemAdded:
                {
                    if (listChangedEventArg.NewIndex > listposition || listposition >= (list?.Count??0) - 1)
                    {
                        if (listChangedEventArg.NewIndex == listposition && listposition == list!.Count - 1 && listposition != -1)
                        {
                            OnCurrentItemChanged(EventArgs.Empty);
                        }
                        if (listposition == -1)
                        {
                            ChangeRecordState(0, false, false, true, false);
                        }
                        UpdateIsBinding();
                        OnItemChanged(resetEvent);
                        break;
                    }

                    ChangeRecordState(listposition + 1, true, true, listposition != (list?.Count??0) - 2, false);
                    UpdateIsBinding();
                    OnItemChanged(resetEvent);
                    if (listposition != (list?.Count ?? 0) - 1)
                    {
                        break;
                    }
                    OnPositionChanged(EventArgs.Empty);
                    break;
                }
                case ListChangedType.ItemDeleted:
                {
                    if (listChangedEventArg.NewIndex == listposition)
                    {
                        ChangeRecordState(Math.Min(listposition, Count - 1), true, false, true, false);
                        OnItemChanged(resetEvent);
                        break;
                    }

                    if (listChangedEventArg.NewIndex >= listposition)
                    {
                        OnItemChanged(resetEvent);
                        break;
                    }
                    ChangeRecordState(listposition - 1, true, false, true, false);
                    OnItemChanged(resetEvent);
                    break;
                }
                case ListChangedType.ItemMoved:
                {
                    if (listChangedEventArg.OldIndex == listposition)
                    {
                        ChangeRecordState(listChangedEventArg.NewIndex, true, Position > -1 && Position < (list?.Count??0), true, false);
                    }
                    else if (listChangedEventArg.NewIndex == listposition)
                    {
                        ChangeRecordState(listChangedEventArg.OldIndex, true, Position > -1 && Position < (list?.Count ?? 0), true, false);
                    }
                    OnItemChanged(resetEvent);
                    break;
                }
                case ListChangedType.ItemChanged:
                {
                    if (listChangedEventArg.NewIndex == listposition)
                    {
                        OnCurrentItemChanged(EventArgs.Empty);
                    }
                    OnItemChanged(new ItemChangedEventArgs(listChangedEventArg.NewIndex));
                    break;
                }
                case ListChangedType.PropertyDescriptorAdded:
                case ListChangedType.PropertyDescriptorDeleted:
                case ListChangedType.PropertyDescriptorChanged:
                {
                    lastGoodKnownRow = -1;
                    if (listposition == -1 && (list?.Count ?? 0) > 0)
                    {
                        ChangeRecordState(0, true, false, true, false);
                    }
                    else if (listposition > (list?.Count ?? 0) - 1)
                    {
                        ChangeRecordState((list?.Count ?? 0) - 1, true, false, true, false);
                    }
                    OnMetaDataChanged(EventArgs.Empty);
                    break;
                }
            }
            OnListChanged(listChangedEventArg);
        }
        finally
        {
            suspendPushDataInCurrentChanged = false;
        }
    }

    /// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected internal override void OnCurrentChanged(EventArgs e)
    {
        if (!inChangeRecordState)
        {
            var num = lastGoodKnownRow;
            var flag = false;
            if (!suspendPushDataInCurrentChanged)
            {
                flag = CurrencyManager_PushData();
            }
            if (Count > 0)
            {
                var item = list?[Position];
                (item as IEditableObject)?.BeginEdit();
            }
            try
            {
                if (!flag || flag && num != -1)
                {
                    onCurrentChangedHandler?.Invoke(this, e);
                    onCurrentItemChangedHandler?.Invoke(this, e);
                }
            }
            catch (Exception exception)
            {
                OnDataError(exception);
            }
        }
    }

    /// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected internal override void OnCurrentItemChanged(EventArgs e)
    {
        onCurrentItemChangedHandler?.Invoke(this, e);
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.CurrencyManager.ItemChanged" /> event.</summary>
    /// <param name="e">An <see cref="T:System.Windows.Forms.ItemChangedEventArgs" /> that contains the event data. </param>
    protected virtual void OnItemChanged(ItemChangedEventArgs e)
    {
        var flag = false;
        if ((e.Index == listposition || e.Index == -1 && Position < Count) && !inChangeRecordState)
        {
            flag = CurrencyManager_PushData();
        }
        try
        {
            onItemChanged?.Invoke(this, e);
        }
        catch (Exception exception)
        {
            OnDataError(exception);
        }
        if (flag)
        {
            OnPositionChanged(EventArgs.Empty);
        }
    }

    private void OnListChanged(ListChangedEventArgs e)
    {
        onListChanged?.Invoke(this, e);
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.CurrencyManager.MetaDataChanged" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected internal void OnMetaDataChanged(EventArgs e)
    {
        onMetaDataChangedHandler?.Invoke(this, e);
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.PositionChanged" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected virtual void OnPositionChanged(EventArgs e)
    {
        try
        {
            onPositionChangedHandler?.Invoke(this, e);
        }
        catch (Exception exception)
        {
            OnDataError(exception);
        }
    }

    /// <summary>Forces a repopulation of the data-bound list.</summary>
    /// <filterpriority>1</filterpriority>
    public void Refresh()
    {
        if ((list?.Count ?? 0) <= 0)
        {
            listposition = -1;
        }
        else if (listposition >= (list?.Count ?? 0))
        {
            lastGoodKnownRow = -1;
            listposition = 0;
        }
        List_ListChanged(list, new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    internal void Release()
    {
        UnwireEvents(list);
    }

    /// <summary>Removes the item at the specified index.</summary>
    /// <param name="index">The index of the item to remove from the list. </param>
    /// <exception cref="T:System.IndexOutOfRangeException">There is no row at the specified <paramref name="index" />. </exception>
    /// <filterpriority>1</filterpriority>
    public override void RemoveAt(int index)
    {
        list?.RemoveAt(index);
    }

    /// <summary>Resumes data binding.</summary>
    /// <filterpriority>1</filterpriority>
    public override void ResumeBinding()
    {
        lastGoodKnownRow = -1;
        try
        {
            if (!shouldBind)
            {
                shouldBind = true;
                listposition = list == null || list.Count == 0 ? -1 : 0;
                UpdateIsBinding();
            }
        }
        catch
        {
            shouldBind = false;
            UpdateIsBinding();
            throw;
        }
    }

    private protected override void SetDataSource(object? source)
    {
        if (dataSource == source)
        {
            return;
        }
        Release();
        dataSource = source;
        list = null;
        finalType = null;
        var listValue = source;
        if (listValue is Array)
        {
            finalType = listValue.GetType();
            listValue = (Array)listValue;
        }
        if (listValue is IListSource)
        {
            listValue = ((IListSource)listValue).GetList();
        }
        if (!(listValue is IList))
        {
            if (listValue != null)
            {
                throw new ArgumentException("ListManagerSetDataSource");
            }
            throw new ArgumentNullException("source");
        }
        if (finalType == null)
        {
            finalType = listValue.GetType();
        }
        list = (IList)listValue;
        WireEvents(list);
        if (list.Count <= 0)
        {
            listposition = -1;
        }
        else
        {
            listposition = 0;
        }
        OnItemChanged(resetEvent);
        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1, -1));
        UpdateIsBinding();
    }

    internal void SetSort(PropertyDescriptor property, ListSortDirection sortDirection)
    {
        if (list is IBindingList && ((IBindingList)list).SupportsSorting)
        {
            ((IBindingList)list).ApplySort(property, sortDirection);
        }
    }

    /// <summary>Suspends data binding to prevents changes from updating the bound data source.</summary>
    /// <filterpriority>1</filterpriority>
    public override void SuspendBinding()
    {
        lastGoodKnownRow = -1;
        if (shouldBind)
        {
            shouldBind = false;
            UpdateIsBinding();
        }
    }

    internal void UnwireEvents(IList? listValue)
    {
        if (listValue is IBindingList && ((IBindingList)listValue).SupportsChangeNotification)
        {
            ((IBindingList)listValue).ListChanged -= List_ListChanged;
        }
    }

    /// <summary>Updates the status of the binding.</summary>
    protected override void UpdateIsBinding()
    {
        UpdateIsBinding(true);
    }

    private void UpdateIsBinding(bool raiseItemChangedEvent)
    {
        var flag = list == null || list.Count <= 0 || !shouldBind ? false : listposition != -1;
        if (list != null && bound != flag)
        {
            bound = flag;
            var num = flag ? 0 : -1;
            ChangeRecordState(num, bound, Position != num, true, false);
            var count = Bindings.Count;
            for (var i = 0; i < count; i++)
            {
                Bindings[i].UpdateIsBinding();
            }
            if (raiseItemChangedEvent)
            {
                OnItemChanged(resetEvent);
            }
        }
    }

    private void UpdateLastGoodKnownRow(ListChangedEventArgs e)
    {
        switch (e.ListChangedType)
        {
            case ListChangedType.Reset:
            {
                lastGoodKnownRow = -1;
                return;
            }
            case ListChangedType.ItemAdded:
            {
                if (e.NewIndex > lastGoodKnownRow || lastGoodKnownRow >= (List?.Count??0) - 1)
                {
                    break;
                }
                lastGoodKnownRow++;
                return;
            }
            case ListChangedType.ItemDeleted:
            {
                if (e.NewIndex != lastGoodKnownRow)
                {
                    break;
                }
                lastGoodKnownRow = -1;
                return;
            }
            case ListChangedType.ItemMoved:
            {
                if (e.OldIndex != lastGoodKnownRow)
                {
                    break;
                }
                lastGoodKnownRow = e.NewIndex;
                return;
            }
            case ListChangedType.ItemChanged:
            {
                if (e.NewIndex != lastGoodKnownRow)
                {
                    break;
                }
                lastGoodKnownRow = -1;
                break;
            }
            default:
            {
                return;
            }
        }
    }

    internal void WireEvents(IList? listValue)
    {
        if (listValue is IBindingList && ((IBindingList)listValue).SupportsChangeNotification)
        {
            ((IBindingList)listValue).ListChanged += List_ListChanged;
        }
    }

    /// <summary>Occurs when the current item has been altered.</summary>
    /// <filterpriority>1</filterpriority>
    public event ItemChangedEventHandler? ItemChanged
    {
        add => onItemChanged += value;
        remove => onItemChanged -= value;
    }

    /// <summary>Occurs when the list changes or an item in the list changes.</summary>
    /// <filterpriority>1</filterpriority>
    public event ListChangedEventHandler? ListChanged
    {
        add => onListChanged += value;
        remove => onListChanged -= value;
    }

    /// <summary>Occurs when the metadata of the <see cref="P:System.Windows.Forms.CurrencyManager.List" /> has changed.</summary>
    /// <filterpriority>1</filterpriority>
    public event EventHandler? MetaDataChanged
    {
        add => onMetaDataChangedHandler += value;
        remove => onMetaDataChangedHandler -= value;
    }
}