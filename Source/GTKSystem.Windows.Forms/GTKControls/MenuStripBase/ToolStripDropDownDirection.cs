namespace System.Windows.Forms;

//
// 摘要:
//     Specifies the direction in which a System.Windows.Forms.ToolStripDropDown control
//     is displayed relative to its parent control.
public enum ToolStripDropDownDirection
{
    //
    // 摘要:
    //     Uses the mouse position to specify that the System.Windows.Forms.ToolStripDropDown
    //     is displayed above and to the left of its parent control.
    AboveLeft = 0,
    //
    // 摘要:
    //     Uses the mouse position to specify that the System.Windows.Forms.ToolStripDropDown
    //     is displayed above and to the right of its parent control.
    AboveRight = 1,
    //
    // 摘要:
    //     Uses the mouse position to specify that the System.Windows.Forms.ToolStripDropDown
    //     is displayed below and to the left of its parent control.
    BelowLeft = 2,
    //
    // 摘要:
    //     Uses the mouse position to specify that the System.Windows.Forms.ToolStripDropDown
    //     is displayed below and to the right of its parent control.
    BelowRight = 3,
    //
    // 摘要:
    //     Compensates for nested drop-down controls and specifies that the System.Windows.Forms.ToolStripDropDown
    //     is displayed to the left of its parent control.
    Left = 4,
    //
    // 摘要:
    //     Compensates for nested drop-down controls and specifies that the System.Windows.Forms.ToolStripDropDown
    //     is displayed to the right of its parent control.
    Right = 5,
    //
    // 摘要:
    //     Compensates for nested drop-down controls and responds to the System.Windows.Forms.RightToLeft
    //     setting, specifying either System.Windows.Forms.ToolStripDropDownDirection.Left
    //     or System.Windows.Forms.ToolStripDropDownDirection.Right accordingly.
    Default = 7
}