using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms
{
    public class ToolStripDropDown : WidgetControl<Gtk.Menu>
    {
        public ToolStripItemCollection toolStripItemCollection;
        public ToolStripDropDown():base()
        {
            toolStripItemCollection = new ToolStripItemCollection(base.Control);
        }

        public ToolStripItemCollection Items
        {
            get
            {
                return toolStripItemCollection;
            }
        }

        public Size ImageScalingSize { get; set; }
    }
}
