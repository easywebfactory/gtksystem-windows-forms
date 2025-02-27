/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public class CheckBox : Control
{
    public readonly CheckBoxBase self = new();
    public override object GtkControl => self;
    public CheckBox()
    {
        self.Toggled += Self_Toggled;
    }

    private void Self_Toggled(object? sender, EventArgs e)
    {
        if (CheckedChanged != null && self.IsVisible)
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        if (CheckStateChanged != null && self.IsVisible)
            CheckStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public override string? Text { get => self.Label;
        set => self.Label = value;
    }
    public bool Checked { get => self.Active;
        set => self.Active = value;
    }

    private CheckState _checkState = CheckState.Unchecked;
    public CheckState CheckState
    {
        get => _checkState == CheckState.Indeterminate ? _checkState : self.Active ? CheckState.Checked : CheckState.Unchecked;
        set
        {
            if (_checkState != value)
            {
                CheckedChanged?.Invoke(this, EventArgs.Empty);
                CheckStateChanged?.Invoke(this, EventArgs.Empty);
                _checkState = value;
            }
            self.Active = value != CheckState.Unchecked;
        }
    }
    public event EventHandler? CheckedChanged;
    public virtual event EventHandler? CheckStateChanged;
}