﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Windows.Forms
{
    /// <summary>
    ///  Specifies the appearance of a button.
    /// </summary>
    [Flags]
    public enum ButtonState
    {
        //
        // 摘要:
        //     The button has its normal appearance (three-dimensional).
        Normal = 0,
        //
        // 摘要:
        //     The button is inactive (grayed).
        Inactive = 256,
        //
        // 摘要:
        //     The button appears pressed.
        Pushed = 512,
        //
        // 摘要:
        //     The button has a checked or latched appearance. Use this appearance to show that
        //     a toggle button has been pressed.
        Checked = 1024,
        //
        // 摘要:
        //     The button has a flat, two-dimensional appearance.
        Flat = 16384,
        /// <summary>
        ///  All viable flags in the bit mask are used.
        /// </summary>
        All = Flat | Checked | Pushed | Inactive,
    }
}
