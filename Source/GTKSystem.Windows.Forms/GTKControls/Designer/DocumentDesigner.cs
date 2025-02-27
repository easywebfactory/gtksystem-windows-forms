// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;


namespace System.Windows.Forms.Design;

/// <summary>
///  Provides a designer that extends the ScrollableControlDesigner and implements
///  IRootDesigner.
/// </summary>
[ToolboxItemFilter("System.Windows.Forms")]
public class DocumentDesigner : ScrollableControlDesigner, IRootDesigner, IToolboxUser
{

    internal static IDesignerSerializationManager? manager;

    public ViewTechnology[] SupportedTechnologies => throw new NotImplementedException();

    public override IComponent Component => throw new NotImplementedException();

    public override DesignerVerbCollection Verbs => throw new NotImplementedException();

    public override bool CanModifyComponents => throw new NotImplementedException();

    public object GetView(ViewTechnology technology)
    {
        throw new NotImplementedException();
    }

    public override void DoDefaultAction()
    {
        throw new NotImplementedException();
    }

    public new void Initialize(IComponent component)
    {
        throw new NotImplementedException();
    }

    public override void Dispose()
    {
        throw new NotImplementedException();
    }

    public bool GetToolSupported(ToolboxItem tool)
    {
        throw new NotImplementedException();
    }

    public void ToolPicked(ToolboxItem tool)
    {
        throw new NotImplementedException();
    }

    public override bool AddComponent(IComponent component, string name, bool firstAdd)
    {
        throw new NotImplementedException();
    }

    public override bool IsDropOk(IComponent component)
    {
        throw new NotImplementedException();
    }

    public override Control GetDesignerControl()
    {
        throw new NotImplementedException();
    }

    public override Control GetControlForComponent(object? component)
    {
        throw new NotImplementedException();
    }
}