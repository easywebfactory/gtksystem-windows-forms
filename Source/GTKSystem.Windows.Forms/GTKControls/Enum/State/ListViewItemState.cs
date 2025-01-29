// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Windows.Forms
{
    /// <summary>
    ///     Defines constants that represent the possible states of a System.Windows.Forms.ListViewItem.
    /// </summary>
    [Flags]
    public enum ListViewItemStates
    {
        /// <summary>
        ///     The item is selected.
        /// </summary>
        Selected = 1,
        /// <summary>
        ///     The item is disabled.
        /// </summary>
        Grayed = 2,
        /// <summary>
        ///     The item is checked.
        /// </summary>
        Checked = 8,
        /// <summary>
        ///     The item has focus.
        /// </summary>
        Focused = 16,
        /// <summary>
        ///     The item is in its default state.
        /// </summary>
        Default = 32,
        /// <summary>
        ///     The item is currently under the mouse pointer.
        /// </summary>
        Hot = 64,
        /// <summary>
        ///     The item is marked.
        /// </summary>
        Marked = 128,
        /// <summary>
        ///     The item is in an indeterminate state.
        /// </summary>
        Indeterminate = 256,
        /// <summary>
        ///     The item should indicate a keyboard shortcut.
        /// </summary>
        ShowKeyboardCues = 512
    }
}
