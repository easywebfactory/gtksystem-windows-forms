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
    public partial class GroupBox : WidgetContainerControl<Gtk.Frame>
    {
        private Gtk.Fixed contaner;
        private ControlCollection _controls;
        public GroupBox() : base()
        {
            Widget.StyleContext.AddClass("GroupBox");
            Widget.StyleContext.AddClass("BorderRadiusStyle");
            base.Control.LabelXalign = 0.03f;
            contaner = new Gtk.Fixed();
            contaner.Margin = 0;
            contaner.MarginStart = 0;
            contaner.MarginTop = 0;
            _controls = new ControlCollection(this, contaner);
            base.Control.Add(contaner);
            base.Control.Realized += Control_Realized;
        }

        private void Control_Realized(object sender, EventArgs e)
        {
            foreach (Gtk.Widget wid in contaner.Children)
            {
                wid.MarginTop = (int)wid.Data["InitMarginTop"] - 15;
                wid.Data["InitMarginTop"] = wid.MarginTop;
                wid.MarginStart = (int)wid.Data["InitMarginStart"];
            }
            this.Control.ShowAll();
        }
        public override string Text { get { return base.Control.Label; } set { base.Control.Label = value; } }
        public override ControlCollection Controls => _controls;

        public override void SuspendLayout()
        {
            _Created = false;
        }
        public override void ResumeLayout(bool resume)
        {
            _Created = resume == false;
        }

        public override void PerformLayout()
        {
            _Created = true;
        }

    }
}
