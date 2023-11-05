// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace System.Windows.Forms
{

    public class ToolStripTextBox : ToolStripItem
    {
        public ToolStripTextBox()
        {
            base.Text = "[TextBox]";
            base.ToolTipText = "No supper TextBox";
        }
    }

}
