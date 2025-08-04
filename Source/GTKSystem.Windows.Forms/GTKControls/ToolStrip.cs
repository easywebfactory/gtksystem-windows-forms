/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.Drawing;

namespace System.Windows.Forms
{
    public class ToolStrip : Control
    {
        public readonly ToolStripBase self = new ToolStripBase();
        public override object GtkControl => self;
        public ToolStripItemCollection toolStripItemCollection;
        public ToolStrip() : base()
        {
            self.ToolbarStyle = ToolbarStyle.BothHoriz;
            self.IconSize = IconSize.SmallToolbar;
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
        public Size ImageScalingSize {
            get => _ImageScalingSize;
            set
            {
                _ImageScalingSize = value;
                self.ImageScalingSize = _ImageScalingSize;
            }
        }
        public ToolStripLayoutStyle LayoutStyle { get; set; }
        public override event EventHandler Click;
        public event EventHandler CheckedChanged;
        public event EventHandler CheckStateChanged;
        public event ToolStripItemClickedEventHandler DropDownItemClicked;

        private TextImageRelation textImageRelation;
        public TextImageRelation TextImageRelation {  
            get=> textImageRelation; 
            set {
                textImageRelation = value;
                if (value == TextImageRelation.ImageAboveText || value == TextImageRelation.TextAboveImage)
                {
                    self.ToolbarStyle = ToolbarStyle.Both;
                    self.IconSize = IconSize.Dialog;
                }
                if (value == TextImageRelation.ImageBeforeText || value == TextImageRelation.TextBeforeImage)
                {
                    self.ToolbarStyle = ToolbarStyle.BothHoriz;
                    self.IconSize = IconSize.SmallToolbar;
                }
                //if (value == TextImageRelation.Overlay )
                //{
                //    self.ToolbarStyle = ToolbarStyle.BothHoriz;
                //    self.IconSize = IconSize.SmallToolbar;
                //}
            }
        }
    }
}
