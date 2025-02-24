// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace System.Windows.Forms.PropertyGridInternal;

internal partial class GridEntry : GridItem, ITypeDescriptorContext
{
    public PropertyGrid OwnerGrid { get; set; }
    public GridEntry _parent;
    protected GridEntry(PropertyGrid ownerGrid, GridEntry parent)
    {
        _parent = parent;
        OwnerGrid = ownerGrid;
    }
    public GridEntry(GridEntry parent, GridItemType itemtype, int level, string label, object value, string description)
    {
        _parent = parent;
        this.Parent = parent;
        this.GridItemType = itemtype;
        this.Level = level;
        this.Label = label;
        this.value = value;
        this.Description = description;
    }
    public override GridItemCollection GridItems { get; }

    public override GridItemType GridItemType { get; }

    public override string Label { get; }

    public override GridItem Parent { get; }

    public override PropertyDescriptor PropertyDescriptor { get; }
    internal object value;
    public override object Value { get => value; }

    public override bool Select()
    {
        return false;
    }

    public Type ValueType { get; set; }
    public int Level { get; set; }
    public bool Editable { get; set; }
    public string Description { get; set; }
    internal System.Reflection.PropertyInfo PropertyInfo { get; set; }



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
