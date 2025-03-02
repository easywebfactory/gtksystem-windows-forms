using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

/// <summary>Maintains a <see cref="T:System.Windows.Forms.Binding" /> between an object's property and a data-bound control property.</summary>
/// <filterpriority>2</filterpriority>
public class PropertyManager : BindingManagerBase
{
    private object? dataSource;

    private string? propName;

    private PropertyDescriptor? propInfo;

    private bool bound;

    internal override Type? BindType => dataSource?.GetType();

    /// <returns>The number of rows managed by the <see cref="T:System.Windows.Forms.BindingManagerBase" />.</returns>
    /// <filterpriority>1</filterpriority>
    public override int Count => 1;

    /// <summary>Gets the object to which the data-bound property belongs.</summary>
    /// <returns>An <see cref="T:System.Object" /> that represents the object to which the property belongs.</returns>
    /// <filterpriority>1</filterpriority>
    public override object? Current => dataSource;

    internal override object? DataSource => dataSource;

    internal override bool IsBinding => dataSource != null;

    /// <returns>A zero-based index that specifies a position in the underlying list.</returns>
    /// <filterpriority>1</filterpriority>
    public override int Position
    {
        get => 0;
        set
        {
        }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PropertyManager" /> class.</summary>
    public PropertyManager()
    {
    }

    internal PropertyManager(object? dataSource) : base(dataSource)
    {
    }

    internal PropertyManager(object? dataSource, string? propName)
    {
        Init(dataSource, propName);
    }

    private void Init(object? source, string? name)
    {
        propName = name;
        SetDataSource(source);
    }

    /// <filterpriority>1</filterpriority>
    public override void AddNew()
    {
        throw new NotSupportedException("DataBindingAddNewNotSupportedOnPropertyManager");
    }

    /// <filterpriority>1</filterpriority>
    public override void CancelCurrentEdit()
    {
        var current = Current as IEditableObject;
        current?.CancelEdit();
        PushData();
    }

    /// <filterpriority>1</filterpriority>
    public override void EndCurrentEdit()
    {
        bool flag;
        PullData(out flag);
        if (flag)
        {
            var current = Current as IEditableObject;
            current?.EndEdit();
        }
    }

    internal override PropertyDescriptorCollection? GetItemProperties(PropertyDescriptor?[]? listAccessors)
    {
        return ListBindingHelper.GetListItemProperties(dataSource, listAccessors);
    }

    internal override string? GetListName()
    {
        if (dataSource != null)
        {
            return string.Concat(TypeDescriptor.GetClassName(dataSource), ".", propName);
        }
        return null;
    }

    /// <returns>The name of the list supplying the data for the binding.</returns>
    /// <param name="listAccessors">An <see cref="T:System.Collections.ArrayList" /> containing the table's bound properties. </param>
    protected internal override string? GetListName(ArrayList? listAccessors)
    {
        return "";
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.CurrentChanged" /> event.</summary>
    /// <param name="ea">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected internal override void OnCurrentChanged(EventArgs ea)
    {
        PushData();
        onCurrentChangedHandler?.Invoke(this, ea);
        onCurrentItemChangedHandler?.Invoke(this, ea);
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.CurrentItemChanged" /> event.</summary>
    /// <param name="ea">An <see cref="T:System.EventArgs" /> containing the event data.</param>
    protected internal override void OnCurrentItemChanged(EventArgs ea)
    {
        PushData();
        onCurrentItemChangedHandler?.Invoke(this, ea);
    }

    private void PropertyChanged(object? sender, EventArgs ea)
    {
        EndCurrentEdit();
        OnCurrentChanged(EventArgs.Empty);
    }

    /// <param name="index">The index of the row to delete. </param>
    /// <filterpriority>1</filterpriority>
    public override void RemoveAt(int index)
    {
        throw new NotSupportedException("DataBindingRemoveAtNotSupportedOnPropertyManager");
    }

    /// <filterpriority>1</filterpriority>
    public override void ResumeBinding()
    {
        OnCurrentChanged(new EventArgs());
        if (!bound)
        {
            try
            {
                bound = true;
                UpdateIsBinding();
            }
            catch
            {
                bound = false;
                UpdateIsBinding();
                throw;
            }
        }
    }

    private protected override void SetDataSource(object? source)
    {
        if (dataSource != null && !string.IsNullOrEmpty(propName))
        {
            propInfo?.RemoveValueChanged(dataSource, PropertyChanged);
            propInfo = null;
        }
        dataSource = source;
        if (dataSource != null && !string.IsNullOrEmpty(propName))
        {
            propInfo = TypeDescriptor.GetProperties(source).Find(propName??string.Empty, true);
            if (propInfo == null)
            {
                throw new ArgumentException("PropertyManagerPropDoesNotExist");
            }
            propInfo.AddValueChanged(source!, PropertyChanged);
        }
    }

    /// <summary>Suspends the data binding between a data source and a data-bound property.</summary>
    /// <filterpriority>1</filterpriority>
    public override void SuspendBinding()
    {
        EndCurrentEdit();
        if (bound)
        {
            try
            {
                bound = false;
                UpdateIsBinding();
            }
            catch
            {
                bound = true;
                UpdateIsBinding();
                throw;
            }
        }
    }

    /// <summary>Updates the current <see cref="T:System.Windows.Forms.Binding" /> between a data binding and a data-bound property.</summary>
    protected override void UpdateIsBinding()
    {
        for (var i = 0; i < Bindings.Count; i++)
        {
            Bindings[i].UpdateIsBinding();
        }
    }
}