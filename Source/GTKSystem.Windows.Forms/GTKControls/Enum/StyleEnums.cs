namespace System.Windows.Forms
{
    public enum BorderStyle
    {
        /// <summary>
        ///     No border.
        /// </summary>
        None = 0,
        /// <summary>
        ///     A single-line border.
        /// </summary>
        FixedSingle = 1,
        /// <summary>
        ///     A three-dimensional border.
        /// </summary>
        Fixed3D = 2
    }
    public enum FormBorderStyle
    {
        /// <summary>
        ///     No border.
        /// </summary>
        None,
        /// <summary>
        ///     A fixed, single-line border.
        /// </summary>
        FixedSingle,
        /// <summary>
        ///     A fixed, three-dimensional border.
        /// </summary>
        Fixed3D,
        /// <summary>
        ///     A thick, fixed dialog-style border.
        /// </summary>
        FixedDialog,
        /// <summary>
        ///     A resizable border.
        /// </summary>
        Sizable,
        /// <summary>
        ///     A tool window border that is not resizable. A tool window does not appear in
        ///     the taskbar or in the window that appears when the user presses ALT+TAB. Although
        ///     forms that specify System.Windows.Forms.FormBorderStyle.FixedToolWindow typically
        ///     are not shown in the taskbar, you must also ensure that the System.Windows.Forms.Form.ShowInTaskbar
        ///     property is set to false, since its default value is true.
        /// </summary>
        FixedToolWindow,
        /// <summary>
        ///     A resizable tool window border. A tool window does not appear in the taskbar
        ///     or in the window that appears when the user presses ALT+TAB.
        /// </summary>
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

    /// <summary>
    ///     Defines constants that indicate the alignment of content within a System.Windows.Forms.DataGridView
    ///     cell.
    /// </summary>
    public enum DataGridViewContentAlignment
    {
        /// <summary>
        ///     The alignment is not set.
        /// </summary>
        NotSet = 0,
        /// <summary>
        ///     The content is aligned vertically at the top and horizontally at the left of
        ///     a cell.
        /// </summary>
        TopLeft = 1,
        /// <summary>
        ///     The content is aligned vertically at the top and horizontally at the center of
        ///     a cell.
        /// </summary>
        TopCenter = 2,
        /// <summary>
        ///     The content is aligned vertically at the top and horizontally at the right of
        ///     a cell.
        /// </summary>
        TopRight = 4,
        /// <summary>
        ///     The content is aligned vertically at the middle and horizontally at the left
        ///     of a cell.
        /// </summary>
        MiddleLeft = 16,
        /// <summary>
        ///     The content is aligned at the vertical and horizontal center of a cell.
        /// </summary>
        MiddleCenter = 32,
        /// <summary>
        ///     The content is aligned vertically at the middle and horizontally at the right
        ///     of a cell.
        /// </summary>
        MiddleRight = 64,
        /// <summary>
        ///     The content is aligned vertically at the bottom and horizontally at the left
        ///     of a cell.
        /// </summary>
        BottomLeft = 256,
        /// <summary>
        ///     The content is aligned vertically at the bottom and horizontally at the center
        ///     of a cell.
        /// </summary>
        BottomCenter = 512,
        /// <summary>
        ///     The content is aligned vertically at the bottom and horizontally at the right
        ///     of a cell.
        /// </summary>
        BottomRight = 1024
    }
}
