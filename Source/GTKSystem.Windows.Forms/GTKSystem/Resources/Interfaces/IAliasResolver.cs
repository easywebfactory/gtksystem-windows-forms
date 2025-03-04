// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace System.Windows.Forms.Resources.Interfaces;

internal interface IAliasResolver
{
    AssemblyName? ResolveAlias(string alias);
    void PushAlias(string alias, AssemblyName name);
}