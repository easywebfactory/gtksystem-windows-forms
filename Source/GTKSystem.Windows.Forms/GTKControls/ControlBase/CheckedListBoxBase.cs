namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class CheckedListBoxBase : ScrollableBoxBase
    {
        internal CheckedListBoxBase() : base()
        {
            this.Override.AddClass("CheckedListBox");
            this.BorderWidth = 1;
        }
    }
}
