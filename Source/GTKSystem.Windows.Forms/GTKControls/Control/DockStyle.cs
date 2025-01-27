namespace System.Windows.Forms
{
    public enum DockStyle
    {
        //
        // summary:
        //     The control is not docked.
        None = 0,
        //
        // summary:
        //     The control's top edge is docked to the top of its containing control.
        Top = 1,
        //
        // summary:
        //     The control's bottom edge is docked to the bottom of its containing control.
        Bottom = 2,
        //
        // summary:
        //     The control's left edge is docked to the left edge of its containing control.
        Left = 3,
        //
        // summary:
        //     The control's right edge is docked to the right edge of its containing control.
        Right = 4,
        //
        // summary:
        //     All the control's edges are docked to the all edges of its containing control
        //     and sized appropriately.
        Fill = 5
    }
}