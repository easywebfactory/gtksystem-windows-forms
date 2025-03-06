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
            self.ParentSet += Self_ParentSet;
        }

        private void Self_ParentSet(object o, Gtk.ParentSetArgs args)
        {
            Gtk.Container con = self.Parent as Gtk.Container;
            foreach (var widget in con.Children)
            {
                if (widget is Gtk.RadioButton group)
                {
                    ((Gtk.RadioButton)o).Group = new Gtk.RadioButton[0];
                    //加入容器内的第一个radio配组
                    ((Gtk.RadioButton)o).JoinGroup(group);
                     break;
                }
            }
            self.Active = _Checked;
            self.Toggled += Self_Toggled;
        }

        private void Self_Toggled(object sender, EventArgs e)
        {
            if (CheckedChanged != null && self.IsVisible)
                CheckedChanged(this, e);
        }
        public event EventHandler CheckedChanged;

        public override string Text { get { return self.Label; } set { self.Label = value;} }
        public bool Checked { get { return self.Active; } set { _Checked = true; self.Active = true; } }
        private bool _Checked;
    }
}
