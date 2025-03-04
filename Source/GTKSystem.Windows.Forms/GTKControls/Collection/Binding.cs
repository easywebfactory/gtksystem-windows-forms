using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Windows.Forms;

/// <summary>Represents the simple binding between the property value of an object and the property value of a control.</summary>
/// <filterpriority>1</filterpriority>
public class Binding
{
    private IBindableComponent? control;

    private BindingManagerBase? bindingManagerBase;

    private readonly BindToObject? bindToObject;

    private readonly string propertyName = "";

    private PropertyDescriptor? propInfo;

    private PropertyDescriptor? propIsNullInfo;

    private EventDescriptor? validateInfo;

    private TypeConverter? propInfoConverter;

    private bool formattingEnabled = true;

    private bool bound;

    private bool modified;

    private bool inSetPropValue;

    private bool inPushOrPull;

    private bool inOnBindingComplete;

    private string formatString = string.Empty;

    private IFormatProvider? formatInfo;

    private object? nullValue;

    private object? dsNullValue = Formatter.GetDefaultDataSourceNullValue(null);

    private bool dsNullValueSet;

    private ConvertEventHandler? onParse;

    private ConvertEventHandler? onFormat;

    private ControlUpdateMode controlUpdateMode;

    private DataSourceUpdateMode dataSourceUpdateMode;

    private BindingCompleteEventHandler? onComplete;

    /// <summary>Gets the control the <see cref="T:System.Windows.Forms.Binding" /> is associated with.</summary>
    /// <returns>The <see cref="T:System.Windows.Forms.IBindableComponent" /> the <see cref="T:System.Windows.Forms.Binding" /> is associated with.</returns>
    /// <filterpriority>1</filterpriority>
    /// <PermissionSet>
    ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
    /// </PermissionSet>
    [DefaultValue(null)]
    public IBindableComponent? BindableComponent
    {
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        get => control;
        internal set => control = value;
    }

    /// <summary>Gets the <see cref="T:System.Windows.Forms.BindingManagerBase" /> for this <see cref="T:System.Windows.Forms.Binding" />.</summary>
    /// <returns>The <see cref="T:System.Windows.Forms.BindingManagerBase" /> that manages this <see cref="T:System.Windows.Forms.Binding" />.</returns>
    /// <filterpriority>1</filterpriority>
    public BindingManagerBase? BindingManagerBase => bindingManagerBase;

    /// <summary>Gets an object that contains information about this binding based on the <paramref name="dataMember" /> parameter in the <see cref="Overload:System.Windows.Forms.Binding.#ctor" /> constructor.</summary>
    /// <returns>A <see cref="T:System.Windows.Forms.BindingMemberInfo" /> that contains information about this <see cref="T:System.Windows.Forms.Binding" />.</returns>
    /// <filterpriority>1</filterpriority>
    public BindingMemberInfo? BindingMemberInfo => bindToObject?.BindingMemberInfo;

    internal BindToObject? BindToObject => bindToObject;

    internal bool ComponentCreated => IsComponentCreated(control);

    /// <summary>Gets the control that the binding belongs to.</summary>
    /// <returns>The <see cref="T:System.Windows.Forms.Control" /> that the binding belongs to.</returns>
    /// <filterpriority>1</filterpriority>
    /// <PermissionSet>
    ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
    /// </PermissionSet>
    [DefaultValue(null)]
    public Control? Control
    {
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        get => control as Control;
        internal set => control = value;
    }

    /// <summary>Gets or sets when changes to the data source are propagated to the bound control property.</summary>
    /// <returns>One of the <see cref="T:System.Windows.Forms.ControlUpdateMode" /> values. The default is <see cref="F:System.Windows.Forms.ControlUpdateMode.OnPropertyChanged" />.</returns>
    [DefaultValue(ControlUpdateMode.OnPropertyChanged)]
    public ControlUpdateMode ControlUpdateMode
    {
        get => controlUpdateMode;
        set
        {
            if (controlUpdateMode != value)
            {
                controlUpdateMode = value;
                if (IsBinding)
                {
                    PushData();
                }
            }
        }
    }

