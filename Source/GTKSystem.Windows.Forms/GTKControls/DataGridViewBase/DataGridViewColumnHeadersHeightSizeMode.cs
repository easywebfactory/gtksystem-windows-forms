namespace System.Windows.Forms
{
    /// <summary>
    ///     Defines values for specifying how the height of the column headers is adjusted.
    /// </summary>
    public enum DataGridViewColumnHeadersHeightSizeMode
    {
        /// <summary>
        ///     Users can adjust the column header height with the mouse.
        /// </summary>
        EnableResizing,
        /// <summary>
        ///     Users cannot adjust the column header height with the mouse.
        /// </summary>
        DisableResizing,
        /// <summary>
        ///     The column header height adjusts to fit the contents of all the column header
        ///     cells.
        /// </summary>
        AutoSize
    }
}