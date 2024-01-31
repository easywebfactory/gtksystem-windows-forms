using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{

    public class ToolStripSeparator : WidgetToolStrip<Gtk.SeparatorMenuItem>
    {
        public ToolStripSeparator() : base()
        {

        }
        public override Size Size { get; set; }
        public override int Width { get; set; }
    }
}
