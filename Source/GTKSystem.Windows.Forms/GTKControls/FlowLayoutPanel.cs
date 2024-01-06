using Gtk;
using Pango;
using System.ComponentModel;
using System.Windows.Forms.Layout;
using System.Xml.Linq;

namespace System.Windows.Forms
{
	[ProvideProperty("FlowBreak", typeof(Control))]
	[DefaultProperty("FlowDirection")]
	public class FlowLayoutPanel : WidgetContainerControl<Gtk.FlowBox>, IExtenderProvider
    {
        private ObjectCollection _controls;
        public FlowLayoutPanel() : base()
        {
            Widget.StyleContext.AddClass("FlowLayoutPanel");
            base.Control.Orientation = Gtk.Orientation.Horizontal;
            base.Control.Halign = Align.Start;
            base.Control.MinChildrenPerLine = 1;
            base.Control.MaxChildrenPerLine = 999;
            base.Control.ColumnSpacing = 0;
            base.Control.BorderWidth = 1;

            base.Control.ChildActivated += Control_ChildActivated;
            _controls = new ObjectCollection(this);

        }

        private void Control_ChildActivated(object o, ChildActivatedArgs args)
        {
            var c = args.Child;
        }

        private FlowDirection _FlowDirection;
        [DefaultValue(FlowDirection.LeftToRight)]
		[Localizable(true)]
		public FlowDirection FlowDirection
		{
            get { return _FlowDirection; }
            set
            {
                if (value == FlowDirection.LeftToRight || value == FlowDirection.RightToLeft) { base.Control.Orientation = Gtk.Orientation.Horizontal; }
                else if (value == FlowDirection.TopDown || value == FlowDirection.BottomUp) { base.Control.Orientation = Gtk.Orientation.Vertical; }
            }
        }
        public override ControlCollection Controls => _controls;
        public bool CanExtend(object extendee)
        {
            return true;
        }
        public class ObjectCollection : ControlCollection
        {
            public ObjectCollection(FlowLayoutPanel owner) : base(owner)
            {

            }
            public override int Add(object item)
            {
                Gtk.FlowBoxChild box = new FlowBoxChild();
                box.HeightRequest = 20;
                box.Valign = Align.Start;
                box.Halign = Align.Start;
                box.MarginStart = 0;
                
                Control control = (Control)item;
                Gtk.Widget widg = control.Widget;
                widg.Valign = Align.Start;
                widg.Halign = Align.Start;
                box.Add(widg);
                return base.AddWidget(box);
            }
        }
    }
}
