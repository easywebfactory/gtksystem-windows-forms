// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace System.Windows.Forms.Design;

/// <summary>
///  The ParentControlDesigner class builds on the ControlDesigner.  It adds the ability
///  to manipulate child components, and provides a selection UI handler for all
///  components it contains.
/// </summary>
public class ParentControlDesigner : ControlDesigner, IOleDragClient
{
    public virtual bool CanModifyComponents => throw new NotImplementedException();

    public virtual bool AddComponent(IComponent component, string name, bool firstAdd)
    {
        throw new NotImplementedException();
    }

    public virtual Control GetControlForComponent(object? component)
    {
        throw new NotImplementedException();
    }

    public virtual Control GetDesignerControl()
    {
        throw new NotImplementedException();
    }

    public virtual bool IsDropOk(IComponent component)
    {
        throw new NotImplementedException();
    }
}