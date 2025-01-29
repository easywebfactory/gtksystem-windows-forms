// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Windows.Forms
{
    /// <summary>
    ///     Defines constants that represent the possible states of a System.Windows.Forms.TreeNode.
    /// </summary>
    [Flags]
    public enum TreeNodeStates
    {
        /// <summary>
        ///     The node is selected.
        /// </summary>
        Selected = 1,
        /// <summary>
        ///     The node is disabled.
        /// </summary>
        Grayed = 2,
        /// <summary>
        ///     The node is checked.
        /// </summary>
        Checked = 8,
        /// <summary>
        ///     The node has focus.
        /// </summary>
        Focused = 16,
        /// <summary>
        ///     The node is in its default state.
        /// </summary>
        Default = 32,
        /// <summary>
        ///     The node is hot. This state occurs when the System.Windows.Forms.TreeView.HotTracking
        ///     property is set to true and the mouse pointer is over the node.
        /// </summary>
        Hot = 64,
        /// <summary>
        ///     The node is marked.
        /// </summary>
        Marked = 128,
        /// <summary>
        ///     The node in an indeterminate state.
        /// </summary>
        Indeterminate = 256,
        /// <summary>
        ///     The node should indicate a keyboard shortcut.
        /// </summary>
        ShowKeyboardCues = 512
    }
}
