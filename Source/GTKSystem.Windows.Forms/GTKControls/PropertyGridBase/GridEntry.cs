// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms.PropertyGridInternal;

public partial class GridEntry : GridItem, ITypeDescriptorContext
{
    public PropertyGrid OwnerGrid { get; set; }
    public GridEntry _parent;
    public object OwnerObj { get; set; }
    protected GridEntry(PropertyGrid ownerGrid, GridEntry parent)
    {
        _parent = parent;
        OwnerGrid = ownerGrid;
    }
    public GridEntry(GridEntry parent, GridItemType itemtype, int level, string label, object entryobj, string description)
    {
        _parent = parent;
        this.Parent = parent;
        this.GridItemType = itemtype;
        this.Level = level;
        this.Label = label;
        this.OwnerObj = entryobj;
        this.Description = description;
    }
    public override GridItemCollection GridItems
    {
        get
        {
            List<GridEntry> items = new List<GridEntry>();
            object obj = Value;
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            if (properties != null)
            {
                foreach (PropertyInfo property in properties)
                {
                    if (property.CanWrite && property.CanRead)
                    {
                        string description = "";
                        var attri3 = property.GetCustomAttributes(typeof(DescriptionAttribute));
                        if (attri3 != null && attri3.Count() > 0)
                        {
                            DescriptionAttribute attribute = (DescriptionAttribute)attri3.FirstOrDefault();
                            description = attribute.Description;
                        }
                        items.Add(new GridEntry(this, GridItemType.Property, 2, property.Name, obj, description) { Editable = property.CanWrite, ValueType = property.PropertyType, PropertyInfo = property });
                    }
                }
            }
            return new GridItemCollection(items.ToArray());
        }
    }

    public override GridItemType GridItemType { get; }

    public override string Label { get; }

    public override GridItem Parent { get; }

    public override PropertyDescriptor PropertyDescriptor { get; }
    public override object Value
    {
        get
        {
            if (PropertyInfo == null)
            {
                return OwnerObj.GetType().GetProperty(this.Label)?.GetValue(OwnerObj);
            }
            else
            {
                return PropertyInfo.GetValue(OwnerObj);
            }
        }
    }
    public override void SetValue(object value)
    {
        PropertyInfo?.SetValue(OwnerObj,value);
    }
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
