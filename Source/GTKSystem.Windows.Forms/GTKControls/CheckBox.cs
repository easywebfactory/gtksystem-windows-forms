/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;

namespace System.Windows.Forms;

[DesignerCategory("Component")]
public partial class CheckBox : Control
{
    public readonly CheckBoxBase self = new();
    public override object GtkControl => self;

    public CheckBox()
    {
        self.Toggled += Self_Toggled;
        self.ButtonReleaseEvent += Self_ButtonReleaseEvent;
    }

    private void Self_ButtonReleaseEvent(object o, Gtk.ButtonReleaseEventArgs args)
    {
        if (self.Inconsistent)
            self.Inconsistent = false;
    }

    private void Self_Toggled(object sender, EventArgs e)
    {
        if (CheckedChanged != null && self.IsVisible)
            CheckedChanged(this, EventArgs.Empty);
        if (CheckStateChanged != null && self.IsVisible)
            CheckStateChanged(this, EventArgs.Empty);
    }

    public override string Text
    {
        get => self.Label;
        set => self.Label = value;
    }

    public bool Checked
    {
        get => self.Active;
        set
        {
            self.Active = value;
            if (self.IsRealized)
            {
                self.Inconsistent = false;
            }
        }
    }

    public CheckState CheckState
    {
        get
        {
            if (self.Inconsistent)
            {
                return CheckState.Indeterminate;
            }

            return self.Active ? CheckState.Checked : CheckState.Unchecked;
        }
        set
        {
            self.Inconsistent = value == CheckState.Indeterminate;
            self.Active = value == CheckState.Checked;
        }
    }

    public event EventHandler? CheckedChanged;
    public virtual event EventHandler? CheckStateChanged;
}