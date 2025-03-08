namespace System.Windows.Forms
{
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
}
