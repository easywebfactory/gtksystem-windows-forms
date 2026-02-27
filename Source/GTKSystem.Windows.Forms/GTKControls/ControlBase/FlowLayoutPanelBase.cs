namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class FlowLayoutPanelBase : Gtk.FlowBox
    {
        public FlowLayoutPanelBase() : base()
        {
            this.SelectionMode = Gtk.SelectionMode.None;
            this.ActivateOnSingleClick = false;
        }
    }
}
