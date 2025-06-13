/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Linq;


namespace System.Windows.Forms
{
    [DesignerCategory("UserControl")]
    [ToolboxItem(true)]
    public partial class UserControl : ContainerControl
    {
        public readonly UserControlBase self = new UserControlBase();
        public override object GtkControl => self;
        private Gtk.Overlay contaner;
        private ControlCollection _controls;

        public UserControl() : base()
        {
            self.Override.sender = this;
            contaner = new Gtk.Overlay();
            contaner.MarginStart = 0;
            contaner.MarginTop = 0;
            contaner.BorderWidth = 0;
            contaner.Halign = Align.Fill;
            contaner.Valign = Align.Fill;
            contaner.Hexpand = false;
            contaner.Vexpand = false;
            contaner.Add(new Gtk.Fixed() { Halign = Align.Fill, Valign = Align.Fill });
            _controls = new ControlCollection(this, contaner);
            self.Add(contaner);
            self.Shown += Self_Shown;
        }
        private bool Is_Control_Shown = false;
        private void Self_Shown(object sender, EventArgs e)
        {
            if (Is_Control_Shown == false)
            {
                Is_Control_Shown = true;
                OnLoad(EventArgs.Empty);
                Load?.Invoke(this, e);
            }
        }

        public override Padding Padding
        {
            get => base.Padding;
            set
            {
                base.Padding = value;
                contaner.MarginStart = value.Left;
                contaner.MarginTop = value.Top;
                contaner.MarginEnd = value.Right;
                contaner.MarginBottom = value.Bottom;
            }
        }
        public override event EventHandler Load;
        public System.Drawing.SizeF AutoScaleDimensions { get; set; }
        public System.Windows.Forms.AutoScaleMode AutoScaleMode { get; set; }
        public override ControlCollection Controls => _controls;

        public override void SuspendLayout()
        {

        }
        public override void ResumeLayout(bool performLayout)
        {

        }
    }
}
