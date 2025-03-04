// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;

namespace System.ComponentModel.Design;

/// <summary>
///  The default designer for all components.
/// </summary>
public class ComponentDesigner : ITreeDesigner, IDesignerFilter, IComponentInitializer
{
    private protected const string settingsKeyName = "SettingsKey";

    public virtual ICollection Children => throw new NotImplementedException();

    public virtual IDesigner Parent => throw new NotImplementedException();

    public virtual IComponent Component => throw new NotImplementedException();

    public virtual DesignerVerbCollection Verbs => throw new NotImplementedException();

    public virtual void DoDefaultAction()
    {

    }

    public virtual void Initialize(IComponent component)
    {

    }

    public virtual void Dispose()
    {

    }

    public void PostFilterAttributes(IDictionary attributes)
    {

    }

    public void PostFilterEvents(IDictionary events)
    {

    }

    public void PostFilterProperties(IDictionary properties)
    {

    }

    public void PreFilterAttributes(IDictionary attributes)
    {

    }

    public void PreFilterEvents(IDictionary events)
    {

    }

    public void PreFilterProperties(IDictionary properties)
    {

    }

    public void InitializeExistingComponent(IDictionary defaultValues)
    {

    }

    public void InitializeNewComponent(IDictionary defaultValues)
    {

    }
}