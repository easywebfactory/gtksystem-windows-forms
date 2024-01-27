// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using GLib;
//using Gtk;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{

    public class ToolStripSeparator : Gtk.SeparatorMenuItem
    {
        public ToolStripSeparator(IntPtr raw)
            : base(raw)
        {
        }

        public ToolStripSeparator()
        {
        }

    }
}
