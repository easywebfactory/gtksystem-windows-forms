using Gtk;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace System.Windows.Forms
{
    public class ToolStripMenuItem : WidgetToolStrip<Gtk.MenuItem>
    {
        public ToolStripMenuItem():base("ToolStripMenuItem")
        {
            DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
        }

        public override CheckState CheckState { 
            get => base.CheckState;
            set
            {
                base.CheckState = value;
                if (this.Widget is Gtk.CheckMenuItem checkMenuItem)
                {
                    if (value == CheckState.Indeterminate)
                    {
                        checkMenuItem.DrawAsRadio = true;
                    }
                    else if (value == CheckState.Checked)
                    {
                        checkMenuItem.DrawAsRadio = false;
                    }
                }
            }
        }
        public override bool Checked {
            get
            {
                if (this.Widget is Gtk.CheckMenuItem checkMenuItem)
                {
                   return checkMenuItem.Active;
                }
                return base.Checked; 
            }
            set { 
                base.Checked = value;
                if (this.Widget is Gtk.CheckMenuItem checkMenuItem)
                {
                    checkMenuItem.Active = value;
                }
            } }
    }
}
