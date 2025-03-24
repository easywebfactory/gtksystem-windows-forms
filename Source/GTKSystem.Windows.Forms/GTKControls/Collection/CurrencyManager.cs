using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

/// <summary>Manages a list of <see cref="T:System.Windows.Forms.Binding" /> objects.</summary>
/// <filterpriority>2</filterpriority>
public class CurrencyManager : BindingManagerBase
{
    private object? _dataSource;

    private IList? _list;

    private bool _bound;

    private bool _shouldBind = true;

    /// <summary>Specifies the current position of the <see cref="T:System.Windows.Forms.CurrencyManager" /> in the list.</summary>
    protected int ListPosition = -1;

    private int _lastGoodKnownRow = -1;

    private bool _pullingData;

    private bool _inChangeRecordState;

    private bool _suspendPushDataInCurrentChanged;

    private ItemChangedEventHandler? _itemChanged;

    private ListChangedEventHandler? _listChanged;

    private readonly ItemChangedEventArgs _resetEvent = new(-1);

    private EventHandler? _metaDataChanged;

    /// <summary>Specifies the data type of the list.</summary>
    protected Type? FinalType;

    internal bool AllowAdd
    {
        get
        {
            if (_list is IBindingList)
            {
                return ((IBindingList)_list).AllowNew;
            }
            if (_list == null)
            {
                return false;
            }
            if (_list.IsReadOnly)
            {
                return false;
            }
            return !_list.IsFixedSize;
        }
    }

    internal bool AllowEdit
    {
        get
        {
            if (_list is IBindingList)
            {
                return ((IBindingList)_list).AllowEdit;
            }
            if (_list == null)
            {
                return false;
            }
            return !_list.IsReadOnly;
        }
    }

    internal bool AllowRemove
    {
        get
        {
            if (_list is IBindingList)
            {
                return ((IBindingList)_list).AllowRemove;
            }
            if (_list == null)
            {
                return false;
            }
            if (_list.IsReadOnly)
            {
                return false;
            }
            return !_list.IsFixedSize;
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
            if (_list == null)
            {
                return 0;
            }
            return _list.Count;
        }
    }

    /// <summary>Gets the current item in the list.</summary>
    /// <returns>A list item of type <see cref="T:System.Object" />.</returns>
    /// <filterpriority>1</filterpriority>
    public override object? Current => this[Position];

    internal override object? DataSource => _dataSource;

    internal override bool IsBinding => _bound;

    internal object? this[int index]
    {
        get
        {
            if (index < 0 || index >= _list?.Count)
            {
                throw new IndexOutOfRangeException("ListManagerNoValue");
            }
            return _list?[index];
        }
        set
        {
            if (index < 0 || index >= _list?.Count)
            {
                throw new IndexOutOfRangeException("ListManagerNoValue");
            }

            if (_list != null)
            {
                _list[index] = value;
            }
        }
    }

    /// <summary>Gets the list for this <see cref="T:System.Windows.Forms.CurrencyManager" />.</summary>
    /// <returns>An <see cref="T:System.Collections.IList" /> that contains the list.</returns>
    /// <filterpriority>1</filterpriority>
    public IList? List => _list;

    /// <summary>Gets or sets the position you are at within the list.</summary>
    /// <returns>A number between 0 and <see cref="P:System.Windows.Forms.CurrencyManager.Count" /> minus 1.</returns>
    /// <filterpriority>1</filterpriority>
    public override int Position
    {
        get => ListPosition;
        set
        {
            if (ListPosition == -1)
            {
                return;
            }
            if (value < 0)
            {
                value = 0;
            }
            var count = _list?.Count ?? 0;
            if (value >= count)
            {
                value = count - 1;
            }
            ChangeRecordState(value, ListPosition != value, true, true, false);
        }
    }

