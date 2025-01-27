// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;

namespace System.ComponentModel.Design
{
    /// <summary>
    ///  The default designer for all components.
    /// </summary>
    public partial class ComponentDesigner : ITreeDesigner, IDesignerFilter, IComponentInitializer
    {
        private InheritanceAttribute? _inheritanceAttribute;

        private DesignerVerbCollection? _verbs;
        private bool _settingsKeyExplicitlySet;
        private IComponent? _component;

        private protected const string SettingsKeyName = "SettingsKey";

        public ICollection Children => throw new NotImplementedException();

        public IDesigner Parent => throw new NotImplementedException();

        public IComponent Component => throw new NotImplementedException();

        public DesignerVerbCollection Verbs => throw new NotImplementedException();

        public void DoDefaultAction()
        {

        }

        public void Initialize(IComponent component)
        {

        }

        public void Dispose()
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
}