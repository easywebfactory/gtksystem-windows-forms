using System.Drawing;

namespace System.Windows.Forms
{
    public class ToolStripDropDown : ToolStripItem
    {
        //public readonly ToolStripDropDownBase self = new ToolStripDropDownBase();
        public readonly Gtk.Menu self = new Gtk.Menu();
        public override Gtk.Widget Widget => self;
        public ToolStripDropDown() : base()
        {

        }

        public Size ImageScalingSize { get; set; }
    }
}
