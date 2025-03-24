using System.ComponentModel;

namespace System.Windows.Forms;

internal class BindToObject
{
    private PropertyDescriptor? _fieldInfo;

    private readonly BindingMemberInfo _dataMember;

    private readonly object _dataSource;

    private BindingManagerBase? _bindingManager;

    private readonly Binding? _owner;

    private string _errorText = string.Empty;

    private bool _dataSourceInitialized;

    private bool _waitingOnDataSource;

    internal BindingManagerBase? BindingManagerBase => _bindingManager;

    internal BindingMemberInfo BindingMemberInfo => _dataMember;

    internal Type? BindToType
    {
        get
        {
            if (_dataMember.BindingField.Length != 0)
            {
                return _fieldInfo?.PropertyType;
            }
            var bindType = _bindingManager?.BindType;
            if (typeof(Array).IsAssignableFrom(bindType))
            {
                bindType = bindType?.GetElementType();
            }
            return bindType;
        }
    }

    internal string DataErrorText => _errorText;

    internal object DataSource => _dataSource;

    internal PropertyDescriptor? FieldInfo => _fieldInfo;

    private bool IsDataSourceInitialized
    {
        get
        {
            if (_dataSourceInitialized)
            {
                return true;
            }

            if (_dataSource is not ISupportInitializeNotification supportInitializeNotification || supportInitializeNotification.IsInitialized)
            {
                _dataSourceInitialized = true;
                return true;
            }
            if (_waitingOnDataSource)
            {
                return false;
            }
            supportInitializeNotification.Initialized += DataSource_Initialized;
            _waitingOnDataSource = true;
            return false;
        }
    }

    internal BindToObject(Binding owner, object dataSource, string? dataMember)
    {
        _owner = owner;
        _dataSource = dataSource;
        _dataMember = new BindingMemberInfo(dataMember);
        CheckBinding();
    }

    internal void CheckBinding()
    {
        if (_owner is { BindableComponent: not null } && _owner.ControlAtDesignTime())
        {
            return;
        }

        var component = _owner?.BindingManagerBase?.Current;
        if (_owner?.BindingManagerBase != null && _fieldInfo != null && _owner.BindingManagerBase.IsBinding && !(_owner.BindingManagerBase is CurrencyManager))
        {
            if (component != null)
            {
                _fieldInfo?.RemoveValueChanged(component, PropValueChanged);
            }
        }
        if (_owner == null || _owner.BindingManagerBase == null || _owner.BindableComponent == null || !_owner.ComponentCreated || !IsDataSourceInitialized)
        {
            _fieldInfo = null;
        }
        else
        {
            var bindingField = _dataMember.BindingField;
            _fieldInfo = _owner.BindingManagerBase.GetItemProperties()?.Find(bindingField, true);
            if (_owner.BindingManagerBase.DataSource != null && _fieldInfo == null && bindingField.Length > 0)
            {
                throw new ArgumentException(@"ListBindingBindField", nameof(_dataMember));
            }
            if (_fieldInfo != null && _owner.BindingManagerBase.IsBinding && !(_owner.BindingManagerBase is CurrencyManager))
            {
                if (component != null)
                {
                    _fieldInfo.AddValueChanged(component, PropValueChanged);
                }
            }
        }
    }

    private void DataSource_Initialized(object? sender, EventArgs e)
    {
        if (_dataSource is ISupportInitializeNotification supportInitializeNotification)
        {
            supportInitializeNotification.Initialized -= DataSource_Initialized;
        }
        _waitingOnDataSource = false;
        _dataSourceInitialized = true;
        CheckBinding();
    }

    private string GetErrorText(object? value)
    {
        var dataErrorInfo = value as IDataErrorInfo;
        var empty = string.Empty;
        if (dataErrorInfo != null)
        {
            empty = _fieldInfo != null ? dataErrorInfo[_fieldInfo.Name] : dataErrorInfo.Error;
        }
        return empty ?? string.Empty;
    }

    internal object? GetValue()
    {
        var current = _bindingManager?.Current;
        _errorText = GetErrorText(current);
        if (_fieldInfo != null)
        {
            if (current != null)
            {
                current = _fieldInfo.GetValue(current);
            }
        }
        return current;
    }

    private void PropValueChanged(object? sender, EventArgs e)
    {
        _bindingManager?.OnCurrentChanged(EventArgs.Empty);
    }

    internal void SetBindingManagerBase(BindingManagerBase? lManager)
    {
        if (_bindingManager == lManager)
        {
            return;
        }
        if (_bindingManager != null && _fieldInfo != null && _bindingManager.IsBinding && !(_bindingManager is CurrencyManager))
        {
            if (_bindingManager.Current != null)
            {
                _fieldInfo.RemoveValueChanged(_bindingManager.Current, PropValueChanged);
            }

            _fieldInfo = null;
        }
        _bindingManager = lManager;
        CheckBinding();
    }

    internal void SetValue(object? value)
    {
        object? current = null;
        if (_fieldInfo == null)
        {
            if (_bindingManager is CurrencyManager currencyManager)
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
            current = _bindingManager?.Current;
            (current as IEditableObject)?.BeginEdit();
            if (!_fieldInfo.IsReadOnly)
            {
                _fieldInfo.SetValue(current, value);
            }
        }
        _errorText = GetErrorText(current);
    }
}