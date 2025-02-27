// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Windows.Forms;

internal partial class PropertyStore
{
    private sealed class PaddingWrapper
    {
        public Padding padding;

        public PaddingWrapper(Padding padding)
        {
            this.padding = padding;
        }
    }
}
