/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;
using static Gtk.Paned;


namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class SplitContainer : ContainerControl
    {
        public readonly SplitContainerBase self = new SplitContainerBase();
        public override object GtkControl => self;
        public SplitContainer() : base()
        {
            _panel1 = new SplitterPanel(this);
            _panel1.contaner.Name = "Child1";
            _panel2 = new SplitterPanel(this);
            _panel2.contaner.Name = "Child2";
            self.Pack1(_panel1.Widget, false, true);
            self.Pack2(_panel2.Widget, true, true);
        }

        private SplitterPanel _panel1;
        private SplitterPanel _panel2;
        public SplitterPanel Panel1
        {
            get
            {
                return _panel1;
            }
            set
            {
                _panel1 = value;
            }
        }
        public SplitterPanel Panel2 {
            get
            {
                return _panel2;
            }
            set
            {
                _panel2 = value;
            }
        }
        public int _SplitterDistance;
        public int SplitterDistance { get => self.Position + 5; set { _SplitterDistance = Math.Max(1, value - 5); self.Position = _SplitterDistance; } }
        private int _SplitterWidth;
        public int SplitterWidth { get { return _SplitterWidth; } set { _SplitterWidth = value; self.WideHandle = value > 2; } }
        public int SplitterIncrement { get; set; }
        public Orientation Orientation {
            get { return self.Orientation == Gtk.Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal; }
            set {
                self.Orientation = value == Orientation.Horizontal ? Gtk.Orientation.Vertical : Gtk.Orientation.Horizontal;
            }
        }
        private FixedPanel _fixedPanel = FixedPanel.Panel1;
        public FixedPanel FixedPanel {
            get => _fixedPanel;
            set {
                _fixedPanel = value;
                bool resize = value == FixedPanel.Panel2;
                ((PanedChild)self[_panel1.Widget]).Resize = resize;
                ((PanedChild)self[_panel2.Widget]).Resize = !resize;
            } 
        }
    }

}
