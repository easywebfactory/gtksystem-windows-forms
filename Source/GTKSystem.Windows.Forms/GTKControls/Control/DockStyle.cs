namespace System.Windows.Forms;

public enum DockStyle
{
    //
    // 摘要:
    //     The control is not docked.
    None = 0,
    //
    // 摘要:
    //     The control's top edge is docked to the top of its containing control.
    Top = 1,
    //
    // 摘要:
    //     The control's bottom edge is docked to the bottom of its containing control.
    Bottom = 2,
    //
    // 摘要:
    //     The control's left edge is docked to the left edge of its containing control.
    Left = 3,
    //
    // 摘要:
    //     The control's right edge is docked to the right edge of its containing control.
    Right = 4,
    //
    // 摘要:
    //     All the control's edges are docked to the all edges of its containing control
    //     and sized appropriately.
    Fill = 5
}