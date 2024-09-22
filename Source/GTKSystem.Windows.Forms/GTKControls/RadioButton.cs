/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class RadioButton : Control
    {
        public readonly RadioButtonBase self = new RadioButtonBase();
        public override object GtkControl => self;
        public RadioButton():base() {
            self.Realized += Control_Realized;
        }

        private void Self_Toggled(object sender, EventArgs e)
        {
            if (CheckedChanged != null && self.IsVisible)
                CheckedChanged(this, e);
        }

        private void Control_Realized(object sender, EventArgs e)
        {
            Gtk.Container con = self.Parent as Gtk.Container;
            foreach (var widget in con.AllChildren)
            {
                if (widget is Gtk.RadioButton)
                {
                    //加入容器内的第一个radio配组
                    ((Gtk.RadioButton)sender).JoinGroup((Gtk.RadioButton)widget);
                    break;
                }
            }
            self.Active = _Checked;
            self.Toggled += Self_Toggled;
        }
        public event EventHandler CheckedChanged;

        public override string Text { get { return self.Label; } set { self.Label = value;} }
        public bool Checked { get { return self.Active; } set { _Checked = true; self.Active = true; } }
        private bool _Checked;
    }
}
