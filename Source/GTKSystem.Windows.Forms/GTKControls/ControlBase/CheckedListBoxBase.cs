namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class CheckedListBoxBase : ScrollableBoxBase
    {
        public CheckedListBoxBase() : base()
        {
            this.Override.AddClass("CheckedListBox");
            this.BorderWidth = 1;
        }
    }
}
