// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Drawing
{
    //
    // summary:
    //     Specifies style information applied to text.
    [Flags]
    public enum FontStyle
    {
        //
        // summary:
        //     Normal text.
        Regular = 0,
        //
        // summary:
        //     Bold text.
        Bold = 1,
        //
        // summary:
        //     Italic text.
        Italic = 2,
        //
        // summary:
        //     Underlined text.
        Underline = 4,
        //
        // summary:
        //     Text with a line through the middle.
        Strikeout = 8
    }
}
