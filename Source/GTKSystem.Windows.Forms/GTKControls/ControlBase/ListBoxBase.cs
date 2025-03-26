namespace System.Windows.Forms;

public sealed class ListBoxBase : ScrollableBoxBase
{
    public Gtk.ListBox ListBox { get; } = new();

    public ListBoxBase()
    {
        Override.AddClass("ListBox");
        ListBox.BorderWidth = 1;
        ListBox.Margin = 0;
        ListBox.Hexpand = true;
        ListBox.Vexpand = true;
        AutoScroll= true;
        Add(ListBox);
    }
}