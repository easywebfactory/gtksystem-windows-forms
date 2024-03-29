﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Windows.Forms
{
    //
    // 摘要:
    //     Specifies the state of an item that is being drawn.
    [Flags]
    public enum DrawItemState
    {
        //
        // 摘要:
        //     The item currently has no state.
        None = 0,
        //
        // 摘要:
        //     The item is selected.
        Selected = 1,
        //
        // 摘要:
        //     The item is grayed. Only menu controls use this value.
        Grayed = 2,
        //
        // 摘要:
        //     The item is unavailable.
        Disabled = 4,
        //
        // 摘要:
        //     The item is checked. Only menu controls use this value.
        Checked = 8,
        //
        // 摘要:
        //     The item has focus.
        Focus = 16,
        //
        // 摘要:
        //     The item is in its default visual state.
        Default = 32,
        //
        // 摘要:
        //     The item is being hot-tracked, that is, the item is highlighted as the mouse
        //     pointer passes over it.
        HotLight = 64,
        //
        // 摘要:
        //     The item is inactive.
        Inactive = 128,
        //
        // 摘要:
        //     The item displays without a keyboard accelerator.
        NoAccelerator = 256,
        //
        // 摘要:
        //     The item displays without the visual cue that indicates it has focus.
        NoFocusRect = 512,
        //
        // 摘要:
        //     The item is the editing portion of a System.Windows.Forms.ComboBox.
        ComboBoxEdit = 4096
    }
}
