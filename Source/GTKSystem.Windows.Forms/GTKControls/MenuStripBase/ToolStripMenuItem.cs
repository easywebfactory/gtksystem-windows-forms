using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace System.Windows.Forms
{
    public class ToolStripMenuItem : ToolStripItem
    {
        public ToolStripMenuItem():base()
        {
            base.Shown += ToolStripMenuItem_Shown;
        }

        private void ToolStripMenuItem_Shown(object sender, EventArgs e)
        {
            if (Checked == true)
            {
                AlwaysShowImage =CheckState == CheckState.Checked || CheckState == CheckState.Indeterminate;
                if (CheckState == CheckState.Indeterminate)
                {
                    Image = Gtk.Image.NewFromIconName("pan-end-symbolic", Gtk.IconSize.Menu);
                }
                else
                {
                    Image = Gtk.Image.NewFromIconName("object-select-symbolic", Gtk.IconSize.Menu);
                }
            }
        }

        
    }
}
