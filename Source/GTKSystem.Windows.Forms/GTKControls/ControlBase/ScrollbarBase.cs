using GLib;

namespace System.Windows.Forms;

public sealed class ScrollbarBase<T>: Gtk.Scrollbar, IControlGtk
{
    [Obsolete]
    public new static GType GType => typeof(T) == typeof(Gtk.HScrollbar) ? Gtk.HScrollbar.GType : Gtk.VScrollbar.GType;

    public IGtkControlOverride Override { get; set; }
    internal ScrollbarBase(Gtk.Orientation orientation): base(orientation, new Gtk.Adjustment(0, 0, 100, 1, 10, 0))
    {
        Override = new GtkFormsControlOverride(this);
    }
}