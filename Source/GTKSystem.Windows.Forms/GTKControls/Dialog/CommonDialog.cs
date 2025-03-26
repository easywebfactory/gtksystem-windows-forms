/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;

namespace System.Windows.Forms;

[ToolboxItemFilter("System.Windows.Forms")]
public abstract class CommonDialog : Component
{
    private static readonly object helpRequestEvent = new();
    public object? Tag { get; set; }

    public event EventHandler? HelpRequest
    {
        add => Events.AddHandler(helpRequestEvent, value);
        remove => Events.RemoveHandler(helpRequestEvent, value);
    }
    protected virtual void OnHelpRequest(EventArgs e)
    {
        var handler = (EventHandler)Events[helpRequestEvent];
        handler?.Invoke(this, e);
    }
    public abstract void Reset();

    protected abstract bool RunDialog(IWin32Window? owner);

    public virtual DialogResult ShowDialog() => ShowDialog(owner: null);

    public virtual DialogResult ShowDialog(IWin32Window? owner)
    {
        var result = DialogResult.Cancel;
        var runresult = RunDialog(owner);
        if (runresult)
        {
            result = DialogResult.OK;
        }
        return result;
    }
}