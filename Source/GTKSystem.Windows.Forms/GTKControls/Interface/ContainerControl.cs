 
namespace System.Windows.Forms
{
    public class ContainerControl : ScrollableControl, IContainerControl
    {
        public Control ActiveControl { get; set; }

        public bool ActivateControl(Control active)
        {
            return (active != null && ActiveControl != null && ActiveControl.Equals(active));
        }
    }
}
