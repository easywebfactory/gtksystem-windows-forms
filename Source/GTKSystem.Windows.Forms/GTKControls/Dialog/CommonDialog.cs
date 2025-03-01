/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
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
            bool runresult = RunDialog(owner);
            if (runresult)
            {
                result = DialogResult.OK;
            }
            return result;
        }
    }
}