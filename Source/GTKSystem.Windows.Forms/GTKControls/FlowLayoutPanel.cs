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
    public partial class FlowLayoutPanel : Panel, IExtenderProvider
    {
        public FlowLayoutPanelBase layoutEngine = new FlowLayoutPanelBase();
        private ObjectCollection _controls;
        public FlowLayoutPanel() : base("FlowLayoutPanel")
        {
            self.Override.sender = this;
            layoutEngine.Orientation = Gtk.Orientation.Horizontal;
            layoutEngine.MinChildrenPerLine = 1;
            layoutEngine.MaxChildrenPerLine = 30;
            layoutEngine.ColumnSpacing = 1;
            layoutEngine.Halign = Align.Fill;
            layoutEngine.Valign = Align.Start;
            _controls = new ObjectCollection(this, layoutEngine);
            Gtk.Viewport viewport = new Gtk.Viewport();
            viewport.Add(layoutEngine);
            self.Add(viewport);
        }

        private FlowDirection _FlowDirection = FlowDirection.LeftToRight;
		public FlowDirection FlowDirection
		{
            get { return _FlowDirection; }
            set
            {
                _FlowDirection = value;
                if (value == FlowDirection.LeftToRight || value == FlowDirection.RightToLeft) { 
                    layoutEngine.Orientation = Gtk.Orientation.Horizontal;
                    layoutEngine.Halign = Align.Fill;
                    layoutEngine.Valign = Align.Start;
                }
                else if (value == FlowDirection.TopDown || value == FlowDirection.BottomUp) { 
                    layoutEngine.Orientation = Gtk.Orientation.Vertical;
                    layoutEngine.Halign = Align.Start;
                    layoutEngine.Valign = Align.Fill;
                }
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
            return false;
        }
        public class ObjectCollection : ControlCollection
        {
            private FlowLayoutPanel _owner;
            private FlowLayoutPanelBase LayoutEngine;
            public ObjectCollection(FlowLayoutPanel owner, FlowLayoutPanelBase container) : base(owner, container)
            {
                _owner = owner;
                LayoutEngine = container;
            }
            public override void Add(Control control)
            {
                Gtk.FlowBoxChild item = new FlowBoxChild();
                item.Valign = Align.Start;
                item.Halign = Align.Start;
                item.Add(control.Widget);
                LayoutEngine.Add(item);

                control.Location=new Drawing.Point(0, 0);
                control.LockLocation = true;
                control.Parent = _owner;
                base.Add(control);

            }
            public override void Remove(Control control)
            {
                if (control is null)
                {
                    return;
                }
                if (control.Widget.Parent != null)
                    LayoutEngine.Remove(control.Widget.Parent);
                InnerList.Remove(control);
            }
        }
    }
}