    /// <summary>Gets the data source for this binding.</summary>
    /// <returns>An <see cref="T:System.Object" /> that represents the data source.</returns>
    /// <filterpriority>1</filterpriority>
    public object? DataSource => bindToObject?.DataSource;

    /// <summary>Gets or sets the value to be stored in the data source if the control value is null or empty.</summary>
    /// <returns>The <see cref="T:System.Object" /> to be stored in the data source when the control property is empty or null. The default is <see cref="T:System.DBNull" /> for value types and null for non-value types.</returns>
    public object? DataSourceNullValue
    {
        get => dsNullValue;
        set
        {
            if (!Equals(dsNullValue, value))
            {
                var obj = dsNullValue;
                dsNullValue = value;
                dsNullValueSet = true;
                if (IsBinding)
                {
                    var obj1 = bindToObject?.GetValue();
                    if (Formatter.IsNullData(obj1, obj))
                    {
                        WriteValue();
                    }
                    if (Formatter.IsNullData(obj1, value))
                    {
                        ReadValue();
                    }
                }
            }
        }
    }

    /// <summary>Gets or sets a value that indicates when changes to the bound control property are propagated to the data source.</summary>
    /// <returns>A value that indicates when changes are propagated. The default is <see cref="F:System.Windows.Forms.DataSourceUpdateMode.OnValidation" />.</returns>
    [DefaultValue(DataSourceUpdateMode.OnValidation)]
    public DataSourceUpdateMode DataSourceUpdateMode
    {
        get => dataSourceUpdateMode;
        set => dataSourceUpdateMode = value;
    }

    /// <summary>Gets or sets the <see cref="T:System.IFormatProvider" /> that provides custom formatting behavior.</summary>
    /// <returns>The <see cref="T:System.IFormatProvider" /> implementation that provides custom formatting behavior.</returns>
    /// <filterpriority>1</filterpriority>
    [DefaultValue(null)]
    public IFormatProvider? FormatInfo
    {
        get => formatInfo;
        set
        {
            if (ReferenceEquals(formatInfo, value))
            {
                return;
            }

            formatInfo = value;
            if (IsBinding)
            {
                PushData();
            }
        }
    }

    /// <summary>Gets or sets the format specifier characters that indicate how a value is to be displayed.</summary>
    /// <returns>The string of format specifier characters that indicate how a value is to be displayed.</returns>
    /// <filterpriority>1</filterpriority>
    public string FormatString
    {
        get => formatString;
        set
        {
            if (value == null)
            {
                value = string.Empty;
            }
            if (!value.Equals(formatString))
            {
                formatString = value;
                if (IsBinding)
                {
                    PushData();
                }
            }
        }
    }

    /// <summary>Gets or sets a value indicating whether type conversion and formatting is applied to the control property data.</summary>
    /// <returns>true if type conversion and formatting of control property data is enabled; otherwise, false. The default is false.</returns>
    /// <filterpriority>1</filterpriority>
    public bool FormattingEnabled
    {
        get => formattingEnabled;
        set
        {
            if (formattingEnabled != value)
            {
                formattingEnabled = value;
                if (IsBinding)
                {
                    PushData();
                }
            }
        }
    }

    internal bool IsBindable
    {
        get
        {
            if (control == null || propertyName.Length <= 0 || bindToObject?.DataSource == null)
            {
                return false;
            }
            return bindingManagerBase != null;
        }
    }

    /// <summary>Gets a value indicating whether the binding is active.</summary>
    /// <returns>true if the binding is active; otherwise, false.</returns>
    /// <filterpriority>1</filterpriority>
    public bool IsBinding => bound;

    /// <summary>Gets or sets the <see cref="T:System.Object" /> to be set as the control property when the data source contains a <see cref="T:System.DBNull" /> value. </summary>
    /// <returns>The <see cref="T:System.Object" /> to be set as the control property when the data source contains a <see cref="T:System.DBNull" /> value. The default is null.</returns>
    /// <filterpriority>1</filterpriority>
    public object? NullValue
    {
        get => nullValue;
        set
        {
            if (!Equals(nullValue, value))
            {
                nullValue = value;
                if (IsBinding && Formatter.IsNullData(bindToObject?.GetValue(), dsNullValue))
                {
                    PushData();
                }
            }
        }
    }

