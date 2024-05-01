/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class CheckBox : Control
    {
        public readonly CheckBoxBase self = new CheckBoxBase();
        public override object GtkControl => self;
        public CheckBox() {
             
        }

        public override string Text { get { return self.Label; } set { self.Label = value; } }
        public  bool Checked { get { return self.Active; } set { self.Active = value; } }
        public CheckState CheckState { get { return self.Active ? CheckState.Checked : CheckState.Unchecked; } set { self.Active = value != CheckState.Unchecked; } }
        
        public event EventHandler CheckedChanged
        {
            add { self.Toggled += value; }
            remove { self.Toggled -= value; }
        }
        public virtual event EventHandler CheckStateChanged
        {
            add { self.Toggled += value; }
            remove { self.Toggled -= value; }
        }
    }
}
