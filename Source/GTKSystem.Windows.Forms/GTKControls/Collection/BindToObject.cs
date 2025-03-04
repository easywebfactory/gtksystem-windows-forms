using System.ComponentModel;

namespace System.Windows.Forms;

internal class BindToObject
{
    private PropertyDescriptor? fieldInfo;

    private readonly BindingMemberInfo dataMember;

    private readonly object dataSource;

    private BindingManagerBase? bindingManager;

    private readonly Binding owner;

    private string errorText = string.Empty;

    private bool dataSourceInitialized;

    private bool waitingOnDataSource;

    internal BindingManagerBase? BindingManagerBase => bindingManager;

    internal BindingMemberInfo BindingMemberInfo => dataMember;

    internal Type? BindToType
    {
        get
        {
            if (dataMember.BindingField.Length != 0)
            {
                return fieldInfo?.PropertyType;
            }
            var bindType = bindingManager?.BindType;
            if (typeof(Array).IsAssignableFrom(bindType))
            {
                bindType = bindType?.GetElementType();
            }
            return bindType;
        }
    }

    internal string DataErrorText => errorText;

    internal object DataSource => dataSource;

    internal PropertyDescriptor? FieldInfo => fieldInfo;

    private bool IsDataSourceInitialized
    {
        get
        {
            if (dataSourceInitialized)
            {
                return true;
            }
            var supportInitializeNotification = dataSource as ISupportInitializeNotification;
            if (supportInitializeNotification == null || supportInitializeNotification.IsInitialized)
            {
                dataSourceInitialized = true;
                return true;
            }
            if (waitingOnDataSource)
            {
                return false;
            }
            supportInitializeNotification.Initialized += DataSource_Initialized;
            waitingOnDataSource = true;
            return false;
        }
    }

    internal BindToObject(Binding owner, object dataSource, string? dataMember)
    {
        this.owner = owner;
        this.dataSource = dataSource;
        this.dataMember = new BindingMemberInfo(dataMember);
        CheckBinding();
    }

    internal void CheckBinding()
    {
        if (owner is { BindableComponent: not null } && owner.ControlAtDesignTime())
        {
            return;
        }

        var component = owner.BindingManagerBase?.Current;
        if (owner?.BindingManagerBase != null && fieldInfo != null && owner.BindingManagerBase.IsBinding && !(owner.BindingManagerBase is CurrencyManager))
        {
            if (component != null)
            {
                fieldInfo?.RemoveValueChanged(component, PropValueChanged);
            }
        }
        if (owner == null || owner.BindingManagerBase == null || owner.BindableComponent == null || !owner.ComponentCreated || !IsDataSourceInitialized)
        {
            fieldInfo = null;
        }
        else
        {
            var bindingField = dataMember.BindingField;
            fieldInfo = owner.BindingManagerBase.GetItemProperties()?.Find(bindingField, true);
            if (owner.BindingManagerBase.DataSource != null && fieldInfo == null && bindingField.Length > 0)
            {
                throw new ArgumentException(@"ListBindingBindField", nameof(dataMember));
            }
            if (fieldInfo != null && owner.BindingManagerBase.IsBinding && !(owner.BindingManagerBase is CurrencyManager))
            {
                if (component != null)
                {
                    fieldInfo.AddValueChanged(component, PropValueChanged);
                }
            }
        }
    }

    private void DataSource_Initialized(object? sender, EventArgs e)
    {
        var supportInitializeNotification = dataSource as ISupportInitializeNotification;
        if (supportInitializeNotification != null)
        {
            supportInitializeNotification.Initialized -= DataSource_Initialized;
        }
        waitingOnDataSource = false;
        dataSourceInitialized = true;
        CheckBinding();
    }

    private string GetErrorText(object? value)
    {
        var dataErrorInfo = value as IDataErrorInfo;
        var empty = string.Empty;
        if (dataErrorInfo != null)
        {
            empty = fieldInfo != null ? dataErrorInfo[fieldInfo.Name] : dataErrorInfo.Error;
        }
        return empty ?? string.Empty;
    }

    internal object? GetValue()
    {
        var current = bindingManager?.Current;
        errorText = GetErrorText(current);
        if (fieldInfo != null)
        {
            if (current != null)
            {
                current = fieldInfo.GetValue(current);
            }
        }
        return current;
    }

    private void PropValueChanged(object? sender, EventArgs e)
    {
        bindingManager?.OnCurrentChanged(EventArgs.Empty);
    }

    internal void SetBindingManagerBase(BindingManagerBase? lManager)
    {
        if (bindingManager == lManager)
        {
            return;
        }
        if (bindingManager != null && fieldInfo != null && bindingManager.IsBinding && !(bindingManager is CurrencyManager))
        {
            if (bindingManager.Current != null)
            {
                fieldInfo.RemoveValueChanged(bindingManager.Current, PropValueChanged);
            }

            fieldInfo = null;
        }
        bindingManager = lManager;
        CheckBinding();
    }

    internal void SetValue(object? value)
    {
        object? current = null;
        if (fieldInfo == null)
        {
            var currencyManager = bindingManager as CurrencyManager;
            if (currencyManager != null)
            {
                if (value != null)
                {
                    currencyManager[currencyManager.Position] = value;
                    current = value;
                }
            }
        }
        else
        {
            current = bindingManager?.Current;
            (current as IEditableObject)?.BeginEdit();
            if (!fieldInfo.IsReadOnly)
            {
                fieldInfo.SetValue(current, value);
            }
        }
        errorText = GetErrorText(current);
    }
}