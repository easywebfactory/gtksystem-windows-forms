using Gtk;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	public class StatusStrip : ToolStrip
    {
        public StatusStrip():base()
        {
            this.Control.StyleContext.AddClass("StatusStrip");
            Dock = DockStyle.Bottom;
        }
        [DefaultValue(false)]
        public bool ShowItemToolTips { get; set; }

        [DefaultValue(true)]
		public bool SizingGrip { get; set; }

        [DefaultValue(true)]
		public bool Stretch { get; set; }
    }
}
