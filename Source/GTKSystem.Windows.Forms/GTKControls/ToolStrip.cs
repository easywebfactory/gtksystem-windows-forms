using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using Gtk;

namespace System.Windows.Forms
{
    public class ToolStrip : WidgetControl<Gtk.MenuBar>
    {
        public ToolStripItemCollection toolStripItemCollection;
        public ToolStrip()
        {
            this.Control.StyleContext.AddClass("ToolStrip");
            this.Control.Hexpand = false;
            this.Control.Vexpand = false;
            this.Control.Valign = Gtk.Align.Start;
            this.Control.Halign = Gtk.Align.Start;
            toolStripItemCollection = new ToolStripItemCollection(this);
            base.Control.ActivateCurrent += ToolStripItem_Activated;
        }

        private void ToolStripItem_Activated(object sender, ActivateCurrentArgs e)
        {
            if (Click != null)
            {
                Click(sender, e);
            }
            if (CheckedChanged != null)
            {
                CheckedChanged(this, e);
            }
            if (CheckStateChanged != null)
            {
                CheckStateChanged(this, e);
            }
            if (DropDownItemClicked != null)
            {
                DropDownItemClicked(this, new ToolStripItemClickedEventArgs(new ToolStripItem()));
            }
        }

        public ToolStripItemCollection Items
        {
            get
            {
                return toolStripItemCollection;
            }
        }

        public Size ImageScalingSize { get; set; }

        public override event EventHandler Click;
        public event EventHandler CheckedChanged;
        public event EventHandler CheckStateChanged;
        public event ToolStripItemClickedEventHandler DropDownItemClicked;
    }
}
