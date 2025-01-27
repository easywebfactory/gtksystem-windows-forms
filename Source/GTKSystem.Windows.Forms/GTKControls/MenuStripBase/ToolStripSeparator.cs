using System.Drawing;

namespace System.Windows.Forms
{

    public class ToolStripSeparator : WidgetToolStrip<Gtk.SeparatorMenuItem>
    {
        public ToolStripSeparator() : base("ToolStripSeparator")
        {
            this.MenuItem.Hexpand = false;
            this.MenuItem.Vexpand = false;
            this.MenuItem.Halign=Gtk.Align.Center;
            this.MenuItem.Valign=Gtk.Align.Center;
            this.MenuItem.StyleContext.AddClass("ToolStripSeparator");
        }
        public override Size Size { get => base.Size;
            set {
                if (value.Width > value.Height)
                    base.Size = new Size(value.Width, 2); 
                else
                    base.Size = new Size(2, value.Height);
            }
        }
        public override int Width { get; set; }
    }
}
