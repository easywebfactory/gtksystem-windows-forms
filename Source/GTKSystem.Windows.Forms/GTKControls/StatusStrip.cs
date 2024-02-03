using Gtk;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	public class StatusStrip : ToolStrip
    {
        public StatusStrip():base()
        {
            this.Control.StyleContext.RemoveClass("ToolStrip");
            this.Control.StyleContext.AddClass("StatusStrip");
            Dock = DockStyle.Bottom;

        }
        public override Size Size { get => base.Size; set => base.Size = new Size(value.Width, 20); }
        [DefaultValue(false)]
        public bool ShowItemToolTips { get; set; }

        [DefaultValue(true)]
		public bool SizingGrip { get; set; }

        [DefaultValue(true)]
		public bool Stretch { get; set; }
    }
}
