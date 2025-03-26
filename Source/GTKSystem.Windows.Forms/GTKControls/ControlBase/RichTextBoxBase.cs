namespace System.Windows.Forms;

public sealed class RichTextBoxBase : ScrollableBoxBase
{
    internal Gtk.TextView TextView = new();
    public RichTextBoxBase()
    {
        Override.AddClass("RichTextBox");
        TextView.BorderWidth = 1;
        TextView.Margin = 0;
        TextView.WrapMode = Gtk.WrapMode.Char;
        TextView.Halign = Gtk.Align.Fill;
        TextView.Valign = Gtk.Align.Fill;
        TextView.Hexpand = true;
        TextView.Vexpand = true;
        AutoScroll = true;
        Add(TextView);
    }
}