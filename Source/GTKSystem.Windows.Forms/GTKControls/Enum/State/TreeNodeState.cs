// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Windows.Forms
{
    //
    // 摘要:
    //     Defines constants that represent the possible states of a System.Windows.Forms.TreeNode.
    [Flags]
    public enum TreeNodeStates
    {
        //
        // 摘要:
        //     The node is selected.
        Selected = 1,
        //
        // 摘要:
        //     The node is disabled.
        Grayed = 2,
        //
        // 摘要:
        //     The node is checked.
        Checked = 8,
        //
        // 摘要:
        //     The node has focus.
        Focused = 16,
        //
        // 摘要:
        //     The node is in its default state.
        Default = 32,
        //
        // 摘要:
        //     The node is hot. This state occurs when the System.Windows.Forms.TreeView.HotTracking
        //     property is set to true and the mouse pointer is over the node.
        Hot = 64,
        //
        // 摘要:
        //     The node is marked.
        Marked = 128,
        //
        // 摘要:
        //     The node in an indeterminate state.
        Indeterminate = 256,
        //
        // 摘要:
        //     The node should indicate a keyboard shortcut.
        ShowKeyboardCues = 512
    }
}
