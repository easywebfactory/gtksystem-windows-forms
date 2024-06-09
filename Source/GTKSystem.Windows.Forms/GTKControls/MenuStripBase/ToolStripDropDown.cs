using Gtk;
using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms
{
    public class ToolStripDropDown : ToolStripItem
    {
        //public readonly ToolStripDropDownBase self = new ToolStripDropDownBase();
        public readonly Gtk.Menu self = new Gtk.Menu();
        public override Widget Widget => self;
        public ToolStripItemCollection toolStripItemCollection;
        public ToolStripDropDown():base()
        {
            toolStripItemCollection = new ToolStripItemCollection(this);
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
