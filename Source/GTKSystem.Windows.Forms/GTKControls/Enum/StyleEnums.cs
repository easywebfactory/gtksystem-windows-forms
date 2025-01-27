namespace System.Windows.Forms
{
    public enum BorderStyle
    {
        //
        // summary:
        //     No border.
        None = 0,
        //
        // summary:
        //     A single-line border.
        FixedSingle = 1,
        //
        // summary:
        //     A three-dimensional border.
        Fixed3D = 2
    }
    public enum FormBorderStyle
    {
        //
        // summary:
        //     No border.
        None,
        //
        // summary:
        //     A fixed, single-line border.
        FixedSingle,
        //
        // summary:
        //     A fixed, three-dimensional border.
        Fixed3D,
        //
        // summary:
        //     A thick, fixed dialog-style border.
        FixedDialog,
        //
        // summary:
        //     A resizable border.
        Sizable,
        //
        // summary:
        //     A tool window border that is not resizable. A tool window does not appear in
        //     the taskbar or in the window that appears when the user presses ALT+TAB. Although
        //     forms that specify System.Windows.Forms.FormBorderStyle.FixedToolWindow typically
        //     are not shown in the taskbar, you must also ensure that the System.Windows.Forms.Form.ShowInTaskbar
        //     property is set to false, since its default value is true.
        FixedToolWindow,
        //
        // summary:
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
    // summary:
    //     Defines constants that indicate the alignment of content within a System.Windows.Forms.DataGridView
    //     cell.
    public enum DataGridViewContentAlignment
    {
        //
        // summary:
        //     The alignment is not set.
        NotSet = 0,
        //
        // summary:
        //     The content is aligned vertically at the top and horizontally at the left of
        //     a cell.
        TopLeft = 1,
        //
        // summary:
        //     The content is aligned vertically at the top and horizontally at the center of
        //     a cell.
        TopCenter = 2,
        //
        // summary:
        //     The content is aligned vertically at the top and horizontally at the right of
        //     a cell.
        TopRight = 4,
        //
        // summary:
        //     The content is aligned vertically at the middle and horizontally at the left
        //     of a cell.
        MiddleLeft = 16,
        //
        // summary:
        //     The content is aligned at the vertical and horizontal center of a cell.
        MiddleCenter = 32,
        //
        // summary:
        //     The content is aligned vertically at the middle and horizontally at the right
        //     of a cell.
        MiddleRight = 64,
        //
        // summary:
        //     The content is aligned vertically at the bottom and horizontally at the left
        //     of a cell.
        BottomLeft = 256,
        //
        // summary:
        //     The content is aligned vertically at the bottom and horizontally at the center
        //     of a cell.
        BottomCenter = 512,
        //
        // summary:
        //     The content is aligned vertically at the bottom and horizontally at the right
        //     of a cell.
        BottomRight = 1024
    }
}
