// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.Designer;

/// <summary>
///  Provides a designer that extends the ScrollableControlDesigner and implements
///  IRootDesigner.
/// </summary>
[ToolboxItemFilter("System.Windows.Forms")]
public class DocumentDesigner : ScrollableControlDesigner, IRootDesigner, IToolboxUser
{

    internal static IDesignerSerializationManager? manager;

    public ViewTechnology[] SupportedTechnologies => throw new NotImplementedException();

    public object GetView(ViewTechnology technology)
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
}
