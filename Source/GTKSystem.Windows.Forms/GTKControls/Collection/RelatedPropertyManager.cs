using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

internal class RelatedPropertyManager : PropertyManager
{
    private BindingManagerBase? parentManager;

    private string? dataField;

    private PropertyDescriptor? fieldInfo;

    internal override Type? BindType => fieldInfo?.PropertyType;

    public override object? Current
    {
        get
        {
            if (DataSource == null)
            {
                return null;
            }
            return fieldInfo?.GetValue(DataSource);
        }
    }

    internal RelatedPropertyManager(BindingManagerBase? parentManager, string? dataField) : base(GetCurrentOrNull(parentManager), dataField)
    {
        Bind(parentManager, dataField);
    }

    private void Bind(BindingManagerBase? manager, string? field)
    {
        parentManager = manager;
        dataField = field;
        if (field != null)
        {
            fieldInfo = manager?.GetItemProperties()?.Find(field, true);
        }

        if (fieldInfo == null)
        {
            throw new ArgumentException("RelatedListManagerChild");
        }

        if (manager != null)
        {
            manager.CurrentItemChanged += ParentManager_CurrentItemChanged;
        }

        Refresh();
    }

    private static object? GetCurrentOrNull(BindingManagerBase? parentManager)
    {
        if ((parentManager?.Position??0) < 0 || parentManager?.Position >= parentManager?.Count)
        {
            return null;
        }
        return parentManager?.Current;
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
        Refresh();
    }

    private void Refresh()
    {
        EndCurrentEdit();
        SetDataSource(GetCurrentOrNull(parentManager));
        OnCurrentChanged(EventArgs.Empty);
    }
}