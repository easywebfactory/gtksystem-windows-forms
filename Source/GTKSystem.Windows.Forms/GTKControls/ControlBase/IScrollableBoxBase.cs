using Gtk;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public interface IScrollableBoxBase
    {
        bool AutoScroll { get; set; }
        Widget Child { get; }
        bool HScroll { get; set; }
        GtkControlOverride Override { get; set; }
        bool VScroll { get; set; }
        event System.Windows.Forms.ScrollEventHandler Scroll;
        void Add(Widget child);
        void AddClass(string cssClass);
    }
}