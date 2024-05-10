using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{

    public class ToolStripSeparator : WidgetToolStrip<Gtk.SeparatorMenuItem>
    {
        public ToolStripSeparator() : base()
        {
            this.Control.Hexpand = false;
            this.Control.Vexpand = false;
            this.Control.Halign=Gtk.Align.Start;
            this.Control.Valign=Gtk.Align.Center;
            this.Control.HeightRequest = 25;
            this.Control.WidthRequest = 2;
        }
        public override Size Size { get => base.Size; set => base.Size = new Size(2, value.Height); }
        public override int Width { get; set; }
    }
}
