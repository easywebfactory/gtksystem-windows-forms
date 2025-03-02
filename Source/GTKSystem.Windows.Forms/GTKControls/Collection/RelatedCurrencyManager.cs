using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

internal class RelatedCurrencyManager : CurrencyManager
{
    private BindingManagerBase? parentManager;

    private string? dataField;

    private PropertyDescriptor? fieldInfo;

    private static readonly List<BindingManagerBase?> ignoreItemChangedTable;

    static RelatedCurrencyManager()
    {
        ignoreItemChangedTable = [];
    }

    internal RelatedCurrencyManager(BindingManagerBase? parentManager, string? dataField) : base(null)
    {
        Bind(parentManager, dataField);
    }

    internal void Bind(BindingManagerBase? manager, string? dataFieldName)
    {
        UnwireParentManager(parentManager);
        parentManager = manager;
        dataField = dataFieldName;
        fieldInfo = manager?.GetItemProperties()?.Find(dataFieldName??string.Empty, true);
        if (fieldInfo == null || !typeof(IList).IsAssignableFrom(fieldInfo.PropertyType))
        {
            throw new ArgumentException("RelatedListManagerChild");
        }
        finalType = fieldInfo.PropertyType;
        WireParentManager(parentManager);
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
        propertyDescriptorArray[0] = fieldInfo;
        return parentManager?.GetItemProperties(propertyDescriptorArray);
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
        listAccessors?.Insert(0, fieldInfo);
        return parentManager?.GetListName(listAccessors);
    }

    private void ParentManager_CurrentItemChanged(object? sender, EventArgs e)
    {
        if (ignoreItemChangedTable.Contains(parentManager))
        {
            return;
        }
        var num = listposition;
        try
        {
            PullData();
        }
        catch (Exception exception)
        {
            OnDataError(exception);
        }
        if (!(parentManager is CurrencyManager))
        {
            if (parentManager is { Current: not null })
            {
                SetDataSource(fieldInfo?.GetValue(parentManager.Current));
            }

            listposition = Count > 0 ? 0 : -1;
        }
        else
        {
            var currencyManager = (CurrencyManager)parentManager;
            if (currencyManager.Count <= 0)
            {
                currencyManager.AddNew();
                try
                {
                    ignoreItemChangedTable.Add(currencyManager);
                    currencyManager.CancelCurrentEdit();
                }
                finally
                {
                    if (ignoreItemChangedTable.Contains(currencyManager))
                    {
                        ignoreItemChangedTable.Remove(currencyManager);
                    }
                }
            }
            else
            {
                if (currencyManager.Current != null)
                {
                    SetDataSource(fieldInfo?.GetValue(currencyManager.Current));
                }

                listposition = Count > 0 ? 0 : -1;
            }
        }
        if (num != listposition)
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