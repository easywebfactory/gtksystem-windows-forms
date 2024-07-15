
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{

    [ToolboxItemFilter("System.Windows.Forms")]
    public abstract class CommonDialog : Component
    {
        private static readonly object s_helpRequestEvent = new object();
        public CommonDialog()
        {

        }
        public object Tag { get; set; }

        public event EventHandler HelpRequest
        {
            add => Events.AddHandler(s_helpRequestEvent, value);
            remove => Events.RemoveHandler(s_helpRequestEvent, value);
        }
        protected virtual void OnHelpRequest(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[s_helpRequestEvent];
            handler?.Invoke(this, e);
        }
        public abstract void Reset();

        protected abstract bool RunDialog(IWin32Window owner);

        public virtual DialogResult ShowDialog() => ShowDialog(owner: null);

        public virtual DialogResult ShowDialog(IWin32Window owner)
        {
            DialogResult result = DialogResult.Cancel;
            try
            {
               bool runresult = RunDialog(owner);
                if (runresult)
                {
                    result = DialogResult.OK;
                }
            }
            finally
            {
               base.Dispose();
            }
            return result;
        }
    }
}