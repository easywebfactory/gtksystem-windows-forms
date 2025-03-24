using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

internal class RelatedCurrencyManager : CurrencyManager
{
    private BindingManagerBase? _parentManager;

    private string? _dataField;

    private PropertyDescriptor? _fieldInfo;

    private static readonly List<BindingManagerBase?> s_ignoreItemChangedTable;

    static RelatedCurrencyManager()
    {
        s_ignoreItemChangedTable = [];
    }

    internal RelatedCurrencyManager(BindingManagerBase? parentManager, string? dataField) : base(null)
    {
        Bind(parentManager, dataField);
    }

    internal void Bind(BindingManagerBase? manager, string? dataFieldName)
    {
        UnwireParentManager(_parentManager);
        _parentManager = manager;
        _dataField = dataFieldName;
        _fieldInfo = manager?.GetItemProperties()?.Find(dataFieldName??string.Empty, true);
        if (_fieldInfo == null || !typeof(IList).IsAssignableFrom(_fieldInfo.PropertyType))
        {
            throw new ArgumentException("RelatedListManagerChild");
        }
        FinalType = _fieldInfo.PropertyType;
        WireParentManager(_parentManager);
        ParentManager_CurrentItemChanged(manager, EventArgs.Empty);
    }

    internal override PropertyDescriptorCollection? GetItemProperties(PropertyDescriptor?[]? listAccessors)
    {
        PropertyDescriptor?[]? propertyDescriptorArray;
        if (listAccessors == null || listAccessors.Length == 0)
        {
            propertyDescriptorArray = new PropertyDescriptor[1];
        }
        else
        {
            propertyDescriptorArray = new PropertyDescriptor[listAccessors.Length + 1];
            listAccessors.CopyTo(propertyDescriptorArray, 1);
        }
        propertyDescriptorArray[0] = _fieldInfo;
        return _parentManager?.GetItemProperties(propertyDescriptorArray);
    }

    public override PropertyDescriptorCollection? GetItemProperties()
    {
        return GetItemProperties(null);
    }

    internal override string? GetListName()
    {
        var listName = GetListName(new ArrayList());
        if (listName?.Length > 0)
        {
            return listName;
        }
        return base.GetListName();
    }

    protected internal override string? GetListName(ArrayList? listAccessors)
    {
        listAccessors?.Insert(0, _fieldInfo);
        return _parentManager?.GetListName(listAccessors);
    }

    private void ParentManager_CurrentItemChanged(object? sender, EventArgs e)
    {
        if (s_ignoreItemChangedTable.Contains(_parentManager))
        {
            return;
        }
        var num = ListPosition;
        try
        {
            PullData();
        }
        catch (Exception exception)
        {
            OnDataError(exception);
        }
        if (!(_parentManager is CurrencyManager))
        {
            if (_parentManager is { Current: not null })
            {
                SetDataSource(_fieldInfo?.GetValue(_parentManager.Current));
            }

            ListPosition = Count > 0 ? 0 : -1;
        }
        else
        {
            var currencyManager = (CurrencyManager)_parentManager;
            if (currencyManager.Count <= 0)
            {
                currencyManager.AddNew();
                try
                {
                    s_ignoreItemChangedTable.Add(currencyManager);
                    currencyManager.CancelCurrentEdit();
                }
                finally
                {
                    if (s_ignoreItemChangedTable.Contains(currencyManager))
                    {
                        s_ignoreItemChangedTable.Remove(currencyManager);
                    }
                }
            }
            else
            {
                if (currencyManager.Current != null)
                {
                    SetDataSource(_fieldInfo?.GetValue(currencyManager.Current));
                }

                ListPosition = Count > 0 ? 0 : -1;
            }
        }
        if (num != ListPosition)
        {
            OnPositionChanged(EventArgs.Empty);
        }
        OnCurrentChanged(EventArgs.Empty);
        OnCurrentItemChanged(EventArgs.Empty);
    }

    private void ParentManager_MetaDataChanged(object? sender, EventArgs e)
    {
        OnMetaDataChanged(e);
    }

    private void UnwireParentManager(BindingManagerBase? bmb)
    {
        if (bmb != null)
        {
            bmb.CurrentItemChanged -= ParentManager_CurrentItemChanged;
            if (bmb is CurrencyManager manager)
            {
                manager.MetaDataChanged -= ParentManager_MetaDataChanged;
            }
        }
    }

    private void WireParentManager(BindingManagerBase? bmb)
    {
        if (bmb != null)
        {
            bmb.CurrentItemChanged += ParentManager_CurrentItemChanged;
            if (bmb is CurrencyManager manager)
            {
                manager.MetaDataChanged += ParentManager_MetaDataChanged;
            }
        }
    }
}