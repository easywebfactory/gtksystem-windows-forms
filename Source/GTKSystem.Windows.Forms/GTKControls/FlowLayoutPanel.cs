/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;


namespace System.Windows.Forms
{
	[ProvideProperty("FlowBreak", typeof(Control))]
	[DefaultProperty("FlowDirection")]
    [DesignerCategory("Component")]
    public partial class FlowLayoutPanel : Control, IExtenderProvider
    {
        public readonly FlowLayoutPanelBase self = new FlowLayoutPanelBase();
        public override object GtkControl => self;
        private ObjectCollection _controls;
        public FlowLayoutPanel() : base()
        {
            self.Orientation = Gtk.Orientation.Horizontal;
            self.Halign = Align.Start;
            self.Valign = Align.Start;
            self.MinChildrenPerLine = 1;
            self.MaxChildrenPerLine = 999;
            self.ColumnSpacing = 0;
            self.BorderWidth = 0;
            _controls = new ObjectCollection(this);
        }

        private FlowDirection _FlowDirection;
		public FlowDirection FlowDirection
		{
            get { return _FlowDirection; }
            set
            {
                if (value == FlowDirection.LeftToRight || value == FlowDirection.RightToLeft) { self.Orientation = Gtk.Orientation.Horizontal; }
                else if (value == FlowDirection.TopDown || value == FlowDirection.BottomUp) { self.Orientation = Gtk.Orientation.Vertical; }
            }
        }

        public bool WrapContents { get; set; }

        public bool GetFlowBreak(Control control)
        {
            return false;
        }

        public void SetFlowBreak(Control control, bool value)
        {

        }
        public override ControlCollection Controls => _controls;
        public bool CanExtend(object extendee)
        {
            return true;
        }
        public class ObjectCollection : ControlCollection
        {
            private FlowLayoutPanel _owner;
            public ObjectCollection(FlowLayoutPanel owner) : base(owner)
            {
                _owner = owner;
            }
            public override int Add(object item)
            {
                Gtk.FlowBoxChild box = new FlowBoxChild();
                box.Valign = Align.Start;
                box.Halign = Align.Start;
                box.Expand = false;
                Control control = (Control)item;
                control.Location=new Drawing.Point(0, 0);
                control.LockLocation = true;
                control.Parent = _owner;
                Gtk.Widget widg = control.Widget;
                widg.Valign = Align.Start;
                widg.Halign = Align.Start;
                widg.Expand = false;
                box.Add(widg);
                _owner.self.Add(box);
                return base.AddWidget(box, control);
            }
        }
    }
}
