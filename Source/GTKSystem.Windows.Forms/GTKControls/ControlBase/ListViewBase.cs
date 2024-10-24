namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ListViewBase : ScrollableBoxBase
    {
        public ListViewBase() : base()
        {
            this.StyleContext.AddClass("view");
            this.Override.AddClass("ListView");
            base.BorderWidth = 1;
            base.AutoScroll = true;
        }
    }
}
