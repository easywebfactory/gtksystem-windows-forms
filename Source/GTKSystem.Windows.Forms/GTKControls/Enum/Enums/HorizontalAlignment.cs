﻿using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    /// <summary>
    ///  Specifies how an object or text in a control is horizontally aligned
    ///  relative to an element of the control.
    /// </summary>
    [ComVisible(true)]
    public enum HorizontalAlignment
    {
        /// <summary>
        ///  The object or text is aligned on the left of the control element.
        /// </summary>
        Left = 0,

        /// <summary>
        ///  The object or text is aligned on the right of the control element.
        /// </summary>
        Right = 1,

        /// <summary>
        ///  The object or text is aligned in the center of the control element.
        /// </summary>
        Center = 2,
    }
}