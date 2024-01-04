using Gtk;
using Pango;
using System.ComponentModel;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	[ProvideProperty("FlowBreak", typeof(Control))]
	[DefaultProperty("FlowDirection")]
	public class FlowLayoutPanel : WidgetContainerControl<Gtk.FlowBox>, IExtenderProvider
    {
        public FlowLayoutPanel() : base()
        {
            Widget.StyleContext.AddClass("FlowLayoutPanel");
		 
        }
        public override LayoutEngine LayoutEngine
		{
			get
			{
				throw null;
			}
		}
		[DefaultValue(FlowDirection.LeftToRight)]
		[Localizable(true)]
		public FlowDirection FlowDirection
		{
			get;
			set;
		}

		[DefaultValue(true)]
		[Localizable(true)]
		public bool WrapContents
		{
            get;
            set;
        }
		bool IExtenderProvider.CanExtend(object obj)
		{
			throw null;
		}

		[DefaultValue(false)]
		[DisplayName("FlowBreak")]
		public bool GetFlowBreak(Control control)
		{
			throw null;
		}

		[DisplayName("FlowBreak")]
		public void SetFlowBreak(Control control, bool value)
		{
			 
		}
	}
}
