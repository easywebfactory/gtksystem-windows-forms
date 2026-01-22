using System.Drawing;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ToolStripBase : Gtk.Toolbar, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public Size ImageScalingSize { get; set; } = new Size(20, 20);
        public ToolStripBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("ToolStrip");
            this.Hexpand = false;
            this.Vexpand = false;
            this.Valign = Gtk.Align.Start;
            this.Halign = Gtk.Align.Fill;
            this.ShowArrow = true;
        }
    }
}
