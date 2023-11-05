// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Windows.Forms
{
    //
    // 摘要:
    //     Defines constants that represent the possible states of a System.Windows.Forms.ListViewItem.
    [Flags]
    public enum ListViewItemStates
    {
        //
        // 摘要:
        //     The item is selected.
        Selected = 1,
        //
        // 摘要:
        //     The item is disabled.
        Grayed = 2,
        //
        // 摘要:
        //     The item is checked.
        Checked = 8,
        //
        // 摘要:
        //     The item has focus.
        Focused = 16,
        //
        // 摘要:
        //     The item is in its default state.
        Default = 32,
        //
        // 摘要:
        //     The item is currently under the mouse pointer.
        Hot = 64,
        //
        // 摘要:
        //     The item is marked.
        Marked = 128,
        //
        // 摘要:
        //     The item is in an indeterminate state.
        Indeterminate = 256,
        //
        // 摘要:
        //     The item should indicate a keyboard shortcut.
        ShowKeyboardCues = 512
    }
}
