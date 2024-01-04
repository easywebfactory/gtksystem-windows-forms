/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class CheckBox : WidgetControl<Gtk.CheckButton>
    {
        public CheckBox() {
            Widget.StyleContext.AddClass("CheckBox");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
        }

        public override string Text { get { return base.Control.Label; } set { base.Control.Label = value; } }
        public  bool Checked { get { return base.Control.Active; } set { base.Control.Active = value; } }
        public CheckState CheckState { get { return base.Control.Active ? CheckState.Checked : CheckState.Unchecked; } set { base.Control.Active = value != CheckState.Unchecked; } }
        
        public event EventHandler CheckedChanged
        {
            add { base.Control.Toggled += (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
            remove { base.Control.Toggled -= (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
        }
        public virtual event EventHandler CheckStateChanged
        {
            add { base.Control.Toggled += (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
            remove { base.Control.Toggled -= (object sender, EventArgs e) => { if (base.Control.IsRealized) { value.Invoke(this, e); } }; }
        }
    }
}
