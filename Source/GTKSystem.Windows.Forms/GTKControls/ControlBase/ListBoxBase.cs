namespace System.Windows.Forms;

public sealed class ListBoxBase : ScrollableBoxBase
{
    public Gtk.ListBox listBox = new();
    public ListBoxBase()
    {
        Override.AddClass("ListBox");
        listBox.BorderWidth = 1;
        listBox.Margin = 0;
        listBox.Hexpand = true;
        listBox.Vexpand = true;
        AutoScroll= true;
        Add(listBox);
    }
}