namespace System.Windows.Forms
{

    public class ToolStripItemClickedEventArgs : EventArgs
    {

        public ToolStripItemClickedEventArgs(ToolStripItem clickedItem)
        {
            this.ClickedItem = clickedItem;
        }

        public ToolStripItem ClickedItem { get; }
    }
}
