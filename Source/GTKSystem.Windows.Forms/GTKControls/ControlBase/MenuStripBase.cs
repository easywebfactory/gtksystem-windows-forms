using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public class MenuStripBase : Gtk.MenuBar, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public Size ImageScalingSize { get; set; } = new Size(20, 20);
        public MenuStripBase() : base()
        {
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("MenuStrip");
            this.Hexpand = false;
            this.Vexpand = false;
            this.Valign = Gtk.Align.Start;
            this.Halign = Gtk.Align.Fill;
        }
    }
}
