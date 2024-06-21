namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ListViewBase : ScrollableBoxBase
    {
        internal ListViewBase() : base()
        {
            this.Override.AddClass("ListView");
            base.AutoScroll = true;
        }
    }
}
