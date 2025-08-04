/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class MenuStrip : Control
    {
        public MenuStripBase self = new MenuStripBase();
        public override object GtkControl => self;
        public ToolStripItemCollection toolStripItemCollection;
        public MenuStrip() : base()
        {
            toolStripItemCollection = new ToolStripItemCollection(this);
        }
        public ToolStripItemCollection Items
        {
            get
            {
                return toolStripItemCollection;
            }
        }

        private Size _ImageScalingSize;
        public Size ImageScalingSize
        {
            get => _ImageScalingSize;
            set
            {
                _ImageScalingSize = value;
                self.ImageScalingSize = _ImageScalingSize;
            }
        }
        public ToolStripLayoutStyle LayoutStyle { get; set; }
    }
}