    /// <summary>Gets or sets the name of the control's data-bound property.</summary>
    /// <returns>The name of a control property to bind to.</returns>
    /// <filterpriority>1</filterpriority>
    [DefaultValue("")]
    public string PropertyName => propertyName;

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class that simple-binds the indicated control property to the specified data member of the data source.</summary>
    /// <param name="propertyName">The name of the control property to bind. </param>
    /// <param name="dataSource">An <see cref="T:System.Object" /> that represents the data source. </param>
    /// <param name="dataMember">The property or list to bind to. </param>
    /// <exception cref="T:System.Exception">
    ///   <paramref name="propertyName" /> is neither a valid property of a control nor an empty string (""). </exception>
    /// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.</exception>
    public Binding(string propertyName, object dataSource, string? dataMember) : this(propertyName, dataSource, dataMember, false, DataSourceUpdateMode.OnValidation, null, string.Empty, null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class that binds the indicated control property to the specified data member of the data source, and optionally enables formatting to be applied.</summary>
    /// <param name="propertyName">The name of the control property to bind. </param>
    /// <param name="dataSource">An <see cref="T:System.Object" /> that represents the data source. </param>
    /// <param name="dataMember">The property or list to bind to. </param>
    /// <param name="formattingEnabled">true to format the displayed data; otherwise, false. </param>
    /// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.-or-The property given is a read-only property.</exception>
    /// <exception cref="T:System.Exception">Formatting is disabled and <paramref name="propertyName" /> is neither a valid property of a control nor an empty string (""). </exception>
    public Binding(string propertyName, object dataSource, string? dataMember, bool formattingEnabled) : this(propertyName, dataSource, dataMember, formattingEnabled, DataSourceUpdateMode.OnValidation, null, string.Empty, null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class that binds the specified control property to the specified data member of the specified data source. Optionally enables formatting and propagates values to the data source based on the specified update setting.</summary>
    /// <param name="propertyName">The name of the control property to bind. </param>
    /// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
    /// <param name="dataMember">The property or list to bind to.</param>
    /// <param name="formattingEnabled">true to format the displayed data; otherwise, false.</param>
    /// <param name="dataSourceUpdateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
    /// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.-or-The data source or data member or control property specified are associated with another binding in the collection.</exception>
    public Binding(string propertyName, object dataSource, string? dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode) : this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, null, string.Empty, null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class that binds the indicated control property to the specified data member of the specified data source. Optionally enables formatting, propagates values to the data source based on the specified update setting, and sets the property to the specified value when a <see cref="T:System.DBNull" /> is returned from the data source.</summary>
    /// <param name="propertyName">The name of the control property to bind. </param>
    /// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
    /// <param name="dataMember">The property or list to bind to.</param>
    /// <param name="formattingEnabled">true to format the displayed data; otherwise, false.</param>
    /// <param name="dataSourceUpdateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
    /// <param name="nullValue">The <see cref="T:System.Object" /> to be applied to the bound control property if the data source value is <see cref="T:System.DBNull" />.</param>
    /// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.-or-The data source or data member or control property specified are associated with another binding in the collection.</exception>
    public Binding(string propertyName, object dataSource, string? dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object? nullValue) : this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, string.Empty, null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class that binds the specified control property to the specified data member of the specified data source. Optionally enables formatting with the specified format string; propagates values to the data source based on the specified update setting; and sets the property to the specified value when a <see cref="T:System.DBNull" /> is returned from the data source.</summary>
    /// <param name="propertyName">The name of the control property to bind. </param>
    /// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
    /// <param name="dataMember">The property or list to bind to.</param>
    /// <param name="formattingEnabled">true to format the displayed data; otherwise, false.</param>
    /// <param name="dataSourceUpdateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
    /// <param name="nullValue">The <see cref="T:System.Object" /> to be applied to the bound control property if the data source value is <see cref="T:System.DBNull" />.</param>
    /// <param name="formatString">One or more format specifier characters that indicate how a value is to be displayed.</param>
    /// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.-or-The data source or data member or control property specified are associated with another binding in the collection.</exception>
    public Binding(string propertyName, object dataSource, string? dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object? nullValue, string formatString) : this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, formatString, null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Binding" /> class with the specified control property to the specified data member of the specified data source. Optionally enables formatting with the specified format string; propagates values to the data source based on the specified update setting; enables formatting with the specified format string; sets the property to the specified value when a <see cref="T:System.DBNull" /> is returned from the data source; and sets the specified format provider.</summary>
    /// <param name="propertyName">The name of the control property to bind. </param>
    /// <param name="dataSource">An <see cref="T:System.Object" /> representing the data source. </param>
    /// <param name="dataMember">The property or list to bind to.</param>
    /// <param name="formattingEnabled">true to format the displayed data; otherwise, false.</param>
    /// <param name="dataSourceUpdateMode">One of the <see cref="T:System.Windows.Forms.DataSourceUpdateMode" /> values.</param>
    /// <param name="nullValue">The <see cref="T:System.Object" /> to be applied to the bound control property if the data source value is <see cref="T:System.DBNull" />.</param>
    /// <param name="formatString">One or more format specifier characters that indicate how a value is to be displayed.</param>
    /// <param name="formatInfo">An implementation of <see cref="T:System.IFormatProvider" /> to override default formatting behavior.</param>
    /// <exception cref="T:System.ArgumentException">The property given by <paramref name="propertyName" /> does not exist on the control.-or-The data source or data member or control property specified are associated with another binding in the collection.</exception>
    public Binding(string propertyName, object dataSource, string? dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object? nullValue, string formatString, IFormatProvider? formatInfo)
    {
        bindToObject = new BindToObject(this, dataSource, dataMember);
        this.propertyName = propertyName;
        this.formattingEnabled = formattingEnabled;
        this.formatString = formatString;
        this.nullValue = nullValue;
        this.formatInfo = formatInfo;
        this.formattingEnabled = formattingEnabled;
        this.dataSourceUpdateMode = dataSourceUpdateMode;
        CheckBinding();
    }

    private Binding()
    {
    }

    private void binding_MetaDataChanged(object? sender, EventArgs e)
    {
        CheckBinding();
    }

    private void BindTarget(bool bind)
    {
        if (!bind)
        {
            if (propInfo != null && control != null)
            {
                EventHandler eventHandler = Target_PropertyChanged;
                propInfo.RemoveValueChanged(control, eventHandler);
            }
            if (validateInfo != null)
            {
                CancelEventHandler cancelEventHandler = Target_Validate;
                validateInfo.RemoveEventHandler(control, cancelEventHandler);
            }
        }
        else if (IsBinding)
        {
            if (propInfo != null && control != null)
            {
                EventHandler eventHandler1 = Target_PropertyChanged;
                propInfo.AddValueChanged(control, eventHandler1);
            }
            if (validateInfo != null)
            {
                validateInfo.AddEventHandler(control, (CancelEventHandler)Target_Validate);
            }
        }
    }

    private void CheckBinding()
    {
        bindToObject?.CheckBinding();
        if (control == null || propertyName.Length <= 0)
        {
            propInfo = null;
            validateInfo = null;
        }
        else
        {
            control.DataBindings?.CheckDuplicates(this);
            var type = control.GetType();
            var str = string.Concat(propertyName, "IsNull");
            PropertyDescriptor? item = null;
            PropertyDescriptor? propertyDescriptor = null;
            var inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(control)[typeof(InheritanceAttribute)];
            var propertyDescriptorCollections = inheritanceAttribute == null || inheritanceAttribute.InheritanceLevel == InheritanceLevel.NotInherited ? TypeDescriptor.GetProperties(control) : TypeDescriptor.GetProperties(type);
            for (var i = 0; i < propertyDescriptorCollections.Count; i++)
            {
                if (item == null && string.Equals(propertyDescriptorCollections[i].Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    item = propertyDescriptorCollections[i];
                    if (propertyDescriptor != null)
                    {
                        break;
                    }
                }
                if (propertyDescriptor == null && string.Equals(propertyDescriptorCollections[i].Name, str, StringComparison.OrdinalIgnoreCase))
                {
                    propertyDescriptor = propertyDescriptorCollections[i];
                    if (item != null)
                    {
                        break;
                    }
                }
            }
            if (item == null)
            {
                throw new ArgumentException(@"ListBindingBindProperty", nameof(PropertyName));
            }
            if (item.IsReadOnly && controlUpdateMode != ControlUpdateMode.Never)
            {
                throw new ArgumentException(@"ListBindingBindPropertyReadOnly", nameof(PropertyName));
            }
            propInfo = item;
            propInfoConverter = propInfo.Converter;
            if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && !propertyDescriptor.IsReadOnly)
            {
                propIsNullInfo = propertyDescriptor;
            }
            EventDescriptor? eventDescriptor = null;
            var str1 = "Validating";
            var events = TypeDescriptor.GetEvents(control);
            var num = 0;
            while (num < events.Count)
            {
                if (eventDescriptor != null || !string.Equals(events[num].Name, str1, StringComparison.OrdinalIgnoreCase))
                {
                    num++;
                }
                else
                {
                    eventDescriptor = events[num];
                    break;
                }
            }
            validateInfo = eventDescriptor;
        }
        UpdateIsBinding();
    }

    internal bool ControlAtDesignTime()
    {
        IComponent? component = control;
        var site = component?.Site;
        if (site == null)
        {
            return false;
        }
        return site.DesignMode;
    }

    private BindingCompleteEventArgs CreateBindingCompleteEventArgs(BindingCompleteContext context, Exception? ex)
    {
        var flag = false;
        string? empty;
        var bindingCompleteState = BindingCompleteState.Success;
        if (ex == null)
        {
            empty = BindToObject?.DataErrorText;
            if (!string.IsNullOrEmpty(empty))
            {
                bindingCompleteState = BindingCompleteState.DataError;
            }
        }
        else
        {
            empty = ex.Message;
            bindingCompleteState = BindingCompleteState.Exception;
            flag = true;
        }
        return new BindingCompleteEventArgs(this, bindingCompleteState, context, empty, ex, flag);
    }

    private object? FormatObject(object? value)
    {
        if (ControlAtDesignTime())
        {
            return value;
        }
        var propertyType = propInfo?.PropertyType ?? typeof(object);
        if (formattingEnabled)
        {
            var convertEventArg = new ConvertEventArgs(value, propertyType);
            OnFormat(convertEventArg);
            if (convertEventArg.Value != value)
            {
                return convertEventArg.Value;
            }
            TypeConverter? converter = null;
            if (bindToObject?.FieldInfo != null)
            {
                converter = bindToObject.FieldInfo.Converter;
            }
            return Formatter.FormatObject(value, propertyType, converter, propInfoConverter, formatString, formatInfo, nullValue, dsNullValue);
        }
        var convertEventArg1 = new ConvertEventArgs(value, propertyType);
        OnFormat(convertEventArg1);
        var obj = convertEventArg1.Value;
        if (propertyType == typeof(object))
        {
            return value;
        }
        if (obj != null && (obj.GetType().IsSubclassOf(propertyType) || obj.GetType() == propertyType))
        {
            return obj;
        }
        var typeConverter = TypeDescriptor.GetConverter(value != null ? value.GetType() : typeof(object));
        if (typeConverter != null && typeConverter.CanConvertTo(propertyType))
        {
            if (value != null)
            {
                obj = typeConverter.ConvertTo(value, propertyType);
            }

            return obj;
        }
        if (value is IConvertible)
        {
            obj = Convert.ChangeType(value, propertyType, CultureInfo.CurrentCulture);
            if (obj != null && (obj.GetType().IsSubclassOf(propertyType) || obj.GetType() == propertyType))
            {
                return obj;
            }
        }
        throw new FormatException("ListBindingFormatFailed");
    }

    private void FormLoaded(object? sender, EventArgs e)
    {
        CheckBinding();
    }

    private object? GetDataSourceNullValue(Type? type)
    {
        if (!dsNullValueSet)
        {
            return Formatter.GetDefaultDataSourceNullValue(type);
        }
        return dsNullValue;
    }

    private object? GetPropValue()
    {
        var obj = DataSourceNullValue;
        var value = false;
        if (propIsNullInfo != null)
        {
            value = control != null && (bool)propIsNullInfo.GetValue(control);
        }

        if (control != null)
        {
            obj = !value ? propInfo?.GetValue(control) ?? DataSourceNullValue : DataSourceNullValue;
        }

        return obj;
    }

    internal static bool IsComponentCreated(IBindableComponent? component)
    {
        var control = component as Control;
        if (control == null)
        {
            return true;
        }
        return control.Created;
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Binding.BindingComplete" /> event. </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.BindingCompleteEventArgs" />  that contains the event data. </param>
    protected virtual void OnBindingComplete(BindingCompleteEventArgs e)
    {
        if (!inOnBindingComplete)
        {
            try
            {
                try
                {
                    inOnBindingComplete = true;
                    onComplete?.Invoke(this, e);
                }
                catch (Exception exception)
                {
                    if (ClientUtils.IsSecurityOrCriticalException(exception))
                    {
                        throw;
                    }
                    e.Cancel = true;
                }
            }
            finally
            {
                inOnBindingComplete = false;
            }
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Binding.Format" /> event.</summary>
    /// <param name="cevent">A <see cref="T:System.Windows.Forms.ConvertEventArgs" /> that contains the event data. </param>
    protected virtual void OnFormat(ConvertEventArgs cevent)
    {
        onFormat?.Invoke(this, cevent);
        if (!formattingEnabled && !(cevent.Value is DBNull) && cevent.DesiredType != null && !cevent.DesiredType.IsInstanceOfType(cevent.Value) && cevent.Value is IConvertible)
        {
            cevent.Value = Convert.ChangeType(cevent.Value, cevent.DesiredType, CultureInfo.CurrentCulture);
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Binding.Parse" /> event.</summary>
    /// <param name="cevent">A <see cref="T:System.Windows.Forms.ConvertEventArgs" /> that contains the event data. </param>
    protected virtual void OnParse(ConvertEventArgs cevent)
    {
        onParse?.Invoke(this, cevent);
        if (!formattingEnabled && !(cevent.Value is DBNull) && cevent is { Value: not null, DesiredType: not null } && !cevent.DesiredType.IsInstanceOfType(cevent.Value) && cevent.Value is IConvertible)
        {
            cevent.Value = Convert.ChangeType(cevent.Value, cevent.DesiredType, CultureInfo.CurrentCulture);
        }
    }

    private object? ParseObject(object? value)
    {
        var bindToType = bindToObject?.BindToType;
        if (formattingEnabled)
        {
            var convertEventArg = new ConvertEventArgs(value, bindToType);
            OnParse(convertEventArg);
            var obj = convertEventArg.Value;
            if (!Equals(value, obj))
            {
                return obj;
            }
            TypeConverter? converter = null;
            if (bindToObject?.FieldInfo != null)
            {
                converter = bindToObject.FieldInfo.Converter;
            }
            return Formatter.ParseObject(value, bindToType, value == null ? propInfo?.PropertyType : value.GetType(), converter, propInfoConverter, formatInfo, nullValue, GetDataSourceNullValue(bindToType));
        }
        var convertEventArg1 = new ConvertEventArgs(value, bindToType);
        OnParse(convertEventArg1);
        if (convertEventArg1.Value != null && bindToType != null && (convertEventArg1.Value.GetType().IsSubclassOf(bindToType) || convertEventArg1.Value.GetType() == bindToType || convertEventArg1.Value is DBNull))
        {
            return convertEventArg1.Value;
        }
        var typeConverter = TypeDescriptor.GetConverter(value != null ? value.GetType() : typeof(object));
        if (typeConverter != null && bindToType != null && typeConverter.CanConvertTo(bindToType))
        {
            if (value != null)
            {
                return typeConverter.ConvertTo(value, bindToType);
            }

            return null;
        }
        if (value is IConvertible)
        {
            if (bindToType != null)
            {
                var obj1 = Convert.ChangeType(value, bindToType, CultureInfo.CurrentCulture);
                if (obj1 != null && (obj1.GetType().IsSubclassOf(bindToType) || obj1.GetType() == bindToType))
                {
                    return obj1;
                }
            }
        }
        return null;
    }

    internal bool PullData()
    {
        return PullData(true, false);
    }

    internal bool PullData(bool reformat)
    {
        return PullData(reformat, false);
    }

    internal bool PullData(bool reformat, bool force)
    {
        if (ControlUpdateMode == ControlUpdateMode.Never)
        {
            reformat = false;
        }
        var flag = false;
        object? value = null;
        Exception? exception = null;
        if (!IsBinding)
        {
            return false;
        }
        if (!force)
        {
            if (propInfo is { SupportsChangeEvents: true } && !modified)
            {
                return false;
            }
            if (DataSourceUpdateMode == DataSourceUpdateMode.Never)
            {
                return false;
            }
        }
        if (inPushOrPull && formattingEnabled)
        {
            return false;
        }
        inPushOrPull = true;
        var propValue = GetPropValue();
        try
        {
            value = ParseObject(propValue);
        }
        catch (Exception exception1)
        {
            exception = exception1;
        }
        try
        {
            try
            {
                if (exception != null || !FormattingEnabled && value == null)
                {
                    flag = true;
                    value = bindToObject?.GetValue();
                }
                if (reformat && !FormattingEnabled | !flag)
                {
                    var obj = FormatObject(value);
                    if (force || !FormattingEnabled || !Equals(obj, propValue))
                    {
                        SetPropValue(obj);
                    }
                }
                if (!flag)
                {
                    bindToObject?.SetValue(value);
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                if (!FormattingEnabled)
                {
                    throw;
                }
            }
        }
        finally
        {
            inPushOrPull = false;
        }
        if (!FormattingEnabled)
        {
            modified = false;
            return false;
        }
        var bindingCompleteEventArg = CreateBindingCompleteEventArgs(BindingCompleteContext.DataSourceUpdate, exception);
        OnBindingComplete(bindingCompleteEventArg);
        if (bindingCompleteEventArg.BindingCompleteState == BindingCompleteState.Success && !bindingCompleteEventArg.Cancel)
        {
            modified = false;
        }
        return bindingCompleteEventArg.Cancel;
    }

    internal bool PushData()
    {
        return PushData(false);
    }

    internal bool PushData(bool force)
    {
        Exception? exception = null;
        if (!force && ControlUpdateMode == ControlUpdateMode.Never)
        {
            return false;
        }
        if (inPushOrPull && formattingEnabled)
        {
            return false;
        }
        inPushOrPull = true;
        try
        {
            try
            {
                if (!IsBinding)
                {
                    SetPropValue(null);
                }
                else
                {
                    SetPropValue(FormatObject(bindToObject?.GetValue()));
                    modified = false;
                }
            }
            catch (Exception? exception1)
            {
                exception = exception1;
                if (!FormattingEnabled)
                {
                    throw;
                }
            }
        }
        finally
        {
            inPushOrPull = false;
        }
        if (!FormattingEnabled)
        {
            return false;
        }
        var bindingCompleteEventArg = CreateBindingCompleteEventArgs(BindingCompleteContext.ControlUpdate, exception);
        OnBindingComplete(bindingCompleteEventArg);
        return bindingCompleteEventArg.Cancel;
    }

    /// <summary>Sets the control property to the value read from the data source.</summary>
    public void ReadValue()
    {
        PushData(true);
    }

    internal void SetBindableComponent(IBindableComponent? value)
    {
        BindingContext? bindingContext;
        if (control != value)
        {
            var bindableComponent = control;
            BindTarget(false);
            control = value;
            BindTarget(true);
            try
            {
                CheckBinding();
            }
            catch
            {
                BindTarget(false);
                control = bindableComponent;
                BindTarget(true);
                throw;
            }
            if (control == null || !IsComponentCreated(control))
            {
                bindingContext = null;
            }
            else
            {
                bindingContext = control.BindingContext;
            }
            BindingContext.UpdateBinding(bindingContext, this);
            var form = value as Form;
            if (form != null)
            {
                form.Load += FormLoaded;
            }
        }
    }

    internal void SetListManager(BindingManagerBase? bindingManager)
    {
        if (bindingManagerBase is CurrencyManager)
        {
            ((CurrencyManager)bindingManagerBase).MetaDataChanged -= binding_MetaDataChanged;
        }
        bindingManagerBase = bindingManager;
        if (bindingManagerBase is CurrencyManager)
        {
            ((CurrencyManager)bindingManagerBase).MetaDataChanged += binding_MetaDataChanged;
        }
        BindToObject?.SetBindingManagerBase(bindingManager);
        CheckBinding();
    }

    private void SetPropValue(object? value)
    {
        if (ControlAtDesignTime())
        {
            return;
        }
        inSetPropValue = true;
        try
        {
            if (value == null ? false : !Formatter.IsNullData(value, DataSourceNullValue))
            {
                propInfo?.SetValue(control, value);
            }
            else if (propIsNullInfo != null)
            {
                propIsNullInfo.SetValue(control, true);
            }
            else if (propInfo?.PropertyType != typeof(object))
            {
                propInfo?.SetValue(control, null);
            }
            else
            {
                propInfo.SetValue(control, DataSourceNullValue);
            }
        }
        finally
        {
            inSetPropValue = false;
        }
    }

    private bool ShouldSerializeDataSourceNullValue()
    {
        if (!dsNullValueSet)
        {
            return false;
        }
        return dsNullValue != Formatter.GetDefaultDataSourceNullValue(null);
    }

    private bool ShouldSerializeFormatString()
    {
        if (formatString == null)
        {
            return false;
        }
        return formatString.Length > 0;
    }

    private bool ShouldSerializeNullValue()
    {
        return nullValue != null;
    }

    private void Target_PropertyChanged(object? sender, EventArgs e)
    {
        if (inSetPropValue)
        {
            return;
        }
        if (IsBinding)
        {
            modified = true;
            if (DataSourceUpdateMode == DataSourceUpdateMode.OnPropertyChanged)
            {
                PullData(false);
                modified = true;
            }
        }
    }

    private void Target_Validate(object? sender, CancelEventArgs e)
    {
        try
        {
            if (PullData(true))
            {
                e.Cancel = true;
            }
        }
        catch
        {
            e.Cancel = true;
        }
    }

    internal void UpdateIsBinding()
    {
        var flag = IsBindable && ComponentCreated && (bindingManagerBase?.IsBinding ?? false);
        if (bound != flag)
        {
            bound = flag;
            BindTarget(flag);
            if (bound)
            {
                if (controlUpdateMode == ControlUpdateMode.Never)
                {
                    PullData(false, true);
                    return;
                }
                PushData();
            }
        }
    }

    /// <summary>Reads the current value from the control property and writes it to the data source.</summary>
    public void WriteValue()
    {
        PullData(true, true);
    }

    /// <summary>Occurs when the <see cref="P:System.Windows.Forms.Binding.FormattingEnabled" /> property is set to true and a binding operation is complete, such as when data is pushed from the control to the data source or vice versa</summary>
    public event BindingCompleteEventHandler? BindingComplete
    {
        add => onComplete += value;
        remove => onComplete -= value;
    }

    /// <summary>Occurs when the property of a control is bound to a data value.</summary>
    /// <filterpriority>1</filterpriority>
    public event ConvertEventHandler? Format
    {
        add => onFormat += value;
        remove => onFormat -= value;
    }

    /// <summary>Occurs when the value of a data-bound control changes.</summary>
    /// <filterpriority>1</filterpriority>
    public event ConvertEventHandler? Parse
    {
        add => onParse += value;
        remove => onParse -= value;
    }
}