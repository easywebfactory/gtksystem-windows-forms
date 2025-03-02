using Gtk;

namespace System.Windows.Forms;

public interface IScrollableBoxBase
{
    int WidthRequest { get; set; }
    int HeightRequest { get; set; }
    bool AutoScroll { get; set; }
    Widget Child { get; }
    bool HScroll { get; set; }
    IGtkControlOverride Override { get; set; }
    bool VScroll { get; set; }
    event ScrollEventHandler? Scroll;
    void Add(Widget child);
    void AddClass(string cssClass);
}