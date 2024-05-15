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
    public partial class Button : Control
    {
        public readonly ButtonBase self = new ButtonBase();
        public override object GtkControl => self;
        public Button() : base()
        {

        }
        public override string Text { get => self.Label; set => self.Label = value; }

        public override event EventHandler Click
        {
            add { self.Clicked += value; }
            remove { self.Clicked -= value; }
        }
        private System.Drawing.Image _image;
        public System.Drawing.Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                self.Override.Image = new Bitmap(value.PixbufData);
            }
        }
        public System.Drawing.ContentAlignment ImageAlign { get; set; }
    }
}
