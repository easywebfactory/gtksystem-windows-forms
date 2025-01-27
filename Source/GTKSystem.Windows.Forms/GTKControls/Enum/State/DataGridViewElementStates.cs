namespace System.Windows.Forms
{
    //
    // summary:
    //     Specifies the user interface (UI) state of a element within a System.Windows.Forms.DataGridView
    //     control.
    [Flags]
    public enum DataGridViewElementStates
    {
        //
        // summary:
        //     Indicates that an element is in its default state.
        None = 0,
        //
        // summary:
        //     Indicates the an element is currently displayed onscreen.
        Displayed = 1,
        //
        // summary:
        //     Indicates that an element cannot be scrolled through the UI.
        Frozen = 2,
        //
        // summary:
        //     Indicates that an element will not accept user input to change its value.
        ReadOnly = 4,
        //
        // summary:
        //     Indicates that an element can be resized through the UI. This value is ignored
        //     except when combined with the System.Windows.Forms.DataGridViewElementStates.ResizableSet
        //     value.
        Resizable = 8,
        //
        // summary:
        //     Indicates that an element does not inherit the resizable state of its parent.
        ResizableSet = 16,
        //
        // summary:
        //     Indicates that an element is in a selected (highlighted) UI state.
        Selected = 32,
        //
        // summary:
        //     Indicates that an element is visible (displayable).
        Visible = 64
    }
}
