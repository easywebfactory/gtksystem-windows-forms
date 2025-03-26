// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms.PropertyGridInternal;

internal class GridEntry : GridItem, ITypeDescriptorContext
{
    public PropertyGrid? OwnerGrid { get; set; }
    public GridEntry? _parent;
    protected GridEntry(PropertyGrid? ownerGrid, GridEntry? parent)
    {
        _parent = parent;
        OwnerGrid = ownerGrid;
    }
    public GridEntry(GridEntry? parent, GridItemType itemtype, int level, string? label, object? value, string? description)
    {
        _parent = parent;
        Parent = parent;
        GridItemType = itemtype;
        Level = level;
        Label = label;
        this.value = value;
        Description = description;
    }

    public override GridItemCollection? GridItems => gridItems;

    public override GridItemType GridItemType { get; }

    public override string? Label { get; }

    public override GridItem? Parent { get; }

    public override PropertyDescriptor? PropertyDescriptor => propertyDescriptor;

    internal object? value;
    internal GridItemCollection? gridItems;
    internal PropertyDescriptor? propertyDescriptor;
    public override object? Value => value;

    public override bool Select()
    {
        return false;
    }

    public Type? ValueType { get; set; }
    public int Level { get; set; }
    public bool Editable { get; set; }
    public string? Description { get; set; }
    internal PropertyInfo? PropertyInfo { get; set; }



    public void OnComponentChanged()
    {
        throw new NotImplementedException();
    }

    public bool OnComponentChanging()
    {
        throw new NotImplementedException();
    }

    public object GetService(Type serviceType)
    {
        throw new NotImplementedException();
    }

    public IContainer Container => throw new NotImplementedException();

    public object Instance => throw new NotImplementedException();
}
