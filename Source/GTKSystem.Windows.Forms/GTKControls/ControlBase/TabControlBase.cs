using Gtk;
using System;
using System.Linq;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class TabControlBase : Gtk.Notebook, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public TabControlBase() : base()
        {
            this.Scrollable = true;
            this.EnablePopup = false;
            this.Override = new GtkControlOverride(this);
            this.StyleContext.AddClass("TabControl");
            base.Halign = Gtk.Align.Start;
            base.Valign = Gtk.Align.Start;
        }
    }
}
