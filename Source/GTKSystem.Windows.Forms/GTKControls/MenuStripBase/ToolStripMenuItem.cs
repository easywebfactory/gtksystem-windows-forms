using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace System.Windows.Forms
{
    public class ToolStripMenuItem : WidgetToolStrip<Gtk.MenuItem>
    {
        public ToolStripMenuItem():base()
        {
            DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
        }

        public override CheckState CheckState { 
            get => base.CheckState;
            set
            {
                base.CheckState = value;
                if (value == CheckState.Indeterminate)
                {
                   // IcoImage = Gtk.Image.NewFromIconName("pan-end-symbolic", Gtk.IconSize.Menu);
                }
                else
                {
                   // IcoImage = Gtk.Image.NewFromIconName("object-select-symbolic", Gtk.IconSize.Menu);
                }
            }
        }

    }
}
