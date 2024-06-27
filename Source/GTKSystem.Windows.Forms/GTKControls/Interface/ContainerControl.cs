 
namespace System.Windows.Forms
{
    public class ContainerControl : ScrollableControl, IContainerControl
    {
        private Control _ActiveControl;
        public Control ActiveControl { get => _ActiveControl; set { ActivateControl(value); } }
        public ContainerControl() : base()
        {
        }
        public bool ActivateControl(Control active)
        {
            _ActiveControl = active;
            if (active != null)
            {
                if (active.GtkControl is Gtk.Widget widget)
                {
                    return widget.Activate();
                }
            }
            return false;
        }
        public virtual StatusStrip StatusStrip { get; set; }
    }
}
