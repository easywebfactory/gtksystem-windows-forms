﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{

    public partial class PropertyGrid
    {
        private record TabInfo(PropertyTab Tab, PropertyTabScope Scope, ToolStripButton Button)
        {
            public Type TabType => Tab.GetType();
            public PropertyTab Tab { get; } = Tab;
            public PropertyTabScope Scope { get; } = Scope;
            public ToolStripButton Button { get; } = Button;

            public void Deconstruct(out PropertyTab Tab, out PropertyTabScope Scope, out ToolStripButton Button)
            {
                Tab = this.Tab;
                Scope = this.Scope;
                Button = this.Button;
            }
        }
    }
}