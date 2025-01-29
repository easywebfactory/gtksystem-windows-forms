namespace System.Windows.Forms
{
    public enum DragDropEffects
    {
        /// <summary>The drop target does not accept the data.</summary>
        None = 0x0,

        /// <summary>The data from the drag source is copied to the drop target.</summary>
        Copy = 0x1,

        /// <summary>The data from the drag source is moved to the drop target.</summary>
        Move = 0x2,

        /// <summary>The data from the drag source is linked to the drop target.</summary>
        Link = 0x4,

        /// <summary>The target can be scrolled while dragging to locate a drop position that is not currently visible in the target.</summary>
        Scroll = int.MinValue,

        /// <summary>The combination of the <see cref="F:System.Windows.DragDropEffects.Copy" />, <see cref="F:System.Windows.Forms.DragDropEffects.Move" />, and <see cref="F:System.Windows.Forms.DragDropEffects.Scroll" /> effects.</summary>
        All = -2147483645
    }
}