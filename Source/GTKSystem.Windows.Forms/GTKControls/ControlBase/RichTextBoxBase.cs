namespace System.Windows.Forms;

public sealed class RichTextBoxBase : ScrollableBoxBase
{
    internal Gtk.TextView textView = new();
    public RichTextBoxBase()
    {
        Override.AddClass("RichTextBox");
        textView.BorderWidth = 1;
        textView.Margin = 0;
        textView.WrapMode = Gtk.WrapMode.Char;
        textView.Halign = Gtk.Align.Fill;
        textView.Valign = Gtk.Align.Fill;
        textView.Hexpand = true;
        textView.Vexpand = true;
        AutoScroll = true;
        Add(textView);
    }
}