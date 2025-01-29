// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Windows.Forms
{
    /// <summary>
    ///     Specifies the state of an item that is being drawn.
    /// </summary>
    [Flags]
    public enum DrawItemState
    {
        /// <summary>
        ///     The item currently has no state.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The item is selected.
        /// </summary>
        Selected = 1,

        /// <summary>
        ///     The item is grayed. Only menu controls use this value.
        /// </summary>
        Grayed = 2,

        /// <summary>
        ///     The item is unavailable.
        /// </summary>
        Disabled = 4,

        /// <summary>
        ///     The item is checked. Only menu controls use this value.
        /// </summary>
        Checked = 8,

        /// <summary>
        ///     The item has focus.
        /// </summary>
        Focus = 16,

        /// <summary>
        ///     The item is in its default visual state.
        /// </summary>
        Default = 32,

        /// <summary>
        ///     The item is being hot-tracked, that is, the item is highlighted as the mouse
        ///     pointer passes over it.
        /// </summary>
        HotLight = 64,

        /// <summary>
        ///     The item is inactive.
        /// </summary>
        Inactive = 128,

        /// <summary>
        ///     The item displays without a keyboard accelerator.
        /// </summary>
        NoAccelerator = 256,

        /// <summary>
        ///     The item displays without the visual cue that indicates it has focus.
        /// </summary>
        NoFocusRect = 512,

        /// <summary>
        ///     The item is the editing portion of a System.Windows.Forms.ComboBox.
        /// </summary>
        ComboBoxEdit = 4096
    }
}