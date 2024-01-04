/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class RadioButton : WidgetControl<Gtk.RadioButton>
    {
        public RadioButton():base(new Gtk.RadioButton("baseradio")) {
            Widget.StyleContext.AddClass("RadioButton");
            base.Control.Realized += Control_Realized; ;
        }

        private void Control_Realized(object sender, EventArgs e)
        {
            Gtk.Container con = this.WidgetParent as Gtk.Container;
            foreach (var widget in con.AllChildren)
            {
                if (widget is Gtk.RadioButton)
                {
                    //加入容器内的第一个radio配组
                    ((Gtk.RadioButton)sender).JoinGroup((Gtk.RadioButton)widget);
                    break;
                }
            }
            base.Control.Active = _Checked;
            isLoaded = true;
        }
        bool isLoaded = false;
        public event EventHandler CheckedChanged
        {
            add { base.Control.Toggled += (object sender, EventArgs e) => { if (isLoaded) { value.Invoke(this, e); } }; }
            remove { base.Control.Toggled -= (object sender, EventArgs e) => { if (isLoaded) { value.Invoke(this, e); } }; }
        }

        public override string Text { get { return base.Control.Label; } set { base.Control.Label = value;} }
        public bool Checked { get { return base.Control.Active; } set { _Checked = true; base.Control.Active = true; } }
        private bool _Checked;
    }
}