    internal bool ShouldBind => _shouldBind;

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
        if (_list is not IBindingList bindingLists)
        {
            throw new NotSupportedException("CurrencyManagerCantAddNew");
        }
        bindingLists.AddNew();
        if (_list != null)
        {
            ChangeRecordState(_list.Count - 1, Position != _list.Count - 1, Position != _list.Count - 1, true, true);
        }
    }

    /// <summary>Cancels the current edit operation.</summary>
    /// <filterpriority>1</filterpriority>
    public override void CancelCurrentEdit()
    {
        if (Count > 0)
        {
            var editableObject = (_list != null && (Position < 0 || Position >= _list.Count) ? null : _list?[Position]) as IEditableObject;
            editableObject?.CancelEdit();
            var cancelAddNew = _list as ICancelAddNew;
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
        if (newPosition == -1 && (_list?.Count ?? 0) == 0)
        {
            if (ListPosition != -1)
            {
                ListPosition = -1;
                OnPositionChanged(EventArgs.Empty);
            }
            return;
        }
        if ((newPosition < 0 || newPosition >= Count) && IsBinding)
        {
            throw new IndexOutOfRangeException("ListManagerBadPosition");
        }
        var num = ListPosition;
        if (endCurrentEdit)
        {
            _inChangeRecordState = true;
            try
            {
                EndCurrentEdit();
            }
            finally
            {
                _inChangeRecordState = false;
            }
        }
        if (validating & pullData)
        {
            CurrencyManager_PullData();
        }
        ListPosition = Math.Min(newPosition, Count - 1);
        if (validating)
        {
            OnCurrentChanged(EventArgs.Empty);
        }
        if (num != ListPosition & firePositionChange)
        {
            OnPositionChanged(EventArgs.Empty);
        }
    }

    /// <summary>Throws an exception if there is no list, or the list is empty.</summary>
    /// <exception cref="T:System.Exception">There is no list, or the list is empty. </exception>
    protected void CheckEmpty()
    {
        if (_dataSource == null || _list == null || _list.Count == 0)
        {
            throw new InvalidOperationException("ListManagerEmptyList");
        }
    }

    private bool CurrencyManager_PullData()
    {
        bool flag;
        _pullingData = true;
        try
        {
            PullData(out flag);
        }
        finally
        {
            _pullingData = false;
        }
        return flag;
    }

    private bool CurrencyManager_PushData()
    {
        if (_pullingData)
        {
            return false;
        }
        var num = ListPosition;
        if (_lastGoodKnownRow != -1)
        {
            try
            {
                PushData();
            }
            catch (Exception exception)
            {
                OnDataError(exception);
                ListPosition = _lastGoodKnownRow;
                PushData();
            }
            _lastGoodKnownRow = ListPosition;
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
            _lastGoodKnownRow = ListPosition;
        }
        return num != ListPosition;
    }

    /// <summary>Ends the current edit operation.</summary>
    /// <filterpriority>1</filterpriority>
    public override void EndCurrentEdit()
    {
        if (Count > 0 && CurrencyManager_PullData())
        {
            var editableObject = (_list != null && (Position < 0 || Position >= _list.Count) ? null : _list?[Position]) as IEditableObject;
            editableObject?.EndEdit();
            var cancelAddNew = _list as ICancelAddNew;
            cancelAddNew?.EndNew(Position);
        }
    }

    internal int Find(PropertyDescriptor? property, object key, bool keepIndex)
    {
        if (key == null)
        {
            throw new ArgumentNullException("key");
        }
        if (property != null && _list is IBindingList && ((IBindingList)_list).SupportsSearching)
        {
            return ((IBindingList)_list).Find(property, key);
        }
        if (property != null)
        {
            for (var i = 0; i < _list?.Count; i++)
            {
                if (key.Equals(property.GetValue(_list[i])))
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private void FindGoodRow()
    {
        var count = _list?.Count ?? 0;
        for (var i = 0; i < count; i++)
        {
            ListPosition = i;
            try
            {
                PushData();
                ListPosition = i;
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
        return ListBindingHelper.GetListItemProperties(_list, listAccessors);
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
        if (!(_list is ITypedList))
        {
            return FinalType?.Name;
        }
        return ((ITypedList)_list).GetListName(null);
    }

    /// <summary>Gets the name of the list supplying the data for the binding using the specified set of bound properties.</summary>
    /// <returns>If successful, a <see cref="T:System.String" /> containing name of the list supplying the data for the binding; otherwise, an <see cref="F:System.String.Empty" /> string.</returns>
    /// <param name="listAccessors">An <see cref="T:System.Collections.ArrayList" /> of properties to be found in the data source.</param>
    protected internal override string? GetListName(ArrayList? listAccessors)
    {
        if (!(_list is ITypedList))
        {
            return "";
        }
        var propertyDescriptorArray = new PropertyDescriptor[listAccessors?.Count ?? 0];
        if (listAccessors != null)
        {
            listAccessors.CopyTo(propertyDescriptorArray, 0);
        }

        return ((ITypedList)_list).GetListName(propertyDescriptorArray);
    }

    internal ListSortDirection GetSortDirection()
    {
        if (!(_list is IBindingList) || !((IBindingList)_list).SupportsSorting)
        {
            return ListSortDirection.Ascending;
        }
        return ((IBindingList)_list).SortDirection;
    }

    internal PropertyDescriptor? GetSortProperty()
    {
        if (!(_list is IBindingList) || !((IBindingList)_list).SupportsSorting)
        {
            return null;
        }
        return ((IBindingList)_list).SortProperty;
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
        var num = ListPosition;
        UpdateLastGoodKnownRow(listChangedEventArg);
        UpdateIsBinding();
        if ((_list?.Count ?? 0) == 0)
        {
            ListPosition = -1;
            if (num != -1)
            {
                OnPositionChanged(EventArgs.Empty);
                OnCurrentChanged(EventArgs.Empty);
            }
            if (listChangedEventArg.ListChangedType == ListChangedType.Reset && e.NewIndex == -1)
            {
                OnItemChanged(_resetEvent);
            }
            if (listChangedEventArg.ListChangedType == ListChangedType.ItemDeleted)
            {
                OnItemChanged(_resetEvent);
            }
            if (e.ListChangedType == ListChangedType.PropertyDescriptorAdded || e.ListChangedType == ListChangedType.PropertyDescriptorDeleted || e.ListChangedType == ListChangedType.PropertyDescriptorChanged)
            {
                OnMetaDataChanged(EventArgs.Empty);
            }
            OnListChanged(listChangedEventArg);
            return;
        }
        _suspendPushDataInCurrentChanged = true;
        try
        {
            switch (listChangedEventArg.ListChangedType)
            {
                case ListChangedType.Reset:
                    {
                        if (ListPosition != -1 || (_list?.Count ?? 0) <= 0)
                        {
                            ChangeRecordState(Math.Min(ListPosition, _list?.Count ?? 0 - 1), true, false, true, false);
                        }
                        else
                        {
                            ChangeRecordState(0, true, false, true, false);
                        }
                        UpdateIsBinding(false);
                        OnItemChanged(_resetEvent);
                        break;
                    }
                case ListChangedType.ItemAdded:
                    {
                        if (listChangedEventArg.NewIndex > ListPosition || ListPosition >= (_list?.Count ?? 0) - 1)
                        {
                            if (listChangedEventArg.NewIndex == ListPosition && ListPosition == _list!.Count - 1 && ListPosition != -1)
                            {
                                OnCurrentItemChanged(EventArgs.Empty);
                            }
                            if (ListPosition == -1)
                            {
                                ChangeRecordState(0, false, false, true, false);
                            }
                            UpdateIsBinding();
                            OnItemChanged(_resetEvent);
                            break;
                        }

                        ChangeRecordState(ListPosition + 1, true, true, ListPosition != (_list?.Count ?? 0) - 2, false);
                        UpdateIsBinding();
                        OnItemChanged(_resetEvent);
                        if (ListPosition != (_list?.Count ?? 0) - 1)
                        {
                            break;
                        }
                        OnPositionChanged(EventArgs.Empty);
                        break;
                    }
                case ListChangedType.ItemDeleted:
                    {
                        if (listChangedEventArg.NewIndex == ListPosition)
                        {
                            ChangeRecordState(Math.Min(ListPosition, Count - 1), true, false, true, false);
                            OnItemChanged(_resetEvent);
                            break;
                        }

                        if (listChangedEventArg.NewIndex >= ListPosition)
                        {
                            OnItemChanged(_resetEvent);
                            break;
                        }
                        ChangeRecordState(ListPosition - 1, true, false, true, false);
                        OnItemChanged(_resetEvent);
                        break;
                    }
                case ListChangedType.ItemMoved:
                    {
                        if (listChangedEventArg.OldIndex == ListPosition)
                        {
                            ChangeRecordState(listChangedEventArg.NewIndex, true, Position > -1 && Position < (_list?.Count ?? 0), true, false);
                        }
                        else if (listChangedEventArg.NewIndex == ListPosition)
                        {
                            ChangeRecordState(listChangedEventArg.OldIndex, true, Position > -1 && Position < (_list?.Count ?? 0), true, false);
                        }
                        OnItemChanged(_resetEvent);
                        break;
                    }
                case ListChangedType.ItemChanged:
                    {
                        if (listChangedEventArg.NewIndex == ListPosition)
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
                        _lastGoodKnownRow = -1;
                        if (ListPosition == -1 && (_list?.Count ?? 0) > 0)
                        {
                            ChangeRecordState(0, true, false, true, false);
                        }
                        else if (ListPosition > (_list?.Count ?? 0) - 1)
                        {
                            ChangeRecordState((_list?.Count ?? 0) - 1, true, false, true, false);
                        }
                        OnMetaDataChanged(EventArgs.Empty);
                        break;
                    }
            }
            OnListChanged(listChangedEventArg);
        }
        finally
        {
            _suspendPushDataInCurrentChanged = false;
        }
    }

    /// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected internal override void OnCurrentChanged(EventArgs e)
    {
        if (!_inChangeRecordState)
        {
            var num = _lastGoodKnownRow;
            var flag = false;
            if (!_suspendPushDataInCurrentChanged)
            {
                flag = CurrencyManager_PushData();
            }
            if (Count > 0)
            {
                var item = _list?[Position];
                (item as IEditableObject)?.BeginEdit();
            }
            try
            {
                if (!flag || flag && num != -1)
                {
                    OnCurrencyChanged(e);
                    OnCurrentItemChanged(e);
                }
            }
            catch (Exception exception)
            {
                OnDataError(exception);
            }
        }
    }

    protected virtual void OnCurrencyChanged(EventArgs e)
    {
        OnCurrentChangedHandler?.Invoke(this, e);
    }

    /// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected internal override void OnCurrentItemChanged(EventArgs e)
    {
        OnCurrentItemChangedHandler?.Invoke(this, e);
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.CurrencyManager.ItemChanged" /> event.</summary>
    /// <param name="e">An <see cref="T:System.Windows.Forms.ItemChangedEventArgs" /> that contains the event data. </param>
    protected virtual void OnItemChanged(ItemChangedEventArgs e)
    {
        var flag = false;
        if ((e.Index == ListPosition || e.Index == -1 && Position < Count) && !_inChangeRecordState)
        {
            flag = CurrencyManager_PushData();
        }
        try
        {
            _itemChanged?.Invoke(this, e);
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
        _listChanged?.Invoke(this, e);
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.CurrencyManager.MetaDataChanged" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected internal void OnMetaDataChanged(EventArgs e)
    {
        _metaDataChanged?.Invoke(this, e);
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.PositionChanged" /> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected virtual void OnPositionChanged(EventArgs e)
    {
        try
        {
            OnPositionChangedHandler?.Invoke(this, e);
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
        if ((_list?.Count ?? 0) <= 0)
        {
            ListPosition = -1;
        }
        else if (ListPosition >= (_list?.Count ?? 0))
        {
            _lastGoodKnownRow = -1;
            ListPosition = 0;
        }
        List_ListChanged(_list, new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    internal void Release()
    {
        UnwireEvents(_list);
    }

    /// <summary>Removes the item at the specified index.</summary>
    /// <param name="index">The index of the item to remove from the list. </param>
    /// <exception cref="T:System.IndexOutOfRangeException">There is no row at the specified <paramref name="index" />. </exception>
    /// <filterpriority>1</filterpriority>
    public override void RemoveAt(int index)
    {
        _list?.RemoveAt(index);
    }

    /// <summary>Resumes data binding.</summary>
    /// <filterpriority>1</filterpriority>
    public override void ResumeBinding()
    {
        _lastGoodKnownRow = -1;
        try
        {
            if (!_shouldBind)
            {
                _shouldBind = true;
                ListPosition = _list == null || _list.Count == 0 ? -1 : 0;
                UpdateIsBinding();
            }
        }
        catch
        {
            _shouldBind = false;
            UpdateIsBinding();
            throw;
        }
    }

    private protected override void SetDataSource(object? source)
    {
        if (_dataSource == source)
        {
            return;
        }
        Release();
        _dataSource = source;
        _list = null;
        FinalType = null;
        var listValue = source;
        if (listValue is Array)
        {
            FinalType = listValue.GetType();
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
        if (FinalType == null)
        {
            FinalType = listValue.GetType();
        }
        _list = (IList)listValue;
        WireEvents(_list);
        if (_list.Count <= 0)
        {
            ListPosition = -1;
        }
        else
        {
            ListPosition = 0;
        }
        OnItemChanged(_resetEvent);
        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1, -1));
        UpdateIsBinding();
    }

    internal void SetSort(PropertyDescriptor property, ListSortDirection sortDirection)
    {
        if (_list is IBindingList && ((IBindingList)_list).SupportsSorting)
        {
            ((IBindingList)_list).ApplySort(property, sortDirection);
        }
    }

    /// <summary>Suspends data binding to prevents changes from updating the bound data source.</summary>
    /// <filterpriority>1</filterpriority>
    public override void SuspendBinding()
    {
        _lastGoodKnownRow = -1;
        if (_shouldBind)
        {
            _shouldBind = false;
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
        var flag = _list == null || _list.Count <= 0 || !_shouldBind ? false : ListPosition != -1;
        if (_list != null && _bound != flag)
        {
            _bound = flag;
            var num = flag ? 0 : -1;
            ChangeRecordState(num, _bound, Position != num, true, false);
            var count = Bindings.Count;
            for (var i = 0; i < count; i++)
            {
                Bindings[i].UpdateIsBinding();
            }
            if (raiseItemChangedEvent)
            {
                OnItemChanged(_resetEvent);
            }
        }
    }

    private void UpdateLastGoodKnownRow(ListChangedEventArgs e)
    {
        switch (e.ListChangedType)
        {
            case ListChangedType.Reset:
                {
                    _lastGoodKnownRow = -1;
                    return;
                }
            case ListChangedType.ItemAdded:
                {
                    if (e.NewIndex > _lastGoodKnownRow || _lastGoodKnownRow >= (List?.Count ?? 0) - 1)
                    {
                        break;
                    }
                    _lastGoodKnownRow++;
                    return;
                }
            case ListChangedType.ItemDeleted:
                {
                    if (e.NewIndex != _lastGoodKnownRow)
                    {
                        break;
                    }
                    _lastGoodKnownRow = -1;
                    return;
                }
            case ListChangedType.ItemMoved:
                {
                    if (e.OldIndex != _lastGoodKnownRow)
                    {
                        break;
                    }
                    _lastGoodKnownRow = e.NewIndex;
                    return;
                }
            case ListChangedType.ItemChanged:
                {
                    if (e.NewIndex != _lastGoodKnownRow)
                    {
                        break;
                    }
                    _lastGoodKnownRow = -1;
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
        add => _itemChanged += value;
        remove => _itemChanged -= value;
    }

    /// <summary>Occurs when the list changes or an item in the list changes.</summary>
    /// <filterpriority>1</filterpriority>
    public event ListChangedEventHandler? ListChanged
    {
        add => _listChanged += value;
        remove => _listChanged -= value;
    }

    /// <summary>Occurs when the metadata of the <see cref="P:System.Windows.Forms.CurrencyManager.List" /> has changed.</summary>
    /// <filterpriority>1</filterpriority>
    public event EventHandler? MetaDataChanged
    {
        add => _metaDataChanged += value;
        remove => _metaDataChanged -= value;
    }
}