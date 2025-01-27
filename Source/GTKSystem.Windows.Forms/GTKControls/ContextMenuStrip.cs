/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;

namespace System.Windows.Forms
{
    public class ContextMenuStrip:ToolStripDropDownMenu
    {
        public ContextMenuStrip() { 
        
        }
        public ContextMenuStrip(IContainer container) {
        
        }
        protected void SetVisibleCore(bool visible) { }
    }

}
