﻿namespace System.Windows.Forms
{
    public enum BorderStyle
    {
        //
        // 摘要:
        //     No border.
        None = 0,
        //
        // 摘要:
        //     A single-line border.
        FixedSingle = 1,
        //
        // 摘要:
        //     A three-dimensional border.
        Fixed3D = 2
    }
    public enum FormBorderStyle
    {
        //
        // 摘要:
        //     No border.
        None,
        //
        // 摘要:
        //     A fixed, single-line border.
        FixedSingle,
        //
        // 摘要:
        //     A fixed, three-dimensional border.
        Fixed3D,
        //
        // 摘要:
        //     A thick, fixed dialog-style border.
        FixedDialog,
        //
        // 摘要:
        //     A resizable border.
        Sizable,
        //
        // 摘要:
        //     A tool window border that is not resizable. A tool window does not appear in
        //     the taskbar or in the window that appears when the user presses ALT+TAB. Although
        //     forms that specify System.Windows.Forms.FormBorderStyle.FixedToolWindow typically
        //     are not shown in the taskbar, you must also ensure that the System.Windows.Forms.Form.ShowInTaskbar
        //     property is set to false, since its default value is true.
        FixedToolWindow,
        //
        // 摘要:
        //     A resizable tool window border. A tool window does not appear in the taskbar
        //     or in the window that appears when the user presses ALT+TAB.
        SizableToolWindow
    }

    public enum FormStartPosition
    {
        WindowsDefaultLocation,
        CenterScreen,
        Manual,
        CenterParent,
        WindowsDefaultBounds
    }
    public enum SizeGripStyle
    {
        /// <summary>
        ///  The size grip is automatically display when needed.
        /// </summary>
        Auto = 0,

        /// <summary>
        ///  The sizing grip is always shown on the form.
        /// </summary>
        Show = 1,

        /// <summary>
        ///  The sizing grip is hidden.
        /// </summary>
        Hide = 2,
    }
    public enum Column
    {
        Fixed,
        Number,
        Severity,
        Description,
        Pulse,
        Icon,
        Active,
        Sensitive,
        Num
    };

    //
    // 摘要:
    //     Defines constants that indicate the alignment of content within a System.Windows.Forms.DataGridView
    //     cell.
    public enum DataGridViewContentAlignment
    {
        //
        // 摘要:
        //     The alignment is not set.
        NotSet = 0,
        //
        // 摘要:
        //     The content is aligned vertically at the top and horizontally at the left of
        //     a cell.
        TopLeft = 1,
        //
        // 摘要:
        //     The content is aligned vertically at the top and horizontally at the center of
        //     a cell.
        TopCenter = 2,
        //
        // 摘要:
        //     The content is aligned vertically at the top and horizontally at the right of
        //     a cell.
        TopRight = 4,
        //
        // 摘要:
        //     The content is aligned vertically at the middle and horizontally at the left
        //     of a cell.
        MiddleLeft = 16,
        //
        // 摘要:
        //     The content is aligned at the vertical and horizontal center of a cell.
        MiddleCenter = 32,
        //
        // 摘要:
        //     The content is aligned vertically at the middle and horizontally at the right
        //     of a cell.
        MiddleRight = 64,
        //
        // 摘要:
        //     The content is aligned vertically at the bottom and horizontally at the left
        //     of a cell.
        BottomLeft = 256,
        //
        // 摘要:
        //     The content is aligned vertically at the bottom and horizontally at the center
        //     of a cell.
        BottomCenter = 512,
        //
        // 摘要:
        //     The content is aligned vertically at the bottom and horizontally at the right
        //     of a cell.
        BottomRight = 1024
    }
}
