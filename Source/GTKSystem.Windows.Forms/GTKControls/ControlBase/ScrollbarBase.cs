using GLib;
using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class ScrollbarBase<T>: Gtk.Scrollbar, IControlGtk
    {
        public new static GType GType
        {
            get
            {
                return (typeof(T) == typeof(Gtk.HScrollbar)) ? Gtk.HScrollbar.GType : Gtk.VScrollbar.GType;
            }
        }
        
        public GtkControlOverride Override { get; set; }
        internal ScrollbarBase(Orientation orientation): base(orientation, new Gtk.Adjustment(0, 0, 100, 1, 10, 0))
        {
            this.Override = new GtkControlOverride(this);
        }
    }
}
