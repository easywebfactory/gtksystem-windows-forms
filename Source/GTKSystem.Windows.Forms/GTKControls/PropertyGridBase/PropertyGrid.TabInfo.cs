// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Windows.Forms.Design;

namespace System.Windows.Forms;

public partial class PropertyGrid
{
    private class TabInfo(PropertyTab tab, PropertyTabScope scope, ToolStripButton button)
    {
        public Type TabType => Tab.GetType();
        public PropertyTab Tab { get; } = tab;
        public PropertyTabScope Scope { get; } = scope;
        public ToolStripButton Button { get; } = button;

        public void Deconstruct(out PropertyTab tab, out PropertyTabScope scope, out ToolStripButton button)
        {
            tab = Tab;
            scope = Scope;
            button = Button;
        }
    }
}