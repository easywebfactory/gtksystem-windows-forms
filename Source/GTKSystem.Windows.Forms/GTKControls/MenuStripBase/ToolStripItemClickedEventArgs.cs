namespace System.Windows.Forms;

public class ToolStripItemClickedEventArgs : EventArgs
{

    public ToolStripItemClickedEventArgs(ToolStripItem clickedItem)
    {
        ClickedItem = clickedItem;
    }

    public ToolStripItem ClickedItem { get; }
}